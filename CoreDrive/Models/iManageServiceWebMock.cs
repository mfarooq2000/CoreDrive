using System;
using System.Collections.Generic;
using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNet.Mvc;
//using TieWeb.Models;
//using Microsoft.AspNet.Authorization;
//using log4net;

namespace CoreDrive.Models
{
    public class iManageServiceWebMock
    {
        Random random = new Random();
        int dropdownsDummyDataMaxLimit = 68;
        string[] Issues = { "Adequate Housing", "Administration of Justice", "Albinism", "Business and Human Rights", "Children", "Civil and Political Rights", "Climate change", "Coercive measures ", "Cultural rights", "Death penalty", "Democracy", "Detention", "Development (Good Governance and Debt)", "Disability and Human Rights", "Disappearances", "Discrimination", "Economic, Social and Cultural Rights", "Education", "Environment", "Executions", "Food", "Forced evictions", "Freedom of Opinion and Expression", "Freedom of peaceful assembly and of association", "Freedom of Religion and Belief", "Health", "HIV/AIDS", "Human Rights Defenders", "Human rights education and training", "Human Rights Indicators", "Humanitarian action", "Independence of Judiciary", "Indigenous Peoples", "Internal Displacement", "International Order", "International Solidarity", "Land and Human Rights", "Mercenaries", "Migration", "Minorities", "Nationality", "Older persons", "Plans of Action for the Promotion and Protection of Human Rights", "Poverty", "Privacy", "Racism", "Rule of Law", "Situations", "Slavery", "Social Security", "Terrorism", "The 2030 Agenda for Sustainable Development", "Torture", "Trade and Investment", "Traditional values", "Trafficking in Persons", "Transitional Justice", "Treaty Body Strengthening", "Urbanization and Human Rights", "Violent extremism", "Water and sanitation", "Women", "Youth" };


        public iManageServiceWebMock()
        {
            //Fill workspaces from Database
            getWorkSpacesFromDB();
        }

        public CoreDriveWorkspace getWorkSpace(long workspaceID)
        {
            CoreDriveWorkspace cdWS = null;

            if (workspaceID >= 1 && workspaceID <= 10)
            {
                int i = 1;

                cdWS = new CoreDriveWorkspace();
                cdWS.Id = i;
                cdWS.IssueName = "Issue " + i;
                cdWS.UniqueReferenceNumber = "Ref Number " + i; //TODO: where is refNo
                cdWS.WorkCategory = i; //TODO
                cdWS.IssueOpenDate = DateTime.Today.AddDays(i * -1);
                cdWS.IssueType = i;
                cdWS.Status = 1;//TODO: where is status
                cdWS.ComplianceOrgUnit =  i;
                cdWS.LegalEntity = i; //TODO
                cdWS.LineOfBusiness = "Custom-" + i;
                cdWS.RelatedIssue = "Related issue " + i; //TODO
                cdWS.Branch = "branch " + i; //TODO
                cdWS.CostCenter = "CostCenter " + i; //TODO
                cdWS.Confidential = imSecurityType.imPrivate;
                cdWS.PaperFile = true; //TODO


                cdWS.GACLs.Add(new ACL("Administrator", "Administrator Group", true, imNOS.imOSVirtual, imAccessRight.imRightAll));
                cdWS.GACLs.Add(new ACL("EndUser", "End User Group", true, imNOS.imOSExternal, imAccessRight.imRightRead));

            }

            return cdWS;
        }

