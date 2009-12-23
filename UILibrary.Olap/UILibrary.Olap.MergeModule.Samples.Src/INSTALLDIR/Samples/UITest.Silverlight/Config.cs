using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using System.IO;
using System.ServiceModel;

namespace UILibrary.Olap.UITestApplication
{
	using Init;

	public class Config
	{
		public static Data _Default = null;
		static Config() { Load(); }
		public static void Load()
		{
			try
			{
				_Default = IsolatedStorageSettings.ApplicationSettings["Config"] as Data;
			}
			catch
			{
			}
			if (_Default == null)
			{
				RestoreDefault();
				Save();
			}
		}
		public static void RestoreDefault()
		{
			_Default = new Data();
		}
		public static void Save()
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
		public class Data
		{
			public string ConectionStringOLAP;
			public string InitializeWebServiceUrl;
			public string ConnectionStringId { get { return "OLAPConnectionString"; } }
			public string DataWebServiceUrl;
			public Data()
			{
				SetDefault();
			}
			public void SetDefault()
			{
				ConectionStringOLAP = @"Data Source=.\sql2008;Integrated Security=SSPI;Initial Catalog=Adventure Works DW";
				InitializeWebServiceUrl = new Uri(new Uri(Ranet.AgOlap.Services.ServiceManager.BaseAddress), @"InitializeWebService.asmx").AbsoluteUri;
				DataWebServiceUrl = new Uri(new Uri(Ranet.AgOlap.Services.ServiceManager.BaseAddress), @"OlapWebService.asmx").AbsoluteUri;
			}
		}
		public static void CheckDataService(Action OnSuccess, Action OnError)
		{
			try
			{
				string wdUrl = _Default.DataWebServiceUrl;

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
