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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Ranet.AgOlap.Controls.General;

namespace Ranet.AgOlap.Controls.PivotGrid.Conditions
{
    public partial class CellConditionControl : UserControl
    {
        public CellConditionControl()
        {
            InitializeComponent();

            // Стилизуем скроллеры
            StyleContainer styleContainer = new StyleContainer();
            if (styleContainer.Resources != null &&
                styleContainer.Resources.Contains("ScrollViewerGlowStyle"))
            {
                Scroller.Style = styleContainer.Resources["ScrollViewerGlowStyle"] as Style;
            }

            Ranet.AgOlap.Features.ScrollViewerMouseWheelSupport.AddMouseWheelSupport(Scroller);
        }

        ~CellConditionControl()
        {
            Ranet.AgOlap.Features.ScrollViewerMouseWheelSupport.RemoveMouseWheelSupport(Scroller);
        }

        CellCondition m_Condition = null;
        public CellCondition Condition
        {
            get { return m_Condition; }
        }

        public void Initialize(CellCondition condition)
        {
            m_Condition = condition;
            IsEnabled = Condition != null;

            ConditionTypeCombo.EditEnd -= new EventHandler(ConditionTypeCombo_EditEnd);
            ConditionTypeCombo.ConditionType = Condition != null ? Condition.ConditionType : CellConditionType.None;
            ConditionTypeCombo.EditEnd += new EventHandler(ConditionTypeCombo_EditEnd);

            ShowValue.Checked -= new RoutedEventHandler(PropertyChecked);
            ShowValue.Unchecked -= new RoutedEventHandler(PropertyChecked);
            ShowValue.IsChecked = Condition != null ? Condition.Appearance.Options.ShowValue : false;
            ShowValue.Checked += new RoutedEventHandler(PropertyChecked);
            ShowValue.Unchecked += new RoutedEventHandler(PropertyChecked);

            UseBackColor.Checked -= new RoutedEventHandler(PropertyChecked);
            UseBackColor.Unchecked -= new RoutedEventHandler(PropertyChecked);
            UseBackColor.IsChecked = Condition != null ? Condition.Appearance.Options.UseBackColor : false;
            UseBackColor.Checked += new RoutedEventHandler(PropertyChecked);
            UseBackColor.Unchecked += new RoutedEventHandler(PropertyChecked);

            UseForeColor.Checked -= new RoutedEventHandler(PropertyChecked);
            UseForeColor.Unchecked -= new RoutedEventHandler(PropertyChecked);
            UseForeColor.IsChecked = Condition != null ? Condition.Appearance.Options.UseForeColor : false;
            UseForeColor.Checked += new RoutedEventHandler(PropertyChecked);
            UseForeColor.Unchecked += new RoutedEventHandler(PropertyChecked);

            UseAllOptions.Checked -= new RoutedEventHandler(PropertyChecked);
            UseAllOptions.Unchecked -= new RoutedEventHandler(PropertyChecked);
            UseAllOptions.IsChecked = Condition != null ? Condition.Appearance.Options.UseAllOptions : false;
            UseAllOptions.Checked += new RoutedEventHandler(PropertyChecked);
            UseAllOptions.Unchecked += new RoutedEventHandler(PropertyChecked);

            IgnoreAllOptions.Checked -= new RoutedEventHandler(PropertyChecked);
            IgnoreAllOptions.Unchecked -= new RoutedEventHandler(PropertyChecked);
            IgnoreAllOptions.IsChecked = Condition != null ? Condition.Appearance.Options.IgnoreAllOptions : false;
            IgnoreAllOptions.Checked += new RoutedEventHandler(PropertyChecked);
            IgnoreAllOptions.Unchecked += new RoutedEventHandler(PropertyChecked);

            ShowProgressBar.Checked -= new RoutedEventHandler(PropertyChecked);
            ShowProgressBar.Unchecked -= new RoutedEventHandler(PropertyChecked);
            ShowProgressBar.IsChecked = Condition != null ? Condition.Appearance.Options.UseProgressBar : false;
            ShowProgressBar.Checked += new RoutedEventHandler(PropertyChecked);
            ShowProgressBar.Unchecked += new RoutedEventHandler(PropertyChecked);

            numMinValue.Value = Condition != null ? Condition.Appearance.ProgressBarOptions.MinValue : 0;
            numMaxValue.Value = Condition != null ? Condition.Appearance.ProgressBarOptions.MaxValue : 1;

            comboBackColor.ColorsComboBox.SelectionChanged -= new SelectionChangedEventHandler(ColorsComboBox_SelectionChanged);
            comboBackColor.SelectItem(Condition != null ? Condition.Appearance.BackColor : Colors.Transparent);
            comboBackColor.ColorsComboBox.SelectionChanged += new SelectionChangedEventHandler(ColorsComboBox_SelectionChanged);

            comboForeColor.ColorsComboBox.SelectionChanged -= new SelectionChangedEventHandler(ColorsComboBox_SelectionChanged);
            comboForeColor.SelectItem(Condition != null ? Condition.Appearance.ForeColor : Colors.Transparent);
            comboForeColor.ColorsComboBox.SelectionChanged += new SelectionChangedEventHandler(ColorsComboBox_SelectionChanged);

            comboBorderColor.ColorsComboBox.SelectionChanged -= new SelectionChangedEventHandler(ColorsComboBox_SelectionChanged);
            comboBorderColor.SelectItem(Condition != null ? Condition.Appearance.BorderColor : Colors.Transparent);
            comboBorderColor.ColorsComboBox.SelectionChanged += new SelectionChangedEventHandler(ColorsComboBox_SelectionChanged);

            comboStartColor.ColorsComboBox.SelectionChanged -= new SelectionChangedEventHandler(ColorsComboBox_SelectionChanged);
            comboStartColor.SelectItem(Condition != null ? Condition.Appearance.ProgressBarOptions.StartColor : Colors.Transparent);
            comboStartColor.ColorsComboBox.SelectionChanged += new SelectionChangedEventHandler(ColorsComboBox_SelectionChanged);

            comboEndColor.ColorsComboBox.SelectionChanged -= new SelectionChangedEventHandler(ColorsComboBox_SelectionChanged);
            comboEndColor.SelectItem(Condition != null ? Condition.Appearance.ProgressBarOptions.EndColor : Colors.Transparent);
            comboEndColor.ColorsComboBox.SelectionChanged += new SelectionChangedEventHandler(ColorsComboBox_SelectionChanged);
        }

