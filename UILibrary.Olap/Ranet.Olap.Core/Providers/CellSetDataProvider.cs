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
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ranet.Olap.Core.Data;

namespace Ranet.Olap.Core.Providers
{
    public class CellSetDataProvider : IPivotGridDataProvider
    {
        public CellSetDataProvider(CellSetData cs_descr)
        {
            m_CellSet_Descr = cs_descr;
        }

        //public CellData GetCellDescription(int col, int row)
        //{
        //    if (m_CellSet_Descr != null && col >= 0 && row >= 0)
        //    {
        //        foreach (CellData cell in m_CellSet_Descr.Cells)
        //        {
        //            if (cell.Axis0_Coord == col &&
        //                cell.Axis1_Coord == row)
        //            {
        //                return cell;
        //            }
        //        }
        //    }
        //    return null;
        //}

        Dictionary<CellData, CellInfo> m_CellInfos = new Dictionary<CellData, CellInfo>();
        public CellInfo GetCellInfo(int column_index, int row_index)
        {
            CellInfo cell_Info = null;
            CellData cell_data = null;

            if (m_CellSet_Descr != null && 
                Rows != null && // чтобы проинициализировать
                Columns != null && // чтобы проинициализировать
                column_index >= 0 &&
                column_index < m_Columns_LowestMembers.Count &&
                row_index < m_Rows_LowestMembers.Count) 
            {
                if(row_index >= 0)
                    cell_data = m_CellSet_Descr.GetCellDescription(column_index, row_index);
                else
                    cell_data = m_CellSet_Descr.GetCellDescription(column_index);
                
                if (cell_data != null)
                {
                    if (m_CellInfos.ContainsKey(cell_data))
                    {
                        cell_Info = m_CellInfos[cell_data];
                    }
                    else
                    {
                        if (row_index >= 0)
                        {
                            cell_Info = new CellInfo(cell_data,
                               m_Columns_LowestMembers[column_index],
                               m_Rows_LowestMembers[row_index], 
                               GetInvisibleCoords(column_index, row_index));
                        }
                        else
                        {
                            cell_Info = new CellInfo(cell_data,
                               m_Columns_LowestMembers[column_index],
                               MemberInfo.Empty, 
                               GetInvisibleCoords(column_index, row_index));
                        }
                        m_CellInfos.Add(cell_data, cell_Info);
                    }
                }
            }
            return cell_Info;
        }

        public CellInfo GetCellByTuple(IDictionary<String, MemberInfo> tuple)
        {
            if (tuple != null)
            {
                // Придется перебирать все ячейки
                for (int col_index = 0; col_index < Columns_Size; col_index++)
                {
                    for (int row_index = 0; row_index < Rows_Size; row_index++)
                    {
                        CellInfo info = GetCellInfo(col_index, row_index);
                        if (info != null && info.CompareByTuple(tuple))
                            return info;
                    }
                }
            }
            return null;
        }

        #region IPivotGridDataProvider Members

        private CellSetData m_CellSet_Descr = null;
        public CellSetData CellSet_Description
        {
            get {
                return m_CellSet_Descr;
            }
        }

        private Dictionary<string, object> m_Properties;
        public IDictionary<string, object> Properties
        {
            get
            {
                if (m_Properties == null)
                {
                    m_Properties = new Dictionary<string, object>();
                    m_Properties.Add("CONNECTION_STRING", "");
                }

                return m_Properties;
            }
        }

        private MemberInfoCollection m_Columns;
        public MemberInfoCollection Columns
        {
            get
            {
                if (m_Columns == null)
                {
                    m_Columns = CreateFields(0);
                }

                return m_Columns;
            }
        }

        private MemberInfoCollection m_Rows;
        public MemberInfoCollection Rows
        {
            get
            {
                if (m_Rows == null)
                {
                    m_Rows = CreateFields(1);
                }

                return m_Rows;
            }
        }

