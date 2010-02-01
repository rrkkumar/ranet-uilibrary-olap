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
using System.Text;
using System.Windows;

namespace UILibrary.Olap.UITestApplication
{
	public class MyDesigner : Ranet.AgOlap.Controls.PivotMdxDesignerControl
	{
		protected override void InitializePivotGrid(string query)
		{
			base.InitializePivotGrid(query);
			//PivotGrid.DefaultMemberAction = Ranet.AgOlap.Controls.MemberClickBehaviorTypes.ExpandCollapse;
		}
		public string GetCurrentMdxQuery()
		{
			return this.PivotGrid.DataManager.GetDataSourceInfo(null).MDXQuery;
		}
	}

	public partial class Page
	{
		void initmdxDesignerButton_Click(object sender, RoutedEventArgs e)
		{
			// NON required property
			// by default URL= <BackToApplicationClientBin>\..\OlapWebService.asmx
			// pivotMdxDesignerControl.URL = WSDataUrl;

			this.pivotMdxDesignerControl.Connection = ConnectionStringId;
			this.pivotMdxDesignerControl.CanSelectCube = true;
			this.pivotMdxDesignerControl.AutoExecuteQuery = false;
			this.pivotMdxDesignerControl.UpdateScript = tbUpdateScript.Text;

			this.pivotMdxDesignerControl.Initialize();
		}
		void exportMdxLayoutButton_Click(object sender, RoutedEventArgs e)
		{
			tbMdxDesignerLayout.Text = pivotMdxDesignerControl.ExportMdxLayoutInfo();
			MessageBox.Show("Mdx Designer Layout was exported. See Mdx Designer Layout tab.", "Information", MessageBoxButton.OK);
		}
		void importMdxLayoutButton_Click(object sender, RoutedEventArgs e)
		{
			pivotMdxDesignerControl.ImportMdxLayoutInfo(tbMdxDesignerLayout.Text);
		}
		private void exportMDXQueryButton_Click(object sender, RoutedEventArgs e)
		{
			tbMdxQuery.Text = pivotMdxDesignerControl.GetCurrentMdxQuery();
			MessageBox.Show("Mdx query was exported. See Mdx Query tab.", "Information", MessageBoxButton.OK);
		}
	}
}
