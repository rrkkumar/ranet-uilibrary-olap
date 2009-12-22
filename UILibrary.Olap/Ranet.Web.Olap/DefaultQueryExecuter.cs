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

using Microsoft.AnalysisServices.AdomdClient;

namespace Ranet.Web.Olap
{
    using System.Text;
    using Ranet.Olap.Core.Data;
    using Ranet.Olap.Core;
    using Ranet.Olap.Core.Providers;

    public class DefaultQueryExecuter : IQueryExecuter
    {
        ConnectionInfo m_Connection = null;
        public ConnectionInfo Connection
        {
            get {
                if (m_Connection == null)
                    m_Connection = new ConnectionInfo();
                return m_Connection; 
            }
        }

        public DefaultQueryExecuter(ConnectionInfo connection)
        {
            m_Connection = connection;
        }

        public bool Cancel()
        {
            // TODO: Lock
            if (m_CurrentCmd != null)
            {
                try
                {
                    m_CurrentCmd.Cancel();
                }
                catch (AdomdErrorResponseException)
                {
                }
                finally
                {
                    m_CurrentCmd = null;
                }
            }

            return true;
        }

        private AdomdCommand m_CurrentCmd;

        public int ExecuteNonQuery(string query)
        {
            try
            {
                if (Connection != null)
                {
                    AdomdConnection conn = AdomdConnectionPool.GetConnection(Connection.ConnectionString);
                    using (AdomdCommand cmd = new AdomdCommand(query, conn))
                    {
                        return cmd.ExecuteNonQuery();
                    }
                }
                return 0;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public CellSetData ExecuteQuery(string query)
        {
            try
            {
                if (Connection != null)
                {
                    AdomdConnection conn = AdomdConnectionPool.GetConnection(Connection.ConnectionString);
                    using (AdomdCommand cmd = new AdomdCommand(query, conn))
                    {
                        CellSet cs = cmd.ExecuteCellSet();

                        CellSetDescriptionProvider provider = new CellSetDescriptionProvider(cs);
                        provider.CellSet.Connection.ConnectionString = Connection.ConnectionString;
                        provider.CellSet.Connection.ConnectionID = Connection.ConnectionID;
                        if (cs.OlapInfo != null &&
                            cs.OlapInfo.CubeInfo != null &&
                            cs.OlapInfo.CubeInfo.Cubes != null &&
                            cs.OlapInfo.CubeInfo.Cubes.Count > 0)
                        {
                            provider.CellSet.CubeName = cs.OlapInfo.CubeInfo.Cubes[0].CubeName;
                        }

                        return provider.CellSet;
                    }
                }
                return null;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}
