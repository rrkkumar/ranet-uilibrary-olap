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
using System.Windows.Browser;
using Ranet.AgOlap.Controls.Forms;
using System.Windows.Controls.Primitives;
using Ranet.AgOlap.Controls.DataSourceInfo;
using System.Globalization;
using System.Threading;
using System.IO;
using System.Text;
//using Ranet.Olap.Core.Compressor;
//using Ranet.Olap.Core.Compressor.GZip;

namespace UILibrary.Olap.UITestApplication
{
    public partial class Page : UserControl
    {
        public Page()
        {
            CultureInfo ci = new CultureInfo("ru");
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            InitializeComponent();

            //member_connectionString.Text = @"Provider=MSOLAP.3;Data Source=dpp-petr\sql2008;Integrated Security=SSPI;Initial Catalog=BPM_Topsoft";
            //member_cubeName.Text = "[Бюджет]";
            //member_hierarchyName.Text = "[Договор].[Договоры]";

            pivorGrid_connectionString.Text = @"Provider=MSOLAP.4;Data Source=DPP-601\SQL2008;Integrated Security=SSPI;Initial Catalog=Galaktika BI AS Write";
            pivorGrid_connectionString.Text = @"Provider=MSOLAP.3;Data Source=dpp-petr\sql2008;Integrated Security=SSPI;Initial Catalog=BPM_Topsoft";
            pivorGrid_connectionString.Text = @"Provider=MSOLAP.3;Data Source=DPP-VM-IPMSDEMO\SQL2008;Integrated Security=SSPI;Initial Catalog=Galaktika IPMS";
            pivorGrid_connectionString.Text = @"Provider=MSOLAP.3;Data Source=VM-BIDEMO\sql2008;Integrated Security=SSPI;Initial Catalog=BPM_Topsoft";
            //pivorGrid_connectionString.Text = @"Provider=MSOLAP.4;Data Source=DPP-VM-IPMS\SQL2008;Integrated Security=SSPI;Initial Catalog=Galaktika IPMS";
            pivorGrid_connectionString.Text = @"Provider=MSOLAP.4;Data Source=DPP-675;Integrated Security=SSPI;Initial Catalog=Adventure Works DW";
            //pivorGrid_connectionString.Text = @"Provider=MSOLAP.4;Data Source=DPP-VM-IPMSDEMO\SQL2008;Integrated Security=SSPI;Initial Catalog=Galaktika IPMS";
            //pivorGrid_connectionString.Text = @"Provider=MSOLAP.4;Data Source=DPP-601\SQL2008;Integrated Security=SSPI;Initial Catalog=Galaktika BI AS";
            //pivorGrid_connectionString.Text = @"Provider=MSOLAP.4;Data Source=DPP-675;Integrated Security=SSPI;Initial Catalog=Adventure Works DW";
            //pivorGrid_connectionString.Text = @"Provider=MSOLAP.4;Data Source=DPP-675\SQL2008;Integrated Security=SSPI;Initial Catalog=Galaktika BI AS";

            pivorGrid_query.Text = @"SELECT
HIERARCHIZE(Crossjoin(
{StrToSet('[Период].[ГКМ].[Год].&[2008]')} 
,StrToSet('{[Сценарий].[Сценарии].Members}')))
Dimension Properties PARENT_UNIQUE_NAME on 0
,HIERARCHIZE({StrToSet('[Персона].[Персонал].[Весь персонал]')}) 
Dimension Properties PARENT_UNIQUE_NAME on 1
,{[Measures].[Сумма]} on 2
,StrToSet('[Статья].[Статьи].&[и_Ресурсы_Загрузка]') on 3
,StrToSet('[ЦФО].[Менеджмент].&[У-5]') on 4
,StrToSet('[Контрагент].[Контрагенты].[Все контрагенты]') on 5
,StrToSet('[Проект].[Проекты].[Все проекты]') on 6
,StrToSet('[Договор].[Договоры].[Все договоры]') on 7
,StrToSet('[Подразделение].[Подразделения].[Все подразделения]') on 8
,StrToSet('[Номенклатура].[Вид-Группа-Номенклатура].[Вся номенклатура]') on 9
,StrToSet('[Бизнес-процесс].[Бизнес-процессы].[Все бизнес-процессы]') on 10
,StrToSet('[Вид Деятельности].[Вид-Группа-Деятельность].[Вид].&[Технологические работы]') on 11
FROM
[Бюджет]
CELL PROPERTIES 
BACK_COLOR, CELL_ORDINAL, FORE_COLOR, FONT_NAME, FONT_SIZE, FONT_FLAGS, FORMAT_STRING, VALUE, FORMATTED_VALUE, UPDATEABLE

 ";

            pivorGrid_query.Text = @"select Subset({[Customer].[Customer Geography].AllMembers}, 0, 200) on 0 from [Adventure Works]";
            pivorGrid_query.Text = @"select Crossjoin({[Date].[Calendar].[Calendar Year].Members}, {[Product].[Product Categories].[Category].Members}) on 0, Subset({[Customer].[Customer Geography].AllMembers}, 0, 200) on 1 from [Adventure Works]";

            pivorGrid_query.Text = @"SELECT
HIERARCHIZE(Crossjoin({StrToSet('[Период].[ГКМ].[Год].&[2008]')},StrToSet('{{[Сценарий].[Сценарии].&[План]},{[Сценарий].[Сценарии].&[Факт]},{[Сценарий].[Сценарии].[Все сценарии].[% выполнения]}}'))) DIMENSION PROPERTIES PARENT_UNIQUE_NAME ON 0,
HIERARCHIZE(DRILLDOWNMEMBER(DRILLDOWNMEMBER({StrToSet('[Персона].[Персонал].[Весь персонал]')},[Персона].[Персонал].[Весь персонал]),[Персона].[Персонал].&[0x806600000000000A])) DIMENSION PROPERTIES PARENT_UNIQUE_NAME ON 1
FROM [Бюджет]
WHERE 
({[Measures].[Сумма]},StrToSet('[Статья].[Статьи].&[и_Ресурсы_Загрузка]'),StrToSet('[ЦФО].[Менеджмент].&[У-5]'),StrToSet('[Контрагент].[Контрагенты].[Все контрагенты]'),StrToSet('[Проект].[Проекты].[Все проекты]'),StrToSet('[Договор].[Договоры].[Все договоры]'),StrToSet('[Подразделение].[Подразделения].[Все подразделения]'),StrToSet('[Номенклатура].[Вид-Группа-Номенклатура].[Вся номенклатура]'),StrToSet('[Бизнес-процесс].[Бизнес-процессы].[Все бизнес-процессы]'),StrToSet('[Вид Деятельности].[Вид-Группа-Деятельность].[Вид].&[Технологические работы]'))
CELL PROPERTIES BACK_COLOR,CELL_ORDINAL,FORE_COLOR,FONT_NAME,FONT_SIZE,FONT_FLAGS,FORMAT_STRING,VALUE,FORMATTED_VALUE,UPDATEABLE";

            pivorGrid_query.Text = @"SELECT
HIERARCHIZE(Crossjoin(
{StrToSet('[Период].[ГКМ].[Год].&[2008]')} 
,StrToSet('{{[Сценарий].[Сценарии].&[План]},{[Сценарий].[Сценарии].&[Факт]},{[Сценарий].[Сценарии].[Все сценарии].[% выполнения]}}')))
Dimension Properties PARENT_UNIQUE_NAME on 0

,HIERARCHIZE({StrToSet('[Персона].[Персонал].[Весь персонал]')}) 
Dimension Properties PARENT_UNIQUE_NAME on 1
,{[Measures].[Сумма]} on 2
,StrToSet('[Статья].[Статьи].&[и_Ресурсы_Загрузка]') on 3
,StrToSet('[ЦФО].[Менеджмент].&[У-5]') on 4
,StrToSet('[Контрагент].[Контрагенты].[Все контрагенты]') on 5
,StrToSet('[Проект].[Проекты].[Все проекты]') on 6
,StrToSet('[Договор].[Договоры].[Все договоры]') on 7
,StrToSet('[Подразделение].[Подразделения].[Все подразделения]') on 8
,StrToSet('[Номенклатура].[Вид-Группа-Номенклатура].[Вся номенклатура]') on 9
,StrToSet('[Бизнес-процесс].[Бизнес-процессы].[Все бизнес-процессы]') on 10
,StrToSet('[Вид Деятельности].[Вид-Группа-Деятельность].[Вид].&[Технологические работы]') on 11
FROM
[Бюджет]
CELL PROPERTIES 
BACK_COLOR, CELL_ORDINAL, FORE_COLOR, FONT_NAME, FONT_SIZE, FONT_FLAGS, FORMAT_STRING, VALUE, FORMATTED_VALUE, UPDATEABLE
";

            // DATAMEMBER + UNKNOWNMEMBER
            pivorGrid_query.Text = @"Select {[Проект].[Проекты].AllMembers} on 0,
 {[Номенклатура].[Вид-Группа-Номенклатура].[Номенклатура].Members} on 1 from [Бюджет]";

            // CUSTOM_ROLLUP + UNARY_OPERATOR
            pivorGrid_query.Text = @"WITH

MEMBER [Measures].[Индекс] AS iif(([Measures].[СметаБаз] = 0),null,([Measures].[СметаТек] / [Measures].[СметаБаз]))

SELECT

{{(([Measures].[СметаБаз] * [Период].[Год].[(All)]) * StrToSet('{{[Вид затрат].[Вид затрат].&[ВЗ_СМР]}, {[Вид затрат].[Вид затрат].&[ВЗ_Оборудование]}, {[Вид затрат].[Вид затрат].&[ВЗ_Прочие]}}')),(([Measures].[СметаТек] * [Период].[Год].[Год].Members) * StrToSet('{{[Вид затрат].[Вид затрат].&[ВЗ_СМР]}, {[Вид затрат].[Вид затрат].&[ВЗ_Оборудование]}, {[Вид затрат].[Вид затрат].&[ВЗ_Прочие]}}')),(([Measures].[Индекс] * [Период].[Год].[Год].Members) * StrToSet('{{[Вид затрат].[Вид затрат].&[ВЗ_СМР]}, {[Вид затрат].[Вид затрат].&[ВЗ_Оборудование]}, {[Вид затрат].[Вид затрат].&[ВЗ_Прочие]}}'))}} DIMENSION PROPERTIES PARENT_UNIQUE_NAME, CUSTOM_ROLLUP, UNARY_OPERATOR ON 0,

HIERARCHIZE(DRILLDOWNMEMBER(FILTER(DRILLDOWNMEMBER({StrToSet('[Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций]')},[Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций]),((((not (([Объект инвестиций].[Объект инвестиций].CURRENTMEMBER  is  [Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций]) AND ([Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций].Children.Count <> 0))) AND (not (IsSibling([Объект инвестиций].[Объект инвестиций].CURRENTMEMBER,[Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций]) AND (not ([Объект инвестиций].[Объект инвестиций].CURRENTMEMBER  is  [Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций]))))) AND (not IsAncestor([Объект инвестиций].[Объект инвестиций].CURRENTMEMBER,[Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций]))) AND (IsAncestor([Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций],[Объект инвестиций].[Объект инвестиций].CURRENTMEMBER) OR ([Объект инвестиций].[Объект инвестиций].CURRENTMEMBER  is  [Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций])))),[Объект инвестиций].[Объект инвестиций].&[БАЛ.АТИ.Свод])) DIMENSION PROPERTIES PARENT_UNIQUE_NAME, CUSTOM_ROLLUP, UNARY_OPERATOR ON 1

FROM (

SELECT

{StrToSet('[Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций]')} ON 0

FROM (

SELECT

{StrToSet('[Период].[Год].&[2008]')} ON 0

FROM (

SELECT

{StrToSet('{{[Вид затрат].[Вид затрат].&[ВЗ_СМР]}, {[Вид затрат].[Вид затрат].&[ВЗ_Оборудование]}, {[Вид затрат].[Вид затрат].&[ВЗ_Прочие]}}')} ON 0

FROM [GalaktikaIPMS]

)

)

)

WHERE 

StrToTuple('( 

[Поток].[Поток].&[Моделирование]

,[Сценарий].[Сценарий].&[2008НБалАЭС].DATAMEMBER)')

CELL PROPERTIES BACK_COLOR,CELL_ORDINAL,FORE_COLOR,FONT_NAME,FONT_SIZE,FONT_FLAGS,FORMAT_STRING,VALUE,FORMATTED_VALUE,UPDATEABLE
";

  // Несколько строк в наименовании
            pivorGrid_query.Text = @"WITH

MEMBER [Тип значения].[Тип значения].[Отклонение] AS 'iif(IsLeaf([Объект инвестиций].[Объект инвестиций].CurrentMember)

 ,[Тип значения].[Тип значения].&[План] - [Тип значения].[Тип значения].&[Факт]

 ,SUM(iif(IsLeaf([Объект инвестиций].[Объект инвестиций].CurrentMember),[Объект инвестиций].[Объект инвестиций].CurrentMember,[Объект инвестиций].[Объект инвестиций].Children),[Тип значения].[Тип значения].[Отклонение]))'

SET [Объекты инвестиций] AS iif(('{{[Объект инвестиций].[Объект инвестиций].&[С01]}, {[Объект инвестиций].[Объект инвестиций].&[С23]}, {[Объект инвестиций].[Объект инвестиций].&[С24]}, {[Объект инвестиций].[Объект инвестиций].&[С47]}}' = '[Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций]'),{[Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций],[Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций].Children},StrToSet('{{[Объект инвестиций].[Объект инвестиций].&[С01]}, {[Объект инвестиций].[Объект инвестиций].&[С23]}, {[Объект инвестиций].[Объект инвестиций].&[С24]}, {[Объект инвестиций].[Объект инвестиций].&[С47]}}'))

SELECT

{[Тип значения].[Тип значения].&[План],[Тип значения].[Тип значения].&[Факт],[Тип значения].[Тип значения].[Отклонение]} DIMENSION PROPERTIES PARENT_UNIQUE_NAME,CUSTOM_ROLLUP,UNARY_OPERATOR ON 0,

{[Объекты инвестиций]} DIMENSION PROPERTIES PARENT_UNIQUE_NAME,CUSTOM_ROLLUP,UNARY_OPERATOR ON 1

FROM (

SELECT

{StrToSet('{{[Объект инвестиций].[Объект инвестиций].&[С01]}, {[Объект инвестиций].[Объект инвестиций].&[С23]}, {[Объект инвестиций].[Объект инвестиций].&[С24]}, {[Объект инвестиций].[Объект инвестиций].&[С47]}}')} ON 0

FROM (

SELECT

{StrToSet('[Объект инвестиций].[ЦФО].[Все объекты инвестиций]')} ON 0

FROM [GalaktikaIPMS]

)

)

WHERE 

StrToTuple('([Measures].[СчМесяц]

, [Показатель].[Показатель].&[Финансирование] 

, [Сценарий].[Сценарий].[Все сценарии]

, [Период].[ГКМ].[Все периоды]

, [Счет].[Счет].&[С_19]

, [Вид цен].[Вид цен].&[тек]

, [Вид значения].[Вид значения].&[Оборот]

, [Поток].[Поток].[Все потоки]

)')

CELL PROPERTIES BACK_COLOR,CELL_ORDINAL,FORE_COLOR,FONT_NAME,FONT_SIZE,FONT_FLAGS,FORMAT_STRING,VALUE,FORMATTED_VALUE,UPDATEABLE

"; 

