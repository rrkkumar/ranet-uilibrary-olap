/*   
    Copyright (C) 2009 Galaktika Corporation ZAO

    This file is a part of Ranet.UILibrary.Olap
 
    Ranet.UILibrary.Olap is a free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
      
    You should have received a copy of the GNU General Public License
    along with Ranet.UILibrary.Olap.  If not, see
  	<http://www.gnu.org/licenses/> 
  
    If GPL v.3 is not suitable for your products or company,
    Galaktika Corp provides Ranet.UILibrary.Olap under a flexible commercial license
    designed to meet your specific usage and distribution requirements.
    If you have already obtained a commercial license from Galaktika Corp,
    you can use this file under those license terms.
*/

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
using Ranet.AgOlap.Controls.PivotGrid.Data;
using System.Windows.Browser;
using System.Collections.Generic;
using System.Text;
using Ranet.AgOlap.Controls.General;
using Ranet.Olap.Core.Providers;
using Ranet.AgOlap.Controls.ContextMenu;
using Ranet.Olap.Core.Providers.ClientServer;
using System.Windows.Media.Imaging;
using Ranet.Olap.Core.Data;
using Ranet.AgOlap.Providers;

namespace Ranet.AgOlap.Controls.PivotGrid.Controls
{
    public class MemberActionEventArgs : EventArgs
    {
        public MemberActionEventArgs(
            int axisNum,
            MemberInfo member,
            MemberActionType action)
        {
            this.Axis = axisNum;
            this.Member = member;
            this.Action = action;
        }

        public readonly int Axis = 0;
        public readonly MemberInfo Member = null;
        public readonly MemberActionType Action = MemberActionType.Expand;
    }

    public class MemberClickEventArgs : EventArgs
    {
        public MemberClickEventArgs(
            MemberInfo member)
        {
            this.Member = member;
        }

        public MemberClickEventArgs(
            MemberInfo member, Point point)
            : this(member)
        {
            this.Position = point;
        }

        public readonly MemberInfo Member = null;
        public readonly Point Position = default(Point);
        public CustomContextMenu ContextMenu;
    }

    public abstract class MemberControl : PivotGridItem
    {
        public const int CORNER_RADIUS = 2;

        Border m_Border = null;

        MemberInfo m_Member = null;
        public MemberInfo Member
        {
            get { return m_Member; }
        }

        protected readonly PivotGridControl Owner = null;
        protected virtual bool IsInteractive
        {
            get { return true; }
        }

        protected virtual bool UseHint
        {
            get { return true; }
        }

        /// <summary>
        /// Признак возможности использования команд "+" и "-" для элемента
        /// </summary>
        public readonly bool UseExpandingCommands = false;

        public MemberControl(PivotGridControl owner, MemberInfo info)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            if(info == null)
                throw new ArgumentNullException("info");
            
            m_Member = info;
            Owner = owner;

            this.Margin = new Thickness(0, 0, 0, 0);

            m_Border = new Border();
            m_Border.BorderBrush = Owner.MembersBorder;
            m_Border.BorderThickness = new Thickness(0, 0, 1, 1);
            m_Border.CornerRadius = new CornerRadius(CORNER_RADIUS);
            
            m_Border.Background = Owner.MembersBackground;

            //m_Border.Child = RootPanel;
            m_Border.Child = LayoutRoot;

            if (IsInteractive)
            {
                CaptionText.MouseEnter += new MouseEventHandler(MemberControl_MouseEnter);
                CaptionText.MouseLeave += new MouseEventHandler(MemberControl_MouseLeave);
                CaptionText.MouseLeftButtonDown += new MouseButtonEventHandler(CaptionText_MouseLeftButtonDown);

                PlusMinusButton expander = null;
             
                if (Member.ChildCount > 0 && !Member.IsCalculated)
                {
                    expander = new PlusMinusButton();
                    if (Member.DrilledDown)
                        expander.IsExpanded = true;
                    expander.CheckedChanged += new EventHandler(expander_CheckedChanged);
                    UseExpandingCommands = true;
                    expander.Height = expander.Width = Math.Max(5, 9 * Scale);
                    LayoutRoot.Children.Add(expander);
                }
                else
                {
                    ListMemberControl ctrl = new ListMemberControl();
                    ctrl.Opacity = 0.35;
                    ctrl.Height = ctrl.Width = Math.Max(5, 9 * Scale);
                    LayoutRoot.Children.Add(ctrl);
                }

            }

