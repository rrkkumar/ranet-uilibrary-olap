using System;
using System.Net;

namespace Ranet.Olap.Core.Providers.ClientServer
{
    public enum OlapActionTypes
    { 
        Unknown,
        GetMetadata,
        GetMembers,
        GetPivotData,
        MemberAction,
        StorageAction,
        ServiceCommand,
        GetToolBarInfo,
        UpdateCube,
        ExecuteQuery
    }

    public class OlapActionBase
    {
        public OlapActionTypes ActionType = OlapActionTypes.Unknown;

        public OlapActionBase()
        { }
    }
}
