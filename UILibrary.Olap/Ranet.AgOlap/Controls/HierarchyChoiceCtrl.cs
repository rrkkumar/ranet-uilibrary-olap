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
using Ranet.AgOlap.Controls.General;
using Ranet.AgOlap.Controls.General.Tree;
using Ranet.Olap.Core.Metadata;
using System.Collections.Generic;
using Ranet.AgOlap.Controls.General.ClientServer;
using Ranet.AgOlap.Commands;
using Ranet.Olap.Core;

namespace Ranet.AgOlap.Controls
{
    public class HierarchyChoiceCtrl : AgTreeControlBase
    {
        /// <summary>
        /// Указывает необходимость отображать информация о всех типах кубов (Cube, Dimension, Unknown)
        /// </summary>
        public bool ShowAllCubes;

        CustomTree Tree = null;

        public HierarchyChoiceCtrl()
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
            HierarchyTreeNode node = e.NewValue as HierarchyTreeNode;
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

        public String m_DimensionUniqueName = String.Empty;
        /// <summary>
        /// Имя OLAP измерения
        /// </summary>
        public String DimensionUniqueName
        {
            get
            {
                return m_DimensionUniqueName;
            }
            set
            {
                m_DimensionUniqueName = value;
            }
        }
        #endregion Свойства для настройки на OLAP

        private HierarchyInfo m_SelectedInfo = null;
        public HierarchyInfo SelectedInfo
        {
            get {
                return m_SelectedInfo;
            }
        }

        Dictionary<String, CustomTreeNode> m_GroupNodes = new Dictionary<String, CustomTreeNode>();

        public void Initialize()
        {
            RefreshTree();
        }

        private void RefreshTree()
        {
            m_GroupNodes.Clear();
            Tree.Items.Clear();

            CreateRootDimensionNode();
        }

        #region Корневой узел для измерения
        private void CreateRootDimensionNode()
        {
            if (String.IsNullOrEmpty(CubeName))
            {
                LogManager.LogMessage(Localization.HierarchyChoiceControl_Name, Localization.Error + "! " + String.Format(Localization.ControlPropertyNotInitialized_Message, Localization.CubeName_PropertyDesc));
                return;
            }

            if (String.IsNullOrEmpty(DimensionUniqueName))
            {
                LogManager.LogMessage(Localization.HierarchyChoiceControl_Name, Localization.Error + "! " + String.Format(Localization.ControlPropertyNotInitialized_Message, Localization.DimensionUniqueName_PropertyDesc));
                return;
            }

            Tree.IsWaiting = true;
            MetadataQuery args = CommandHelper.CreateGetDimensionQueryArgs(Connection, CubeName, DimensionUniqueName);
            Loader.LoadData(XmlSerializationUtility.Obj2XmlStr(args, Common.Namespace), new MetadataQueryWrapper<CustomTreeNode>(args, null));
        }

        void GetDimension_InvokeCommandCompleted(DataLoaderEventArgs e, CustomTreeNode parentNode)
        {
            m_GroupNodes.Clear();
            Tree.Items.Clear();

            if (!String.IsNullOrEmpty(e.Result.Content))
            {
                DimensionInfo dimension = XmlSerializationUtility.XmlStr2Obj<DimensionInfo>(e.Result.Content);
                if (dimension != null)
                {
                    DimensionTreeNode dimNode = new DimensionTreeNode(dimension);
                    dimNode.IsWaiting = true;
                    dimNode.Expanded += new RoutedEventHandler(dimNode_Expanded);
                    Tree.Items.Add(dimNode);
                    dimNode.IsExpanded = true;
                }
            }
        }

        void dimNode_Expanded(object sender, RoutedEventArgs e)
        {
            DimensionTreeNode dimNode = sender as DimensionTreeNode;
            if (dimNode != null && !dimNode.IsInitialized)
            {
                GetHierarchies(dimNode);
            }
        }
        #endregion Корневой узел для измерения

        private void GetHierarchies(DimensionTreeNode node)
        {
            String dimUniqueName = String.Empty;
            if (node != null)
            {
                dimUniqueName = (node.Info as DimensionInfo).UniqueName;
            }

            MetadataQuery args = CommandHelper.CreateGetHierarchiesQueryArgs(Connection, CubeName, dimUniqueName);
            Loader.LoadData(XmlSerializationUtility.Obj2XmlStr(args, Common.Namespace), new MetadataQueryWrapper<CustomTreeNode>(args, node));
        }

