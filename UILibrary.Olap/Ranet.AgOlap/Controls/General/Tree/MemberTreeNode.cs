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
using System.Windows.Controls;
using Ranet.AgOlap.Controls.MemberChoice.Info;
using Ranet.AgOlap.Controls.General.Tree;
using Ranet.Olap.Core.Data;

namespace Ranet.AgOlap.Controls.General.Tree
{
    public class MemberTreeNode : TreeViewItem
    {
        public const String KEY0_PROPERTY = "KEY0";

        OlapMemberInfo m_MemberInfo = null;
        public OlapMemberInfo MemberInfo
        {
            get {
                return m_MemberInfo;
            }
        }

        TreeItemControl item_ctrl;

        public MemberTreeNode(OlapMemberInfo info, bool useMultiSelect)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            m_MemberInfo = info;

            item_ctrl = new TreeItemControl(useMultiSelect);

            if (info.Info != null && info.Info.Member != null)
                item_ctrl.Text = MemberInfo.Info.Member.Caption;
            else
                item_ctrl.Text = String.Empty;

            // В случае множ. выбора клик на иконке используем для изменения состояния
            item_ctrl.IconClick += new EventHandler(item_ctrl_IconClick);
            item_ctrl.MouseDoubleClick += new MouseDoubleClickEventHandler(item_ctrl_MouseDoubleClick);
            Header = item_ctrl;

            if (useMultiSelect)
            {
                UpdateNodeIcon();
            }

            info.StateChanged += new OlapMemberInfo.StateChangedEventHandler(info_StateChanged);
        }

        MemberVisualizationTypes m_MemberVisualizationType = MemberVisualizationTypes.Caption;
        public MemberVisualizationTypes MemberVisualizationType
        {
            get { return m_MemberVisualizationType; }
            set
            {
                m_MemberVisualizationType = value;
                // Определяем что именно нужно светить в контроле
                if (MemberInfo.Info != null && MemberInfo.Info.Member != null)
                    item_ctrl.Text = MemberInfo.Info.Member.GetText(m_MemberVisualizationType);
                else
                    item_ctrl.Text = String.Empty;
            }
        }

