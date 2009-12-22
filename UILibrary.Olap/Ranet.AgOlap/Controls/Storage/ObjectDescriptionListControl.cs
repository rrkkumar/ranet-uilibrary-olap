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
using Ranet.AgOlap.Controls.General.Tree;
using Ranet.AgOlap.Controls.General;
using Ranet.Olap.Core;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Ranet.AgOlap.Controls.Storage
{
    public class ObjectDescriptionListControl : UserControl
    {
        CustomTree m_Tree;
        ObjectDescriptionControl m_Description;
        Grid grdIsWaiting;

        public ObjectDescriptionListControl()
        {
            Grid LayoutRoot = new Grid();
            
            m_Tree = new CustomTree() { BorderBrush = new SolidColorBrush(Colors.DarkGray) };
            m_Tree.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(m_Tree_SelectedItemChanged);
            LayoutRoot.Children.Add(m_Tree);

            grdIsWaiting = new Grid() { Background = new SolidColorBrush(Color.FromArgb(125, 0xFF, 0xFF, 0xFF)) };
            grdIsWaiting.Visibility = Visibility.Collapsed;
            BusyControl m_Waiting = new BusyControl();
            m_Waiting.Text = Localization.Loading;
            grdIsWaiting.Children.Add(m_Waiting);
            LayoutRoot.Children.Add(grdIsWaiting);
            Grid.SetColumnSpan(grdIsWaiting, LayoutRoot.ColumnDefinitions.Count > 0 ? LayoutRoot.ColumnDefinitions.Count : 1);
            Grid.SetRowSpan(grdIsWaiting, LayoutRoot.RowDefinitions.Count > 0 ? LayoutRoot.RowDefinitions.Count : 1);

            this.Content = LayoutRoot;
        }

        public event EventHandler<CustomEventArgs<ObjectStorageFileDescription>> SelectionChanged;
        public event EventHandler<CustomEventArgs<ObjectStorageFileDescription>> ObjectSelected;

        void m_Tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeNode<ObjectStorageFileDescription> node = e.NewValue as TreeNode<ObjectStorageFileDescription>;
            ObjectStorageFileDescription descr = null;
            if (node != null)
                descr = node.Info;

            EventHandler<CustomEventArgs<ObjectStorageFileDescription>> handler = SelectionChanged;
            if (handler != null)
            {
                handler(this, new CustomEventArgs<ObjectStorageFileDescription>(descr));
            }
        }

        public ObjectStorageFileDescription CurrentObject
        {
            get
            {
                TreeNode<ObjectStorageFileDescription> node = m_Tree.SelectedItem as TreeNode<ObjectStorageFileDescription>;
                if(node != null)
                {
                    return node.Info;
                }
                return null;
            }
        }

        bool m_IsWaiting = false;
        public bool IsWaiting
        {
            get { return m_IsWaiting; }
            set
            {
                if (m_IsWaiting != value)
                {
                    if (value)
                    {
                        this.Cursor = Cursors.Wait;
                        grdIsWaiting.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        this.Cursor = Cursors.Arrow;
                        grdIsWaiting.Visibility = Visibility.Collapsed;
                    }
                    this.IsEnabled = !value;
                    m_IsWaiting = value;
                }
            }
        }

        List<ObjectStorageFileDescription> m_List = null;
        public void Initialize(List<ObjectStorageFileDescription> list)
        {
            m_List = list;
            m_Tree.Items.Clear();
            if (list != null)
            {
                foreach (ObjectStorageFileDescription descr in list)
                {
                    BitmapImage icon = UriResources.Images.FileExtension16;
                    if (String.IsNullOrEmpty(descr.ContentFileName))
                        icon = UriResources.Images.FileError16;
                    TreeNode<ObjectStorageFileDescription> node = new TreeNode<ObjectStorageFileDescription>(descr.Description.Caption, icon, descr);
                    node.MouseDoubleClick += new MouseDoubleClickEventHandler(node_MouseDoubleClick);
                    m_Tree.Items.Add(node);
                }
            }
        }

        void node_MouseDoubleClick(object sender, EventArgs e)
        {
            TreeNode<ObjectStorageFileDescription> node = sender as TreeNode<ObjectStorageFileDescription>;
            if (node != null && node.Info != null)
            {
                EventHandler<CustomEventArgs<ObjectStorageFileDescription>> handler = ObjectSelected;
                if (handler != null)
                {
                    handler(this, new CustomEventArgs<ObjectStorageFileDescription>(node.Info));
                }
            }
        }

        public bool Contains(String name)
        {
            if (m_List != null)
            {
                foreach (ObjectStorageFileDescription descr in m_List)
                {
                    if (descr.Description.Name == name)
                        return true;
                }
            }
            return false;
        }
    }

    public class CustomEventArgs<T> : EventArgs
    {
        public readonly T Args;

        public CustomEventArgs(T args)
        {
            Args = args;
        }
    }
}
