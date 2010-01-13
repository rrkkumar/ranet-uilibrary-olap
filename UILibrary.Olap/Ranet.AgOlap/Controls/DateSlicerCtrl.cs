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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Ranet.AgOlap.Commands;
using Ranet.AgOlap.Controls.General;
using Ranet.Olap.Core;
using Ranet.Olap.Core.Data;
using Ranet.Olap.Core.Providers.ClientServer;
using Ranet.AgOlap.Controls.Buttons;

namespace Ranet.AgOlap.Controls
{
    public enum Direction
    {
        Horizontal,
        Vertical,
        Both
    }
    public class ChangedDirectionEventArgs : EventArgs
    {
        public readonly Direction Direction = Direction.Horizontal;

        public ChangedDirectionEventArgs(Direction direction)
        {
            Direction = direction;
        }
    }

    public class SlicerCtrl : AgControlBase
    {
        private Grid m_DataGrid;
        private BusyControl m_Waiting;
        private StackPanel m_Panel;
        private Dictionary<String, MemberData> m_LoadedMembers = new Dictionary<String, MemberData>();
        public const double CellWidth = 50;
        public const double CellHeight = 20;
        private double CellsCount = 25;
        private List<int> m_slicedButtons = new List<int>();
        
        public SlicerCtrl()
        {
            Grid LayoutRoot = new Grid();
            m_DataGrid = new Grid();
            LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition() {Width = GridLength.Auto});
            LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition() {MaxWidth = 20});
            LayoutRoot.RowDefinitions.Add(new RowDefinition() {MaxHeight = 20});
            LayoutRoot.RowDefinitions.Add(new RowDefinition(){ Height = GridLength.Auto});
            LayoutRoot.RowDefinitions.Add(new RowDefinition() {Height = GridLength.Auto});
            m_Panel = new StackPanel();
            m_Panel.Orientation = Orientation.Horizontal;
            RanetHotButton m_Clear = new RanetHotButton();
            m_Clear.Width = 20;
            m_Clear.Height = 20;
            m_Clear.Content = "C";
            m_Clear.Click +=new RoutedEventHandler(m_Clear_Click);
            LayoutRoot.Children.Add(m_Clear);
            Grid.SetRow(m_Clear,0);
            Grid.SetColumn(m_Clear,1);
            LayoutRoot.Children.Add(m_Panel);
            Grid.SetRow(m_Panel, 1);
            Grid.SetColumn(m_Panel, 0);
            LayoutRoot.Children.Add(m_DataGrid);
            Grid.SetColumn(m_DataGrid, 0);
            Grid.SetRow(m_DataGrid, 2);

            m_Waiting = new BusyControl();
            m_Waiting.Text = Localization.Loading;
            //LayoutRoot.Children.Add(m_Waiting);
            //Grid.SetRow(m_Waiting, 1);

            IsBusy = false;

