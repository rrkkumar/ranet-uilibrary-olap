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
using Ranet.Olap.Core.Metadata;
using Ranet.AgOlap.Controls.General.ClientServer;
using Ranet.AgOlap.Commands;
using Ranet.Olap.Core;
using System.Collections.Generic;
using Ranet.AgOlap.Controls.General.ItemControls;

namespace Ranet.AgOlap.Controls
{
    public class ServerExplorerCtrl : AgTreeControlBase
    {
        /// <summary>
        /// Указывает необходимость отображать информация о всех типах кубов (Cube, Dimension, Unknown)
        /// </summary>
        public bool ShowAllCubes;

        CustomTree Tree = null;
        ComboBoxEx Cubes_ComboBox = null;

        public ServerExplorerCtrl()
        {
            Tree = new CustomTree();
            Tree.Margin = new Thickness(0, 3, 0, 0);

            Border border = new Border() { BorderBrush = new SolidColorBrush(Colors.DarkGray), BorderThickness = new Thickness(1) };
            Grid LayoutRoot = new Grid() { Margin = new Thickness(3) };
            LayoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(22) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition());

            Cubes_ComboBox = new ComboBoxEx();
            Cubes_ComboBox.SelectionChanged += new SelectionChangedEventHandler(Cubes_ComboBox_SelectionChanged);

            LayoutRoot.Children.Add(Cubes_ComboBox);
            LayoutRoot.Children.Add(Tree);
            Grid.SetRow(Tree, 1);

            border.Child = LayoutRoot;
            base.Content = border;

            Tree.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(Tree_SelectedItemChanged);
        }

        void Cubes_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshTree();
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

        #endregion Свойства для настройки на OLAP

        String CurrentCubeName
        {
            get
            {
                CubeDefInfo info = CurrentCube;
                if (info != null)
                    return info.Name;
                return String.Empty;
            }
        }

        CubeDefInfo CurrentCube
        {
            get
            {
                CubeItemControl ctrl = Cubes_ComboBox.Combo.SelectedItem as CubeItemControl;
                if (ctrl != null)
                {
                    return ctrl.Info;
                }
                return null;
            }
        }

        public void Initialize()
        {
            ClearTree();

            GetCubes();
        }

        void ClearTree()
        {
            Tree.Items.Clear();
            m_KPIGroupNodes.Clear();
            m_HierarchyGroupNodes.Clear();
            m_MeasuresGroupNodes.Clear();
        }

        private void RefreshTree()
        {
            ClearTree();

            CreateRootCubeNode();
        }

        #region Дерево: Кубы
        void CreateRootCubeNode()
        {
            CubeDefInfo info = CurrentCube;
            if (info != null)
            {
                //Будем выводить информацию для всего куба
                CubeTreeNode cubeNode = new CubeTreeNode(info);
                Tree.Items.Add(cubeNode);

                cubeNode.Expanded += new RoutedEventHandler(cubeNode_Expanded);

                // Добавляем узел KPIs
                KPIsFolderTreeNode kpisNode = new KPIsFolderTreeNode();
                cubeNode.Items.Add(kpisNode);
                kpisNode.IsWaiting = true;
                kpisNode.Expanded += new RoutedEventHandler(kpisNode_Expanded);

                // Добавляем узел Measures
                MeasuresFolderTreeNode measuresNode = new MeasuresFolderTreeNode();
                measuresNode.Text = "Measures";
                cubeNode.Items.Add(measuresNode);
                measuresNode.Icon = UriResources.Images.Measure16;
                measuresNode.IsWaiting = true;
                measuresNode.Expanded += new RoutedEventHandler(measuresNode_Expanded);

                cubeNode.IsWaiting = true;
                cubeNode.IsExpanded = true;
            }
        }

