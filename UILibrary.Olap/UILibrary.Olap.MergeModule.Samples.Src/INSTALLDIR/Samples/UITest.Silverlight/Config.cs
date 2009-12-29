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
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.IO.IsolatedStorage;
using System.ServiceModel;

namespace UILibrary.Olap.UITestApplication
{
	using Init;

	public class Config
	{
		public static Data _Default = null;
		public static FieldInfo[] DataFields;
		public static Action[] Getters;
		static Config()
		{
			Load();
		}

		public static void Load()
		{
			try
			{
				_Default = IsolatedStorageSettings.ApplicationSettings["Config"] as Data;
			}
			catch
			{
			}
			try
			{
				if (_Default == null)
				{
					_Default = new Data();
					Save();
				}
			}
			catch (Exception E)
			{
				MessageBox.Show(E.ToString(), "Error", MessageBoxButton.OK);
			}
		}
		public static void Save()
		{
			try
			{
				try
				{
					IsolatedStorageSettings.ApplicationSettings["Config"] = _Default;
				}
				catch
				{
					IsolatedStorageSettings.ApplicationSettings.Add("Config", _Default);
				}
				IsolatedStorageSettings.ApplicationSettings.Save();
			}
			catch (Exception E)
			{
				MessageBox.Show(E.ToString(), "Error", MessageBoxButton.OK);
			}
		}
		

		public class Data
		{
			public string InitializeWebServiceUrl { get; set; }
			public string OlapWebServiceUrl { get; set; }
			
			public string ConnectionStringId { get { return "OLAPConnectionString"; } }
			public string ConectionStringOLAP { get; set; }

			public string MdxQuery { get; set; }
			public string UpdateScript { get; set; }
			public string MdxDesignerLayout { get; set; }

			public string MinValue { get; set; }
			public string LowValue { get; set; }
			public string CurrentValue { get; set; }
			public string HightValue { get; set; }
			public string MaxValue { get; set; }
			public string GaugeTextTemplate { get; set; }
			public string GaugeToolTipTemplate { get; set; }

			public Data()
			{
				SetDefault();
			}
			
			public void SetDefault()
			{
				ConectionStringOLAP = @"Data Source=.\sql2008;Integrated Security=SSPI;Initial Catalog=Adventure Works DW";
				InitializeWebServiceUrl = new Uri(new Uri(Ranet.AgOlap.Services.ServiceManager.BaseAddress), @"InitializeWebService.asmx").AbsoluteUri;
				OlapWebServiceUrl = new Uri(new Uri(Ranet.AgOlap.Services.ServiceManager.BaseAddress), @"OlapWebService.asmx").AbsoluteUri;

				MdxQuery = @"select [Product].[Product Categories].[Category] on 0 ,[Date].[Calendar].[Calendar Year]  DIMENSION PROPERTIES PARENT_UNIQUE_NAME on 1 from [Adventure Works]
 where [Measures].[Sales Amount]";

				UpdateScript = "";
				MdxDesignerLayout = "";

				MinValue = "10";
				LowValue = "20";
				CurrentValue = "82";
				HightValue = "80";
				MaxValue = "180";
				GaugeTextTemplate= @"Text={2}/{4}. Feel free to change.";
				GaugeToolTipTemplate = @"{0}/{1}/{2}/{3}/{4}.
You can dynamically change values and tooltip text.";	
			}
		}
		public static void CheckDataService(Action OnSuccess, Action OnError)
		{
			try
			{
				string wdUrl = _Default.OlapWebServiceUrl;

				var wds = Ranet.AgOlap.Services.ServiceManager.CreateService
				<Ranet.AgOlap.OlapWebService.OlapWebServiceSoapClient
				, Ranet.AgOlap.OlapWebService.OlapWebServiceSoap
				>(wdUrl);

				wds.PerformOlapServiceActionCompleted += (object sender, Ranet.AgOlap.OlapWebService.PerformOlapServiceActionCompletedEventArgs e) =>
				{
					if (e.Error == null)
					{
						if (e.Result == "OK")
						{
							if (OnSuccess != null)
								OnSuccess();

						}
						else if (e.Result == null)
						{
							MessageBox.Show("Data service has returned 'null'", "Error", MessageBoxButton.OK);
							if (OnError != null)
								OnError();

						}
						else
						{
							MessageBox.Show(e.Result, "Error", MessageBoxButton.OK);
							if (OnError != null)
								OnError();
						}
					}
					else
					{
						MessageBox.Show(e.Error.ToString(), "Error", MessageBoxButton.OK);
						if (OnError != null)
							OnError();
					}
				};
				wds.PerformOlapServiceActionAsync("CheckExist", "");
			}
			catch (Exception E)
			{
				MessageBox.Show(E.ToString(), "Error", MessageBoxButton.OK);
				if (OnError != null)
					OnError();
			}
		}
		public static void Init(string connectionStringId, string connectionString, Action OnSuccess, Action OnError)
		{
			var service = Ranet.AgOlap.Services.ServiceManager.CreateService
			 <InitializeWebServiceSoapClient
			 , InitializeWebServiceSoap
			 >(_Default.InitializeWebServiceUrl);

			service.InitConnectionStringCompleted += (object sender, Init.InitConnectionStringCompletedEventArgs e) =>
			{
				if (e.Error == null)
				{
					if (e.Result == null)
						CheckDataService(OnSuccess, OnError);
					else
					{
						MessageBox.Show(e.Result, "Error", MessageBoxButton.OK);
						if (OnError != null)
							OnError();

					}
				}
				else
				{
					MessageBox.Show(e.Error.ToString(), "Error", MessageBoxButton.OK);
					if (OnError != null)
						OnError();
				}
			};
			service.InitConnectionStringAsync(connectionStringId, connectionString);
		}
	}
}
