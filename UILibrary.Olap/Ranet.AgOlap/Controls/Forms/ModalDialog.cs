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

namespace Ranet.AgOlap.Controls.Forms
{
    public class ModalDialog
    {
        FloatingDialog m_Dialog = null;

        public double MinWidth
        {
            get { return m_Dialog.MinWidth; }
            set { m_Dialog.MinWidth = value; }
        }

        public double MinHeight
        {
            get { return m_Dialog.MinHeight; }
            set { m_Dialog.MinHeight = value; }
        }
        
        public double Width
        {
            get { return m_Dialog.Width; }
            set { m_Dialog.Width = value; }
        }

        public double Height
        {
            get { return m_Dialog.Height; }
            set { m_Dialog.Height = value; }
        }
        
        bool m_ContentIsInitialized = false;
        public String Caption
        {
            get { return m_Dialog.Caption; }
            set { m_Dialog.Caption = value; }
        }

        Grid gridContentContainer;
        public ModalDialog()
        {
            m_Dialog = new FloatingDialog();
            m_Dialog.Caption = String.Empty;

            Grid PopUpLayoutRoot = new Grid();
            PopUpLayoutRoot.RowDefinitions.Add(new RowDefinition() { });
            PopUpLayoutRoot.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            // Кнопки OK и Cancel
            StackPanel buttonsPanel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
            buttonsPanel.Margin = new Thickness(5, 0, 5, 5);

            RanetButton OkButton = new RanetButton() { Width = 75, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 0, 10, 0) };
            OkButton.Content = Localization.DialogButton_Ok;
            OkButton.Click += new RoutedEventHandler(OkButton_Click);
            buttonsPanel.Children.Add(OkButton);

            RanetButton CancelButton = new RanetButton() { Width = 75, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 0, 0, 0) };
            CancelButton.Content = Localization.DialogButton_Cancel;
            CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
            buttonsPanel.Children.Add(CancelButton);

            gridContentContainer = new Grid();
            gridContentContainer.Margin = new Thickness(5);

            PopUpLayoutRoot.Children.Add(gridContentContainer);
            PopUpLayoutRoot.Children.Add(buttonsPanel);
            Grid.SetRow(buttonsPanel, 1);

            m_Dialog.SetContent(PopUpLayoutRoot);
            m_Dialog.DialogClosed += new EventHandler<DialogResultArgs>(m_Dialog_DialogClosed);
            //m_Dialog.Width = 500;
            m_Dialog.MinWidth = OkButton.Width + CancelButton.Width + 20;
            //m_Dialog.Height = 400;
        }

        public UIElement Content
        {
            get
            {
                if (gridContentContainer.Children.Count > 0)
                    return gridContentContainer.Children[0];
                return null;
            }
            set
            {
                gridContentContainer.Children.Clear();
                gridContentContainer.Children.Add(value);
            }
        }

        void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            m_Dialog.Close(); 
        }

        void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResultArgs args = Raise_DialogOk();
            if (args.Cancel)
                return;

            m_Dialog.Close(); 
        }

        public void Show()
        {
            m_Dialog.Show();
        }

        public void Close()
        {
            m_Dialog.Close();
        }

        void m_Dialog_DialogClosed(object sender, DialogResultArgs e)
        {
            DialogResultArgs args = new DialogResultArgs(DialogResult.Cancel);
            EventHandler<DialogResultArgs> handler = DialogCancel;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public event EventHandler<DialogResultArgs> DialogOk;
        public event EventHandler<DialogResultArgs> DialogCancel;

        DialogResultArgs Raise_DialogOk()
        {
            EventHandler<DialogResultArgs> handler = DialogOk;
            DialogResultArgs args = new DialogResultArgs(DialogResult.OK);
            if (handler != null)
            {
                handler(this, args);
            }
            return args;
        }

        public object Tag = null;
    }
}
