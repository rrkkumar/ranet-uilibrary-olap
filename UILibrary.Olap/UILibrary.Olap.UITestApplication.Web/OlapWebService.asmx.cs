using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Ranet.AgOlap.Controls.General.ClientServer;

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
        public String PerformOlapServiceAction(String schemaType, String schema)
        {
            return base.PerformOlapServiceAction(schemaType, schema);
        }

        #region Obsolete

        [WebMethod]
        [Obsolete("Use PerformOlapServiceAction")]
        public String GetMetaData(String schema)
        {
            return PerformOlapServiceAction("GetMetadata",schema);
        }

        [WebMethod]
        [Obsolete("Use PerformOlapServiceAction")]
        public String GetMembersData(String schema)
        {
            return PerformOlapServiceAction("GetMembers", schema);
        }

        [WebMethod]
        [Obsolete("Use PerformOlapServiceAction")]
        public String GetPivotData(String schema)
        {
            return PerformOlapServiceAction("GetPivotData",schema);
        }

        [WebMethod]
        [Obsolete("Use PerformOlapServiceAction")]
        public String PerformMemberAction(String schema)
        {
            return PerformOlapServiceAction("MemberAction", schema);
        }

        [WebMethod]
        [Obsolete("Use PerformOlapServiceAction")]
        public String PerformStorageAction(String schema)
        {
            return PerformOlapServiceAction("StorageAction", schema);
        }

        [WebMethod]
        [Obsolete("Use PerformOlapServiceAction")]
        public String PerformServiceCommand(String schema)
        {
            return PerformOlapServiceAction("ServiceCommand", schema);
        }

        [WebMethod]
        [Obsolete("Use PerformOlapServiceAction")]
        public String GetToolBarInfo(String schema)
        {
            return PerformOlapServiceAction("GetToolBarInfo", schema);
        }

        [WebMethod]
        [Obsolete("Use PerformOlapServiceAction")]
        public String UpdateCube(String schema)
        {
            return PerformOlapServiceAction("UpdateCube", schema);
        }

        [WebMethod]
        [Obsolete("Use PerformOlapServiceAction")]
        public String ExecuteQuery(String schema)
        {
            return PerformOlapServiceAction("ExecuteQuery", schema);
        }
        #endregion Obsolete

        [WebMethod]
        public void RunExcel(String schema)
        {
            String res = PerformOlapServiceAction("ServiceCommand", schema);
            this.Context.Response.ClearContent();
            this.Context.Response.Buffer = true;
            this.Context.Response.ContentType = "application/vnd.ms-excel";
            this.Context.Response.Write(res);
        }
        
        [WebMethod]
        public String About()
        {
            return "Web Service for Visual OLAP Controls Library";
        }
    }
}
