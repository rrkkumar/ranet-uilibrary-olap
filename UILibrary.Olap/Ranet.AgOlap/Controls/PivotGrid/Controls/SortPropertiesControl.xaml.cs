using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Ranet.Olap.Core.Providers;
using Ranet.AgOlap.Controls.General.ItemControls;
using Ranet.Olap.Core.Metadata;
using Ranet.AgOlap.Controls.General.Tree;
using Ranet.AgOlap.Controls.General;

namespace Ranet.AgOlap.Controls.PivotGrid.Controls
{
    public enum PivotTableSortTypes
    { 
        SortByProperty,
        SortByValue
    }

    public partial class SortPropertiesControl : UserControl
    {
        public SortPropertiesControl()
        {
            InitializeComponent();

            lblSortType.Text = Localization.SortingDirection_Label;
            lblNone.Text = Localization.Sorting_None_Label;
            lblAscending.Text = Localization.Sorting_Ascending_Label;
            lblDescending.Text = Localization.Sorting_Descending_Label;
            lblSortBy.Text = Localization.SortingBy_Label;
            lblProperty.Text = Localization.Property_Label;
            lblMeasure.Text = Localization.Measure_Label;

            comboProperty.Items.Add(new MemberPropertyItemControl(new MemberPropertyInfo("Caption", "Caption")));
            comboProperty.Items.Add(new MemberPropertyItemControl(new MemberPropertyInfo("Key0", "Key0")));

            comboProperty.SelectedIndex = 0;

            comboMeasure.Items.Add(new NoneItemControl());
            comboMeasure.SelectedIndex = 0;

            comboMeasure.IsEnabledChanged += new DependencyPropertyChangedEventHandler(comboMeasure_IsEnabledChanged);
            rbNone.Checked += new RoutedEventHandler(rbNone_Checked);
            rbNone.Unchecked += new RoutedEventHandler(rbNone_Unchecked);
        }

        void comboMeasure_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (comboMeasure.IsEnabled && SortType == PivotTableSortTypes.SortByValue)
            {
                if (!m_MeasuresIsLoaded)
                {
                    EventHandler<CustomEventArgs<EventArgs>> handler = LoadMeasures;
                    if (handler != null)
                    {
                        comboMeasure.Items.Clear();
                        WaitTreeControl ctrl = new WaitTreeControl() { Text = Localization.Loading };
                        comboMeasure.Items.Add(ctrl);
                        comboMeasure.SelectedIndex = 0;
                        comboMeasure.IsEnabled = false;

                        CustomEventArgs<EventArgs> args = new CustomEventArgs<EventArgs>(EventArgs.Empty);
                        handler(this, args);

                        if (args.Cancel)
                        {
                            comboMeasure.IsEnabled = true;
                            comboMeasure.Items.Clear();
                            comboMeasure.Items.Add(new NoneItemControl());
                            comboMeasure.SelectedIndex = 0;
                        }
                    }

                }
            }
        }

        void rbNone_Unchecked(object sender, RoutedEventArgs e)
        {
            if(SortType == PivotTableSortTypes.SortByValue)
                comboMeasure.IsEnabled = true;
            if (SortType == PivotTableSortTypes.SortByProperty)
                comboProperty.IsEnabled = true;
        }

        void rbNone_Checked(object sender, RoutedEventArgs e)
        {
            if (SortType == PivotTableSortTypes.SortByProperty)
                comboProperty.IsEnabled = false;
            if (SortType == PivotTableSortTypes.SortByValue)
                comboMeasure.IsEnabled = false;
        }

        bool m_MeasuresIsLoaded = false;
        public event EventHandler<CustomEventArgs<EventArgs>> LoadMeasures;


        public void InitializeMeasuresList(List<MeasureInfo> measures)
        {
            comboMeasure.Items.Clear();
            comboMeasure.Items.Add(new NoneItemControl());

            if (measures != null)
            {
                m_MeasuresIsLoaded = true;
                foreach (var item in measures)
                { 
                    comboMeasure.Items.Add(new MeasureItemControl(item));
                }
            }

            comboMeasure.IsEnabled = true;
            SelectMeasure(m_SortDescriptor != null ? m_SortDescriptor.SortBy : String.Empty);
        }

