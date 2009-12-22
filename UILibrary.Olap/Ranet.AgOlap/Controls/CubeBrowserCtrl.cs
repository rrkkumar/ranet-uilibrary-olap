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
using Ranet.AgOlap.Controls.General.ClientServer;
using Ranet.AgOlap.Commands;
using Ranet.Olap.Core.Metadata;
using Ranet.Olap.Core;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;

namespace Ranet.AgOlap.Controls
{
    public class DragNodeArgs<T> : EventArgs 
    {
        public readonly TreeViewItem Node;
        public readonly T Args;

        public DragNodeArgs(TreeViewItem node, T args)
        {
            Node = node;
            Args = args;
        }
    }

    
    public class CubeBrowserCtrl : AgTreeControlBase
    {
        CustomTree Tree = null;

        #region Коллекции созданных узлов
        Dictionary<String, DimensionTreeNode> m_Dimension_Nodes = new Dictionary<string, DimensionTreeNode>();
        Dictionary<String, HierarchyTreeNode> m_Hierarchy_Nodes = new Dictionary<string, HierarchyTreeNode>();
        Dictionary<String, LevelTreeNode> m_Level_Nodes = new Dictionary<string, LevelTreeNode>();
        Dictionary<String, MeasureTreeNode> m_Measure_Nodes = new Dictionary<string, MeasureTreeNode>();
        Dictionary<String, KpiValueTreeNode> m_KpiValue_Nodes = new Dictionary<string, KpiValueTreeNode>();
        Dictionary<String, KpiGoalTreeNode> m_KpiGoal_Nodes = new Dictionary<string, KpiGoalTreeNode>();
        Dictionary<String, KpiTrendTreeNode> m_KpiTrend_Nodes = new Dictionary<string, KpiTrendTreeNode>();
        Dictionary<String, KpiStatusTreeNode> m_KpiStatus_Nodes = new Dictionary<string, KpiStatusTreeNode>();

        void ClearDictionaries()
        {
            m_Hierarchy_Nodes.Clear();
            m_Level_Nodes.Clear();
            m_Measure_Nodes.Clear();
            m_KpiValue_Nodes.Clear();
            m_KpiGoal_Nodes.Clear();
            m_KpiTrend_Nodes.Clear();
            m_KpiStatus_Nodes.Clear();
        }

        public DimensionTreeNode FindDimensionNode(String uniqueName)
        {
            if (m_Dimension_Nodes.ContainsKey(uniqueName))
                return m_Dimension_Nodes[uniqueName];
            return null;
        }

        public HierarchyTreeNode FindHierarchyNode(String uniqueName)
        {
            if (m_Hierarchy_Nodes.ContainsKey(uniqueName))
                return m_Hierarchy_Nodes[uniqueName];
            return null;
        }

        public LevelTreeNode FindLevelNode(String uniqueName)
        {
            if (m_Level_Nodes.ContainsKey(uniqueName))
                return m_Level_Nodes[uniqueName];
            return null;
        }

        public MeasureTreeNode FindMeasureNode(String uniqueName)
        {
            if (m_Measure_Nodes.ContainsKey(uniqueName))
                return m_Measure_Nodes[uniqueName];
            return null;
        }

        public KpiValueTreeNode FindKpiValueNode(String name)
        {
            if (m_KpiValue_Nodes.ContainsKey(name))
                return m_KpiValue_Nodes[name];
            return null;
        }

        public KpiGoalTreeNode FindKpiGoalNode(String name)
        {
            if (m_KpiGoal_Nodes.ContainsKey(name))
                return m_KpiGoal_Nodes[name];
            return null;
        }

        public KpiStatusTreeNode FindKpiStatusNode(String name)
        {
            if (m_KpiStatus_Nodes.ContainsKey(name))
                return m_KpiStatus_Nodes[name];
            return null;
        }

