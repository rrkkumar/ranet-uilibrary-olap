using System;
using System.Text;
using System.Windows;

namespace UILibrary.Olap.UITestApplication
{
	public partial class Page : System.Windows.Controls.UserControl
	{
		void initServerExplorerButton_Click(object sender, RoutedEventArgs e)
		{
			// serverExplorerControl.URL = WSDataUrl;
			serverExplorerControl.Connection = ConnectionStringId;
			serverExplorerControl.ShowAllCubes = false;
			if (AllCubes.IsChecked!=null)
				serverExplorerControl.ShowAllCubes = AllCubes.IsChecked.Value;
			serverExplorerControl.Initialize();
		}
	}
}
