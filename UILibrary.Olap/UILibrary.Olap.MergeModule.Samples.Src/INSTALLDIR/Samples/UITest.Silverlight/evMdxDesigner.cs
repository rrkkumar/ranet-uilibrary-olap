using System;
using System.Text;
using System.Windows;

namespace UILibrary.Olap.UITestApplication
{
	public partial class Page : System.Windows.Controls.UserControl
	{
		void initmdxDesignerButton_Click(object sender, RoutedEventArgs e)
		{
			// NON required property
			// by default URL= <BackToApplicationClientBin>\..\InitializeWebService.asmx
			// pivotMdxDesignerControl.URL = WSDataUrl;

			pivotMdxDesignerControl.Connection = ConnectionStringId;
			pivotMdxDesignerControl.CanSelectCube = true;
			pivotMdxDesignerControl.AutoExecuteQuery = false;
			pivotMdxDesignerControl.Initialize();
		}
		string MdxLayout = String.Empty;
		void exportMdxLayoutButton_Click(object sender, RoutedEventArgs e)
		{
			mdxDesignerLayout.Text = pivotMdxDesignerControl.ExportMdxLayoutInfo();
			MessageBox.Show("Mdx Designer Layout was exported. See Mdx Designer Layout tab.", "Information", MessageBoxButton.OK);
		}
		void importMdxLayoutButton_Click(object sender, RoutedEventArgs e)
		{
			pivotMdxDesignerControl.ImportMdxLayoutInfo(mdxDesignerLayout.Text);
		}
		private void exportMDXQueryButton_Click(object sender, RoutedEventArgs e)
		{
			pivorGrid_query.Text=pivotMdxDesignerControl.MdxQuery;
			MessageBox.Show("Mdx query was exported. See Mdx Query tab.", "Information", MessageBoxButton.OK);
		}
	}
}