        public KpiTrendTreeNode FindKpiTrendNode(String name)
        {
            if (m_KpiTrend_Nodes.ContainsKey(name))
                return m_KpiTrend_Nodes[name];
            return null;
        }
        #endregion Коллекции созданных узлов

        MeasureGroupCombo comboMeasureGroup;

        public CubeBrowserCtrl()
        {
            //Border border = new Border() { BorderBrush = new SolidColorBrush(Colors.DarkGray), BorderThickness = new Thickness(1) };
            //Grid LayoutRoot = new Grid() { Margin = new Thickness(3) };
            Grid LayoutRoot = new Grid() { Margin = new Thickness(0) };
            LayoutRoot.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            LayoutRoot.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            LayoutRoot.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            LayoutRoot.RowDefinitions.Add(new RowDefinition());

            HeaderControl measureGroup_Header = new HeaderControl(UriResources.Images.MeasureGroup16, Localization.MeasureGroup) { Margin = new Thickness(0, 0, 0, 3) };
            LayoutRoot.Children.Add(measureGroup_Header);
            Grid.SetRow(measureGroup_Header, 0);

            comboMeasureGroup = new MeasureGroupCombo();// { Margin = new Thickness(0, 5, 0, 0)};
            comboMeasureGroup.SelectionChanged += new EventHandler(comboMeasureGroup_SelectionChanged);
            LayoutRoot.Children.Add(comboMeasureGroup);
            Grid.SetRow(comboMeasureGroup, 1);

            HeaderControl cube_Header = new HeaderControl(UriResources.Images.Cube16, Localization.MdxDesigner_CubeMetadata) { Margin = new Thickness(0, 5, 0, 3) };
            LayoutRoot.Children.Add(cube_Header);
            Grid.SetRow(cube_Header, 2);

            Tree = new CustomTree();// { Margin = new Thickness(0, 5, 0, 0) };
            LayoutRoot.Children.Add(Tree);
            Grid.SetRow(Tree, 3);

            //border.Child = LayoutRoot;
            //base.Content = border;
            base.Content = LayoutRoot;

            Tree.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(Tree_SelectedItemChanged);
        }

        void comboMeasureGroup_SelectionChanged(object sender, EventArgs e)
        {
            CreateRootCubeNode();
        }

        void Tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            InfoBase info = null;
            CubeTreeNode node = e.NewValue as CubeTreeNode;
            if (node != null)
            {
                info = node.Info;
            }
            Raise_SelectedItemChanged(info);
        }

        #region Свойства для настройки на OLAP

        private String m_Connection = String.Empty;
        /// <summary>
        /// ID соединения
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

        private String m_CubeName = String.Empty;
        /// <summary>
        /// Имя куба
        /// </summary>
        public String CubeName
        {
            get
            {
                return m_CubeName;
            }
            set
            {
                m_CubeInfo = null;
                m_CubeName = value;
            }
        }
        #endregion Свойства для настройки на OLAP

        CubeDefInfo m_CubeInfo = null;
        /// <summary>
        /// Метаинформация о кубе
        /// </summary>
        public CubeDefInfo CubeInfo
        {
            get { return m_CubeInfo; }
        }

        public void Initialize()
        {
            ClearTree();

            GetCube();
        }

        public bool DragNodes = false;

        void ClearTree()
        {
            ClearDictionaries();

            Tree.Items.Clear();
            m_KPIGroupNodes.Clear();
            m_HierarchyGroupNodes.Clear();
            m_MeasuresGroupNodes.Clear();
        }

        // Summary:
        //     Occurs when the System.Windows.Controls.Primitives.Thumb control loses mouse
        //     capture.
        public event EventHandler<DragNodeArgs<DragCompletedEventArgs>> DragCompleted;
        //
        // Summary:
        //     Occurs one or more times as the mouse pointer is moved when a System.Windows.Controls.Primitives.Thumb
        //     control has logical focus and mouse capture.
        public event EventHandler<DragNodeArgs<DragDeltaEventArgs>> DragDelta;
        //
        // Summary:
        //     Occurs when a System.Windows.Controls.Primitives.Thumb control receives logical
        //     focus and mouse capture.
        public event EventHandler<DragNodeArgs<DragStartedEventArgs>> DragStarted;

