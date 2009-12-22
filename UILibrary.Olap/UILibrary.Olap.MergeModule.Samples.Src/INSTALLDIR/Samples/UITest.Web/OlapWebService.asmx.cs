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
	  public String PerformOlapServiceAction(String schemaType, String schema)
	  {
			if (schemaType=="CheckExist")
				return "OK";
				
			return base.PerformOlapServiceAction(schemaType, schema);
		}
	}
}
