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
using Ranet.AgOlap.Controls.Forms;
using Ranet.Olap.Core.Storage;
using Ranet.Olap.Core;
using Ranet.AgOlap.Controls.Storage;
using System.Collections.Generic;

namespace Ranet.AgOlap.Controls.General
{
    public class ObjectSaveAsDialog
    {
        IStorageManager m_StorageManager = null;
        ObjectDescriptionControl m_Description;
        ObjectDescriptionListControl m_List;
        ModalDialog m_Dlg;

        ILogService m_LogManager = null;
        public virtual ILogService LogManager
        {
            get
            {
                if (m_LogManager == null)
                    m_LogManager = new DefaultLogManager();
                return m_LogManager;
            }
            set
            {
                m_LogManager = value;
            }
        }

        public ObjectSaveAsDialog(IStorageManager storageManager)
        {
            m_StorageManager = storageManager;
            m_Dlg = new ModalDialog();

            m_Dlg.MinHeight = 200;
            m_Dlg.MinWidth = 300;
            m_Dlg.Height = 400;
            m_Dlg.Width = 500;
            m_Dlg.Caption = Localization.SaveAsDialog_Caption;
            m_Dlg.DialogOk += new EventHandler<DialogResultArgs>(Dlg_DialogOk);

            Grid LayoutRoot = new Grid();
            LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition());
            LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition());

            m_List = new ObjectDescriptionListControl();
            m_List.SelectionChanged += new EventHandler<CustomEventArgs<ObjectStorageFileDescription>>(m_List_SelectionChanged);
            m_List.ObjectSelected += new EventHandler<CustomEventArgs<ObjectStorageFileDescription>>(m_List_ObjectSelected);
            LayoutRoot.Children.Add(m_List);

            GridSplitter splitter_Vert = new GridSplitter();
            splitter_Vert.IsTabStop = false;
            LayoutRoot.Children.Add(splitter_Vert);
            Grid.SetColumn(splitter_Vert, 0);
            Grid.SetRow(splitter_Vert, 0);
            splitter_Vert.Background = new SolidColorBrush(Colors.Transparent);
            splitter_Vert.HorizontalAlignment = HorizontalAlignment.Right;
            splitter_Vert.VerticalAlignment = VerticalAlignment.Stretch;
            splitter_Vert.Width = 3;

            m_Description = new ObjectDescriptionControl() { Margin = new Thickness(5, 0, 0, 0) };
            LayoutRoot.Children.Add(m_Description);
            Grid.SetColumn(m_Description, 1);

            m_Dlg.Content = LayoutRoot;

            if (m_StorageManager != null)
                m_StorageManager.InvokeCompleted += new EventHandler<DataLoaderEventArgs>(m_StorageManager_ActionCompleted);
        }

        void m_List_SelectionChanged(object sender, CustomEventArgs<ObjectStorageFileDescription> e)
        {
            // Выводим информацию о текущем выбранном элементе
            if (m_List.CurrentObject != null)
                m_Description.Object = m_List.CurrentObject.Description;
            else
                m_Description.Object = null;
        }

        void m_List_ObjectSelected(object sender, CustomEventArgs<ObjectStorageFileDescription> e)
        {
            // Выводим предупреждение о затирании объекта
            if (MessageBox.Show(Localization.ObjectSaveDialog_Replace_Message, Localization.Warning, MessageBoxButton.OKCancel) != MessageBoxResult.OK)
            {
                return;
            }
            m_Dlg.Close();
            EventHandler handler = DialogOk;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
        
        void m_StorageManager_ActionCompleted(object sender, DataLoaderEventArgs e)
        {
            if (e.Error != null)
            {
                LogManager.LogException(Localization.PivotGridControl_Name, e.Error);
                return;
            }

            if (e.Result.ContentType == InvokeContentType.Error)
            {
                LogManager.LogMessage(Localization.PivotGridControl_Name, Localization.Error + "! " + e.Result.Content);
                return;
            }

            StorageActionArgs args = e.UserState as StorageActionArgs;
            if (args != null)
            {
                if (args.ActionType == StorageActionTypes.GetList)
                {
                    List<ObjectStorageFileDescription> list = XmlSerializationUtility.XmlStr2Obj<List<ObjectStorageFileDescription>>(e.Result.Content);
                    m_List.Initialize(list);
                    m_List.IsWaiting = false;
                }
            }
        }

        public ObjectDescription Object
        {
            get { return m_Description.Object; }
        }

        public event EventHandler DialogOk;
        
        void Dlg_DialogOk(object sender, DialogResultArgs e)
        {
            if (m_Description.Object != null)
            {
                if (String.IsNullOrEmpty(m_Description.Object.Name))
                {
                    MessageBox.Show(Localization.ObjectSaveDialog_NameIsEmpty_Message, Localization.Warning, MessageBoxButton.OK);
                    return;
                }
                else
                {
                    if (m_List.Contains(m_Description.Object.Name))
                    {
                        // Выводим предупреждение о затирании объекта
                        if(MessageBox.Show(Localization.ObjectSaveDialog_Replace_Message, Localization.Warning, MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                        {
                            return;
                        }
                    }
                }

                m_Dlg.Close();
                EventHandler handler = DialogOk;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }

        StorageContentTypes m_ContentType = StorageContentTypes.None;
        public StorageContentTypes ContentType
        {
            get { return m_ContentType; }
            set { m_ContentType = value; }
        }

        public void Show()
        {
            m_Dlg.Show();

            m_List.Initialize(null);
            m_List.IsWaiting = true;

            StorageActionArgs args = new StorageActionArgs();
            args.ActionType = StorageActionTypes.GetList;
            args.ContentType = ContentType;
            m_StorageManager.Invoke(XmlSerializationUtility.Obj2XmlStr(args, Common.Namespace), args);
        }
    }
}
