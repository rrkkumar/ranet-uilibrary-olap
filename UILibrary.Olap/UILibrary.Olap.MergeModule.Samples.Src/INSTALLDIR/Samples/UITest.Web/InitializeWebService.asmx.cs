/*   
    Copyright (C) 2009 Galaktika Corporation ZAO

    This file is a part of Ranet.UILibrary.Olap
 
    Ranet.UILibrary.Olap is a free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
      
    You should have received a copy of the GNU General Public License
    along with Ranet.UILibrary.Olap.  If not, see <http://www.gnu.org/licenses/>.
  
    If GPL v.3 is not suitable for your products or company,
    Galaktika Corp provides Ranet.UILibrary.Olap under a flexible commercial license
    designed to meet your specific usage and distribution requirements.
    If you have already obtained a commercial license from Galaktika Corp,
    you can use this file under those license terms.
*/

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
				if (string.IsNullOrEmpty(connectionString))
					connectionString = this.Application[connectionName] as string;
					
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