            // Вчисляемые эелемнты выстраивались в иерархию
            pivorGrid_query.Text = @"WITH
SET [ОИ1] AS {[Объект инвестиций].[Объект инвестиций].Members} SET [ОИ2] AS {iif(('[Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций]' = '[Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций]'),{[Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций],[Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций].Children},StrToSet('[Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций]'))} SET [ОИ] AS {Filter([Объект инвестиций].[Объект инвестиций].Members,((([Объект инвестиций].[Объект инвестиций].CurrentMember.PROPERTIES('Вид') = 'Стройка') and ([Объект инвестиций].[Объект инвестиций].CurrentMember.PROPERTIES('Проект') <> '')) and (not [Объект инвестиций].[Объект инвестиций].IS_DATAMEMBER)))} SET [ИФ] AS {StrToSet('[Источник финансирования].[Источник финансирования].[Все источники финансирования]')} MEMBER [Объект инвестиций].[Объект инвестиций].[ИТОГО по ОИ] AS [Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций] MEMBER [Объект инвестиций].[Объект инвестиций].[ИТОГО по ОИ(ВЗИФ)] AS ([Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций],[Источник финансирования].[Источник финансирования].[Все источники финансирования],[Measures].[ВЗИФ])
MEMBER [Объект инвестиций].[Объект инвестиций].[Расхождение с Итого по ИФ] AS ([Объект инвестиций].[Объект инвестиций].[ИТОГО по ОИ] - [Объект инвестиций].[Объект инвестиций].[ИТОГО по ОИ(ВЗИФ)]),BACK_COLOR='iif([Объект инвестиций].[Объект инвестиций].[Расхождение с Итого по ИФ]<>0,RGB(255, 128, 128),RGB(255, 255, 255))'
MEMBER [Источник финансирования].[Источник финансирования].[ИТОГО по ИФ] AS (StrToMember('[Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций]'),[Источник финансирования].[Источник финансирования].[Все источники финансирования]) MEMBER [Объект инвестиций].[Объект инвестиций].[ИТОГО по ИФ(ВЗ)] AS ([Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций],[Источник финансирования].[Источник финансирования].[Все источники финансирования],[Measures].[ВЗ]) MEMBER [Источник финансирования].[Источник финансирования].[Расхождение с Итого по ОИ] AS ([Источник финансирования].[Источник финансирования].[ИТОГО по ИФ] - [Объект инвестиций].[Объект инвестиций].[ИТОГО по ИФ(ВЗ)]),BACK_COLOR='iif([Источник финансирования].[Расхождение с Итого по ОИ]<>0,RGB(255, 128, 128),RGB(255, 255, 255))'
SELECT
Hierarchize({(Iif(('1' = '1'),StrToSet('{ {
  {[Показатель].[Показатель].&[Освоение] ,[Показатель].[Показатель].&[ВводОФ] ,[Показатель].[Показатель].&[Активы] ,[Показатель].[Показатель].&[НЗС]}
* {[Вид значения].[Вид значения].&[Начало]}
* {[Тип значения].[Тип значения].&[Факт]} } ,{
  {[Показатель].[Показатель].&[Освоение], [Показатель].[Показатель].&[ВводОФ]}
* {[Вид значения].[Вид значения].&[Прирост]}
* {[Тип значения].[Тип значения].&[План], [Тип значения].[Тип значения].&[Факт]} } ,{
  {[Показатель].[Показатель].&[Освоение] ,[Показатель].[Показатель].&[ВводОФ] ,[Показатель].[Показатель].&[НЗС]}
* {[Вид значения].[Вид значения].&[Конец]}
* {[Тип значения].[Тип значения].&[Факт]} } }'),StrToSet('{ {
  {[Показатель].[Показатель].&[Смета] ,[Показатель].[Показатель].&[СметаОстаток] ,[Показатель].[Показатель].&[Освоение] ,[Показатель].[Показатель].&[ВводОФ] ,[Показатель].[Показатель].&[Активы] ,[Показатель].[Показатель].&[НЗС]}
* {[Вид значения].[Вид значения].&[Начало]}
* {[Тип значения].[Тип значения].&[Факт]} } ,{
  {[Показатель].[Показатель].&[Освоение], [Показатель].[Показатель].&[ВводОФ]}
* {[Вид значения].[Вид значения].&[Прирост]}
* {[Тип значения].[Тип значения].&[План], [Тип значения].[Тип значения].&[Факт]} } ,{
  {[Показатель].[Показатель].&[Освоение] ,[Показатель].[Показатель].&[ВводОФ] ,[Показатель].[Показатель].&[НЗС] ,[Показатель].[Показатель].&[СметаОстаток]}
* {[Вид значения].[Вид значения].&[Конец]}
* {[Тип значения].[Тип значения].&[Факт]} }
}')) * StrToSet('{[Вид затрат].[Вид затрат].&[ВЗ_Строительство]}'))}) DIMENSION PROPERTIES PARENT_UNIQUE_NAME,CUSTOM_ROLLUP,UNARY_OPERATOR, KEY0 ON 0, Hierarchize({(Iif(('1' = '1'),{[Источник финансирования].[Источник финансирования].[Расхождение с Итого по ОИ],[Источник финансирования].[Источник финансирования].[ИТОГО по ИФ],[ИФ]},{[Объект инвестиций].[Объект инвестиций].[Расхождение с Итого по ИФ],[Объект инвестиций].[Объект инвестиций].[ИТОГО по ОИ],[ОИ]}) * StrToSet('{[Вид цен].[Вид цен].&[баз],[Вид цен].[Вид цен].&[тек]}'))}) DIMENSION PROPERTIES PARENT_UNIQUE_NAME,CUSTOM_ROLLUP,UNARY_OPERATOR, KEY0 ON 1 FROM ( SELECT {Iif(('1' = '0'),StrToSet('[Объект инвестиций].[Проект].[Все объекты инвестиций]'),StrToSet('[Источник финансирования].[Источник финансирования].[Все источники финансирования]'))} ON 0 FROM [GalaktikaIPMS]
)
WHERE
Iif(('1' = '1'),StrToTuple('( [Measures].[ВЗИФ]
				, [Поток].[Поток].[Все потоки]
				, [Сценарий].[Сценарий].[Все сценарии]
				, [Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций]
				, [Период].[Год].&[2008]
				)'),StrToTuple('( [Measures].[ВЗ]
				, [Поток].[Поток].[Все потоки]
				, [Сценарий].[Сценарий].[Все сценарии]
				, [Период].[Год].&[2008]
				)'))
CELL PROPERTIES BACK_COLOR,CELL_ORDINAL,FORE_COLOR,FONT_NAME,FONT_SIZE,FONT_FLAGS,FORMAT_STRING,VALUE,FORMATTED_VALUE,UPDATEABLE
";

            // UNARY_OPERATOR не отображалась тильда 
            pivorGrid_query.Text = @"WITH

SET [PickPeriod] AS 'StrToSet(""{[Период].[ГКМД].[Год].&[2008]}"")'

SET [PickScenario] AS '{[Сценарий].[Сценарии].&[План],[Сценарий].[Сценарии].&[Факт],[Сценарий].[Сценарии].[% выполнения]}'

SET [PickItem] AS 'StrToSet(""{{[Статья].[Статьи].&[Индексы]}, {[Статья].[Статьи].&[Доходы]}, {[Статья].[Статьи].&[Затраты]}, {[Статья].[Статьи].&[Баланс]}, {[Статья].[Статьи].[Все статьи].UNKNOWNMEMBER}}"")'

SELECT

HIERARCHIZE(DRILLDOWNMEMBER({Crossjoin([PickPeriod],[PickScenario])},[Период].[ГКМД].[Год].&[2008])) DIMENSION PROPERTIES PARENT_UNIQUE_NAME,CUSTOM_ROLLUP,UNARY_OPERATOR,KEY0 ON 0,

HIERARCHIZE(DRILLDOWNMEMBER(DRILLDOWNMEMBER(DRILLDOWNMEMBER({[PickItem]},[Статья].[Статьи].&[Индексы]),[Статья].[Статьи].&[Затраты]),[Статья].[Статьи].&[Доходы])) DIMENSION PROPERTIES PARENT_UNIQUE_NAME,CUSTOM_ROLLUP,UNARY_OPERATOR,KEY0 ON 1,

[Measures].[Сумма] ON 2,

StrToSet('[Номенклатура].[Вид-Группа-Номенклатура].[Вся номенклатура]') ON 3,

StrToSet('[Контрагент].[Контрагенты].[Все контрагенты]') ON 4,

StrToSet('[Проект].[Проекты].[Все проекты]') ON 5,

StrToSet('[Персона].[Персонал].[Весь персонал]') ON 6,

StrToSet('[ЦФО].[Менеджмент].&[У-5]') ON 7

FROM [Бюджет]

CELL PROPERTIES BACK_COLOR,CELL_ORDINAL,FORE_COLOR,FONT_NAME,FONT_SIZE,FONT_FLAGS,FORMAT_STRING,VALUE,FORMATTED_VALUE,UPDATEABLE

";
            // Вместо ключа 0 для элементов уровня All отображаем наименование
            pivorGrid_query.Text = @"Select {[Geography].[Geography].Members} DIMENSION PROPERTIES PARENT_UNIQUE_NAME,CUSTOM_ROLLUP,UNARY_OPERATOR,KEY0 on 0,
{[Customer].[Customer Geography].[Country].Members} DIMENSION PROPERTIES PARENT_UNIQUE_NAME,CUSTOM_ROLLUP,UNARY_OPERATOR,KEY0 on 1
 from
[Adventure Works]";

            // UNARY_OPERATOR не отображалась тильда 
//            pivorGrid_query.Text = @"WITH
//MEMBER [Measures].[Освоение_План] AS ([Показатель].[Показатель].&[Освоение],[Вид значения].[Вид значения].&[Оборот],[Тип значения].[Тип значения].&[План],[Measures].[ВЗ])
//MEMBER [Measures].[Освоение_Факт] AS ([Показатель].[Показатель].&[Освоение],[Вид значения].[Вид значения].&[Оборот],[Тип значения].[Тип значения].&[Факт],[Measures].[ВЗ])
//MEMBER [Measures].[Финансирование_План] AS ([Показатель].[Показатель].&[Финансирование КВЛ],[Вид значения].[Вид значения].&[Оборот],[Тип значения].[Тип значения].&[План],[Measures].[ВЗ])
//MEMBER [Measures].[Финансирование_Факт] AS ([Показатель].[Показатель].&[Финансирование КВЛ],[Вид значения].[Вид значения].&[Оборот],[Тип значения].[Тип значения].&[Факт],[Measures].[ВЗ])
//MEMBER [Measures].[ВводОФ_План] AS ([Показатель].[Показатель].&[ВводОФ],[Вид значения].[Вид значения].&[Оборот],[Тип значения].[Тип значения].&[План],[Measures].[ВЗ])
//MEMBER [Measures].[ВводОФ_Факт] AS ([Показатель].[Показатель].&[ВводОФ],[Вид значения].[Вид значения].&[Оборот],[Тип значения].[Тип значения].&[Факт],[Measures].[ВЗ])
//MEMBER [Measures].[Освоение_ПланМеньшеФакт] AS Iif(([Measures].[Освоение_План] < [Measures].[Освоение_Факт]),1,0)
//MEMBER [Measures].[Освоение_ПланМеньше0] AS Iif(([Measures].[Освоение_План] < 0),1,0)
//MEMBER [Measures].[Освоение_ФактМеньше0] AS Iif(([Measures].[Освоение_Факт] < 0),1,0)
//MEMBER [Measures].[Финансирование_ПланМеньшеФакт] AS Iif(([Measures].[Финансирование_План] < [Measures].[Финансирование_Факт]),1,0)
//MEMBER [Measures].[Финансирование_ПланМеньше0] AS Iif(([Measures].[Финансирование_План] < 0),1,0)
//MEMBER [Measures].[Финансирование_ФактМеньше0] AS Iif(([Measures].[Финансирование_Факт] < 0),1,0)
//MEMBER [Measures].[ВводОФ_ПланМеньшеФакт] AS Iif(([Measures].[ВводОФ_План] < [Measures].[ВводОФ_Факт]),1,0)
//MEMBER [Measures].[ВводОФ_ПланМеньше0] AS Iif(([Measures].[ВводОФ_План] < 0),1,0)
//MEMBER [Measures].[ВводОФ_ФактМеньше0] AS Iif(([Measures].[ВводОФ_Факт] < 0),1,0)
//MEMBER [Measures].[% выпонения] AS 'iif (IsNull([Measures].[Освоение_План]) or ([Measures].[Освоение_План] = 0),NULL,([Measures].[Освоение_Факт]/[Measures].[Освоение_План]))',FORMAT_STRING='Percent',BACK_COLOR='iif(IsNull([Measures].[% выпонения]=0) or ([Measures].[% выпонения]=0),RGB(255, 255, 255)
//,iif([Measures].[% выпонения]<0.7,RGB(255, 128, 128)
//,iif([Measures].[% выпонения]<0.9,RGB(255, 255, 153),RGB(204, 255, 204))))'
//MEMBER [Measures].[Тенденция] AS (StrToTuple('([Сценарий].[Сценарий].&[2008Н],[Measures].[% выпонения])') - StrToTuple('([Сценарий].[Сценарий].&[2008Н],[Measures].[% выпонения])'))
//MEMBER [Сценарий].[Сценарий].[Результат] AS StrToMember('[Сценарий].[Сценарий].&[2008Н]')
//SET [MeaCompare] AS '{[Measures].[Освоение_План], [Measures].[Освоение_Факт], [Measures].[% выпонения]}'
//SELECT
//{({StrToMember('[Сценарий].[Сценарий].&[2008Н]'),StrToMember('[Сценарий].[Сценарий].&[2008Н]')} * [MeaCompare]),([Сценарий].[Сценарий].[Результат],[Measures].[Тенденция])} DIMENSION PROPERTIES PARENT_UNIQUE_NAME,CUSTOM_ROLLUP,UNARY_OPERATOR,KEY0 ON 0,
//{(StrToMember('[Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций]') * {[Вид цен].[Вид цен].&[баз],[Вид цен].[Вид цен].&[тек]})} DIMENSION PROPERTIES PARENT_UNIQUE_NAME,CUSTOM_ROLLUP,UNARY_OPERATOR,KEY0 ON 1
//FROM (
//SELECT
//{StrToSet('[Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций]')} ON 0
//FROM (
//SELECT
//{StrToSet('{{[Вид затрат].[Вид затрат].&[ВЗ_СМР]}, {[Вид затрат].[Вид затрат].&[ВЗ_Оборудование]}, {[Вид затрат].[Вид затрат].&[ВЗ_Прочие]}}')} ON 0
//FROM (
//SELECT
//{StrToSet('[Источник финансирования].[Источник финансирования].[Все источники финансирования]')} ON 0
//FROM (
//SELECT
//{StrToSet('[Объект инвестиций].[ЦФО].[Все объекты инвестиций]')} ON 0
//FROM [GalaktikaIPMS]
//)
//)
//)
//)
//WHERE 
//StrToTuple('([Период].[ГКМ].[Все периоды])')
//CELL PROPERTIES BACK_COLOR,CELL_ORDINAL,FORE_COLOR,FONT_NAME,FONT_SIZE,FONT_FLAGS,FORMAT_STRING,VALUE,FORMATTED_VALUE";     

  // Появлялось многоточие при масштабировании
//            pivorGrid_query.Text = @"WITH
//
//MEMBER [Тип значения].[Тип значения].[Отклонение] AS 'iif(IsLeaf([Объект инвестиций].[Объект инвестиций].CurrentMember)
//
// ,[Тип значения].[Тип значения].&[План] - [Тип значения].[Тип значения].&[Факт]
//
// ,SUM(iif(IsLeaf([Объект инвестиций].[Объект инвестиций].CurrentMember),[Объект инвестиций].[Объект инвестиций].CurrentMember,[Объект инвестиций].[Объект инвестиций].Children),[Тип значения].[Тип значения].[Отклонение]))'
//
//SET [Объекты инвестиций] AS iif(('{{[Объект инвестиций].[Объект инвестиций].&[С01]}, {[Объект инвестиций].[Объект инвестиций].&[С23]}, {[Объект инвестиций].[Объект инвестиций].&[С24]}, {[Объект инвестиций].[Объект инвестиций].&[С47]}}' = '[Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций]'),{[Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций],[Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций].Children},StrToSet('{{[Объект инвестиций].[Объект инвестиций].&[С01]}, {[Объект инвестиций].[Объект инвестиций].&[С23]}, {[Объект инвестиций].[Объект инвестиций].&[С24]}, {[Объект инвестиций].[Объект инвестиций].&[С47]}}'))
//
//SELECT
//
//{[Тип значения].[Тип значения].&[План]} DIMENSION PROPERTIES PARENT_UNIQUE_NAME,CUSTOM_ROLLUP,UNARY_OPERATOR ON 0,
//
//{[Участник].[По видам].[Вид].&[Инжиниринг]} DIMENSION PROPERTIES PARENT_UNIQUE_NAME,CUSTOM_ROLLUP,UNARY_OPERATOR ON 1
//
//FROM (
//
//SELECT
//
//{StrToSet('{{[Объект инвестиций].[Объект инвестиций].&[С01]}, {[Объект инвестиций].[Объект инвестиций].&[С23]}, {[Объект инвестиций].[Объект инвестиций].&[С24]}, {[Объект инвестиций].[Объект инвестиций].&[С47]}}')} ON 0
//
//FROM (
//
//SELECT
//
//{StrToSet('[Объект инвестиций].[ЦФО].[Все объекты инвестиций]')} ON 0
//
//FROM [GalaktikaIPMS]
//
//)
//
//)
//
//WHERE 
//
//StrToTuple('([Measures].[СчМесяц]
//
//, [Показатель].[Показатель].&[Финансирование] 
//
//, [Сценарий].[Сценарий].[Все сценарии]
//
//, [Период].[ГКМ].[Все периоды]
//
//, [Счет].[Счет].&[С_19]
//
//, [Вид цен].[Вид цен].&[тек]
//
//, [Вид значения].[Вид значения].&[Оборот]
//
//, [Поток].[Поток].[Все потоки]
//
//)')
//
//CELL PROPERTIES BACK_COLOR,CELL_ORDINAL,FORE_COLOR,FONT_NAME,FONT_SIZE,FONT_FLAGS,FORMAT_STRING,VALUE,FORMATTED_VALUE,UPDATEABLE
//";

 

            // Ошибка с сохранением позиции при раскрытии
//            pivorGrid_query.Text = @"WITH
//
//MEMBER [Тип значения].[Тип значения].[Отклонение] AS 'iif(IsLeaf([Объект инвестиций].[Объект инвестиций].CurrentMember)
//
// ,[Тип значения].[Тип значения].&[План] - [Тип значения].[Тип значения].&[Факт]
//
// ,SUM(iif(IsLeaf([Объект инвестиций].[Объект инвестиций].CurrentMember),[Объект инвестиций].[Объект инвестиций].CurrentMember,[Объект инвестиций].[Объект инвестиций].Children),[Тип значения].[Тип значения].[Отклонение]))'
//
//SET [Объекты инвестиций] AS iif(('{{[Объект инвестиций].[Объект инвестиций].&[С01]}, {[Объект инвестиций].[Объект инвестиций].&[С23]}, {[Объект инвестиций].[Объект инвестиций].&[С24]}, {[Объект инвестиций].[Объект инвестиций].&[С47]}}' = '[Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций]'),{[Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций],[Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций].Children},StrToSet('{{[Объект инвестиций].[Объект инвестиций].&[С01]}, {[Объект инвестиций].[Объект инвестиций].&[С23]}, {[Объект инвестиций].[Объект инвестиций].&[С24]}, {[Объект инвестиций].[Объект инвестиций].&[С47]}}'))
//
//SELECT
//
//{[Тип значения].[Тип значения].&[План],[Тип значения].[Тип значения].&[Факт],[Тип значения].[Тип значения].[Отклонение]} DIMENSION PROPERTIES PARENT_UNIQUE_NAME,CUSTOM_ROLLUP,UNARY_OPERATOR ON 0,
//
//CrossJoin({[Объекты инвестиций]},{[Участник].[По видам].[Вид].Members}) DIMENSION PROPERTIES PARENT_UNIQUE_NAME,CUSTOM_ROLLUP,UNARY_OPERATOR ON 1
//
//FROM (
//
//SELECT
//
//{StrToSet('{{[Объект инвестиций].[Объект инвестиций].&[С01]}, {[Объект инвестиций].[Объект инвестиций].&[С23]}, {[Объект инвестиций].[Объект инвестиций].&[С24]}, {[Объект инвестиций].[Объект инвестиций].&[С47]}}')} ON 0
//
//FROM (
//
//SELECT
//
//{StrToSet('[Объект инвестиций].[ЦФО].[Все объекты инвестиций]')} ON 0
//
//FROM [GalaktikaIPMS]
//
//)
//
//)
//
//WHERE 
//
//StrToTuple('([Measures].[СчМесяц]
//
//, [Показатель].[Показатель].&[Финансирование] 
//
//, [Сценарий].[Сценарий].[Все сценарии]
//
//, [Период].[ГКМ].[Все периоды]
//
//, [Счет].[Счет].&[С_19]
//
//, [Вид цен].[Вид цен].&[тек]
//
//, [Вид значения].[Вид значения].&[Оборот]
//
//, [Поток].[Поток].[Все потоки]
//
//)')
//
//CELL PROPERTIES BACK_COLOR,CELL_ORDINAL,FORE_COLOR,FONT_NAME,FONT_SIZE,FONT_FLAGS,FORMAT_STRING,VALUE,FORMATTED_VALUE,UPDATEABLE
//";



            // Adwenture Works
            //pivorGrid_query.Text = @"select Crossjoin({[Date].[Calendar].[Calendar Year].Members}, {[Product].[Product Categories].[Category].Members}) on 0, Subset({[Customer].[Customer Geography].AllMembers}, 0, 200) on 1 from [Adventure Works]";


//            pivorGrid_query.Text = @"WITH
//SET [PickPeriod] AS '{
// Ancestor([Период].[ГКМД].[День].[18.01.2008],[Период].[ГКМД].[Месяц])
//,Ancestor([Период].[ГКМД].[День].[18.01.2008],[Период].[ГКМД].[Квартал])
//,Ancestor([Период].[ГКМД].[День].[18.01.2008],[Период].[ГКМД].[Год])
//}'
//MEMBER [Сценарий].[Сценарии].[По плану] AS '[Сценарий].[Сценарии].&[План]'
//MEMBER [Сценарий].[Сценарии].[Фактически] AS '[Сценарий].[Сценарии].&[Факт]'
//SET [PickScenario] AS '{
// [Сценарий].[Сценарии].[По плану]
//,[Сценарий].[Сценарии].[Фактически]
////  [Сценарий].[Сценарии].&[План]
//// ,[Сценарий].[Сценарии].&[Факт]
// ,[Сценарий].[Сценарии].[% выполнения]
// }'
//SET [PickCFO] AS StrToSet('[ЦФО].[Менеджмент].&[У-5]')
//SET [PickProject] AS StrToSet('[Проект].[Проекты].[Все проекты]')
//SELECT
//{StrToTuple('([Период].[ГКМД].[День].[18.01.2008],[Сценарий].[Сценарии].&[Факт])'),Crossjoin([PickPeriod],[PickScenario])} DIMENSION PROPERTIES PARENT_UNIQUE_NAME ON 0,
//HIERARCHIZE({Crossjoin([PickCFO],[PickProject])}) DIMENSION PROPERTIES PARENT_UNIQUE_NAME ON 1,
//{[Measures].[Сумма]} ON 2,
//StrToSet('[Статья].[Статьи].&[и_Ресурсы_Загрузка]') ON 3,
//StrToSet('[Вид Деятельности].[Вид-Группа-Деятельность].[Вид].&[Технологические работы]') ON 4,
//StrToSet('[Контрагент].[Контрагенты].[Все контрагенты]') ON 5,
//StrToSet('[Персона].[Персонал].[Весь персонал]') ON 6,
//StrToSet('[Договор].[Договоры].[Все договоры]') ON 7,
//StrToSet('[Подразделение].[Подразделения].[Все подразделения]') ON 8,
//StrToSet('[Номенклатура].[Вид-Группа-Номенклатура].[Вся номенклатура]') ON 9,
//StrToSet('[Бизнес-процесс].[Бизнес-процессы].[Все бизнес-процессы]') ON 10
//FROM [Бюджет]
//CELL PROPERTIES BACK_COLOR,CELL_ORDINAL,FORE_COLOR,FONT_NAME,FONT_SIZE,FONT_FLAGS,FORMAT_STRING,VALUE,FORMATTED_VALUE,UPDATEABLE
//";

//            pivorGrid_updateScript.Text = @"UPDATE CUBE [Бюджет]
//SET 
//(
// Iif($$[Статья].[Статьи]$$.LEVEL_NUMBER=0,$$[Статья].[Статьи]$$.UNKNOWNMEMBER,Iif(IsLeaf($$[Статья].[Статьи]$$),$$[Статья].[Статьи]$$,$$[Статья].[Статьи]$$.DATAMEMBER))
//,iif(IsLeaf($$[Сценарий].[Сценарии]$$),$$[Сценарий].[Сценарии]$$,$$[Сценарий].[Сценарии]$$.DATAMEMBER)
//,Iif($$[ЦФО].[Менеджмент]$$.LEVEL_NUMBER=0,$$[ЦФО].[Менеджмент]$$.UNKNOWNMEMBER,Iif(IsLeaf($$[ЦФО].[Менеджмент]$$),$$[ЦФО].[Менеджмент]$$,$$[ЦФО].[Менеджмент]$$.DATAMEMBER))
//,Iif($$[Персона].[Персонал]$$.LEVEL_NUMBER=0,$$[Персона].[Персонал]$$.UNKNOWNMEMBER,Iif(IsLeaf($$[Персона].[Персонал]$$),$$[Персона].[Персонал]$$,$$[Персона].[Персонал]$$.DATAMEMBER))
//,DESCENDANTS(LinkMember($$[Период].[ГКМ]$$,[Период].[ГКМД]),,LEAVES).Item(0)
//,Iif($$[Вид Деятельности].[Вид-Группа-Деятельность]$$.LEVEL_NUMBER=0,$$[Вид Деятельности].[Вид-Группа-Деятельность]$$.UNKNOWNMEMBER,Iif(IsLeaf($$[Вид Деятельности].[Вид-Группа-Деятельность]$$),$$[Вид Деятельности].[Вид-Группа-Деятельность]$$,$$[Вид Деятельности].[Вид-Группа-Деятельность]$$.DATAMEMBER))
//,Iif($$[Проект].[Проекты]$$.LEVEL_NUMBER=0,$$[Проект].[Проекты]$$.UNKNOWNMEMBER,Iif(IsLeaf($$[Проект].[Проекты]$$),$$[Проект].[Проекты]$$,$$[Проект].[Проекты]$$.DATAMEMBER))
//,Iif($$[Контрагент].[Контрагенты]$$.LEVEL_NUMBER=0,$$[Контрагент].[Контрагенты]$$.UNKNOWNMEMBER,Iif(IsLeaf($$[Контрагент].[Контрагенты]$$),$$[Контрагент].[Контрагенты]$$,$$[Контрагент].[Контрагенты]$$.DATAMEMBER))
//,Iif($$[Подразделение].[Подразделения]$$.LEVEL_NUMBER=0,$$[Подразделение].[Подразделения]$$.UNKNOWNMEMBER,Iif(IsLeaf($$[Подразделение].[Подразделения]$$),$$[Подразделение].[Подразделения]$$,$$[Подразделение].[Подразделения]$$.DATAMEMBER))
//,Iif($$[Договор].[Договоры]$$.LEVEL_NUMBER=0,$$[Договор].[Договоры]$$.UNKNOWNMEMBER,Iif(IsLeaf($$[Договор].[Договоры]$$),$$[Договор].[Договоры]$$,$$[Договор].[Договоры]$$.DATAMEMBER))
//,Iif($$[Номенклатура].[Вид-Группа-Номенклатура]$$.LEVEL_NUMBER=0,$$[Номенклатура].[Вид-Группа-Номенклатура]$$.UNKNOWNMEMBER,Iif(IsLeaf($$[Номенклатура].[Вид-Группа-Номенклатура]$$),$$[Номенклатура].[Вид-Группа-Номенклатура]$$,$$[Номенклатура].[Вид-Группа-Номенклатура]$$.DATAMEMBER))
//,Iif($$[Бизнес-процесс].[Бизнес-процессы]$$.LEVEL_NUMBER=0,$$[Бизнес-процесс].[Бизнес-процессы]$$.UNKNOWNMEMBER,Iif(IsLeaf($$[Бизнес-процесс].[Бизнес-процессы]$$),$$[Бизнес-процесс].[Бизнес-процессы]$$,$$[Бизнес-процесс].[Бизнес-процессы]$$.DATAMEMBER))
//,$$[Measures]$$
//)= $$newValue$$";

//            pivorGrid_query.Text = @"WITH
//MEMBER [Measures].[Лидеры] AS StrToMember('[Measures].[Internet Sales Amount]')
//SET [измерение] AS iif((1 = 5),StrToSet('[Source Currency].[Source Currency].[All Source Currencies]'),iif((1 = 1),StrToSet('[Customer].[Customer Geography].[All Customers]'),iif((1 = 2),StrToSet('[Product].[Product Categories].[All]'),iif((1 = 3),StrToSet('[Promotion].[Promotions].[All Promotions]'),iif((1 = 4),StrToSet('[Sales Territory].[Sales Territory].[All Sales Territories]'),StrToSet('[Source Currency].[Source Currency].[All Source Currencies]'))))))
//SET [PickData_XResult] AS order(Filter(iif((0 = 1),Descendants([измерение],iif((1 = 1),[Customer].[Customer Geography].[Customer],iif((1 = 2),[Product].[Product Categories].[Product],iif((1 = 3),[Promotion].[Promotions].[Type],iif((1 = 4),[Sales Territory].[Sales Territory].[Region],[Sales Reason].[Sales Reasons].[Sales Reason]))))),Descendants([измерение],1,LEAVES)),(not (Round([Measures].[Лидеры],2) = 0))),[Measures].[Лидеры],BDesc)
//SET [PickData_XTop] AS Iif((0 = 0),Head([PickData_XResult],15),Tail([PickData_XResult],15))
//SET [PickData_XOther] AS Iif((0 = 0),{},Except([PickData_XResult],[PickData_XTop]))
//MEMBER [Customer].[Customer Geography].[TOP-Other] AS Aggregate([PickData_XOther])
//MEMBER [Product].[Product Categories].[TOP-Other] AS Aggregate([PickData_XOther])
//MEMBER [Source Currency].[Source Currency].[TOP-Other] AS Aggregate([PickData_XOther])
//MEMBER [Promotion].[Promotions].[TOP-Other] AS Aggregate([PickData_XOther])
//MEMBER [Sales Territory].[Sales Territory].[TOP-Other] AS Aggregate([PickData_XOther])
//SET [TOP_Other] AS Iif((0 = 0),{},{iif((1 = 5),[Source Currency].[Source Currency].[TOP-Other],iif((1 = 1),[Customer].[Customer Geography].[TOP-Other],iif((1 = 2),[Product].[Product Categories].[TOP-Other],iif((1 = 3),[Promotion].[Promotions].[TOP-Other],[Sales Territory].[Sales Territory].[TOP-Other]))))})
//SET [PickDataVisible] AS Union([PickData_XTop],[TOP_Other])
//MEMBER [Measures].[Стало мера] AS SUM(STRTOSET('[Date].[Calendar].[Calendar Year].&[2002]'),StrToMember('[Measures].[Internet Sales Amount]')),FORMAT_STRING='### ### ### ### ###.00'
//MEMBER [Measures].[Было мера] AS SUM(STRTOSET('[Date].[Calendar].[Calendar Year].&[2003]'),StrToMember('[Measures].[Internet Sales Amount]')),FORMAT_STRING='### ### ### ### ###.00'
//MEMBER [Measures].[Абс мера] AS ([Measures].[Стало мера] - [Measures].[Было мера]),BACK_COLOR=Iif((([Measures].[Стало мера] - [Measures].[Было мера]) = Null),rgb(255,255,255),Iif((([Measures].[Стало мера] - [Measures].[Было мера]) >= 0),rgb(204,255,204),rgb(255,204,204))),FORMAT_STRING='### ### ### ### ###.00'
//MEMBER [Measures].[% отклонения] AS Iif((([Measures].[Стало мера] = 0) and ([Measures].[Было мера] = 0)),0,iif(([Measures].[Стало мера] = 0),(-100),(100 - (([Measures].[Было мера] / [Measures].[Стало мера]) * 100)))),BACK_COLOR=Iif((([Measures].[Стало мера] - [Measures].[Было мера]) = Null),rgb(255,255,255),Iif((([Measures].[Стало мера] - [Measures].[Было мера]) >= 0),rgb(204,255,204),rgb(255,204,204))),FORMAT_STRING='### ### ### ### ###.00'
//MEMBER [Measures].[Общий объем1] AS Sum([PickData_XTop],[Measures].[Было мера])
//MEMBER [Measures].[Общий объем2] AS Sum([PickData_XTop],[Measures].[Стало мера])
//MEMBER [Measures].[Мера, доля было] AS (([Measures].[Было мера] / [Measures].[Общий объем1]) * 100),FORMAT_STRING='### ### ### ### ###.00'
//MEMBER [Measures].[Мера, доля стало] AS (([Measures].[Стало мера] / [Measures].[Общий объем2]) * 100),FORMAT_STRING='### ### ### ### ###.00'
//MEMBER [Measures].[Мера, доля абс] AS ([Measures].[Мера, доля стало] - [Measures].[Мера, доля было]),BACK_COLOR=Iif((([Measures].[Мера, доля стало] - [Measures].[Мера, доля было]) = Null),rgb(255,255,255),Iif((([Measures].[Мера, доля стало] - [Measures].[Мера, доля было]) >= 0),rgb(204,255,204),rgb(255,204,204))),FORMAT_STRING='### ### ### ### ###.00'
//MEMBER [Measures].[Номер] AS iif((1 = 1),(Rank([Customer].[Customer Geography].CurrentMember,[PickData_XResult]) - 1),iif((1 = 2),(Rank([Product].[Category].CurrentMember,[PickData_XResult]) - 1),iif((1 = 3),(Rank([Promotion].[Promotions].CurrentMember,[PickData_XResult]) - 1),iif((1 = 4),(Rank([Sales Territory].[Sales Territory].CurrentMember,[PickData_XResult]) - 1),(Rank([Sales Reason].[Sales Reasons].CurrentMember,[PickData_XResult]) - 1)))))
//MEMBER [Measures].[Мера, нарас было] AS ([Measures].[Было мера] + Iif(([Measures].[Номер] = 0),0,([PickData_XTop].Item(([Measures].[Номер] - 1)).Item(0),[Measures].[Мера, нарас было]))),FORMAT_STRING='### ### ### ### ###.00'
//MEMBER [Measures].[Мера, нарас стало] AS ([Measures].[Стало мера] + Iif(([Measures].[Номер] = 0),0,([PickData_XTop].Item(([Measures].[Номер] - 1)).Item(0),[Measures].[Мера, нарас стало]))),FORMAT_STRING='### ### ### ### ###.00'
//MEMBER [Measures].[Мера, нарас абс] AS ([Measures].[Мера, нарас стало] - [Measures].[Мера, нарас было]),BACK_COLOR=Iif((([Measures].[Мера, нарас стало] - [Measures].[Мера, нарас было]) = Null),rgb(255,255,255),Iif((([Measures].[Мера, нарас стало] - [Measures].[Мера, нарас было]) >= 0),rgb(204,255,204),rgb(255,204,204))),FORMAT_STRING='### ### ### ### ###.00'
//SET [PickData_Элементы] AS IIF(('0' = '0'),[PickDataVisible],IIF(('0' = '1'),Filter([PickDataVisible],(([Measures].[Было мера] > 0) AND ([Measures].[Стало мера] > 0))),IIf(('0' = '2'),Filter([PickDataVisible],([Measures].[Стало мера] = 0)),Filter([PickDataVisible],([Measures].[Было мера] = 0)))))
//MEMBER [Date].[Fiscal].[Мера] AS Aggregate([Date].[Fiscal].[(All)])
//MEMBER [Date].[Fiscal].[Мера, доля] AS Aggregate([Date].[Fiscal].[(All)])
//MEMBER [Date].[Fiscal].[Мера, нарас] AS Aggregate([Date].[Fiscal].[(All)])
//MEMBER [Measures].[Было] AS iif(([Date].[Fiscal].CurrentMember  is  [Date].[Fiscal].[Мера, нарас]),[Measures].[Мера, нарас было],iif(([Date].[Fiscal].CurrentMember  is  [Date].[Fiscal].[Мера, доля]),[Measures].[Мера, доля было],[Measures].[Было мера])),FORMAT_STRING='### ### ### ### ###.00'
//MEMBER [Measures].[Стало] AS iif(([Date].[Fiscal].CurrentMember  is  [Date].[Fiscal].[Мера, нарас]),[Measures].[Мера, нарас стало],iif(([Date].[Fiscal].CurrentMember  is  [Date].[Fiscal].[Мера, доля]),[Measures].[Мера, доля стало],[Measures].[Стало мера])),FORMAT_STRING='### ### ### ### ###.00'
//MEMBER [Measures].[Абс] AS iif(([Date].[Fiscal].CurrentMember  is  [Date].[Fiscal].[Мера, нарас]),[Measures].[Мера, нарас абс],iif(([Date].[Fiscal].CurrentMember  is  [Date].[Fiscal].[Мера, доля]),[Measures].[Мера, доля абс],[Measures].[Абс мера])),BACK_COLOR=Iif((([Measures].[Стало] - [Measures].[Было]) = Null),rgb(255,255,255),Iif((([Measures].[Стало] - [Measures].[Было]) >= 0),rgb(204,255,204),rgb(255,204,204))),FORMAT_STRING='### ### ### ### ###.00'
//SET [Мер] AS {[Measures].[Стало],[Measures].[Было],[Measures].[Абс]}
//SET [Result] AS iif((0 = 0),[PickData_Элементы],iif((0 = 1),Filter([PickData_Элементы],([Measures].[Measures].[Абс мера] < 0)),Filter([PickData_Элементы],([Measures].[Абс мера] > 0))))
//SELECT
//{([Date].[Fiscal].[Мера] * {[Мер],[Measures].[% отклонения]}),([Date].[Fiscal].[Мера, доля] * [Мер]),([Date].[Fiscal].[Мера, нарас] * [Мер])} ON 0,
//NON EMPTY HIERARCHIZE(DRILLDOWNMEMBER(DRILLDOWNMEMBER(DRILLDOWNMEMBER(DRILLDOWNMEMBER(DRILLDOWNMEMBER(DRILLDOWNMEMBER(DRILLDOWNMEMBER(DRILLDOWNMEMBER(DRILLDOWNMEMBER(DRILLDOWNMEMBER(DRILLDOWNMEMBER({[Result]},[Customer].[Customer Geography].[Country].&[Australia]),[Customer].[Customer Geography].[State-Province].&[NSW]&[AU]),[Customer].[Customer Geography].[City].&[Goulburn]&[NSW]),[Customer].[Customer Geography].[City].&[Matraville]&[NSW]),[Customer].[Customer Geography].[Country].&[France]),[Customer].[Customer Geography].[State-Province].&[45]&[FR]),[Customer].[Customer Geography].[Country].&[Canada]),[Customer].[Customer Geography].[State-Province].&[AB]&[CA]),[Customer].[Customer Geography].[State-Province].&[BC]&[CA]),[Customer].[Customer Geography].[City].&[Metchosin]&[BC]),[Customer].[Customer Geography].[City].&[Newton]&[BC])) DIMENSION PROPERTIES PARENT_UNIQUE_NAME ON 1
//FROM (
//SELECT
//{(StrToSet('[Sales Reason].[Sales Reasons].[All Sales Reasons]'),StrToSet('[Customer].[Customer Geography].[All Customers]'),StrToSet('[Product].[Product Categories].[All]'),StrToSet('[Promotion].[Promotions].[All Promotions]'),StrToSet('[Sales Territory].[Sales Territory].[All Sales Territories]'),StrToSet('[Source Currency].[Source Currency].[All Source Currencies]'),{StrToSet('[Date].[Calendar].[Calendar Year].&[2002]'),StrToSet('[Date].[Calendar].[Calendar Year].&[2003]')})} ON 0
//FROM [Adventure Works]
//)
//CELL PROPERTIES BACK_COLOR,CELL_ORDINAL,FORE_COLOR,FONT_NAME,FONT_SIZE,FONT_FLAGS,FORMAT_STRING,VALUE,FORMATTED_VALUE";

//            pivorGrid_query.Text = @"SELECT
//
//CrossJoin({[Период].[ГКМД].Levels(0).Members},{[Сценарий].[Сценарий].Levels(0).Members}) DIMENSION PROPERTIES PARENT_UNIQUE_NAME,HIERARCHY_UNIQUE_NAME ON 0,
//
//Hierarchize({[Контрагент].[Страна-Город-Контрагент].Levels(0).Members}) DIMENSION PROPERTIES PARENT_UNIQUE_NAME,HIERARCHY_UNIQUE_NAME ON 1
//
//FROM [Товарные и финансовые потоки]
//
//WHERE 
//
//{[Measures].[Сумма]}
//
//CELL PROPERTIES BACK_COLOR,CELL_ORDINAL,FORE_COLOR,FONT_NAME,FONT_SIZE,FONT_FLAGS,FORMAT_STRING,VALUE,FORMATTED_VALUE,UPDATEABLE
//";

//            pivorGrid_query.Text = @"SELECT
//Hierarchize(CrossJoin({[Период].[ГКМД].[Год].&[2009]},{[Сценарий].[Сценарий].Levels(0).Members})) DIMENSION PROPERTIES PARENT_UNIQUE_NAME,HIERARCHY_UNIQUE_NAME ON 0,
//Hierarchize({[Контрагент].[Страна-Город-Контрагент].Levels(0).Members}) DIMENSION PROPERTIES PARENT_UNIQUE_NAME,HIERARCHY_UNIQUE_NAME ON 1
//FROM [Товарные и финансовые потоки]
//WHERE 
//{[Measures].[Сумма]}
//CELL PROPERTIES BACK_COLOR,CELL_ORDINAL,FORE_COLOR,FONT_NAME,FONT_SIZE,FONT_FLAGS,FORMAT_STRING,VALUE,FORMATTED_VALUE,UPDATEABLE
//";


//            pivorGrid_query.Text = @"with
//member [Measures].[АТИ] as Iif( [Measures].[АТИ по ОИ] > [Measures].[АТИ по ИФ], [Measures].[АТИ по ОИ], [Measures].[АТИ по ИФ])
//,BACK_COLOR = iif(not([Measures].[АТИ по ОИ] = Null ) and not([Measures].[АТИ по ИФ] = Null) and ([Measures].[АТИ по ОИ] <> [Measures].[АТИ по ИФ])
//, RGB(255, 128, 128), RGB(255, 255, 255))
//select
//non empty{StrToSet('[Показатель].[Показатель].[Все показатели]') * StrToSet('[Вид затрат].[Вид затрат].[Все виды затрат]')* {[Measures].[АТИ по ОИ], [Measures].[АТИ по ИФ], [Measures].[АТИ]} } Dimension Properties PARENT_UNIQUE_NAME  on 0,
//non empty{
//{[Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций],[Объект инвестиций].[Объект инвестиций].[Все объекты инвестиций].Children}
//*
//{[Источник финансирования].[Источник финансирования].[Все источники финансирования],[Источник финансирования].[Источник финансирования].[Все источники финансирования].Children}
//*
//{[Вид цен].[Вид цен].&[баз],[Вид цен].[Вид цен].&[тек]}} Dimension Properties PARENT_UNIQUE_NAME  on 1
//from [GalaktikaIPMS]
//CELL PROPERTIES BACK_COLOR, CELL_ORDINAL, FORE_COLOR, FONT_NAME, FONT_SIZE, FONT_FLAGS, FORMAT_STRING, VALUE, FORMATTED_VALUE";
              

//            pivorGrid_query.Text = @"with 
//
// 
//
//member [Сценарий].[Сценарий].[% выполнения плана] as iif(IsNull([Сценарий].[Сценарий].[Факт]) or
//
//(IsNull([Сценарий].[Сценарий].[план])) or ([Сценарий].[Сценарий].[план] = 0),
//
//NULL,[Сценарий].[Сценарий].[Факт] / [Сценарий].[Сценарий].[план] )
//
//,FORMAT_STRING = 'Percent'
//
//,BACK_COLOR = iif(IsNull([Сценарий].[Сценарий].[план]) or ([Сценарий].[Сценарий].[план] = 0)
//
//                , RGB(255, 255, 255), // белый
//
//                iif([Сценарий].[Сценарий].[% выполнения плана] < 0.75,
//
//                               RGB(255, 128, 128), // красный
//
//                               iif([Сценарий].[Сценарий].[% выполнения плана] < 0.95,
//
//                                               RGB(255, 255, 153), // желтый,
//
//                                               RGB(204, 255, 204)  // зеленый
//
//                               )
//
//                )
//
//)
//
// 
//
//select 
//
//{ {StrToSet('[Сценарий].[Сценарий].[план]'),[Сценарий].[Сценарий].[% выполнения плана]} * {[Measures].[Сумма],[Measures].[Количество]} } Dimension Properties PARENT_UNIQUE_NAME on 0,
//
// 
//
//{ HIERARCHIZE(StrToSet('[Номенклатура].[Тип-Группа-Номенклатура].[Вся номенклатура]')) } Dimension Properties PARENT_UNIQUE_NAME on 1
//
//from 
//
//(select 
//
//{(
//
//  StrToMember('[Потоки].[Потоки].[Товарные]')
//
//, StrToMember('[Направление].[Направление].[Прямое]')
//
//, StrToMember('[Валюта представления].[Валюта].&[0]')
//
//, StrToMember('[Вид деятельности].[Вид деятельности].[Продажи]')
//
//)} on 0 from [Товарные и финансовые потоки]
//
//)
//
//where
//
//(
//
// [Показатель].[Показатель].[выбранный]
//
//, [Учет отклонений].[Учет отклонений].&[без учета отклонений]
//
//, StrToMember('[Валюта представления].[Валюта].&[0]')
//
//, StrToMember('[Потоки].[Потоки].[Товарные]')
//
//, StrToMember('[Направление].[Направление].[Прямое]')
//
//, StrToMember('[Вид деятельности].[Вид деятельности].[Продажи]')
//
//)
//
// 
//
//CELL PROPERTIES 
//
//BACK_COLOR, CELL_ORDINAL, FORE_COLOR, FONT_NAME, FONT_SIZE, FONT_FLAGS, FORMAT_STRING, VALUE, FORMATTED_VALUE, UPDATEABLE
//";
 


//            pivorGrid_query.Text = @"WITH
//
//SET [PickData_X] AS iif((2=1),StrToSet('[Период].[ГКМД].[Все периоды]'),iif((2=2),StrToSet('[Номенклатура].[Тип-Группа-Номенклатура].[Вся номенклатура]'),iif((2=4),(StrToSet('[Номенклатура].[Тип-Группа-Номенклатура].[Вся номенклатура]')*StrToSet('[Единица измерения].[Единица измерения].[Все единицы измерения]')),iif((2=8),StrToSet('[Бренд].[Бренд].[Все товары]'),iif((2=16),StrToSet('[Контрагент].[Страна-Город-Контрагент].[Все контрагенты]'),iif((2=32),StrToSet('[Категория контрагента].[Категория контрагента].[Все категории контрагентов]'),iif((2=64),StrToSet('[Канал сбыта].[Канал сбыта].[Все каналы сбыта]'),iif((2=128),StrToSet('[Договор].[Договор].[Все договоры]'),iif((2=256),StrToSet('[Центр ответственности].[Центр ответственности].[Все центры ответственности]'),StrToSet('[Номенклатура].[Тип-Группа-Номенклатура].[Вся номенклатура]'))))))))))
//
//MEMBER [Сценарий].[Сценарий].[% выполнения плана] AS iif(((IsNull([Сценарий].[Сценарий].[Факт])or([Сценарий].[Сценарий].[план ручной]=NULL))or([Сценарий].[Сценарий].[план ручной]=0)),NULL,([Сценарий].[Сценарий].[Факт]/[Сценарий].[Сценарий].[план ручной])),FORMAT_STRING='Percent',BACK_COLOR=iif((IsNull(iif(({{Sale.Variables.Selectors._isPlanWrite}}=1),StrToMember('{{Sale.Variables.Filter_MDX.mdxСценарий}}'),[Сценарий].[Сценарий].[План]))or(iif(({{Sale.Variables.Selectors._isPlanWrite}}=1),StrToMember('{{Sale.Variables.Filter_MDX.mdxСценарий}}'),[Сценарий].[Сценарий].[План])=0)),RGB(255,255,255),iif(([Сценарий].[Сценарий].[% выполнения плана]<0.75),RGB(255,128,128),iif(([Сценарий].[Сценарий].[% выполнения плана]<0.95),RGB(255,255,153),RGB(204,255,204))))
//
//MEMBER [Сценарий].[Сценарий].[Перевыполнено] AS iif(IsNull([Сценарий].[Сценарий].[План]),NULL,iif((([Сценарий].[Сценарий].[Факт]-[Сценарий].[Сценарий].[план ручной])>0),([Сценарий].[Сценарий].[Факт]-[Сценарий].[Сценарий].[план ручной]),NULL))
//
//MEMBER [Сценарий].[Сценарий].[Невыполнено] AS iif(IsNull([Сценарий].[Сценарий].[план ручной]),NULL,iif((([Сценарий].[Сценарий].[план ручной]-[Сценарий].[Сценарий].[Факт])>0),([Сценарий].[Сценарий].[план ручной]-[Сценарий].[Сценарий].[Факт]),NULL)),BACK_COLOR=iif((IsNull(iif(({{Sale.Variables.Selectors._isPlanWrite}}=1),StrToMember('{{Sale.Variables.Filter_MDX.mdxСценарий}}'),[Сценарий].[Сценарий].[План]))or(iif(({{Sale.Variables.Selectors._isPlanWrite}}=1),StrToMember('{{Sale.Variables.Filter_MDX.mdxСценарий}}'),[Сценарий].[Сценарий].[План])=0)),RGB(255,255,255),iif(([Сценарий].[Сценарий].[% выполнения плана]<0.75),RGB(255,128,128),iif(([Сценарий].[Сценарий].[% выполнения плана]<0.95),RGB(255,255,153),RGB(204,255,204))))
//
//SET [PickData_Scenario] AS {StrToSet('[Сценарий].[Сценарий].[план ручной]'),iif((Intersect([Сценарий].[Сценарий].[план ручной],StrToSet('[Сценарий].[Сценарий].[план ручной]')).Count<>0),{[Сценарий].[Сценарий].[% выполнения плана],[Сценарий].[Сценарий].[Перевыполнено],[Сценарий].[Сценарий].[Невыполнено]},{})}
//
//SELECT
//
//{([PickData_Scenario]*{[Measures].[Сумма],[Measures].[Количество]})} DIMENSION PROPERTIES PARENT_UNIQUE_NAME ON 0,
//
//HIERARCHIZE(DRILLDOWNMEMBER(DRILLDOWNMEMBER({HIERARCHIZE([PickData_X])},[Номенклатура].[Тип-Группа-Номенклатура].[Вся номенклатура]),[Номенклатура].[Тип-Группа-Номенклатура].&[T.1])) DIMENSION PROPERTIES PARENT_UNIQUE_NAME ON 1
//
//FROM (
//
//SELECT
//
//{(StrToSet('[Период].[ГКМД].[Все периоды]'),StrToSet('[Номенклатура].[Тип-Группа-Номенклатура].[Вся номенклатура]'),StrToSet('[Единица измерения].[Единица измерения].[Все единицы измерения]'),StrToSet('[Бренд].[Бренд].[Все товары]'),StrToSet('[Контрагент].[Страна-Город-Контрагент].[Все контрагенты]'),StrToSet('[Категория контрагента].[Категория контрагента].[Все категории контрагентов]'),StrToSet('[Канал сбыта].[Канал сбыта].[Все каналы сбыта]'),StrToSet('[Договор].[Договор].[Все договоры]'),StrToSet('[Центр ответственности].[Центр ответственности].[Все центры ответственности]'),StrToMember('[Организационная единица].[Организационная единица].[Все организационные единицы]'),StrToMember('[Потоки].[Потоки].[Товарные]'),StrToMember('[Направление].[Направление].[Прямое]'),StrToMember('[Валюта представления].[Валюта].&[0]'),StrToMember('[Вид деятельности].[Вид деятельности].[Продажи]'))} ON 0
//
//FROM [Товарные и финансовые потоки]
//
//)
//
//WHERE 
//
//([Показатель].[Показатель].[выбранный],[Учет отклонений].[Учет отклонений].&[без учета отклонений],StrToMember('[Валюта представления].[Валюта].&[0]'),StrToMember('[Потоки].[Потоки].[Товарные]'),StrToMember('[Направление].[Направление].[Прямое]'),StrToMember('[Вид деятельности].[Вид деятельности].[Продажи]'))
//
//CELL PROPERTIES BACK_COLOR,CELL_ORDINAL,FORE_COLOR,FONT_NAME,FONT_SIZE,FONT_FLAGS,FORMAT_STRING,VALUE,FORMATTED_VALUE,UPDATEABLE
//
// 
//
//";

//            pivorGrid_query.Text = @"WITH
//
//SET [Period] AS StrToSet('[Период].[ГКМД].[Все периоды]')
//
//SET [Center] AS StrToSet('[Центр ответственности].[Центр ответственности].[Все центры ответственности]')
//
//SET [Nomenclature] AS StrToSet('[Номенклатура].[Тип-Группа-Номенклатура].[Вся номенклатура]')
//
//SET [Contragent] AS StrToSet('[Контрагент].[Страна-Город-Контрагент].[Все контрагенты]')
//
//SET [Dogovor] AS StrToSet('[Договор].[Договор].[Все договоры]')
//
//SELECT
//
//{[Measures].[Сумма],[Measures].[Количество]} ON 0,
//
//HIERARCHIZE(DRILLUPMEMBER(DRILLUPMEMBER(DRILLUPMEMBER(DRILLUPMEMBER(DRILLUPMEMBER({HIERARCHIZE(((((StrToSet('[Период].[ГКМД].[Все периоды]')*StrToSet('[Центр ответственности].[Центр ответственности].[Все центры ответственности]'))*StrToSet('[Номенклатура].[Тип-Группа-Номенклатура].[Вся номенклатура]'))*StrToSet('[Контрагент].[Страна-Город-Контрагент].[Все контрагенты]'))*StrToSet('[Договор].[Договор].[Все договоры]')))},[Период].[ГКМД].[Все периоды]),[Центр ответственности].[Центр ответственности].[Все центры ответственности]),[Номенклатура].[Тип-Группа-Номенклатура].[Вся номенклатура]),[Контрагент].[Страна-Город-Контрагент].[Все контрагенты]),[Договор].[Договор].[Все договоры])) DIMENSION PROPERTIES PARENT_UNIQUE_NAME ON 1
//
//FROM [Товарные и финансовые потоки]
//
//WHERE 
//
//(StrToSet('[Организационная единица].[Организационная единица].[Все организационные единицы]'),[Сценарий].[Сценарий].[план],[Учет отклонений].[Учет отклонений].&[без учета отклонений],StrToSet('[Потоки].[Потоки].[Товарные]'),[Показатель].[Показатель].[выбранный],StrToSet('[Направление].[Направление].[Прямое]'),StrToMember('[Валюта представления].[Валюта].&[0]'),StrToSet('[Вид деятельности].[Вид деятельности].[Продажи]'))
//
//CELL PROPERTIES BACK_COLOR,CELL_ORDINAL,FORE_COLOR,FONT_NAME,FONT_SIZE,FONT_FLAGS,FORMAT_STRING,VALUE,FORMATTED_VALUE,UPDATEABLE
//";

//            pivorGrid_query.Text = @"SELECT
//HIERARCHIZE(Crossjoin(
//{StrToSet('[Период].[ГКМ].[Год].&[2008]')} 
//,StrToSet('{{[Сценарий].[Сценарии].&[План]},{[Сценарий].[Сценарии].&[Факт]},{[Сценарий].[Сценарии].[Все сценарии].[% выполнения]}}')))
//Dimension Properties PARENT_UNIQUE_NAME on 0
//,HIERARCHIZE({StrToSet('[Персона].[Персонал].[Весь персонал]')}) 
//Dimension Properties PARENT_UNIQUE_NAME on 1
//,{[Measures].[Сумма]} on 2
//,StrToSet('[Статья].[Статьи].&[и_Ресурсы_Загрузка]') on 3
//,StrToSet('[ЦФО].[Менеджмент].&[У-5]') on 4
//,StrToSet('[Контрагент].[Контрагенты].[Все контрагенты]') on 5
//,StrToSet('[Проект].[Проекты].[Все проекты]') on 6
//,StrToSet('[Договор].[Договоры].[Все договоры]') on 7
//,StrToSet('[Подразделение].[Подразделения].[Все подразделения]') on 8
//,StrToSet('[Номенклатура].[Вид-Группа-Номенклатура].[Вся номенклатура]') on 9
//,StrToSet('[Бизнес-процесс].[Бизнес-процессы].[Все бизнес-процессы]') on 10
//,StrToSet('[Вид Деятельности].[Вид-Группа-Деятельность].[Вид].&[Технологические работы]') on 11
//FROM
//[Бюджет]
//CELL PROPERTIES 
//BACK_COLOR, CELL_ORDINAL, FORE_COLOR, FONT_NAME, FONT_SIZE, FONT_FLAGS, FORMAT_STRING, VALUE, FORMATTED_VALUE, UPDATEABLE
//";
            
//            pivorGrid_updateScript.Text = @"UPDATE CUBE [Бюджет]
//SET 
//(
//
// iif(IsLeaf($$[Сценарий].[Сценарии]$$),$$[Сценарий].[Сценарии]$$,$$[Сценарий].[Сценарии]$$.DATAMEMBER)
//
//,iif(IsLeaf($$[Статья].[Статьи]$$),$$[Статья].[Статьи]$$,$$[Статья].[Статьи]$$.DATAMEMBER)
//
//,DESCENDANTS(LinkMember($$[Период].[ГКМ]$$,[Период].[ГКМД]),,LEAVES).Item(0)
//
//,$$[Measures]$$
//
//)= $$newValue$$ - $$oldValue$$";
            pivorGrid_updateScript.Text = @"UPDATE CUBE [Бюджет]
SET 
(
 Iif($$[Статья].[Статьи]$$.LEVEL_NUMBER=0,$$[Статья].[Статьи]$$.UNKNOWNMEMBER,Iif(IsLeaf($$[Статья].[Статьи]$$),$$[Статья].[Статьи]$$,$$[Статья].[Статьи]$$.DATAMEMBER))
,iif(IsLeaf($$[Сценарий].[Сценарии]$$),$$[Сценарий].[Сценарии]$$,$$[Сценарий].[Сценарии]$$.DATAMEMBER)
,Iif($$[ЦФО].[Менеджмент]$$.LEVEL_NUMBER=0,$$[ЦФО].[Менеджмент]$$.UNKNOWNMEMBER,Iif(IsLeaf($$[ЦФО].[Менеджмент]$$),$$[ЦФО].[Менеджмент]$$,$$[ЦФО].[Менеджмент]$$.DATAMEMBER))
,Iif($$[Персона].[Персонал]$$.LEVEL_NUMBER=0,$$[Персона].[Персонал]$$.UNKNOWNMEMBER,Iif(IsLeaf($$[Персона].[Персонал]$$),$$[Персона].[Персонал]$$,$$[Персона].[Персонал]$$.DATAMEMBER))
,DESCENDANTS(LinkMember($$[Период].[ГКМ]$$,[Период].[ГКМД]),,LEAVES).Item(0)
,Iif($$[Вид Деятельности].[Вид-Группа-Деятельность]$$.LEVEL_NUMBER=0,$$[Вид Деятельности].[Вид-Группа-Деятельность]$$.UNKNOWNMEMBER,Iif(IsLeaf($$[Вид Деятельности].[Вид-Группа-Деятельность]$$),$$[Вид Деятельности].[Вид-Группа-Деятельность]$$,$$[Вид Деятельности].[Вид-Группа-Деятельность]$$.DATAMEMBER))
,Iif($$[Проект].[Проекты]$$.LEVEL_NUMBER=0,$$[Проект].[Проекты]$$.UNKNOWNMEMBER,Iif(IsLeaf($$[Проект].[Проекты]$$),$$[Проект].[Проекты]$$,$$[Проект].[Проекты]$$.DATAMEMBER))
,Iif($$[Контрагент].[Контрагенты]$$.LEVEL_NUMBER=0,$$[Контрагент].[Контрагенты]$$.UNKNOWNMEMBER,Iif(IsLeaf($$[Контрагент].[Контрагенты]$$),$$[Контрагент].[Контрагенты]$$,$$[Контрагент].[Контрагенты]$$.DATAMEMBER))
,Iif($$[Подразделение].[Подразделения]$$.LEVEL_NUMBER=0,$$[Подразделение].[Подразделения]$$.UNKNOWNMEMBER,Iif(IsLeaf($$[Подразделение].[Подразделения]$$),$$[Подразделение].[Подразделения]$$,$$[Подразделение].[Подразделения]$$.DATAMEMBER))
,Iif($$[Договор].[Договоры]$$.LEVEL_NUMBER=0,$$[Договор].[Договоры]$$.UNKNOWNMEMBER,Iif(IsLeaf($$[Договор].[Договоры]$$),$$[Договор].[Договоры]$$,$$[Договор].[Договоры]$$.DATAMEMBER))
,Iif($$[Номенклатура].[Вид-Группа-Номенклатура]$$.LEVEL_NUMBER=0,$$[Номенклатура].[Вид-Группа-Номенклатура]$$.UNKNOWNMEMBER,Iif(IsLeaf($$[Номенклатура].[Вид-Группа-Номенклатура]$$),$$[Номенклатура].[Вид-Группа-Номенклатура]$$,$$[Номенклатура].[Вид-Группа-Номенклатура]$$.DATAMEMBER))
,Iif($$[Бизнес-процесс].[Бизнес-процессы]$$.LEVEL_NUMBER=0,$$[Бизнес-процесс].[Бизнес-процессы]$$.UNKNOWNMEMBER,Iif(IsLeaf($$[Бизнес-процесс].[Бизнес-процессы]$$),$$[Бизнес-процесс].[Бизнес-процессы]$$,$$[Бизнес-процесс].[Бизнес-процессы]$$.DATAMEMBER))
,$$[Measures]$$
)= $$newValue$$";
            
//            pivorGrid_connectionString.Text = @"Provider=MSOLAP.3;Data Source=DPP-PETR\SQL2008;Integrated Security=SSPI;Initial Catalog=BI";
//            pivorGrid_query.Text = @"SELECT
//
//HIERARCHIZE(Crossjoin({[Период].[ГКМД].[Все периоды].Children},{[Сценарий].[Сценарий].[план],[Сценарий].[Сценарий].[факт],[Сценарий].[Сценарий].[% выполнения]})) DIMENSION PROPERTIES PARENT_UNIQUE_NAME ON 0,
//
//NON EMPTY HIERARCHIZE({[Контрагент].[Страна-Город-Контрагент].[Контрагент].Members}) DIMENSION PROPERTIES PARENT_UNIQUE_NAME ON 1
//
//FROM [Товарные и финансовые потоки]
//
//WHERE 
//
//({[Measures].[Сумма]},{[Валюта представления].[Валюта].&[2]},{[Вид деятельности].[Вид деятельности].[Продажи]},{[Потоки].[Потоки].[Товарные]})
//
//CELL PROPERTIES BACK_COLOR,CELL_ORDINAL,FORE_COLOR,FONT_NAME,FONT_SIZE,FONT_FLAGS,FORMAT_STRING,VALUE,FORMATTED_VALUE,UPDATEABLE";

 

 

            /*pivorGrid_query.Text = @"SELECT

HIERARCHIZE({Crossjoin({Descendants([Период].[ГКМ].[Год].&[2008],[Период].[ГКМ].[Месяц])},StrToSet('{{[Сценарий].[Сценарии].[Все сценарии].[% выполнения]}, {[Сценарий].[Сценарии].&[Факт]}, {[Сценарий].[Сценарии].&[План]}}'))}) DIMENSION PROPERTIES PARENT_UNIQUE_NAME ON 0,

HIERARCHIZE(StrToSet('{{[Статья].[Статьи].&[НормЗимн]}, {[Статья].[Статьи].&[ПробГод]}, {[Статья].[Статьи].&[АТП].DATAMEMBER}, {[Статья].[Статьи].&[КолЧас]}, {[Статья].[Статьи].&[ЦенГСМ]}, {[Статья].[Статьи].&[НормЛетн]}, {[Статья].[Статьи].&[СтГСМ]}, {[Статья].[Статьи].&[ПотрГСМ]}, {[Статья].[Статьи].&[НормЧас]}}')) DIMENSION PROPERTIES PARENT_UNIQUE_NAME ON 1,
[Measures] on 2

FROM [Бюджет]

CELL PROPERTIES BACK_COLOR,CELL_ORDINAL,FORE_COLOR,FONT_NAME,FONT_SIZE,FONT_FLAGS,FORMAT_STRING,VALUE,FORMATTED_VALUE,UPDATEABLE";

            pivorGrid_updateScript.Text = @"UPDATE CUBE [Бюджет]
SET 
(

 iif(IsLeaf($$[Сценарий].[Сценарии]$$),$$[Сценарий].[Сценарии]$$,$$[Сценарий].[Сценарии]$$.DATAMEMBER)

,iif(IsLeaf($$[Статья].[Статьи]$$),$$[Статья].[Статьи]$$,$$[Статья].[Статьи]$$.DATAMEMBER)

,DESCENDANTS(LinkMember($$[Период].[ГКМ]$$,[Период].[ГКМД]),,LEAVES).Item(0)

,$$[Measures]$$

)= $$newValue$$";*/

            //pl.MouseEnter += new MouseEventHandler(pl_MouseEnter);
            //pl.MouseLeave += new MouseEventHandler(pl_MouseLeave);

            date_connectionString.Text = @"Provider=MSOLAP.3;Data Source=dpp-petr\sql2008;Integrated Security=SSPI;Initial Catalog=BPM_Topsoft";
            date_cubeName.Text = "[Бюджет]";
            date_dateLevelUniqueName.Text = "[Период].[ГКМД].[День]";

            mdxDesigner_subCube.Text = @"Select {[Date].[Calendar].[Calendar Year].&[2002], [Date].[Calendar].[Calendar Year].&[2003]} on 0 from [Adventure Works]";
        }

        void pl_MouseLeave(object sender, MouseEventArgs e)
        {
            HtmlPage.Document.DetachEvent("oncontextmenu", new EventHandler<HtmlEventArgs>(ContentMenu_EventHandler));            
        }

        void pl_MouseEnter(object sender, MouseEventArgs e)
        {
            HtmlPage.Document.AttachEvent("oncontextmenu", new EventHandler<HtmlEventArgs>(ContentMenu_EventHandler));
        }

        void ContentMenu_EventHandler(object sender, HtmlEventArgs e)
        {
            e.PreventDefault();
            e.StopPropagation();
        }

        private void getKPIsButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeWebService.InitializeWebServiceSoapClient service = new UILibrary.Olap.UITestApplication.InitializeWebService.InitializeWebServiceSoapClient();
            service.InitConnectionStringAsync("kpiChoiceControl", kpi_connectionString.Text);
            service.InitConnectionStringCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(kpiChoice_InitConnectionStringCompleted);
        }

        void kpiChoice_InitConnectionStringCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                kpiChoiceControl.URL = "http://localhost:1688/OlapWebService.asmx";
                kpiChoiceControl.AConnection = "kpiChoiceControl";
                kpiChoiceControl.ACubeName = kpi_cubeName.Text;
            }
            else
            {
                MessageBox.Show(e.Error.ToString());
            }  
        }

