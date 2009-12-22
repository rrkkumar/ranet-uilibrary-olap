/*   
    Copyright (C) 2009 Galaktika Corporation ZAO

    This file is a part of Ranet.UILibrary.Olap
 
    Ranet.UILibrary.Olap is a free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
      
    You should have received a copy of the GNU General Public License
    along with Ranet.UILibrary.Olap.  If not, see <http://www.gnu.org/licenses/>.
  
    If GPL v.3 is not suitable for your products or company,
    Galaktika Corp provides Ranet.UILibrary.Olap under a flexible commercial license
    designed to meet your specific usage and distribution requirements.
    If you have already obtained a commercial license from Galaktika Corp,
    you can use this file under those license terms.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ranet.Olap.Mdx;

namespace Ranet.Olap.Core.Providers.MemberActions
{
    public class DrillActionContainer : IMdxFastClonable
    {
        public DrillActionContainer(String memberUniqueName, String hierarchyUniqueName)
        {
            MemberUniqueName = memberUniqueName;
            HierarchyUniqueName = hierarchyUniqueName;
        }

        public readonly string MemberUniqueName;
        public readonly string HierarchyUniqueName;

        public IMdxAction Action = null;
        public IList<DrillActionContainer> Children = new List<DrillActionContainer>();


        #region ICloneable Members

        /*private DrillActionContainer CloneFrom(DrillActionContainer source)
        {
            DrillActionContainer dest = null;
            if (source != null)
            {
                dest = new DrillActionContainer(source.MemberUniqueName, source.HierarchyUniqueName);
                if (source.Action != null)
                {
                    dest.Action = (IMdxAction)source.Action.Clone();
                }
                foreach (DrillActionContainer child in source.Children)
                {
                    dest.Children.Add(this.CloneFrom(child));
                }
            }
            return dest;
        }*/

        public object Clone()
        {
            //DrillActionContainer container = CloneFrom(this);
            DrillActionContainer dest = null;
            dest = new DrillActionContainer(this.MemberUniqueName, this.HierarchyUniqueName);
            if (this.Action != null)
            {
                dest.Action = (IMdxAction)this.Action.Clone();
            }
            foreach (DrillActionContainer child in this.Children)
            {
                dest.Children.Add((DrillActionContainer)child.Clone());
            }
            return dest;
            //return container;
        }

        #endregion
    }


    public interface IMdxAction : IMdxFastClonable
    {
        MdxObject Process(MdxObject mdx);
    }

    public class MdxExpandAction : IMdxAction
    {

        public MdxExpandAction(String uniqueName)
        {
            this.MemberUniqueName = uniqueName;
        }
        public readonly string MemberUniqueName;

        #region IMdxAction Members

        public MdxObject Process(MdxObject mdx)
        {
            MdxExpression expr = mdx as MdxExpression;
            if (expr == null) return mdx;

            return new MdxFunctionExpression(
                "DRILLDOWNMEMBER",
                new MdxExpression[] 
                {
                    expr,
                    new MdxObjectReferenceExpression(this.MemberUniqueName)
                });
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return new MdxExpandAction(MemberUniqueName);
        }

        #endregion
    }

    public class MdxCollapseAction : IMdxAction
    {
        public MdxCollapseAction(String uniqueName)
        {
            this.MemberUniqueName = uniqueName;
        }

        public readonly string MemberUniqueName;

        #region IMdxAction Members

        public MdxObject Process(MdxObject mdx)
        {
            MdxExpression expr = mdx as MdxExpression;
            if (expr == null) return mdx;

            return new MdxFunctionExpression(
                "DRILLUPMEMBER",
                new MdxExpression[] 
                {
                    expr,
                    new MdxObjectReferenceExpression(this.MemberUniqueName)
                });
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return new MdxCollapseAction(MemberUniqueName);
        }

        #endregion
    }

    public class MdxDrillDownAction : IMdxAction
    {
        public MdxDrillDownAction(String uniqueName, String hierarchyUniqueName, int levelDepth)
        {
            this.MemberUniqueName = uniqueName;
            this.HierarchyUniqueName = hierarchyUniqueName;
            this.LevelDepth = levelDepth;
        }

        public readonly int LevelDepth = 0;
        public readonly string MemberUniqueName;
        public readonly string HierarchyUniqueName;

        #region IMdxAction Members

        public MdxObject Process(MdxObject mdx)
        {
            MdxExpression expr = mdx as MdxExpression;
            if (expr == null) return mdx;

            /*
WITH
MEMBER [Сценарий].[Сценарии].[Сценарий] AS 'iif (1,[Сценарий].[Сценарии].&[План],[Сценарий].[Сценарии].&[Факт])'
SELECT
HIERARCHIZE({Descendants([Период].[ГКМ].[Год].&[2008],[Период].[ГКМ].[Месяц])}) DIMENSION PROPERTIES PARENT_UNIQUE_NAME ON 0,
HIERARCHIZE(
    // ФИЛЬТР
    FILTER
	(
		DRILLDOWNMEMBER(FILTER(DRILLDOWNMEMBER(Crossjoin({StrToSet('[Персона].[Персонал].[Весь персонал]')},{[Номенклатура].[Вид-Группа-Номенклатура].[Вид].&[Технологические работы].Children}),[Номенклатура].[Вид-Группа-Номенклатура].[Группа].&[Абонентское и гарантийное обслуживание]),(((not ([Номенклатура].[Вид-Группа-Номенклатура].CURRENTMEMBER is [Номенклатура].[Вид-Группа-Номенклатура].[Группа].&[Абонентское и гарантийное обслуживание]))AND(not IsSibling([Номенклатура].[Вид-Группа-Номенклатура].CURRENTMEMBER,[Номенклатура].[Вид-Группа-Номенклатура].[Группа].&[Абонентское и гарантийное обслуживание])))AND(not IsAncestor([Номенклатура].[Вид-Группа-Номенклатура].CURRENTMEMBER,[Номенклатура].[Вид-Группа-Номенклатура].[Группа].&[Абонентское и гарантийное обслуживание])))),[Номенклатура].[Вид-Группа-Номенклатура].[Номенклатура].&[0x80000000000000DE])
        , 
        // УСЛОВИЕ ДЛЯ ФИЛЬТРА
		(
                // ПЕРВОЕ УСЛОВИЕ - Убираем сам элемент если у него количество дочерних не равно 0
					not (
							([Номенклатура].[Вид-Группа-Номенклатура].CURRENTMEMBER is [Номенклатура].[Вид-Группа-Номенклатура].[Номенклатура].&[0x80000000000000DE])
						and ([Номенклатура].[Вид-Группа-Номенклатура].[Номенклатура].&[0x80000000000000DE].Children.Count <> 0)
						)
				AND
                // ВТОРОЕ УСЛОВИЕ - Убираем соседей, кроме самого элемента             
					not (
							IsSibling([Номенклатура].[Вид-Группа-Номенклатура].CURRENTMEMBER,[Номенклатура].[Вид-Группа-Номенклатура].[Номенклатура].&[0x80000000000000DE])
						and 
							not([Номенклатура].[Вид-Группа-Номенклатура].CURRENTMEMBER is [Номенклатура].[Вид-Группа-Номенклатура].[Номенклатура].&[0x80000000000000DE])
						)
				AND
                // ТРЕТЬЕ УСЛОВИЕ - Убираем предков элемента
					not IsAncestor([Номенклатура].[Вид-Группа-Номенклатура].CURRENTMEMBER,[Номенклатура].[Вид-Группа-Номенклатура].[Номенклатура].&[0x80000000000000DE])
 				// ЧЕТВЕРТОЕ УСЛОВИЕ - Оставляем только потомков
				AND 
				(
					IsAncestor([Номенклатура].[Вид-Группа-Номенклатура].[Номенклатура].&[0x80000000000000DE], [Номенклатура].[Вид-Группа-Номенклатура].CURRENTMEMBER) or ([Номенклатура].[Вид-Группа-Номенклатура].CURRENTMEMBER is [Номенклатура].[Вид-Группа-Номенклатура].[Номенклатура].&[0x80000000000000DE])
				)
		)
	)

) DIMENSION PROPERTIES PARENT_UNIQUE_NAME ON 1

FROM [Бюджет]
WHERE(
[Measures].[Количество],
[Статья].[Статьи].&[и_Ресурсы_Загрузка],
[ЦФО].[Менеджмент].&[У-5],
[Подразделение].[Подразделения].[Все подразделения].UNKNOWNMEMBER,
[Сценарий].[Сценарии].&[План]
)
            */
            MdxExpression drillDownExpr = new MdxFunctionExpression(
                "DRILLDOWNMEMBER",
                new MdxExpression[] 
                {
                    expr,
                    new MdxObjectReferenceExpression(this.MemberUniqueName)
                });

            return new MdxFunctionExpression(
                "FILTER",
                new MdxExpression[] 
                {
                    drillDownExpr,

                    new MdxBinaryExpression(
                        new MdxBinaryExpression(

                        new MdxBinaryExpression
                        (
                            // ПЕРВОЕ УСЛОВИЕ - Убираем сам элемент если у него количество дочерних не равно 0
                            new MdxUnaryExpression
                            (
                                "not ",
                                new MdxBinaryExpression
                                (
                                        // Левый операнд
                                        // Кусок([Номенклатура].[Вид-Группа-Номенклатура].CURRENTMEMBER is [Номенклатура].[Вид-Группа-Номенклатура].[Номенклатура].&[0x80000000000000DE])
                                        new MdxBinaryExpression(
                                            new MdxPropertyExpression(
                                                new MdxObjectReferenceExpression(this.HierarchyUniqueName),
                                                "CURRENTMEMBER")
                                            ,
                                            new MdxObjectReferenceExpression(this.MemberUniqueName),
                                            " is "
                                        ),
                                        // Правый операнд
                                        // Кусок ([Номенклатура].[Вид-Группа-Номенклатура].[Номенклатура].&[0x80000000000000DE].Children.Count <> 0)
                                        new MdxBinaryExpression(
                                                // Левый операнд 
                                                new MdxPropertyExpression(
                                                    new MdxPropertyExpression(
                                                        new MdxObjectReferenceExpression(this.MemberUniqueName),
                                                        "Children"),
                                                    "Count"), 
                                                // Правый операнд 
                                                new MdxConstantExpression("0", MdxConstantKind.Integer),
                                                // Операция
                                                "<>"
                                        ),
                                        // Операция
                                        "AND"
                                )
                                    
                            ),
                            // ВТОРОЕ УСЛОВИЕ - Убираем соседей, кроме самого элемента             
                            new MdxUnaryExpression
                            (
                                "not ",
                                new MdxBinaryExpression
                                (
                                    // Левый операнд
                                    // Кусок IsSibling([Номенклатура].[Вид-Группа-Номенклатура].CURRENTMEMBER,[Номенклатура].[Вид-Группа-Номенклатура].[Номенклатура].&[0x80000000000000DE])
                                    new MdxFunctionExpression
                                    (
                                        "IsSibling",
                                        new MdxExpression[] 
                                        {
                                            new MdxPropertyExpression(
                                                new MdxObjectReferenceExpression(this.HierarchyUniqueName),
                                               "CURRENTMEMBER"),
                                            new MdxObjectReferenceExpression(this.MemberUniqueName)
                                        }
                                    ),
                                    // Правый операнд 
                                    // Кусок not([Номенклатура].[Вид-Группа-Номенклатура].CURRENTMEMBER is [Номенклатура].[Вид-Группа-Номенклатура].[Номенклатура].&[0x80000000000000DE])
                                    new MdxUnaryExpression
                                    (
                                        "not ",
                                        new MdxBinaryExpression
                                        (
                                            new MdxPropertyExpression(
                                                new MdxObjectReferenceExpression(this.HierarchyUniqueName),
                                                "CURRENTMEMBER")
                                            ,
                                            new MdxObjectReferenceExpression(this.MemberUniqueName),
                                            " is "
                                        )
                                    )
                                    ,
                                    // Операция
                                    "AND"
                                )
                            )
                            ,
                            // Операция
                            "AND"
                        ),
                        // ТРЕТЬЕ УСЛОВИЕ - Убираем предков элемента
                        new MdxUnaryExpression
                        (
                            "not ",
                            new MdxFunctionExpression(
                                "IsAncestor",
                                new MdxExpression[] 
                                {
                                    new MdxPropertyExpression(
                                        new MdxObjectReferenceExpression(this.HierarchyUniqueName),
                                        "CURRENTMEMBER"),
                                    new MdxObjectReferenceExpression(this.MemberUniqueName)
                                })
                        )
                        ,
                        // Операция
                        "AND"
                        )
                        ,

         				// ЧЕТВЕРТОЕ УСЛОВИЕ - Оставляем только потомков
                        // Кусок IsAncestor([Номенклатура].[Вид-Группа-Номенклатура].[Номенклатура].&[0x80000000000000DE], [Номенклатура].[Вид-Группа-Номенклатура].CURRENTMEMBER) or ([Номенклатура].[Вид-Группа-Номенклатура].CURRENTMEMBER is [Номенклатура].[Вид-Группа-Номенклатура].[Номенклатура].&[0x80000000000000DE])
                        new MdxBinaryExpression
                        (
                            new MdxFunctionExpression
                            (
                                "IsAncestor",
                                new MdxExpression[] 
                                {
                                    new MdxObjectReferenceExpression(this.MemberUniqueName),
                                    new MdxPropertyExpression(
                                        new MdxObjectReferenceExpression(this.HierarchyUniqueName),
                                        "CURRENTMEMBER")
                                }
                            ),
                            new MdxBinaryExpression
                            (
                                new MdxPropertyExpression(
                                    new MdxObjectReferenceExpression(this.HierarchyUniqueName),
                                    "CURRENTMEMBER")
                                    ,
                                new MdxObjectReferenceExpression(this.MemberUniqueName),
                                " is "
                            ),
                            "OR"
                        )
                        ,
                        // Операция
                        "AND"
                    )
                }
            );

        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return new MdxDrillDownAction(MemberUniqueName, HierarchyUniqueName, LevelDepth);
        }

        #endregion
    }

    public class MdxDrillUpAction : IMdxAction
    {
        public MdxDrillUpAction(String uniqueName, String hierarchyUniqueName, int levelDepth)
        {
            this.MemberUniqueName = uniqueName;
            this.HierarchyUniqueName = hierarchyUniqueName;
            this.LevelDepth = levelDepth;
        }

        public readonly int LevelDepth = 0;
        public readonly string MemberUniqueName;
        public readonly string HierarchyUniqueName;

        #region IMdxAction Members

        public MdxObject Process(MdxObject mdx)
        {
            MdxExpression expr = mdx as MdxExpression;
            if (expr == null) return mdx;
            /*
             Формируем запрос вида:
             *    SELECT
HIERARCHIZE(
FILTER(
DRILLUPMEMBER(HIERARCHIZE(CROSSJOIN(DRILLDOWNMEMBER({[Customer].[Customer Geography].[Country].[Australia]},[Customer].[Customer Geography].[Country].&[Australia]),{[Sales Territory].[Sales Territory].[Sales Territory Group].Members})),[Customer].[Customer Geography].[State-Province].&[NSW]&[AU])
,
IsSibling([Customer].[Customer Geography].CURRENTMEMBER,[Customer].[Customer Geography].[State-Province].&[NSW]&[AU].PARENT)
)
) ON 0,
HIERARCHIZE(HIERARCHIZE(head({[Product].[Product Categories].[Category].Members},10))) ON 1
FROM [Adventure Works]
             */
            MdxExpression drillUpExpr = new MdxFunctionExpression(
                "DRILLUPMEMBER",
                new MdxExpression[] 
                {
                    expr,
                    new MdxObjectReferenceExpression(this.MemberUniqueName)
                });

            return new MdxFunctionExpression(
                "FILTER",
                new MdxExpression[] 
                {
                    drillUpExpr,
                    new MdxFunctionExpression(
                        "IsSibling",
                        new MdxExpression[] 
                        {
                            new MdxPropertyExpression(
                                new MdxObjectReferenceExpression(this.HierarchyUniqueName),
                                "CURRENTMEMBER"),
                            new MdxPropertyExpression(
                                new MdxObjectReferenceExpression(this.MemberUniqueName),
                                "PARENT")
                        }
                    )             
                        
                });
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return new MdxDrillUpAction(MemberUniqueName, HierarchyUniqueName, LevelDepth);
        }

        #endregion
    }
}