        PivotTableSortTypes m_SortType = PivotTableSortTypes.SortByProperty;
        public PivotTableSortTypes SortType
        {
            get { return m_SortType; }
            set
            {
                m_SortType = value;
                lblMeasure.Visibility = m_SortType == PivotTableSortTypes.SortByValue ? Visibility.Visible : Visibility.Collapsed;
                comboMeasure.Visibility = m_SortType == PivotTableSortTypes.SortByValue ? Visibility.Visible : Visibility.Collapsed;

                lblProperty.Visibility = m_SortType == PivotTableSortTypes.SortByProperty ? Visibility.Visible : Visibility.Collapsed;
                comboProperty.Visibility = m_SortType == PivotTableSortTypes.SortByProperty ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        SortDescriptor m_SortDescriptor;
        public void Initialize(PivotTableSortTypes sortType, SortDescriptor sortDescriptor)
        {
            SortType = sortType;
            m_SortDescriptor = sortDescriptor;

            if (sortDescriptor == null)
            {
                rbNone.IsChecked = true;
                if (sortType == PivotTableSortTypes.SortByValue)
                {
                    SelectMeasure(String.Empty);
                }
                if (sortType == PivotTableSortTypes.SortByProperty)
                {
                    SelectProperty(String.Empty);
                }
            }
            else
            {
                switch (sortDescriptor.Type)
                {
                    case SortTypes.Ascending:
                        rbAscending.IsChecked = true;
                        break;
                    case SortTypes.Descending:
                        rbDescending.IsChecked = true;
                        break;
                    default:
                        rbNone.IsChecked = true;
                        break;
                }

                if (sortType == PivotTableSortTypes.SortByProperty)
                {
                    SelectProperty(sortDescriptor.SortBy);
                }
                if (sortType == PivotTableSortTypes.SortByValue)
                {
                    SelectMeasure(sortDescriptor.SortBy);
                }
            }
        }

        public SortDescriptor SortDescriptor
        {
            get {
                var sortDescriptor = new SortDescriptor();
                if (rbAscending.IsChecked.Value)
                    sortDescriptor.Type = SortTypes.Ascending;
                if (rbDescending.IsChecked.Value)
                    sortDescriptor.Type = SortTypes.Descending;

                if (SortType == PivotTableSortTypes.SortByProperty && sortDescriptor.Type != SortTypes.None)
                {
                    MemberPropertyItemControl property = comboProperty.SelectedItem as MemberPropertyItemControl;
                    if (property != null && property.Info != null)
                        sortDescriptor.SortBy = property.Info.UniqueName;
                }

                if (SortType == PivotTableSortTypes.SortByValue && sortDescriptor.Type != SortTypes.None)
                {
                    MeasureItemControl measure = comboMeasure.SelectedItem as MeasureItemControl;
                    if (measure != null && measure.Info != null)
                        sortDescriptor.SortBy = measure.Info.UniqueName;
                }
                return sortDescriptor;
            }
        }

        private void lblNone_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!rbNone.IsChecked.Value)
            {
                rbNone.IsChecked = new bool?(true);
            }
        }

        private void lblAscending_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!rbAscending.IsChecked.Value)
            {
                rbAscending.IsChecked = new bool?(true);
            }
        }

        private void lblDescending_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!rbDescending.IsChecked.Value)
            {
                rbDescending.IsChecked = new bool?(true);
            }
        }

        public void SelectMeasure(String uniqueName)
        {
            if (String.IsNullOrEmpty(uniqueName))
            {
                if (comboMeasure.Items.Count > 0)
                    comboMeasure.SelectedIndex = 0;
            }
            else
            {
                for (int i = 0; i < comboMeasure.Items.Count; i++)
                {
                    var ctrl = comboMeasure.Items[i] as MeasureItemControl;
                    if (ctrl != null && ctrl.Info != null && ctrl.Info.UniqueName == uniqueName)
                    {
                        comboMeasure.SelectedIndex = i;
                        return;
                    }
                }
            }
        }

        public void SelectProperty(String name)
        {
            if (String.IsNullOrEmpty(name))
            {
                if (comboProperty.Items.Count > 0)
                    comboProperty.SelectedIndex = 0;
            }
            else
            {
                for (int i = 0; i < comboProperty.Items.Count; i++)
                {
                    var ctrl = comboProperty.Items[i] as MemberPropertyItemControl;
                    if (ctrl != null && ctrl.Info != null && ctrl.Info.Name == name)
                    {
                        comboProperty.SelectedIndex = i;
                        return;
                    }
                }
            }
        }
    }
}
