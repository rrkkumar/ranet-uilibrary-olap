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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AnalysisServices.AdomdClient;
using Ranet.Olap.Core.Data;

namespace Ranet.Olap.Core.Providers
{
    public class CellSetDescriptionProvider
    {
        CellSet m_CS = null;

        public CellSetDescriptionProvider(CellSet cs)
        {
            m_CS = cs;
        }

        CellSetData m_CellSet = null;
        public CellSetData CellSet
        {
            get {
                if (m_CellSet == null)
                {
                    m_CellSet = BuildDescription(m_CS);    
                }
                return m_CellSet; 
            }
        }

        private CellSetData BuildDescription(CellSet cs)
        {
            CellSetData cs_descr = new CellSetData();
            if (cs != null)
            {
                if (cs.OlapInfo != null &&
                    cs.OlapInfo.CubeInfo != null &&
                    cs.OlapInfo.CubeInfo.Cubes != null &&
                    cs.OlapInfo.CubeInfo.Cubes.Count > 0)
                {
                    cs_descr.CubeName = cs.OlapInfo.CubeInfo.Cubes[0].CubeName;
                }

                // Оси
                foreach(Axis axis in cs.Axes)
                {
                    cs_descr.Axes.Add(BuildAxisDescription(axis));
                }

                // Ось фильтров - пока эта информация не нужна
                // cs_descr.FilterAxis = BuildAxisDescription(cs.FilterAxis); 

                if (cs.Axes.Count == 1)
                {
                    for (int col = 0; col < cs.Axes[0].Positions.Count; col++)
                    {
                        CellData cell_descr = new CellData();
                        cell_descr.Axis0_Coord = col;
                        cell_descr.Axis1_Coord = -1;
                        cell_descr.Value = GetValue(col);
                        cs_descr.Cells.Add(cell_descr);
                    }
                }

                if (cs.Axes.Count >= 2)
                { 
                    for(int col = 0; col < cs.Axes[0].Positions.Count; col++)
                    {
                        for (int row = 0; row < cs.Axes[1].Positions.Count; row++)
                        {
                            CellData cell_descr = new CellData();
                            cell_descr.Axis0_Coord = col;
                            cell_descr.Axis1_Coord = row;
                            cell_descr.Value = GetValue(col, row);
                            cs_descr.Cells.Add(cell_descr);
                        }
                    }
                }
            }
            return cs_descr;
        }

        private AxisData BuildAxisDescription(Axis axis)
        {
            AxisData axis_descr = null;
            if (axis != null)
            {
                axis_descr = new AxisData();
                axis_descr.Name = axis.Name;

                foreach (Position pos in axis.Positions)
                {
                    PositionData pos_desc = new PositionData();
                    foreach (Member member in pos.Members)
                    {
                        MemberData member_desc = CreateMemberMemberDescription(member);
                        pos_desc.Members.Add(member_desc);
                    }
                    axis_descr.Positions.Add(pos_desc);
                }
            }

            return axis_descr;
        }


        /// <summary>
        /// Внутренний кэш. Ключ - уник. имя уровня, значение - уник. имя иерархии
        /// </summary>
        Dictionary<String, String> m_HierarchiesToLevelsCache = new Dictionary<string, string>();

        /// <summary>
        /// Формирует описание для элемента
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        private MemberData CreateMemberMemberDescription(Member member)
        {
            if (member == null)
                return null;
            MemberData info = new MemberData();
            info.Caption = member.Caption;
            info.ChildCount = member.ChildCount;
            info.Description = member.Description;
            info.DrilledDown = member.DrilledDown;
            info.LevelDepth = member.LevelDepth;
            info.LevelName = member.LevelName;
            info.Name = member.Name;
            info.ParentSameAsPrevious = member.ParentSameAsPrevious;

            String hierarchyUniqueName = GetMemberPropertyValue(member, "HIERARCHY_UNIQUE_NAME");
            if (hierarchyUniqueName != null)
            {
                info.HierarchyUniqueName = hierarchyUniqueName;
            }
            else
            {
                if (m_HierarchiesToLevelsCache.ContainsKey(info.LevelName))
                {
                    info.HierarchyUniqueName = m_HierarchiesToLevelsCache[info.LevelName];
                }
                else
                {
                    try
                    {
                        info.HierarchyUniqueName = member.ParentLevel.ParentHierarchy.UniqueName;
                    }
                    catch (System.InvalidOperationException)
                    {
                        info.HierarchyUniqueName = String.Empty;
                    }

                    m_HierarchiesToLevelsCache[info.LevelName] = info.HierarchyUniqueName;
                }
            }

            info.UniqueName = member.UniqueName;

            // Свойства
            foreach (MemberProperty mp in member.MemberProperties)
            {
                info.MemberProperties.Add(new PropertyData(mp.Name, mp.Value));
            }
            return info;
        }

        /// <summary>
        /// Возвращает значение свойства для элемента
        /// </summary>
        /// <param name="member"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        private String GetMemberPropertyValue(Member member, String propName)
        {
            if (member == null || member.MemberProperties == null || String.IsNullOrEmpty(propName))
                return null;
            MemberProperty prop = member.MemberProperties.Find(propName);
            if (prop != null && prop.Value != null)
            {
                return prop.Value.ToString();
            }
            return null;
        }

        public CellValueData GetValue(params int[] index)
        {
            if (m_CS == null)
            {
                return CellValueData.Empty;
            }

            int[] indexVector = new int[m_CS.Axes.Count];
            for (int i = 0; i < indexVector.Length; i++)
            {
                indexVector[i] = 0;
            }
            index.CopyTo(indexVector, 0);

            Cell cell = null;
            try
            {
                cell = m_CS[indexVector];
            }
            catch (ArgumentOutOfRangeException)
            {
                return CellValueData.Empty;
            }
            if (cell != null)
            {
                object value = null;
                string displayName = string.Empty;

                try
                {
                    displayName = cell.FormattedValue;
                }
                catch (Exception exc)
                {
                    displayName = CellValueData.ERROR;
                }

                try
                {
                    value = cell.Value;
                }
                catch (Exception exc)
                {
                    value = exc.ToString();
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
                
                CellValueData res = new CellValueData(value, displayName);
                foreach (CellProperty prop in cell.CellProperties)
                {
                    try
                    {
                        res.Properties.Add(new PropertyData(prop.Name, prop.Value));
                    }
                    catch (Microsoft.AnalysisServices.AdomdClient.AdomdErrorResponseException ex)
                    {
                        res.Properties.Add(new PropertyData(prop.Name, ex.ToString()));
                    }
                }

                return res;
            }

            return null;
        }

    }
}
