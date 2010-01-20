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

namespace Ranet.Olap.Mdx
{
	[Flags]
	public enum MemberState : byte
	{
		None = 0,
		CanBeExpanded = 1,
		Expanded = 2,
		CanBeCollapsed = 4,
		Collapsed = 8,
		CanBeDrilledDown = 16,
		DrilledDown = 32,
		CanBeDrilledUp = 64,
		DrilledUp = 128
	}
	public class MdxQueryContext
	{
		public AxisInfo Axis0;
		public AxisInfo Axis1;
	}
	public class AxisInfo
	{
		public readonly List<Hierarchy> Hierarchies = new List<Hierarchy>();
		public readonly List<AxisCell> FirstColumnMembers = new List<AxisCell>();
		public readonly Dictionary<Tuple, AxisCell> AllCellsByTuple = new Dictionary<Tuple, AxisCell>();
		internal Stack<AxisCell> DrillDownHistory = new Stack<AxisCell>();
		
		public void UndoDrillDown()
		{
			DrillDownHistory.Pop();
		}
		public Hierarchy AddHierarchy(string HierarchyUniqueName)
		{
	    var result = new Hierarchy(this, HierarchyUniqueName, Hierarchies.Count);
			Hierarchies.Add(result);
			return result;
		}
		public AxisCell GetOrAddCell4Tuple(Tuple Tuple)
		{
			AxisCell result = null;
			if (!AllCellsByTuple.TryGetValue(Tuple, out result))
			{
				if (Tuple.Member.ParentMember != null)
					result = GetOrAddCell4Tuple(new Tuple(Tuple.ParentTuple, Tuple.Member.ParentMember));
				else if (Tuple.ParentTuple != null)
					result = GetOrAddCell4Tuple(Tuple.ParentTuple);

				
				result = new AxisCell(result, Tuple);
				AllCellsByTuple[Tuple]=result;
				
				// FirstHierarchyMembers
			}
			return result;
		}
	}
	public delegate string MetadataResolver(string UniqueName);

	public class Hierarchy
	{
		public readonly MdxQueryContext QueryContext;
		public readonly AxisInfo AxisInfo;
		public readonly List<Level> Levels = new List<Level>();
		public readonly int Index;
		public readonly string HierarchyUniqueName;
		internal readonly Dictionary<string, HierarchyMember> Members = new Dictionary<string, HierarchyMember>();
		internal readonly List<HierarchyMember> FirstLevelMembers = new List<HierarchyMember>();

		public readonly Dictionary<Tuple,AxisCell> ExpandedCells = new Dictionary<Tuple,AxisCell>();
		public readonly Dictionary<Tuple, AxisCell> CollapsedCells = new Dictionary<Tuple, AxisCell>();

		
		public event MetadataResolver ParentUniqueNameResolve;
		
		internal Level GetOrAddNextLevel4Member(HierarchyMember HierarchyMember)
		{
			int levelIndex;
			if(HierarchyMember==null)
				levelIndex=0;
			else
				levelIndex=HierarchyMember.Level.Index+1;
			
			Level result;
			if (Levels.Count <= levelIndex)
			{
				result = new Level(this,Levels.Count);
				Levels.Add(result);
			}	
			else
				result = Levels[levelIndex];
			
			return result;
		}

		public HierarchyMember GetOrAddMember(string UniqueNameInHierarchy)
		{
			HierarchyMember result = null;
			if (!Members.TryGetValue(UniqueNameInHierarchy, out result))
			{
				HierarchyMember ParentMember;
				
				if (ParentUniqueNameResolve != null)
					ParentMember = GetOrAddMember(ParentUniqueNameResolve(UniqueNameInHierarchy));
				else
					ParentMember = null;

				var Level = GetOrAddNextLevel4Member(ParentMember);
				result = new HierarchyMember(this, ParentMember, Level, UniqueNameInHierarchy);
			}
			return result;
		}
		internal Hierarchy(AxisInfo AxisInfo, string HierarchyUniqueName, int Index)
		{
			this.AxisInfo = AxisInfo;
			this.Index=Index;
			this.HierarchyUniqueName = HierarchyUniqueName;
		}
	}
	public class Level
	{
		public readonly Hierarchy Hierarchy;
		public readonly int Index;