            this.Content = LayoutRoot;
            //this.SlicerHeight = 9;
            //this.SlicerWidth = 9;           
            //this.m_DataGrid.MouseLeftButtonDown += new MouseButtonEventHandler(m_DataGrid_MouseLeftButtonDown);
            //this.m_DataGrid.MouseLeftButtonUp += new MouseButtonEventHandler(m_DataGrid_MouseLeftButtonUp);
            this.DirectionChanged += new EventHandler<ChangedDirectionEventArgs>(SlicerCtrl_DirectionChanged);
            //this.m_Panel.MouseMove += new MouseEventHandler(m_Panel_MouseMove);
            //this.Content = m_DataGrid;
        }
        

        void SlicerCtrl_DirectionChanged(object sender, ChangedDirectionEventArgs e)
        {
            if (e.Direction == Controls.Direction.Vertical)
            {
                this.SlicerWidth = 1;
                this.SlicerHeight = CellsCount;
            }
            if (e.Direction ==Controls.Direction.Horizontal)
            {
                this.SlicerHeight = 1;
                this.SlicerWidth = CellsCount;
            }
            if (e.Direction ==Controls.Direction.Both)
            {
                this.SlicerWidth = this.SlicerHeight = Math.Floor(Math.Sqrt(CellsCount));                 
            }
        }

        private bool Slicing
        {
            get; set;
        }

        protected bool NeedReload = true;        

        public double SlicerHeight
        {
            get; set;
        }

        public double SlicerWidth
        {
            get; set;
        }

        private Direction m_Direction = Controls.Direction.Horizontal; 
        public Direction Direction
        {
            get { return m_Direction; }
            set
            {
                if (m_Direction != value)
                {
                    this.CellsCount = SlicerWidth*SlicerHeight;
                    Raise_DirectionChanged(value);
                }
                m_Direction = value;
            }
        }

        #region Свойства для настройки на OLAP

        public String Query { get; set; }        
        
        private String m_Connection = String.Empty;
        /// <summary>
        /// Описание соединения с БД
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
                NeedReload = true;
            }
        }

        private string m_CubeName;
        /// <summary>
        /// Имя OLAP куба
        /// </summary>
        public String CubeName
        {
            get
            {
                return m_CubeName;
            }
            set
            {
                m_CubeName = value;
                NeedReload = true;
            }
        }

        private string m_DayLevelUniqueName = String.Empty;
        /// <summary>
        /// Уникальное имя уровня, хранящего дни
        /// </summary>
        public String DayLevelUniqueName
        {
            get
            {
                return m_DayLevelUniqueName;
            }
            set
            {
                m_DayLevelUniqueName = value;
                NeedReload = true;
            }
        }

        private string m_DateToUniqueNameTemplate = String.Empty;
        /// <summary>
        /// Шаблон для преобразования даты в уникальное имя
        /// </summary>
        public String DateToUniqueNameTemplate
        {
            get
            {
                return m_DateToUniqueNameTemplate;
            }
            set
            {
                m_DateToUniqueNameTemplate = value;
                NeedReload = true;
            }
        }
        #endregion Свойства для настройки на OLAP

        bool m_IsBusy = false;
        public bool IsBusy
        {
            get { return m_IsBusy; }
            set
            {
                m_IsBusy = value;
                if (value)
                        this.IsEnabled = false;
                    else
                        this.IsEnabled = true;                
            }
        }


        public event EventHandler<ChangedDirectionEventArgs> DirectionChanged;
        void Raise_DirectionChanged(Direction direction)
        {
            EventHandler<ChangedDirectionEventArgs> handler = DirectionChanged;
            if (handler != null)
            {
                handler(this, new ChangedDirectionEventArgs(direction));
            }
        }

        public void Initialize()
        {
            m_LoadedMembers.Clear();
            m_slicedButtons.Clear();
            LoadMembers();
        }

        void LoadMembers()
        {
            if (String.IsNullOrEmpty(Connection))
            {
                // Сообщение в лог
                StringBuilder builder = new StringBuilder();
                if (String.IsNullOrEmpty(Connection))
                    builder.Append(Localization.Connection_PropertyDesc);
                LogManager.LogError(this, String.Format(Localization.ControlSettingsNotInitialized_Message, builder.ToString()));
                return;
            }

            if (!String.IsNullOrEmpty(DayLevelUniqueName))
            {
                String query = "Select {" + DayLevelUniqueName +
                    ".Members} on 0, {} on 1 from " + OlapHelper.ConvertToQueryStyle(CubeName);

                IsBusy = true;

                MdxQueryArgs args = CommandHelper.CreateMdxQueryArgs(Connection, query);
                OlapDataLoader.LoadData(args, null);
            }
        }

        #region Загрузчики
        IDataLoader m_OlapDataLoader = null;
        public IDataLoader OlapDataLoader
        {
            set
            {
                if (m_OlapDataLoader != null)
                {
                    m_OlapDataLoader.DataLoaded -= new EventHandler<DataLoaderEventArgs>(Loader_DataLoaded);
                }
                m_OlapDataLoader = value;
                m_OlapDataLoader.DataLoaded += new EventHandler<DataLoaderEventArgs>(Loader_DataLoaded);
            }
            get
            {
                if (m_OlapDataLoader == null)
                {
                    m_OlapDataLoader = GetDataLoader();
                    m_OlapDataLoader.DataLoaded += new EventHandler<DataLoaderEventArgs>(Loader_DataLoaded);
                }
                return m_OlapDataLoader;
            }
        }

        protected virtual IDataLoader GetDataLoader()
        {
            return new OlapDataLoader(URL);
        }
        #endregion Загрузчики

        void Loader_DataLoaded(object sender, DataLoaderEventArgs e)
        {
            IsBusy = false;

            if (e.Error != null)
            {
                LogManager.LogError(this, e.Error.ToString());
                return;
            }

            if (e.Result.ContentType == InvokeContentType.Error)
            {
                LogManager.LogError(this, e.Result.Content);
                return;
            }

            //CellSetData cs_descr = XmlSerializationUtility.XmlStr2Obj<CellSetData>(e.Result);
            CellSetData cs_descr = CellSetData.Deserialize(e.Result.Content);
            OnDatesLoaded(cs_descr);

            UpdateGridCells();
        }

        
        void OnDatesLoaded(CellSetData cs_descr)
        {
            m_LoadedMembers.Clear();
            if (cs_descr.Axes.Count > 0)
            {
                foreach (PositionData position in cs_descr.Axes[0].Positions)
                {
                    if (position.Members.Count > 0)
                    {
                        if (!m_LoadedMembers.ContainsKey(cs_descr.Axes[0].Members[position.Members[0].Id].UniqueName))
                            m_LoadedMembers.Add(cs_descr.Axes[0].Members[position.Members[0].Id].UniqueName, cs_descr.Axes[0].Members[position.Members[0].Id]);
                    }
                }
            }
        }

        /// <summary>
        /// Кэш сгенеренных кнопок для slicer
        /// </summary>       
        Dictionary<int, RanetToggleButton> slicerChildren = new Dictionary<int, RanetToggleButton>();

        void UpdateGridCells()
        {
            slicerChildren.Clear();
            this.m_Panel.Children.Clear();
            this.m_DataGrid.Children.Clear();
            if (this.Direction == Controls.Direction.Vertical)
            {
                m_Panel.Orientation = Orientation.Vertical;
                List<string> values = new List<string>();
                var enumerator = m_LoadedMembers.GetEnumerator();
                for (int i = 0; i < this.SlicerHeight + 1; i++)
                {
                    if (enumerator.Current.Value != null && !String.IsNullOrEmpty(enumerator.Current.Value.Caption))
                    {
                        RanetToggleButton button = new RanetToggleButton();
                        button.ButtonId = i;
                        button.Checked += new RoutedEventHandler(button_Checked);
                        button.Unchecked += new RoutedEventHandler(button_Unchecked);
                        ToolTipService.SetToolTip(button, enumerator.Current.Value.Caption);
                        button.Content = enumerator.Current.Value.Caption;
                        slicerChildren.Add(i, button);
                    }
                    enumerator.MoveNext();
                    
                }            
                foreach (var child in slicerChildren)
                {
                    this.m_Panel.Children.Add(child.Value);
                }
            }
            if (this.Direction == Controls.Direction.Horizontal)
            {
                m_Panel.Orientation = Orientation.Horizontal;   
                List<string> values = new List<string>();
                var enumerator = m_LoadedMembers.GetEnumerator();
                for (int i = 0; i < this.SlicerWidth + 1; i++)
                {                  
                    if (enumerator.Current.Value != null && !String.IsNullOrEmpty(enumerator.Current.Value.Caption))
                    {
                        RanetToggleButton button = new RanetToggleButton();
                        button.ButtonId = i;
                        button.Checked += new RoutedEventHandler(button_Checked);
                        button.Unchecked += new RoutedEventHandler(button_Unchecked);
                        ToolTipService.SetToolTip(button, enumerator.Current.Value.Caption);
                        button.Content = enumerator.Current.Value.Caption;
                        slicerChildren.Add(i, button);
                    }                                            
                    enumerator.MoveNext();
                }
                foreach (var child in slicerChildren)
                {
                    this.m_Panel.Children.Add(child.Value);
                }
            }

            if (this.Direction == Controls.Direction.Both)
            {
                //m_DataGrid = new Grid();
                var enumerator = m_LoadedMembers.GetEnumerator();
                for (int i = 0; i < this.SlicerHeight; i++)
                {
                    m_DataGrid.RowDefinitions.Add(new RowDefinition() {Height = GridLength.Auto } );                       
                }
                for (int j = 0; j < this.SlicerWidth + 1; j++)
                {
                    m_DataGrid.ColumnDefinitions.Add(new ColumnDefinition() {Width = GridLength.Auto});                 
                }
                int count = 0;
                for (int i = 0; i < this.SlicerHeight; i++)
                {
                    for (int j = 0; j < this.SlicerWidth + 1; j++)
                    {
                        if (enumerator.Current.Value != null && !String.IsNullOrEmpty(enumerator.Current.Value.Caption))
                        {
                            RanetToggleButton button = new RanetToggleButton();
                            button.ButtonId = i;
                            button.Checked += new RoutedEventHandler(button_Checked);
                            button.Unchecked += new RoutedEventHandler(button_Unchecked);
                            ToolTipService.SetToolTip(button, enumerator.Current.Value.Caption);
                            button.Content = enumerator.Current.Value.Caption;
                            slicerChildren.Add(count,button);
                            count++;
                            m_DataGrid.Children.Add(button);
                            Grid.SetRow(button, i);
                            Grid.SetColumn(button, j);
                        }
                        enumerator.MoveNext();
                    }
                }                                
            }           
        }

        void button_Unchecked(object sender, RoutedEventArgs e)
        {
            if (m_slicedButtons.Contains((sender as RanetToggleButton).ButtonId))
            {
                this.m_slicedButtons.Remove((sender as RanetToggleButton).ButtonId);
                this.ApplySelection();
            }
        }

        void button_Checked(object sender, RoutedEventArgs e)
        {
            if (!m_slicedButtons.Contains((sender as RanetToggleButton).ButtonId))
            {
                this.m_slicedButtons.Add((sender as RanetToggleButton).ButtonId);
                this.ApplySelection();
            }
        }

        
        void m_Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearSelection();
        }
        
        public void ClearSelection()
        {
            foreach (var child in slicerChildren)
            {
                child.Value.IsChecked = false;
            }
        }


        public Dictionary<string,MemberData> GetSelectedElements()
        {
            Dictionary<string, MemberData> result = null;
            if (m_LoadedMembers!= null)
            {
                var enumerator = m_LoadedMembers.GetEnumerator();
                result = new Dictionary<string, MemberData>();              
                int i = 1;
                do
                {
                    if (m_slicedButtons.Contains(i))
                    {
                        result.Add(enumerator.Current.Key, enumerator.Current.Value);                        
                    }
                    i++;
                    enumerator.MoveNext();
                } while (enumerator.Current.Value != null);
            }
            return result;
        }

        protected virtual void ApplySelection()
        {            
        }       

        bool m_IsReadyToSelection = false;
        public bool IsReadyToSelection
        {
            get
            {
                return m_IsReadyToSelection;
            }
        }       
    }
}
