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
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using Ranet.Olap.Core.Providers;

namespace Ranet.AgOlap.Controls.ValueDelivery
{
    public class TupleItemArgs : EventArgs
    {
        public readonly TupleItem Item = null;
        public TupleItemArgs(TupleItem item)
        {
            Item = item;
        }
    }

    public class TupleItem
    {
        public String Hierarchy { get; set; }
        public String Caption { get; set; }

        public TupleItem()
        {
        }

        public readonly MemberInfo Info = null;

        public TupleItem(MemberInfo member)
        {
            if (member == null)
                throw new ArgumentNullException("member");
            Info = member;
            Hierarchy = member.HierarchyUniqueName;
            Caption = member.Caption;
        }
    }


    public class CellTupleControl : UserControl
    {
        CellInfo m_Cell = null;
        DataGrid m_Grid;

        public CellTupleControl()
        {
            Grid LayoutRoot = new Grid();
            m_Grid = new DataGrid();
            m_Grid.AutoGenerateColumns = false;
            m_Grid.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            m_Grid.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            m_Grid.RowHeight = 22;
            m_Grid.IsReadOnly = true;
            m_Grid.SelectionChanged += new SelectionChangedEventHandler(m_Grid_SelectionChanged);
            LayoutRoot.Children.Add(m_Grid);

            DataGridTextColumn hierarchyColumn = new DataGridTextColumn();
            hierarchyColumn.Header = Localization.ValueDeliveryControl_Hierarchy;
            hierarchyColumn.Binding= new System.Windows.Data.Binding("Hierarchy");
            m_Grid.Columns.Add(hierarchyColumn);

            DataGridTextColumn memberColumn = new DataGridTextColumn();
            memberColumn.Header = Localization.ValueDeliveryControl_Member;
            memberColumn.Binding = new System.Windows.Data.Binding("Caption");
            m_Grid.Columns.Add(memberColumn);

            this.Content = LayoutRoot;
        }

        public event EventHandler<TupleItemArgs> SelectedItemChanged;
        void Raise_SelectedItemChanged(TupleItemArgs args)
        {
            EventHandler<TupleItemArgs> handler = SelectedItemChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        void m_Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Raise_SelectedItemChanged(new TupleItemArgs(m_Grid.SelectedItem as TupleItem));
        }

        public void Initialize(CellInfo cell)
        {
            m_Cell = cell;

            List<TupleItem> list = new List<TupleItem>();
            IDictionary<String, MemberInfo> tuple = m_Cell.GetTuple();
            foreach (MemberInfo member in tuple.Values)
            {
                list.Add(new TupleItem(member));
            }
            m_Grid.ItemsSource = list;
            if (list.Count > 0)
                m_Grid.SelectedIndex = 0;
            else
                m_Grid.SelectedIndex = -1;
        }

    }
}
