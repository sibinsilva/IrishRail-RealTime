using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace IrishRail
{
    public static class StationData
    {
        private static HttpClient CreateClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }
        public static ArrayOfObjStation GetAllStationCodes()
        {
            try
            {
                ArrayOfObjStation StationList = new ArrayOfObjStation();
                XmlDocument maindoc = new XmlDocument();
                using (var httpClient = CreateClient())
                {
                    string URL = "http://api.irishrail.ie/realtime/realtime.asmx/getAllStationsXML";
                    maindoc.Load(URL);
                    XmlSerializer serializer = new XmlSerializer(typeof(ArrayOfObjStation));
                    using (var reader = new StringReader(maindoc.InnerXml))
                    {
                        StationList = (ArrayOfObjStation)serializer.Deserialize(reader);
                    }
                    
                }
                return StationList;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public static ArrayOfObjStationData GetStationTrains(string code)
        {
            ArrayOfObjStationData StationTrainList = new ArrayOfObjStationData();
            XmlDocument maindoc = new XmlDocument();
            using (var httpClient = CreateClient())
            {
                string URL = "http://api.irishrail.ie/realtime/realtime.asmx/getStationDataByCodeXML?StationCode=" + code;
                maindoc.Load(URL);
                XmlSerializer serializer =
        new XmlSerializer(typeof(ArrayOfObjStationData));
                using (var reader = new StringReader(maindoc.InnerXml))
                {
                    StationTrainList = (ArrayOfObjStationData)serializer.Deserialize(reader);
                }
                return StationTrainList;
            }
        }
    }
}
