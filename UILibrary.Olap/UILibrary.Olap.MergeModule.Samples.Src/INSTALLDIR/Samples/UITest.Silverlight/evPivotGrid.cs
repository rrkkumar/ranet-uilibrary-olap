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
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Ranet.AgOlap.Controls.PivotGrid.Conditions;

namespace UILibrary.Olap.UITestApplication
{
	public partial class Page
	{
		void initPivotGridButton_Click(object sender, RoutedEventArgs e)
		{
			// pivotGridControl.URL = WSDataUrl;
			pivotGridControl.Connection = ConnectionStringId;
			pivotGridControl.Query = tbMdxQuery.Text;
			pivotGridControl.UpdateScript = tbUpdateScript.Text;
			pivotGridControl.MembersViewMode = (Ranet.AgOlap.Controls.ViewModeTypes)cbMembersViewMode.SelectedIndex;
			pivotGridControl.MemberVisualizationType = (Ranet.Olap.Core.Data.MemberVisualizationTypes)cbMemberVisualizationType.SelectedIndex;
			pivotGridControl.DataReorganizationType = (Ranet.Olap.Core.Providers.DataReorganizationTypes)cbDataReorganizationType.SelectedIndex;
			pivotGridControl.DefaultMemberAction = (Ranet.AgOlap.Controls.MemberClickBehaviorTypes)cbDefaultMemberAction.SelectedIndex;
			pivotGridControl.ColumnTitleClickBehavior = (Ranet.AgOlap.Controls.ColumnTitleClickBehavior)cbColumnTitleClickBehavior.SelectedIndex;
			pivotGridControl.DrillDownMode = (Ranet.AgOlap.Controls.DrillDownMode)cbDrillDownMode.SelectedIndex;
			pivotGridControl.IsUpdateable = ckbIsUpdateable.IsChecked.Value;
			pivotGridControl.AutoWidthColumns = ckbAutoWidthColumns.IsChecked.Value;
			pivotGridControl.ColumnsIsInteractive = ckbColumnsIsInteractive.IsChecked.Value;
			pivotGridControl.RowsIsInteractive = ckbRowsIsInteractive.IsChecked.Value;
			pivotGridControl.UseColumnsAreaHint = ckbUseColumnsAreaHint.IsChecked.Value;
			pivotGridControl.UseRowsAreaHint = ckbUseRowsAreaHint.IsChecked.Value;
			pivotGridControl.UseCellsAreaHint = ckbUseCellsAreaHint.IsChecked.Value;
			pivotGridControl.UseCellConditionsDesigner = ckbUseCellConditionsDesigner.IsChecked.Value;
			if (!pivotGridControl.UseCellConditionsDesigner)
			{
				var conds = new CellConditionsDescriptor("[Measures].[Internet Sales Amount]");
				var cellApp = new CellAppearanceObject(Colors.Cyan, Colors.Black, Colors.Black);
				cellApp.Options.UseBackColor = true;
				// cellApp.Options.UseBorderColor=true;
				var cond = new CellCondition(CellConditionType.GreaterOrEqual, 1000000.0, 1000000.0, cellApp);
				conds.Conditions.Add(cond);
				pivotGridControl.CustomCellsConditions = new List<CellConditionsDescriptor>();
				pivotGridControl.CustomCellsConditions.Add(conds);
			}
			pivotGridControl.Initialize();
		}
	}
}