        public List<CoreDriveWorkspace> searchWorkSpacesFromDB(bool IsAdvancedSearch, CoreDriveWorkspace workspace, string query)
        {
            List<CoreDriveWorkspace> workSpaces = getWorkSpacesFromDB();
            List<CoreDriveWorkspace> searchResults = new List<CoreDriveWorkspace>();
            if (workSpaces != null && workSpaces.Count > 0)
            {
                if (IsAdvancedSearch)
                {
                    List<CoreDriveWorkspace> advacedSearchResults = new List<CoreDriveWorkspace>();
                    advacedSearchResults = workSpaces;
                    if (!string.IsNullOrWhiteSpace(workspace.IssueName))
                        advacedSearchResults = workSpaces.Where(w => w.IssueName.StartsWith(workspace.IssueName, StringComparison.CurrentCultureIgnoreCase)).ToList();

                    if (workspace.WorkCategory > 0)
                        advacedSearchResults.AddRange(workSpaces.Where(w => w.WorkCategory == workspace.WorkCategory).ToList());

                    if (workspace.IssueType > 0)
                        advacedSearchResults.AddRange(workSpaces.Where(w => w.IssueType == workspace.IssueType).ToList());

                    if (workspace.ComplianceOrgUnit > 0)
                        advacedSearchResults.AddRange(workSpaces.Where(w => w.ComplianceOrgUnit == workspace.ComplianceOrgUnit).ToList());

                    if (workspace.LegalEntity > 0)
                        advacedSearchResults.AddRange(workSpaces.Where(w => w.LegalEntity == workspace.LegalEntity).ToList());

                    if (!string.IsNullOrWhiteSpace(workspace.LineOfBusiness))
                        advacedSearchResults = workSpaces.Where(w => w.LineOfBusiness.StartsWith(workspace.LineOfBusiness, StringComparison.CurrentCultureIgnoreCase)).ToList();

                    advacedSearchResults = advacedSearchResults.Distinct().ToList();

                    if (workspace.Status > 0)
                        advacedSearchResults.AddRange(workSpaces.Where(w => w.Status == workspace.Status).ToList());

                    if (!string.IsNullOrWhiteSpace(workspace.UniqueReferenceNumber))
                        advacedSearchResults = workSpaces.Where(w => w.UniqueReferenceNumber.StartsWith(workspace.UniqueReferenceNumber, StringComparison.CurrentCultureIgnoreCase)).ToList();

                    searchResults = advacedSearchResults;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(query))
                        searchResults = workSpaces.Where(w => w.IssueName.StartsWith(query, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    else
                        searchResults = workSpaces;
                }
            }
            return searchResults;
        }

        public List<CoreDriveWorkspace> getWorkSpacesFromDB()
        {
            List<CoreDriveWorkspace> workSpaces = new List<CoreDriveWorkspace>();
            CoreDriveWorkspace cdWS;
            for (int i = 0; i < Issues.Length; i++)
            {
                cdWS = new CoreDriveWorkspace();
                cdWS.Id = i;
                cdWS.IssueName = Issues[i];
                cdWS.WorkCategory =  i;
                cdWS.UniqueReferenceNumber = "Ref Number " + i; //TODO: where is refNo
                cdWS.IssueType = i;
                cdWS.ComplianceOrgUnit =  i;
                cdWS.LineOfBusiness = "Custom-" + i;
                cdWS.Confidential = imSecurityType.imPrivate;
                cdWS.Status = (i % 3 == 0) ? 1 : 2;

                workSpaces.Add(cdWS);
            }
            return workSpaces;
        }


        public List<ACL> searchSecurityGroups(string value)
        {
            List<ACL> GACLs = new List<ACL>();
            GACLs.Add(new ACL("Administrator", "Administrator Group", true, imNOS.imOSVirtual, imAccessRight.imRightAll));
            GACLs.Add(new ACL("EndUser", "End User Group", true, imNOS.imOSExternal, imAccessRight.imRightRead));
            GACLs.Add(new ACL("Public", "Public Group", true, imNOS.imOSVirtual, imAccessRight.imRightAll));
            GACLs.Add(new ACL("Department", "Department Group", true, imNOS.imOSExternal, imAccessRight.imRightRead));

            return GACLs;
        }

        public List<Item> getOptionSet(string key)
        {
            List<Item> ret = new List<Item>();
            switch (key)
            {
                case "WorkCategories": ret = getWorkCategories(); break;
                case "Issues": ret = getIssueTypes(); break;
                case "ComplianceOrgUnits": ret = getComplianceOrgUnits(); break;
                case "LegalEntities": ret = getLegalEntities(); break;
            }
            return ret;
        }


        private List<Item> getWorkCategories()
        {
            List<Item> ret = new List<Item>();
            for (int i = 1; i <= dropdownsDummyDataMaxLimit; i++)
                ret.Add(new Item() { Name = "Work Category " + i, Value= i,  Selected = false});
            return ret;
        }

        private List<Item> getIssueTypes()
        {
            List<Item> ret = new List<Item>();
            for (int i = 1; i <= dropdownsDummyDataMaxLimit; i++)
                ret.Add(new Item() { Name = "Issue Type " + i, Value = i, Selected = false });
            return ret;
        }

        private List<Item> getComplianceOrgUnits()
        {
            List<Item> ret = new List<Item>();
            for (int i = 1; i <= dropdownsDummyDataMaxLimit; i++)
                ret.Add(new Item() { Name = "Compliance Org Unit " + i, Value = i, Selected = false });
            return ret;
        }

        private List<Item> getLegalEntities()
        {
            List<Item> ret = new List<Item>();
            for (int i = 1; i <= dropdownsDummyDataMaxLimit; i++)
                ret.Add(new Item() { Name = "Legal Entity " + i, Value = i, Selected = false });
            return ret;
        }

      

    }


}