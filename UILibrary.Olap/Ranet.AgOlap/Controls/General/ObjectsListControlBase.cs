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
using Ranet.AgOlap.Controls.Storage;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Ranet.AgOlap.Controls.General
{
    public class CustomEventArgs<T> : EventArgs
    {
        public readonly T Args;

        public CustomEventArgs(T args)
        {
            Args = args;
        }
    }

    public class SelectionChangedEventArgs<T> : EventArgs
    {
        public readonly T OldValue;
        public readonly T NewValue;

        public SelectionChangedEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    public class ObjectsListControlBase<T> : UserControl
    {
        protected CustomTree m_Tree;
        Grid grdIsWaiting;

        public ObjectsListControlBase()
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

            this.Loaded += new RoutedEventHandler(ObjectsListControlBase_Loaded);
        }

        void ObjectsListControlBase_Loaded(object sender, RoutedEventArgs e)
        {
            if (m_Tree.Items.Count > 0)
            {
                TreeNode<T> node = m_Tree.Items[0] as TreeNode<T>;
                if (node != null)
                {
                    node.IsSelected = true;
                }
            }
        }

        public event EventHandler<SelectionChangedEventArgs<T>> SelectionChanged;
        public event EventHandler<CustomEventArgs<T>> ObjectSelected;

        void m_Tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeNode<T> node_old = e.OldValue as TreeNode<T>;
            T descr_old = default(T);
            if (node_old != null)
            {
                descr_old = node_old.Info;
            }

            TreeNode<T> node_new = e.NewValue as TreeNode<T>;
            T descr_new = default(T);
            if (node_new != null)
            {
                descr_new = node_new.Info;
            }
            else
            {
                // Иногда по неясным причинам в e.NewValue приходит {object} хотя при установке IsSelected = true узел был корректный. Поэтому в случае если узел определить не удалось, считаем что выбран нулевой узел
                if (m_Tree.Items.Count > 0)
                {
                    TreeNode<T> node0 = m_Tree.Items[0] as TreeNode<T>;
                    if (node0 != null)
                    {
                        descr_new = node0.Info; 
                    }
                }
            }

            EventHandler<SelectionChangedEventArgs<T>> handler = SelectionChanged;
            if (handler != null)
            {
                handler(this, new SelectionChangedEventArgs<T>(descr_old, descr_new));
            }

            Refresh();
        }

        public T CurrentObject
        {
            get
            {
                TreeNode<T> node = null;
                // m_Tree.SelectedItem иногда равно null. Несмотря на то что среди узлов есть такой, у которого IsSlected == true

                if (m_Tree.SelectedItem == null)
                {
                    // Сами попытаемся найти выбранный элемент.
                    foreach (object obj in m_Tree.Items)
                    {
                        TreeNode<T> x = obj as TreeNode<T>;
                        if (x != null && x.IsSelected)
                        {
                            node = x;
                            break;
                        }
                    }
                }
                else
                {
                    node = m_Tree.SelectedItem as TreeNode<T>;
                }

                if(node != null)
                {
                    return node.Info;
                }
                return default(T);
            }
            set
            {
                if (value != null)
                {
                    TreeViewItem selected = null;
                    foreach (object obj in m_Tree.Items)
                    {
                        TreeViewItem item = obj as TreeViewItem;
                        if (item != null)
                        {
                            item.IsSelected = false;
                            TreeNode<T> node = item as TreeNode<T>;
                            if (node != null && node.Info.Equals(value))
                            {
                                selected = item;
                            }
                        }
                    }
                    if (selected != null)
                        selected.IsSelected = true;
                }
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

        List<T> m_List = null;
        public List<T> List
        {
            get {
                if (m_List == null)
                {
                    m_List = new List<T>();
                }
                return m_List;
            }
        }

        public void Initialize(List<T> list)
        {
            m_List = list;
            m_Tree.Items.Clear();
            if (list != null)
            {
                foreach (T descr in list)
                {
                    AddItemNode(descr);
                }
            }
        }

        public virtual void Refresh()
        {
        }

        public void AddItem(T item)
        {
            if (item != null)
            {
                List.Add(item);
                AddItemNode(item);
                CurrentObject = item;
            }
        }

        public void DeleteItem(T item)
        {
            if (item != null)
            {
                if(List.Contains(item))
                    List.Remove(item);
                foreach (object obj in m_Tree.Items)
                {
                    TreeNode<T> node = obj as TreeNode<T>;
                    if (node != null && node.Info.Equals(item))
                    {
                        m_Tree.Items.Remove(node);
                        return;
                    }
                }
            }
        }

        void AddItemNode(T item)
        {
            if (item != null)
            {
                TreeNode<T> node = BuildTreeNode(item);
                if (node != null)
                {
                    node.MouseDoubleClick += new MouseDoubleClickEventHandler(node_MouseDoubleClick);
                    m_Tree.Items.Add(node);
                }
            }
        }

        public virtual TreeNode<T> BuildTreeNode(T item)
        {
            return new TreeNode<T>("Node", null, item);
        }

        void node_MouseDoubleClick(object sender, EventArgs e)
        {
            TreeNode<T> node = sender as TreeNode<T>;
            if (node != null && node.Info != null)
            {
                EventHandler<CustomEventArgs<T>> handler = ObjectSelected;
                if (handler != null)
                {
                    handler(this, new CustomEventArgs<T>(node.Info));
                }
            }
        }

        public virtual bool Contains(String name)
        {
            return false;
        }
    }
}
