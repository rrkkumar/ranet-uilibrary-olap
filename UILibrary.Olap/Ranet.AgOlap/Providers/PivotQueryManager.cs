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
using Ranet.Olap.Core;
using Ranet.AgOlap.Providers.MemberActions;
using Ranet.Olap.Mdx.Compiler;
using System.Text;
using Ranet.Olap.Mdx;
using System.Collections.Generic;
using Ranet.Olap.Core.Providers.ClientServer;
using Ranet.Olap.Core.Data;
using Ranet.Olap.Core.Providers;

namespace Ranet.AgOlap.Providers
{
	public enum ServiceCommandType
	{
		None,
		Refresh,
		ToBegin,
		Forward,
		Back,
		ToEnd,
		HideEmptyRows,
		HideEmptyColumns,
		ShowEmptyRows,
		ShowEmptyColumns,
		GetDataSourceInfo,
		ExportToExcel,
		NormalAxes,
		RotateAxes
	}

	public class PivotQueryManager : HistoryManager<HistoryItem4MdxQuery>
	{
		public string Query { get; private set; }
		public readonly String UpdateScript = String.Empty;

		private MdxSelectStatement m_OriginalStatement = null;
		public Func<MdxObject, MdxActionContext, MdxObject> ConcretizeMdxObject { get; set; }

		public SortDescriptor Axis0_MeasuresSort
		{
			get { return CurrentHistoryItem.ColumnsActionChain.MeasuresSort; }
			set
			{
				AddCurrentStateToHistory();
				CurrentHistoryItem.ColumnsActionChain.MeasuresSort = value;
			}
		}
		public SortDescriptor Axis1_MeasuresSort
		{
			get { return CurrentHistoryItem.RowsActionChain.MeasuresSort; }
			set
			{
				AddCurrentStateToHistory();
				CurrentHistoryItem.RowsActionChain.MeasuresSort = value;
			}
		}

		public PivotQueryManager(String query, String updateScript)
		{
			//m_Connection = connection;
			Query = query;
			UpdateScript = updateScript;
			AddHistoryItem(new HistoryItem4MdxQuery());
			if (string.IsNullOrEmpty(Query))
				return;

			using (MdxDomProvider provider = MdxDomProvider.CreateProvider())
			{
				m_OriginalStatement = provider.ParseMdx(this.Query) as MdxSelectStatement;
			}
		}

		public void ChangeQuery(String query)
		{
			Query = query;
			ClearHistory();

			m_OriginalStatement = null;
		}
		public virtual DataSourceInfoArgs GetDataSourceInfo(UpdateEntry entry)
		{
			DataSourceInfoArgs res = new DataSourceInfoArgs();

			try
			{
				using (MdxDomProvider provider = MdxDomProvider.CreateProvider())
				{
					StringBuilder sb = new StringBuilder();
					provider.GenerateMdxFromDom(this.CreateWrappedStatement(), sb, new MdxGeneratorOptions());

					String new_Query = sb.ToString();

					res.MDXQuery = new_Query;
					res.MovedAxes_MDXQuery = MoveAxes(new_Query);
				}
			}
			catch (Exception ex)
			{
				res.MDXQuery = ex.ToString();
			}

			//res.ConnectionString = Connection.ConnectionID;
			res.UpdateScript = UpdateScript;

			return res;
		}

		public PivotGridToolBarInfo GetToolBarInfo()
		{
			PivotGridToolBarInfo toolBarInfo = new PivotGridToolBarInfo();
			toolBarInfo.HistorySize = this.HistorySize;
			toolBarInfo.CurrentHistoryIndex = this.CurrentHistoryItemIndex;
			toolBarInfo.HideEmptyRows = this.CurrentHistoryItem.RowsActionChain.HideEmpty;
			toolBarInfo.HideEmptyColumns = this.CurrentHistoryItem.ColumnsActionChain.HideEmpty;
			toolBarInfo.RotateAxes = this.CurrentHistoryItem.RotateAxes;

			return toolBarInfo;
		}

