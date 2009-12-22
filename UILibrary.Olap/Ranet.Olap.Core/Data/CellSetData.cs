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
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Globalization;

namespace Ranet.Olap.Core.Data
{
    public class CellSetData
    {
        string m_CubeName = String.Empty;
        public string CubeName
        {
            get { return m_CubeName; }
            set { m_CubeName = value; }
        }

        ConnectionInfo m_Connection = new ConnectionInfo();
        public ConnectionInfo Connection
        {
            get {
                if (m_Connection == null)
                    m_Connection = new ConnectionInfo();
                return m_Connection; 
            }
            set { m_Connection = value; }
        }


        List<AxisData> m_Axes = null;
        /// <summary>
        /// Оси
        /// </summary>
        public List<AxisData> Axes
        {
            get
            {
                if (m_Axes == null)
                    m_Axes = new List<AxisData>();
                return m_Axes;
            }
            set { m_Axes = value; }
        }

        List<CellData> m_Cells = null;
        /// <summary>
        /// Ячейки
        /// </summary>
        public List<CellData> Cells
        {
            get
            {
                if (m_Cells == null)
                    m_Cells = new List<CellData>();
                return m_Cells;
            }
            set { 
                m_Cells = value;
                m_Cells2D = new Cache2D<CellData>();
            }
        }

        // Пока информация о FilterAxis не нужна
        //AxisData m_FilterAxis = null;
        //public AxisData FilterAxis
        //{
        //    get { return m_FilterAxis; }
        //    set { m_FilterAxis = value; }
        //}

        public CellData GetCellDescription(int col)
        {
            if (col >= 0)
            {
                foreach (CellData cell in Cells)
                {
                    if (cell.Axis0_Coord == col)
                    {
                        return cell;
                    }
                }
            }

            return null;
        }

        public CellData GetCellDescription(int col, int row)
        {
            CellData cell = m_Cells2D[col, row];
            if (cell == null)
            {
                if (col >= 0 && row >= 0)
                {
                    foreach (CellData c in Cells)
                    {
                        if (c.Axis0_Coord == col &&
                            c.Axis1_Coord == row)
                        {
                            m_Cells2D.Add(c, col, row);
                            return c;
                        }
                    }
                }
            }

            return cell;
        }

        public static CellSetData Parse(string xml)
        {
            return XmlSerializationUtility.XmlStr2Obj<CellSetData>(xml);
        }

        Cache2D<CellData> m_Cells2D = new Cache2D<CellData>();

        internal void Serialize(XmlWriter writer)
        {
            if (writer == null)
                return;

            // Начало
            writer.WriteStartElement("CellSetData");
            // Свойства
            writer.WriteElementString("CubeName", this.CubeName.ToString(CultureInfo.InvariantCulture));
            // Соедиение
            Connection.Serialize(writer);
            
            // Оси - начало
            writer.WriteStartElement("Axes");
            foreach (AxisData axis in Axes)
            {
                axis.Serialize(writer);
            }
            // Оси - конец
            writer.WriteEndElement();

            // Ячейки - начало
            writer.WriteStartElement("Cells");
            foreach (CellData cell in Cells)
            {
                cell.Serialize(writer);
            }
            // Ячейки - конец
            writer.WriteEndElement();

            // Конец
            writer.WriteEndElement();
        }

        public static String Serialize(CellSetData cs)
        {
            StringBuilder sb = new StringBuilder();

            if (cs != null)
            {
                //XmlWriterSettings settings = new XmlWriterSettings();
                //settings.ConformanceLevel = ConformanceLevel.Fragment;

                //XmlWriter writer = XmlWriter.Create(sb, settings);
                XmlWriter writer = XmlWriter.Create(sb);
                cs.Serialize(writer);
                writer.Close();


            }
            return sb.ToString();
        }

        internal static CellSetData Deserialize(XmlReader reader)
        {
            if (reader != null)
            {
                try
                {
                    if (!(reader.NodeType == XmlNodeType.Element &&
                        reader.Name == "CellSetData"))
                    {
                        reader.ReadToFollowing("CellSetData");
                    }

                    CellSetData target = new CellSetData();
                    // Начало - CellSetData
                    reader.ReadStartElement("CellSetData");

                    // Свойства
                    reader.ReadStartElement("CubeName");
                    if (reader.NodeType == XmlNodeType.Text)
                    {
                        target.CubeName = reader.ReadContentAsString();
                        reader.ReadEndElement();
                    }

                    // Соединение
                    target.Connection = ConnectionInfo.Deserialize(reader);

                    // Оси
                    reader.ReadStartElement("Axes");
                    AxisData axis = null;
                    do
                    {
                        axis = AxisData.Deserialize(reader);
                        if (axis != null)
                            target.Axes.Add(axis);
                    } while (axis != null);
                    if (reader.NodeType == XmlNodeType.EndElement &&
                        reader.Name == "Axes")
                    {
                        reader.ReadEndElement();
                    }

                    // Ячейки
                    reader.ReadStartElement("Cells");
                    CellData cell = null;
                    do
                    {
                        cell = CellData.Deserialize(reader);
                        if (cell != null)
                            target.Cells.Add(cell);
                    } while (cell != null);
                    if (reader.NodeType == XmlNodeType.EndElement &&
                        reader.Name == "Cells")
                    {
                        reader.ReadEndElement();
                    }

                    // Конец - CellSetData
                    if (reader.NodeType == XmlNodeType.EndElement &&
                        reader.Name == "CellSetData")
                    {
                        reader.ReadEndElement();
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

        public static CellSetData Deserialize(String str)
        {
            if (!String.IsNullOrEmpty(str))
            {
                StringReader str_reader = new StringReader(str);
                XmlReader reader = XmlReader.Create(str_reader);

                return CellSetData.Deserialize(reader);
            }
            else
                return null;
        //    StringReader str_reader = new StringReader(str);
        //    XmlReader reader = XmlReader.Create(str_reader);

        //    XmlWriter writer = XmlWriter.Create(reader);
        //    this.Serialize(writer);
        //    writer.Close();
        }
    }
}
