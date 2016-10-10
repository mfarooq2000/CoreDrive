using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreDrive.Models;
using CoreDrive.Authentication;
using System.Web.Caching;
using System.Web.Configuration;

namespace CoreDrive.Controllers
{
    [CustomAuthorize]
    public class HomeController : Controller
    {
        iManageServiceWebMock iManage;
        private ConfigManager cm;
        private List<Item> lstWorkCategories = new List<Item>();
        public List<Item> lstIssueTypes = new List<Item>();
        public List<Item> lstComplianceOrgUnits = new List<Item>();
        public List<Item> lstLegalEntities = new List<Item>();

        private const string WORK_CATEGORIES_CACHE_KEY = "WorkCategories";
        private const string ISSUES_CACHE_KEY = "Issues";
        private const string COMPLIANCE_ORG_UNITS_CACHE_KEY = "ComplianceOrgUnits";
        private const string LEGAL_ENTITIES_CACHE_KEY = "LegalEntities";

        public HomeController()
        {
            iManage = new iManageServiceWebMock();
            cm = new ConfigManager();
            //workSpaceDataSet = iManage.getWorkSpacesFromDB();
        }

        public ActionResult Index()
        {
            SearchModel model = new SearchModel();
            PopulateAdvancedSearchDropdowns();
            model.lstWorkCategories = lstWorkCategories;
            model.lstIssueTypes = lstIssueTypes;
            model.lstComplianceOrgUnits = lstComplianceOrgUnits;
            model.lstLegalEntities = lstLegalEntities;

            model.WorkSpaceDataSet = null;
            ViewBag.AdvancedSearch = false;
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(SearchModel model)
        {
            model.WorkSpaceDataSet = iManage.searchWorkSpacesFromDB(false, null, model.q);
             PopulateAdvancedSearchDropdowns();

            model.lstWorkCategories = lstWorkCategories;
            model.lstIssueTypes = lstIssueTypes;
            model.lstComplianceOrgUnits = lstComplianceOrgUnits;
            model.lstLegalEntities = lstLegalEntities;

            ViewBag.AdvancedSearch = false;
            return View(model);
        }

        public ActionResult AdvancedSearch()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AdvancedSearch(SearchModel model)
        {
            PopulateAdvancedSearchDropdowns();
            model.lstWorkCategories = lstWorkCategories;
            model.lstIssueTypes = lstIssueTypes;
            model.lstComplianceOrgUnits = lstComplianceOrgUnits;
            model.lstLegalEntities = lstLegalEntities;

            model.WorkSpaceDataSet = iManage.searchWorkSpacesFromDB(true, model.WorkSpace, "");
            ViewBag.AdvancedSearch = true;
            return View("Index", model);
        }

        public ActionResult Issue()
        {
            IssueModel model = new IssueModel();
            PopulateAdvancedSearchDropdowns();
            model.lstWorkCategories = lstWorkCategories;
            model.lstIssueTypes = lstIssueTypes;
            model.lstComplianceOrgUnits = lstComplianceOrgUnits;
            model.lstLegalEntities = lstLegalEntities;
            return View(model);
        }


        private void PopulateAdvancedSearchDropdowns()
        {
            //Get the Lookup and Autocomplete Data
            int CacheTimeoutKey = Convert.ToInt32(cm.CacheTimeoutKey);
            lstWorkCategories = GetWorkCategoriesFromCache(CacheTimeoutKey, WORK_CATEGORIES_CACHE_KEY);
            lstIssueTypes = GetWorkCategoriesFromCache(CacheTimeoutKey, ISSUES_CACHE_KEY);
            lstComplianceOrgUnits = GetWorkCategoriesFromCache(CacheTimeoutKey, COMPLIANCE_ORG_UNITS_CACHE_KEY);
            lstLegalEntities = GetWorkCategoriesFromCache(CacheTimeoutKey, LEGAL_ENTITIES_CACHE_KEY);
        }

        private List<Item> GetWorkCategoriesFromCache(int CacheTimeoutKey, string CacheKey)
        {
            List<Item> optionList = new List<Item>();
            if (HttpRuntime.Cache[CacheKey] == null)
            {
                optionList = iManage.getOptionSet(CacheKey);
                
                if (optionList != null)
                {
                    HttpRuntime.Cache.Insert(CacheKey, optionList, null, DateTime.Now.AddMinutes(CacheTimeoutKey), Cache.NoSlidingExpiration);
                }
            }
            else
            {
                optionList = (List<Item>)HttpRuntime.Cache[CacheKey];
            }
            return optionList;
        }

       
    }
}