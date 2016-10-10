using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace CoreDrive.Models
{
    #region /****** Models *******/
    public class AccountModel
    {
        public Guid AccountId { get; set; }
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Trusted Login?")]
        public bool TrustedLogin { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }
    }

    public class SearchModel
    {
        public CoreDriveWorkspace WorkSpace { get; set; }
        public List<CoreDriveWorkspace> WorkSpaceDataSet { get; set; }
        public List<Item> lstWorkCategories { get; set; }
        public List<Item> lstIssueTypes { get; set; }
        public List<Item> lstComplianceOrgUnits { get; set; }
        public List<Item> lstLegalEntities { get; set; }
        public string q { get; set; }
    }

    public class IssueModel
    {
        public CoreDriveWorkspace WorkSpace { get; set; }
        public List<Item> lstWorkCategories { get; set; }
        public List<Item> lstIssueTypes { get; set; }
        public List<Item> lstComplianceOrgUnits { get; set; }
        public List<Item> lstLegalEntities { get; set; }
        public List<Item> lstAccessGroups { get; set; }
    }

    public class iManageUser
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public iManageUser(string userID, string userName)
        {
            UserID = userID;
            UserName = userName;
        }
    }

    public class CoreDriveWorkspace
    {
        public int Id { get; set; }
        public string IssueName { get; set; }
        public string UniqueReferenceNumber { get; set; }
        public int IssueType { get; set; }
        public int WorkCategory { get; set; }
        public DateTime IssueOpenDate { get; set; }
        public int Status { get; set; }
        public int ComplianceOrgUnit { get; set; }
        public int LegalEntity { get; set; }
        public string LineOfBusiness { get; set; }
        public string RelatedIssue { get; set; }
        public string Branch { get; set; }
        public string CostCenter { get; set; }
        public imSecurityType Confidential { get; set; }
        public bool PaperFile { get; set; }
        //public bool Confidential { get; set; }
        public List<ACL> GACLs = new List<ACL>();
    }

    public class ACL
    {
        public ACL(string groupName, string fullName, bool enabled, imNOS nos, imAccessRight right)
        {
            GroupName = groupName;
            FullName = fullName;
            Enabled = enabled;
            Nos = nos;
            Right = right;
        }

        public string GroupName { get; set; }
        public imAccessRight Right { get; set; }
        public string FullName { get; set; }
        public bool Enabled { get; set; }
        public imNOS Nos { get; set; }
    }

    public class Item
    {
        public int Value { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }
    #endregion

    #region /****** Enumerations *******/

  

    public enum imSecurityType
    {
        imPublic = 80,
        imView = 86,
        imPrivate = 88,
    }
    public enum imNOS
    {
        imOSNovell = 1,
        imOSVirtual = 2,
        imOSNT = 3,
        imOSNovellNDS = 4,
        imOSExternal = 5,
        imOSNTADS = 6,
        imOSNetscapeDS = 7,
    }
    public enum imAccessRight
    {
        imRightNone = 0,
        imRightRead = 1,
        imRightReadWrite = 2,
        imRightAll = 3,
    }
    #endregion

    public class ConfigManager
    {
        public int CacheTimeoutKey { get { try { return Int32.Parse(WebConfigurationManager.AppSettings["CacheTimeoutKey"]); } catch { return 20; } } }
    }

}