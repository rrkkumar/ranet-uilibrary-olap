 using System;
 using System.Windows;
 using System.Globalization;
 using System.Threading;
 
 namespace UILibrary.Olap.UITestApplication
 {
 	public partial class Page : System.Windows.Controls.UserControl
 	{
 		string WSDataUrl { get { return Config._Default.DataWebServiceUrl; } }
 		string ConnectionStringId { get { return Config._Default.ConnectionStringId; } }
 
 		public Page()
 		{
 			//CultureInfo ci = new CultureInfo("ru");
 			//Thread.CurrentThread.CurrentCulture = ci;
 			//Thread.CurrentThread.CurrentUICulture = ci;
 			InitializeComponent();
 			SetDefault();
 		}
 		public void SetDefault()
 		{
 			try
 			{
 				//this.HostDirectory.Text = Config._Default.HostDirectory;
 				this.InitializeWebServiceUrl.Text = Config._Default.InitializeWebServiceUrl;
 				this.DataServiceUrl.Text = WSDataUrl;
 				
 				this.OLAPConnectionString.Text = Config._Default.ConectionStringOLAP;
 				this.GaugeRound360Base.CurrentValue = 82;
 				this.pivorGrid_query.Text = @"select [Product].[Product Categories].[Category] on 0 ,[Date].[Calendar].[Calendar Year]  DIMENSION PROPERTIES PARENT_UNIQUE_NAME on 1 from [Adventure Works]
 where [Measures].[Sales Amount]";

 			}
 			catch (Exception E)
 			{
 				MessageBox.Show(E.ToString());
 			}
 		}
 
 	}
 }
 