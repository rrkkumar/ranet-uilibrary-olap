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
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;

namespace Ranet.Olap.Core.Data
{
    public class AxisData
    {
        public AxisData()
        { 
        }

        String m_Name = String.Empty;
        /// <summary>
        /// Название
        /// </summary>
        public String Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        List<PositionData> m_Positions = null;
        /// <summary>
        /// Позиции на оси
        /// </summary>
        public List<PositionData> Positions
        {
            get
            {
                if (m_Positions == null)
                    m_Positions = new List<PositionData>();
                return m_Positions;
            }
            set { m_Positions = value; }
        }

        internal void Serialize(XmlWriter writer)
        {
            if (writer == null)
                return;

            writer.WriteStartElement("AxisData");
            // Свойства
            writer.WriteElementString("Name", this.Name.ToString(CultureInfo.InvariantCulture));
            
            // Позиции - начало
            writer.WriteStartElement("Positions");
            foreach (PositionData pos in Positions)
            {
                pos.Serialize(writer);
            }
            // Позиции - конец
            writer.WriteEndElement();

            writer.WriteEndElement();
        }

        internal static AxisData Deserialize(XmlReader reader)
        {
            if (reader != null)
            {
                try
                {
                    AxisData target = null;
                    if (reader.NodeType == XmlNodeType.Element &&
                        reader.Name == "AxisData")
                    {
                        target = new AxisData();

                        reader.ReadStartElement("AxisData");

                        reader.ReadStartElement("Name");
                        if (reader.NodeType == XmlNodeType.Text)
                        {
                            target.Name = reader.ReadContentAsString();
                            reader.ReadEndElement();
                        }

                        // Позиции 
                        reader.ReadStartElement("Positions");
                        PositionData pos = null;
                        do
                        {
                            pos = PositionData.Deserialize(reader);
                            if (pos != null)
                                target.Positions.Add(pos);
                        } while (pos != null);
                        if (reader.NodeType == XmlNodeType.EndElement &&
                            reader.Name == "Positions")
                        {
                            reader.ReadEndElement();
                        }

                        if (reader.NodeType == XmlNodeType.EndElement &&
                            reader.Name == "AxisData")
                        {
                            reader.ReadEndElement();
                        }
                    }
                    return target;
                }
                catch (XmlException ex)
                {
                    throw ex;
                    //return null;
                }
            }
            return null;
        }
    }
}
