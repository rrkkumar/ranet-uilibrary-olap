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
using Ranet.AgOlap.Controls.PivotGrid.Data;
using Ranet.Olap.Core.Data;
using Ranet.Olap.Core.Providers;

namespace Ranet.AgOlap.Controls.General
{
    public class PropertyItem
    {
        public String Property { get; set; }
        public String Value { get; set; }

        public PropertyItem()
        {
        }

        public PropertyItem(String property, String value)
        {
            Property = property;
            Value = value;
        }
    }

    public partial class PropertiesListControl : UserControl
    {
        DataGridTextColumn propertyColumn = null;
        DataGridTextColumn valueColumn = null;
        public PropertiesListControl()
        {
            InitializeComponent();

            grid.AutoGenerateColumns = false;
            grid.IsReadOnly = true;
            grid.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            grid.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;

            propertyColumn = new DataGridTextColumn();
            propertyColumn.Header = Localization.PropertiesList_PropertyColumn;
            propertyColumn.Binding = new System.Windows.Data.Binding("Property");
            propertyColumn.CanUserResize = true;
            propertyColumn.Width = new DataGridLength(200);
            grid.Columns.Add(propertyColumn);

            valueColumn = new DataGridTextColumn();
            valueColumn.Header = Localization.PropertiesList_ValueColumn;
            valueColumn.Binding = new System.Windows.Data.Binding("Value");
            valueColumn.CanUserResize = true;
            valueColumn.Width = new DataGridLength(200);
            grid.Columns.Add(valueColumn);

            //grid.ItemsSource = "H e l l o W o r l d !".Split();
            Clear();
            grid.RowHeight = 22;
        }

        public DataGridLength PropertyColumnWidth
        {
            get { return propertyColumn.Width; }
            set { propertyColumn.Width = value; }
        }

        public DataGridLength ValueColumnWidth
        {
            get { return valueColumn.Width; }
            set { valueColumn.Width = value; }
        }

        public void Clear()
        {
            List<PropertyItem> list = new List<PropertyItem>();
            grid.ItemsSource = list;
        }

        public void Initialize(IProperties propertiesOwner)
        {
            MemberInfo mi = propertiesOwner as MemberInfo;
            List<PropertyItem> list = new List<PropertyItem>();
            if (propertiesOwner != null && propertiesOwner.PropertiesDictionary != null)
            {
                foreach (KeyValuePair<String, object> pair in propertiesOwner.PropertiesDictionary)
                {
                    PropertyItem item = new PropertyItem();
                    item.Property = pair.Key;
                    if (item.Property == "DrilledDown" && mi != null)
                    {
                        // Элементы на оси объединяются если идут подряд одинаковые. При этом значение данного свойства формируется по ИЛИ
                        item.Value = mi.DrilledDown.ToString();
                    }
                    else
                    {
                        if (pair.Value != null)
                        {
                            item.Value = pair.Value.ToString();
                        }
                        else
                        {
                            item.Value = String.Empty;
                        }
                    }
                    list.Add(item);
                }
            }
            grid.ItemsSource = list;
        }

        public void Initialize(IPropertiesData propertiesOwner)
        {
            List<PropertyItem> list = new List<PropertyItem>();
            if (propertiesOwner != null)
            {
                foreach (PropertyData pair in propertiesOwner.Properties)
                {
                    PropertyItem item = new PropertyItem();
                    item.Property = pair.Name;
                    if (pair.Value != null)
                    {
                        item.Value = pair.Value.ToString();
                    }
                    else
                    {
                        item.Value = String.Empty;
                    }
                    list.Add(item);
                }
            }
            grid.ItemsSource = list;
        }

        public void Initialize(MemberData propertiesOwner)
        {
            List<PropertyItem> list = new List<PropertyItem>();
            if (propertiesOwner != null)
            {
                foreach (PropertyData pair in propertiesOwner.MemberProperties)
                {
                    PropertyItem item = new PropertyItem();

                    String caption = pair.Name;
                    if (caption.StartsWith("-") && caption.EndsWith("-"))
                        caption = caption.Trim('-');
                    item.Property = caption;

                    if (pair.Value != null)
                    {
                        item.Value = pair.Value.ToString();
                    }
                    else
                    {
                        item.Value = String.Empty;
                    }
                    list.Add(item);
                }
            }
            grid.ItemsSource = list;
        }
    }
}