            // Название элемента
            LayoutRoot.Children.Add(CaptionText);
            Grid.SetColumn(CaptionText, 1);

            // Визуализация DATAMEMBER, UNKNOWNMEMBER,CUSTOM_ROLLUP и UNARY_OPERATOR
            if (Member != null)
            {
                BitmapImage custom_image = null;

                if (Member.UniqueName.Trim().EndsWith("DATAMEMBER"))
                {
                    custom_image = UriResources.Images.DataMember16;
                }

                if (Member.UniqueName.Trim().EndsWith("UNKNOWNMEMBER"))
                {
                    custom_image = UriResources.Images.UnknownMember16;
                }

                // CUSTOM_ROLLUP отображается своей иконкой.
                // UNARY_OPERATOR - каждый своей иконкой.
                // Если оба свойства установлены, то отображаем скомбинированную иконку
                if(String.IsNullOrEmpty(Member.Unary_Operator))
                {
                    // Только CUSTOM_ROLLUP
                    if (Member.HasCustomRollup)
                    {
                        custom_image = UriResources.Images.CalcFunction16;
                    }
                }
                else
                {
                    // UNARY_OPERATOR
                    if (custom_image == null && Member.Unary_Operator.Trim() == "+")
                    {
                        if (Member.HasCustomRollup)
                        {
                            custom_image = UriResources.Images.CalcFunctionPlus16;
                        }
                        else
                        {
                            custom_image = UriResources.Images.CalcPlus16;
                        }
                    }
                    if (custom_image == null && Member.Unary_Operator.Trim() == "-")
                    {
                        if (Member.HasCustomRollup)
                        {
                            custom_image = UriResources.Images.CalcFunctionMinus16;
                        }
                        else
                        {
                            custom_image = UriResources.Images.CalcMinus16;
                        }
                    }
                    if (custom_image == null && Member.Unary_Operator.Trim() == "*")
                    {
                        if (Member.HasCustomRollup)
                        {
                            custom_image = UriResources.Images.CalcFunctionMultiply16;
                        }
                        else
                        {
                            custom_image = UriResources.Images.CalcMultiply16;
                        }
                    }
                    if (custom_image == null && Member.Unary_Operator.Trim() == "/")
                    {
                        if (Member.HasCustomRollup)
                        {
                            custom_image = UriResources.Images.CalcFunctionDivide16;
                        }
                        else
                        {
                            custom_image = UriResources.Images.CalcDivide16;
                        }
                    }
                    if (custom_image == null && Member.Unary_Operator.Trim() == "~")
                    {
                        if (Member.HasCustomRollup)
                        {
                            custom_image = UriResources.Images.CalcFunctionTilde16;
                        }
                        else
                        {
                            custom_image = UriResources.Images.CalcTilde16;
                        }
                    }
                    if (custom_image == null && Member.Unary_Operator.Trim() == "=")
                    {
                        if (Member.HasCustomRollup)
                        {
                            custom_image = UriResources.Images.CalcFunctionEqual16;
                        }
                        else
                        {
                            custom_image = UriResources.Images.CalcEqual16;
                        }
                    }
                    if (custom_image == null)
                    {
                        if (Member.HasCustomRollup)
                        {
                            custom_image = UriResources.Images.CalcFunctionPercent16;
                        }
                        else
                        {
                            custom_image = UriResources.Images.CalcPercent16;
                        }
                    }
                }

                if (custom_image != null)
                {
                    Image image1 = new Image() { Margin = new Thickness(0, 0, 2, 0) };
                    image1.Width = Math.Max(8, 16 * Scale);
                    image1.Height = Math.Max(8, 16 * Scale);
                    image1.Source = custom_image;
                    LayoutRoot.Children.Add(image1);
                    Grid.SetColumn(image1, 3);
                }
            }