		internal Level(Hierarchy Hierarchy, int Index)
		{
			this.Hierarchy=Hierarchy;
			this.Index = Index;
		}
	}
	public class HierarchyMember
	{
		public readonly Hierarchy Hierarchy;
		public readonly Level Level;
		public readonly HierarchyMember ParentMember;
		public readonly string UniqueName;
		public readonly List<HierarchyMember> Childs;
		
		public MdxQueryContext QueryContext { get { return Hierarchy.QueryContext; } }

		public HierarchyMember CreateChild(string UniqueNameInHierarchy)
		{
			return new HierarchyMember(this.Hierarchy, this, this.Hierarchy.GetOrAddNextLevel4Member(this), UniqueNameInHierarchy);
		}
		internal HierarchyMember(Hierarchy Hierarchy, HierarchyMember ParentMember, Level Level, string UniqueNameInHierarchy)
		{
			this.Hierarchy = Hierarchy;
			this.UniqueName = UniqueNameInHierarchy;
			this.Level = Level;
			this.ParentMember = ParentMember;
			
			Hierarchy.Members[UniqueNameInHierarchy] = this;
			if (ParentMember==null)
				Hierarchy.FirstLevelMembers.Add(this);
			else
				ParentMember.Childs.Add(this);
			
		}
	}
	public class Tuple
	{
		public readonly Tuple ParentTuple;
		public readonly HierarchyMember Member;
		public MdxQueryContext QueryContext { get { return Member.QueryContext; } }

		public Tuple(Tuple ParentTuple, HierarchyMember Member)
		{
			this.ParentTuple = ParentTuple;
			this.Member = Member;
		}
		internal IEnumerable<MdxExpression> GenerateMembers(List<MdxExpression> result)
		{	
			if(this.ParentTuple!=null)
				this.ParentTuple.GenerateMembers(result);
				
			result.Add(new MdxObjectReferenceExpression(this.Member.UniqueName));
			return result;
		}

	}
	public class AxisCell
	{
		public readonly Tuple Tuple;
		public readonly AxisCell ParentAxisCell;
		public MemberState State;
		public readonly List<AxisCell> ChildsInMyHierarchy;
		public readonly List<AxisCell> ChildsInNextHierarchy;

		internal AxisCell(AxisCell ParentAxisCell, Tuple Tuple)
		{
			this.Tuple = Tuple;
			this.ParentAxisCell = ParentAxisCell;
			if (ParentAxisCell!=null)
			{
				if (object.ReferenceEquals(ParentAxisCell.Tuple.Member.Hierarchy,Tuple.Member.Hierarchy))
					ParentAxisCell.ChildsInMyHierarchy.Add(this);
				else
					ParentAxisCell.ChildsInNextHierarchy.Add(this);
			}
		}
		public void DrillDown()
		{
			Tuple.Member.Hierarchy.AxisInfo.DrillDownHistory.Push(this);
		}
		void Expand()
		{
			var CollapsedCells=Tuple.Member.Hierarchy.CollapsedCells;
			if (CollapsedCells.ContainsKey(Tuple))
				CollapsedCells.Remove(Tuple);
			
			var ExpandedCells = Tuple.Member.Hierarchy.ExpandedCells;
			if(!ExpandedCells.ContainsKey(Tuple))
				ExpandedCells.Add(Tuple,this);
		}
		void Collapse()
		{
			var ExpandedCells = Tuple.Member.Hierarchy.ExpandedCells;
			if (!ExpandedCells.ContainsKey(Tuple))
				ExpandedCells.Remove(Tuple);
				
			var CollapsedCells = Tuple.Member.Hierarchy.CollapsedCells;
			if (CollapsedCells.ContainsKey(Tuple))
				CollapsedCells.Add(Tuple,this);

		}
	}
}
