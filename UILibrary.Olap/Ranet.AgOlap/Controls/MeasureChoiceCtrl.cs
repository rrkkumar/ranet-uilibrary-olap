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
using Ranet.AgOlap.Controls.General.ClientServer;
using Ranet.AgOlap.Commands;
using Ranet.Olap.Core;
using Ranet.Olap.Core.Metadata;
using System.Collections.Generic;
using System.Text;

namespace Ranet.AgOlap.Controls
{
    public class MeasureChoiceCtrl : AgTreeControlBase, IChoiceControl
    {
        CustomTree Tree = null;

        public MeasureChoiceCtrl()
        {
            Tree = new CustomTree();

            Grid LayoutRoot = new Grid();
            base.Content = LayoutRoot;
            LayoutRoot.Children.Add(Tree);
            Tree.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(Tree_SelectedItemChanged);
        }

        void Tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            InfoBase info = null;
            MeasureTreeNode node = e.NewValue as MeasureTreeNode;
            if (node != null)
            {
                info = node.Info;
            }

            m_IsReadyToSelection = node != null;

            Raise_SelectedItemChanged(info);
        }

        bool m_IsReadyToSelection = false;
        public bool IsReadyToSelection
        {
            get
            {
                return m_IsReadyToSelection;
            }
        }

        #region Свойства для настройки на OLAP
        /// <summary>
        /// 
        /// </summary>
        private String m_Connection = String.Empty;
        /// <summary>
        /// Описание соединения с БД для идентификации соединения на сервере (строка соединения либо ID)
        /// </summary>
        public String Connection
        {
            get
            {
                return m_Connection;
            }
            set 
            {
                m_Connection = value;
            }
        }

        /// <summary>
        /// Имя OLAP куба
        /// </summary>
        String m_CubeName = String.Empty;
        /// <summary>
        /// Имя OLAP куба
        /// </summary>
        public String CubeName
        {
            get
            {
                return this.m_CubeName;
            }
            set
            {
                m_CubeName = value;
            }
        }
        #endregion Свойства для настройки на OLAP

        public MeasureInfo SelectedInfo
        {
            get {
                MeasureTreeNode node = null;
                node = Tree.SelectedItem as MeasureTreeNode;

                if (node != null)
                {
                    //Запоминаем выбранный элемент
                    return node.Info as MeasureInfo;
                }

                return null;
            }
        }

        public void Initialize()
        {
            RefreshTree();
        }

        private void RefreshTree()
        {
            Tree.Items.Clear();
            m_GroupNodes.Clear();

            // Добавляем узел Measures
            FolderTreeNode measuresNode = new FolderTreeNode();
            measuresNode.Text = "Measures";
            Tree.Items.Add(measuresNode);
            measuresNode.ExpandedIcon = UriResources.Images.Measure16;
            measuresNode.CollapsedIcon = UriResources.Images.Measure16;

            measuresNode.IsExpanded = true;

            if (String.IsNullOrEmpty(Connection) ||
                String.IsNullOrEmpty(CubeName))
            {
                // Сообщение в лог
                StringBuilder builder = new StringBuilder();
                if (String.IsNullOrEmpty(Connection))
                    builder.Append(Localization.Connection_PropertyDesc + ", ");
                if (String.IsNullOrEmpty(CubeName))
                    builder.Append(Localization.CubeName_PropertyDesc);
                LogManager.LogMessage(Localization.MeasureChoiceControl_Name, Localization.Error + "! " + String.Format(Localization.ControlSettingsNotInitialized_Message, builder.ToString()));

                measuresNode.IsError = true;
                return;
            }

            measuresNode.IsWaiting = true;
            MetadataQuery args = CommandHelper.CreateGetMeasuresQueryArgs(Connection, CubeName);
            Loader.LoadData(XmlSerializationUtility.Obj2XmlStr(args, Common.Namespace), measuresNode);
        }