		static MdxSelectStatement GetMoveAxesStatement(String originalQuery)
		{
			MdxSelectStatement statement = null;
			if (!String.IsNullOrEmpty(originalQuery))
			{
				using (MdxDomProvider provider = MdxDomProvider.CreateProvider())
				{
					statement = provider.ParseMdx(originalQuery) as MdxSelectStatement;
					if (statement != null)
					{
						if (statement.Where == null)
							statement.Where = new MdxWhereClause();

						if (statement.Axes != null && statement.Axes.Count > 2)
						{
							IList<MdxExpression> expr = new List<MdxExpression>();
							if (statement.Where.Expression != null)
							{
								expr.Add(statement.Where.Expression);
							}

							for (int i = 2; i < statement.Axes.Count; i++)
							{
								expr.Add(statement.Axes[i].Expression);
							}

							statement.Where.Expression = new MdxTupleExpression(expr);
							while (statement.Axes.Count > 2)
								statement.Axes.RemoveAt(2);
						}
					}
				}
			}
			return statement;
		}

		private String MoveAxes(String originalQuery)
		{
			string result = String.Empty;
			MdxSelectStatement statement = GetMoveAxesStatement(originalQuery);
			if (statement != null)
			{
				using (MdxDomProvider provider = MdxDomProvider.CreateProvider())
				{
					StringBuilder sb = new StringBuilder();
					provider.GenerateMdxFromDom(statement, sb, new MdxGeneratorOptions());
					result = sb.ToString();
				}
			}
			return result;
		}
		public String PerformMemberAction(PerformMemberActionArgs args)
		{
			if (args != null)
			{
				switch (args.Action)
				{
					case MemberActionType.Expand:
					case MemberActionType.Collapse:
					case MemberActionType.DrillDown:
					case MemberActionType.DrillUp:
						AddCurrentStateToHistory();
						break;
				}

				switch (args.Action)
				{
					case MemberActionType.Expand:
						ExpandMember(args);
						break;
					case MemberActionType.Collapse:
						CollapseMember(args);
						break;
					case MemberActionType.DrillDown:
						DrillDownMember(args);
						break;
					case MemberActionType.DrillUp:
						DrillUpMember(args);
						break;
				}
			}
			return RefreshQuery();
		}

		public String PerformServiceCommand(ServiceCommandType actionType)
		{
			try
			{
				switch (actionType)
				{
					case ServiceCommandType.HideEmptyColumns:
					case ServiceCommandType.ShowEmptyColumns:
					case ServiceCommandType.RotateAxes:
					case ServiceCommandType.NormalAxes:
					case ServiceCommandType.HideEmptyRows:
					case ServiceCommandType.ShowEmptyRows:
						AddCurrentStateToHistory();
						break;
					default:
						break;
				}

				switch (actionType)
				{
					case ServiceCommandType.Forward:
						this.MoveNext();
						break;
					case ServiceCommandType.Back:
						this.MoveBack();
						break;
					case ServiceCommandType.ToBegin:
						this.ToBegin();
						break;
					case ServiceCommandType.ToEnd:
						this.ToEnd();
						break;
					case ServiceCommandType.HideEmptyColumns:
						this.CurrentHistoryItem.ColumnsActionChain.HideEmpty = true;
						break;
					case ServiceCommandType.ShowEmptyColumns:
						this.CurrentHistoryItem.ColumnsActionChain.HideEmpty = false;
						break;
					case ServiceCommandType.RotateAxes:
						this.CurrentHistoryItem.RotateAxes = true;
						break;
					case ServiceCommandType.NormalAxes:
						this.CurrentHistoryItem.RotateAxes = false;
						break;
					case ServiceCommandType.HideEmptyRows:
						this.CurrentHistoryItem.RowsActionChain.HideEmpty = true;
						break;
					case ServiceCommandType.ShowEmptyRows:
						this.CurrentHistoryItem.RowsActionChain.HideEmpty = false;
						break;
					case ServiceCommandType.ExportToExcel:
						break;
					case ServiceCommandType.GetDataSourceInfo:
						break;
					default:
						break;
				}

			}
			catch (Exception)
			{
				throw;
			}

			return RefreshQuery();
		}