            m_EllipsisText = new TextBlock() { Text = "..." };
            m_EllipsisText.FontSize = Owner.DefaultFontSize * Scale;
            LayoutRoot.Children.Add(m_EllipsisText);
            m_EllipsisText.Margin = new Thickness(-1, 0, 0, 0);
            m_EllipsisText.TextAlignment = TextAlignment.Left;
            m_EllipsisText.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetColumn(m_EllipsisText, 2);
            m_EllipsisText.Visibility = Visibility.Collapsed;

            this.SizeChanged += new SizeChangedEventHandler(MemberControl_SizeChanged);

            //if(UseHint)
            //{
            //    // Текст подсказки
            //    IList<MemberInfo> anc = new List<MemberInfo>();
            //    this.Member.CollectAncestors(anc);
            //    StringBuilder sb = new StringBuilder();
            //    foreach (MemberInfo mv in anc)
            //    {
            //        sb.AppendLine(mv.Caption + " : " + mv.UniqueName);
            //    }
            //    String tupleToStr = sb.ToString();
            //    tupleToStr = tupleToStr.TrimEnd('\n');
            //    tupleToStr = tupleToStr.TrimEnd('\r');

            //    // Подсказка
            //    m_ToolTip = new ToolTipControl();
            //    m_ToolTip.Caption = CaptionText.Text;
            //    m_ToolTip.Text = tupleToStr;
            //    ToolTipService.SetToolTip(this, m_ToolTip);
            //}

            this.Content = m_Border;
        }

        void MemberControl_MouseLeave(object sender, MouseEventArgs e)
        {
            CaptionText.TextDecorations = null;
            //if (m_EllipsisText != null)
            //{
            //    m_EllipsisText.TextDecorations = null;
            //}
        }

        void MemberControl_MouseEnter(object sender, MouseEventArgs e)
        {
            // Текст отображаем подчеркнутым чтобы использовать как гиперссылку
            CaptionText.TextDecorations = TextDecorations.Underline;
            //if (m_EllipsisText != null)
            //{
            //    m_EllipsisText.TextDecorations = TextDecorations.Underline;
            //}
        }

        void expander_CheckedChanged(object sender, EventArgs e)
        {
            MemberActionType action = MemberActionType.Expand;
            if (Member.DrilledDown)
                action = MemberActionType.Collapse;

            PlusMinusButton expander = sender as PlusMinusButton;
            if (expander != null)
            {
                expander.CheckedChanged -= new EventHandler(expander_CheckedChanged);
                expander.IsChecked = new bool?(!expander.IsChecked.Value);
                expander.CheckedChanged += new EventHandler(expander_CheckedChanged);
            }

            Raise_DrillDownMember(action);
        }

