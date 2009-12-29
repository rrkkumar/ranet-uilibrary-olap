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

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var scroller = GetScroller();
            if (scroller != null)
            {
                Ranet.AgOlap.Features.ScrollViewerMouseWheelSupport.AddMouseWheelSupport(scroller, this);
            }
        }

        public ScrollViewer GetScroller()
        {
            return base.GetTemplateChild("ScrollViewer") as ScrollViewer;
        }
    }
}
