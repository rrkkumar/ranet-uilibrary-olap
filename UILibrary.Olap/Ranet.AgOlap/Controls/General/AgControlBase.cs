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

namespace Ranet.AgOlap.Controls.General
{
    public interface ILogService
    {
        void LogException(String caption, Exception ex);
        void LogMessage(String caption, String message);
    }

    public class DefaultLogManager : ILogService
    {
        #region ILogService Members
        public void LogException(String caption, Exception ex)
        {
            //throw ex;
            if (ex != null)
            {
                MessageBox.Show(ex.Message, caption, MessageBoxButton.OK);
            }
        }

        public void LogMessage(String caption, string message)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK);
        }
        #endregion
    }


    public class AgControlBase : UserControl
    {
        public virtual String URL{ get; set; }

        ILogService m_LogManager = null;
        public virtual ILogService LogManager
        {
            get {
                if (m_LogManager == null)
                    m_LogManager = new DefaultLogManager();
                return m_LogManager;
            }
            set {
                m_LogManager = value;
            }
        }

        public static Rect GetSLBounds(FrameworkElement item)
        {
            Point pos = AgControlBase.GetSilverlightPos(item);
            return new Rect(pos, new Size(item.ActualWidth, item.ActualHeight));
        }

        public static Point GetSilverlightPos(UIElement item)
        {
            Point point = new Point(0, 0);

            if (item != null)
            {
                try
                {
                    Point transformPoint = Application.Current.RootVisual.TransformToVisual(item).Transform(point);
                    return new Point(transformPoint.X * -1, transformPoint.Y * -1);
                }
                catch (ArgumentException ex)
                {
                }
            }

            return point;
        }
    }
}