        IDataLoader m_Loader = null;
        public IDataLoader Loader
        {
            set
            {
                if (m_Loader != null)
                {
                    m_Loader.DataLoaded -= new EventHandler<DataLoaderEventArgs>(Loader_DataLoaded);
                }
                m_Loader = value;
                m_Loader.DataLoaded += new EventHandler<DataLoaderEventArgs>(Loader_DataLoaded);
            }
            get
            {
                if (m_Loader == null)
                {
                    m_Loader = new MetadataLoader(URL);
                    m_Loader.DataLoaded += new EventHandler<DataLoaderEventArgs>(Loader_DataLoaded);
                }
                return m_Loader;
            }
        }

        void ShowErrorInTree(CustomTreeNode parentNode)
        {
            if (parentNode != null)
            {
                parentNode.IsError = true;
            }
            else
            {
                Tree.IsError = true;
            }
        }

        void Loader_DataLoaded(object sender, DataLoaderEventArgs e)
        {
            CustomTreeNode parentNode = e.UserState as CustomTreeNode;
            if (parentNode != null)
            {
                parentNode.IsWaiting = false;
            }
            else
            {
                Tree.IsWaiting = false;
            }

            if (e.Error != null)
            {
                ShowErrorInTree(parentNode);
                LogManager.LogException(Localization.MeasureChoiceControl_Name, e.Error);
                return;
            }

            if (e.Result.ContentType == InvokeContentType.Error)
            {
                ShowErrorInTree(parentNode);
                LogManager.LogMessage(Localization.MeasureChoiceControl_Name, Localization.Error + "! " + e.Result.Content);
                return;
            }

            List<MeasureInfo> measures = XmlSerializationUtility.XmlStr2Obj<List<MeasureInfo>>(e.Result.Content);
            if (measures != null)
            {
                foreach (MeasureInfo info in measures)
                {
                    CustomTreeNode groupNode = null;
                    String groupName = String.Empty;
                    //Показатели могут быть сгруппированы в группы
                    PropertyInfo pi = info.GetProperty("MEASUREGROUP_NAME");
                    if (pi != null && pi.Value != null)
                    {
                        groupName = pi.Value.ToString();
                    }
                    // Создаем узел для группы
                    if (!String.IsNullOrEmpty(groupName))
                    {
                        if (m_GroupNodes.ContainsKey(groupName))
                        {
                            groupNode = m_GroupNodes[groupName];
                        }
                        else
                        {
                            groupNode = new FolderTreeNode();
                            groupNode.Text = groupName;
                            m_GroupNodes[groupName] = groupNode;

                            if (parentNode == null)
                                Tree.Items.Add(groupNode);
                            else
                                parentNode.Items.Add(groupNode);
                        }
                    }

                    if (groupNode == null)
                        groupNode = parentNode;

                    MeasureTreeNode node = new MeasureTreeNode(info);
                    // Показатели будут конечными узлами. Двойной клик на них будет равнозначен выбору
                    node.Expanded += new RoutedEventHandler(node_Expanded);
                    node.Collapsed += new RoutedEventHandler(node_Collapsed);

                    if (groupNode == null)
                        Tree.Items.Add(node);
                    else
                        groupNode.Items.Add(node);

                }
            }
        }

        Dictionary<String, CustomTreeNode> m_GroupNodes = new Dictionary<String, CustomTreeNode>();

        void node_Collapsed(object sender, RoutedEventArgs e)
        {
            Raise_ApplySelection();
        }

        void node_Expanded(object sender, RoutedEventArgs e)
        {
            Raise_ApplySelection();
        }

        /// <summary>
        /// Событие генерируется после окончания выбора
        /// </summary>
        public event EventHandler ApplySelection;

        /// <summary>
        /// Генерирует событие "Выбор окончен"
        /// </summary>
        private void Raise_ApplySelection()
        {
            EventHandler handler = ApplySelection;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