        List<MemberInfo> m_Columns_LowestMembers = new List<MemberInfo>();
        /// <summary>
        /// Количество элементов на последней линии в колонках
        /// </summary>
        public int Columns_Size
        {
            get {
                return m_Columns_LowestMembers.Count;
            }
        }
        List<MemberInfo> m_Rows_LowestMembers = new List<MemberInfo>();
        /// <summary>
        /// Количество элементов на последней линии в строках
        /// </summary>
        public int Rows_Size
        {
            get
            {
                return m_Rows_LowestMembers.Count;
            }
        }

        private MemberInfoCollection CreateFields(int axisNum)
        {
            if (axisNum == 0)
                m_Columns_LowestMembers.Clear();
            if (axisNum == 1)
                m_Rows_LowestMembers.Clear();

            MemberInfoCollection fields = new MemberInfoCollection(null);
            Dictionary<int, List<MemberInfo>> tmp = new Dictionary<int, List<MemberInfo>>();

            if (m_CellSet_Descr != null && m_CellSet_Descr.Axes.Count > axisNum)
            {
                int position_Index = 0;
                foreach (PositionData pos in m_CellSet_Descr.Axes[axisNum].Positions)
                {
                    MemberInfoCollection container = fields;
                    int depth = pos.Members.Count;
                    for (int i = 0; i < pos.Members.Count; i++)
                    {
                        List<MemberInfo> line = null;
                        if (tmp.ContainsKey(i))
                        {
                            line = tmp[i];
                        }
                        else
                        {
                            line = new List<MemberInfo>();
                            tmp.Add(i, line);
                        }
                        
                        MemberData member = pos.Members[i];

                        // Если PARENT_UNIQUE_NAME запрашивалось то берем его, иначе будем спрашивать у куба 
                        String parentUniqueName = GetMemberPropertyValue(member, "PARENT_UNIQUE_NAME");
                        
                        // Если родитель является DrilledDown, то данный элемент должен попасть в коллекцию DrilledDownChildren
                        MemberInfo parentInfo = null;
                        try
                        {
                            if (!String.IsNullOrEmpty(parentUniqueName))
                            {
                                int posIndex = ReversePos(line, parentUniqueName);
                                if (posIndex > -1)
                                {
                                    parentInfo = line[posIndex];
                                }

                                if (parentInfo != null && parentInfo.DrilledDown)
                                {
                                    // Если линия не нулевая. То объект должен попать в коллекцию DrilledDown только в том случае если и владелец данной коллекции и сам элемент, который мы проверяем пересекаются с одним и тем же элементом
                                    // Ситуацию можно увидеть в запросе 
                                    /*select [Measures].[Internet Sales Amount] on 0,
                                    {([Date].[Calendar].[Calendar Year].&[2001],  [Product].[Product Categories].[Category].[Bikes]),
                                    ([Date].[Calendar].[Calendar Semester].&[2001]&[2], [Product].[Product Categories].[Subcategory].[Mountain Bikes]),
                                    ([Date].[Calendar].[Calendar Year].&[2001], [Product].[Product Categories].[Category].[Clothing])}
                                     on 1
                                    from [Adventure Works]*/
                                    if (i == 0 ||
                                        (i > 0 && tmp[i - 1][posIndex] != null && tmp[i - 1][position_Index] != null && tmp[i - 1][posIndex] == tmp[i - 1][position_Index]))
                                    {
                                        container = parentInfo.DrilledDownChildren;
                                    }
                                }
                            }
                            else
                            {
                                // Если это нулевой элемент на линии, то он однозначно должен находиться в коллекции Children
                                // Иначе он может попасть в коллекцию DrilledDown если у предыдущего элемента установлен флаг DrilledDown 
                                if (position_Index > 0)
                                {
                                    MemberInfo prevInLine = line[line.Count - 1];
                                    if (prevInLine != null && prevInLine.UniqueName != member.UniqueName && prevInLine.DrilledDown)
                                    {
                                        // Для вычисляемых элементов свойство DrilledDown работает неправильно. И в этом случае считаем что для того чтобы элемент попал в коллекцию DrilledDown у него должна быть и глубина уровня больше чем у предыдущего
                                        if (prevInLine.LevelDepth < member.LevelDepth)
                                        {
                                            // Если линия не нулевая. То объект должен попать в коллекцию DrilledDown только в том случае если и владелец данной коллекции и сам элемент, который мы проверяем пересекаются с одним и тем же элементом
                                            // Ситуацию можно увидеть в запросе 
                                            /*select [Measures].[Internet Sales Amount] on 0,
                                            {([Date].[Calendar].[Calendar Year].&[2001],  [Product].[Product Categories].[Category].[Bikes]),
                                            ([Date].[Calendar].[Calendar Semester].&[2001]&[2], [Product].[Product Categories].[Subcategory].[Mountain Bikes]),
                                            ([Date].[Calendar].[Calendar Year].&[2001], [Product].[Product Categories].[Category].[Clothing])}
                                             on 1
                                            from [Adventure Works]*/
                                            if (i == 0 ||
                                                (i > 0 && tmp[i - 1][line.Count - 1] != null && tmp[i - 1][position_Index] != null && tmp[i - 1][line.Count - 1] == tmp[i - 1][position_Index]))
                                            {
                                                container = prevInLine.DrilledDownChildren;
                                            }
                                        }
                                        else
                                        {
                                            // Пытаемся исправить то, что для вычисляемых элементов свойство DrilledDown работает неправильно.
                                            prevInLine.DrilledDown = false;
                                        }
                                    }
                                }
                            }
                        }
                        catch (System.NotSupportedException)
                        { 
                        }

                        // Если элемент существует, то он должен быть последним
                        // Если он окажется не последним, то это значит что эти элементы объединению не подлежат и нужно создавать новый элемент
                        MemberInfo field = null;
                        if (line.Count > 0)
                            field = line[line.Count - 1];
                        // Элементы последней линии объединению не подлежат
                        if (field == null || field.UniqueName != member.UniqueName || i == (depth - 1) || (field.Parent != null && container.m_Owner != null && field.Parent != container.m_Owner))
                        {
                            field = CreateMemberInfo(member);
                            line.Add(field);
                            // Важно чтобы добавление в контейнер было после добавления в линию, т.к. в Add идет установка Parent
                            container.Add(field);

                            if (i == (depth - 1))
                            {
                                if (axisNum == 0)
                                    m_Columns_LowestMembers.Add(field);
                                if (axisNum == 1)
                                    m_Rows_LowestMembers.Add(field);
                            }
                        }
                        else
                        {
                            field.DrilledDown = field.DrilledDown | member.DrilledDown;
                            line.Add(field);
                        }

                        container = field.Children; 
                    }
                    position_Index++;
                }
            }

            return fields;
        }

