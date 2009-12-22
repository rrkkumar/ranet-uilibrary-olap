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
using Ranet.AgOlap.Controls.General.ClientServer;
using Ranet.AgOlap;
using Ranet.Olap.Core.Metadata;
using Ranet.AgOlap.Controls.MemberChoice.ClientServer;
using Ranet.Olap.Core.Data;
using Ranet.Olap.Core.Providers;
using Ranet.Olap.Web.PivotGrid;
using Ranet.Web.Olap.PivotGrid;
using Ranet.Olap.Core;
using Ranet.Olap.Core.Providers.ClientServer;
using System.IO;
using System.Xml;
using Microsoft.AnalysisServices.AdomdClient;
using Ranet.Olap.Core.Storage;
using System.Web;
using System.Configuration;

namespace Ranet.Web.Olap
{
    public class OlapWebServiceBase : System.Web.Services.WebService
    {
        public OlapWebServiceBase()
        {
            if (ConfigurationSettings.AppSettings.AllKeys.Contains("CompressData"))
            {
                String str = ConfigurationSettings.AppSettings["CompressData"];
                try
                {
                    if (!String.IsNullOrEmpty(str))
                        UseCompress = Convert.ToBoolean(str);
                }
                catch (System.FormatException)
                {
                }
            }
        }

        bool UseCompress = true;

        public String PerformOlapServiceAction(String schemaType, String schema)
        {
            try
            {
                object type = OlapActionTypes.Parse(typeof(OlapActionTypes), schemaType, true);
                if (type != null)
                {
                    OlapActionTypes actionType = (OlapActionTypes)type;
                    switch (actionType)
                    {
                        case OlapActionTypes.GetMetadata:
                            return GetMetaData(schema);
                        case OlapActionTypes.GetMembers:
                            return GetMembersData(schema);
                        case OlapActionTypes.GetPivotData:
                            return GetPivotData(schema);
                        case OlapActionTypes.MemberAction:
                            return PerformMemberAction(schema);
                        case OlapActionTypes.StorageAction:
                            return PerformStorageAction(schema);
                        case OlapActionTypes.ServiceCommand:
                            return PerformServiceCommand(schema);
                        case OlapActionTypes.GetToolBarInfo:
                            return GetToolBarInfo(schema);
                        case OlapActionTypes.UpdateCube:
                            return UpdateCube(schema);
                        case OlapActionTypes.ExecuteQuery:
                            return ExecuteQuery(schema);
                    }
                }

                // Metadata
                //OlapActionBase metadata_args = XmlUtility.XmlStr2Obj<OlapActionBase>(schema);
                //if (metadata_args != null && metadata_args.ActionType == OlapActionTypes.GetMetadata)
                //{
                //    return GetMetaData(schema);
                //}

                //// Members
                //OlapActionBase members_args = XmlUtility.XmlStr2Obj<OlapActionBase>(schema);
                //if (members_args != null && members_args.ActionType == OlapActionTypes.GetMembers)
                //{
                //    return GetMembersData(schema);
                //}

                InvokeResultDescriptor result = new InvokeResultDescriptor();
                return XmlUtility.Obj2XmlStr(result, Common.Namespace);
            }
            catch (Exception ex)
            {
                InvokeResultDescriptor result = new InvokeResultDescriptor();
                result.Content = ex.ToString();
                result.ContentType = InvokeContentType.Error;
                return XmlUtility.Obj2XmlStr(result, Common.Namespace);
            }
        }

