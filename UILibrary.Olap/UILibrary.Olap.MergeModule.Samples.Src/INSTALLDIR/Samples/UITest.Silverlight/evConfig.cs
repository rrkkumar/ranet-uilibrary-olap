using System;
using System.Text;
using System.Windows;

namespace UILibrary.Olap.UITestApplication
{
	public partial class Page : System.Windows.Controls.UserControl
	{
		void SetDefaultValues_Click(object sender, RoutedEventArgs e)
		{
			Config._Default.SetDefault();
			Config.Save();
			SetDefault();
		}
		private void CheckConnection_Click(object sender, RoutedEventArgs e)
		{
			//Config._Default.HostDirectory = HostDirectory.Text;
			CheckedInfo.Text = "Connection checking started...";
			Config._Default.InitializeWebServiceUrl = InitializeWebServiceUrl.Text;
			Config._Default.DataWebServiceUrl = DataServiceUrl.Text;
			Config._Default.ConectionStringOLAP = OLAPConnectionString.Text;
			Config.Init
			(ConnectionStringId, OLAPConnectionString.Text
			, () =>
			{
				try
				{

					Config.Save();
					CheckedInfo.Text = "Connection has been succesfully checked.";
					System.Windows.Browser.HtmlPage.Window.SetProperty("status","Done!");
					//Application.Current.Host.Content.NavigationState="Done!";
					//MessageBox.Show(CheckedInfo.Text, "OK", MessageBoxButton.OK);
					//initCubeChoiceButton_Click(null, null);
					//initPivotGridButton_Click(null, null);
					//initmdxDesignerButton_Click(null, null);
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