        private void getMeasuresButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeWebService.InitializeWebServiceSoapClient service = new UILibrary.Olap.UITestApplication.InitializeWebService.InitializeWebServiceSoapClient();
            service.InitConnectionStringAsync("measureChoiceControl", measure_connectionString.Text);
            service.InitConnectionStringCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(measureChoice_InitConnectionStringCompleted);
        }

        void measureChoice_InitConnectionStringCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                measureChoiceControl.URL = "http://localhost:1688/OlapWebService.asmx";
                measureChoiceControl.AConnection = "measureChoiceControl";
                measureChoiceControl.ACubeName = measure_cubeName.Text;
            }
            else
            {
                MessageBox.Show(e.Error.ToString());
            }
        }

        private void getLevelsButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeWebService.InitializeWebServiceSoapClient service = new UILibrary.Olap.UITestApplication.InitializeWebService.InitializeWebServiceSoapClient();
            service.InitConnectionStringAsync("levelChoiceControl", level_connectionString.Text);
            service.InitConnectionStringCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(levelChoice_InitConnectionStringCompleted);
        }

        void levelChoice_InitConnectionStringCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                levelChoiceControl.URL = "http://localhost:1688/OlapWebService.asmx";
                levelChoiceControl.AConnection = "levelChoiceControl";
                levelChoiceControl.ACubeName = level_cubeName.Text;
                levelChoiceControl.ADimensionUniqueName = level_dimensionName.Text;
                levelChoiceControl.AHierarchyUniqueName = level_hierarchyName.Text;
            }
            else
            {
                MessageBox.Show(e.Error.ToString());
            }
        }

        //string ExpandString(byte[] buffer)
        //{
        //    // turn buffer into a memory stream
        //    MemoryStream ms = new MemoryStream(buffer);

        //    // attach decompressor stream to memory stream
        //    C1.C1Zip.C1ZStreamReader sr = new C1.C1Zip.C1ZStreamReader(ms);

        //    // read uncompressed data
        //    StreamReader reader = new StreamReader(sr);
        //    return reader.ReadToEnd();
        //}

        //byte[] CompressString(string str)
        //{
        //    // open memory stream
        //    MemoryStream ms = new MemoryStream();

        //    // attach compressor stream to memory stream
        //    C1.C1Zip.C1ZStreamWriter sw = new C1.C1Zip.C1ZStreamWriter(ms);

        //    // write data into compressor stream
        //    StreamWriter writer = new StreamWriter(sw);
        //    writer.Write(str);

        //    // flush any pending data
        //    writer.Flush();

        //    // return the memory buffer
        //    return ms.ToArray();
        //}

        private void getMembersButton_Click(object sender, RoutedEventArgs e)
        {
            //String input_str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<InvokeResultDescriptor xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Content />\r\n  <ContentType>MultidimData</ContentType>\r\n</InvokeResultDescriptor>";
            //byte[] array_zipped_out = ZipCompressor.CompressString(input_str);
            //String zipped_str = Convert.ToBase64String(array_zipped_out);//Encoding.ASCII.GetString(a);

            //byte[] array_zipped_in = Convert.FromBase64String(zipped_str);
            //String output_str = ZipCompressor.ExpandString(array_zipped_in);

            //memberChoiceControl.ASelectLeafs = true;
            memberChoiceControl.AMultiSelect = true;
            InitializeWebService.InitializeWebServiceSoapClient service = new UILibrary.Olap.UITestApplication.InitializeWebService.InitializeWebServiceSoapClient();
            service.InitConnectionStringCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(memberChoice_InitConnectionStringCompleted);
            service.InitConnectionStringAsync("memberChoiceControl", member_connectionString.Text);
        }

        void memberChoice_InitConnectionStringCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                //memberChoiceControl.URL = "http://localhost:1688/OlapWebService.asmx";
                memberChoiceControl.AConnection = "memberChoiceControl";
                memberChoiceControl.ACubeName = member_cubeName.Text;
                memberChoiceControl.AHierarchyName = member_hierarchyName.Text;
                memberChoiceControl.AUseStepLoading = true;
                memberChoiceControl.AStep = 5;
                memberChoiceControl.AMultiSelect = true;
                //memberChoiceControl.AStartLevelUniqueName = "[Product].[Product Categories].[Subcategory]";
                //memberChoiceControl.AShowOnlyFirstLevelMembers = true;

                memberChoice.Connection = "memberChoiceControl";
                memberChoice.CubeName = member_cubeName.Text;
                memberChoice.HierarchyUniqueName = member_hierarchyName.Text;
                memberChoice.UseStepLoading = true;
                memberChoice.Step = 5;
                memberChoice.MultiSelect = true;
                memberChoice.Initialize();
            }
            else
            {
                MessageBox.Show(e.Error.ToString());
            }
        }

        private void initPivotGridButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeWebService.InitializeWebServiceSoapClient service = new UILibrary.Olap.UITestApplication.InitializeWebService.InitializeWebServiceSoapClient();
            service.Endpoint.Address = new System.ServiceModel.EndpointAddress(new Uri("http://localhost:1688/InitializeWebService.asmx"));
            service.InitConnectionStringCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(pivotGrid_InitConnectionStringCompleted);
            service.InitConnectionStringAsync("pivotGridControl", pivorGrid_connectionString.Text);
        }

        void pivotGrid_InitConnectionStringCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                pivotGridControl.URL = "http://localhost:1688/OlapWebService.asmx";
                pivotGridControl.Connection = "pivotGridControl";
                pivotGridControl.Query = pivorGrid_query.Text;
                pivotGridControl.UpdateScript = pivorGrid_updateScript.Text;
                pivotGridControl.IsUpdateable = true;
                pivotGridControl.ColumnsIsInteractive = false;
                //pivotGridControl.UseColumnsAreaHint = false;
                //pivotGridControl.UseRowsAreaHint = false;
                //pivotGridControl.UseCellsAreaHint = false;
                //pivotGridControl.MemberVisualizationType = Ranet.Olap.Core.Data.MemberVisualizationTypes.KeyAndCaption;
                pivotGridControl.Initialize();
            }
            else {
                MessageBox.Show(e.Error.ToString());
            }
        }

        private void initCubeChoiceButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeWebService.InitializeWebServiceSoapClient service = new UILibrary.Olap.UITestApplication.InitializeWebService.InitializeWebServiceSoapClient();
            service.InitConnectionStringAsync("cubeChoiceControl", cube_connectionString.Text);
            service.InitConnectionStringCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(cubeChoice_InitConnectionStringCompleted);
        }

        void cubeChoice_InitConnectionStringCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                cubeChoiceControl.URL = "http://localhost:1688/OlapWebService.asmx";
                cubeChoiceControl.Connection = "cubeChoiceControl";
                cubeChoiceControl.Initialize();
            }
            else
            {
                MessageBox.Show(e.Error.ToString());
            }  
        }

        private void initDimensionChoiceButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeWebService.InitializeWebServiceSoapClient service = new UILibrary.Olap.UITestApplication.InitializeWebService.InitializeWebServiceSoapClient();
            service.InitConnectionStringAsync("dimensionChoiceControl", dimension_connectionString.Text);
            service.InitConnectionStringCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(dimensionChoice_InitConnectionStringCompleted);
        }

        void dimensionChoice_InitConnectionStringCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                dimensionChoiceControl.URL = "http://localhost:1688/OlapWebService.asmx";
                dimensionChoiceControl.Connection = "dimensionChoiceControl";
                dimensionChoiceControl.CubeName = dimension_cubeName.Text;
                dimensionChoiceControl.Initialize();
            }
            else
            {
                MessageBox.Show(e.Error.ToString());
            }     
        }

        private void initHierarchyChoiceButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeWebService.InitializeWebServiceSoapClient service = new UILibrary.Olap.UITestApplication.InitializeWebService.InitializeWebServiceSoapClient();
            service.InitConnectionStringAsync("hierarchyChoiceControl", hierarchy_connectionString.Text);
            service.InitConnectionStringCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(hierarchyChoice_InitConnectionStringCompleted);

            //PopUpQuestionDialog dlg = new PopUpQuestionDialog();
            //dlg.DialogClosed += new EventHandler<DialogResultArgs>(dlg_DialogClosed);
            //dlg.Caption = "Внимание...";
            //dlg.Show("Внимание...", "Вы действительно хотите проинициализировать контрол выбора иерархии измерения?", DialogButtons.YesNo);
        }

        void hierarchyChoice_InitConnectionStringCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            hierarchyChoiceControl.URL = "http://localhost:1688/OlapWebService.asmx";
            hierarchyChoiceControl.Connection = "hierarchyChoiceControl";
            hierarchyChoiceControl.CubeName = hierarchy_cubeName.Text;
            hierarchyChoiceControl.DimensionUniqueName = hierarchy_dimensionName.Text;
            hierarchyChoiceControl.Initialize();
        }

        void dlg_DialogClosed(object sender, DialogResultArgs e)
        {
            if (e.Result == DialogResult.Yes)
            {
                InitializeWebService.InitializeWebServiceSoapClient service = new UILibrary.Olap.UITestApplication.InitializeWebService.InitializeWebServiceSoapClient();
                service.InitConnectionStringAsync("hierarchyChoiceControl", hierarchy_connectionString.Text);
                hierarchyChoiceControl.URL = "http://localhost:1688/OlapWebService.asmx";
                hierarchyChoiceControl.Connection = "hierarchyChoiceControl";
                hierarchyChoiceControl.CubeName = hierarchy_cubeName.Text;
                hierarchyChoiceControl.DimensionUniqueName = hierarchy_dimensionName.Text;
                hierarchyChoiceControl.Initialize();
            }
        }

        private void initServerExplorerButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeWebService.InitializeWebServiceSoapClient service = new UILibrary.Olap.UITestApplication.InitializeWebService.InitializeWebServiceSoapClient();
            service.InitConnectionStringAsync("serverExplorerControl", server_connectionString.Text);
            service.InitConnectionStringCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(serverExplorer_InitConnectionStringCompleted);
        }

        void serverExplorer_InitConnectionStringCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                serverExplorerControl.URL = "http://localhost:1688/OlapWebService.asmx";
                serverExplorerControl.Connection = "serverExplorerControl";
                serverExplorerControl.Initialize();
            }
            else
            {
                MessageBox.Show(e.Error.ToString());
            }  
        }

        private void customCodeTestButton_Click(object sender, RoutedEventArgs e)
        {
            CultureInfo ci = new CultureInfo("ru");
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            //if (this.pivotGridControl.PivotPanelControl.CellsControl.FocusedCell != null)
            //{
            //    Rect rect = this.pivotGridControl.PivotPanelControl.CellsControl.FocusedCell.SLBounds;
            //}
        }

        private void initDateChoiceButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeWebService.InitializeWebServiceSoapClient service = new UILibrary.Olap.UITestApplication.InitializeWebService.InitializeWebServiceSoapClient();
            service.InitConnectionStringAsync("dateChoiceControl", date_connectionString.Text);
            service.InitConnectionStringCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(dateChoice_InitConnectionStringCompleted);
        }

        void dateChoice_InitConnectionStringCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                dateChoiceControl.URL = "http://localhost:1688/OlapWebService.asmx";
                dateChoiceControl.Connection = "dateChoiceControl";
                dateChoiceControl.CubeName = date_cubeName.Text;
                dateChoiceControl.DayLevelUniqueName = date_dateLevelUniqueName.Text;
                dateChoiceControl.DateToUniqueNameTemplate = dateChoiceControl.DayLevelUniqueName + ".[<DD>.<MM>.<YYYY>]";

                dateChoiceControlPopUp.URL = "http://localhost:1688/OlapWebService.asmx";
                dateChoiceControlPopUp.Connection = "dateChoiceControl";
                dateChoiceControlPopUp.CubeName = date_cubeName.Text;
                dateChoiceControlPopUp.DayLevelUniqueName = date_dateLevelUniqueName.Text;
                dateChoiceControlPopUp.DateToUniqueNameTemplate = dateChoiceControl.DayLevelUniqueName + ".[<DD>.<MM>.<YYYY>]";
            }
            else
            {
                MessageBox.Show(e.Error.ToString());
            }
        }

        private void initmdxDesignerButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeWebService.InitializeWebServiceSoapClient service = new UILibrary.Olap.UITestApplication.InitializeWebService.InitializeWebServiceSoapClient();
            service.InitConnectionStringAsync("mdxDesigner", mdxDesigner_connectionString.Text);
            service.InitConnectionStringCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(mdxDesigner_InitConnectionStringCompleted);
        }

        void mdxDesigner_InitConnectionStringCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                pivotMdxDesignerControl.URL = "http://localhost:1688/OlapWebService.asmx";
                pivotMdxDesignerControl.Connection = "mdxDesigner";
                pivotMdxDesignerControl.CubeName = mdxDesigner_cubeName.Text;
                pivotMdxDesignerControl.SubCube = mdxDesigner_subCube.Text;
                pivotMdxDesignerControl.Initialize();
            }
            else
            {
                MessageBox.Show(e.Error.ToString());
            }
        }

        String MdxLayout = String.Empty;
        private void exportMdxLayoutButton_Click(object sender, RoutedEventArgs e)
        {
            MdxLayout = pivotMdxDesignerControl.ExportMdxLayoutInfo();
        }

        private void importMdxLayoutButton_Click(object sender, RoutedEventArgs e)
        {
            pivotMdxDesignerControl.ImportMdxLayoutInfo(MdxLayout);
        }

        private void initFastPivotGridButton_Click(object sender, RoutedEventArgs e)
        {
            //InitializeWebService.InitializeWebServiceSoapClient service = new UILibrary.Olap.UITestApplication.InitializeWebService.InitializeWebServiceSoapClient();
            //service.Endpoint.Address = new System.ServiceModel.EndpointAddress(new Uri("http://localhost:1688/InitializeWebService.asmx"));
            //service.InitConnectionStringCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(pivotFastGrid_InitConnectionStringCompleted);
            //service.InitConnectionStringAsync("pivotGridControl1", pivorGrid_connectionString.Text);
        }

        void pivotFastGrid_InitConnectionStringCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            //if (e.Error == null)
            //{
            //    fastPivotGridControl.URL = "http://localhost:1688/OlapWebService.asmx";
            //    fastPivotGridControl.Connection = "pivotGridControl1";
            //    fastPivotGridControl.Query = pivorGrid_query.Text;
            //    fastPivotGridControl.UpdateScript = pivorGrid_updateScript.Text;
            //    //fastPivotGridControl.IsUpdateable = true;
            //    fastPivotGridControl.Initialize();
            //}
            //else
            //{
            //    MessageBox.Show(e.Error.ToString());
            //}
        }

        private void initValueCopyButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeWebService.InitializeWebServiceSoapClient service = new UILibrary.Olap.UITestApplication.InitializeWebService.InitializeWebServiceSoapClient();
            service.Endpoint.Address = new System.ServiceModel.EndpointAddress(new Uri("http://localhost:1688/InitializeWebService.asmx"));
            service.InitConnectionStringCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(ValueCopy_InitConnectionStringCompleted);
            service.InitConnectionStringAsync("valueCopyDesignerControl", valueCopy_connectionString.Text);
        }

        void ValueCopy_InitConnectionStringCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                valueCopyControl.URL = "http://localhost:1688/OlapWebService.asmx";
                valueCopyControl.Connection = "valueCopyDesignerControl";
                valueCopyControl.CubeName = valueCopy_cubeName.Text;
                valueCopyControl.Query = valueCopy_query.Text;
                valueCopyControl.Initialize();
            }
            else
            {
                MessageBox.Show(e.Error.ToString());
            }
        }
    }
}
