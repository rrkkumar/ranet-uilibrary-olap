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
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Ranet.Olap.Core;
using Ranet.Olap.Core.Data;
using Ranet.AgOlap.Commands;
using System.Collections.Generic;
using Ranet.AgOlap.Controls.PivotGrid.Data;
using Ranet.AgOlap.Controls.General;
using System.Windows.Controls.Primitives;
using Ranet.AgOlap.Controls.DataSourceInfo;
using Ranet.AgOlap.Features;
using System.IO.IsolatedStorage;
using System.IO;
using Ranet.AgOlap.Controls.Buttons;
using Ranet.AgOlap.Controls.ToolBar;
using Ranet.AgOlap.Controls.PivotGrid.Controls;
using Ranet.AgOlap.Controls.PivotGrid;
using System.Threading;
using Ranet.AgOlap.Controls.Forms;
using Ranet.AgOlap.Controls.PivotGrid.Conditions;
using Ranet.AgOlap.Controls.ValueDelivery;
using System.Globalization;
using System.Text;
using Ranet.Olap.Core.Metadata;
using Ranet.AgOlap.Controls.General.ClientServer;
using Ranet.AgOlap.Controls.MemberChoice.ClientServer;
using Ranet.AgOlap.Controls.ValueCopy;
using Ranet.Olap.Core.Providers;
using Ranet.Olap.Core.Providers.ClientServer;
using Ranet.AgOlap.Providers;
using Ranet.AgOlap.Controls.Data;
using Ranet.Olap.Core.Storage;
using Ranet.AgOlap.Controls.ContextMenu;
using Ranet.Olap.Mdx;

namespace Ranet.AgOlap.Controls
{

    public partial class UpdateablePivotGridControl
    {
			MdxQueryContext qContext = null;
			
			
			AxisCell FindCell(int AxisInd, MemberInfo m)
			{
				Ranet.Olap.Mdx.AxisCell result = null;
				var axis = qContext.Axes[AxisInd];
				var list = new List<MemberInfo>();
				m.CollectAncestors(list);
				Ranet.Olap.Mdx.Tuple parentTuple = null;
				Ranet.Olap.Mdx.Tuple parentTuple2 = null;
				Ranet.Olap.Mdx.Hierarchy parentHier = null;
				for (int i = list.Count - 1; i >= 0; i--)
				{
					var member = list[i];
					var qHier = axis.GetOrAddHierarchy(member.HierarchyUniqueName);
					var qMember = qHier.GetOrAddMember(member.UniqueName);
					if (parentHier != qHier)
					{
						parentHier = qHier;
						parentTuple = parentTuple2;
					}
					result = axis.GetOrAddCell4Tuple(new Ranet.Olap.Mdx.Tuple(parentTuple, qMember));
					parentTuple2 = result.Tuple;
				}
				return result;
			}
			string generateQuery(MemberActionEventArgs e)
			{
				var cell = FindCell(e.Axis, e.Member);
				switch (e.Action)
				{
					case MemberActionType.Collapse:
						cell.Collapse();
						break;
					case MemberActionType.Expand:
						cell.Expand();
						break;
					default:
						break;
				}
				var sb = new StringBuilder();
				Ranet.Olap.Mdx.Compiler.MdxDomProvider.CreateProvider().GenerateMdxFromDom(qContext.GenerateCurrent(), sb, new Ranet.Olap.Mdx.Compiler.MdxGeneratorOptions());
				return sb.ToString();
			}
    
    }
}


