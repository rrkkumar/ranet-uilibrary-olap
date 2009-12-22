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
using Ranet.Olap.Core.Metadata;
using Ranet.AgOlap.Controls.General.ItemControls;
using System.Text;
using System.Windows.Controls.Primitives;
using Ranet.AgOlap.Controls.Combo;

namespace Ranet.AgOlap.Controls.MdxDesigner.CalculatedMembers
{
    public partial class CalcMemberControl : UserControl
    {
        IList<String> m_FormatStrings = null;
        internal IList<String> FormatStrings
        {
            get { return m_FormatStrings; }
            set
            {
                m_FormatStrings = value;
                // Инициализируем список строк форматирования
                comboFormatString.Initialize(value);
            }
        }

        public CalcMemberControl()
        {
            InitializeComponent();

            lblName.Text = lblName1.Text = Localization.CalcMemberEditor_NameLabel;
            lblScript.Text = Localization.CalcMemberEditor_ScriptLabel;
            lblExpression.Text = Localization.CalcMemberEditor_ExpressionLabel;
            tabMemberTab.Header = Localization.CalcMemberEditor_MemberTab;
            tabScriptTab.Header = Localization.CalcMemberEditor_ScriptTab;

            comboFormatString.SelectionChanged += new EventHandler(comboFormatString_SelectionChanged);
            comboFormatString.EditStart += new EventHandler(comboBox_EditStart);
            comboFormatString.EditEnd += new EventHandler(FormatString_EditEnd);

            txtName.KeyDown += new KeyEventHandler(txtName_KeyDown);
            txtExpression.TextChanged += new TextChangedEventHandler(txtExpression_TextChanged);

            TabCtrl.SelectionChanged += new SelectionChangedEventHandler(TabCtrl_SelectionChanged);

            comboNonEmptyBehavior.ItemsSource = m_NonEmptyBehavoirSource;
            comboNonEmptyBehavior.DropDownOpened += new EventHandler(comboBox_EditStart);
            comboNonEmptyBehavior.DropDownClosed += new EventHandler(comboNonEmptyBehavior_DropDownClosed);
        }

        void comboNonEmptyBehavior_DropDownClosed(object sender, EventArgs e)
        {
            Raise_InnerEditEnd();
            Member.NonEmptyBehavior.Clear();
            foreach (var item in comboNonEmptyBehavior.SelectedItems)
            {
                if(!String.IsNullOrEmpty(item.Text))
                    Member.NonEmptyBehavior.Add(item.Text);
            }
        }

        List<ComboBoxItemData> m_NonEmptyBehavoirSource = new List<ComboBoxItemData>();


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

        void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Raise_EditEnd();
            }
        }

        void comboFormatString_SelectionChanged(object sender, EventArgs e)
        {
            if (m_Member != null)
            {
                m_Member.FormatString = String.Empty;

                ItemControlBase ctrl = comboFormatString.CurrentItem;
                if (ctrl != null && ctrl.Tag == null)
                    m_Member.FormatString = ctrl.Text;
            } 
        }

        void TabCtrl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabCtrl.SelectedItem == tabScriptTab)
            {
                RefreshScriptTab();
            }
        }

        void RefreshScriptTab()
        {
            txtName1.Text = m_Member != null ? m_Member.Name : String.Empty;
            txtScript.Text = m_Member != null ? m_Member.GetScript() : String.Empty;
        }

        void txtExpression_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (m_Member != null)
                m_Member.Expression = txtExpression.Text;
        }

        void FormatString_EditEnd(object sender, EventArgs e)
        {
            Raise_InnerEditEnd();
        }

        void comboBox_EditStart(object sender, EventArgs e)
        {
            Raise_InnerEditStart();
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

        /// <summary>
        /// Начато вложенное редактирование
        /// </summary>
        public event EventHandler InnerEditStart;
        void Raise_InnerEditStart()
        {
            EventHandler handler = InnerEditStart;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Вложенное редактирование закончено
        /// </summary>
        public event EventHandler InnerEditEnd;
        void Raise_InnerEditEnd()
        {
            EventHandler handler = InnerEditEnd;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        CalcMemberInfo m_Member = null;
        public CalcMemberInfo Member
        {
            get { return m_Member; }
        }

        public String NameText
        {
            get { return txtName.Text; }
        }

        public void InitializeMetadata(CubeDefInfo cubeInfo)
        {
            comboNonEmptyBehavior.ItemsSource = null;
            m_NonEmptyBehavoirSource.Clear();
            if (cubeInfo != null)
            {
                foreach (var info in cubeInfo.Measures)
                {
                    // Не выводим вычисляемые меры
                    if (String.IsNullOrEmpty(info.Expression))
                    {
                        m_NonEmptyBehavoirSource.Add(new ComboBoxItemData() { Text = info.Name });
                    }
                }
            }
            comboNonEmptyBehavior.ItemsSource = m_NonEmptyBehavoirSource;
        }
        
        public void Initialize(CalcMemberInfo info)
        {
            foreach (var item in m_NonEmptyBehavoirSource)
            {
                item.IsChecked = false;
            }
            if (info != null)
            {
                foreach (var nonEmpty in info.NonEmptyBehavior)
                {
                    foreach (var item in m_NonEmptyBehavoirSource)
                    {
                        if (item.Text == nonEmpty)
                        {
                            item.IsChecked = true;
                        }
                    }
                }
            }

            this.IsEnabled = info != null;

            m_Member = info;
            txtName.Text = info != null ? info.Name : String.Empty;
            txtExpression.Text = info != null ? info.Expression : String.Empty;

            // Ищем соответствующую строку форматирования
            comboFormatString.SelectItem(info != null ? info.FormatString : String.Empty);

            if (TabCtrl.SelectedItem == tabScriptTab)
            {
                RefreshScriptTab();
            }
        }
    }
}
