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
		}

		string FormatMdx(string mdx, out string errors)
		{
			using (var dp = Ranet.Olap.Mdx.Compiler.MdxDomProvider.CreateProvider())
			{
				var mdxObj = dp.ParseMdx(mdx);
				var sb = new StringBuilder();
				try
				{
					var op=new Ranet.Olap.Mdx.Compiler.MdxGeneratorOptions();
					op.EvaluateConstantExpressions=(bool)ckbIIF_Subst.IsChecked;
					dp.GenerateMdxFromDom(mdxObj, sb, op);
					errors=string.Empty;
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
