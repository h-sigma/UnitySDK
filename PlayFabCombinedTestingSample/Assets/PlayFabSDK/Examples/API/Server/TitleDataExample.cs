using System;
using UnityEngine;

namespace PlayFab.Examples.Server
{
    public static class TitleDataExample
    {
        #region Controller Event Handling
        static TitleDataExample()
        {
            PfSharedControllerEx.RegisterEventMessage(PfSharedControllerEx.EventType.OnUserLogin, OnUserLogin);
        }

        public static void SetUp()
        {
            // The static constructor is called as a by-product of this call
        }

        private static void OnUserLogin(string playFabId, string characterId, PfSharedControllerEx.Api eventSourceApi, bool requiresFullRefresh)
        {
            GetTitleData();
            GetTitleInternalData();
        }
        #endregion Controller Event Handling

        #region Title Data - Information stored per-title, usually title-global information
        public static void GetTitleData()
        {
            var getRequest = new ServerModels.GetTitleDataRequest();
            // getRequest.Keys = new System.Collections.Generic.List<string>() { filterKey };
            PlayFabServerAPI.GetTitleData(getRequest, GetTitleDataCallback, PfSharedControllerEx.FailCallback("GetTitleData"));
        }
        private static void GetTitleDataCallback(ServerModels.GetTitleDataResult result)
        {
            foreach (var eachTitleEntry in result.Data)
                PfSharedModelEx.titleData[eachTitleEntry.Key] = eachTitleEntry.Value;
            PfSharedControllerEx.PostEventMessage(PfSharedControllerEx.EventType.OnTitleDataLoaded, null, null, PfSharedControllerEx.Api.Server, false);
        }

        public static void GetTitleInternalData()
        {
            var getRequest = new ServerModels.GetTitleDataRequest();
            // getRequest.Keys = new System.Collections.Generic.List<string>() { filterKey };
            PlayFabServerAPI.GetTitleInternalData(getRequest, GetInternalTitleDataCallback, PfSharedControllerEx.FailCallback("GetTitleInternalData"));
        }
        private static void GetInternalTitleDataCallback(ServerModels.GetTitleDataResult result)
        {
            foreach (var eachTitleEntry in result.Data)
                PfSharedModelEx.titleInternalData[eachTitleEntry.Key] = eachTitleEntry.Value;
            PfSharedControllerEx.PostEventMessage(PfSharedControllerEx.EventType.OnTitleDataLoaded, null, null, PfSharedControllerEx.Api.Server, false);
        }

        public static Action SetTitleData(string titleDataKey, string titleDataValue, bool internalData = false)
        {
            if (string.IsNullOrEmpty(titleDataValue))
                titleDataValue = null; // Ensure that this field is removed

            Action output = () =>
            {
                // This api-call updates one titleData key at a time.
                // You can remove a key by setting the value to null.
                var updateRequest = new ServerModels.SetTitleDataRequest();
                updateRequest.Key = titleDataKey;
                updateRequest.Value = titleDataValue;

                if (internalData)
                    PlayFabServerAPI.SetTitleInternalData(updateRequest, SetInternalTitleDataCallback, PfSharedControllerEx.FailCallback("SetTitleInternalData"));
                else
                    PlayFabServerAPI.SetTitleData(updateRequest, SetTitleDataCallback, PfSharedControllerEx.FailCallback("SetTitleData"));
            };
            return output;
        }
        private static void SetTitleDataCallback(ServerModels.SetTitleDataResult result)
        {
            string dataKey = ((ServerModels.SetTitleDataRequest)result.Request).Key;
            string dataValue = ((ServerModels.SetTitleDataRequest)result.Request).Value;

            if (string.IsNullOrEmpty(dataValue))
            {
                PfSharedModelEx.titleData.Remove(dataKey);
            }
            else
            {
                PfSharedModelEx.titleData[dataKey] = dataValue;
            }

            PfSharedControllerEx.PostEventMessage(PfSharedControllerEx.EventType.OnTitleDataChanged, null, null, PfSharedControllerEx.Api.Server, false);
        }
        private static void SetInternalTitleDataCallback(ServerModels.SetTitleDataResult result)
        {
            string dataKey = ((ServerModels.SetTitleDataRequest)result.Request).Key;
            string dataValue = ((ServerModels.SetTitleDataRequest)result.Request).Value;

            if (string.IsNullOrEmpty(dataValue))
                PfSharedModelEx.titleInternalData.Remove(dataKey);
            else
                PfSharedModelEx.titleInternalData[dataKey] = dataValue;

            PfSharedControllerEx.PostEventMessage(PfSharedControllerEx.EventType.OnTitleDataChanged, null, null, PfSharedControllerEx.Api.Server, false);
        }
        #endregion Title Data - Information stored per-title, usually title-global information

        #region Publisher Data - Information stored for all titles under a publisher
        public static void GetPublisherData()
        {
            var getRequest = new ServerModels.GetPublisherDataRequest();
            // getRequest.Keys = new System.Collections.Generic.List<string>() { filterKey };
            PlayFabServerAPI.GetPublisherData(getRequest, GetPublisherDataCallback, PfSharedControllerEx.FailCallback("GetPublisherData"));
        }
        private static void GetPublisherDataCallback(ServerModels.GetPublisherDataResult result)
        {
            foreach (var eachPublisherEntry in result.Data)
                PfSharedModelEx.publisherData[eachPublisherEntry.Key] = eachPublisherEntry.Value;
            PfSharedControllerEx.PostEventMessage(PfSharedControllerEx.EventType.OnTitleDataLoaded, null, null, PfSharedControllerEx.Api.Server, false);
        }

        public static Action SetPublisherData(string PublisherDataKey, string PublisherDataValue)
        {
            if (string.IsNullOrEmpty(PublisherDataValue))
                PublisherDataValue = null; // Ensure that this field is removed

            Action output = () =>
            {
                // This api-call updates one PublisherData key at a time.
                // You can remove a key by setting the value to null.
                var updateRequest = new ServerModels.SetPublisherDataRequest();
                updateRequest.Key = PublisherDataKey;
                updateRequest.Value = PublisherDataValue;

                PlayFabServerAPI.SetPublisherData(updateRequest, SetPublisherDataCallback, PfSharedControllerEx.FailCallback("SetPublisherData"));
            };
            return output;
        }
        private static void SetPublisherDataCallback(ServerModels.SetPublisherDataResult result)
        {
            string dataKey = ((ServerModels.SetPublisherDataRequest)result.Request).Key;
            string dataValue = ((ServerModels.SetPublisherDataRequest)result.Request).Value;

            if (string.IsNullOrEmpty(dataValue))
            {
                PfSharedModelEx.publisherData.Remove(dataKey);
            }
            else
            {
                PfSharedModelEx.publisherData[dataKey] = dataValue;
            }

            PfSharedControllerEx.PostEventMessage(PfSharedControllerEx.EventType.OnTitleDataChanged, null, null, PfSharedControllerEx.Api.Server, false);
        }
        #endregion Publisher Data - Information stored for all titles under a publisher
    }
}
