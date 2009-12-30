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
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace UILibrary.Olap.UITestApplication
{
	public partial class Page : System.Windows.Controls.UserControl
	{
		void BindData()
		{
			foreach (var pi in Config.Default.Data.GetType().GetProperties())
			{
				var myBinding = new Binding("Data."+pi.Name);
				myBinding.Source = Config.Default;
				myBinding.Mode = BindingMode.TwoWay;
				myBinding.UpdateSourceTrigger = UpdateSourceTrigger.Default;
				myBinding.BindsDirectlyToSource = true;
				myBinding.NotifyOnValidationError=true;
				var tb=this.FindName("tb"+pi.Name) as TextBox;
				if (tb==null)
					continue;
				tb.SetBinding(TextBox.TextProperty, myBinding);
			}

			//GaugeSetValues();
		}
		void SetDefaultValues_Click(object sender, RoutedEventArgs e)
		{
			Config.SetDefault();
		}
		void SaveCurrentValues_Click(object sender, RoutedEventArgs e)
		{
			Config.Save();
		}
		private void LoadLastSavedValues_Click(object sender, RoutedEventArgs e)
		{
			Config.Load();
		}
		private void CheckConnection_Click(object sender, RoutedEventArgs e)
		{
			CheckedInfo.Text = "Connection checking started...";
			Config.Init
			(ConnectionStringId, tbOLAPConnectionString.Text
			, () =>
			{
				try
				{
					CheckedInfo.Text = "Connection has been succesfully checked.";
					//System.Windows.Browser.HtmlPage.Window.SetProperty("status","Done!");
					//initCubeChoiceButton_Click(null, null);
					//initmdxDesignerButton_Click(null, null);
					//initPivotGridButton_Click(null, null);
					//initServerExplorerButton_Click(null, null);
				}
				catch (Exception E)
				{
					MessageBox.Show(E.ToString(), "Error", MessageBoxButton.OK);
				}
			}
			, () =>
			{
				CheckedInfo.Text = "There were errors while connection checking....";
			}
			);
		}
	}
}