        public event MouseDoubleClickEventHandler MouseDoubleClick;
        void Raise_MouseDoubleClick(EventArgs e)
        {
            MouseDoubleClickEventHandler handler = MouseDoubleClick;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        void item_ctrl_MouseDoubleClick(object sender, EventArgs e)
        {
            Raise_MouseDoubleClick(e);
        }

        void info_StateChanged(OlapMemberInfo sender)
        {
            UpdateNodeIcon();
        }

        void item_ctrl_IconClick(object sender, EventArgs e)
        {
            //Переводим в следующее состояние
            MemberInfo.SetNextState();

            //Генерим событие "Изменилось стоятояние выбранности"
            //Raise_SelectionStateChanged();
        }

        private void UpdateNodeIcon()
        {
            TreeItemControl item_ctrl = Header as TreeItemControl;
            if (item_ctrl != null)
            {
                item_ctrl.Icon = MemberChoiceControl.GetIconImage(MemberInfo);
            }
        }

        public bool IsWaiting
        {
            set
            {
                TreeViewItem waitItem = GetSpecialNode(SpecialNodes.Wait);
                if (!value)
                {
                    if (waitItem != null)
                    {
                        
                        Items.Remove(waitItem);
                    }
                }
                else
                {
                    if (waitItem == null)
                        AddSpecialNode(SpecialNodes.Wait);
                }
            }
            get
            {
                if (GetSpecialNode(SpecialNodes.Wait) != null)
                    return true;
                else
                    return false;
            }
        }

        //public bool IsReloadAll
        //{
        //    set
        //    {
        //        TreeViewItem nextItem = GetSpecialNode(SpecialNodes.ReloadAll);
        //        if (!value)
        //        {
        //            if (nextItem != null)
        //            {
        //                nextItem.Expanded -= new RoutedEventHandler(node_Expanded);
        //                Items.Remove(nextItem);
        //            }
        //        }
        //        else
        //        {
        //            if (nextItem == null)
        //                AddSpecialNode(SpecialNodes.ReloadAll);
        //        }
        //    }
        //    get
        //    {
        //        if (GetSpecialNode(SpecialNodes.ReloadAll) == null)
        //            return false;
        //        else
        //            return true;
        //    }
        //}

        public bool IsFullLoaded
        {
            set
            {
                TreeViewItem nextItem = GetSpecialNode(SpecialNodes.LoadNext);
                //TreeViewItem allItem = GetSpecialNode(SpecialNodes.LoadAll);
                if (value)
                {
                    if (nextItem != null)
                    {
                        LoadNextTreeNode next = nextItem as LoadNextTreeNode;
                        if (next != null)
                        {
                            next.MouseDoubleClick -= new MouseDoubleClickEventHandler(SpecialNode_MouseDoubleClick);
                        }
                        Items.Remove(nextItem);
                    }
                    //if (allItem != null)
                    //{
                    //    allItem.Expanded -= new RoutedEventHandler(node_Expanded);
                    //    Items.Remove(allItem);
                    //}
                }
                else
                {
                    if (nextItem == null)
                        AddSpecialNode(SpecialNodes.LoadNext);
                    //if (allItem == null)
                    //    AddSpecialNode(SpecialNodes.LoadAll);
                }
            }
            get
            {
                if (GetSpecialNode(SpecialNodes.LoadNext) == null/* &&
                    GetSpecialNode(SpecialNodes.LoadAll) == null*/)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Проверяет загружались ли дочерние узлы. 
        /// </summary>
        public bool IsInitialized
        {
            get
            {
                //Если у элемента один дочерний и он "WaitNode", то значит данные не грузились
                if (Items.Count == 1 && IsWaiting)
                {
                    return false;
                }
                return true;
            }
        }

        private enum SpecialNodes
        { 
            Wait,
            LoadNext/*,
            LoadAll,
            ReloadAll*/
        }

        public bool IsPreloaded
        {
            set {
                if (value)
                {
                    item_ctrl.ItemText.FontStyle = FontStyles.Italic;
                    item_ctrl.ItemText.Foreground = new SolidColorBrush(Colors.DarkGray);
                }
                else
                {
                    item_ctrl.ItemText.FontStyle = FontStyles.Normal;
                    item_ctrl.ItemText.Foreground = new SolidColorBrush(Colors.Black);
                }
            }
            get {
                if (item_ctrl.ItemText.FontStyle != FontStyles.Normal)
                    return true;
                return false;
            }
        }

        private TreeViewItem AddSpecialNode(SpecialNodes nodeType)
        {
            TreeViewItem new_node = null;
            switch (nodeType)
            { 
                case SpecialNodes.Wait:
                    new_node = new WaitTreeNode();
                    break;
                case SpecialNodes.LoadNext:
                    LoadNextTreeNode node = new LoadNextTreeNode();
                    node.MouseDoubleClick += new MouseDoubleClickEventHandler(SpecialNode_MouseDoubleClick);
                    new_node = node;
                    break;
                //case SpecialNodes.LoadAll:
                //    node = new LoadAllTreeNode();
                //    node.Expanded += new RoutedEventHandler(node_Expanded);
                //    break;
                //case SpecialNodes.ReloadAll:
                //    node = new ReloadAllTreeNode();
                //    node.Expanded += new RoutedEventHandler(node_Expanded);
                //    break;
            }
            if (new_node != null)
            {
                Items.Add(new_node);
            }
            return new_node;
        }

        void SpecialNode_MouseDoubleClick(object sender, EventArgs e)
        {
            MouseDoubleClickEventHandler handler = this.Special_MouseDoubleClick;
            if (handler != null)
            {
                handler(sender, EventArgs.Empty);
            }
        }

        public event MouseDoubleClickEventHandler Special_MouseDoubleClick;

        TreeViewItem GetSpecialNode(SpecialNodes nodeType)
        {
            foreach (object obj in Items)
            {
                switch (nodeType)
                {
                    case SpecialNodes.Wait:
                        WaitTreeNode wait = obj as WaitTreeNode;
                        if (wait != null)
                            return wait;
                        break;
                    case SpecialNodes.LoadNext:
                        LoadNextTreeNode next = obj as LoadNextTreeNode;
                        if (next != null)
                            return next;
                        break;
                    //case SpecialNodes.LoadAll:
                    //    LoadAllTreeNode all = obj as LoadAllTreeNode;
                    //    if (all != null)
                    //        return all;
                    //    break;
                    //case SpecialNodes.ReloadAll:
                    //    ReloadAllTreeNode reload = obj as ReloadAllTreeNode;
                    //    if (reload != null)
                    //        return reload;
                    //    break;
                }
            }
            return null;
        }


    }
}
