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

namespace Ranet.AgOlap.Controls.Combo
{
    public class CheckedListBoxItem : ListBoxItem
    {
        public CheckedListBoxItem()
        {
            DefaultStyleKey = typeof(CheckedListBoxItem);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