        void cubeNode_Expanded(object sender, RoutedEventArgs e)
        {
            CubeTreeNode cubeNode = sender as CubeTreeNode;
            if (cubeNode != null && !cubeNode.IsInitialized)
            {
                GetDimensions(cubeNode);
            }
        }
        #endregion Дерево: Кубы

        #region Дерево: Измерения
        void GetDimensions(CubeTreeNode cubeNode)
        {
            if (cubeNode != null)
            {
                cubeNode.IsWaiting = true;
            }

            MetadataQuery args = CommandHelper.CreateGetDimensionsQueryArgs(Connection, CurrentCubeName);
            Loader.LoadData(XmlSerializationUtility.Obj2XmlStr(args, Common.Namespace), new MetadataQueryWrapper<CustomTreeNode>(args, cubeNode));
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

        void dimNode_Expanded(object sender, RoutedEventArgs e)
        {
            DimensionTreeNode dimNode = sender as DimensionTreeNode;
            if (dimNode != null && !dimNode.IsInitialized)
            {
                MetadataQuery args = CommandHelper.CreateGetHierarchiesQueryArgs(Connection, CurrentCubeName, (dimNode.Info as DimensionInfo).UniqueName);
                Loader.LoadData(XmlSerializationUtility.Obj2XmlStr(args, Common.Namespace), new MetadataQueryWrapper<CustomTreeNode>(args, dimNode));
            }
        }

        #endregion Дерево: Измерения

        #region Дерево: Иераррхии
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
        #endregion Дерево: Иерархии

        #region Дерево: Уровни
        private void GetLevels(HierarchyTreeNode node)
        {
            String dimUniqueName = String.Empty;
            String hierarchyUniqueName = String.Empty;
            if (node != null)
            {
                dimUniqueName = (node.Info as HierarchyInfo).ParentDimensionId;
                hierarchyUniqueName = (node.Info as HierarchyInfo).UniqueName;
            }

            MetadataQuery args = CommandHelper.CreateGetLevelsQueryArgs(Connection, CurrentCubeName, dimUniqueName, hierarchyUniqueName);
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
        void kpisNode_Expanded(object sender, RoutedEventArgs e)
        {
            KPIsFolderTreeNode kpisNode = sender as KPIsFolderTreeNode;
            if (kpisNode != null && !kpisNode.IsInitialized)
            {
                GetKPIs(kpisNode);
            }
        }

        private void GetKPIs(KPIsFolderTreeNode node)
        {
            MetadataQuery args = CommandHelper.CreateGetKPIsQueryArgs(Connection, CurrentCubeName);
            Loader.LoadData(XmlSerializationUtility.Obj2XmlStr(args, Common.Namespace), new MetadataQueryWrapper<CustomTreeNode>(args, node));
        }

        void GetKPIs_InvokeCommandCompleted(DataLoaderEventArgs e, CustomTreeNode parentNode)
        {
            List<KpiInfo> kpis = XmlSerializationUtility.XmlStr2Obj<List<KpiInfo>>(e.Result.Content);
            if (kpis != null)
            {
                foreach (KpiInfo info in kpis)
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

                }
            } 
        }
        #endregion Дерево: KPI

        #region Дерево: Меры
        void measuresNode_Expanded(object sender, RoutedEventArgs e)
        {
            MeasuresFolderTreeNode measuresNode = sender as MeasuresFolderTreeNode;
            if (measuresNode != null && !measuresNode.IsInitialized)
            {
                GetMeasures(measuresNode);
            }
        }

        private void GetMeasures(MeasuresFolderTreeNode node)
        {
            MetadataQuery args = CommandHelper.CreateGetMeasuresQueryArgs(Connection, CurrentCubeName);
            Loader.LoadData(XmlSerializationUtility.Obj2XmlStr(args, Common.Namespace), new MetadataQueryWrapper<CustomTreeNode>(args, node));
        }

        void GetMeasures_InvokeCommandCompleted(DataLoaderEventArgs e, CustomTreeNode parentNode)
        {
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
                    if (groupNode == null)
                        Tree.Items.Add(node);
                    else
                        groupNode.Items.Add(node);
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
                if (wrapper != null && wrapper.Schema.QueryType == MetadataQueryType.GetCubes)
                {
                    Cubes_ComboBox.IsEnabled = false;
                    Cubes_ComboBox.Clear();
                }
                LogManager.LogException(Localization.ServerExplorerControl_Name, e.Error);
                return;
            }

            if (e.Result.ContentType == InvokeContentType.Error)
            {
                ShowErrorInTree(parentNode);
                if (wrapper != null && wrapper.Schema.QueryType == MetadataQueryType.GetCubes)
                {
                    Cubes_ComboBox.IsEnabled = false;
                    Cubes_ComboBox.Clear();
                }
                LogManager.LogMessage(Localization.ServerExplorerControl_Name, Localization.Error + "! " + e.Result.Content);
                return;
            }

            if (wrapper != null)
            {
                switch (wrapper.Schema.QueryType)
                {
                    case MetadataQueryType.GetCubes:
                        GetCubes_InvokeCommandCompleted(e, parentNode);
                        break;
                    case MetadataQueryType.GetDimensions:
                        GetDimensions_InvokeCommandCompleted(e, parentNode);
                        break;
                    case MetadataQueryType.GetHierarchies:
                        GetHierarchies_InvokeCommandCompleted(e, parentNode);
                        break;
                    case MetadataQueryType.GetLevels:
                        GetLevels_InvokeCommandCompleted(e, parentNode);
                        break;
                    case MetadataQueryType.GetKPIs:
                        GetKPIs_InvokeCommandCompleted(e, parentNode);
                        break;
                    case MetadataQueryType.GetMeasures:
                        GetMeasures_InvokeCommandCompleted(e, parentNode);
                        break;
                }
            }

        }

