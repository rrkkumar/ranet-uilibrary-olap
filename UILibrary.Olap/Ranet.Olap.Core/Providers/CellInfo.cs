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
using Ranet.Olap.Core.Data;

namespace Ranet.Olap.Core.Providers
{
    public class CellInfo
    {
        public readonly CellData CellDescr = null;
        public readonly MemberInfo ColumnMember = null;
        public readonly MemberInfo RowMember = null;
        public readonly IList<MemberInfo> InvisibleCoords = null;

        public CellInfo(CellData cell_descr, MemberInfo column, MemberInfo row, IList<MemberInfo> invisibleCoords)
        {
            if (cell_descr == null)
            {
                throw new ArgumentNullException("cell_descr");
            }
            if (column == null)
            {
                throw new ArgumentNullException("column");
            }
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }
            if (invisibleCoords == null)
            {
                throw new ArgumentNullException("invisibleCoords");
            }

            CellDescr = cell_descr;
            ColumnMember = column;
            RowMember = row;
            InvisibleCoords = invisibleCoords;
        }


        public bool IsUpdateable
        {
            get
            {
                if (this.HasCustomRollupInTuple == false && this.CellDescr.Value != null && this.CellDescr.Value.CanUpdate)
                    return true;
                return false;
            }
        }

        IDictionary<String, MemberInfo> tuple = null;
        String tupleToStr = String.Empty;
        String shortTupleToStr = String.Empty;

        /// <summary>
        /// Возвращает тапл для ячейки. Ключ: имя иерархии; Значение: элемент
        /// </summary>
        /// <returns></returns>
        public IDictionary<String, MemberInfo> GetTuple()
        {
            if (tuple == null)
            {
                tuple = new Dictionary<String, MemberInfo>();
                if (this.ColumnMember != null && ColumnMember != MemberInfo.Empty)
                {
                    this.ColumnMember.CollectAncestors(tuple);
                }
                if (this.RowMember != null && RowMember != MemberInfo.Empty)
                {
                    this.RowMember.CollectAncestors(tuple);
                }

                foreach (MemberInfo mi in this.InvisibleCoords)
                {
                    if (!tuple.ContainsKey(mi.HierarchyUniqueName))
                        tuple.Add(mi.HierarchyUniqueName, mi);
                }
            }
            return tuple;
        }

        public bool CompareByTuple(IDictionary<String, MemberInfo> new_tuple)
        {
            IDictionary<String, MemberInfo> tuple = GetTuple();
            if (tuple != null && new_tuple != null)
            {
                bool is_equal = true;
                if (new_tuple.Count == tuple.Count)
                {
                    foreach (String hierarchy in new_tuple.Keys)
                    {
                        if (!(tuple.ContainsKey(hierarchy) &&
                            tuple[hierarchy].UniqueName == new_tuple[hierarchy].UniqueName))
                        {
                            is_equal = false;
                            break;
                        }
                    }
                }
                return is_equal;
            }
            return false;
        }

        /// <summary>
        /// Возвращает в виде строки крайние координаты ячейки
        /// </summary>
        /// <returns></returns>
        public String GetShortTupleToStr()
        {
            if (String.IsNullOrEmpty(shortTupleToStr))
            {
                StringBuilder sb = new StringBuilder();
                if (this.ColumnMember != null && this.ColumnMember != null)
                {
                    sb.AppendLine(this.ColumnMember.Caption + " : " + this.ColumnMember.UniqueName);
                }
                if (this.RowMember != null && this.RowMember != null)
                {
                    sb.AppendLine(this.RowMember.Caption + " : " + this.RowMember.UniqueName);
                }

                shortTupleToStr = sb.ToString();
                shortTupleToStr = shortTupleToStr.TrimEnd('\n');
                shortTupleToStr = shortTupleToStr.TrimEnd('\r');
            }
            return shortTupleToStr;
        }

        private bool HasCustomRollupInTuple
        {
            get
            {
                bool hasRollup = false;
                IList<MemberInfo> tuple = new List<MemberInfo>();
                if (this.RowMember != null)
                {
                    this.RowMember.CollectAncestors(tuple);
                }
                if (this.ColumnMember != null)
                {
                    this.ColumnMember.CollectAncestors(tuple);
                }
                foreach (MemberInfo mv in tuple)
                {
                    hasRollup = hasRollup | mv.HasCustomRollup;
                }

                /*if (this.InvisibleCoords != null)
                {
                    foreach (MemberInfo mi in this.InvisibleCoords)
                    {
                        hasRollup = hasRollup | mi.HasCustomRollup;
                    }
                }*/

                return hasRollup;
            }
        }

        public object Value
        {
            get
            {
                if (CellDescr != null && CellDescr.Value != null)
                    return CellDescr.Value.Value;
                return null;
            }
        }

        public String ValueToString
        {
            get
            {
                if (CellDescr != null && CellDescr.Value != null && CellDescr.Value.Value != null)
                    return CellDescr.Value.Value.ToString();
                return String.Empty;
            }
        }

        bool m_IsModified = false;
        public bool IsModified
        {
            set
            {
                m_IsModified = value;
            }
            get
            {
                return m_IsModified;
            }
        }

        String m_ModifiedValue = String.Empty;
        public String ModifiedValue
        {
            get
            {
                return m_ModifiedValue;
            }
            set 
            {
                m_ModifiedValue = value;
            }
        }
    }
}
