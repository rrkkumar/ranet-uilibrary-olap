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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Ranet.AgOlap.Controls.MdxDesigner;
using Ranet.Olap.Core.Providers;
using Ranet.AgOlap.Controls.General.Tree;

namespace Ranet.AgOlap.Controls.General.DataGrid
{
    public class DragGridItemArgs<T> : EventArgs
    {
        public readonly KpiDataGrid Item;
        public readonly T Args;

        public DragGridItemArgs(KpiDataGrid item, T args)
        {
            Item = item;
            Args = args;
        }
    }

    public class KpiDataGrid : DragDropControl
    {
        private Grid LayoutRoot;
        private System.Windows.Controls.DataGrid m_Grid = new System.Windows.Controls.DataGrid();
        public KpiDataGrid()
        {
            LayoutRoot = new Grid();
            LayoutRoot.Children.Add(m_Grid);
            this.Content = LayoutRoot;
        }     

        bool m_IsReadyToDrop = false;
        public bool IsReadyToDrop
        {
            get { return m_IsReadyToDrop; }
            set
            {
                if (m_IsReadyToDrop != value)
                {
                    m_IsReadyToDrop = value;
                    if (value)
                    {
                        this.Effect = new System.Windows.Media.Effects.DropShadowEffect() { ShadowDepth = 1, Opacity = 0.8, Color = Colors.Blue };
                    }
                    else
                    {
                        this.Effect = null;
                    }
                }
            }
        }

        //public void AddRow(KpiView row)
        //{        
        //    if (row != null)
        //    {
        //        this.DragStarted += new EventHandler<DragGridItemArgs<DragStartedEventArgs>>(KpiDataGrid_DragStarted);
        //        this.DragDelta += new EventHandler<DragGridItemArgs<DragDeltaEventArgs>>(KpiDataGrid_DragDelta);
        //        this.DragCompleted += new EventHandler<DragGridItemArgs<DragCompletedEventArgs>>(KpiDataGrid_DragCompleted);               
        //    }
        //}    
    
        public void AddRow(KpiView row)
        {
            if (row != null)
            {
                this.DragStarted += new DragStartedEventHandler(KpiDataGrid_DragStarted);
                this.DragDelta += new DragDeltaEventHandler(KpiDataGrid_DragDelta);
                this.DragCompleted += new DragCompletedEventHandler(KpiDataGrid_DragCompleted);
            }
        }

        void KpiDataGrid_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            
        }

        void KpiDataGrid_DragDelta(object sender, DragDeltaEventArgs e)
        {
            
        }

        void KpiDataGrid_DragStarted(object sender, DragStartedEventArgs e)
        {
            
        }

        public System.Windows.Controls.DataGrid Grid
        {
            get { return this.m_Grid; }
        }
               
    }

}
