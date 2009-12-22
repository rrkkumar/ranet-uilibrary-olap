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
using System.Text;

namespace Ranet.SampleApplications.MdxFormatter
{
	class Program
	{
		static void Main(string[] args)
		{
			string[] fnames;
			if (args.Length == 0)
				fnames = System.IO.Directory.GetFiles(".", "*.mdx");
			else
				fnames = args;


			foreach (var fname in fnames)
			{
				using (var DP = Ranet.Olap.Mdx.Compiler.MdxDomProvider.CreateProvider())
				{
					var mdx = DP.ParseMdx(new System.IO.StreamReader(fname, Encoding.Default));
					if (DP.Errors.Length != 0)
					{
						Console.WriteLine(new System.IO.StreamReader(fname, Encoding.Default).ReadToEnd());
						Console.WriteLine();
						Console.WriteLine("There was errors while parsing MDX:");
						Console.WriteLine(DP.Errors.ToString());
						Console.WriteLine();
					}
					else
					{
						DP.GenerateMdxFromDom
						(mdx
						, Console.Out
						, new Ranet.Olap.Mdx.Compiler.MdxGeneratorOptions()
						);
						Console.WriteLine();
					}
				}
			}
		}
	}
}
