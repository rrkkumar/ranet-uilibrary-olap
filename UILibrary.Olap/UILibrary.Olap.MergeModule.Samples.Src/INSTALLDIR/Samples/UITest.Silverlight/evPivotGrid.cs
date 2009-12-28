using System;
using System.Text;
using System.Windows;

namespace UILibrary.Olap.UITestApplication
{
	public partial class Page : System.Windows.Controls.UserControl
	{
		void initPivotGridButton_Click(object sender, RoutedEventArgs e)
		{
			// pivotGridControl.URL = WSDataUrl;
			pivotGridControl.Connection = ConnectionStringId;
			pivotGridControl.Query = pivorGrid_query.Text;
			pivotGridControl.UpdateScript = pivorGrid_updateScript.Text;
			pivotGridControl.IsUpdateable = true;
			pivotGridControl.ColumnsIsInteractive = true;
			pivotGridControl.UseColumnsAreaHint = true;
			pivotGridControl.UseRowsAreaHint = true;
			pivotGridControl.UseCellsAreaHint = true;
			pivotGridControl.MemberVisualizationType = Ranet.Olap.Core.Data.MemberVisualizationTypes.KeyAndCaption;
			pivotGridControl.Initialize();
		}
	}
}
