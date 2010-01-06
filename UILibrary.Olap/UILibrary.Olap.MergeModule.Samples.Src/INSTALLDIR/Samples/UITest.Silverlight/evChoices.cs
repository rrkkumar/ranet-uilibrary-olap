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
	public partial class Page
	{
		void initCubeChoiceButton_Click(object sender, RoutedEventArgs e)
		{
			// cubeChoiceControl.URL = WSDataUrl;
			cubeChoiceControl.Connection = ConnectionStringId;
			cubeChoiceControl.Initialize();
		}
		void cubeChoiceControl_SelectedItemChanged(object sender, Ranet.AgOlap.Controls.General.ItemEventArgs e)
		{
			if (cubeChoiceControl.SelectedInfo==null)
				return;
			
			getKPIsButton_Click(null, null);
			getMeasuresButton_Click(null, null);
			initDimensionChoiceButton_Click(null, null);
		}
		void getKPIsButton_Click(object sender, RoutedEventArgs e)
		{
			if (cubeChoiceControl.SelectedInfo == null)
			{
				MessageBox.Show("Please select a Cube before initializing KPIChoice control.");				
				return;
			}
			// kpiChoiceControl.URL = WSDataUrl;
			kpiChoiceControl.AConnection = ConnectionStringId;
			kpiChoiceControl.ACubeName = cubeChoiceControl.SelectedInfo.Name;
		}
		void getMeasuresButton_Click(object sender, RoutedEventArgs e)
		{
			if (cubeChoiceControl.SelectedInfo == null)
			{
				MessageBox.Show("Please select a Cube before initializing MeasureChoice control.");
				return;
			}
			// measureChoiceControl.URL = WSDataUrl;
			measureChoiceControl.AConnection = ConnectionStringId;
			measureChoiceControl.ACubeName = cubeChoiceControl.SelectedInfo.Name;
		}
		void initDimensionChoiceButton_Click(object sender, RoutedEventArgs e)
		{
			if (cubeChoiceControl.SelectedInfo == null)
			{
				MessageBox.Show("Please select a Cube before initializing DimensionChoice control.");
				return;
			}

			// dimensionChoiceControl.URL = WSDataUrl;
			dimensionChoiceControl.Connection = ConnectionStringId;
			dimensionChoiceControl.CubeName = cubeChoiceControl.SelectedInfo.Name;
			dimensionChoiceControl.Initialize();
		}
		private void dimensionChoiceControl_SelectedItemChanged(object sender, Ranet.AgOlap.Controls.General.ItemEventArgs e)
		{
			if (cubeChoiceControl.SelectedInfo == null)
				return;
			if (dimensionChoiceControl.SelectedInfo == null)
				return;
				
			initHierarchyChoiceButton_Click(null, null);
		}
		void initHierarchyChoiceButton_Click(object sender, RoutedEventArgs e)
		{
			if (cubeChoiceControl.SelectedInfo == null)
			{
				MessageBox.Show("Please select a Cube before initializing HierarchyChoice control.");
				return;
			}
			if (dimensionChoiceControl.SelectedInfo == null)
			{
				MessageBox.Show("Please select a Dimension before initializing HierarchyChoice control.");
				return;
			}
			// hierarchyChoiceControl.URL = WSDataUrl;
			hierarchyChoiceControl.Connection = ConnectionStringId;
			hierarchyChoiceControl.CubeName = cubeChoiceControl.SelectedInfo.Name;
			hierarchyChoiceControl.DimensionUniqueName = dimensionChoiceControl.SelectedInfo.UniqueName;
			hierarchyChoiceControl.Initialize();
		}
		private void hierarchyChoiceControl_SelectedItemChanged(object sender, Ranet.AgOlap.Controls.General.ItemEventArgs e)
		{
			if (cubeChoiceControl.SelectedInfo == null)
				return;
			if (hierarchyChoiceControl.SelectedInfo == null)
				return;
				
			getMembersButton_Click(null, null);
		}
		private void getMembersButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			if (cubeChoiceControl.SelectedInfo == null)
			{
				MessageBox.Show("Please select a Cube before initializing MembersChoice control.");
				return;
			}
			if (hierarchyChoiceControl.SelectedInfo == null)
			{
				MessageBox.Show("Please select a Hierarchy before initializing MembersChoice control.");
				return;
			}

			// memberChoice.URL = WSDataUrl;
			memberChoice.Connection = ConnectionStringId;
			memberChoice.CubeName = cubeChoiceControl.SelectedInfo.Name;
			memberChoice.HierarchyUniqueName = hierarchyChoiceControl.SelectedInfo.UniqueName;
			memberChoice.UseStepLoading = true;
			memberChoice.Step = 5;
			memberChoice.MultiSelect = true;
			memberChoice.Initialize();
		}
	}
}
