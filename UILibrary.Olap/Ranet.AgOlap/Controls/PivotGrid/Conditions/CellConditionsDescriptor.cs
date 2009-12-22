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

namespace Ranet.AgOlap.Controls.PivotGrid.Conditions
{
    public class CellConditionsDescriptor
    {
        public CellConditionsDescriptor()
        { 
        }

        public CellConditionsDescriptor(String memberUniqueName)
        {
            m_MemberUniqueName = memberUniqueName;    
        }

        private String m_MemberUniqueName = string.Empty;
        /// <summary>
        /// Объект, для которого настраиваются условия
        /// </summary>
        public String MemberUniqueName
        {
            get
            {
                return m_MemberUniqueName;
            }
            set
            {
                m_MemberUniqueName = value;
            }
        }

        private IList<CellCondition> m_Conditions = null;
        /// <summary>
        /// Коллекция условий
        /// </summary>
        public IList<CellCondition> Conditions
        {
            get
            {
                if (m_Conditions == null)
                    m_Conditions = new List<CellCondition>();
                return m_Conditions;
            }
            set
            {
                m_Conditions = value;
            }
        }

        /// <summary>
        /// Возвращает список условий, который удовлетворяет значение ячейки
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public static List<CellCondition> TestToConditions(double value, CellConditionsDescriptor conditionDescriptor)
        {
            if (conditionDescriptor == null)
                return null;

            List<CellCondition> list = new List<CellCondition>();
            foreach (CellCondition cond in conditionDescriptor.Conditions)
            {
                double val1, val2;
                try
                {
                    val1 = Convert.ToDouble(cond.Value1);
                    val2 = Convert.ToDouble(cond.Value2);
                }
                catch
                {
                    continue;
                }

                bool ok = false;
                switch (cond.ConditionType)
                {
                    case CellConditionType.Between:
                        if (val1 < value && value < val2)
                            ok = true;
                        break;
                    case CellConditionType.NotBetween:
                        if (value <= val1 || val2 <= value)
                            ok = true;
                        break;
                    case CellConditionType.Equal:
                        if (val1 == value)
                            ok = true;
                        break;
                    case CellConditionType.Greater:
                        if (value > val1)
                            ok = true;
                        break;
                    case CellConditionType.GreaterOrEqual:
                        if (value >= val1)
                            ok = true;
                        break;
                    case CellConditionType.Less:
                        if (value < val1)
                            ok = true;
                        break;
                    case CellConditionType.LessOrEqual:
                        if (value <= val1)
                            ok = true;
                        break;
                    case CellConditionType.NotEqual:
                        if (value != val1)
                            ok = true;
                        break;
                }

                if (ok)
                    list.Add(cond);
            }
            return list;
        }
    }
}
