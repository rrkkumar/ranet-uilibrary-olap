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
using Ranet.AgOlap.Controls.General;

namespace Ranet.AgOlap.Controls.MdxDesigner.CalculatedMembers
{
    public partial class NamedSetControl : UserControl
    {
        public NamedSetControl()
        {
            InitializeComponent();

            lblName.Text = Localization.CalcMemberEditor_NameLabel;
            txtExpression.Text = Localization.CalcMemberEditor_ExpressionLabel;

            txtExpression.TextChanged += new TextChangedEventHandler(txtScript_TextChanged);
            txtName.KeyDown += new KeyEventHandler(txtName_KeyDown);
        }

        void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Raise_EditEnd();
            }
        }

        /// <summary>
        /// Редактирование начато
        /// </summary>
        public event EventHandler EditStart;
        void Raise_EditStart()
        {
            EventHandler handler = EditStart;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// Редактирование закончено
        /// </summary>
        public event EventHandler EditEnd;
        void Raise_EditEnd()
        {
            EventHandler handler = EditEnd;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        bool m_IsReadyToDrop = false;
        public bool IsReadyToDrop
        {
            get { return m_IsReadyToDrop; }
            set
            {
                if (m_IsReadyToDrop != value)
                {
                    m_IsReadyToDrop = value;
                    if (value)
                    {
                        brdExpression.BorderBrush = new SolidColorBrush(Color.FromArgb(50, Colors.Blue.R, Colors.Blue.G, Colors.Blue.B));
                        brdExpression.Background = new SolidColorBrush(Color.FromArgb(20, Colors.Blue.R, Colors.Blue.G, Colors.Blue.B));
                    }
                    else
                    {
                        brdExpression.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        brdExpression.Background = new SolidColorBrush(Colors.Transparent);
                    }
                }
            }
        }

        public void Drop(Point point, String str)
        {
            if (IsReadyToDrop && !String.IsNullOrEmpty(str))
            {
                txtExpression.Text += " " + str;
            }
        }

        public bool CanDrop(Point point)
        {
            if (IsEnabled)
            {
                Rect expression_Bounds = AgControlBase.GetSLBounds(brdExpression);
                if (expression_Bounds.Contains(point))
                {
                    return true;
                }
            }
            return false;
        }

        void txtScript_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (m_Set != null)
                m_Set.Expression = txtExpression.Text;
        }

        CalculatedNamedSetInfo m_Set = null;
        public CalculatedNamedSetInfo Set
        {
            get { return m_Set; }
        }

        public String NameText
        {
            get { return txtName.Text; }
        }

        public void Initialize(CalculatedNamedSetInfo info)
        {
            this.IsEnabled = info != null;

            m_Set = info;
            txtName.Text = info != null ? info.Name : String.Empty;
            txtExpression.Text = info != null ? info.Expression : String.Empty;
        }
    }
}