        public int ReversePos(List<MemberInfo> members, String uniqueName)
        {
            if (members != null)
            {
                for (int i = members.Count - 1; i >= 0; i--)
                {
                    MemberInfo mi = members[i];
                    if (mi.UniqueName == uniqueName)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private String GetMemberPropertyValue(MemberData member, String propName)
        {
            if (member == null || member.MemberProperties == null || String.IsNullOrEmpty(propName))
                return null;
            foreach(PropertyData prop in member.MemberProperties)
            if (prop.Name == propName && prop.Value != null)
            {
                return prop.Value.ToString();
            }
            return null;
        }

        private MemberInfo CreateInvisibleMemberInfo(MemberData member)
        {
            if (member == null)
                return null;

            MemberInfo info = new InvisibleMemberInfo();
            InitMemberInfo(info, member);

            return info;
        }    

        private MemberInfo CreateMemberInfo(MemberData member)
        {
            if (member == null)
                return null;

            MemberInfo info = new MemberInfo();
            InitMemberInfo(info, member);

            return info;
        }    
    
        private void InitMemberInfo(MemberInfo info, MemberData member)
        {
            if (info == null || member == null)
                return;

            info.Caption = member.Caption;
            info.ChildCount = member.ChildCount;
            info.Description = member.Description;
            info.DrilledDown = member.DrilledDown;
            info.LevelDepth = member.LevelDepth;
            info.LevelName = member.LevelName;
            info.Name = member.Name;
            info.ParentSameAsPrevious = member.ParentSameAsPrevious;

            info.HierarchyUniqueName = member.HierarchyUniqueName;
            info.Custom_Rollup = member.Custom_Rollup;
            info.Unary_Operator = member.Unary_Operator;

            info.UniqueName = member.UniqueName;

            // В коллекцию свойств добавляем MemberProperties
            foreach (PropertyData pair in member.MemberProperties)
            {
                String caption = pair.Name;
                if (caption.StartsWith("-") && caption.EndsWith("-"))
                    caption = caption.Trim('-');

                if (!info.PropertiesDictionary.ContainsKey(caption))
                    info.PropertiesDictionary.Add(caption, pair.Value);
            }
        }

        //private void CreateHeaders(IList<PivotAreaData> list, int axisIndex)
        //{
        //    if (m_CellSet.Axes.Count > axisIndex)
        //    {
        //        foreach (Hierarchy h in m_CellSet.Axes[axisIndex].Set.Hierarchies)
        //        {
        //            list.Add(new PivotAreaData(h.UniqueName, h.Caption));
        //        }
        //    }
        //}

        public CellValueInfo GetValue(params int[] index)
        {
            if (m_CellSet_Descr == null || index == null || index.Length < 2)
            {
                return CellValueInfo.Empty;
            }

            foreach (CellData cell_descr in m_CellSet_Descr.Cells)
            {
                if (cell_descr.Axis0_Coord == index[0] &&
                    cell_descr.Axis1_Coord == index[1])
                {
                    object value = null;
                    string displayName = string.Empty;

                    if (cell_descr.Value != null)
                    {
                        try
                        {
                            displayName = cell_descr.Value.DisplayValue;
                            value = cell_descr.Value.Value;
                        }
                        catch (Exception exc)
                        {
                            value = exc;
                        }
                    }

                    if (string.IsNullOrEmpty(displayName))
                    {
                        if (value == null)
                        {
                            displayName = String.Empty;
                        }
                        else
                        {
                            displayName = value.ToString();
                        }
                    }

                    CellValueInfo res = new CellValueInfo(value, displayName);
                    foreach (PropertyData prop in cell_descr.Value.Properties)
                    {
                        res.Properties.Add(prop.Name, prop.Value);
                    }
                    return res;
                }

            }

            return CellValueInfo.Empty;
        }

        public IList<MemberInfo> GetInvisibleCoords(params int[] index)
        {
            IList<MemberInfo> res = new List<MemberInfo>();
            if (m_CellSet_Descr == null)
            {
                return res;
            }

            for (int i = index.Length; i < m_CellSet_Descr.Axes.Count; i++)
            {
                if (m_CellSet_Descr.Axes[i].Positions.Count > 0 &&
                    m_CellSet_Descr.Axes[i].Positions[0].Members.Count > 0)
                {
                    MemberInfo member = this.CreateInvisibleMemberInfo(m_CellSet_Descr.Axes[i].Positions[0].Members[0]);
                    res.Add(member);
                }
            }

            return res;
        }


        /*public IEnumerable<MemberInfo> GetFilterCoords()
        {
            if (m_CellSet == null)
            {
                return new MemberInfo[] { };
            }

            List<MemberInfo> res = new List<MemberInfo>();
            foreach(Tuple tuple in m_CellSet.FilterAxis.Set.Tuples)
            {
                if(tuple.Members.Count > 0)
                {
                    MemberInfo member = this.CreateMemberInfo(tuple.Members[0]);
                    res.Add(member);
                }
            }
            return res;
        }*/

        /*
        private PivotFieldCollection CreateFields()
        {
            List<PivotField> fields = new List<PivotField>();
            foreach (Axis axis in m_CellSet.Axes)
            {
                foreach (Hierarchy h in axis.Set.Hierarchies)
                {
                        PivotField field = 
                            new PivotField(h.UniqueName, h.Caption);
                        foreach (Property prop in h.Properties)
                        {
                            field.Properties.Add(prop.Name, prop.Value);
                        }

                        fields.Add(field);
                }
            }

            return new PivotFieldCollection(fields);
        }
        */

        #endregion
    }
}

/*
private class FieldAccessorPropertyDescriptor : PropertyDescriptor
{
    private string m_Caption;
    public FieldAccessorPropertyDescriptor(string uniqueName, string caption)
        : base(uniqueName, new Attribute[] { })
    {
        m_Caption = caption;
    }

    public override string DisplayName
    {
        get
        {
            return m_Caption;
        }
    }

    public override bool CanResetValue(object component)
    {
        throw new NotImplementedException();
    }

    public override object GetValue(object component)
    {
        return null;
    }

    public override void SetValue(object component, object value)
    {
        throw new NotImplementedException();
    }

    public override bool ShouldSerializeValue(object component)
    {
        throw new NotImplementedException();
    }

    public override void ResetValue(object component)
    {
        throw new NotImplementedException();
    }

    public override Type ComponentType
    {
        get 
        {
            return typeof(CellSetRow);
        }
    }

    public override Type PropertyType
    {
        get 
        {
            return typeof(object);
        }
    }

    public override bool IsReadOnly
    {
        get 
        {
            return true;
        }
    }
}

private class CellSetRow : ICustomTypeDescriptor
{
    private CellSetDataProvider m_Provider;
    private int[] m_IndexVector;
    public CellSetRow(CellSetDataProvider provider, int[] indexVector)
    {
        m_Provider = provider;
        m_IndexVector = indexVector;
    }

    public PivotFieldValue GetValue()
    {
        Cell cell = m_Provider.m_CellSet[m_IndexVector];
        object value = null;
        try
        {
            value = cell.Value;
        }
        catch (Exception exc)
        {
            value = exc;
        }

        string formattedValue = string.Empty;
        try
        {
            formattedValue = cell.FormattedValue;
        }
        catch
        {
        }

        return new PivotFieldValue(value, formattedValue);
    }

    #region ICustomTypeDescriptor Members

    public AttributeCollection GetAttributes()
    {
        return new AttributeCollection(new Attribute[] { });
    }

    public string GetClassName()
    {
        return string.Empty;
    }

    public string GetComponentName()
    {
        return string.Empty;
    }

    public TypeConverter GetConverter()
    {
        return TypeDescriptor.GetConverter(typeof(object));
    }

    public EventDescriptor GetDefaultEvent()
    {
        return null;
    }

    public PropertyDescriptor GetDefaultProperty()
    {
        return null;
    }

    public object GetEditor(Type editorBaseType)
    {
        return null;
    }

    public EventDescriptorCollection GetEvents(Attribute[] attributes)
    {
        return new EventDescriptorCollection(new EventDescriptor[] { });
    }

    public EventDescriptorCollection GetEvents()
    {
        return new EventDescriptorCollection(new EventDescriptor[] { });
    }

    public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
    {
        return this.GetProperties();
    }

    public PropertyDescriptorCollection GetProperties()
    {
        PropertyDescriptor[] props = new PropertyDescriptor[m_Provider.Fields.Count];
        //m_Provider.Fields.CopyTo(props, 0);
        return new PropertyDescriptorCollection(props, true);
    }

    public object GetPropertyOwner(PropertyDescriptor pd)
    {
        return this;
    }

    #endregion
}

private class CellSetEnumerator : IEnumerator
{
    private CellSet m_CellSet;
    public CellSetEnumerator(CellSet cs)
    {
        m_CellSet = cs;
    }

    #region IEnumerator Members

    public object Current
    {
        get 
        {
            throw new NotImplementedException();
        }
    }

    public bool MoveNext()
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    #endregion
}
*/