        void ColorsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshObject();
        }

        void PropertyChecked(object sender, RoutedEventArgs e)
        {
            RefreshObject();
        }

        void ConditionTypeCombo_EditEnd(object sender, EventArgs e)
        {
            RefreshObject();
        }

        public event EventHandler PropertyChanged;

        void RefreshObject()
        {
            if (Condition != null)
            {
                Condition.ConditionType = ConditionTypeCombo.ConditionType;

                Condition.Appearance.Options.ShowValue = ShowValue.IsChecked.Value;
                Condition.Appearance.Options.UseBackColor = UseBackColor.IsChecked.Value;
                Condition.Appearance.Options.UseForeColor = UseForeColor.IsChecked.Value;
                Condition.Appearance.Options.UseBorderColor = UseBorderColor.IsChecked.Value;
                Condition.Appearance.Options.UseAllOptions = UseAllOptions.IsChecked.Value;
                Condition.Appearance.Options.IgnoreAllOptions = IgnoreAllOptions.IsChecked.Value;
                Condition.Appearance.Options.UseProgressBar = ShowProgressBar.IsChecked.Value;

                if (comboBackColor.CurrentObject != null)
                {
                    Condition.Appearance.BackColor = comboBackColor.CurrentObject.ColorValue;
                }

                if (comboForeColor.CurrentObject != null)
                {
                    Condition.Appearance.ForeColor = comboForeColor.CurrentObject.ColorValue;
                }

                if (comboBorderColor.CurrentObject != null)
                {
                    Condition.Appearance.BorderColor = comboBorderColor.CurrentObject.ColorValue;
                }

                if (comboStartColor.CurrentObject != null)
                {
                    Condition.Appearance.ProgressBarOptions.StartColor = comboStartColor.CurrentObject.ColorValue;
                }

                if (comboEndColor.CurrentObject != null)
                {
                    Condition.Appearance.ProgressBarOptions.EndColor = comboEndColor.CurrentObject.ColorValue;
                }

                Condition.Appearance.ProgressBarOptions.MinValue = numMinValue.Value;
                Condition.Appearance.ProgressBarOptions.MaxValue = numMaxValue.Value;

                Raise_PropertyChanged();
            }
        }

        public void EndEdit()
        {
            RefreshObject();
        }

        void Raise_PropertyChanged()
        {
            EventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
