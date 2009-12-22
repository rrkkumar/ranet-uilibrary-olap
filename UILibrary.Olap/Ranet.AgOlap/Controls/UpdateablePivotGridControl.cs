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
using Ranet.Olap.Core;
using Ranet.Olap.Core.Data;
using Ranet.AgOlap.Commands;
using System.Collections.Generic;
using Ranet.AgOlap.Controls.PivotGrid.Data;
using Ranet.AgOlap.Controls.General;
using System.Windows.Controls.Primitives;
using Ranet.AgOlap.Controls.DataSourceInfo;
using Ranet.AgOlap.Features;
using System.IO.IsolatedStorage;
using System.IO;
using Ranet.AgOlap.Controls.Buttons;
using Ranet.AgOlap.Controls.ToolBar;
using Ranet.AgOlap.Controls.PivotGrid.Controls;
using Ranet.AgOlap.Controls.PivotGrid;
using System.Threading;
using Ranet.AgOlap.Controls.Forms;
using Ranet.AgOlap.Controls.PivotGrid.Conditions;
using Ranet.AgOlap.Controls.ValueDelivery;
using System.Globalization;
using System.Text;
using Ranet.Olap.Core.Metadata;
using Ranet.AgOlap.Controls.General.ClientServer;
using Ranet.AgOlap.Controls.MemberChoice.ClientServer;
using Ranet.AgOlap.Controls.ValueCopy;
using Ranet.Olap.Core.Providers;
using Ranet.Olap.Core.Providers.ClientServer;

namespace Ranet.AgOlap.Controls
{
    public class UpdateablePivotGridControl : AgControlBase
    {
        //private bool m_IsWaiting = false;
        //public bool IsWaiting
        //{
        //    get { 
        //        return m_IsWaiting; 
        //    }
        //    set {

        //        if (m_IsWaiting != value)
        //        {
        //            if (value)
        //            {
        //                this.Cursor = Cursors.Wait;
        //            }
        //            else
        //            {
        //                this.Cursor = Cursors.Arrow;    
        //            }
        //            m_IsWaiting = value;
        //        }
        //    }
        //}

        //ScrollablePivotGrid m_ScrollableDataControl = null;
        //protected ScrollablePivotGrid ScrollableDataControl
        //{
        //    get
        //    {
        //        return m_ScrollableDataControl;
        //    }
        //}

        //PivotGridPanel m_PivotGridPanel;
        PivotGridControl m_PivotGrid;
        public PivotGridControl PivotGrid
        {
            get
            {
                return m_PivotGrid;
            }
        }

        //public PivotGridPanel PivotPanelControl
        //{
        //    get
        //    {
        //        return m_PivotGridPanel;
        //    }
        //}

        Grid LayoutRoot = new Grid();
        RanetToolBar ToolBar = new RanetToolBar();

        RanetToolBarButton RefreshButton = null;
        protected RanetToolBarButton BackButton = null;
        protected RanetToolBarButton ForwardButton = null;
        protected RanetToolBarButton ToBeginButton = null;
        protected RanetToolBarButton ToEndButton = null;
        protected RanetToolBarSplitter NavigationToolBarSplitter = null;
        RanetToolBarButton GoToFocusedCellButton = null;

        RanetToolBarButton RestoreDefaultSizeButton = null;
        RanetToggleButton HideEmptyColumnsButton = null;
        RanetToggleButton HideEmptyRowsButton = null;
        RanetToggleButton EditButton = null;
        RanetToggleButton UseChangesCasheButton = null;
        RanetToolBarButton ConfirmEditButton = null;
        RanetToolBarButton CancelEditButton = null;
        RanetToolBarButton ExportToExcelButton = null;
        RanetToolBarButton CopyToClipboardButton = null;
        RanetToolBarButton PasteFromClipboardButton = null;
        RanetToggleButton RotateAxesButton = null;
        RanetToggleButton HideHintsButton = null;
        ZoomingToolBarControl ZoomControl = null;


        public event RoutedEventHandler ExportToExcelClick;
        protected void OnExportToExcelClick()
        {
            if (ExportToExcelClick != null)
                ExportToExcelClick(this, new RoutedEventArgs());
        }

        bool m_ShowToolBar = true;
        public bool ShowToolBar
        {
            get { return m_ShowToolBar; }
            set
            {
                m_ShowToolBar = value;
                if (value)
                {
                    ToolBar.Visibility = Visibility.Visible;
                }
                else
                { ToolBar.Visibility = Visibility.Collapsed; }
            }
        }

        bool m_IsWaiting = false;
        protected bool IsWaiting
        {
            get { return m_IsWaiting; }
            set {
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
                    this.UpdateLayout();
                }
            }
        }

        public bool UseModifyDataConfirm = false;


