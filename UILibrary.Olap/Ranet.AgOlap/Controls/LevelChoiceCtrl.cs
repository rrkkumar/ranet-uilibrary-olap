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
using Ranet.Olap.Core.Metadata;
using Ranet.AgOlap.Controls.General;
using Ranet.AgOlap.Controls.General.ClientServer;
using Ranet.AgOlap.Commands;
using Ranet.Olap.Core;
using System.Collections.Generic;
using System.Text;

namespace Ranet.AgOlap.Controls
{
    public class LevelChoiceCtrl : AgTreeControlBase, IChoiceControl
    {
        CustomTree Tree = null;
        public LevelChoiceCtrl()
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
            LevelTreeNode node = e.NewValue as LevelTreeNode;
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

        public String m_HierarchyUniqueName = String.Empty;
        /// <summary>
        /// Имя иерархии OLAP измерения
        /// </summary>
        public String HierarchyUniqueName
        {
            get
            {
                return m_HierarchyUniqueName;
            }
            set
            {
                m_HierarchyUniqueName = value;
            }
        }
        #endregion Свойства для настройки на OLAP

        public LevelInfo SelectedInfo
        {
            get
            {
                LevelTreeNode node = null;
                node = Tree.SelectedItem as LevelTreeNode;

                if (node != null)
                {
                    //Запоминаем выбранный элемент
                    return node.Info as LevelInfo;
                }

                return null;
            }
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
                LogManager.LogException(Localization.LevelChoiceControl_Name, e.Error);
                return;
            }


            if (e.Result.ContentType == InvokeContentType.Error)
            {
                ShowErrorInTree(parentNode);
                LogManager.LogMessage(Localization.LevelChoiceControl_Name, Localization.Error + "! " + e.Result.Content);
                return;
            }

            if (wrapper != null)
            {
                switch (wrapper.Schema.QueryType)
                { 
                    case MetadataQueryType.GetDimensions:
                        GetDimensions_InvokeCommandCompleted(e, parentNode);
                        break;
                    case MetadataQueryType.GetHierarchy:
                        GetHierarchy_InvokeCommandCompleted(e, parentNode);
                        break;
                    case MetadataQueryType.GetDimension:
                        GetDimension_InvokeCommandCompleted(e, parentNode);
                        break;
                    case MetadataQueryType.GetHierarchies:
                        GetHierarchies_InvokeCommandCompleted(e, parentNode);
                        break;
                    case MetadataQueryType.GetLevels:
                        GetLevels_InvokeCommandCompleted(e, parentNode);
                        break;
                }
            }

        }

        public void Initialize()
        {
            RefreshTree();
        }

        Dictionary<String, CustomTreeNode> m_GroupNodes = new Dictionary<String, CustomTreeNode>();

        private void RefreshTree()
        {
            m_GroupNodes.Clear();
            Tree.Items.Clear();

            //Если имя иерархии задано, то отображать в дереве нужно только эту иерархию
            if (!String.IsNullOrEmpty(HierarchyUniqueName) && !String.IsNullOrEmpty(DimensionUniqueName))
            {
                CreateRootHierarchyNode();
            }
            else
            {
                //Если задано имя измерения, то нужно отображать информацию только для этого измерения
                if (!String.IsNullOrEmpty(DimensionUniqueName))
                {
                    CreateRootDimensionNode();
                }
                else
                {
                    CreateRootCubeNode();
                }
            }
        }

        #region Корневой узел для куба
        void CreateRootCubeNode()
        {
            //Будем выводить информацию для всего куба
            CustomTreeNode cubeNode = new CustomTreeNode();
            cubeNode.Text = CubeName;
            cubeNode.Icon = UriResources.Images.Cube16;
            Tree.Items.Add(cubeNode);
            cubeNode.Expanded += new RoutedEventHandler(cubeNode_Expanded);
            cubeNode.IsWaiting = true;
            cubeNode.IsExpanded = true;
        }

