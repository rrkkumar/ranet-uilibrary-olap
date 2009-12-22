using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace UILibrary.Olap.UITestApplication.Web
{
    /// <summary>
    /// Summary description for InitializeWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class InitializeWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public void InitConnectionString(String connectionName, String connectionString)
        {
            this.Application[connectionName] = connectionString;
        }
    }
}
