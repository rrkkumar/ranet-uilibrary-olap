/*   
    Copyright (C) 2009 Galaktika Corporation ZAO

    This file is a part of Ranet.UILibrary.Olap
 
    Ranet.UILibrary.Olap is a free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
      
    You should have received a copy of the GNU General Public License
    along with Ranet.UILibrary.Olap.  If not, see
  	<http://www.gnu.org/licenses/> 
  
    If GPL v.3 is not suitable for your products or company,
    Galaktika Corp provides Ranet.UILibrary.Olap under a flexible commercial license
    designed to meet your specific usage and distribution requirements.
    If you have already obtained a commercial license from Galaktika Corp,
    you can use this file under those license terms.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Ranet.AgOlap.Controls.ToolBar;
using Ranet.AgOlap.Controls.General;

namespace Ranet.AgOlap.Controls.PivotGrid.Conditions
{
    public partial class CustomCellConditionsEditor : UserControl
    {
        public CustomCellConditionsEditor()
        {
            InitializeComponent();
            DescriptorsList.SelectionChanged += new EventHandler<SelectionChangedEventArgs<CellConditionsDescriptor>>(DescriptorsList_SelectionChanged);
            DescriptorDetails.EditEnd += new EventHandler(DescriptorDetails_EditEnd);
        }

        void DescriptorDetails_EditEnd(object sender, EventArgs e)
        {
            DescriptorsList.Refresh();
        }

        void DescriptorsList_SelectionChanged(object sender, SelectionChangedEventArgs<CellConditionsDescriptor> e)
        {
            EndEdit();
            DescriptorDetails.Initialize(e.NewValue);
        }

        public void EndEdit()
        {
            if (DescriptorDetails.IsEnabled && DescriptorDetails.Descriptor != null)
            {
                DescriptorDetails.EndEdit();
            }

            DescriptorsList.Refresh();
        }

        public void Initialize(List<CellConditionsDescriptor> cellsConditions)
        {
            DescriptorsList.Initialize(cellsConditions);
            if (cellsConditions == null || cellsConditions.Count == 0)
            {
                DescriptorDetails.Initialize(null);
            }
        }

        public List<CellConditionsDescriptor> CellsConditions
        {
            get { return DescriptorsList.List; }
        }
    }
}