        void GetHierarchies_InvokeCommandCompleted(DataLoaderEventArgs e, CustomTreeNode parentNode)
        {
            List<HierarchyInfo> hierarchies = XmlSerializationUtility.XmlStr2Obj<List<HierarchyInfo>>(e.Result.Content);
            if (hierarchies != null)
            {
                foreach (HierarchyInfo info in hierarchies)
                {
                    CustomTreeNode groupNode = null;

                    //Иерархии могут быть сгруппированы в папки. Причем папки могут быть вложенными. Например: "Динамика\\Оборачиваемость"
                    if (!String.IsNullOrEmpty(info.DisplayFolder))
                    {
                        // Если папка по такому же полному пути уже создана то все Ок
                        if (m_GroupNodes.ContainsKey(info.DisplayFolder))
                        {
                            groupNode = m_GroupNodes[info.DisplayFolder];
                        }
                        else
                        {
                            CustomTreeNode prevNode = parentNode;
                            // Разбиваем полный путь на составляющие и создаем папку для каждой из них
                            String[] groups = info.DisplayFolder.Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                            if (groups != null)
                            {
                                foreach (String groupName in groups)
                                {
                                    // Создаем узел для группы
                                    if (!String.IsNullOrEmpty(groupName))
                                    {
                                        if (m_GroupNodes.ContainsKey(groupName))
                                        {
                                            prevNode = m_GroupNodes[groupName];
                                        }
                                        else
                                        {
                                            groupNode = new FolderTreeNode();
                                            groupNode.Text = groupName;
                                            m_GroupNodes[groupName] = groupNode;

                                            if (prevNode == null)
                                                Tree.Items.Add(groupNode);
                                            else
                                                prevNode.Items.Add(groupNode);

                                            prevNode = groupNode;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (groupNode == null)
                        groupNode = parentNode;

                    HierarchyTreeNode hierarchyNode = new HierarchyTreeNode(info);
                    // Измерения будут конечными узлами. Двойной клик на них будет равнозначен выбору
                    hierarchyNode.Expanded += new RoutedEventHandler(node_Expanded);
                    hierarchyNode.Collapsed += new RoutedEventHandler(node_Collapsed);
                    if (groupNode == null)
                        Tree.Items.Add(hierarchyNode);
                    else
                        groupNode.Items.Add(hierarchyNode);

                }
            }
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
            CustomTreeNode parentNode = null;
            MetadataQueryWrapper<CustomTreeNode> wrapper = e.UserState as MetadataQueryWrapper<CustomTreeNode>;
            if (wrapper != null)
            {
                parentNode = wrapper.UserData;
                if (parentNode != null)
                {
                    parentNode.IsWaiting = false;
                }
                else
                {
                    Tree.IsWaiting = false;
                }
            }

            if (e.Error != null)
            {
                ShowErrorInTree(parentNode);
                LogManager.LogException(Localization.HierarchyChoiceControl_Name, e.Error);
                return;
            }

            if (e.Result.ContentType == InvokeContentType.Error)
            {
                ShowErrorInTree(parentNode);
                LogManager.LogMessage(Localization.HierarchyChoiceControl_Name, Localization.Error + "! " + e.Result.Content);
                return;
            }

            if (wrapper != null)
            {
                switch (wrapper.Schema.QueryType)
                {
                    case MetadataQueryType.GetDimension:
                        GetDimension_InvokeCommandCompleted(e, parentNode);
                        break;
                    case MetadataQueryType.GetHierarchies:
                        GetHierarchies_InvokeCommandCompleted(e, parentNode);
                        break;
                }
            }
        }

        void node_Collapsed(object sender, RoutedEventArgs e)
        {
            ApplyItemsSelection();
            Raise_ApplySelection();
        }

        void node_Expanded(object sender, RoutedEventArgs e)
        {
            ApplyItemsSelection();
            Raise_ApplySelection();
        }

        /// <summary>
        /// Событие генерируется после окончания выбора
        /// </summary>
        public event EventHandler ApplySelection;

        /// <summary>
        /// Событие генерируется при нажатии на кнопку Отмена
        /// </summary>
        public event EventHandler CancelSelection;

        /// <summary>
        /// Генерирует событие "Выбор окончен"
        /// </summary>
        private void Raise_ApplySelection()
        {
            EventHandler handler = ApplySelection;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Генерирует событие "Отмена"
        /// </summary>
        private void Raise_CancelSelection()
        {
            EventHandler handler = CancelSelection;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyItemsSelection();
            Raise_ApplySelection();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Генерируем событие - Отмена
            Raise_CancelSelection();
        }

        private void ApplyItemsSelection()
        {
            HierarchyTreeNode node = null;
            node = Tree.SelectedItem as HierarchyTreeNode;

            if (node != null)
            {
                //Запоминаем выбранный элемент
                m_SelectedInfo = node.Info as HierarchyInfo;
            }
        }
    }
}