        #region Дерево: Кубы
        void CreateRootCubeNode()
        {
            ClearTree();

            CubeDefInfo info = m_CubeInfo;
            if (info != null)
            {
                //Будем выводить информацию для всего куба
                CubeTreeNode cubeNode = new CubeTreeNode(info);
                Tree.Items.Add(cubeNode);

                // Добавляем узел KPIs
                KPIsFolderTreeNode kpisNode = new KPIsFolderTreeNode();
                cubeNode.Items.Add(kpisNode);
                //kpisNode.IsWaiting = true;
                //kpisNode.Expanded += new RoutedEventHandler(kpisNode_Expanded);
                CreateKPIs(kpisNode, m_CubeInfo);

                // Добавляем узел Measures
                MeasuresFolderTreeNode measuresNode = new MeasuresFolderTreeNode();
                measuresNode.Text = "Measures";
                cubeNode.Items.Add(measuresNode);
                measuresNode.Icon = UriResources.Images.Measure16;
                //measuresNode.IsWaiting = true;
                //measuresNode.Expanded += new RoutedEventHandler(measuresNode_Expanded);
                CreateMeasures(measuresNode, m_CubeInfo);

                //cubeNode.IsWaiting = true;
                //cubeNode.Expanded += new RoutedEventHandler(cubeNode_Expanded);
                CreateDimensions(cubeNode, info);

                cubeNode.IsExpanded = true;
            }
        }

