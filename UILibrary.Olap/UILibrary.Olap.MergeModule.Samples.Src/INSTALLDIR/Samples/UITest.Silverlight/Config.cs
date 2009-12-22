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
			public string HostDirectory;
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
				HostDirectory = new Uri(Application.Current.Host.Source, @"../").AbsoluteUri;
				InitializeWebServiceUrl = new Uri(Application.Current.Host.Source, @"../InitializeWebService.asmx").AbsoluteUri;
				DataWebServiceUrl = new Uri(Application.Current.Host.Source, @"../OlapWebService.asmx").AbsoluteUri;
			}
		}
		public static void CheckDataService(Action OnSuccess)
		{
			try
			{
				string wdUrl = _Default.DataWebServiceUrl;

				var wds = new Ranet.AgOlap.OlapWebService.OlapWebServiceSoapClient
				 ("OlapWebServiceSoap"
				 , new EndpointAddress(wdUrl)
				 );
				wds.PerformOlapServiceActionCompleted += (object sender, Ranet.AgOlap.OlapWebService.PerformOlapServiceActionCompletedEventArgs e) =>
				{
					if (e.Error == null)
					{
						if (e.Result=="OK")
						{
							if(OnSuccess!=null)
								OnSuccess();
								
						}
						else if (e.Result == null)
							MessageBox.Show("Data service has returned 'null'", "Error", MessageBoxButton.OK);
						else
							MessageBox.Show(e.Result, "Error", MessageBoxButton.OK);
					}
					else
						MessageBox.Show(e.Error.ToString(), "Error", MessageBoxButton.OK);
				};
				wds.PerformOlapServiceActionAsync("CheckExist","");
			}
			catch (Exception E)
			{
				MessageBox.Show(E.ToString(), "Error", MessageBoxButton.OK);
			}
		}
		public static void Init(string connectionStringId, string connectionString, Action OnSuccess)
		{
			var service =
			 new InitializeWebServiceSoapClient
			 ("InitializeWebServiceSoap"
			 , new EndpointAddress(_Default.InitializeWebServiceUrl)
			 );
			service.InitConnectionStringCompleted += (object sender, Init.InitConnectionStringCompletedEventArgs e) =>
			{
				if (e.Error == null)
				{
					if (e.Result == null)
						CheckDataService(OnSuccess);
					else
						MessageBox.Show(e.Result, "Error", MessageBoxButton.OK);
				}
				else
					MessageBox.Show(e.Error.ToString(), "Error", MessageBoxButton.OK);
			};
			service.InitConnectionStringAsync(connectionStringId, connectionString);
		}
	}
}
