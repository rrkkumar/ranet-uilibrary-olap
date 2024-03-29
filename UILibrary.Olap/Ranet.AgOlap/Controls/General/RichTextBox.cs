﻿/*   
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
using Ranet.AgOlap.Features;
using System.Windows.Browser;
using Ranet.AgOlap.Controls.ContextMenu;

namespace Ranet.AgOlap.Controls.General
{
    public class RichTextBox : TextBox
    {

        public RichTextBox()
        {
            DefaultStyleKey = typeof(RichTextBox);

            base.MouseEnter += new MouseEventHandler(RichTextBox_MouseEnter);
            base.MouseLeave += new MouseEventHandler(RichTextBox_MouseLeave);
            this.IsMouseWheelAttached = false;
        }

        public ScrollViewer GetScroller()
        {
            return base.GetTemplateChild("ContentElement") as ScrollViewer;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var readOnlyVisualElement = base.GetTemplateChild("ReadOnlyVisualElement") as Border;
            if (readOnlyVisualElement != null)
            {
                readOnlyVisualElement.Background = new SolidColorBrush(Colors.Transparent);
            }
        } 

        public bool IsMouseWheelAttached { get; set; }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            if (!this.IsMouseWheelAttached)
            {
                this.AddMouseWheelSupport();
            }
        }

        void itemCut_ItemClick(object sender, EventArgs e)
        {
            itemCopy_ItemClick(sender, e);
            itemDelete_ItemClick(sender, e);
        }

        void itemCopy_ItemClick(object sender, EventArgs e)
        {
            this.Focus();
            Clipboard.SetClipboardText(base.SelectedText);
        }

        void itemDelete_ItemClick(object sender, EventArgs e)
        {
            this.Focus();
            base.Text = base.Text.Remove(base.SelectionStart, base.SelectionLength);
        }

        void itemPaste_ItemClick(object sender, EventArgs e)
        {
            this.Focus();
            Clipboard.SetClipboardText(base.SelectedText);
        }

        void itemSelectAll_ItemClick(object sender, EventArgs e)
        {
            this.Focus();
            base.SelectAll();
        }

        void RichTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            HtmlPage.Document.DetachEvent("oncontextmenu", new EventHandler<HtmlEventArgs>(ContentMenu_EventHandler));
        }

        void RichTextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            HtmlPage.Document.AttachEvent("oncontextmenu", new EventHandler<HtmlEventArgs>(ContentMenu_EventHandler));
        }

        ContextMenuItem m_ItemDelete;
        ContextMenuItem m_ItemCopy;
        ContextMenuItem m_ItemCut;
        ContextMenuItem m_ItemPaste;
        protected virtual CustomContextMenu CreateContextMenu()
        {
            CustomContextMenu menu = new CustomContextMenu();
            m_ItemCut = new ContextMenuItem(Localization.ContextCmd_Cut);
            m_ItemCut.Icon = UriResources.Images.Cut16;
            m_ItemCut.ItemClick += new EventHandler(itemCut_ItemClick);
            menu.AddMenuItem(m_ItemCut);
            m_ItemCopy = new ContextMenuItem(Localization.ContextCmd_Copy);
            m_ItemCopy.Icon = UriResources.Images.Copy16;
            m_ItemCopy.ItemClick += new EventHandler(itemCopy_ItemClick);
            menu.AddMenuItem(m_ItemCopy);
            m_ItemPaste = new ContextMenuItem(Localization.ContextCmd_Paste);
            m_ItemPaste.Icon = UriResources.Images.Paste16;
            m_ItemPaste.ItemClick += new EventHandler(itemPaste_ItemClick);
            menu.AddMenuItem(m_ItemPaste);
            m_ItemDelete = new ContextMenuItem(Localization.ContextCmd_Delete);
            m_ItemDelete.Icon = UriResources.Images.Delete16;
            m_ItemDelete.ItemClick += new EventHandler(itemDelete_ItemClick);
            menu.AddMenuItem(m_ItemDelete);
            menu.AddMenuSplitter();
            ContextMenuItem item = new ContextMenuItem(Localization.ContextCmd_SelectAll);
            item.ItemClick += new EventHandler(itemSelectAll_ItemClick);
            menu.AddMenuItem(item);

            return menu;
        }

        private CustomContextMenu m_ContextMenu;
        protected CustomContextMenu ContextMenu
        {
            get
            {
                if (m_ContextMenu == null)
                {
                    m_ContextMenu = this.CreateContextMenu();
                }

                m_ItemCopy.IsEnabled = base.SelectionLength > 0;
                m_ItemDelete.IsEnabled = m_ItemCut.IsEnabled =
                    m_ItemCopy.IsEnabled && !base.IsReadOnly;
                m_ItemPaste.IsEnabled = !base.IsReadOnly && !string.IsNullOrEmpty(Clipboard.GetClipboardText());

                return m_ContextMenu;
            }
        }

        void ContentMenu_EventHandler(object sender, HtmlEventArgs e)
        {
            Rect bounds = AgControlBase.GetSLBounds(this);
            if (bounds.Contains(new Point(e.ClientX, e.ClientY)))
            {
                e.PreventDefault();
                e.StopPropagation();
                this.ContextMenu.SetLocation(new Point(e.OffsetX, e.OffsetY));
                this.ContextMenu.IsDropDownOpen = true;
            }
        }
    }

    public class SimpleTextBox : TextBox
    {
        public SimpleTextBox()
        {
            DefaultStyleKey = typeof(SimpleTextBox);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var readOnlyVisualElement = base.GetTemplateChild("ReadOnlyVisualElement") as Border;
            if (readOnlyVisualElement != null)
            {
                readOnlyVisualElement.Background = new SolidColorBrush(Colors.Transparent);
            }
        } 
    }

    public class SingleLineTextBox : SimpleTextBox
    {
        public SingleLineTextBox()
            : base()
        {
            
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var scrollViewer = base.GetTemplateChild("ContentElement") as ScrollViewer;
            if (scrollViewer != null)
            {
                scrollViewer.Style = null;
            }
        } 
    }
}