        Grid grdIsWaiting;
        public UpdateablePivotGridControl()
        {
            RowDefinition row0 = new RowDefinition();
            row0.Height = GridLength.Auto;
            RowDefinition row1 = new RowDefinition();

            LayoutRoot.RowDefinitions.Add(row0);
            LayoutRoot.RowDefinitions.Add(row1);

            RefreshButton = new RanetToolBarButton();
            RefreshButton.Click += new RoutedEventHandler(RefreshButton_Click);
            RefreshButton.Content = UiHelper.CreateIcon(UriResources.Images.Refresh16);
            ToolTipService.SetToolTip(RefreshButton, Localization.PivotGrid_RefreshButton_ToolTip);

            ToBeginButton = new RanetToolBarButton();
            ToBeginButton.Click += new RoutedEventHandler(ToBeginButton_Click);
            ToBeginButton.Content = UiHelper.CreateIcon(UriResources.Images.ToBegin16);
            ToolTipService.SetToolTip(ToBeginButton, Localization.PivotGrid_ToBeginButton_ToolTip);

            BackButton = new RanetToolBarButton();
            BackButton.Click += new RoutedEventHandler(BackButton_Click);
            BackButton.Content = UiHelper.CreateIcon(UriResources.Images.Back16);
            ToolTipService.SetToolTip(BackButton, Localization.PivotGrid_BackButton_ToolTip);

            ForwardButton = new RanetToolBarButton();
            ForwardButton.Click += new RoutedEventHandler(ForwardButton_Click);
            ForwardButton.Content = UiHelper.CreateIcon(UriResources.Images.Forward16);
            ToolTipService.SetToolTip(ForwardButton, Localization.PivotGrid_ForwardButton_ToolTip);

            ToEndButton = new RanetToolBarButton();
            ToEndButton.Click += new RoutedEventHandler(ToEndButton_Click);
            ToEndButton.Content = UiHelper.CreateIcon(UriResources.Images.ToEnd16);
            ToolTipService.SetToolTip(ToEndButton, Localization.PivotGrid_ToEndButton_ToolTip);

            EditButton = new RanetToggleButton();
            ToolTipService.SetToolTip(EditButton, Localization.PivotGrid_EditButton_ToolTip);
            EditButton.Checked += new RoutedEventHandler(EditButton_Click);
            EditButton.Unchecked += new RoutedEventHandler(EditButton_Click);
            EditButton.Content = UiHelper.CreateIcon(UriResources.Images.EditCells16);

            UseChangesCasheButton = new RanetToggleButton();
            ToolTipService.SetToolTip(UseChangesCasheButton, Localization.PivotGrid_UseChangesCasheButton_ToolTip);
            UseChangesCasheButton.Checked += new RoutedEventHandler(UseChangesCasheButton_Checked);
            UseChangesCasheButton.Unchecked += new RoutedEventHandler(UseChangesCasheButton_Checked);
            UseChangesCasheButton.Content = UiHelper.CreateIcon(UriResources.Images.UseChangesCache16);

            CopyToClipboardButton = new RanetToolBarButton();
            ToolTipService.SetToolTip(CopyToClipboardButton, Localization.PivotGrid_CopyToClipboardButton_ToolTip);
            CopyToClipboardButton.Click += new RoutedEventHandler(CopyToClipboardoButton_Click);
            CopyToClipboardButton.Content = UiHelper.CreateIcon(UriResources.Images.Copy16);

            PasteFromClipboardButton = new RanetToolBarButton();
            ToolTipService.SetToolTip(PasteFromClipboardButton, Localization.PivotGrid_PasteFromClipboardButton_ToolTip);
            PasteFromClipboardButton.Click += new RoutedEventHandler(PasteFromClipboardoButton_Click);
            PasteFromClipboardButton.Content = UiHelper.CreateIcon(UriResources.Images.Paste16);

            ConfirmEditButton = new RanetToolBarButton(UriResources.Images.ConfirmEdit16, Localization.PivotGrid_ConfirmEditButton_Caption);
            ToolTipService.SetToolTip(ConfirmEditButton, Localization.PivotGrid_ConfirmEditButton_ToolTip);
            ConfirmEditButton.Click += new RoutedEventHandler(ConfirmEditButton_Click);

            CancelEditButton = new RanetToolBarButton();
            ToolTipService.SetToolTip(CancelEditButton, Localization.PivotGrid_CancelEditButton_ToolTip);
            CancelEditButton.Click += new RoutedEventHandler(CancelEditButton_Click);
            CancelEditButton.Content = UiHelper.CreateIcon(UriResources.Images.CancelEdit16);

            RestoreDefaultSizeButton = new RanetToolBarButton();
            ToolTipService.SetToolTip(RestoreDefaultSizeButton, Localization.PivotGrid_RestoreDefaultSize_ToolTip);
            RestoreDefaultSizeButton.Click += new RoutedEventHandler(RestoreDefaultSizeButton_Click);
            RestoreDefaultSizeButton.Content = UiHelper.CreateIcon(UriResources.Images.RestoreSize16);

            HideEmptyRowsButton = new RanetToggleButton();
            HideEmptyRowsButton.Click += new RoutedEventHandler(HideEmptyRowsButton_Click);
            HideEmptyRowsButton.Content = UiHelper.CreateIcon(UriResources.Images.HideEmptyRows16);
            ToolTipService.SetToolTip(HideEmptyRowsButton, Localization.PivotGrid_HideEmptyRowsButton_ToolTip);

            HideEmptyColumnsButton = new RanetToggleButton();
            HideEmptyColumnsButton.ClickMode = ClickMode.Press;
            HideEmptyColumnsButton.Click += new RoutedEventHandler(HideEmptyColumnsButton_Click);
            HideEmptyColumnsButton.Content = UiHelper.CreateIcon(UriResources.Images.HideEmptyColumns16);
            ToolTipService.SetToolTip(HideEmptyColumnsButton, Localization.PivotGrid_HideEmptyColumnsButton_ToolTip);

            GoToFocusedCellButton = new RanetToolBarButton();
            GoToFocusedCellButton.Click += new RoutedEventHandler(GoToFocusedCellButton_Click);
            GoToFocusedCellButton.Content = UiHelper.CreateIcon(UriResources.Images.ToFocused16);
            ToolTipService.SetToolTip(GoToFocusedCellButton, Localization.PivotGrid_GoToFocusedCellButton_ToolTip);

            ExportToExcelButton = new RanetToolBarButton();
            ToolTipService.SetToolTip(ExportToExcelButton, Localization.PivotGrid_ExportToExcelButton_ToolTip);
            ExportToExcelButton.Click += new RoutedEventHandler(ExportToExcelButton_Click);
            ExportToExcelButton.Content = UiHelper.CreateIcon(UriResources.Images.ExportToExcel16);

            RotateAxesButton = new RanetToggleButton();
            RotateAxesButton.ClickMode = ClickMode.Press;
            RotateAxesButton.Click += new RoutedEventHandler(RotateAxesButton_Click);
            RotateAxesButton.Content = UiHelper.CreateIcon(UriResources.Images.RotateAxes16);
            ToolTipService.SetToolTip(RotateAxesButton, Localization.PivotGrid_RotateAxesButton_ToolTip);

            HideHintsButton = new RanetToggleButton();
            HideHintsButton.ClickMode = ClickMode.Press;
            HideHintsButton.Click += new RoutedEventHandler(HideHintsButton_Click);
            HideHintsButton.Content = UiHelper.CreateIcon(UriResources.Images.HideHint16);
            ToolTipService.SetToolTip(HideHintsButton, Localization.PivotGrid_HideHintsButton_ToolTip);

            ZoomControl = new ZoomingToolBarControl();
            ToolTipService.SetToolTip(ZoomControl, Localization.PivotGrid_ZoomingControl_ToolTip);
            ZoomControl.ValueChanged += new EventHandler(ZoomControl_ValueChanged);

            ToolBar.AddItem(RefreshButton);
            ToolBar.AddItem(m_NavigationButtons_Splitter);
            ToolBar.AddItem(ToBeginButton);
            ToolBar.AddItem(BackButton);
            ToolBar.AddItem(ForwardButton);
            ToolBar.AddItem(ToEndButton);
            NavigationToolBarSplitter = new RanetToolBarSplitter();
            ToolBar.AddItem(NavigationToolBarSplitter);
            ToolBar.AddItem(EditButton);
            ToolBar.AddItem(CopyToClipboardButton);
            ToolBar.AddItem(PasteFromClipboardButton);
            ToolBar.AddItem(UseChangesCasheButton);
            ToolBar.AddItem(ConfirmEditButton);
            ToolBar.AddItem(CancelEditButton);
            ToolBar.AddItem(new RanetToolBarSplitter());
            ToolBar.AddItem(RestoreDefaultSizeButton);
            ToolBar.AddItem(HideEmptyRowsButton);
            ToolBar.AddItem(HideEmptyColumnsButton);
            ToolBar.AddItem(GoToFocusedCellButton);
            ToolBar.AddItem(RotateAxesButton);
            ToolBar.AddItem(HideHintsButton);
            ToolBar.AddItem(new RanetToolBarSplitter());
            ToolBar.AddItem(ExportToExcelButton);
            ToolBar.AddItem(ZoomControl);

            LayoutRoot.Children.Add(ToolBar);
            ToolBar.Margin = new Thickness(0, 0, 0, 3);

            // Сводная таблица
            //m_PivotGridPanel = new PivotGridPanel();
            //LayoutRoot.Children.Add(m_PivotGridPanel);
            //Grid.SetRow(m_PivotGridPanel, 1);

            m_PivotGrid = GetPivotGridControl();
            LayoutRoot.Children.Add(m_PivotGrid);
            Grid.SetRow(m_PivotGrid, 1);

            PivotGrid.DrillDownMember += new MemberActionEventHandler(PivotGrid_DrillDownMember);
            PivotGrid.CellValueChanged += new CellValueChangedEventHandler(CellsControl_CellValueChanged);
            PivotGrid.UndoCellChanges += new EventHandler(CellsControl_UndoCellChanges);
            PivotGrid.Cells_ContextMenuCreated += new EventHandler(CellsControl_ContextMenuCreated);
            PivotGrid.Columns_ContextMenuCreated += new EventHandler(ColumnsControl_ContextMenuCreated);
            PivotGrid.Rows_ContextMenuCreated += new EventHandler(RowsControl_ContextMenuCreated);

            UpdateToolbarButtons(null);
            UpdateEditToolBarButtons();

            grdIsWaiting = new Grid() { Background = new SolidColorBrush(Color.FromArgb(125, 0xFF, 0xFF, 0xFF)) };
            grdIsWaiting.Visibility = Visibility.Collapsed;
            BusyControl m_Waiting = new BusyControl();
            m_Waiting.Text = Localization.Loading;
            grdIsWaiting.Children.Add(m_Waiting);

            LayoutRoot.Children.Add(grdIsWaiting);
            Grid.SetColumnSpan(grdIsWaiting, LayoutRoot.ColumnDefinitions.Count > 0 ? LayoutRoot.ColumnDefinitions.Count : 1);
            Grid.SetRowSpan(grdIsWaiting, LayoutRoot.RowDefinitions.Count > 0 ? LayoutRoot.RowDefinitions.Count : 1);
            this.Content = LayoutRoot;

            PivotGrid.Cells_PerformControlAction += new EventHandler<ControlActionEventArgs<CellInfo>>(CellsControl_PerformControlAction);
            PivotGrid.Members_PerformControlAction += new EventHandler<ControlActionEventArgs<MemberInfo>>(MembersArea_PerformControlAction);

            m_OlapDataLoader = GetOlapDataLoader();
            m_OlapDataLoader.DataLoaded += new EventHandler<DataLoaderEventArgs>(OlapDataLoader_DataLoaded);

            // Метод Initialize необходимо вызывать для RotVisual элемента
            // всего приложения. Перенес его в ClientApp.
            //ScrollViewerMouseWheelSupport.Initialize(this);
            //m_ScrollableDataControl.ScrollView.AddMouseWheelSupport();

            this.KeyDown += new KeyEventHandler(UpdateablePivotGridControl_KeyDown);
        }

