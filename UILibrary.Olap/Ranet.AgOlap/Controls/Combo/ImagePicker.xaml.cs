using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Collections;
using System.IO;

namespace Ranet.AgOlap.Controls.Combo
{
    public class ImageItem
    {
        BitmapImage m_Image = null;
        public BitmapImage Image
        {
            get { return m_Image; }
            set
            {
                m_Image = null;
            }
        }

        String m_Text = String.Empty;
        public String Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }

        public ImageItem(BitmapImage image, String text)
        {
            Image = image;
            Text = text;
        }
    }

    public partial class ImagePicker : UserControl
    {
        public ImagePicker()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            ImageComboBox.ItemsSource = new List<ImageItem>();
        }

        public void Initialize(Assembly assembly)
        {
            List<ImageItem> list = new List<ImageItem>();

            try
            {
                if (assembly != null)
                {
                    string[] resNames = assembly.GetManifestResourceNames();
                    if (resNames != null)
                    {
                        foreach (string resname in resNames)
                        {
                            ResourceManager rm = new ResourceManager(resname.Replace(".resources", ""), assembly);
                            // No delete next string !!!
                            Stream unreal = rm.GetStream(Application.Current.Host.Source.AbsoluteUri);
                            ResourceSet rs = rm.GetResourceSet(Thread.CurrentThread.CurrentUICulture, false, true);
                            if (rs != null)
                            {
                                IDictionaryEnumerator enumerator = rs.GetEnumerator();
                                while (enumerator.MoveNext())
                                {
                                    if (enumerator.Key != null && enumerator.Value != null)
                                    {
                                        if (enumerator.Key.ToString().Contains(".png"))
                                        {
                                            BitmapImage image = new BitmapImage();
                                            var stream = enumerator.Value as Stream;
                                            if (stream != null)
                                            {
                                                image.SetSource(stream);
                                            }
                                            //string asm = assembly.ManifestModule.ToString();
                                            //asm = asm.Replace(".dll", "");
                                            //image.UriSource = new Uri("/" + asm + ";component/" + enumerator.Key.ToString(), UriKind.Relative);
                                            list.Add(new ImageItem(image, enumerator.Key.ToString()));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            finally 
            {
                ImageComboBox.ItemsSource = list;
            }
        }
    }
}
