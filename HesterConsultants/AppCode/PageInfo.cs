using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HesterConsultants.Properties;

namespace HesterConsultants.AppCode
{
    public class PageInfo
    {
        private string pageName;
        private string pageId;
        private string url;

        public PageInfo(string name, string id, string url)
        {
            this.pageName = name;
            this.pageId = id;
            this.url = url;
        }

        public string PageName
        {
            get
            {
                return pageName;
            }
            set
            {
                pageName = value;
            }
        }

        public string PageId
        {
            get
            {
                return pageId;
            }
            set
            {
                pageId = value;
            }
        }

        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
            }
        }

        //public static PageInfo HomeAbsolute
        //{
        //    get
        //    {
        //        return new PageInfo("Home", "home", Settings.Default.CompanyUrlFull + "/");
        //    }
        //}

        public static PageInfo HomeRelative
        {
            get
            {
                return new PageInfo("Home", "home", "/");
            }
        }

        public static PageInfo ClientsRelative
        {
            get
            {
                return new PageInfo("Client Home", "clients", Settings.Default.ClientHomeUrl);
            }
        }

        //public static PageInfo DemosAbsolute
        //{
        //    get
        //    {
        //        return new PageInfo("Demos", "demos", Settings.Default.CompanyUrlFull + "/demos/");
        //    }
        //}

        public static PageInfo DemosRelative
        {
            get
            {
                return new PageInfo("Demos", "demos", Settings.Default.DemosUrl);
            }
        }

        //public static PageInfo ServicesAbsolute
        //{
        //    get
        //    {
        //        return new PageInfo("Services", "services", Settings.Default.CompanyUrlFull + "/services/");
        //    }
        //}

        public static PageInfo ServicesRelative
        {
            get
            {
                return new PageInfo("Services", "services", Settings.Default.ServicesUrl);
            }
        }

        //public static PageInfo AboutAbsolute
        //{
        //    get
        //    {
        //        return new PageInfo("About Us", "about", Settings.Default.CompanyUrlFull + "/about/");
        //    }
        //}

        public static PageInfo AboutRelative
        {
            get
            {
                return new PageInfo("About Us", "about", Settings.Default.AboutUrl);
            }
        }

        //public static PageInfo ContactAbsolute
        //{
        //    get
        //    {
        //        return new PageInfo("Contact", "contact", Settings.Default.CompanyUrlFull + "/contact/");
        //    }
        //}
        
        public static PageInfo ContactRelative
        {
            get
            {
                return new PageInfo("Contact", "contact", Settings.Default.ContactUrl);
            }
        }

        //public static PageInfo ContactSecure
        //{
        //    get
        //    {
        //        return new PageInfo("Contact", "contact", Global.COMPANY_URL_FULL_SECURE + "/contact/");
        //    }
        //}

        //public static PageInfo ReportsAbsolute
        //{
        //    get
        //    {
        //        return new PageInfo("Reports", "reports", Settings.Default.CompanyUrlFull + "/reports/");
        //    }
        //}

        public static PageInfo ReportsRelative
        {
            get
            {
                return new PageInfo("Reports", "reports", "/reports/");
            }
        }

        //public static PageInfo StatsAbsolute
        //{
        //    get
        //    {
        //        return new PageInfo("Stats", "stats", Settings.Default.CompanyUrlFull + "/stats/");
        //    }
        //}

        public static PageInfo StatsRelative
        {
            get
            {
                return new PageInfo("Stats", "stats", "/stats/");
            }
        }

        public static PageInfo AdminRelative
        {
            get
            {
                return new PageInfo("Admin", "admin", "/admin/");
            }
        }

        //private static string RootUrl()
        //{
        //    HttpContext context = HttpContext.Current;
            
        //    if ((bool)context.Session[SiteUtils.SESSION_LOCAL])
        //        return "/";
        //    else
        //        return Settings.Default.CompanyUrlFull.Replace("http:", "https:");
        //}

        //private static string ContactUrl()
        //{
        //    return RootUrl() + "/contact/";
        //}

    }
}
