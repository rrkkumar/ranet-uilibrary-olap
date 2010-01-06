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
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media;

namespace UILibrary.Olap.UITestApplication
{
	public partial class ComboBoxColors : ComboBox
	{
		public ComboBoxColors()
		{
			int i=0;
			foreach (var pic in typeof(Colors).GetProperties(BindingFlags.Static | BindingFlags.Public))
			{
				Color c=(Color)pic.GetValue(null,null);
				this.Items.Add(new ComboBoxItem() { Tag=new SolidColorBrush(c),Content=/*i++.ToString()+"."+*/pic.Name, Background=new SolidColorBrush(c) });
			}
			this.SelectionChanged += new SelectionChangedEventHandler(onSelectionChanged);
			//this.Items.Add("Black");// 0
			//this.Items.Add("Blue");//1
			//this.Items.Add("Brown");//2
			//this.Items.Add("Cyan");//3
			//this.Items.Add("DarkGray");//4
			//this.Items.Add("Gray");//5
			//this.Items.Add("Green");//6
			//this.Items.Add("LightGray");//7
			//this.Items.Add("Magenta");//8
			//this.Items.Add("Orange");//9
			//this.Items.Add("Purple");//10
			//this.Items.Add("Red");//11
			//this.Items.Add("Transparent");//12
			//this.Items.Add("White");//13
			//this.Items.Add("Yellow");//14
		}

		void onSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.Background = new SolidColorBrush((((this.SelectedItem as ComboBoxItem).Tag) as SolidColorBrush).Color);
		}
	}
}
