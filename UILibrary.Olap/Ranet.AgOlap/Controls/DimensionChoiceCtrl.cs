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
using Ranet.AgOlap.Controls.General.ClientServer;
using Ranet.AgOlap.Commands;
using Ranet.Olap.Core;
using System.Collections.Generic;

namespace Ranet.AgOlap.Controls
{
    public class DimensionChoiceCtrl : AgTreeControlBase
    {
        /// <summary>
        /// Указывает необходимость отображать информация о всех типах кубов (Cube, Dimension, Unknown)
        /// </summary>
        public bool ShowAllCubes;

        CustomTree Tree = null;

        public DimensionChoiceCtrl()
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
            DimensionTreeNode node = e.NewValue as DimensionTreeNode;
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

        private DimensionInfo m_SelectedInfo = null;
        public DimensionInfo SelectedInfo
        {
            get {
                return m_SelectedInfo;
            }
        }

        public void Initialize()
        {
            RefreshTree();
        }

        private void RefreshTree()
        {
            Tree.Items.Clear();

            if (String.IsNullOrEmpty(CubeName))
            {
                LogManager.LogMessage(Localization.DimensionChoiceControl_Name, Localization.Error + "! " + String.Format(Localization.ControlPropertyNotInitialized_Message, Localization.CubeName_PropertyDesc));
                return;
            }

            CreateRootCubeNode();
        }

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
                if (String.IsNullOrEmpty(CubeName))
                {
                    cubeNode.IsWaiting = false;
                    LogManager.LogMessage(Localization.DimensionChoiceControl_Name, Localization.Error + "! " + String.Format(Localization.ControlPropertyNotInitialized_Message, Localization.CubeName_PropertyDesc));
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
                        // Измерения будут конечными узлами. Двойной клик на них будет равнозначен выбору
                        dimNode.Expanded += new RoutedEventHandler(node_Expanded);
                        dimNode.Collapsed += new RoutedEventHandler(node_Collapsed);
                        if (parentNode == null)
                            Tree.Items.Add(dimNode);
                        else
                            parentNode.Items.Add(dimNode);
                    }
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
                LogManager.LogException(Localization.DimensionChoiceControl_Name, e.Error);
                return;
            }

            if (e.Result.ContentType == InvokeContentType.Error)
            {
                ShowErrorInTree(parentNode);
                LogManager.LogMessage(Localization.DimensionChoiceControl_Name, Localization.Error + "! " + e.Result.Content);
                return;
            }

            if (wrapper != null)
            {
                switch (wrapper.Schema.QueryType)
                {
                    case MetadataQueryType.GetDimensions:
                        GetDimensions_InvokeCommandCompleted(e, parentNode);
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
            DimensionTreeNode node = null;
            node = Tree.SelectedItem as DimensionTreeNode;

            if (node != null)
            {
                //Запоминаем выбранный элемент
                m_SelectedInfo = node.Info as DimensionInfo;
            }
        }
    }
}