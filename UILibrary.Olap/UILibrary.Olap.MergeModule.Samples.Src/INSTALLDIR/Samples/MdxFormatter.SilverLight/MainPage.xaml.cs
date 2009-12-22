/* 
 Copyright (C) 2009 Galaktika Corporation ZAO

 This file is a part of Ranet.UILibrary.Olap
 
 Ranet.UILibrary.Olap is a free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.
 
 You should have received a copy of the GNU General Public License
 along with Ranet.UILibrary.Olap. If not, see <http://www.gnu.org/licenses/>.
 
 If GPL v.3 is not suitable for your products or company,
 Galaktika Corp provides Ranet.UILibrary.Olap under a flexible commercial license
 designed to meet your specific usage and distribution requirements.
 If you have already obtained a commercial license from Galaktika Corp,
 you can use this file under those license terms.
*/

using System;
using System.Windows;
using System.Windows.Controls;
using System.Text;

namespace Ranet.SampleApplications.MdxFormatter.SilverLight
{
	/**	 */
	public partial class MainPage : UserControl
	{
		/**		 */
		public MainPage()
		{
			InitializeComponent();
			txtPlainMdx.Text = @"with member [TestIIF] as iif(1=1,'equal','notequal')
SELECT HIERARCHIZE ( DRILLDOWNMEMBER ( FILTER ( DRILLDOWNMEMBER ( FILTER ( DRILLDOWNMEMBER ( { [Date].[Calendar].Levels ( 0 ).Members } , [Date].[Calendar].[All Periods] ) , ( ( ( not ( ( [Date].[Calendar].CURRENTMEMBER is [Date].[Calendar].[All Periods] ) AND ( [Date].[Calendar].[All Periods].Children.Count <> 0 ) ) AND not ( IsSibling ( [Date].[Calendar].CURRENTMEMBER , [Date].[Calendar].[All Periods] ) AND not ( [Date].[Calendar].CURRENTMEMBER is [Date].[Calendar].[All Periods] ) ) ) AND not IsAncestor ( [Date].[Calendar].CURRENTMEMBER , [Date].[Calendar].[All Periods] ) ) AND ( IsAncestor ( [Date].[Calendar].[All Periods] , [Date].[Calendar].CURRENTMEMBER ) OR ( [Date].[Calendar].CURRENTMEMBER is [Date].[Calendar].[All Periods] ) ) ) ) , [Date].[Calendar].[Calendar Year].&[2002] ) , ( ( ( not ( ( [Date].[Calendar].CURRENTMEMBER is [Date].[Calendar].[Calendar Year].&[2002] ) AND ( [Date].[Calendar].[Calendar Year].&[2002].Children.Count <> 0 ) ) AND not ( IsSibling ( [Date].[Calendar].CURRENTMEMBER , [Date].[Calendar].[Calendar Year].&[2002] ) AND not ( [Date].[Calendar].CURRENTMEMBER is [Date].[Calendar].[Calendar Year].&[2002] ) ) ) AND not IsAncestor ( [Date].[Calendar].CURRENTMEMBER , [Date].[Calendar].[Calendar Year].&[2002] ) ) AND ( IsAncestor ( [Date].[Calendar].[Calendar Year].&[2002] , [Date].[Calendar].CURRENTMEMBER ) OR ( [Date].[Calendar].CURRENTMEMBER is [Date].[Calendar].[Calendar Year].&[2002] ) ) ) ) , [Date].[Calendar].[Calendar Semester].&[2002]&[1] ) ) DIMENSION PROPERTIES PARENT_UNIQUE_NAME , HIERARCHY_UNIQUE_NAME , CUSTOM_ROLLUP , UNARY_OPERATOR , KEY0 ON 0, HIERARCHIZE ( DRILLUPMEMBER ( FILTER ( DRILLDOWNMEMBER ( { [Product].[Product Categories].Levels ( 0 ).Members } , [Product].[Product Categories].[All Products] ) , ( ( ( not ( ( [Product].[Product Categories].CURRENTMEMBER is [Product].[Product Categories].[All Products] ) AND ( [Product].[Product Categories].[All Products].Children.Count <> 0 ) ) AND not ( IsSibling ( [Product].[Product Categories].CURRENTMEMBER , [Product].[Product Categories].[All Products] ) AND not ( [Product].[Product Categories].CURRENTMEMBER is [Product].[Product Categories].[All Products] ) ) ) AND not IsAncestor ( [Product].[Product Categories].CURRENTMEMBER , [Product].[Product Categories].[All Products] ) ) AND ( IsAncestor ( [Product].[Product Categories].[All Products] , [Product].[Product Categories].CURRENTMEMBER ) OR ( [Product].[Product Categories].CURRENTMEMBER is [Product].[Product Categories].[All Products] ) ) ) ) , [Product].[Product Categories].[Category].&[4] ) ) DIMENSION PROPERTIES PARENT_UNIQUE_NAME , HIERARCHY_UNIQUE_NAME , CUSTOM_ROLLUP , UNARY_OPERATOR , KEY0 ON 1FROM [Adventure Works]WHERE { [Measures].[Sales Amount] } CELL PROPERTIES BACK_COLOR , CELL_ORDINAL , FORE_COLOR , FONT_NAME , FONT_SIZE , FONT_FLAGS , FORMAT_STRING , VALUE , FORMATTED_VALUE , UPDATEABLE
";


		}

		string FormatMdx(string mdx, out string errors)
		{
			using (var dp = Ranet.Olap.Mdx.Compiler.MdxDomProvider.CreateProvider())
			{
				var mdxObj = dp.ParseMdx(mdx);
				var sb = new StringBuilder();
				try
				{
					var op = new Ranet.Olap.Mdx.Compiler.MdxGeneratorOptions();
					op.EvaluateConstantExpressions = (bool)ckbIIF_Subst.IsChecked;
					dp.GenerateMdxFromDom(mdxObj, sb, op);
					errors = string.Empty;
				}
				catch (Exception E)
				{
					errors = E.Message + @"
";
				}
				errors += dp.Errors.ToString();
				return sb.ToString();
			}
		}

		private void btnParseAndGen_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				string e1;
				string mdx1 = FormatMdx(txtPlainMdx.Text, out e1);
				if (string.IsNullOrEmpty(e1))
				{
					string e2;
					string mdx2 = FormatMdx(mdx1, out e2);
					if (!string.IsNullOrEmpty(e2) || (mdx1 != mdx2))
						txtParsedMdx.Text = @"Ошибка: повторные форматирования приводят к различным результатам.
Первое форматирование:
"
						+ mdx1 + @"
Второе форматирование:
"
						+ e2 + mdx2;
					else
						txtParsedMdx.Text = mdx1;
				}
				else
				{
					txtParsedMdx.Text = e1 + mdx1;
				}
			}
			catch (Exception exc)
			{
				txtParsedMdx.Text = exc.ToString();
			}
		}
	}
}
