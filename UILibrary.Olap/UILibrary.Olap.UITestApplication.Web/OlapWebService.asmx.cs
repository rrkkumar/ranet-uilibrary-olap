using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace UILibrary.Olap.UITestApplication.Web
{
    /// <summary>
    /// Summary description for OlapWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class OlapWebService : Ranet.Web.Olap.OlapWebServiceBase
    {
        [WebMethod]
        public String GetMetaData(String schema)
        {
            return base.GetMetaData(schema);
        }

        [WebMethod]
        public String GetMembersData(String schema)
        {
            return base.GetMembersData(schema);
        }

        [WebMethod]
        public String GetPivotData(String schema)
        {
            return base.GetPivotData(schema);
        }

        [WebMethod]
        public String PerformMemberAction(String schema)
        {
            return base.PerformMemberAction(schema);
        }

        [WebMethod]
        public String PerformStorageAction(String schema)
        {
            return base.PerformStorageAction(schema);
        }

        [WebMethod]
        public String PerformServiceCommand(String schema)
        {
            return base.PerformServiceCommand(schema);
        }

        [WebMethod]
        public String GetToolBarInfo(String schema)
        {
            return base.GetToolBarInfo(schema);
        }

        [WebMethod]
        public String UpdateCube(String schema)
        {
            return base.UpdateCube(schema);
        }

        [WebMethod]
        public String ExecuteQuery(String schema)
        {
            return base.ExecuteQuery(schema);
        }

        [WebMethod]
        public void RunExcel(String schema)
        {
            String res = base.PerformServiceCommand(schema);
            this.Context.Response.ClearContent();
            this.Context.Response.Buffer = true;
            this.Context.Response.ContentType = "application/vnd.ms-excel";
            this.Context.Response.Write(res);
        }
    }
}
