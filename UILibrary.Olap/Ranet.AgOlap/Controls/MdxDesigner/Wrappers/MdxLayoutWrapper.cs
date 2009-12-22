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
using System.Collections.Generic;

namespace Ranet.AgOlap.Controls.MdxDesigner.Wrappers
{
    public class MdxLayoutWrapper
    {
        List<AreaItemWrapper> m_Filters;
        public List<AreaItemWrapper> Filters
        {
            get
            {
                if (m_Filters == null)
                {
                    m_Filters = new List<AreaItemWrapper>();
                }
                return m_Filters;
            }
            set { m_Filters = value; }
        }

        List<AreaItemWrapper> m_Rows;
        public List<AreaItemWrapper> Rows
        {
            get
            {
                if (m_Rows == null)
                {
                    m_Rows = new List<AreaItemWrapper>();
                }
                return m_Rows;
            }
            set { m_Rows = value; }
        }

        List<AreaItemWrapper> m_Columns;
        public List<AreaItemWrapper> Columns
        {
            get
            {
                if (m_Columns == null)
                {
                    m_Columns = new List<AreaItemWrapper>();
                }
                return m_Columns;
            }
            set { m_Columns = value; }
        }

        List<AreaItemWrapper> m_Data;
        public List<AreaItemWrapper> Data
        {
            get
            {
                if (m_Data == null)
                {
                    m_Data = new List<AreaItemWrapper>();
                }
                return m_Data;
            }
            set { m_Data = value; }
        }

        public MdxLayoutWrapper()
        { 
        
        }
    }
}
