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
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.IO.IsolatedStorage;
using System.ServiceModel;

namespace UILibrary.Olap.UITestApplication
{
	using Init;

	public class Data
	{
		public string InitializeWebServiceUrl { get; set; }
		public string OlapWebServiceUrl { get; set; }

		public string OLAPConnectionString { get; set; }

		public string MdxQuery { get; set; }
		public string UpdateScript { get; set; }
		public string MdxDesignerLayout { get; set; }

		public string MinValue { get; set; }
		public string LowValue { get; set; }
		public string CurrentValue { get; set; }
		public string HighValue { get; set; }
		public string MaxValue { get; set; }
		public string GaugeTextTemplate { get; set; }
		public string GaugeToolTipTemplate { get; set; }

		public Data()
		{
			SetDefault();
		}

		public Data Clone()
		{
			return (Data)MemberwiseClone();
		}
		void SetDefault()
		{
			OLAPConnectionString = @"Data Source=.\sql2008;Integrated Security=SSPI;Initial Catalog=Adventure Works DW";
			InitializeWebServiceUrl = new Uri(new Uri(Ranet.AgOlap.Services.ServiceManager.BaseAddress), @"InitializeWebService.asmx").AbsoluteUri;
			OlapWebServiceUrl = new Uri(new Uri(Ranet.AgOlap.Services.ServiceManager.BaseAddress), @"OlapWebService.asmx").AbsoluteUri;

			MdxQuery = @"select [Product].[Product Categories].[Category] on 0 ,[Date].[Calendar].[Calendar Year]  DIMENSION PROPERTIES PARENT_UNIQUE_NAME on 1 from [Adventure Works]
 where [Measures].[Sales Amount]";

			UpdateScript = "";
			MdxDesignerLayout = "";

			MinValue = "10";
			LowValue = "20";
			CurrentValue = "82";
			HighValue = "80";
			MaxValue = "180";
			GaugeTextTemplate = @"Text={2}/{4}. Feel free to change.";
			GaugeToolTipTemplate = @"{0}/{1}/{2}/{3}/{4}.
You can dynamically change values and tooltip text.";
		}
	}
	public class Config : INotifyPropertyChanged
	{
		public readonly static Config Default = new Config();
		public readonly static string ConnectionStringId = "OLAPConnectionString";
		
		public event PropertyChangedEventHandler PropertyChanged;
		Data m_Data = null;
		public Data Data 
		{
			get
			{
				if (m_Data==null)
				 Load();
				return m_Data;
			}
			set
			{
				m_Data=value;
				Refresh();
			}
			
		}
		public static void Load()
		{
			try
			{
				Default.m_Data = IsolatedStorageSettings.ApplicationSettings["Config"] as Data;
			}
			catch
			{
			}
			try
			{
				if (Default.m_Data == null)
				{
					Default.m_Data = new Data();
				}
				Default.m_Data = Default.m_Data.Clone();
				Refresh();
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
					IsolatedStorageSettings.ApplicationSettings["Config"] = Default.Data;
				}
				catch
				{
					IsolatedStorageSettings.ApplicationSettings.Add("Config", Default.Data);
				}
				IsolatedStorageSettings.ApplicationSettings.Save();
			}
			catch (Exception E)
			{
				MessageBox.Show(E.ToString(), "Error", MessageBoxButton.OK);
			}
		}
		public static void SetDefault()
		{
			Default.m_Data=new Data();
			Refresh();
		}
		public static void Refresh()
		{
			if (Default.PropertyChanged != null)
				Default.PropertyChanged(Default, new PropertyChangedEventArgs("Data"));
		}
		public static void CheckDataService(Action OnSuccess, Action OnError)
		{
			try
			{
				string wdUrl = Default.Data.OlapWebServiceUrl;

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
			 >(Default.Data.InitializeWebServiceUrl);

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
