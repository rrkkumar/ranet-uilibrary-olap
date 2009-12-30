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

namespace Ranet.AgOlap.Controls.List
{
    public class RanetListBox : ListBox
    {
        public RanetListBox()
        {
            DefaultStyleKey = typeof(RanetListBox);
        }

        ScrollViewer Scroller = null;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Scroller = base.GetTemplateChild("ScrollViewer") as ScrollViewer;
            if (Scroller != null)
            {
                Ranet.AgOlap.Features.ScrollViewerMouseWheelSupport.AddMouseWheelSupport(Scroller, this);
            }
        }


        ~RanetListBox()
        {
            Ranet.AgOlap.Features.ScrollViewerMouseWheelSupport.RemoveMouseWheelSupport(this);
        }
    }
}