        #region Загрузка данных для контрола выбора элемента измерения
        public String GetMembersData(String schema)
        {
            InvokeResultDescriptor result = new InvokeResultDescriptor();
            String res = String.Empty;
            try
            {
                MemberChoiceQuery args = XmlUtility.XmlStr2Obj<MemberChoiceQuery>(schema);

                if (args != null)
                {
                    switch (args.QueryType)
                    {
                        case MemberChoiceQueryType.GetRootMembersCount:
                            res = GetRootMembersCount(args);
                            break;
                        case MemberChoiceQueryType.GetRootMembers:
                            res = GetRootMembers(args);
                            break;
                        case MemberChoiceQueryType.GetChildrenMembers:
                            res = GetChildrenMembers(args);
                            break;
                        case MemberChoiceQueryType.FindMembers:
                            res = FindMembers(args);
                            break;
                        case MemberChoiceQueryType.GetAscendants:
                            res = GetAscendants(args);
                            break;
                        case MemberChoiceQueryType.GetMember:
                            res = GetMember(args);
                            break;
                        case MemberChoiceQueryType.GetMembers:
                            res = GetMembers(args);
                            break;
                        case MemberChoiceQueryType.LoadSetWithAscendants:
                            res = LoadSetWithAscendants(args);
                            break;
                    }
                }
                result.Content = res;
                //if (UseCompress)
                //{
                //    // Архивация строки
                //    String compesed = ZipCompressor.CompressToBase64String(res);
                //    result.Content = compesed;
                //    result.IsArchive = true;
                //}
                result.ContentType = InvokeContentType.MultidimData;
            }
            catch (AdomdConnectionException connection_ex)
            {
                result.Content = connection_ex.Message;
                result.ContentType = InvokeContentType.Error;
            }
            catch (AdomdErrorResponseException response_ex)
            {
                result.Content = response_ex.Message;
                result.ContentType = InvokeContentType.Error;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return XmlUtility.Obj2XmlStr(result, Common.Namespace);
        }

        String LoadSetWithAscendants(MemberChoiceQuery args)
        {
            OlapProvider provider = new OlapProvider(new DefaultQueryExecuter(GetConnection(args.Connection)));

            List<MemberDataWrapper> members = null;
            members = provider.LoadSetWithAscendants(args.Set, args.CubeName, args.SubCube, args.HierarchyUniqueName);

            return XmlUtility.Obj2XmlStr(members, Common.Namespace);
        }

        String GetMembers(MemberChoiceQuery args)
        {
            OlapProvider provider = new OlapProvider(new DefaultQueryExecuter(GetConnection(args.Connection)));

            List<MemberDataWrapper> members = null;
            members = provider.GetMembers(args.CubeName, args.SubCube, args.Set);

            return XmlUtility.Obj2XmlStr(members, Common.Namespace);
        }

        String GetMember(MemberChoiceQuery args)
        {
            OlapProvider provider = new OlapProvider(new DefaultQueryExecuter(GetConnection(args.Connection)));

            MemberDataWrapper member = null;
            member = provider.GetMember(args.CubeName, args.MemberUniqueName, args.Properties);

            return XmlUtility.Obj2XmlStr(member, Common.Namespace);
        }

        String GetRootMembersCount(MemberChoiceQuery args)
        {
            OlapProvider provider = new OlapProvider(new DefaultQueryExecuter(GetConnection(args.Connection)));

            double count = 0;

            if (String.IsNullOrEmpty(args.StartLevelUniqueName))
                count = provider.GetMembersCount(args.CubeName, args.SubCube, args.HierarchyUniqueName, 0);
            else
                count = provider.GetMembersCount(args.CubeName, args.SubCube, args.HierarchyUniqueName, args.StartLevelUniqueName);

            return count.ToString();
        }

        String GetRootMembers(MemberChoiceQuery args)
        {
            OlapProvider provider = new OlapProvider(new DefaultQueryExecuter(GetConnection(args.Connection)));

            List<MemberDataWrapper> members = null;
            if (String.IsNullOrEmpty(args.StartLevelUniqueName))
                members = provider.GetHierarchyMembers(args.CubeName, args.SubCube, args.HierarchyUniqueName, 0, args.BeginIndex, args.Count);
            else
                members = provider.GetLevelMembers(args.CubeName, args.SubCube, args.HierarchyUniqueName, args.StartLevelUniqueName, args.BeginIndex, args.Count);

            return XmlUtility.Obj2XmlStr(members, Common.Namespace);
        }

        String GetChildrenMembers(MemberChoiceQuery args)
        {
            OlapProvider provider = new OlapProvider(new DefaultQueryExecuter(GetConnection(args.Connection)));

            List<MemberDataWrapper> members = null;
            members = provider.GetChildrenMembers(args.CubeName, args.SubCube, args.HierarchyUniqueName, args.MemberUniqueName, args.BeginIndex, args.Count);

            return XmlUtility.Obj2XmlStr(members, Common.Namespace);
        }


        String GetAscendants(MemberChoiceQuery args)
        {
            OlapProvider provider = new OlapProvider(new DefaultQueryExecuter(GetConnection(args.Connection)));

            List<MemberDataWrapper> members = null;
            members = provider.GetAscendants(args.CubeName, args.SubCube, args.HierarchyUniqueName, args.StartLevelUniqueName, args.MemberUniqueName);

            return XmlUtility.Obj2XmlStr(members, Common.Namespace);
        }


        String FindMembers(MemberChoiceQuery args)
        {
            OlapProvider provider = new OlapProvider(new DefaultQueryExecuter(GetConnection(args.Connection)));

            List<MemberDataWrapper> members = null;
            members = provider.SearchMembers(args.CubeName, args.SubCube, args.HierarchyUniqueName, args.StartLevelUniqueName, args.Filter);

            return XmlUtility.Obj2XmlStr(members, Common.Namespace);
        }
        #endregion Загрузка данных для контрола выбора элемента измерения

        #region Загрузка метаданных
        public String GetMetaData(String schema)
        {
            InvokeResultDescriptor result = new InvokeResultDescriptor();
            String res = null;
            try
            {
                MetadataQuery args = XmlUtility.XmlStr2Obj<MetadataQuery>(schema);

                if (args != null)
                {
                    switch (args.QueryType)
                    {
                        case MetadataQueryType.GetCubes:
                            res = GetCubes(args);
                            break;
                        case MetadataQueryType.GetMeasures:
                            res = GetMeasures(args);
                            break;
                        case MetadataQueryType.GetKPIs:
                            res = GetKPIs(args);
                            break;
                        case MetadataQueryType.GetLevels:
                            res = GetLevels(args);
                            break;
                        case MetadataQueryType.GetDimensions:
                            res = GetDimensions(args);
                            break;
                        case MetadataQueryType.GetHierarchies:
                            res = GetHierarchies(args);
                            break;
                        case MetadataQueryType.GetDimension:
                            res = GetDimension(args);
                            break;
                        case MetadataQueryType.GetHierarchy:
                            res = GetHierarchy(args);
                            break;
                        case MetadataQueryType.GetMeasureGroups:
                            res = GetMeasureGroups(args);
                            break;
                        case MetadataQueryType.GetLevelProperties:
                            res = GetLevelProperties(args);
                            break;
                        case MetadataQueryType.GetCubeMetadata:
                        case MetadataQueryType.GetCubeMetadata_AllMembers:
                            res = GetCubeMetadata(args);
                            break;
                    }
                }
                result.Content = res;
                //if (UseCompress)
                //{
                //    // Архивация строки
                //    String compesed = ZipCompressor.CompressToBase64String(res);
                //    result.Content = compesed;
                //    result.IsArchive = true;
                //}
                result.ContentType = InvokeContentType.MultidimData;
            }
            catch (AdomdConnectionException connection_ex)
            {
                result.Content = connection_ex.Message;
                result.ContentType = InvokeContentType.Error;
            }
            catch (AdomdErrorResponseException response_ex)
            {
                result.Content = response_ex.Message;
                result.ContentType = InvokeContentType.Error;
            }
            catch (OlapMetadataResponseException metadata_ex)
            {
                result.Content = metadata_ex.Message;
                result.ContentType = InvokeContentType.Error;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return XmlUtility.Obj2XmlStr(result, Common.Namespace);
        }

        String GetConnectionString(String connection)
        {
            object str = this.Application[connection];
            if (str != null)
                return str.ToString();
            else
                throw new Exception("Connection String not foud in Application state");
        }

        ConnectionInfo GetConnection(String connection)
        {
            object str = this.Application[connection];
            if (str != null)
                return new ConnectionInfo(connection, str.ToString());
            else
                throw new Exception("Connection String not foud in Application state");
        }

        String GetLevelProperties(MetadataQuery args)
        {
            OlapMetadataProvider provider = new OlapMetadataProvider(GetConnectionString(args.Connection));

            // Делать коллекцию с ключем "Имя свойства" нельзя, т.к. свойства KEY1, KEY2 и т.д. есть не у всех уровней и например в контроле выбора элемента измерения при построении уловия поиска придется проверять для каких уровней они есть, а для каких нету
            List<LevelPropertyInfo> list = new List<LevelPropertyInfo>();
            if (String.IsNullOrEmpty(args.LevelUniqueName))
            {
                Dictionary<String, LevelInfo> levels = provider.GetLevels(args.CubeName, args.DimensionUniqueName, args.HierarchyUniqueName);
                foreach (LevelInfo li in levels.Values)
                {
                    Dictionary<String, LevelPropertyInfo> properties = provider.GetLevelProperties(args.CubeName,
                        args.DimensionUniqueName,
                        args.HierarchyUniqueName,
                        li.UniqueName);
                    foreach (LevelPropertyInfo pi in properties.Values)
                    {
                        list.Add(pi);
                    }
                }
            }
            else
            {
                Dictionary<string, LevelPropertyInfo> properties = provider.GetLevelProperties(args.CubeName,
                        args.DimensionUniqueName,
                        args.HierarchyUniqueName,
                        args.LevelUniqueName);
                foreach (LevelPropertyInfo pi in properties.Values)
                {
                    list.Add(pi);
                }
            }

            return XmlUtility.Obj2XmlStr(list, Common.Namespace);
        }

        String GetKPIs(MetadataQuery args)
        {
            OlapMetadataProvider provider = new OlapMetadataProvider(GetConnectionString(args.Connection));

            Dictionary<String, KpiInfo> list = provider.GetKPIs(args.CubeName);

            return XmlUtility.Obj2XmlStr(list.Values.ToList(), Common.Namespace);
        }

        String GetMeasureGroups(MetadataQuery args)
        {
            OlapMetadataProvider provider = new OlapMetadataProvider(GetConnectionString(args.Connection));

            List<MeasureGroupInfo> list = provider.GetMeasureGroups(args.CubeName);

            return XmlUtility.Obj2XmlStr(list, Common.Namespace);
        }

        String GetCubeMetadata(MetadataQuery args)
        {
            OlapMetadataProvider provider = new OlapMetadataProvider(GetConnectionString(args.Connection));

            CubeDefInfo info = provider.GetCubeMetadata(args.CubeName, args.QueryType);

            return XmlUtility.Obj2XmlStr(info, Common.Namespace);
        }

        String GetCubes(MetadataQuery args)
        {
            OlapMetadataProvider provider = new OlapMetadataProvider(GetConnectionString(args.Connection));

            Dictionary<String, CubeDefInfo> list = provider.GetCubes();

            return XmlUtility.Obj2XmlStr(list.Values.ToList(), Common.Namespace);
        }

        String GetMeasures(MetadataQuery args)
        {
            OlapMetadataProvider provider = new OlapMetadataProvider(GetConnectionString(args.Connection));

            Dictionary<String, MeasureInfo> list = provider.GetMeasures(args.CubeName);

            return XmlUtility.Obj2XmlStr(list.Values.ToList(), Common.Namespace);
        }

        String GetLevels(MetadataQuery args)
        {
            OlapMetadataProvider provider = new OlapMetadataProvider(GetConnectionString(args.Connection));

            Dictionary<String, LevelInfo> list = provider.GetLevels(args.CubeName, args.DimensionUniqueName, args.HierarchyUniqueName);

            return XmlUtility.Obj2XmlStr(list.Values.ToList(), Common.Namespace);
        }

        String GetDimensions(MetadataQuery args)
        {
            OlapMetadataProvider provider = new OlapMetadataProvider(GetConnectionString(args.Connection));

            Dictionary<String, DimensionInfo> list = provider.GetDimensions(args.CubeName);

            return XmlUtility.Obj2XmlStr(list.Values.ToList(), Common.Namespace);
        }

        String GetHierarchies(MetadataQuery args)
        {
            OlapMetadataProvider provider = new OlapMetadataProvider(GetConnectionString(args.Connection));

            Dictionary<String, HierarchyInfo> list = provider.GetHierarchies(args.CubeName, args.DimensionUniqueName);

            return XmlUtility.Obj2XmlStr(list.Values.ToList(), Common.Namespace);
        }

        String GetDimension(MetadataQuery args)
        {
            OlapMetadataProvider provider = new OlapMetadataProvider(GetConnectionString(args.Connection));

            DimensionInfo info = provider.GetDimension(args.CubeName, args.DimensionUniqueName);
            return XmlUtility.Obj2XmlStr(info, Common.Namespace);
        }

        String GetHierarchy(MetadataQuery args)
        {
            OlapMetadataProvider provider = new OlapMetadataProvider(GetConnectionString(args.Connection));
            HierarchyInfo info = provider.GetHierarchy(args.CubeName, args.DimensionUniqueName, args.HierarchyUniqueName);
            return XmlUtility.Obj2XmlStr(info, Common.Namespace);
        }
        #endregion Загрузка метаданных

        private String OLAP_DATA_MANAGER = "OLAP_DATA_MANAGER_{0}";

        #region Загрузка метаданных для сводной таблицы
        public String GetPivotData(String schema)
        {
            InvokeResultDescriptor result = new InvokeResultDescriptor();
            String res = String.Empty;
            try
            {
                PivotInitializeArgs args = XmlUtility.XmlStr2Obj<PivotInitializeArgs>(schema);

                if (args != null)
                {
                    if (!String.IsNullOrEmpty(args.Query))
                    {
                        //OlapDataManager dataManager = this.Application[OLAP_DATA_MANAGER] as OlapDataManager;
                        //if (dataManager == null)
                        //{
                        String connStr = GetConnectionString(args.Connection);
                        OlapPivotDataManager dataManager = new OlapPivotDataManager(GetConnection(args.Connection), args.Query, args.UpdateScript);
                        if (!String.IsNullOrEmpty(args.PivotID))
                        {
                            this.Application[String.Format(OLAP_DATA_MANAGER, args.PivotID)] = dataManager;
                        }
                        //}

                        DefaultQueryExecuter manager = new DefaultQueryExecuter(GetConnection(args.Connection));
                        if (!String.IsNullOrEmpty(args.Query))
                        {
                            CellSetData cs_descr = manager.ExecuteQuery(args.Query);
                            res = CellSetData.Serialize(cs_descr);
                            //res = XmlUtility.Obj2XmlStr(cs_descr, Common.Namespace);
                        }
                    }
                    else
                    { 
                        // Пустой запрос
                        res = CellSetData.Serialize(new CellSetData());
                    }
                }

                result.Content = res;
                //if (UseCompress)
                //{
                //    // Архивация строки
                //    String compesed = ZipCompressor.CompressToBase64String(res);
                //    result.Content = compesed;
                //    result.IsArchive = true;
                //}
                result.ContentType = InvokeContentType.MultidimData;
            }
            catch (AdomdConnectionException connection_ex)
            {
                result.Content = connection_ex.Message;
                result.ContentType = InvokeContentType.Error;
            }
            catch (AdomdErrorResponseException response_ex)
            {
                result.Content = response_ex.Message;
                result.ContentType = InvokeContentType.Error;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return XmlUtility.Obj2XmlStr(result, Common.Namespace);
        }

        public String PerformMemberAction(String schema)
        {
            InvokeResultDescriptor result = new InvokeResultDescriptor();
            String res = String.Empty;
            try
            {
                PerformMemberActionArgs args = XmlUtility.XmlStr2Obj<PerformMemberActionArgs>(schema);
                if (args != null)
                {
                    OlapPivotDataManager dataManager = this.Application[String.Format(OLAP_DATA_MANAGER, args.PivotID)] as OlapPivotDataManager;
                    if (dataManager != null)
                    {
                        CellSetData csd = dataManager.PerformMemberAction(args);
                        res = CellSetData.Serialize(csd);
                        //DateTime s1 = DateTime.Now;
                        //res = XmlUtility.Obj2XmlStr(csd, Common.Namespace);
                        //DateTime s2 = DateTime.Now;
                        //System.Diagnostics.Debug.WriteLine(" Member Action result serialize time: " + (s2 - s1).ToString());

                        //DateTime d1 = DateTime.Now;
                        //String r = CellSetData.Serialize(csd);
                        //DateTime d2 = DateTime.Now;
                        //System.Diagnostics.Debug.WriteLine(" Member Action result serialize time (NEW): " + (d2 - d1).ToString());

                        //CellSetData.Deserialize(r);
                    }
                }
                result.Content = res;
                //if (UseCompress)
                //{
                //    // Архивация строки
                //    String compesed = ZipCompressor.CompressToBase64String(res);
                //    result.Content = compesed;
                //    result.IsArchive = true;
                //}
                result.ContentType = InvokeContentType.MultidimData;
            }
            catch (AdomdConnectionException connection_ex)
            {
                result.Content = connection_ex.Message;
                result.ContentType = InvokeContentType.Error;
            }
            catch (AdomdErrorResponseException response_ex)
            {
                result.Content = response_ex.Message;
                result.ContentType = InvokeContentType.Error;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return XmlUtility.Obj2XmlStr(result, Common.Namespace);
        }

        public String PerformServiceCommand(String schema)
        {
            InvokeResultDescriptor result = new InvokeResultDescriptor();
            String res = String.Empty;
            try
            {
                ServiceCommandArgs args = XmlUtility.XmlStr2Obj<ServiceCommandArgs>(schema);
                if (args != null)
                {
                    OlapPivotDataManager dataManager = this.Application[String.Format(OLAP_DATA_MANAGER, args.PivotID)] as OlapPivotDataManager;
                    if (dataManager != null)
                    {
                        switch (args.Command)
                        {
                            case ServiceCommandType.Forward:
                                dataManager.History.MoveNext();
                                res = CellSetData.Serialize(dataManager.RefreshQuery());
                                break;
                            case ServiceCommandType.Back:
                                dataManager.History.MoveBack();
                                res = CellSetData.Serialize(dataManager.RefreshQuery());
                                break;
                            case ServiceCommandType.ToBegin:
                                dataManager.History.ToBegin();
                                res = CellSetData.Serialize(dataManager.RefreshQuery());
                                break;
                            case ServiceCommandType.ToEnd:
                                dataManager.History.ToEnd();
                                res = CellSetData.Serialize(dataManager.RefreshQuery());
                                break;
                            case ServiceCommandType.HideEmptyColumns:
                                dataManager.HideEmptyColumns = true;
                                res = CellSetData.Serialize(dataManager.RefreshQuery());
                                break;
                            case ServiceCommandType.ShowEmptyColumns:
                                dataManager.HideEmptyColumns = false;
                                res = CellSetData.Serialize(dataManager.RefreshQuery());
                                break;
                            case ServiceCommandType.RotateAxes:
                                dataManager.RotateAxes = true;
                                res = CellSetData.Serialize(dataManager.RefreshQuery());
                                break;
                            case ServiceCommandType.NormalAxes:
                                dataManager.RotateAxes = false;
                                res = CellSetData.Serialize(dataManager.RefreshQuery());
                                break;
                            case ServiceCommandType.HideEmptyRows:
                                dataManager.HideEmptyRows = true;
                                res = CellSetData.Serialize(dataManager.RefreshQuery());
                                break;
                            case ServiceCommandType.ShowEmptyRows:
                                dataManager.HideEmptyRows = false;
                                res = CellSetData.Serialize(dataManager.RefreshQuery());
                                break;
                            case ServiceCommandType.ExportToExcel:
                                res = dataManager.ExportToExcel();
                                break;
                            case ServiceCommandType.GetDataSourceInfo:
                                DataSourceInfoArgs info = dataManager.GetDataSourceInfo(args);
                                res = XmlUtility.Obj2XmlStr(info, Common.Namespace);
                                break;
                            default:
                                res = CellSetData.Serialize(dataManager.RefreshQuery());
                                break;
                        }
                        
                        result.Content = res;
                        //if (UseCompress)
                        //{
                        //    // Архивация строки
                        //    String compesed = ZipCompressor.CompressToBase64String(res);
                        //    result.Content = compesed;
                        //    result.IsArchive = true;
                        //}
                        result.ContentType = InvokeContentType.MultidimData;
                    }
                }
            }
            catch (AdomdConnectionException connection_ex)
            {
                result.Content = connection_ex.Message;
                result.ContentType = InvokeContentType.Error;
            }
            catch (AdomdErrorResponseException response_ex)
            {
                result.Content = response_ex.Message;
                result.ContentType = InvokeContentType.Error;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return XmlUtility.Obj2XmlStr(result, Common.Namespace);
        }

        public String GetToolBarInfo(String schema)
        {
            InvokeResultDescriptor result = new InvokeResultDescriptor();
            String res = String.Empty;
            try
            {
                PivotGridToolBarInfo args = XmlUtility.XmlStr2Obj<PivotGridToolBarInfo>(schema);
                if (args != null)
                {
                    OlapPivotDataManager dataManager = this.Application[String.Format(OLAP_DATA_MANAGER, args.PivotID)] as OlapPivotDataManager;
                    if (dataManager != null)
                    {
                        PivotGridToolBarInfo toolBarInfo = new PivotGridToolBarInfo();
                        toolBarInfo.HistorySize = dataManager.History.Size;
                        toolBarInfo.CurrentHistoryIndex = dataManager.History.CurrentHistiryItemIndex;
                        toolBarInfo.HideEmptyRows = dataManager.HideEmptyRows;
                        toolBarInfo.HideEmptyColumns = dataManager.HideEmptyColumns;

                        res = XmlUtility.Obj2XmlStr(toolBarInfo, Common.Namespace);
                    }
                }
                result.Content = res;
                //if (UseCompress)
                //{
                //    // Архивация строки
                //    String compesed = ZipCompressor.CompressToBase64String(res);
                //    result.Content = compesed;
                //    result.IsArchive = true;
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return XmlUtility.Obj2XmlStr(result, Common.Namespace);
        }

        public delegate void MethodInvoker();

        public String ExecuteQuery(String schema)
        {
            InvokeResultDescriptor result = new InvokeResultDescriptor();
            String res = String.Empty;
            try
            {
                MdxQueryArgs args = XmlUtility.XmlStr2Obj<MdxQueryArgs>(schema);

                if (args != null)
                {
                    OlapPivotDataManager dataManager = null;
                    if (!String.IsNullOrEmpty(args.PivotID))
                    { 
                        dataManager = this.Application[String.Format(OLAP_DATA_MANAGER, args.PivotID)] as OlapPivotDataManager;
                    }

                    if (dataManager == null)
                    {
                        dataManager = new OlapPivotDataManager(GetConnection(args.Connection), args.Query, String.Empty);
                    }

                    if (dataManager != null)
                    {
                        if (args.Type == QueryExecutingType.NonQuery)
                        {
                            int i = dataManager.ExecuteNonQuery(args.Query);
                            res = XmlUtility.Obj2XmlStr(i, Common.Namespace);
                        }
                        else
                        {
                            CellSetData cs = dataManager.ExecuteQuery(args.Query);
                            res = CellSetData.Serialize(cs);
                            //res = XmlUtility.Obj2XmlStr(cs, Common.Namespace);
                        }
                    }
                }
                result.Content = res;
                //if (UseCompress)
                //{
                //    // Архивация строки
                //    String compesed = ZipCompressor.CompressToBase64String(res);
                //    result.Content = compesed;
                //    result.IsArchive = true;
                //}
                result.ContentType = InvokeContentType.MultidimData;
            }
            catch (AdomdConnectionException connection_ex)
            {
                result.Content = connection_ex.Message;
                result.ContentType = InvokeContentType.Error;
            }
            catch (AdomdErrorResponseException response_ex)
            {
                result.Content = response_ex.Message;
                result.ContentType = InvokeContentType.Error;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return XmlUtility.Obj2XmlStr(result, Common.Namespace);
        }

        public String UpdateCube(String schema)
        {
            InvokeResultDescriptor result = new InvokeResultDescriptor();
            String res = String.Empty;
            try
            {
                UpdateCubeArgs args = XmlUtility.XmlStr2Obj<UpdateCubeArgs>(schema);
                if (args != null)
                {
                    OlapPivotDataManager dataManager = this.Application[String.Format(OLAP_DATA_MANAGER, args.PivotID)] as OlapPivotDataManager;
                    if (dataManager != null)
                    {
                        if (!String.IsNullOrEmpty(dataManager.UpdateScript))
                        {
                            PivotTableUpdater.UpdateSync(dataManager.UpdateScript, args);
                        }
                    }
                }
                result.Content = res;
            }
            catch (AdomdConnectionException connection_ex)
            {
                result.Content = connection_ex.Message;
                result.ContentType = InvokeContentType.Error;
            }
            catch (AdomdErrorResponseException response_ex)
            {
                result.Content = response_ex.Message;
                result.ContentType = InvokeContentType.Error;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return XmlUtility.Obj2XmlStr(result, Common.Namespace);
        }

        #endregion Загрузка метаданных для сводной таблицы

        #region Работа с хранилищем
        public String PerformStorageAction(String schema)
        {
            InvokeResultDescriptor result = new InvokeResultDescriptor();
            String res = String.Empty;
            try
            {
                StorageActionArgs args = XmlUtility.XmlStr2Obj<StorageActionArgs>(schema);
                if (args != null)
                {
                    FileStorageProvider storageProvider = new FileStorageProvider();
                    switch (args.ActionType)
                    {
                        case StorageActionTypes.Save:
                            args.FileDescription.ContentFileName = args.FileDescription.Description.Name + ".content." + FileStorageProvider.GetFilteExtension(args.ContentType);
                            storageProvider.Save(HttpContext.Current.User, args.FileDescription.Description.Name + "." + FileStorageProvider.GetFilteExtension(args.ContentType), XmlSerializationUtility.Obj2XmlStr(args.FileDescription));
                            storageProvider.Save(HttpContext.Current.User, args.FileDescription.ContentFileName, args.Content);
                            break;
                        case StorageActionTypes.Load:
                            res = storageProvider.Load(HttpContext.Current.User, args.FileDescription.ContentFileName);
                            break;
                        case StorageActionTypes.GetList:
                            res = XmlUtility.Obj2XmlStr(storageProvider.GetList(HttpContext.Current.User, "*." + FileStorageProvider.GetFilteExtension(args.ContentType)), Common.Namespace);
                            break;
                    }
                }
                result.Content = res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return XmlUtility.Obj2XmlStr(result, Common.Namespace);
        }
        #endregion Работа с хранилищем
    }
}
