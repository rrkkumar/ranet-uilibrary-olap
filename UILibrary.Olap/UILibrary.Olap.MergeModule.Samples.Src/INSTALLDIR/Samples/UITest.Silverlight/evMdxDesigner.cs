using System;
using System.Text;
using System.Windows;

namespace UILibrary.Olap.UITestApplication
{
	public partial class Page : System.Windows.Controls.UserControl
	{
		void initmdxDesignerButton_Click(object sender, RoutedEventArgs e)
		{
			//  pivotMdxDesignerControl.URL = WSDataUrl;
			pivotMdxDesignerControl.Connection = ConnectionStringId;
			pivotMdxDesignerControl.CubeName = mdxDesigner_cubeName.Text;
			pivotMdxDesignerControl.SubCube = mdxDesigner_subCube.Text;
			pivotMdxDesignerControl.AutoExecuteQuery = false;
			pivotMdxDesignerControl.Initialize();
		}
		string MdxLayout = String.Empty;
		void exportMdxLayoutButton_Click(object sender, RoutedEventArgs e)
		{
			MdxLayout = pivotMdxDesignerControl.ExportMdxLayoutInfo();
			MessageBox.Show(MdxLayout, "This is exported MdxLayout", MessageBoxButton.OK);
		}
		void importMdxLayoutButton_Click(object sender, RoutedEventArgs e)
		{
			pivotMdxDesignerControl.ImportMdxLayoutInfo(MdxLayout);
		}
	}
}
