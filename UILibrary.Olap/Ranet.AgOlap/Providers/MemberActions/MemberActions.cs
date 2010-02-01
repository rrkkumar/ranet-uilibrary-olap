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
using Ranet.Olap.Mdx;

namespace Ranet.AgOlap.Providers.MemberActions
{
	public abstract class MemberAction
	{
		protected static MdxTupleExpression GenTuple(PerformMemberActionArgs args)
		{
			var tuple = GenTupleBase(args);
			tuple.Members.Add(new MdxObjectReferenceExpression(args.Member.UniqueName));
			return tuple;
		}
		protected static MdxTupleExpression GenTupleBaseCurrent(PerformMemberActionArgs args)
		{
			var tuple = new MdxTupleExpression();
			string lasthier = args.Member.HierarchyUniqueName;
			for (int i = 0; i < args.Ascendants.Count; i++)
			{
				var member = args.Ascendants[i];

				if (lasthier != member.HierarchyUniqueName)
				{
					lasthier = member.HierarchyUniqueName;
					tuple.Members.Insert(0, new MdxObjectReferenceExpression(member.HierarchyUniqueName + ".CURRENTMEMBER"));
				}
			}
			return tuple;
		}
		protected static MdxTupleExpression GenTupleBase(PerformMemberActionArgs args)
		{
			var tuple = new MdxTupleExpression();
			string lasthier = args.Member.HierarchyUniqueName;
			for (int i = 0; i < args.Ascendants.Count; i++)
			{
				var member = args.Ascendants[i];

				if (lasthier != member.HierarchyUniqueName)
				{
					lasthier = member.HierarchyUniqueName;
					tuple.Members.Insert(0, new MdxObjectReferenceExpression(member.UniqueName));
				}
			}
			return tuple;
		}
		public abstract MdxExpression Process(MdxExpression mdx);
		public abstract MemberAction Clone();
	}
	public class MemberActionExpand : MemberAction
	{
		PerformMemberActionArgs args;

		public MemberActionExpand(PerformMemberActionArgs args)
		{
			this.args = args;
		}
		public override MdxExpression Process(MdxExpression expr)
		{
			if (expr == null)
			 return null;
			 
			var tuple = GenTuple(args);
			if (tuple.Members.Count == 1)
			{
				return new MdxFunctionExpression
				("DRILLDOWNMEMBER"
				, expr
				, tuple
				);
			}
			expr = new MdxBinaryExpression
			 (expr
			 , new MdxFunctionExpression(
				 "DRILLDOWNMEMBER"
				 , new MdxFunctionExpression
						("FILTER"
						, expr
						, new MdxBinaryExpression
							(GenTupleBaseCurrent(args)
							, GenTupleBase(args)
							, "IS"
							)
						)
				 , new MdxObjectReferenceExpression(args.Member.UniqueName)
				 )
				, "+"
				);
			return expr;
		}
		public override MemberAction Clone()
		{
			return new MemberActionExpand(args);
		}
	}
	public class MemberActionCollapse : MemberAction
	{
		PerformMemberActionArgs args;

		public MemberActionCollapse(PerformMemberActionArgs args)
		{
			this.args = args;
		}
		public override MdxExpression Process(MdxExpression expr)
		{
			if (expr == null)
			 return null;

			MdxExpression filter = new MdxUnaryExpression
							("NOT"
							, new MdxFunctionExpression
								("ISANCESTOR"
								, new MdxObjectReferenceExpression(args.Member.UniqueName)
								, new MdxObjectReferenceExpression(args.Member.HierarchyUniqueName + ".CURRENTMEMBER")
								)
							);
			var tupleBase = GenTupleBase(args);
			if (tupleBase.Members.Count > 0)
			{
				filter = new MdxBinaryExpression
						(filter
						, new MdxUnaryExpression
							("NOT"
							, new MdxBinaryExpression
								(GenTupleBaseCurrent(args)
								, GenTupleBase(args)
								, "IS"
								)
							)
						, "OR"
						);
			}

			return new MdxFunctionExpression
					("FILTER"
					, expr
					, filter
					);
		}
		public override MemberAction Clone()
		{
			return new MemberActionCollapse(args);
		}
	}
	public class MemberActionDrillDown : MemberAction
	{
		PerformMemberActionArgs args;

		public MemberActionDrillDown(PerformMemberActionArgs args)
		{
			this.args = args;
		}
		public override MdxExpression Process(MdxExpression expr)
		{
			if (expr == null)
			 return null;
			
			string uniqueName = args.Member.UniqueName;
			string hierarchyUniqueName = args.Member.HierarchyUniqueName;

			var drillDownExpr =
				new MdxFunctionExpression
				("Distinct"
				, new MdxFunctionExpression
					("DrillDownMember"
					, expr
					, new MdxObjectReferenceExpression(uniqueName)
					)
				);
			MdxExpression filter = 
					new MdxBinaryExpression
					(	new MdxFunctionExpression
						("IsAncestor"
						, new MdxObjectReferenceExpression(uniqueName)
						, new MdxObjectReferenceExpression(hierarchyUniqueName + ".CURRENTMEMBER")
						)
					, new MdxBinaryExpression
						( new MdxObjectReferenceExpression(uniqueName)
						, new MdxObjectReferenceExpression(hierarchyUniqueName + ".CURRENTMEMBER")
						, "IS"
						)
					, "OR"	
					);
			var tupleBase = GenTupleBase(args);
			if (tupleBase.Members.Count > 0)
			{
				filter = new MdxBinaryExpression
						(filter
						, new MdxBinaryExpression
								(GenTupleBaseCurrent(args)
								, GenTupleBase(args)
								, "IS"
								)
						, "AND"
						);
			}
			return new MdxFunctionExpression("FILTER", drillDownExpr, filter);

		}
		public override MemberAction Clone()
		{
			return new MemberActionDrillDown(args);
		}
	}
}