        void cubeNode_Expanded(object sender, RoutedEventArgs e)
        {
            CustomTreeNode cubeNode = sender as CustomTreeNode;
            if (cubeNode != null && !cubeNode.IsInitialized)
            {
                if (String.IsNullOrEmpty(Connection) || String.IsNullOrEmpty(CubeName))
                {
                    // Сообщение в лог
                    StringBuilder builder = new StringBuilder();
                    if (String.IsNullOrEmpty(Connection))
                        builder.Append(Localization.Connection_PropertyDesc + ", ");
                    if (String.IsNullOrEmpty(CubeName))
                        builder.Append(Localization.CubeName_PropertyDesc);
                    LogManager.LogMessage(Localization.LevelChoiceControl_Name, Localization.Error + "! " + String.Format(Localization.ControlSettingsNotInitialized_Message, builder.ToString()));

                    cubeNode.IsWaiting = false;
                    cubeNode.IsError = true;
                    return;
                }
                
                cubeNode.IsWaiting = true;

                MetadataQuery args = CommandHelper.CreateGetDimensionsQueryArgs(Connection, CubeName);
                Loader.LoadData(XmlSerializationUtility.Obj2XmlStr(args, Common.Namespace), new MetadataQueryWrapper<CustomTreeNode>(args, cubeNode));
            }
        }

        void GetDimensions_InvokeCommandCompleted(DataLoaderEventArgs e, CustomTreeNode parentNode)
        {
            List<DimensionInfo> dimensions = XmlSerializationUtility.XmlStr2Obj<List<DimensionInfo>>(e.Result.Content);
            if (dimensions != null)
            {
                foreach (DimensionInfo info in dimensions)
                {
                    if (info.DimensionType != DimensionInfoTypeEnum.Measure)
                    {
                        DimensionTreeNode dimNode = new DimensionTreeNode(info);
                        dimNode.IsWaiting = true;
                        dimNode.Expanded += new RoutedEventHandler(dimNode_Expanded);
                        if (parentNode == null)
                            Tree.Items.Add(dimNode);
                        else
                            parentNode.Items.Add(dimNode);
                    }
                }
            }
        }
        #endregion Корневой узел для куба

        #region Корневой узел для иерархии
        private void CreateRootHierarchyNode()
        {
            if (String.IsNullOrEmpty(Connection) || 
                String.IsNullOrEmpty(CubeName) ||
                String.IsNullOrEmpty(DimensionUniqueName) ||
                String.IsNullOrEmpty(HierarchyUniqueName))
            {
                // Сообщение в лог
                StringBuilder builder = new StringBuilder();
                if (String.IsNullOrEmpty(Connection))
                    builder.Append(Localization.Connection_PropertyDesc + ", ");
                if (String.IsNullOrEmpty(CubeName))
                    builder.Append(Localization.CubeName_PropertyDesc + ", ");
                if (String.IsNullOrEmpty(DimensionUniqueName))
                    builder.Append(Localization.DimensionUniqueName_PropertyDesc + ", ");
                if (String.IsNullOrEmpty(HierarchyUniqueName))
                    builder.Append(Localization.HierarchyUniqueName_PropertyDesc);
                LogManager.LogMessage(Localization.LevelChoiceControl_Name, Localization.Error + "! " + String.Format(Localization.ControlSettingsNotInitialized_Message, builder.ToString()));

                Tree.IsWaiting = false;
                Tree.IsError = true;
                return;
            }

            Tree.IsWaiting = true;
            MetadataQuery args = CommandHelper.CreateGetHierarchyQueryArgs(Connection, CubeName, DimensionUniqueName, HierarchyUniqueName);
            Loader.LoadData(XmlSerializationUtility.Obj2XmlStr(args, Common.Namespace), new MetadataQueryWrapper<CustomTreeNode>(args, null));
        }

        void GetHierarchy_InvokeCommandCompleted(DataLoaderEventArgs e, CustomTreeNode parentNode)
        {
            HierarchyInfo hierarchy = XmlSerializationUtility.XmlStr2Obj<HierarchyInfo>(e.Result.Content);
            if (hierarchy != null)
            {
                HierarchyTreeNode dimNode = new HierarchyTreeNode(hierarchy);
                dimNode.IsWaiting = true;
                dimNode.Expanded += new RoutedEventHandler(hierarchyNode_Expanded);
                Tree.Items.Add(dimNode);
                dimNode.IsExpanded = true;
            }
        }
        #endregion Корневой узел для иерархии