        void DragThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            EventHandler<DragNodeArgs<DragCompletedEventArgs>> handler = DragCompleted;
            if (handler != null)
            {
                handler(sender, new DragNodeArgs<DragCompletedEventArgs>(sender as TreeViewItem, e));
            }
        }

        void DragThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            EventHandler<DragNodeArgs<DragDeltaEventArgs>> handler = DragDelta;
            if (handler != null)
            {
                handler(sender, new DragNodeArgs<DragDeltaEventArgs>(sender as TreeViewItem, e));
            }
        }

        void DragThumb_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            EventHandler<DragNodeArgs<DragStartedEventArgs>> handler = DragStarted;
            if (handler != null)
            {
                handler(sender, new DragNodeArgs<DragStartedEventArgs>(sender as TreeViewItem, e));
            }
        }

        //void cubeNode_Expanded(object sender, RoutedEventArgs e)
        //{
        //    CubeTreeNode cubeNode = sender as CubeTreeNode;
        //    if (cubeNode != null && !cubeNode.IsInitialized)
        //    {
        //        cubeNode.IsWaiting = false;
        //        CreateDimensions(cubeNode, cubeNode.Info as CubeDefInfo);
        //    }
        //}
        #endregion Дерево: Кубы

        #region Дерево: Измерения
        void CreateDimensions(CustomTreeNode parentNode, CubeDefInfo cube)
        {
            if (cube != null)
            {
                foreach (DimensionInfo info in cube.Dimensions)
                {
                    if (info.DimensionType != DimensionInfoTypeEnum.Measure)
                    {
                        // Учитываем текущую группу мер
                        if (comboMeasureGroup.CurrentItem != null &&
                            (comboMeasureGroup.CurrentItem.Name == Localization.MeasureGroup_All ||
                            comboMeasureGroup.CurrentItem.Dimensions.Contains(info.UniqueName)))
                        {
                            DimensionTreeNode dimNode = new DimensionTreeNode(info);
                            // Тягание
                            AllowDragDrop(dimNode);

                            //dimNode.IsWaiting = true;
                            //dimNode.Expanded += new RoutedEventHandler(dimNode_Expanded);
                            m_Dimension_Nodes[info.UniqueName] = dimNode;
                            CreateHierarchies(dimNode, info);

                            if (parentNode == null)
                                Tree.Items.Add(dimNode);
                            else
                                parentNode.Items.Add(dimNode);
                        }
                    }
                }
            }
        }
 
        //void dimNode_Expanded(object sender, RoutedEventArgs e)
        //{
        //    DimensionTreeNode dimNode = sender as DimensionTreeNode;
        //    if (dimNode != null && !dimNode.IsInitialized)
        //    {
        //        dimNode.IsWaiting = false;
        //        CreateHierarchies(dimNode, dimNode.Info as DimensionInfo);
        //    }
        //}

        #endregion Дерево: Измерения

        #region Дерево: Иераррхии
        void CreateHierarchies(CustomTreeNode parentNode, DimensionInfo dimension)
        {
            if (dimension != null)
            {
                foreach (HierarchyInfo info in dimension.Hierarchies)
                {
                    CustomTreeNode groupNode = null;

                    //Иерархии могут быть сгруппированы в папки. Причем папки могут быть вложенными. Например: "Динамика\\Оборачиваемость"
                    if (!String.IsNullOrEmpty(info.DisplayFolder))
                    {
                        // Если папка по такому же полному пути уже создана то все Ок
                        if (m_HierarchyGroupNodes.ContainsKey(info.DisplayFolder))
                        {
                            groupNode = m_HierarchyGroupNodes[info.DisplayFolder];
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
                                        if (m_HierarchyGroupNodes.ContainsKey(groupName))
                                        {
                                            prevNode = m_HierarchyGroupNodes[groupName];
                                        }
                                        else
                                        {
                                            groupNode = new FolderTreeNode();
                                            groupNode.Text = groupName;
                                            m_HierarchyGroupNodes[groupName] = groupNode;

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
                    // Тягание
                    AllowDragDrop(hierarchyNode);

                    m_Hierarchy_Nodes[info.UniqueName] = hierarchyNode;
                    //hierarchyNode.IsWaiting = true;
                    //hierarchyNode.Expanded += new RoutedEventHandler(hierarchyNode_Expanded);
                    CreateLevels(hierarchyNode, info);

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
                hierarchyNode.IsWaiting = false;
                CreateLevels(hierarchyNode, hierarchyNode.Info as HierarchyInfo);
            }
        }
        #endregion Дерево: Иерархии

        #region Дерево: Уровни
        void CreateLevels(CustomTreeNode parentNode, HierarchyInfo hierarchy)
        {
            if (hierarchy != null)
            {
                int indx = 0;
                bool useAllLevel = true;
                foreach (LevelInfo info in hierarchy.Levels)
                {
                    LevelTreeNode levelNode = new LevelTreeNode(info);
                    // Тягание
                    AllowDragDrop(levelNode);

                    m_Level_Nodes[info.UniqueName] = levelNode;

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
        #endregion Дерево: Уровни

        #region Дерево: KPI
        //void kpisNode_Expanded(object sender, RoutedEventArgs e)
        //{
        //    KPIsFolderTreeNode kpisNode = sender as KPIsFolderTreeNode;
        //    if (kpisNode != null && !kpisNode.IsInitialized)
        //    {
        //        kpisNode.IsWaiting = false;
        //        CreateKPIs(kpisNode, m_CubeInfo);
        //    }
        //}

        void CreateKPIs(CustomTreeNode parentNode, CubeDefInfo cube)
        {
            if (cube != null)
            {
                foreach (KpiInfo info in cube.Kpis)
                {
                    // Учитываем текущую группу мер
                    if (comboMeasureGroup.CurrentItem != null &&
                        (comboMeasureGroup.CurrentItem.Name == Localization.MeasureGroup_All ||
                        comboMeasureGroup.CurrentItem.Kpis.Contains(info.Name)))
                    {
                        CustomTreeNode groupNode = null;

                        //String groupName = String.Empty;
                        //Показатели могут быть сгруппированы в группы. Причем папки могут быть вложенными. Например: "Динамика\\Оборачиваемость"
                        if (!String.IsNullOrEmpty(info.DisplayFolder))
                        {
                            // Если папка по такому же полному пути уже создана то все Ок
                            if (m_KPIGroupNodes.ContainsKey(info.DisplayFolder))
                            {
                                groupNode = m_KPIGroupNodes[info.DisplayFolder];
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
                                            if (m_KPIGroupNodes.ContainsKey(groupName))
                                            {
                                                prevNode = m_KPIGroupNodes[groupName];
                                            }
                                            else
                                            {
                                                groupNode = new FolderTreeNode();
                                                groupNode.Text = groupName;
                                                m_KPIGroupNodes[groupName] = groupNode;

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

                        KpiTreeNode node = new KpiTreeNode(info);

                        if (groupNode == null)
                            Tree.Items.Add(node);
                        else
                            groupNode.Items.Add(node);

                        // "KPI_VALUE"
                        KpiValueTreeNode valueNode = new KpiValueTreeNode(info);
                        AllowDragDrop(valueNode);
                        node.Items.Add(valueNode);
                        m_KpiValue_Nodes[info.Name] = valueNode;

                        // "KPI_GOAL"
                        KpiGoalTreeNode goalNode = new KpiGoalTreeNode(info);
                        AllowDragDrop(goalNode);
                        node.Items.Add(goalNode);
                        m_KpiGoal_Nodes[info.Name] = goalNode;

                        // "KPI_STATUS"
                        KpiStatusTreeNode statusNode = new KpiStatusTreeNode(info);
                        AllowDragDrop(statusNode);
                        node.Items.Add(statusNode);
                        m_KpiStatus_Nodes[info.Name] = statusNode;

                        // "KPI_TREND"
                        KpiTrendTreeNode trendNode = new KpiTrendTreeNode(info);
                        AllowDragDrop(trendNode);
                        node.Items.Add(trendNode);
                        m_KpiTrend_Nodes[info.Name] = trendNode;
                    }
                }
            }
        }
        #endregion Дерево: KPI

        void AllowDragDrop(CustomTreeNode node)
        {
            if (node != null)
            {
                node.UseDragDrop = DragNodes;
                node.DragStarted += new System.Windows.Controls.Primitives.DragStartedEventHandler(DragThumb_DragStarted);
                node.DragDelta += new DragDeltaEventHandler(DragThumb_DragDelta);
                node.DragCompleted += new DragCompletedEventHandler(DragThumb_DragCompleted);
            }
        }

        #region Дерево: Меры
        //void measuresNode_Expanded(object sender, RoutedEventArgs e)
        //{
        //    MeasuresFolderTreeNode measuresNode = sender as MeasuresFolderTreeNode;
        //    if (measuresNode != null && !measuresNode.IsInitialized)
        //    {
        //        measuresNode.IsWaiting = false;
        //        CreateMeasures(measuresNode, m_CubeInfo);
        //    }
        //}

        void CreateMeasures(CustomTreeNode parentNode, CubeDefInfo cube)
        {
            if (cube != null)
            {
                foreach (MeasureInfo info in cube.Measures)
                {
                    // Учитываем текущую группу мер
                    if (comboMeasureGroup.CurrentItem != null &&
                        (comboMeasureGroup.CurrentItem.Name == Localization.MeasureGroup_All ||
                        comboMeasureGroup.CurrentItem.Measures.Contains(info.UniqueName)))
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
                            if (m_MeasuresGroupNodes.ContainsKey(groupName))
                            {
                                groupNode = m_MeasuresGroupNodes[groupName];
                            }
                            else
                            {
                                groupNode = new FolderTreeNode();
                                groupNode.Text = groupName;
                                m_MeasuresGroupNodes[groupName] = groupNode;

                                if (parentNode == null)
                                    Tree.Items.Add(groupNode);
                                else
                                    parentNode.Items.Add(groupNode);
                            }
                        }

                        if (groupNode == null)
                            groupNode = parentNode;

                        MeasureTreeNode node = new MeasureTreeNode(info);
                        // Тягание
                        AllowDragDrop(node);

                        m_Measure_Nodes[info.UniqueName] = node;

                        if (groupNode == null)
                            Tree.Items.Add(node);
                        else
                            groupNode.Items.Add(node);
                    }
                }
            }
        }
        #endregion Дерево: Меры

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
            comboMeasureGroup.IsWaiting = false;
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
                LogManager.LogException(Localization.CubeBrowserControl_Name, e.Error);
                return;
            }

            if (e.Result.ContentType == InvokeContentType.Error)
            {
                ShowErrorInTree(parentNode);
                LogManager.LogMessage(Localization.CubeBrowserControl_Name, Localization.Error + "! " + e.Result.Content);
                return;
            }

            if (wrapper != null)
            {
                switch (wrapper.Schema.QueryType)
                {
                    case MetadataQueryType.GetCubeMetadata:
                    case MetadataQueryType.GetCubeMetadata_AllMembers:
                        GetCubeMetadata_InvokeCommandCompleted(e, parentNode);
                        break;
                    //case MetadataQueryType.GetDimensions:
                    //    GetDimensions_InvokeCommandCompleted(e, parentNode);
                    //    break;
                    //case MetadataQueryType.GetHierarchies:
                    //    GetHierarchies_InvokeCommandCompleted(e, parentNode);
                    //    break;
                    //case MetadataQueryType.GetLevels:
                    //    GetLevels_InvokeCommandCompleted(e, parentNode);
                    //    break;
                    //case MetadataQueryType.GetKPIs:
                    //    GetKPIs_InvokeCommandCompleted(e, parentNode);
                    //    break;
                    //case MetadataQueryType.GetMeasures:
                    //    GetMeasures_InvokeCommandCompleted(e, parentNode);
                    //    break;
                }
            }

        }

        #region Кубы
        void GetCube()
        {
            Tree.Items.Clear();
            Tree.IsWaiting = true;
            comboMeasureGroup.SelectionChanged -= new EventHandler(comboMeasureGroup_SelectionChanged);
            comboMeasureGroup.IsWaiting = true;
            
            MetadataQuery args = CommandHelper.CreateGetCubeMetadataArgs(Connection, CubeName, MetadataQueryType.GetCubeMetadata);
            Loader.LoadData(XmlSerializationUtility.Obj2XmlStr(args, Common.Namespace), new MetadataQueryWrapper<CustomTreeNode>(args, null));
        }

        void GetCubeMetadata_InvokeCommandCompleted(DataLoaderEventArgs e, CustomTreeNode parentNode)
        {
            Tree.Items.Clear();

            CubeDefInfo cube = XmlSerializationUtility.XmlStr2Obj<CubeDefInfo>(e.Result.Content);
            if (cube != null)
            {
                m_CubeInfo = cube;

                comboMeasureGroup.SelectionChanged -= new EventHandler(comboMeasureGroup_SelectionChanged);
                comboMeasureGroup.SelectionChanged += new EventHandler(comboMeasureGroup_SelectionChanged);
                comboMeasureGroup.Initialize(m_CubeInfo.MeasureGroups);
                //CreateRootCubeNode();
            }
        }
        #endregion Кубы

        Dictionary<String, CustomTreeNode> m_KPIGroupNodes = new Dictionary<String, CustomTreeNode>();
        Dictionary<String, CustomTreeNode> m_HierarchyGroupNodes = new Dictionary<String, CustomTreeNode>();
        Dictionary<String, CustomTreeNode> m_MeasuresGroupNodes = new Dictionary<String, CustomTreeNode>();
    }
}
