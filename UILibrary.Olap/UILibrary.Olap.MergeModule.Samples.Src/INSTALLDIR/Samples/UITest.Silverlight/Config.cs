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
	public class Data
	{
		public string OlapWebServiceUrl { get; set; }
		public string OLAPConnectionString { get; set; }

		public string MdxQuery { get; set; }
		public string UpdateScript { get; set; }
		public string MdxDesignerLayout { get; set; }
		public int BackGroundColor { get; set; }

		//public string MinValue { get; set; }
		//public string LowValue { get; set; }
		//public string CurrentValue { get; set; }
		//public string HighValue { get; set; }
		//public string MaxValue { get; set; }
		//public string GaugeText { get; set; }
		//public string GaugeToolTipText { get; set; }

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
			BackGroundColor = 12;
			
			OLAPConnectionString = @"Data Source=.\sql2008;Initial Catalog=Adventure Works DW";
			// OLAPConnectionString = string.Empty;
			
			OlapWebServiceUrl = new Uri(new Uri(Ranet.AgOlap.Services.ServiceManager.BaseAddress), @"OlapWebService.asmx").AbsoluteUri;

			MdxQuery = @"select  [Product].[Product Categories].[Category]  DIMENSION PROPERTIES PARENT_UNIQUE_NAME,KEY0 on 0 ,[Date].[Calendar].[Calendar Year]   DIMENSION PROPERTIES PARENT_UNIQUE_NAME,KEY0 on 1 from [Adventure Works]
 where [Measures].[Sales Amount]
 CELL PROPERTIES BACK_COLOR, CELL_ORDINAL, FORE_COLOR, FONT_NAME, FONT_SIZE, FONT_FLAGS, FORMAT_STRING, VALUE, FORMATTED_VALUE, UPDATEABLE
";

			MdxDesignerLayout = @"<?xml version='1.0' encoding='utf-8'?>
<MdxLayoutWrapper xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
  <Filters />
  <Rows />
  <Columns />
  <Data>
    <AreaItemWrapper xsi:type='Measure_AreaItemWrapper'>
      <CustomProperties />
      <Caption>Internet Sales Amount</Caption>
      <UniqueName>[Measures].[Internet Sales Amount]</UniqueName>
    </AreaItemWrapper>
  </Data>
  <CalculatedMembers />
  <CalculatedNamedSets />
</MdxLayoutWrapper>
";
			UpdateScript = "--You need to enable WriteBack ability for your cube first.";
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
		public static void Init(string connectionStringId, string connectionString, Action OnSuccess, Action OnError)
		{
			var service = Ranet.AgOlap.Services.ServiceManager.CreateService
			 <Ranet.AgOlap.OlapWebService.OlapWebServiceSoapClient
			 , Ranet.AgOlap.OlapWebService.OlapWebServiceSoap
			 >(Default.Data.OlapWebServiceUrl);

			service.PerformOlapServiceActionCompleted += 
			(	object sender
			, Ranet.AgOlap.OlapWebService.PerformOlapServiceActionCompletedEventArgs e
			) =>
			{
				if (e.Error == null)
				{
					if (e.Result == null)
					{
						MessageBox.Show("Data service has returned 'null' as current ConnectionString", "Error", MessageBoxButton.OK);
						if (OnError != null)
							OnError();
					}
					else
					{
						Default.Data.OLAPConnectionString = e.Result;
						if (OnSuccess != null)
							OnSuccess();
					}
				}
				else
				{
					MessageBox.Show(e.Error.ToString(), "Error", MessageBoxButton.OK);
					if (OnError != null)
						OnError();
				}
			};
			service.PerformOlapServiceActionAsync("SetConnectionString", connectionStringId+"="+connectionString);
		}
	}
}
