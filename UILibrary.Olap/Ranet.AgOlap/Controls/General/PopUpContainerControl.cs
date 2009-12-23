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
using Ranet.AgOlap.Controls.Buttons;
using Ranet.AgOlap.Controls.Forms;

namespace Ranet.AgOlap.Controls.General
{
    public class PopUpContainerControl : UserControl
    {
        Grid LayoutRoot = null;
        public readonly TextBox SelectedItemTextBox = null;
        RanetButton m_SelectButton = null;

        public PopUpContainerControl()
        {
            LayoutRoot = new Grid(){Background = new SolidColorBrush(Colors.White) };
            LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition());
            LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto});


            SelectedItemTextBox = new SimpleTextBox(){ IsReadOnly = true, IsTabStop = false };
            SelectedItemTextBox.VerticalAlignment = VerticalAlignment.Stretch;
            SelectedItemTextBox.Margin = new Thickness(0);
            SelectedItemTextBox.Padding = new Thickness(2, 0, 0, 0);
            SelectedItemTextBox.VerticalContentAlignment = VerticalAlignment.Center;
            SelectedItemTextBox.Background = new SolidColorBrush(Colors.White);
            LayoutRoot.Children.Add(SelectedItemTextBox);

            m_SelectButton = new RanetButton();
            LayoutRoot.Children.Add(m_SelectButton);
            m_SelectButton.Margin = new Thickness(-1, 0, 0, 0);
            m_SelectButton.Content = "...";
            //m_SelectButton.Width = 18;
            //m_SelectButton.Height = 18;
//            selectButton.Template = this.Resources["BmButton"] as ControlTemplate;

            m_SelectButton.Click += new RoutedEventHandler(Button_Click);
            Grid.SetColumn(m_SelectButton, 1);

            this.Height = 20;

            this.Content = LayoutRoot;
        }

        public new double Height
        {
            get { return Height; }
            set { 
                base.Height = value;
                m_SelectButton.Height = m_SelectButton.Width = value;
            }
        }

        FloatingDialog m_PopupControl = null;
        public FloatingDialog PopupControl
        {
            get
            {
                if (m_PopupControl == null)
                {
                    m_PopupControl = new FloatingDialog();

                    FrameworkElement element = this;
                    Panel panel = null;
                    while (element != null && element.Parent != null)
                    {
                        if (element.Parent is Panel)
                            panel = element.Parent as Panel;
                        element = element.Parent as FrameworkElement;
                    }

                    if (panel != null)
                    {
                        panel.Children.Add(m_PopupControl.PopUpControl);
                    }
                    else
                    {
                        LayoutRoot.Children.Add(m_PopupControl.PopUpControl);
                    }

                }
                return m_PopupControl;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HandlePopUp();
        }

        private UIElement m_Content;
        public UIElement ContentControl
        {
            set
            {
                //m_PopupControl.Child = value;
                //Grid grid = new Grid();
                //grid.Width = 400;
                //grid.Height = 300;
                //grid.Children.Add(value);
                //m_PopupControl.Child = grid;

                PopupControl.Width = 500;
                PopupControl.Height = 400;

                PopupControl.SetContent(value);

                m_Content = value;
            }
            get {
                return m_Content;
            }
        }

        public event EventHandler BeforePopUp;

        public String Text
        {
            get
            {
                return SelectedItemTextBox.Text;
            }
            set
            {
                SelectedItemTextBox.Text = value;
            }
        }

        public bool IsDropDownOpen
        {
            get
            {
                return PopupControl.IsShowing;
            }
            set
            {
                if (value)
                {
                    EventHandler handler = BeforePopUp;
                    if (handler != null)
                    {
                        handler(this, EventArgs.Empty);
                    }

                    //m_PopupControl.VerticalOffset = this.ActualHeight;
                    //m_PopupControl.HorizontalOffset = 0;
                }

                PopupControl.IsShowing = value;
            }
        }

        private void HandlePopUp()
        {
            if (this.IsDropDownOpen)
            {
                base.Focus();
                this.IsDropDownOpen = false;
                //this.ContentControl.ReleaseMouseCapture();
            }
            else
            {
                this.IsDropDownOpen = true;
                //this.ContentControl.CaptureMouse();
            }
        }
    }
}
