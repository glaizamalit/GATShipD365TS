namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("vwStaff")]
    public partial class Staff
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(6)]
        public string emp_no { get; set; }

        [StringLength(40)]
        public string first { get; set; }

        [StringLength(20)]
        public string last { get; set; }

        [StringLength(128)]
        public string MidName { get; set; }

        [StringLength(20)]
        public string christ { get; set; }

        [StringLength(40)]
        public string name { get; set; }

        [StringLength(4)]
        public string init_ { get; set; }

        [StringLength(3)]
        public string post { get; set; }

        [StringLength(50)]
        public string PostDesc { get; set; }

        [StringLength(128)]
        public string JobCode { get; set; }

        [StringLength(1)]
        public string sex { get; set; }

        [StringLength(128)]
        public string Division { get; set; }

        [StringLength(2)]
        public string loc { get; set; }

        [StringLength(3)]
        public string co { get; set; }

        [StringLength(80)]
        public string Co_Desc { get; set; }

        [StringLength(128)]
        public string BranchCity { get; set; }

        [StringLength(3)]
        public string BranchCode { get; set; }

        [StringLength(30)]
        public string Dept_Desc { get; set; }

        [StringLength(4)]
        public string dept { get; set; }

        [StringLength(50)]
        public string PC_OfEmail { get; set; }

        [StringLength(50)]
        public string EmailDisplayName { get; set; }

        [StringLength(4)]
        public string MgrUserName { get; set; }

        [StringLength(50)]
        public string MgrEmail { get; set; }

        [StringLength(10)]
        public string MgrSFStaffNo { get; set; }

        [StringLength(4)]
        public string HRRepUserName { get; set; }

        [StringLength(50)]
        public string HRRepEmail { get; set; }

        [StringLength(10)]
        public string HRRepSFStaffNo { get; set; }

        public DateTime? commence { get; set; }

        [StringLength(100)]
        public string TimeZoneDesc { get; set; }

        [StringLength(64)]
        public string TimeZoneSF { get; set; }

        public DateTime? term_dt { get; set; }

        [StringLength(4)]
        public string create_by { get; set; }

        public DateTime? create_dt { get; set; }

        public DateTime? update_dt { get; set; }

        [StringLength(4)]
        public string update_by { get; set; }

        [StringLength(8)]
        public string com_no { get; set; }

        [StringLength(10)]
        public string Local_ID { get; set; }

        public DateTime? EffectiveDate { get; set; }

        [StringLength(128)]
        public string CountryID { get; set; }

        [StringLength(3)]
        public string PC_OfTelCC { get; set; }

        [StringLength(5)]
        public string PC_OfTelAC { get; set; }

        [StringLength(20)]
        public string PC_OfTelNo { get; set; }

        [StringLength(5)]
        public string PC_OfTelExt { get; set; }

        [StringLength(3)]
        public string PC_OfFaxCC { get; set; }

        [StringLength(5)]
        public string PC_OfFaxAC { get; set; }

        [StringLength(20)]
        public string PC_OfFaxNo { get; set; }

        [StringLength(255)]
        public string PersonalAddr { get; set; }

        [StringLength(255)]
        public string BusinessAddr { get; set; }

        [StringLength(100)]
        public string CtryOfOper { get; set; }

        [StringLength(100)]
        public string MatrixMgr { get; set; }

        [StringLength(100)]
        public string Proxy { get; set; }

        [StringLength(100)]
        public string Seating { get; set; }

        [StringLength(3)]
        public string PC_MobileCC { get; set; }

        [StringLength(5)]
        public string PC_MobileAC { get; set; }

        [StringLength(20)]
        public string PC_MobileNo { get; set; }

        [StringLength(255)]
        public string EOCStatus { get; set; }

        public DateTime? DOB { get; set; }

        public DateTime? DOCP { get; set; }

        [StringLength(100)]
        public string SecondMgr { get; set; }

        [StringLength(255)]
        public string Term_Reason { get; set; }

        [StringLength(255)]
        public string RecruitmentSource { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string UserLoginID { get; set; }

        [StringLength(500)]
        public string ExitProcPath { get; set; }

        [StringLength(100)]
        public string ExitProcOriName { get; set; }

        public bool? AddToWP { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string CostCentre { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(80)]
        public string JobFamily { get; set; }

        public int? Probation { get; set; }
    }
}
