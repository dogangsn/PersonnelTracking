using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Hosting;

using System.Web.Configuration;
using System.Data.Entity;
using System.Net;

namespace PersonnelTracking
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
           // DevExpress.XtraReports.Web.WebDocumentViewer.Native.WebDocumentViewerBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Disabled;
          //  DevExpress.XtraReports.Web.QueryBuilder.Native.QueryBuilderBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Disabled;
          //  DevExpress.XtraReports.Web.ReportDesigner.Native.ReportDesignerBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Disabled;
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

          //  DevExpress.Web.Mvc.MVCxReportDesigner.StaticInitialize();
        }
        protected void Application_End()
        {
            //var client = new WebClient();
            //var url = App.AdminConfiguration.MonitorHostUrl + "ping.aspx";
            //client.DownloadString(url);
            //Trace.WriteLine("Application Shut Down Ping: " + url);
        }
    }
}
