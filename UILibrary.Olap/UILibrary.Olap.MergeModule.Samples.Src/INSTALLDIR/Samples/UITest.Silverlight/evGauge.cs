using System;
using System.Text;
using System.Windows;

namespace UILibrary.Olap.UITestApplication
{
	public partial class Page : System.Windows.Controls.UserControl
	{
		void Gauge_Changed(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			try
			{
				GaugeRound360Base.MinValue = double.Parse(MinValue.Text);
				GaugeRound360Base.MaxValue = double.Parse(MaxValue.Text);
				GaugeRound360Base.LowValue = double.Parse(LowValue.Text);
				GaugeRound360Base.HightValue = double.Parse(HightValue.Text);
				GaugeRound360Base.CurrentValue = double.Parse(CurrentValue.Text);
				GaugeRound360Base.Text="GaugeRound360Base.Text";
				GaugeRound360Base.ToolTipText = string.Format(
					@"{0}/{1}.
You can dynamically change values and tooltip text."
					, CurrentValue.Text
					,MaxValue.Text
					);
			}
			catch (Exception E)
			{
				MessageBox.Show(E.ToString());
			}
		}
	}
}
