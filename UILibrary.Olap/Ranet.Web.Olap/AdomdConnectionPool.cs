/*   
    Copyright (C) 2009 Galaktika Corporation ZAO

    This file is a part of Ranet.UILibrary.Olap
 
    Ranet.UILibrary.Olap is a free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
      
    You should have received a copy of the GNU General Public License
    along with Ranet.UILibrary.Olap.  If not, see <http://www.gnu.org/licenses/>.
  
    If GPL v.3 is not suitable for your products or company,
    Galaktika Corp provides Ranet.UILibrary.Olap under a flexible commercial license
    designed to meet your specific usage and distribution requirements.
    If you have already obtained a commercial license from Galaktika Corp,
    you can use this file under those license terms.
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Text;
using System.Web;

using Microsoft.AnalysisServices.AdomdClient;

namespace Ranet.Web.Olap
{
    public static class AdomdConnectionPool
    {
        public static AdomdConnection GetConnection(string connectionString)
        {
            //AdomdConnection conn = new AdomdConnection(connectionString);
            AdomdConnection conn = null;
            if (HttpContext.Current != null &&
                HttpContext.Current.Application != null)
            {
                conn = HttpContext.Current.Application[connectionString] as AdomdConnection;
                if(conn != null && conn.State == ConnectionState.Closed)
                    conn.Open();
            }

            if (conn == null)
            {
                conn = new AdomdConnection(connectionString);
                conn.Open();

                if (HttpContext.Current != null &&
                    HttpContext.Current.Application != null)
                {
                    HttpContext.Current.Application[connectionString] = conn;
                }
            }

            return conn;
        }

        public static void Clear()
        {
            foreach (object obj in HttpContext.Current.Session)
            {
                AdomdConnection conn = obj as AdomdConnection;
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
    }
}
