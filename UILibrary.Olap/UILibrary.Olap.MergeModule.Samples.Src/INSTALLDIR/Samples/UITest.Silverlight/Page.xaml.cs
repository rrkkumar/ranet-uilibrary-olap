 using System;
 using System.Windows;
 using System.Globalization;
 using System.Threading;
 
 namespace UILibrary.Olap.UITestApplication
 {
 	public partial class Page : System.Windows.Controls.UserControl
 	{
 		string WSDataUrl { get { return Config.Default.Data.OlapWebServiceUrl; } }
 		string ConnectionStringId { get { return Config.ConnectionStringId; } }
 
 		public Page()
 		{
 			//CultureInfo ci = new CultureInfo("ru");
 			//Thread.CurrentThread.CurrentCulture = ci;
 			//Thread.CurrentThread.CurrentUICulture = ci;
			this.InitializeComponent();
			this.BindData();			
		}
 	}
 }
 