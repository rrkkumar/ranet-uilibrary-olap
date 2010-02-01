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
using Ranet.Olap.Mdx;
using Ranet.Olap.Core.Providers;

namespace Ranet.AgOlap.Providers.MemberActions
{
	public class HistoryItem4MdxQuery : IHistoryItem<HistoryItem4MdxQuery>
	{
		internal readonly AxisInfo ColumnsActionChain;
		internal readonly AxisInfo RowsActionChain;
		public bool RotateAxes { get; set; }
		//public Func<MdxObject, MdxActionContext, MdxObject> ConcretizeMdxObject = null;

		public HistoryItem4MdxQuery()
		{
			ColumnsActionChain = new AxisInfo();
			RowsActionChain = new AxisInfo();
		}
		HistoryItem4MdxQuery(HistoryItem4MdxQuery old)
		{
			ColumnsActionChain = old.ColumnsActionChain.Clone();
			RowsActionChain = old.RowsActionChain.Clone();
			RotateAxes=old.RotateAxes;
			//ConcretizeMdxObject=old.ConcretizeMdxObject;
		}
		//public DrillActionContainer FindDrillActionContainer(IList<DrillActionContainer> actionChain, String memberId)
		//{
		//  if (actionChain == null)
		//    return null;

		//  foreach (DrillActionContainer container in actionChain)
		//  {
		//    if (container.MemberUniqueName == memberId)
		//      return container;
		//    DrillActionContainer child = FindDrillActionContainer(container.Children, memberId);
		//    if (child != null)
		//      return child;
		//  }
		//  return null;
		//}
		//public IList<DrillActionContainer> FindDrillActionContainersByHierarchy(IList<DrillActionContainer> actionChain, String hierarchyUniqueName)
		//{
		//  IList<DrillActionContainer> list = new List<DrillActionContainer>();
		//  if (actionChain == null)
		//    return list;

		//  foreach (DrillActionContainer container in actionChain)
		//  {
		//    if (container.HierarchyUniqueName == hierarchyUniqueName && !list.Contains(container))
		//      list.Add(container);

		//    IList<DrillActionContainer> childList = FindDrillActionContainersByHierarchy(container.Children, hierarchyUniqueName);
		//    if (childList != null)
		//    {
		//      foreach (DrillActionContainer childContainer in childList)
		//      {
		//        if (!list.Contains(childContainer))
		//        {
		//          list.Add(childContainer);
		//        }
		//      }
		//    }
		//  }
		//  return list;
		//}
		public HistoryItem4MdxQuery Clone()
		{
			return new HistoryItem4MdxQuery(this);
		}
		
		public MdxSelectStatement CreateWrappedStatement(MdxSelectStatement original)
		{
			if (original == null)
				return null;

			try
			{
				MdxSelectStatement select = (MdxSelectStatement)original.Clone();

				if (select.Axes.Count <= 0)
					return select;

				var axis0_actions = ColumnsActionChain;
				if (select.Axes[0].Name.ToLower() == "1" || select.Axes[0].Name.ToLower() == "rows")
					axis0_actions = RowsActionChain;

				select.Axes[0] = axis0_actions.GetWrappedAxis(select.Axes[0]/*, this.ConcretizeMdxObject*/);

				if (select.Axes.Count <= 1)
					return select;

				var axis1_actions = RowsActionChain;
				if (select.Axes[1].Name.ToLower() == "0" || select.Axes[1].Name.ToLower() == "columns")
					axis1_actions = ColumnsActionChain;

				select.Axes[1] = axis1_actions.GetWrappedAxis(select.Axes[1]/*, this.ConcretizeMdxObject*/);

				// Переворот осей
				if (RotateAxes)
				{
					if (select.Axes.Count > 1)
					{
						var axis0 = select.Axes[0];
						select.Axes[0] = select.Axes[1];
						select.Axes[0].Name = "0";
						select.Axes[1] = axis0;
						axis0.Name = "1";
					}
				}
				return select;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