		IList<MdxActionBase> getAxisActions(PerformMemberActionArgs args)
		{
			IList<MdxActionBase> actions = null;
			if (!this.CurrentHistoryItem.RotateAxes)
			{
				actions = args.AxisIndex == 0 ? this.CurrentHistoryItem.ColumnsActionChain.Actions : this.CurrentHistoryItem.RowsActionChain.Actions;
			}
			else
			{
				actions = args.AxisIndex == 1 ? this.CurrentHistoryItem.ColumnsActionChain.Actions : this.CurrentHistoryItem.RowsActionChain.Actions;
			}
		  return actions;
		}
		static MdxTupleExpression GenTuple(PerformMemberActionArgs args)
		{
			var tuple = new MdxTupleExpression();
			tuple.Members.Add(new MdxObjectReferenceExpression(args.Member.UniqueName));
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
		private void ExpandMember(PerformMemberActionArgs args)
		{
			var container = getAxisActions(args);
			container.Add(new MdxExpandAction2(args));
		}
		void CollapseMember(PerformMemberActionArgs args)
		{
			var container = getAxisActions(args);
			container.Add(new MdxCollapseAction2(args));
		}
		void DrillDownMember(PerformMemberActionArgs args)
		{
			var container = getAxisActions(args);
			container.Add(new MdxDrillDownAction(args.Member.UniqueName, args.Member.HierarchyUniqueName, args.Member.LevelDepth));
		}
		void DrillUpMember(PerformMemberActionArgs args)
		{
			var container = getAxisActions(args);
			container.Add(new MdxDrillUpAction(args.Member.UniqueName, args.Member.HierarchyUniqueName, args.Member.LevelDepth));
		}
		public virtual String ExportToExcel()
		{
			return String.Empty;
		}

		public String RefreshQuery()
		{
			return this.RefreshQuery(null);
		}

		public String RefreshQuery(Func<MdxObject, MdxObject> objectConsumerPattern)
		{
			String res = String.Empty;
			if (!string.IsNullOrEmpty(Query))
			{
				using (MdxDomProvider provider = MdxDomProvider.CreateProvider())
				{
					StringBuilder sb = new StringBuilder();
					provider.GenerateMdxFromDom(this.CreateWrappedStatement(), sb, new MdxGeneratorOptions());

					res = sb.ToString();
				}
			}

			return res;
		}

		public String WrappedQuery
		{
			get
			{
				using (MdxDomProvider provider = MdxDomProvider.CreateProvider())
				{
					StringBuilder sb = new StringBuilder();
					provider.GenerateMdxFromDom(this.CreateWrappedStatement(), sb, new MdxGeneratorOptions());

					return sb.ToString();
				}
			}
		}

		//public virtual int ExecuteNonQuery(string query)
		//{
		//    if (Executor != null)
		//        return Executor.ExecuteNonQuery(query);
		//    return -1;
		//}

		//public virtual CellSetData ExecuteQuery(string query)
		//{
		//    if (Executor != null)
		//        return Executor.ExecuteQuery(query);
		//    return null;
		//}

		private MdxSelectStatement CreateWrappedStatement()
		{
			if (m_OriginalStatement == null)
				return null;

			try
			{
				MdxSelectStatement select = (MdxSelectStatement)m_OriginalStatement.Clone();
				if (this.CurrentHistoryItem == null)
					return select;

				return this.CurrentHistoryItem.CreateWrappedStatement(select);

			}
			catch (Exception)
			{
				throw;
			}
		}

		public virtual IEnumerable<String> BuildUpdateScripts(String cubeName, IEnumerable<UpdateEntry> entries)
		{
			var commands = UpdateScriptParser.GetUpdateScripts(cubeName, UpdateScript, entries);
			return commands;
		}


		public String BuildDrillThrough(CellInfo cell)
		{
			String result = String.Empty;
			if (cell != null)
			{
				//var tuple = new Dictionary<String, MemberInfo>();
				//if (cell.ColumnMember != null && cell.ColumnMember != MemberInfo.Empty)
				//{
				//    cell.ColumnMember.CollectAncestors(tuple);
				//}
				//if (cell.RowMember != null && cell.RowMember != MemberInfo.Empty)
				//{
				//    cell.RowMember.CollectAncestors(tuple);
				//}

				//var statement = GetMoveAxesStatement(RefreshQuery());
				var statement = this.CreateWrappedStatement();
				var tuple = cell.GetTuple();
				if (statement != null && tuple != null && tuple.Count > 0)
				{
					statement.Axes.Clear();
					List<MdxExpression> members = new List<MdxExpression>();
					foreach (var member in tuple.Values)
					{
						var expr = new MdxObjectReferenceExpression(member.UniqueName, member.Caption);
						members.Add(expr);
					}

					statement.Axes.Add(new MdxAxis("0", new MdxTupleExpression(members)));

					using (MdxDomProvider provider = MdxDomProvider.CreateProvider())
					{
						StringBuilder sb = new StringBuilder();
						provider.GenerateMdxFromDom(statement, sb, new MdxGeneratorOptions());
						result = sb.ToString();
					}

					if (!String.IsNullOrEmpty(result))
						result = String.Format("DRILLTHROUGH {0}", result);
				}
			}
			return result;
		}
	}
}