        #region Кубы
        void GetCubes()
        {
            Cubes_ComboBox.IsEnabled = false;
            Cubes_ComboBox.Clear();

            String NODE_TEXT = Localization.Loading;
            ComboBoxItem item = new ComboBoxItem();
            WaitTreeControl ctrl = new WaitTreeControl();
            ctrl.Text = NODE_TEXT;
            item.Content = ctrl;
            Cubes_ComboBox.Combo.Items.Add(item);
            Cubes_ComboBox.Combo.SelectedIndex = 0;

            MetadataQuery args = CommandHelper.CreateGetCubesQueryArgs(Connection);
            Loader.LoadData(XmlSerializationUtility.Obj2XmlStr(args, Common.Namespace), new MetadataQueryWrapper<CustomTreeNode>(args, null));
        }

        void GetCubes_InvokeCommandCompleted(DataLoaderEventArgs e, CustomTreeNode parentNode)
        {
            Cubes_ComboBox.Clear();
            Cubes_ComboBox.IsEnabled = true;

            List<CubeDefInfo> cubes = XmlSerializationUtility.XmlStr2Obj<List<CubeDefInfo>>(e.Result.Content);
            if (cubes != null)
            {
                foreach (CubeDefInfo info in cubes)
                {
                    if (ShowAllCubes || info.Type == CubeInfoType.Cube)
                    {
                        Cubes_ComboBox.Combo.Items.Add(new CubeItemControl(info));
                    }
                }
            }

            if (Cubes_ComboBox.Combo.Items.Count > 0)
            {
                Cubes_ComboBox.Combo.SelectedIndex = 0;
            }
        }
        #endregion Кубы

        Dictionary<String, CustomTreeNode> m_KPIGroupNodes = new Dictionary<String, CustomTreeNode>();
        Dictionary<String, CustomTreeNode> m_HierarchyGroupNodes = new Dictionary<String, CustomTreeNode>();
        Dictionary<String, CustomTreeNode> m_MeasuresGroupNodes = new Dictionary<String, CustomTreeNode>();
    }
}