        void UpdateablePivotGridControl_KeyDown(object sender, KeyEventArgs e)
        {
            ServiceCommandType service_Command = ServiceCommandType.None;
            switch (e.Key)
            {
                case Key.Right:
                    // Ctrl+Right - навигация на одну запись в истории вперед
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control &&
                        (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift)
                    {
                        if(ForwardButton.IsEnabled)
                            service_Command = ServiceCommandType.Forward;
                    }
                    // Ctrl+Shift+Right - навигация на последнюю запись в истории
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control &&
                        (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                    {
                        if (ToEndButton.IsEnabled)
                            service_Command = ServiceCommandType.ToEnd;
                    }
                    break;
                case Key.Left:
                    // Ctrl+Left - навигация на одну запись в истории назад
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control &&
                        (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift)
                    {
                        if(BackButton.IsEnabled)
                            service_Command = ServiceCommandType.Back;
                    }
                    // Ctrl+Sift+Left - навигация на первую запись в истории
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control &&
                        (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                    {
                        if(ToBeginButton.IsEnabled)
                            service_Command = ServiceCommandType.ToBegin;
                    }
                    break;
            }

            if (service_Command != ServiceCommandType.None)
            {
                RunServiceCommand(service_Command);
                e.Handled = true;
            }
        }

        void ZoomControl_ValueChanged(object sender, EventArgs e)
        {
            PivotGrid.Scale = ZoomControl.Value / 100;
        }

        void HideHintsButton_Click(object sender, RoutedEventArgs e)
        {
            PivotGrid.Cells_UseHint = PivotGrid.Rows_UseHint = PivotGrid.Columns_UseHint = !HideHintsButton.IsChecked.Value;
        }

        protected virtual IDataLoader GetOlapDataLoader()
        {
            return new OlapDataLoader(URL);
        }

        protected virtual PivotGridControl GetPivotGridControl()
        {
            return new PivotGridControl();
        }

        void RotateAxesButton_Click(object sender, RoutedEventArgs e)
        {
            if (RotateAxesButton.IsChecked.Value)
            {
                PivotGrid.AxisIsRotated = true;
                RunServiceCommand(ServiceCommandType.RotateAxes);
            }
            else
            {
                PivotGrid.AxisIsRotated = false;
                RunServiceCommand(ServiceCommandType.NormalAxes);
            }
        }

        public override string URL
        {
            get
            {
                return base.URL;
            }
            set
            {
                base.URL = value;

                OlapDataLoader metadataLoader = OlapDataLoader as OlapDataLoader;
                if (metadataLoader != null)
                {
                    metadataLoader.URL = value;
                }
            }
        }

        void GoToFocusedCellButton_Click(object sender, RoutedEventArgs e)
        {
            PivotGrid.GoToFocusedCell();
        }

        void PivotGrid_DrillDownMember(object sender, MemberActionEventArgs args)
        {
            // Если в кэше есть изменения, то нужно спросить об их сохранении
            if (UseChangesCashe && m_CellChanges.CellChanges.Count > 0)
            {
                MessageBox.Show(Localization.PivotGrid_SaveCachedChanges, Localization.MessageBox_Warning, MessageBoxButton.OK);
                //PopUpQuestionDialog dlg = SaveChangesDlg;
                //dlg.DialogClosed += new EventHandler<Ranet.AgOlap.Controls.Forms.DialogResultArgs>(ColumnsControl_SaveChanges_DialogClosed);
                //dlg.Tag = args;
                //dlg.Show();
                return;
            }

            if (args.Axis == 0 || args.Axis == 1)
            {
                ExportSizeInfo();
                PerformMemberAction(args);
            }
        }

        void PasteFromClipboardoButton_Click(object sender, RoutedEventArgs e)
        {
            //FAST PasteCellsFromClipboard(PivotPanelControl.CellsControl.FocusedCell);
        }

        void CopyToClipboardoButton_Click(object sender, RoutedEventArgs e)
        {
            CopyCellsToClipboard();
        }

        protected bool ExportToExcelFile = true;

        void ExportToExcelButton_Click(object sender, RoutedEventArgs e)
        {
            OnExportToExcelClick();
            try
            {
                bool? ret = new bool?(true);
                if (ExportToExcelFile)
                {
                    ret = SaveFileDialog.ShowDialog();
                }
                if (ret.HasValue && ret.Value == true)
                {
                    RunServiceCommand(ServiceCommandType.ExportToExcel);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogException(Localization.PivotGridControl_Name, ex);
            }
        }

        void RestoreDefaultSizeButton_Click(object sender, RoutedEventArgs e)
        {
            ZoomControl.Value = 100;
            PivotGrid.RestoreDefaultSize();
        }

        RanetToolBarSplitter m_NavigationButtons_Splitter = new RanetToolBarSplitter();

        /// <summary>
        /// Выполняет откат изменений для указанной ячейки
        /// </summary>
        void CellsControl_UndoCellChanges(object sender, EventArgs e)
        {
            if (UseChangesCashe)
            {
                foreach (CellInfo cell in PivotGrid.Selection)
                {
                    m_CellChanges.RemoveChanges(new CellValueChangedEventArgs(cell, String.Empty));                
                }
                UpdateEditToolBarButtons();
            }
        }

        void ToBeginButton_Click(object sender, RoutedEventArgs e)
        {
            RunServiceCommand(ServiceCommandType.ToBegin);
        }

        void ToEndButton_Click(object sender, RoutedEventArgs e)
        {
            RunServiceCommand(ServiceCommandType.ToEnd);
        }

        void MembersArea_PerformControlAction(object sender, ControlActionEventArgs<MemberInfo> e)
        {
            switch (e.Action)
            {
                case ControlActionType.ShowMDX:
                    GetDataSourceInfo(null);
                    break;
                case ControlActionType.ShowProperties:
                    PropertiesDialog dlg = new PropertiesDialog();
                    dlg.PropertiesControl.Initialize(e.UserData);

                    Panel panel = GetRootPanel(this);
                    if (panel != null)
                    {
                        panel.Children.Add(dlg.Dlg.PopUpControl);
                    }

                    // На время убираем контекстное меню сводной таблицы
                    dlg.Dlg.DialogClosed += new EventHandler<DialogResultArgs>(Dlg_DialogClosed);
                    PivotGrid.UseContextMenu = false;

                    dlg.Show();
                    break;
                case ControlActionType.ShowAttributes:
                    ShowMemberAttributes(e.UserData);
                    break;
            }
        }

        Dictionary<String, List<LevelPropertyInfo>> m_LevelProperties = new Dictionary<string, List<LevelPropertyInfo>>();

        void ShowMemberAttributes(MemberInfo member)
        {
            if (member != null)
            {
                if (!m_LevelProperties.ContainsKey(member.HierarchyUniqueName))
                {
                    IsWaiting = true;
                    MetadataQuery args = FastCommandHelper.CreateLoadLevelPropertiesArgs(Connection, m_CSDescr.CubeName, String.Empty, member.HierarchyUniqueName, String.Empty);
                    OlapDataLoader.LoadData(args, new MemberInfoWrapper<MetadataQuery>(member, args));
                }
                else
                {
                    LoadMemberAttributes(member, m_LevelProperties[member.HierarchyUniqueName]);
                }
            }
        }

        void ShowMemberAttributes(MemberDataWrapper member)
        {
            if (member != null)
            {
                PropertiesDialog dlg = new PropertiesDialog();
                dlg.PropertiesControl.Initialize(member.Member);
                Panel panel = GetRootPanel(this);
                if (panel != null)
                {
                    panel.Children.Add(dlg.Dlg.PopUpControl);
                }
                dlg.Show(Localization.CustomPropertiesDialog_Caption);
            }
        }

        void LoadMemberAttributes(MemberInfo member, List<LevelPropertyInfo> properties)
        {
            IsWaiting = true;
            MemberChoiceQuery args = FastCommandHelper.CreateGetMemberArgs(Connection, m_CSDescr.CubeName, String.Empty, member.UniqueName);
            args.Properties = properties;
            OlapDataLoader.LoadData(args, new MemberInfoWrapper<MemberChoiceQuery>(member, args));
        }


        Panel GetRootPanel(FrameworkElement element)
        {
            Panel panel = null;
            while (element != null && element.Parent != null)
            {
                if (element.Parent is Panel)
                    panel = element.Parent as Panel;
                element = element.Parent as FrameworkElement;
            }
            return panel;
        }

        void CellsControl_PerformControlAction(object sender, ControlActionEventArgs<CellInfo> e)
        {
            switch (e.Action)
            {
                case ControlActionType.ShowMDX:
                    UpdateEntry entry = new UpdateEntry();
                    IDictionary<String, MemberInfo> tuple = e.UserData.GetTuple();
                    foreach (MemberInfo mi in tuple.Values)
                    {
                        ShortMemberInfo tuple_mi = new ShortMemberInfo();
                        tuple_mi.HierarchyUniqueName = mi.HierarchyUniqueName;
                        tuple_mi.UniqueName = mi.UniqueName;
                        tuple_mi.LevelDepth = mi.LevelDepth;

                        entry.Tuple.Add(tuple_mi);
                    }

                    try
                    {
                        entry.OldValue = e.UserData.CellDescr.Value.Value.ToString();
                    }
                    catch { }

                    CellValueChangedEventArgs change = m_CellChanges.FindLastChange(e.UserData);
                    if (change != null)
                    {
                        entry.NewValue = change.NewValue;
                    }

                    GetDataSourceInfo(entry);
                    break;
                case ControlActionType.ShowProperties:
                    PropertiesDialog dlg = new PropertiesDialog();
                    dlg.PropertiesControl.Initialize(e.UserData);

                    Panel panel = GetRootPanel(this);
                    if (panel != null)
                    {
                        panel.Children.Add(dlg.Dlg.PopUpControl);
                    }

                    // На время убираем контекстное меню сводной таблицы
                    dlg.Dlg.DialogClosed += new EventHandler<DialogResultArgs>(Dlg_DialogClosed);
                    PivotGrid.UseContextMenu = false;

                    dlg.Show();
                    break;
                case ControlActionType.ValueDelivery:
                    ShowValueDeliveryDialog(e.UserData);
                    break;
                case ControlActionType.ValueCopy:
                    ShowValueCopyDialog(e.UserData);
                    break;
                case ControlActionType.Copy:
                    CopyCellsToClipboard();
                    break;
                case ControlActionType.Paste:
                    PasteCellsFromClipboard(e.UserData);
                    break;
            }
        }

        #region Copy-Paste to Clipboard
        void PasteCellsFromClipboard(CellInfo cell)
        {
            // На всякий случай проверим
            if (!PivotGrid.CanEdit || !PivotGrid.EditMode)
                return;

            String text = Clipboard.GetClipboardText();
            if (!String.IsNullOrEmpty(text))
            {
                // В буфере ячейки хранятся разделенные табуляцией и переходом на новую строку. 
                //  Например ячейки (* - пустая):
                //  3   *   *   *
                //  *   *   *   *
                //  5   *   6   *
                //  *   *   *   *
                // В буфере при копировании из Excel это выглядит так: "3\t\t\t\r\n\r\n5\t\t6"

                // Буфер, в который накапливаются значения из Clipboard
                List<List<String>> buff = new List<List<string>>();

                // Разделим содержимое буфера на строки
                text = text.Trim();
                String[] rows_list = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                if (rows_list != null)
                {
                    // Разделим содержимое каждой строки
                    foreach (String str in rows_list)
                    {
                        List<String> row_Buff = new List<string>();
                        String[] columns_list = str.Split(new string[] { "\t" }, StringSplitOptions.None);
                        if (columns_list != null)
                        {
                            foreach (String val in columns_list)
                            {
                                row_Buff.Add(val);
                            }
                        }

                        buff.Add(row_Buff);
                    }
                }
                // Буфер с целью оптимизации не содержит пустые ячейки, если они вляются последними в строке.
                // Поэтому чтобы найти реальный размер копируемой области по ширине нужно взять максимальную длину из имеющихся
                int width = 0;
                foreach (List<String> row in buff)
                {
                    width = Math.Max(width, row.Count);
                }

                // Обходим буфер и меняем значения ячеек
                int row_index = 0;
                List<CellValueChangedEventArgs> changes = new List<CellValueChangedEventArgs>();
                int start_ColumnIndex = 0;
                int start_RowIndex = 0;
                if (cell != null)
                {
                    start_ColumnIndex = cell.CellDescr.Axis0_Coord;
                    start_RowIndex = cell.CellDescr.Axis1_Coord;
                }
                foreach (List<String> row in buff)
                {
                    for (int col_index = 0; col_index < width; col_index++)
                    {
                        String val = String.Empty;
                        if (col_index < row.Count)
                            val = row[col_index];

                        // Пытаемся по координате получить ячейку
                        CellInfo destination_cell = m_CellSetProvider.GetCellInfo(start_ColumnIndex + col_index, start_RowIndex + row_index);
                        //FAST CellControl destination_cell = PivotPanelControl.CellsControl.GetCell(start_ColumnIndex + col_index, start_RowIndex + row_index);
                        if (destination_cell != null && destination_cell.IsUpdateable)
                        {
                            if (String.IsNullOrEmpty(val))
                            {
                                // Крахотко Саша 07.04.2009 (11:46):
                                // записывать НОЛЬ только в том случае если у нас было какое-то число, если числа не было (в ячейке было null) то оставлять null
                                if (destination_cell.Value != null || destination_cell.IsModified)
                                {
                                    PivotGrid.ChangeCell(destination_cell, "0");
                                    CellValueChangedEventArgs e = new CellValueChangedEventArgs(destination_cell, "0");
                                    changes.Add(e);
                                    continue;
                                }
                            }
                            else
                            {
                                try
                                {
                                    // Пытаемся преобразовать строку в число
                                    double new_val = 0;
                                    new_val = Convert.ToDouble(val);

                                    if (destination_cell.Value != null)
                                    {
                                        double x = 0;
                                        x = Convert.ToDouble(destination_cell.Value.ToString());
                                        if (x == new_val)
                                            continue;
                                    }

                                    PivotGrid.ChangeCell(destination_cell, new_val.ToString());
                                    CellValueChangedEventArgs e = new CellValueChangedEventArgs(destination_cell, new_val.ToString());
                                    changes.Add(e);
                                }
                                catch { }
                            }
                        }
                    }
                    row_index++;
                }

                if (changes.Count > 0)
                    PerformCellChanges(changes);
            }
        }

        void CopyCellsToClipboard()
        {
            IList<CellInfo> cells = PivotGrid.Selection;
            if (cells != null && cells.Count > 0)
            {
                // Ячейки для опреации копирования должны быть в прямоугольной области. Причем область должна быть без пустот
                // Для начала определим максимальные и минимальные индексы используемых строк и столбцов
                int min_row = -1;
                int max_row = -1;
                int min_col = -1;
                int max_col = -1;

                foreach (CellInfo cell in cells)
                {
                    // Индексы для строки
                    if (min_row == -1)
                    {
                        min_row = cell.CellDescr.Axis1_Coord;
                    }
                    else
                    {
                        min_row = Math.Min(min_row, cell.CellDescr.Axis1_Coord);
                    }
            
                    if (max_row == -1)
                    {
                        max_row = cell.CellDescr.Axis1_Coord;
                    }
                    else
                    {
                        max_row = Math.Max(max_row, cell.CellDescr.Axis1_Coord);
                    }
            
                    // Индексы для колонки
                    if (min_col == -1)
                    {
                        min_col = cell.CellDescr.Axis0_Coord;
                    }
                    else
                    {
                        min_col = Math.Min(min_col, cell.CellDescr.Axis0_Coord);
                    }
                    if (max_col == -1)
                    {
                        max_col = cell.CellDescr.Axis0_Coord;
                    }
                    else
                    {
                        max_col = Math.Max(max_col, cell.CellDescr.Axis0_Coord);
                    }
                }

                if (min_col > -1 && max_col > -1 && max_col >= min_col &&
                    min_row > -1 && max_row > -1 && max_row >= min_row)
                {
                    // Теперь определяем массив (двумерный) с размерностью, которую мы определили
                    // И пытаемся заполнить массив тем что есть. Если потом останутся в нем дырки (null) то область получается незамкнутая
                    int col_count = max_col - min_col + 1;
                    int row_count = max_row - min_row + 1;
                    List<List<String>> list = new List<List<string>>();
                    for (int row_index = 0; row_index < row_count; row_index++)
                    {
                        List<String> row = new List<string>();
                        for (int col_index = 0; col_index < col_count; col_index++)
                        {
                            // Зануляем все значения
                            row.Add("_NULL_");
                        }
                        list.Add(row);
                    }
                    
                    // Разбрасываем значения ячеек на свои места
                    foreach (CellInfo cell in cells)
                    {
                        int row_indx = cell.CellDescr.Axis1_Coord - min_row;
                        int col_indx = cell.CellDescr.Axis0_Coord - min_col;
                        if (row_indx >= 0 && row_indx < row_count &&
                            col_indx >= 0 && col_indx < col_count)
                        {
                            if (cell.IsModified)
                            {
                                list[row_indx][col_indx] = cell.ModifiedValue;
                            }
                            else
                            {
                                if (cell.Value != null)
                                    list[row_indx][col_indx] = cell.Value.ToString();
                                else
                                    list[row_indx][col_indx] = null;
                            }
                        }
                    }

                    bool has_dyrki = false;
                    // Определяем есть ли дырки
                    foreach (List<String> rows_list in list)
                    {
                        foreach (String str in rows_list)
                        {
                            if (str == "_NULL_")
                            {
                                has_dyrki = true;
                                break;
                            }
                        }
                    }

                    // Если есть дырки - значит копировать не судьба
                    if (has_dyrki)
                    {
                        MessageBox.Show(Localization.PivotGrid_CellsCopyAreaError, Localization.MessageBox_Warning, MessageBoxButton.OK);
                        return;
                    }

                    StringBuilder sb = new StringBuilder();
                    // Формируем результат. Значения размеляются \t 
                    // Строки разделяются \r\n
                    // Определяем есть ли дырки
                    foreach (List<String> rows_list in list)
                    {
                        int i = 0;
                        foreach (String str in rows_list)
                        {
                            if (i != 0)
                                sb.Append("\t");
                            sb.Append(str);
                            i++;
                        }
                        sb.Append(Environment.NewLine);
                    }

                    Clipboard.SetClipboardText(sb.ToString());
                }
            }
        }
        #endregion Copy-Paste to Clipboard

        #region Копирование данных
        protected virtual void ShowValueCopyDialog(CellInfo cell)
        {
            if (m_CSDescr != null && cell != null && cell.IsUpdateable)
            {
                // Если в кэше есть изменения, то нужно спросить об их сохранении
                if (UseChangesCashe && m_CellChanges.CellChanges.Count > 0)
                {
                    MessageBox.Show(Localization.PivotGrid_SaveCachedChanges, Localization.MessageBox_Warning, MessageBoxButton.OK);
                    return;
                }

                IDictionary<String, MemberWrap> slice = new Dictionary<String, MemberWrap>();
                IDictionary<String, MemberInfo> tuple = cell.GetTuple();
                foreach (String hierarchyUniqueName in tuple.Keys)
                {
                    slice.Add(hierarchyUniqueName, new MemberWrap(tuple[hierarchyUniqueName]));
                }

                ModalDialog dlg = new ModalDialog();
                dlg.MinHeight = 300;
                dlg.MinWidth = 400;
                dlg.Height = 500;
                dlg.Width = 600;
                dlg.Caption = Localization.ValueCopyDialog_Caption;
                dlg.DialogOk += new EventHandler<DialogResultArgs>(ValueCopyDialog_OkButtonClick);

                ValueCopyControl CopyControl = new ValueCopyControl();
                CopyControl.CubeName = m_CSDescr.CubeName;
                CopyControl.ConnectionID = m_CSDescr.Connection.ConnectionID;
                CopyControl.LoadMetaData += new EventHandler(ValueCopyControl_LoadMetaData);
                CopyControl.GetOlapDataLoader += new EventHandler<GetIDataLoaderArgs>(CopyControl_GetMetadataLoader);
                CopyControl.LogManager = this.LogManager;
                CopyControl.Initialize(slice, cell.Value);
                dlg.Content = CopyControl;

                // На время убираем контекстное меню сводной таблицы
                dlg.DialogClosed += new EventHandler<DialogResultArgs>(Dlg_DialogClosed);
                PivotGrid.UseContextMenu = false;

                dlg.Show();
            }
        }

        void CopyControl_GetMetadataLoader(object sender, GetIDataLoaderArgs e)
        {
            e.Loader = GetOlapDataLoader();
            e.Handled = true;
        }

        CubeDefInfo m_CubeMetaData = null;

        void ValueCopyControl_LoadMetaData(object sender, EventArgs e)
        {
            ValueCopyControl copyControl = sender as ValueCopyControl;
            if (copyControl != null)
            {
                if (m_CubeMetaData == null)
                {
                    copyControl.IsBusy = true;
                    MetadataQuery args = FastCommandHelper.CreateGetCubeMetadataArgs(Connection, m_CSDescr.CubeName, MetadataQueryType.GetCubeMetadata_AllMembers);
                    OlapDataLoader.LoadData(args, sender);
                }
                else
                {
                    copyControl.IsBusy = false;
                    copyControl.InitializeMetadata(m_CubeMetaData);
                }
            }
        }

        void ValueCopyDialog_OkButtonClick(object sender, DialogResultArgs e)
        {
            ModalDialog dlg = sender as ModalDialog;
            if (dlg != null)
            {
                ValueCopyControl copyControl = dlg.Content as ValueCopyControl;
                if (copyControl != null)
                {
                    String query = copyControl.BuildCopyScript();
                    if (!String.IsNullOrEmpty(query))
                    {
                        MdxQueryArgs args = FastCommandHelper.CreateMdxQueryArgs(Connection, query);
                        args.PivotID = this.GetHashCode().ToString();
                        args.Type = QueryExecutingType.NonQuery;
                        IsWaiting = true;
                        OlapDataLoader.LoadData(args, "ValueCopyDialog_OkButton");
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
            }
        }
        #endregion Копирование данных

        #region Разноска данных
        protected virtual void ShowValueDeliveryDialog(CellInfo cell)
        {
            if (m_CSDescr != null && cell != null && cell.IsUpdateable)
            {
                // Если в кэше есть изменения, то нужно спросить об их сохранении
                if (UseChangesCashe && m_CellChanges.CellChanges.Count > 0)
                {
                    MessageBox.Show(Localization.PivotGrid_SaveCachedChanges, Localization.MessageBox_Warning, MessageBoxButton.OK);
                    return;
                }

                ModalDialog dlg = new ModalDialog();
                dlg.MinHeight = 300;
                dlg.MinWidth = 400;
                dlg.Height = 500;
                dlg.Width = 600;
                dlg.Caption = Localization.ValueDeliveryDialog_Caption;
                dlg.DialogOk += new EventHandler<DialogResultArgs>(ValueDeliveryDialog_OkButtonClick);

                ValueDeliveryControl DeliveryControl = new ValueDeliveryControl();
                DeliveryControl.CubeName = m_CSDescr.CubeName;
                DeliveryControl.ConnectionID = m_CSDescr.Connection.ConnectionID;
                DeliveryControl.LoadMembers += new EventHandler<QueryEventArgs>(DeliveryControl_LoadMembers);
                DeliveryControl.Initialize(cell);
                dlg.Content = DeliveryControl;

                // На время убираем контекстное меню сводной таблицы
                dlg.DialogClosed += new EventHandler<DialogResultArgs>(Dlg_DialogClosed);
                PivotGrid.UseContextMenu = false;

                dlg.Show();
            }
        }

        void ValueDeliveryDialog_OkButtonClick(object sender, DialogResultArgs e)
        {
            ModalDialog dlg = sender as ModalDialog;
            if (dlg != null)
            {
                ValueDeliveryControl DeliveryControl = dlg.Content as ValueDeliveryControl;
                if (DeliveryControl != null && DeliveryControl.Cell != null)
                {
                    if (DeliveryControl.IsDelivered)
                    {
                        double new_Val = DeliveryControl.OriginalValue - DeliveryControl.DeliveredValue;
                        String value = new_Val.ToString();
                        // В качестве разделителя для числа обязательно должна использоватьеся точка (т.к. эта строка будет помещена в МDX)
                        value = value.Replace(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator, ".");

                        List<UpdateEntry> entries = new List<UpdateEntry>();
                        UpdateEntry entry = BuildUpdateEntry(new CellValueChangedEventArgs(DeliveryControl.Cell, value));
                        DeliveryControl.GetDeliveredMembers();
                        entries.Add(entry);

                        IList<MemberItem> changes = DeliveryControl.GetDeliveredMembers();
                        foreach (MemberItem item in changes)
                        {
                            value = item.NewValue.ToString();
                            // В качестве разделителя для числа обязательно должна использоватьеся точка (т.к. эта строка будет помещена в МDX)
                            value = value.Replace(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator, ".");

                            UpdateEntry item_entry = new UpdateEntry();
                            IDictionary<String, MemberInfo> tuple = DeliveryControl.Cell.GetTuple();
                            foreach (MemberInfo mi in tuple.Values)
                            {
                                ShortMemberInfo tuple_mi = new ShortMemberInfo();
                                tuple_mi.HierarchyUniqueName = mi.HierarchyUniqueName;
                                if (mi.HierarchyUniqueName != item.Member.HierarchyUniqueName)
                                {
                                    tuple_mi.UniqueName = mi.UniqueName;
                                    tuple_mi.LevelDepth = mi.LevelDepth;
                                }
                                else
                                {
                                    tuple_mi.UniqueName = item.Member.UniqueName;
                                    tuple_mi.LevelDepth = item.Member.LevelDepth;
                                }
                                item_entry.Tuple.Add(tuple_mi);
                            }
                            item_entry.NewValue = value;
                            try
                            {
                                item_entry.OldValue = item.Cell.CellDescr.Value.Value.ToString();
                            }
                            catch
                            {
                            }

                            entries.Add(item_entry);
                        }
                        SaveChanges(entries);
                    }
                }
            }
        }

        void DeliveryControl_LoadMembers(object sender, QueryEventArgs e)
        {
            if (e != null && !String.IsNullOrEmpty(e.Query))
            {
                MdxQueryArgs args = FastCommandHelper.CreateMdxQueryArgs(Connection, e.Query);
                args.PivotID = this.GetHashCode().ToString();
                OlapDataLoader.LoadData(args, sender);
            }
        }
        #endregion Разноска данных

        void ShowDataSourceInfo(DataSourceInfoArgs args)
        {
            if (args != null)
            {
                ModalDialog dlg = new ModalDialog();
                dlg.Caption = Localization.DataSourceInfoDialog_Caption;
                dlg.MinHeight = 300;
                dlg.MinWidth = 400;
                dlg.Height = 400;
                dlg.Width = 500;

                DataSourceInfoControl DSInfo = new DataSourceInfoControl();
                dlg.Content = DSInfo;
                DSInfo.UpdateScriptVisibility = PivotGrid.CanEdit;
                DSInfo.Initialize(args);

                // На время убираем контекстное меню сводной таблицы
                dlg.DialogClosed += new EventHandler<DialogResultArgs>(Dlg_DialogClosed);
                PivotGrid.UseContextMenu = false;

                dlg.Show();
            }
        }

        void Dlg_DialogClosed(object sender, DialogResultArgs e)
        {
            PivotGrid.UseContextMenu = true;
        }

        /// <summary>
        /// В случае необходимости выводит диалог с запросом на сохранение изменений
        /// </summary>
        //private PopUpQuestionDialog SaveChangesDlg
        //{
        //    get
        //    {
        //        PopUpQuestionDialog dlg = new PopUpQuestionDialog();
        //        dlg.Caption = "Warning...";
        //        dlg.ContentCtrl.Text = "Cache contains unsaved data! Save cached changes?";
        //        dlg.ContentCtrl.DialogType = DialogButtons.YesNo;
        //        return dlg;
        //    }
        //}


        void GetDataSourceInfo(UpdateEntry userData)
        {
            ServiceCommandArgs args = new ServiceCommandArgs(ServiceCommandType.GetDataSourceInfo, userData);
            args.PivotID = this.GetHashCode().ToString();
            IsWaiting = true;
            OlapDataLoader.LoadData(args, args);
        }

        void dlg_CloseDialog(DialogResult e)
        {
            if (e == DialogResult.Yes)
            {
                SaveChanges(m_CellChanges.CellChanges);
                return;
            }
            if (e == DialogResult.No)
            {
                CancelChanges();
                return;
            }
        }

        UpdateEntry BuildUpdateEntry(CellValueChangedEventArgs arg)
        {
            UpdateEntry entry = null;
            if (arg != null)
            {
                entry = new UpdateEntry();

                IDictionary<String, MemberInfo> tuple = arg.Cell.GetTuple();
                foreach (MemberInfo mi in tuple.Values)
                {
                    ShortMemberInfo tuple_mi = new ShortMemberInfo();
                    tuple_mi.HierarchyUniqueName = mi.HierarchyUniqueName;
                    tuple_mi.UniqueName = mi.UniqueName;
                    tuple_mi.LevelDepth = mi.LevelDepth;

                    entry.Tuple.Add(tuple_mi);
                }
                entry.NewValue = arg.NewValue;
                try
                {
                    entry.OldValue = arg.Cell.CellDescr.Value.Value.ToString();
                }
                catch
                {
                }
            }
            return entry;
        }

        void SaveChanges(List<CellValueChangedEventArgs> changes)
        {
            List<UpdateEntry> entries = new List<UpdateEntry>();

            if (changes != null)
            {
                foreach (CellValueChangedEventArgs arg in changes)
                {
                    UpdateEntry entry = BuildUpdateEntry(arg);
                    if (entry != null)
                    {
                        entries.Add(entry);
                    }
                }
            }

            SaveChanges(entries);
        }

        void SaveChanges(List<UpdateEntry> entries)
        {
            if (entries != null && entries.Count > 0)
            {
                RunUpdateCubeCommand(entries);
            }
            UpdateEditToolBarButtons();
        }

        void CancelChanges()
        {
            if (m_CellChanges.CellChanges.Count > 0)
            {
                m_CellChanges.CellChanges.Clear();
                UpdateEditToolBarButtons();

                RunServiceCommand(ServiceCommandType.Refresh);
            }
        }

        void CancelEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (UseModifyDataConfirm)
            {
                if (MessageBox.Show(Localization.PivotGrid_CancelEditButton_Question, Localization.Confirmation, MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                    return;
            }
            CancelChanges();
        }

        void ConfirmEditButton_Click(object sender, RoutedEventArgs e)
        {
            if(UseModifyDataConfirm)
            {
                if(MessageBox.Show(Localization.PivotGrid_ConfirmEditButton_Question, Localization.Confirmation, MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                    return;
            }
            ExportSizeInfo();
            SaveChanges(m_CellChanges.CellChanges);
        }

        void UseChangesCasheButton_Checked(object sender, RoutedEventArgs e)
        {
            // Если в кэше есть изменения, то нужно спросить об их сохранении
            if (UseChangesCashe && m_CellChanges.CellChanges.Count > 0)
            {
                MessageBox.Show(Localization.PivotGrid_SaveCachedChanges, Localization.MessageBox_Warning, MessageBoxButton.OK);
                //PopUpQuestionDialog dlg = SaveChangesDlg;
                //dlg.DialogClosed += new EventHandler<Ranet.AgOlap.Controls.Forms.DialogResultArgs>(UseChangesCasheButton_SaveChanges_DialogClosed);
                //dlg.Show();
                return;
            }

            if (UseChangesCashe != UseChangesCasheButton.IsChecked.Value)
            {
                UseChangesCashe = UseChangesCasheButton.IsChecked.Value;
            }
        }

        //void UseChangesCasheButton_SaveChanges_DialogClosed(object sender, Ranet.AgOlap.Controls.Forms.DialogResultArgs e)
        //{
        //    dlg_CloseDialog(e.Result);
        //    if (UseChangesCashe != UseChangesCasheButton.IsChecked.Value)
        //    {
        //        UseChangesCashe = UseChangesCasheButton.IsChecked.Value;
        //    }
        //}

        void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Если в кэше есть изменения, то нужно спросить об их сохранении
            if (UseChangesCashe && m_CellChanges.CellChanges.Count > 0)
            {
                MessageBox.Show(Localization.PivotGrid_SaveCachedChanges, Localization.MessageBox_Warning, MessageBoxButton.OK);
                //PopUpQuestionDialog dlg = SaveChangesDlg;
                //dlg.DialogClosed += new EventHandler<Ranet.AgOlap.Controls.Forms.DialogResultArgs>(EditButton_SaveChanges_DialogClosed);
                //dlg.Show();
                return;
            }

            if (EditMode != EditButton.IsChecked.Value)
            {
                PivotGrid.EditMode = EditButton.IsChecked.Value;
            }
            UpdateEditToolBarButtons(true);
        }

        //void EditButton_SaveChanges_DialogClosed(object sender, Ranet.AgOlap.Controls.Forms.DialogResultArgs e)
        //{
        //    dlg_CloseDialog(e.Result);
        //    if (EditMode != EditButton.IsChecked.Value)
        //    {
        //        ScrollableDataControl.PivotGrid.CellsControl.EditMode = EditButton.IsChecked.Value;
        //    }
        //    UpdateEditToolBarButtons();
        //}

        void HideEmptyColumnsButton_Click(object sender, RoutedEventArgs e)
        {
            if (HideEmptyColumnsButton.IsChecked.Value)
                RunServiceCommand(ServiceCommandType.HideEmptyColumns);
            else
                RunServiceCommand(ServiceCommandType.ShowEmptyColumns);
        }

        void HideEmptyRowsButton_Click(object sender, RoutedEventArgs e)
        {
            if (HideEmptyRowsButton.IsChecked.Value)
                RunServiceCommand(ServiceCommandType.HideEmptyRows);
            else
                RunServiceCommand(ServiceCommandType.ShowEmptyRows);
        }

        protected virtual void RowsControl_ContextMenuCreated(object sender, EventArgs e)
        {
        }

        protected virtual void ColumnsControl_ContextMenuCreated(object sender, EventArgs e)
        {
        }

        protected virtual void CellsControl_ContextMenuCreated(object sender, EventArgs e)
        {
        }

        void CellsControl_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            //if (!IsWaiting)
            List<CellValueChangedEventArgs> changes = new List<CellValueChangedEventArgs>();
            changes.Add(e);
            PerformCellChanges(changes);
        }

        void PerformCellChanges(List<CellValueChangedEventArgs> changes_list)
        {
            if (changes_list != null)
            {
                if (!UseChangesCashe)
                {
                    SaveChanges(changes_list);
                    return;
                }
                else
                {
                    foreach (CellValueChangedEventArgs e in changes_list)
                    {
                        m_CellChanges.RemoveChanges(e);
                        m_CellChanges.CellChanges.Add(e);
                    }
                }

                UpdateEditToolBarButtons();
            }
        }

        void RunUpdateCubeCommand(List<UpdateEntry> entries)
        {
            if (String.IsNullOrEmpty(UpdateScript))
            {
                LogManager.LogMessage(Localization.PivotGridControl_Name, Localization.Error + "! " + String.Format(Localization.ControlSettingsNotInitialized_Message, Localization.UpdateScript_PropertyDesc));
                return;
            }

            String cubeName = String.Empty;
            String connectionString = String.Empty;
            if (m_CSDescr != null)
            {
                cubeName = m_CSDescr.CubeName;
                connectionString = m_CSDescr.Connection.ConnectionString;
            }
            UpdateCubeArgs args = FastCommandHelper.CreateUpdateCubeArgs(connectionString, cubeName, entries);
            args.PivotID = this.GetHashCode().ToString();
            IsWaiting = true;
            OlapDataLoader.LoadData(args, args);
        }

        protected void UpdateButtons()
        {
            // Пока что сделаем так, чтобы не моргало. UpdateToolbarButtons(null);
            PivotGridToolBarInfo args = new PivotGridToolBarInfo();
            args.PivotID = this.GetHashCode().ToString();
            IsWaiting = true;
            OlapDataLoader.LoadData(args, args);
        }

        void UpdateToolbarButtons(PivotGridToolBarInfo info)
        {
            if (info == null)
            {
                BackButton.IsEnabled = false;
                ForwardButton.IsEnabled = false;
                ToBeginButton.IsEnabled = false;
                ToEndButton.IsEnabled = false;


                HideEmptyColumnsButton.IsChecked = false;
                HideEmptyRowsButton.IsChecked = false;
            }
            else
            {
                HideEmptyColumnsButton.IsChecked = info.HideEmptyColumns;
                HideEmptyRowsButton.IsChecked = info.HideEmptyRows;

                if (info.HistorySize > 0)
                {
                    ToBeginButton.IsEnabled = BackButton.IsEnabled = info.CurrentHistoryIndex > 0;
                    ToEndButton.IsEnabled = ForwardButton.IsEnabled = info.CurrentHistoryIndex + 1 < info.HistorySize;
                }
                else
                {
                    BackButton.IsEnabled = false;
                    ForwardButton.IsEnabled = false;
                    ToBeginButton.IsEnabled = false;
                    ToEndButton.IsEnabled = false;
                }
            }
        }

        protected virtual void PerformMemberAction(MemberActionEventArgs e)
        {
            //IsWaiting = true;
            IList<MemberInfo> full_tuple = new List<MemberInfo>();
            e.Member.CollectAncestors(full_tuple, true);

            List<ShortMemberInfo> list = new List<ShortMemberInfo>();
            foreach (MemberInfo mi in full_tuple)
            {
                ShortMemberInfo olap_mi = new ShortMemberInfo();
                olap_mi.UniqueName = mi.UniqueName;
                olap_mi.HierarchyUniqueName = mi.HierarchyUniqueName;
                olap_mi.LevelDepth = mi.LevelDepth;
                list.Add(olap_mi);
            }

            ShortMemberInfo member = new ShortMemberInfo();
            member.UniqueName = e.Member.UniqueName;
            member.HierarchyUniqueName = e.Member.HierarchyUniqueName;
            member.LevelDepth = e.Member.LevelDepth;

            PerformMemberActionArgs args = FastCommandHelper.CreatePerformMemberActionArgs(Connection,
                member, e.Axis, e.Action, list);
            args.PivotID = this.GetHashCode().ToString();
            IsWaiting = true;
            OlapDataLoader.LoadData(args, args);
        }

        IDataLoader m_OlapDataLoader = null;
        public IDataLoader OlapDataLoader
        {
            set
            {
                if (m_OlapDataLoader != null)
                {
                    m_OlapDataLoader.DataLoaded -= new EventHandler<DataLoaderEventArgs>(OlapDataLoader_DataLoaded);
                }

                m_OlapDataLoader = value;
                m_OlapDataLoader.DataLoaded += new EventHandler<DataLoaderEventArgs>(OlapDataLoader_DataLoaded);
            }
            get
            {
                return m_OlapDataLoader;
            }
        }

        void OlapDataLoader_DataLoaded(object sender, DataLoaderEventArgs e)
        {
            bool stopWaiting = true;

            try
            {
                // Exception
                if (e.Error != null)
                {
                    LogManager.LogException(Localization.PivotGridControl_Name, e.Error);
                    return;
                }

                // Exception or Message from Olap-Service
                if (e.Result.ContentType == InvokeContentType.Error)
                {
                    LogManager.LogMessage(Localization.PivotGridControl_Name, Localization.Error + "! " + e.Result.Content);
                    return;
                }

                PivotGrid.Focus();

                if (e.UserState != null && e.UserState.ToString() == "ValueCopyDialog_OkButton")
                //ValueCopyControl copy = e.UserState as ValueCopyControl;
                //if (copy != null)
                {
                    stopWaiting = false;
                    RunServiceCommand(ServiceCommandType.Refresh);
                    return;
                }


                ValueDeliveryControl dilivery = e.UserState as ValueDeliveryControl;
                if (dilivery != null)
                {
                    if (!string.IsNullOrEmpty(e.Result.Content))
                    {
                        //CellSetData cs_descr = XmlSerializationUtility.XmlStr2Obj<CellSetData>(e.Result);
                        CellSetData cs_descr = CellSetData.Deserialize(e.Result.Content);
                        dilivery.InitializeMembersList(new CellSetDataProvider(cs_descr));
                    }
                    return;
                }

                PivotInitializeArgs args = e.UserState as PivotInitializeArgs;
                if (args != null)
                {
                    if (!string.IsNullOrEmpty(e.Result.Content))
                    {
                        //CellSetData cs_descr = XmlSerializationUtility.XmlStr2Obj<CellSetData>(e.Result);
                        CellSetData cs_descr = CellSetData.Deserialize(e.Result.Content);
                        Initialize(cs_descr);
                    }
                    else
                    {
                        Initialize(null);
                    }
                    stopWaiting = false;
                    return;
                }

                PerformMemberActionArgs action_args = e.UserState as PerformMemberActionArgs;
                if (action_args != null)
                {
                    stopWaiting = false;
                    MemberActionCompleted(e);
                    return;
                }

                ServiceCommandArgs service_args = e.UserState as ServiceCommandArgs;
                if (service_args != null)
                {
                    ServiceCommandType actionType = service_args.Command;
                    if (actionType == ServiceCommandType.GetDataSourceInfo)
                    {
                        if (!string.IsNullOrEmpty(e.Result.Content))
                        {
                            DataSourceInfoArgs info = XmlSerializationUtility.XmlStr2Obj<DataSourceInfoArgs>(e.Result.Content);
                            ShowDataSourceInfo(info);
                        }
                        return;
                    }

                    // Save to File
                    if (actionType == ServiceCommandType.ExportToExcel && ExportToExcelFile)
                    {
                        SaveToFile(e.Result.Content);
                        return;
                    }

                    if (!string.IsNullOrEmpty(e.Result.Content))
                    {
                        //CellSetData cs_descr = XmlSerializationUtility.XmlStr2Obj<CellSetData>(e.Result);
                        CellSetData cs_descr = CellSetData.Deserialize(e.Result.Content);
                        Initialize(cs_descr);
                    }
                    //this.Focus();
                    stopWaiting = false;
                    UpdateButtons();
                    ServiceCommandCompleted(service_args);
                    return;
                }

                PivotGridToolBarInfo toolbar_args = e.UserState as PivotGridToolBarInfo;
                if (toolbar_args != null)
                {
                    if (!string.IsNullOrEmpty(e.Result.Content))
                    {
                        toolbar_args = XmlSerializationUtility.XmlStr2Obj<PivotGridToolBarInfo>(e.Result.Content);
                        UpdateToolbarButtons(toolbar_args);
                    }
                    //this.Focus();
                    return;
                }

                UpdateCubeArgs update_Args = e.UserState as UpdateCubeArgs;
                if (update_Args != null)
                {
                    stopWaiting = false;
                    m_CellChanges.CellChanges.Clear();
                    UpdateEditToolBarButtons();
                    UpdateCubeCompleted(update_Args);
                    //this.Focus();
                    return;
                }

                ValueCopyControl copyControl = e.UserState as ValueCopyControl;
                if (copyControl != null)
                {
                    CubeDefInfo cs_descr = XmlSerializationUtility.XmlStr2Obj<CubeDefInfo>(e.Result.Content);
                    m_CubeMetaData = cs_descr;
                    copyControl.IsBusy = false;
                    copyControl.InitializeMetadata(cs_descr);
                }

                MemberInfoWrapper<MetadataQuery> metadata_args = e.UserState as MemberInfoWrapper<MetadataQuery>;
                if (metadata_args != null)
                {
                    switch (metadata_args.UserData.QueryType)
                    {
                        case MetadataQueryType.GetLevelProperties:
                            List<LevelPropertyInfo> properties = XmlSerializationUtility.XmlStr2Obj<List<LevelPropertyInfo>>(e.Result.Content);
                            m_LevelProperties[metadata_args.Member.HierarchyUniqueName] = properties;
                            LoadMemberAttributes(metadata_args.Member, properties);
                            break;
                    }
                }

                MemberInfoWrapper<MemberChoiceQuery> member_args = e.UserState as MemberInfoWrapper<MemberChoiceQuery>;
                if (member_args != null)
                {
                    switch (member_args.UserData.QueryType)
                    {
                        case MemberChoiceQueryType.GetMember:
                            MemberDataWrapper member = null;
                            if (!String.IsNullOrEmpty(e.Result.Content))
                            {
                                try
                                {
                                    member = XmlSerializationUtility.XmlStr2Obj<MemberDataWrapper>(e.Result.Content);
                                }
                                catch
                                {
                                }
                            }

                            if (member != null)
                            {
                                ShowMemberAttributes(member);
                            }
                            else
                            {
                                MessageBox.Show(Localization.PivotGrid_CustomProperties_NotFound, Localization.Warning, MessageBoxButton.OK);
                            }
                            break;
                    }
                }
            }
            finally
            {
                if (stopWaiting)
                    IsWaiting = false;
            }
        }

        private SaveFileDialog m_SaveFileDialog = null;
        private SaveFileDialog SaveFileDialog
        {
            get
            {
                if (m_SaveFileDialog == null)
                {
                    m_SaveFileDialog = new SaveFileDialog();
                    m_SaveFileDialog.Filter = Localization.SaveDialog_Filter_XlsFiles;
                }
                return m_SaveFileDialog;
            }
        }


        void SaveToFile(String str)
        {
            try
            {
                //bool? ret = SaveFileDialog.ShowDialog();
                //if (ret.HasValue && ret.Value == true)
                //{
                    using (Stream stream = SaveFileDialog.OpenFile())
                    {
                        byte[] info = (new UTF8Encoding(true)).GetBytes(str);
                        stream.Write(info, 0, info.Length);
                    }
                //}
            }
            catch (Exception ex)
            {
                LogManager.LogException(Localization.PivotGridControl_Name, ex);
            }
        }

        protected virtual void ServiceCommandCompleted(ServiceCommandArgs args)
        { 
        
        }

        protected virtual void UpdateCubeCompleted(UpdateCubeArgs e)
        {
            RunServiceCommand(ServiceCommandType.Refresh);
        }

        protected virtual void MemberActionCompleted(DataLoaderEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Result.Content))
            {
                //CellSetData cs_descr = XmlSerializationUtility.XmlStr2Obj<CellSetData>(e.Result);
                CellSetData cs_descr = CellSetData.Deserialize(e.Result.Content);
                Initialize(cs_descr);
            }
            //this.Focus();
            UpdateButtons();
        }

        void ResetSettings()
        {
            m_CSDescr = null;
            m_CubeMetaData = null;
            m_CellSetProvider = null;
            UpdateToolbarButtons(null);
            UpdateEditToolBarButtons();
        }

        public void Initialize()
        {
            // Отжимаем кнопку "Повернуть оси"
            RotateAxesButton.Click -= new RoutedEventHandler(RotateAxesButton_Click);
            RotateAxesButton.IsChecked = new bool?(false);
            PivotGrid.AxisIsRotated = false;
            RotateAxesButton.Click += new RoutedEventHandler(RotateAxesButton_Click);

            // Устанавливаем значение кнопки Редактирование в соответствии с возможностью редактирования
            EditButton.IsChecked = IsUpdateable;

            ResetSettings();

            // Если запрос пустой, то данные с сервера не читаем
            if (!String.IsNullOrEmpty(Query))
            {
                PivotInitializeArgs args = FastCommandHelper.CreatePivotInitializeArgs(Connection, Query, UpdateScript);
                args.PivotID = this.GetHashCode().ToString();
                IsWaiting = true;
                OlapDataLoader.LoadData(args, args);
            }
            else
            {
                Initialize(null);
            }
        }

        private CellSetData m_CSDescr = null;
        CellSetDataProvider m_CellSetProvider = null;
        protected CellSetDataProvider CellSetProvider
        {
            get { return m_CellSetProvider; }
        }

        protected virtual void Initialize(CellSetData cs_descr)
        {
            try
            {
                ResetSettings();

                IsWaiting = true;
                DateTime start = DateTime.Now;
                //System.Diagnostics.Debug.WriteLine("UpdateablePivotGrid initializing start: " + start.ToString());

                m_CSDescr = cs_descr;
                if (cs_descr != null)
                {
                    m_CellSetProvider = new CellSetDataProvider(cs_descr);
                    ImportSizeInfo();
                }
                PivotGrid.Initialize(m_CellSetProvider);
                UpdateButtons();

                DateTime stop = DateTime.Now;
                //System.Diagnostics.Debug.WriteLine("UpdateablePivotGrid initializing stop: " + stop.ToString());
                System.Diagnostics.Debug.WriteLine("UpdateablePivotGrid initializing time: " + (stop - start).ToString());
            }
            finally
            {
                //IsWaiting = false;
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RunServiceCommand(ServiceCommandType.Refresh);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            RunServiceCommand(ServiceCommandType.Back);
        }

        private void RunServiceCommand(ServiceCommandType actionType)
        {
            if (UseChangesCashe && m_CellChanges.CellChanges.Count > 0)
            {
                MessageBox.Show(Localization.PivotGrid_SaveCachedChanges, Localization.MessageBox_Warning, MessageBoxButton.OK);
                //PopUpQuestionDialog dlg = SaveChangesDlg;
                //dlg.DialogClosed += new EventHandler<Ranet.AgOlap.Controls.Forms.DialogResultArgs>(RunService_SaveChanges_DialogClosed);
                //dlg.Tag = actionType;
                //dlg.Show();
                return;
            }

            if (actionType == ServiceCommandType.Refresh)
                ExportSizeInfo();

            ServiceCommandArgs args = new ServiceCommandArgs(actionType);
            args.PivotID = this.GetHashCode().ToString();
            IsWaiting = true;
            OlapDataLoader.LoadData(args, args);
        }

        public CellControl FocusedCell
        {
            get {
                return PivotGrid.FocusedCell;
            }
        }

        //void RunService_SaveChanges_DialogClosed(object sender, Ranet.AgOlap.Controls.Forms.DialogResultArgs e)
        //{
        //    dlg_CloseDialog(e.Result);

        //    PopUpQuestionDialog dlg = sender as PopUpQuestionDialog;
        //    ServiceCommandType actionType = ServiceCommandType.None;
        //    if (dlg != null && dlg.Tag is ServiceCommandType)
        //    {
        //        actionType = (ServiceCommandType)(dlg.Tag);
        //    }

        //    ServiceCommandArgs args = new ServiceCommandArgs(actionType);
        //    Loader.PerformServiceCommand(XmlSerializationUtility.Obj2XmlStr(args, Common.Namespace), args);
        //}

        #region Настройки шрифтов, размеров по умолчанию
        public double DefaultFontSize
        {
            get { return PivotGrid.DefaultFontSize; }
            set { PivotGrid.DefaultFontSize = value; }
        }

        public double DefaultMemberWidth
        {
            get { return PivotGrid.DEFAULT_WIDTH; }
            set { PivotGrid.DEFAULT_WIDTH = value; }
        }

        public double DefaultMemberHeight
        {
            get { return PivotGrid.DEFAULT_HEIGHT; }
            set { PivotGrid.DEFAULT_HEIGHT = value; }
        }

        public double MinMemberWidth
        {
            get { return PivotGrid.MIN_WIDTH; }
            set { PivotGrid.MIN_WIDTH = value; }
        }

        public double MinMemberHeight
        {
            get { return PivotGrid.MIN_HEIGHT; }
            set { PivotGrid.MIN_HEIGHT = value; }
        }

        public double RowsDrillDownMemberOffset
        {
            get { return PivotGrid.DRILLDOWN_SPACE_WIDTH; }
            set { PivotGrid.DRILLDOWN_SPACE_WIDTH = value; }
        }
        #endregion Настройки шрифтов, размеров по умолчанию

        #region Управление возможностью редактирования ячеек
        public bool IsUpdateable
        {
            get
            {
                return PivotGrid.IsUpdateable;
            }
            set
            {
                PivotGrid.IsUpdateable = value;
                // Устанавливаем значение кнопки Редактирование в соответствии с возможностью редактирования
                EditButton.IsChecked = IsUpdateable;
                UpdateEditToolBarButtons();
            }
        }

        bool m_UseChangesCashe = false;
        public bool UseChangesCashe
        {
            get
            {
                return m_UseChangesCashe;
            }
            set
            {
                m_UseChangesCashe = value;
                UseChangesCasheButton.IsChecked = new bool?(value);
                UpdateEditToolBarButtons();
            }
        }

        public bool EditMode
        {
            get
            {
                return PivotGrid.EditMode;
            }
        }

        /// <summary>
        /// Кэш измененных ячеек
        /// </summary>
        CellChangesCache m_CellChanges = new CellChangesCache();

        void UpdateEditToolBarButtons()
        {
            UpdateEditToolBarButtons(false);
        }
        
        void UpdateEditToolBarButtons(bool skipEditModeButton)
        {
            // Кнопки управления редактированием делаем видимыми только для таблицы, которая поддерживает редактирование
            EditButton.Visibility = PivotGrid.CanEdit ? Visibility.Visible : Visibility.Collapsed;
            //CopyToClipboardButton;
            PasteFromClipboardButton.Visibility = PivotGrid.CanEdit ? Visibility.Visible : Visibility.Collapsed;
            UseChangesCasheButton.Visibility = PivotGrid.CanEdit ? Visibility.Visible : Visibility.Collapsed;
            ConfirmEditButton.Visibility = PivotGrid.CanEdit ? Visibility.Visible : Visibility.Collapsed;
            CancelEditButton.Visibility = PivotGrid.CanEdit ? Visibility.Visible : Visibility.Collapsed;

            if (PivotGrid.CanEdit)
            {
                if (!skipEditModeButton)
                {
                    // Кнопка "Редактирование"
                    if (EditButton.IsChecked.HasValue && EditButton.IsChecked.Value != PivotGrid.EditMode)
                    {
                        EditButton.IsChecked = new bool?(PivotGrid.EditMode);
                    }
                }

                //if (IsUpdateable)
                //{
                    // Делаем кнопки видимыми
                    // EditButton.Visibility = UseChangesCasheButton.Visibility = ConfirmEditButton.Visibility = CancelEditButton.Visibility = Visibility.Visible;
                    EditButton.IsEnabled = true;

                    // Если кнопка btnEdit не нажата, то делаем недоступными кнопки btnUseChangesCache, btnSave, btnCancel
                    UseChangesCasheButton.IsEnabled = EditButton.IsChecked.Value;
                    PasteFromClipboardButton.IsEnabled = EditButton.IsChecked.Value;

                    if (EditButton.IsChecked.Value)
                    {
                        ConfirmEditButton.IsEnabled = CancelEditButton.IsEnabled =
                            m_CellChanges.CellChanges.Count > 0/* || m_AllocationArgs.Count > 0*/;
                        /*btnModifications.Enabled = true;*/
                    }
                    else
                    {
                        ConfirmEditButton.IsEnabled = CancelEditButton.IsEnabled = EditButton.IsChecked.Value;
                        //btnModifications.Enabled = false;
                    }
                //}
                //else
                //{
                //    // Делаем кнопки невидимыми
                //    // EditButton.Visibility = UseChangesCasheButton.Visibility = ConfirmEditButton.Visibility = CancelEditButton.Visibility = Visibility.Collapsed;
                //    EditButton.IsEnabled = UseChangesCasheButton.IsEnabled = PasteFromClipboardButton.IsEnabled = ConfirmEditButton.IsEnabled = CancelEditButton.IsEnabled = false;
                //}

                //EditButton.IsChecked = new bool?(IsUpdateable);
                //UseChangesCasheButton.IsChecked = new bool?(UseChangesCashe);
            }
        }

        #endregion Управление возможностью редактирования ячеек

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
        #endregion Свойства для настройки на OLAP

        public String Query { get; set; }
        public String UpdateScript 
        {
            get { return PivotGrid.UpdateScript; }
            set { 
                PivotGrid.UpdateScript = value;
                UpdateEditToolBarButtons();
            }
        }

        public MemberVisualizationTypes MemberVisualizationType
        {
            get { return PivotGrid.MemberVisualizationType; }
            set { PivotGrid.MemberVisualizationType = value; }
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            RunServiceCommand(ServiceCommandType.Forward);
        }

        /// <summary>
        /// Определяет возможность использования Expand, Collapse, DrillDown
        /// </summary>
        public bool RowsIsInteractive
        {
            get
            {
                return PivotGrid.Axis1_IsInteractive;
            }
            set
            {
                PivotGrid.Axis1_IsInteractive = value;
            }
        }

        /// <summary>
        /// Определяет возможность использования Expand, Collapse, DrillDown
        /// </summary>
        public bool ColumnsIsInteractive
        {
            get
            {
                return PivotGrid.Axis0_IsInteractive;
            }
            set
            {
                PivotGrid.Axis0_IsInteractive = value;
            }
        }

        public void ImportSizeInfo()
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
            if (isoStore != null)
            {
                if (isoStore.FileExists(IsoStorageFile))
                {
                    IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(IsoStorageFile, System.IO.FileMode.Open, isoStore);
                    StreamReader reader = new StreamReader(isoStream);
                    String Text = reader.ReadToEnd();
                    reader.Close();

                    if (!String.IsNullOrEmpty(Text))
                    {
                        PivotGridSizeInfo sizeInfo = XmlSerializationUtility.XmlStr2Obj<PivotGridSizeInfo>(Text);
                        if (sizeInfo != null)
                        {
                            PivotGrid.SetSizeInfo(sizeInfo);
                        }
                    }
                }
            }
        }

        public void ExportSizeInfo()
        {
            PivotGridSizeInfo sizeInfo = PivotGrid.GetSizeInfo();
            if (sizeInfo != null)
            {
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                if (isoStore != null)
                {
                    IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(IsoStorageFile, System.IO.FileMode.Create, isoStore);
                    StreamWriter writer = new StreamWriter(isoStream);
                    writer.Write(XmlSerializationUtility.Obj2XmlStr(sizeInfo));
                    writer.Close();
                }
            }
        }

        String m_IsoStorageFile = String.Empty;
        private String IsoStorageFile
        {
            get
            {
                if (String.IsNullOrEmpty(m_IsoStorageFile))
                {
                    m_IsoStorageFile = this.Name + "_SI.xml";
                    if (!String.IsNullOrEmpty(IsolatedStoragePrefix))
                    {
                        String[] items = IsolatedStoragePrefix.Split(new String[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                        if (items != null && items.Length > 0)
                            m_IsoStorageFile = items[items.Length - 1] + "_" + m_IsoStorageFile;
                    }
                }
                return m_IsoStorageFile;
            }
        }

        String m_IsolatedStoragePrefix = String.Empty;
        public String IsolatedStoragePrefix
        {
            get
            {
                return m_IsolatedStoragePrefix;
            }
            set
            {
                m_IsolatedStoragePrefix = value;
                m_IsoStorageFile = String.Empty;
            }
        }

        #region Настройки для подсказок
        /// <summary>
        /// Использование подсказки в области строк
        /// </summary>
        public bool UseRowsAreaHint
        {
            get { return PivotGrid.Rows_UseHint; }
            set { PivotGrid.Rows_UseHint = value; }
        }

        /// <summary>
        /// Использование подсказки в области колонок
        /// </summary>
        public bool UseColumnsAreaHint
        {
            get { return PivotGrid.Columns_UseHint; }
            set { PivotGrid.Columns_UseHint = value; }
        }

        /// <summary>
        /// Использование подсказки в области ячеек
        /// </summary>
        public bool UseCellsAreaHint
        {
            get { return PivotGrid.Cells_UseHint; }
            set { PivotGrid.Cells_UseHint = value; }
        }
        #endregion Настройки для подсказок

        bool m_UseNavigationButtons = true;
        /// <summary>
        /// Управляет отображением кнопок навигации в сводной таблице
        /// </summary>
        public bool UseNavigationButtons
        {
            get
            {
                return m_UseNavigationButtons;
            }
            set
            {
                if (value)
                {
                    m_NavigationButtons_Splitter.Visibility = Visibility.Visible;
                    ToBeginButton.Visibility = Visibility.Visible;
                    BackButton.Visibility = Visibility.Visible;
                    ForwardButton.Visibility = Visibility.Visible;
                    ToEndButton.Visibility = Visibility.Visible;
                }
                else
                {
                    m_NavigationButtons_Splitter.Visibility = Visibility.Collapsed;
                    ToBeginButton.Visibility = Visibility.Collapsed;
                    BackButton.Visibility = Visibility.Collapsed;
                    ForwardButton.Visibility = Visibility.Collapsed;
                    ToEndButton.Visibility = Visibility.Collapsed;
                }
                m_UseNavigationButtons = value;
            }
        }

        public IList<CellConditionsDescriptor> CustomCellsConditions
        {
            get {
                return PivotGrid.CustomCellsConditions; 
            }
            set {
                PivotGrid.CustomCellsConditions = value; 
            }
        }
    }
}


