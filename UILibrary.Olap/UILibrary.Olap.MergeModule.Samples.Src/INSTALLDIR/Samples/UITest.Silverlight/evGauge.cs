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
using System.Windows;

namespace UILibrary.Olap.UITestApplication
{
	public partial class Page : System.Windows.Controls.UserControl
	{
		void GaugeSetValues()
		{
			try
			{
				GaugeRound360Base.MinValue = double.Parse(tbMinValue.Text);
				GaugeRound360Base.LowValue = double.Parse(tbLowValue.Text);
				GaugeRound360Base.CurrentValue = double.Parse(tbCurrentValue.Text);
				GaugeRound360Base.HighValue = double.Parse(tbHighValue.Text);
				GaugeRound360Base.MaxValue = double.Parse(tbMaxValue.Text);
				
				GaugeRound360Base.Text = string.Format
					( tbGaugeTextTemplate.Text
					, tbMinValue.Text
					, tbLowValue.Text
					, tbCurrentValue.Text
					, tbHighValue.Text
					, tbMaxValue.Text
					);
				
				GaugeRound360Base.ToolTipText = string.Format
					( tbGaugeToolTipTemplate.Text
					, tbMinValue.Text
					, tbLowValue.Text
					, tbCurrentValue.Text
					, tbHighValue.Text
					, tbMaxValue.Text
					);
			}
			catch (Exception E)
			{
				MessageBox.Show(E.ToString());
			}
		}
		void Gauge_Changed(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			GaugeSetValues();
		}
	}
}
