using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Deployment.Application;
using Microsoft.AnalysisServices.AdomdClient;

namespace UILibrary.Olap.UITestApplication.Web
{

	/// <summary>
	/// Summary description for InitializeWebService
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
	[System.Web.Script.Services.ScriptService]
	public class InitializeWebService : System.Web.Services.WebService
	{
		[WebMethod]
		public string InitConnectionString(String connectionName, String connectionString)
		{
			try
			{
				using (var connection = new AdomdConnection(connectionString))
				{
					string s = "";
					connection.Open();
					foreach (var c in connection.Cubes)
					{
						s += c.Name;
					}
				}
				this.Application[connectionName] = connectionString;
				return null;
			}
			catch (Exception E)
			{
				return E.ToString();
			}
		}
	}
}
