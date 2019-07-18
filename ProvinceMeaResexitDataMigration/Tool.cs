using CommonLibrary;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace ProvinceMeaResexitDataMigration
{
    public class Tool
    {
        #region 思路
        /*
         * 本次迁移的数据针对的是省里的限制出境，所以需要限制FlowID 
         * 只有流程走到了"延长限制出境措施审批" 或者 "解除限制出境措施审批" 才需要进行数据迁移
         * 限制出境业务表 Mea_Resexit 而要想知道流程有没有走到具体步骤看 WorkFlowTask
         * WorkFlowTask 根据时间升序 
         * 第一步的ID 就是下一步 PrevID（上一步主键ID）的值 每个流程第一步的PrevID是空
         * 第一步的StepID 就是下一步 PrevStepID(上一步骤ID)的值 每个流程的第一步的PrevStepID是空
         * InstanceID 是每个业务表(Mea_Resexit)的主键ID 也就是说当换了一个流程或者子流程 这个值就会改变 同一流程这个值是不会变的
         * ClueID 值一直保持不变
         * GroupID 在同一个流程里面唯一  SubFlowGroupID为NULL
         * 子流程的第一步的GroupID的值就是上一步即引发子流程的那一步的SubFlowGroupID的值 
         * 当走到子流程第一步时 需要回去修改上一步引发子流程的步骤的SubFlowGroupID值
         * 
             SELECT groupid,subflowgroupid,instanceid,StepName,SenderTime from WorkFlowTask 
             where ClueID ='926973d7-b007-4bba-b2be-82070a4a8f62' ORDER BY sendertime,
             SubFlowGroupID DESC
         */
        #endregion

        #region 获取需要迁移的数据信息
        /// <summary>
        /// 获取需要迁移的数据信息
        /// </summary>
        /// <returns></returns>
        public static List<NeedMigrateModel> GetNeedMigrate()
        {
            // 本次迁移的数据针对的是省里的限制出境，所以需要限制FlowID 
            // 只有流程走到了"延长限制出境措施审批" 或者 "解除限制出境措施审批" 才需要进行数据迁移
            string sql = $@"SELECT DISTINCT ClueID FROM WorkFlowTask where WorkFlowTask.ClueID IN(
                           SELECT Mea_Resexit.ClueID from Mea_Resexit)  AND
                           WorkFlowTask.FlowID = '{Constant.SyxzcjcsFlowID}'
                           AND(WorkFlowTask.StepID = '{Constant.YcxzcjcsspStepID}'
                           or WorkFlowTask.StepID = '{Constant.JcxzcjcsspStepID}')";
            DataTable dt = MySQLHelper.GetDataTable(CommandType.Text, sql);
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            return TableToList.ToDataList<NeedMigrateModel>(dt);
        }
        #endregion

        #region 拆分数据
        /// <summary>
        /// 拆分数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool DataMigration(List<NeedMigrateModel> list)
        {
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.AppSettings["MySQLConn"]))
            {
                conn.Open();
                MySqlTransaction transaction = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = transaction;
                try
                {
                    foreach (var item in list)
                    {
                        string sql = $"SELECT * from Mea_Resexit where clueid ='{item.ClueID}'";
                        DataTable dt = MySQLHelper.GetDataTable(CommandType.Text, sql);
                        if (dt == null || dt.Rows.Count == 0)
                        {
                            continue;
                        }

                        Mea_Resexit model = TableToList.ToDataList<Mea_Resexit>(dt).FirstOrDefault();
                        List<ClueTypeModel> clueTypeList = GetCurrentClueState(item.ClueID);

                        // 是否走了延长措施步骤
                        var yanChangItem = clueTypeList.FirstOrDefault(o => o.Type == 1);
                        string yanChangInstanceID = string.Empty;
                        string jiechuInstanceID = string.Empty;
                        if (yanChangItem != null && yanChangItem.CountNum > 0)
                        {
                            yanChangInstanceID = MeaResexitSplit(model, 1, cmd);
                        }
                        // 是否走了解除措施步骤
                        var jieChuItem = clueTypeList.FirstOrDefault(o => o.Type == 2);
                        if (jieChuItem != null && jieChuItem.CountNum > 0)
                        {
                            jiechuInstanceID = MeaResexitSplit(model, 2, cmd);
                        }
                        WorkFlowTaskSplit(item.ClueID, yanChangInstanceID, jiechuInstanceID, cmd);

                    }

                    transaction.Commit();
                    return true;
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    LogHelper.DoErrorLog(ex.Message);
                    transaction.Rollback();
                    return false;
                }
            }
        }
        #endregion

        #region 验证当前线索状态
        /// <summary>
        /// 验证当前线索是否走了"延长限制出境措施审批" 或者 "解除限制出境措施审批"步骤
        /// </summary>
        /// <param name="clueID"></param>
        /// <returns></returns>
        public static List<ClueTypeModel> GetCurrentClueState(string clueID)
        {
            // 这个sql是为了验证 "延长限制出境措施审批" 或者 "解除限制出境措施审批" 走了没有
            // Type 1 延长 2 解除
            string sql = $@"SELECT COUNT(1) AS CountNum,1 AS 'Type' from workflowtask 
                           where StepID = '{Constant.YcxzcjcsspStepID}' AND ClueID = '{clueID}'
                           UNION ALL
                           SELECT COUNT(1) AS CountNum,2 AS 'Type' from workflowtask
                           where StepID = '{Constant.JcxzcjcsspStepID}' AND ClueID = '{clueID}'";
            DataTable dt = MySQLHelper.GetDataTable(CommandType.Text, sql);
            return TableToList.ToDataList<ClueTypeModel>(dt);
        }
        #endregion

        #region 业务表数据拆分
        /// <summary>
        /// 业务表数据拆分 拆分 MeaResexit表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="type">1 延长 2 解除</param>
        /// <returns></returns>
        public static string MeaResexitSplit(Mea_Resexit model, int type, MySqlCommand cmd)
        {
            Mea_Resexit item = new Mea_Resexit();
            string id = Guid.NewGuid().ToString();
            string sql = string.Empty;
            if (type == 1)
            {
                Console.WriteLine("分割表MeaResexit表，ClueID为：" + model.ClueID + "的延长措施数据");
                LogHelper.DoNormalLog("分割表MeaResexit表，ClueID为：" + model.ClueID + "的延长措施数据");
                sql = $@"INSERT INTO Mea_Resexit(ID,ClueID,SubmitDate,ExtendSubmitDate,SubmitDept,IsAgs,MeasureClueSource,Deputy,
                                ExtendReasonAndTime,ObjOtherCard,UndertakeStaffExtend,
                                FirstTime,ExtendTime,UndertakeDeptExtendOpinion,UndertakeDeptExtendName_CBR,
                                UndertakeDeptExtendDate_CBR,UndertakeDeptExtendOpinion_ZhuRen,UndertakeDeptExtendName,
                                UndertakeDeptExtendOpinionDate,ChargePersonExtendOpinion,ChargePersonExtendName,
                                ChargePersonExtendOpinionDate,ChargeLeaderExtendOpinion,ChargeLeaderExtendName,
                                ChargeLeaderExtendOpinionDate,MainLeaderExtendOpinion,MainLeaderExtendName,
                                MainLeaderExtendOpinionDate,State)
                                VALUES('{id}','{model.ClueID}','{model.SubmitDate}','{model.ExtendSubmitDate}',
                                '{model.SubmitDept}','{model.IsAgs}','{model.MeasureClueSource}','{model.Deputy}',
                                '{model.ExtendReasonAndTime}','{model.ObjOtherCard}','{model.UndertakeStaffExtend}',
                                 '{model.FirstTime}','{model.ExtendTime}','{model.UndertakeDeptExtendOpinion}',
                                 '{model.UndertakeDeptExtendName_CBR}','{model.UndertakeDeptExtendDate_CBR}',
                                 '{model.UndertakeDeptExtendOpinion_ZhuRen}','{model.UndertakeDeptExtendName}',
                                  '{model.UndertakeDeptExtendOpinionDate}','{model.ChargePersonExtendOpinion}',
                                  '{model.ChargePersonExtendName}','{model.ChargePersonExtendOpinionDate}',
                                   '{model.ChargeLeaderExtendOpinion}','{model.ChargeLeaderExtendName}',
                                    '{model.ChargeLeaderExtendOpinionDate}','{model.MainLeaderExtendOpinion}',
                                   '{model.MainLeaderExtendName}','{model.MainLeaderExtendOpinionDate}','延长限制出境')";

            }
            else
            {
                Console.WriteLine("分割表MeaResexit表，ClueID为：" + model.ClueID + "的解除措施数据");
                sql = $@"INSERT INTO Mea_Resexit(ID,ClueID,SubmitDate,ReleaseSubmitDate,SubmitDept,IsAgs,MeasureClueSource,Deputy,
                                ExtendReasonAndTime,ObjOtherCard,UndertakeStaff,ReleaseReason,ObjEducation,FirstTime,ReleaseTime2,
                                UndertakeDeptReleaseOpinion,UndertakeDeptReleaseName_CBR,UndertakeDeptReleaseDate_CBR,
                                UndertakeDeptReleaseOpinion_ZhuRen,UndertakeDeptReleaseName,
                                UndertakeDeptReleaseOpinionDate,ChargePersonReleaseOpinion,ChargePersonReleaseName,
                                ChargePersonReleaseOpinionDate,ChargeLeaderReleaseOpinion,
                                ChargeLeaderReleaseName,ChargeLeaderReleaseOpinionDate,MainLeaderReleaseOpinion,
                                MainLeaderReleaseName,MainLeaderReleaseOpinionDate,State)
                                VALUES('{id}','{model.ClueID}','{model.SubmitDate}','{model.ReleaseSubmitDate}',
                                '{model.SubmitDept}','{model.IsAgs}','{model.MeasureClueSource}','{model.Deputy}',
                                '{model.ExtendReasonAndTime}','{model.ObjOtherCard}','{model.UndertakeStaff}',
                                 '{model.ReleaseReason}','{model.ObjEducation}','{model.FirstTime}',
                                 '{model.ReleaseTime2}','{model.UndertakeDeptReleaseOpinion}',
                                 '{model.UndertakeDeptReleaseName_CBR}','{model.UndertakeDeptReleaseDate_CBR}',
                                 '{model.UndertakeDeptReleaseOpinion_ZhuRen}','{model.UndertakeDeptReleaseName}',
                                  '{model.UndertakeDeptReleaseOpinionDate}','{model.ChargePersonReleaseOpinion}',
                                   '{model.ChargePersonReleaseName}','{model.ChargePersonReleaseOpinionDate}',
                                    '{model.ChargeLeaderReleaseOpinion}','{model.ChargeLeaderReleaseName}',
                                   '{model.ChargeLeaderReleaseOpinionDate}','{model.MainLeaderReleaseOpinion}',
                                    '{model.MainLeaderReleaseName}','{model.MainLeaderReleaseOpinionDate}','解除限制出境')";

            }

            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            Console.WriteLine("分割表MeaResexit表，ClueID为：" + model.ClueID + "的数据分割结束");
            LogHelper.DoNormalLog("分割表MeaResexit表，ClueID为：" + model.ClueID + "的数据分割结束");
            return id;
        }

        #endregion

        #region 流程表数据拆分
        /// <summary>
        /// 流程表数据拆分 拆分 WorkFlowTask表
        /// </summary>
        /// <param name="clueID"></param>
        /// <param name="yanChangInstanceID"></param>
        /// <param name="jiechuInstanceID"></param>
        /// <returns></returns>
        public static void WorkFlowTaskSplit(string clueID, string yanChangInstanceID, string jiechuInstanceID, MySqlCommand cmd)
        {
            Console.WriteLine("开始分割表为WorkFlowTask的数据,clueID为：" + clueID);
            LogHelper.DoNormalLog("开始分割表为WorkFlowTask的数据,clueID为：" + clueID);

            string sql = $@"SELECT * from WorkFlowTask WHERE 
                            clueid ='{clueID}' ORDER BY sendertime";
            DataTable dt = MySQLHelper.GetDataTable(CommandType.Text, sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                List<WorkFlowTaskModel> list = TableToList.ToDataList<WorkFlowTaskModel>(dt);
                int flag = 0;
                string groupID = string.Empty;
                int index = 0;
                string prevGroupID = string.Empty;
                foreach (var item in list)
                {
                    if (item.StepID == Constant.YcxzcjcsspStepID)
                    {
                        flag = 1;
                        groupID = Guid.NewGuid().ToString();
                        WorkFlowTaskModel model = list[index - 1];
                        //List<WorkFlowTaskModel> listCurrent = GetWorkFlowTaskModelByClueID(clueID);
                        //WorkFlowTaskModel currentModel = listCurrent[currentIndex - 1];
                        string currentGroupID = string.IsNullOrEmpty(prevGroupID) ? model.GroupID : prevGroupID;
                        // 插入一条数据
                        string sqlInsert = $@"INSERT INTO WorkFlowTask(ID,PrevID,PrevStepID,FlowID,StepID,StepName,InstanceID,ClueID,
                                       GroupID,Type,Title,SenderID,SenderName,SenderTime,ReceiveID,ReceiveName,ToUserID,
                                       ToUserName,Status,SubFlowGroupID)VALUES('{Guid.NewGuid().ToString()}','{model.ID}',
                                       '{model.StepID}','{model.FlowID}','{Constant.YcxzcjcsStepID}','延长限制出境措施',
                                       '{model.InstanceID}','{clueID}','{currentGroupID}',0,'使用限制出境措施','{model.SenderID}',
                                       '{model.SenderName}','{item.SenderTime}','{item.ReceiveID}','{item.ReceiveName}',
                                       '{item.ToUserID}','{item.ToUserName}',4,'{groupID}')";
                        prevGroupID = groupID;


                        cmd.CommandText = sqlInsert;
                        cmd.ExecuteNonQuery();
                        //MySQLHelper.ExecuteNonQuery(CommandType.Text, sqlInsert);
                        string clueGroupID = model.ClueGroupID;
                        if (string.IsNullOrEmpty(clueGroupID))
                        {
                            clueGroupID = model.GroupID;
                        }
                        // 更新当前数据
                        string sqlUpdate = $@"UPDATE WorkFlowTask set PrevID =NULL,PrevStepID = NULL,
                                              FlowID='{Constant.YcxzcjcsFlowID}',InstanceID ='{yanChangInstanceID}',GroupID='{groupID}'
                                              ,ClueGroupID ='{clueGroupID}' WHERE clueid='{clueID}' AND StepID ='{Constant.YcxzcjcsspStepID}' AND ID= '{item.ID}'";
                        //MySQLHelper.ExecuteNonQuery(CommandType.Text, sqlUpdate);
                        cmd.CommandText = sqlUpdate;
                        cmd.ExecuteNonQuery();

                    }
                    else if (item.StepID == Constant.JcxzcjcsspStepID)
                    {
                        flag = 2;
                        groupID = Guid.NewGuid().ToString();
                        WorkFlowTaskModel model = list[index - 1];
                       // List<WorkFlowTaskModel> listCurrent = GetWorkFlowTaskModelByClueID(clueID);
                       // WorkFlowTaskModel currentModel = listCurrent[currentIndex - 1];
                        string currentGroupID = string.IsNullOrEmpty(prevGroupID) ? model.GroupID : prevGroupID;
                        // 插入一条数据
                        string sqlInsert = $@"INSERT INTO WorkFlowTask(ID,PrevID,PrevStepID,FlowID,StepID,StepName,InstanceID,ClueID,
                                       GroupID,Type,Title,SenderID,SenderName,SenderTime,ReceiveID,ReceiveName,ToUserID,
                                       ToUserName,Status,SubFlowGroupID)VALUES('{Guid.NewGuid().ToString()}','{model.ID}',
                                       '{model.StepID}','{model.FlowID}','{Constant.JcxzcjcsStepID}','解除限制出境措施',
                                       '{model.InstanceID}','{clueID}','{currentGroupID}',0,'使用限制出境措施','{model.SenderID}',
                                       '{model.SenderName}','{item.SenderTime}','{item.ReceiveID}','{item.ReceiveName}',
                                       '{item.ToUserID}','{item.ToUserName}',4,'{groupID}')";
                        //MySQLHelper.ExecuteNonQuery(CommandType.Text, sqlInsert);
                        prevGroupID = groupID;

                        cmd.CommandText = sqlInsert;
                        cmd.ExecuteNonQuery();
                        string clueGroupID = model.ClueGroupID;
                        if (string.IsNullOrEmpty(clueGroupID))
                        {
                            clueGroupID = model.GroupID;
                        }

                        // 更新当前数据
                        string sqlUpdate = $@"UPDATE WorkFlowTask set PrevID =NULL,PrevStepID = NULL,
                                              FlowID='{Constant.JcxzcjcsFlowID}',InstanceID ='{jiechuInstanceID}',GroupID='{groupID}'
                                              ,ClueGroupID ='{clueGroupID}' WHERE clueid='{clueID}' AND StepID ='{Constant.JcxzcjcsspStepID}' AND ID= '{item.ID}'";
                        //MySQLHelper.ExecuteNonQuery(CommandType.Text, sqlUpdate);
                        cmd.CommandText = sqlUpdate;
                        cmd.ExecuteNonQuery();
                    }
                    else if (flag != 0)
                    {
                        if (flag == 1)
                        {
                            // 更新当前数据
                            string sqlUpdate = $@"UPDATE WorkFlowTask set FlowID='{Constant.YcxzcjcsFlowID}',
                                                  InstanceID ='{yanChangInstanceID}',GroupID='{groupID}'
                                                  WHERE clueid='{clueID}' AND StepID ='{item.StepID}' AND ID= '{item.ID}'";
                            //MySQLHelper.ExecuteNonQuery(CommandType.Text, sqlUpdate);
                            cmd.CommandText = sqlUpdate;
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            // 更新当前数据
                            string sqlUpdate = $@"UPDATE WorkFlowTask set FlowID='{Constant.JcxzcjcsFlowID}',
                                                  InstanceID ='{jiechuInstanceID}',GroupID='{groupID}'
                                                  WHERE clueid='{clueID}' AND StepID ='{item.StepID}' AND ID= '{item.ID}'";
                            //MySQLHelper.ExecuteNonQuery(CommandType.Text, sqlUpdate);
                            cmd.CommandText = sqlUpdate;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    index++;
                   // currentIndex++;
                }
            }

            Console.WriteLine("结束分割表为WorkFlowTask的数据,clueID为：" + clueID);
            LogHelper.DoNormalLog("结束分割表为WorkFlowTask的数据,clueID为：" + clueID);
        }
        #endregion

        #region 获取最新线索关联的workflowtask列表信息
        ///// <summary>
        ///// 获取最新线索关联的workflowtask列表信息
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public static List<WorkFlowTaskModel> GetWorkFlowTaskModelByClueID(string clueID)
        //{
        //    string sql = $@"SELECT * FROM workflowtask WHERE ClueID = '{clueID}' ORDER BY SenderTime,SubFlowGroupID desc";
        //    DataTable dt = MySQLHelper.GetDataTable(CommandType.Text, sql);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        return TableToList.ToDataList<WorkFlowTaskModel>(dt);
        //    }
        //    return null;
        //}
        #endregion

    }
}
