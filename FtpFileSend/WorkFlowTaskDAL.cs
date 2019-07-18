using CommonLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace FtpFileSend
{
    public class WorkFlowTaskDAL
    {
        /// <summary>
        /// 获取需要发送的短信数据
        /// </summary>
        /// <returns></returns>
        public static DataTable GetNeedSendMessage()
        {
            string sql = ConfigHelper.SearchSql;
            return MySQLHelper.GetDataTable(CommandType.Text, sql);
        }

        /// <summary>
        /// 导出excel
        /// </summary>
        public static void ExportExcel()
        {
            try
            {
                DataTable dt = GetNeedSendMessage();
                //创建一个工作簿
                Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
                //创建一个 sheet 表
                Aspose.Cells.Worksheet worksheet = workbook.Worksheets[0];
                //设置 sheet 表名称
                worksheet.Name = "待办";
                Aspose.Cells.Cell cell;
                if (!string.IsNullOrWhiteSpace(ConfigHelper.SendMessage))
                {
                    try
                    {
                        List<SendMessage> list = JsonConvert.DeserializeObject<List<SendMessage>>(ConfigHelper.SendMessage);
                        if (list != null)
                        {
                            foreach (var item in list)
                            {
                                DataRow dr2 = dt.NewRow();
                                dr2[0] = item.Name;
                                dr2[1] = item.Content;
                                dr2[2] = item.Phone;
                                dt.Rows.Add(dr2);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.DoErrorLog(ex.Message);
                    }

                }

                int cellIndex = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // 如果当前待办事件数量为0，则不需要添加到excel中
                    if (dt.Rows[i]["count"].ToString() == "0")
                    {
                        continue;
                    }
                    for (int j = 0; j < dt.Columns.Count - 1; j++)
                    {
                        cell = worksheet.Cells[cellIndex, j];
                        cell.PutValue(dt.Rows[i][j]);
                    }
                    cellIndex++;
                }
                string fileName = ConfigHelper.ExcelName;
                string directoryPth = Environment.CurrentDirectory + "\\ExportFile";
                if (!Directory.Exists(directoryPth))
                {
                    Directory.CreateDirectory(directoryPth);
                }
                string path = directoryPth + "\\" + fileName;
                //保存至指定路径
                workbook.Save(directoryPth + "\\" + fileName);
                bool result = FtpUpLoadHelper.UploadFile(path, fileName);
                if (result)
                {
                    LogHelper.DoNormalLog("操作成功");
                }

            }
            catch (Exception ex)
            {
                LogHelper.DoErrorLog(ex.Message);
            }

        }
    }
}