        #region Корневой узел для измерения
        private void CreateRootDimensionNode()
        {
            if (String.IsNullOrEmpty(Connection) ||
                String.IsNullOrEmpty(CubeName) ||
                String.IsNullOrEmpty(DimensionUniqueName))
            {
                // Сообщение в лог
                StringBuilder builder = new StringBuilder();
                if (String.IsNullOrEmpty(Connection))
                    builder.Append(Localization.Connection_PropertyDesc + ", ");
                if (String.IsNullOrEmpty(CubeName))
                    builder.Append(Localization.CubeName_PropertyDesc + ", ");
                if (String.IsNullOrEmpty(DimensionUniqueName))
                    builder.Append(Localization.DimensionUniqueName_PropertyDesc);
                LogManager.LogMessage(Localization.LevelChoiceControl_Name, Localization.Error + "! " + String.Format(Localization.ControlSettingsNotInitialized_Message, builder.ToString()));

                Tree.IsWaiting = false;
                Tree.IsError = true;
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
        #endregion Корневой узел для измерения

         void dimNode_Expanded(object sender, RoutedEventArgs e)
        {
            DimensionTreeNode dimNode = sender as DimensionTreeNode;
            if (dimNode != null && !dimNode.IsInitialized)
            {
                GetHierarchies(dimNode);
            }
        }

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
                    hierarchyNode.IsWaiting = true;
                    hierarchyNode.Expanded += new RoutedEventHandler(hierarchyNode_Expanded);
                    if (groupNode == null)
                        Tree.Items.Add(hierarchyNode);
                    else
                        groupNode.Items.Add(hierarchyNode);

                }
            }
        }

        void hierarchyNode_Expanded(object sender, RoutedEventArgs e)
        {
            HierarchyTreeNode hierarchyNode = sender as HierarchyTreeNode;
            if (hierarchyNode != null && !hierarchyNode.IsInitialized)
            {
                GetLevels(hierarchyNode);
            }
        }

        private void GetLevels(HierarchyTreeNode node)
        {
            String dimUniqueName = String.Empty;
            String hierarchyUniqueName = String.Empty;
            if (node != null)
            {
                dimUniqueName = (node.Info as HierarchyInfo).ParentDimensionId;
                hierarchyUniqueName = (node.Info as HierarchyInfo).UniqueName;
            }

            MetadataQuery args = CommandHelper.CreateGetLevelsQueryArgs(Connection, CubeName, dimUniqueName, hierarchyUniqueName);
            Loader.LoadData(XmlSerializationUtility.Obj2XmlStr(args, Common.Namespace), new MetadataQueryWrapper<CustomTreeNode>(args, node));
        }

        void GetLevels_InvokeCommandCompleted(DataLoaderEventArgs e, CustomTreeNode parentNode)
        {
            List<LevelInfo> levels = XmlSerializationUtility.XmlStr2Obj<List<LevelInfo>>(e.Result.Content);
            if (levels != null)
            {
                int indx = 0;
                bool useAllLevel = true;
                foreach (LevelInfo info in levels)
                {
                    LevelTreeNode levelNode = new LevelTreeNode(info);
                    levelNode.Expanded += new RoutedEventHandler(node_Expanded);
                    levelNode.Collapsed += new RoutedEventHandler(node_Collapsed);

                    //Если нулевой уровень не All, то иконку ставим как для уровня 1
                    if (indx == 0 && info.LevelType != LevelInfoTypeEnum.All)
                        useAllLevel = false;
                    if (!useAllLevel)
                        levelNode.UseAllLevelIcon = false;

                    if (parentNode == null)
                        Tree.Items.Add(levelNode);
                    else
                        parentNode.Items.Add(levelNode);
                    indx++;
                }
            }
        }

        void node_Collapsed(object sender, RoutedEventArgs e)
        {
            Raise_ApplySelection();
        }

        void node_Expanded(object sender, RoutedEventArgs e)
        {
            Raise_ApplySelection();
        }
    }
}
