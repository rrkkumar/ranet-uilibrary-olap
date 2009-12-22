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
    public interface IPropertiesData
    {
        List<PropertyData> Properties { get; }
        PropertyData GetProperty(String name);
    }

    public class PropertyData
    {
        String m_Name = String.Empty;
        public String Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        Object m_Value = null;
        public Object Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        public PropertyData()
        { 
        
        }

        public PropertyData(String name, object value)
        {
            m_Name = name;
            m_Value = value;
        }

        internal void Serialize(XmlWriter writer)
        {
            if (writer == null)
                return;

            writer.WriteStartElement("PropertyData");
            // Свойства
            writer.WriteElementString("Name", this.Name.ToString(CultureInfo.InvariantCulture));
            if (Value != null)
            {
                writer.WriteStartElement("Value");
                writer.WriteAttributeString("Type", Value.GetType().FullName.ToString(CultureInfo.InvariantCulture));
                object val = Convert.ChangeType(Value, typeof(string), CultureInfo.InvariantCulture);
                writer.WriteValue(val);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        internal static PropertyData Deserialize(XmlReader reader)
        {
            if (reader != null)
            {
                try
                {
                    PropertyData target = null;
                    if (reader.NodeType == XmlNodeType.Element &&
                        reader.Name == "PropertyData")
                    {
                        target = new PropertyData();

                        reader.ReadStartElement("PropertyData");

                        reader.ReadStartElement("Name");
                        if (reader.NodeType == XmlNodeType.Text)
                        {
                            target.Name = reader.ReadContentAsString();
                            reader.ReadEndElement();
                        }

                        String type = null;
                        if (reader.MoveToAttribute("Type"))
                        {
                            if (reader.NodeType == XmlNodeType.Attribute &&
                                reader.Name == "Type" &&
                                reader.ReadAttributeValue())
                            {
                                type = reader.Value;
                            }
                        }

                        String value = null;
                        if (reader.MoveToElement())
                        {
                            if (reader.NodeType == XmlNodeType.Element &&
                                reader.Name == "Value")
                                reader.ReadStartElement("Value");
                            if (reader.NodeType == XmlNodeType.Text)
                            {
                                value = reader.ReadContentAsString();
                            }
                            reader.ReadEndElement();
                        }

                        if (value != null && !String.IsNullOrEmpty(type))
                        {
                            target.Value = Convert.ChangeType(value, Type.GetType(type), CultureInfo.InvariantCulture);
                        }

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
    }
}
