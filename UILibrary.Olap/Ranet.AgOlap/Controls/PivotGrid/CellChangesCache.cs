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
using Ranet.AgOlap.Controls.PivotGrid.Controls;
using Ranet.Olap.Core.Providers;

namespace Ranet.AgOlap.Controls.PivotGrid
{
    public class CellChangesCache
    {
        List<CellValueChangedEventArgs> m_CellChanges = new List<CellValueChangedEventArgs>();
        
        /// <summary>
        /// Кэш измененных ячеек
        /// </summary>
        public List<CellValueChangedEventArgs> CellChanges
        {
            get
            {
                return m_CellChanges;
            }
        }

        /// <summary>
        /// Ищет в кэше изменений ячейку
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public CellValueChangedEventArgs FindLastChange(CellInfo cell)
        {
            for (int i = CellChanges.Count - 1; i >= 0; i--)
            { 
                CellValueChangedEventArgs arg = CellChanges[i];
                if (arg.Cell == cell)
                    return arg;
            }
            return null;
        }

        /// <summary>
        /// Ищет в кэше изменений ячейку
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        CellValueChangedEventArgs FindChange(CellValueChangedEventArgs args)
        {
            foreach (CellValueChangedEventArgs arg in CellChanges)
            {
                if (arg.Cell == args.Cell)
                    return arg;
                //if (arg.Tuple != null && args.Tuple != null && arg.Tuple.Count == args.Tuple.Count && arg.Tuple.Count > 0)
                //{
                //    bool isEqual = true;
                //    for (int i = 0; i < arg.Tuple.Count; i++)
                //    {
                //        if (arg.Tuple[i].UniqueName != args.Tuple[i].UniqueName)
                //        {
                //            isEqual = false;
                //            break;
                //        }
                //    }
                //    if (isEqual)
                //        return arg;
                //}
            }
            return null;
        }
        
        public void RemoveChanges(CellValueChangedEventArgs args)
        {
            CellValueChangedEventArgs change = null;
            do
            {
                change = FindChange(args);
                if (change != null)
                {
                    CellChanges.Remove(change);
                }
            } while (change != null);
        }
    }
}