        void CaptionText_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Raise_DrillDownMember(MemberActionType.DrillDown);
        }

        TextBlock m_EllipsisText = null;

        void MemberControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.CaptionText.ActualWidth > this.CaptionText.DesiredSize.Width)
            {
                m_EllipsisText.Visibility = Visibility.Visible;
                m_LayoutRoot.ColumnDefinitions[2].MaxWidth = 12 * Scale;
            }
            else
            {
                m_EllipsisText.Visibility = Visibility.Collapsed;
                m_LayoutRoot.ColumnDefinitions[2].MaxWidth = 0;
            }
        }

        //ToolTipControl m_ToolTip;

        public void RotateCaption(bool rotate)
        {
            if (rotate)
                //m_RootPanel.Visibility = Visibility.Collapsed;
                LayoutRoot.Visibility = Visibility.Collapsed;
            else
                //m_RootPanel.Visibility = Visibility.Visible;
                LayoutRoot.Visibility = Visibility.Visible;
            //RootPanel.RenderTransform = new RotateTransform() { Angle = -90  /*CenterY = -20*/ };
            //RootPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
            //RootPanel.VerticalAlignment = VerticalAlignment.Stretch;
            //RootPanel.Orientation = Orientation.Vertical;
            //RootPanel.Children.Clear();

            //m_CaptionText.RenderTransform = new RotateTransform() { Angle = -90  /*CenterY = -20*/ };
            //RootPanel.Children.Add(m_CaptionText);

        }

        public void ShowUpBorder(bool showBorder)
        {
            if (showBorder)
                m_Border.BorderThickness = new Thickness(m_Border.BorderThickness.Left, 1, m_Border.BorderThickness.Right, m_Border.BorderThickness.Bottom);
            else
                m_Border.BorderThickness = new Thickness(m_Border.BorderThickness.Left, 0, m_Border.BorderThickness.Right, m_Border.BorderThickness.Bottom);
        }

        public void ShowLeftBorder(bool showBorder)
        {
            if (showBorder)
                m_Border.BorderThickness = new Thickness(1, m_Border.BorderThickness.Top, m_Border.BorderThickness.Right, m_Border.BorderThickness.Bottom);
            else
                m_Border.BorderThickness = new Thickness(0, m_Border.BorderThickness.Top, m_Border.BorderThickness.Right, m_Border.BorderThickness.Bottom);
        }

        protected double Scale
        {
            get {
                if (Owner == null)
                {
                    return 1;
                }
                else {
                    return Owner.Scale;
                }

            }
        }

        TextBlock m_CaptionText = null;
        public TextBlock CaptionText
        {
            get
            {
                if (m_CaptionText == null)
                {
                    m_CaptionText = new TextBlock();
                    m_CaptionText.Margin = new Thickness(2, 0, 3, 0);
                    m_CaptionText.TextAlignment = TextAlignment.Left;
                    m_CaptionText.VerticalAlignment = VerticalAlignment.Center;
                    m_CaptionText.HorizontalAlignment = HorizontalAlignment.Stretch;
                }

                MemberVisualizationTypes memberVisualizationType = MemberVisualizationTypes.Caption;
                if (Owner != null)
                {
                    memberVisualizationType = Owner.MemberVisualizationType;
                }
                m_CaptionText.Text = Member.GetText(memberVisualizationType);

                double font_Scaled = Owner.DefaultFontSize * Scale;
                m_CaptionText.FontSize = font_Scaled;
                if (m_EllipsisText != null)
                {
                    m_EllipsisText.FontSize = font_Scaled;
                }
                return m_CaptionText;
            }
        }

        //StackPanel m_RootPanel = null;
        //public StackPanel RootPanel
        //{
        //    get
        //    {
        //        if (m_RootPanel == null)
        //        {
        //            m_RootPanel = new StackPanel();
        //            m_RootPanel.Margin = new Thickness(2, 2, 5, 0);
        //            m_RootPanel.VerticalAlignment = VerticalAlignment.Top;
        //            m_RootPanel.Orientation = Orientation.Horizontal;
        //        }
        //        return m_RootPanel;
        //    }
        //}

        Grid m_LayoutRoot = null;
        public Grid LayoutRoot
        {
            get
            {
                if (m_LayoutRoot == null)
                {
                    m_LayoutRoot = new Grid();
                    m_LayoutRoot.Margin = new Thickness(2, 2 * Scale, 0, 0);
                    m_LayoutRoot.VerticalAlignment = VerticalAlignment.Top;
                    m_LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                    m_LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition());
                    ColumnDefinition column02 = new ColumnDefinition();
                    column02.MaxWidth = 0; /* чтобы при сжимании иконка надвигалась на текст макс. ширину будем далее задавать жестко*/
                    m_LayoutRoot.ColumnDefinitions.Add(column02);
                    m_LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                    //m_RootPanel.Orientation = Orientation.Horizontal;
                }
                return m_LayoutRoot;
            }
        }

        //public RowDefinition Row = null;
        //public ColumnDefinition Column = null;
        
        #region События
        public event MemberActionEventHandler DrillDownMember;
        public void Raise_DrillDownMember(MemberActionType action)
        {
            MemberActionEventHandler handler = this.DrillDownMember;
            if (handler != null)
            {
                if(this is ColumnMemberControl)
                    handler(this, new MemberActionEventArgs(0, this.Member, action));
                if (this is RowMemberControl)
                    handler(this, new MemberActionEventArgs(1, this.Member, action));
            }
        }
        #endregion События
    }
}
