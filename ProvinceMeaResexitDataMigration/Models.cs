namespace ProvinceMeaResexitDataMigration
{
    public class NeedMigrateModel
    {
        public string ClueID { get; set; }

    }

    public class ClueTypeModel
    {
        public int CountNum { get; set; }
        /// <summary>
        /// Type 1 延长 2 解除
        /// </summary>
        public int Type { get; set; }
    }

    public class Mea_Resexit
    {
        public string ID { get; set; }
        public string UserID { get; set; }
        public string DeptID { get; set; }
        public string State { get; set; }
        public string ClueID { get; set; }
        public string SubmitDept { get; set; }
        public string SubmitDate { get; set; }
        public string ObjName { get; set; }
        public string ObjSex { get; set; }
        public string ObjBirth { get; set; }
        public string ObjUnit { get; set; }
        public string ObjPostLevel { get; set; }
        public string ObjIdCard { get; set; }
        public string ObjPolitical { get; set; }
        public string PartyLevel { get; set; }
        public string PersonLevel { get; set; }
        public string CommitteeLevel { get; set; }
        public string ReasonAndTime { get; set; }
        public string UndertakeDeptOpinion { get; set; }
        public string UndertakeDeptOpinionDate { get; set; }
        public string ChargePersonOpinion { get; set; }
        public string ChargePersonOpinionDate { get; set; }
        public string ChargeLeaderOpinion { get; set; }
        public string ChargeLeaderOpinionDate { get; set; }
        public string MainLeaderOpinion { get; set; }
        public string MainLeaderOpinionDate { get; set; }
        public string Affix { get; set; }
        public string UndertakeStaff { get; set; }
        public string FirstTime { get; set; }
        public string ExtendReasonAndTime { get; set; }
        public string UndertakeDeptExtendOpinion { get; set; }
        public string UndertakeDeptExtendOpinionDate { get; set; }
        public string ChargePersonExtendOpinion { get; set; }
        public string ChargePersonExtendOpinionDate { get; set; }
        public string ChargeLeaderExtendOpinion { get; set; }
        public string ChargeLeaderExtendOpinionDate { get; set; }
        public string MainLeaderExtendOpinion { get; set; }
        public string MainLeaderExtendOpinionDate { get; set; }
        public string AffixExtend { get; set; }
        public string UndertakeStaffExtend { get; set; }
        public string SubmitType { get; set; }
        public string ObjNation { get; set; }
        public string ObjOtherCard { get; set; }
        public string ObjEducation { get; set; }
        public string ObjIsCharge { get; set; }
        public string Deputy { get; set; }
        public string JobTime { get; set; }
        public string PartyTime { get; set; }
        public string IsExtend { get; set; }
        public string ReleaseReason { get; set; }
        public string UndertakeDeptReleaseOpinion { get; set; }
        public string ChargePersonReleaseOpinion { get; set; }
        public string ChargeLeaderReleaseOpinion { get; set; }
        public string MainLeaderReleaseOpinion { get; set; }
        public string ObjPost { get; set; }
        public string FinishTime { get; set; }
        public string ExtendTime { get; set; }
        public string ReleaseTime { get; set; }
        public string ExtendSubmitDate { get; set; }
        public string ReleaseSubmitDate { get; set; }
        public string ReleaseTime2 { get; set; }
        public string UndertakeDeptName { get; set; }
        public string ChargePersonName { get; set; }
        public string ChargeLeaderName { get; set; }
        public string MainLeaderName { get; set; }
        public string UndertakeDeptExtendName { get; set; }
        public string ChargePersonExtendName { get; set; }
        public string ChargeLeaderExtendName { get; set; }
        public string MainLeaderExtendName { get; set; }
        public string UndertakeDeptReleaseName { get; set; }
        public string ChargePersonReleaseName { get; set; }
        public string ChargeLeaderReleaseName { get; set; }
        public string MainLeaderReleaseName { get; set; }
        public string UndertakeDeptReleaseOpinionDate { get; set; }
        public string ChargePersonReleaseOpinionDate { get; set; }
        public string ChargeLeaderReleaseOpinionDate { get; set; }
        public string MainLeaderReleaseOpinionDate { get; set; }
        public string UndertakeDeptOpinion_ZhuRen { get; set; }
        public string UndertakeDeptName_CBR { get; set; }
        public string UndertakeDeptDate_CBR { get; set; }
        public string UndertakeDeptExtendOpinion_ZhuRen { get; set; }
        public string UndertakeDeptReleaseOpinion_ZhuRen { get; set; }
        public string UndertakeDeptExtendName_CBR { get; set; }
        public string UndertakeDeptReleaseName_CBR { get; set; }
        public string UndertakeDeptExtendDate_CBR { get; set; }
        public string UndertakeDeptReleaseDate_CBR { get; set; }
        public string MeasureClueSource { get; set; }
        public string IsDeputy { get; set; }
        public string IsCPPCC { get; set; }
        public string IsDdb { get; set; }
        public string IsAgs { get; set; }
        public string UndertakeAgsOpinion { get; set; }
        public string UndertakeAgsOpinionDate { get; set; }
        public string UndertakeAgsOpinionName { get; set; }
        public string UndertakeDeptOpinionDate_CBR { get; set; }
        public string UndertakeAgsLeaderOpinion { get; set; }
        public string UndertakeAgsLeaderOpinionDate { get; set; }
        public string UndertakeAgsLeaderOpinionName { get; set; }
        public string UndertakeCBROpinion { get; set; }
        public string UndertakeCBROpinionDate { get; set; }
        public string UndertakeCBROpinionName { get; set; }
        public string CityUndertakeDeptOpinion { get; set; }
        public string CityUndertakeDeptName_CBR { get; set; }
        public string CityUndertakeDeptOpinionDate_CBR { get; set; }
        public string CityAgsUndertakeDeptOpinion { get; set; }
        public string CityAgsUndertakeDeptName_CBR { get; set; }
        public string CityAgsUndertakeDeptOpinionDate_CBR { get; set; }
        public string CityMainLeaderOpinion { get; set; }
        public string CityMainLeaderName { get; set; }
        public string CityMainLeaderOpinionDate { get; set; }
        public string CityUndertakeDeptExtendOpinion { get; set; }
        public string CityUndertakeDeptExtendName_CBR { get; set; }
        public string CityUndertakeDeptExtendOpinionDate_CBR { get; set; }
        public string CityAgsUndertakeDeptExtendOpinion { get; set; }
        public string CityAgsUndertakeDeptExtendName_CBR { get; set; }
        public string CityAgsUndertakeDeptExtendOpinionDate_CBR { get; set; }
        public string CityUndertakeDeptExtendOpinion_ZhuRen { get; set; }
        public string CityUndertakeDeptExtendName_ZhuRen { get; set; }
        public string CityUndertakeDeptExtendOpinionDate_ZhuRen { get; set; }
        public string CityUndertakeDeptReleaseOpinion { get; set; }
        public string CityUndertakeDeptReleaseName_CBR { get; set; }
        public string CityUndertakeDeptReleaseOpinionDate_CBR { get; set; }
        public string CityAgsUndertakeDeptReleaseOpinion { get; set; }
        public string CityAgsUndertakeDeptReleaseName_CBR { get; set; }
        public string CityAgsUndertakeDeptReleaseOpinionDate_CBR { get; set; }
        public string CityMainLeaderReleaseOpinion { get; set; }
        public string CityMainLeaderReleaseName { get; set; }
        public string CityMainLeaderReleaseOpinionDate { get; set; }
    }

    public class WorkFlowTaskModel
    {  
        public string ID { get; set; }
        public string StepID { get; set; }
        public string FlowID { get; set; }
        public string InstanceID { get; set; }
        public string GroupID { get; set; }
        public string SenderID { get; set; }
        public string SenderName { get; set; }
        public string SenderTime { get; set; }
        public string ReceiveID { get; set; }
        public string ReceiveName { get; set; }
        public string ToUserID { get; set; }
        public string ToUserName { get; set; }
        public string ClueGroupID { get; set; }
    }
}
