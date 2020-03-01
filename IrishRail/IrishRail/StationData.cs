using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;

namespace IrishRail
{
    public class StationData
    {
        private HttpClient CreateClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }
        public List<Stations> GetAllStationCodes()
        {
            StationData station = new StationData();
            XmlDocument maindoc = new XmlDocument();
            XmlDocument doc = new XmlDocument();
            using (var httpClient = CreateClient())
            {
                string URL = "http://api.irishrail.ie/realtime/realtime.asmx/getAllStationsXML";
                maindoc.Load(URL);
                XmlNodeList objStation = maindoc.GetElementsByTagName("StationDesc");
                List<Stations> StationList = new List<Stations>();
                foreach (XmlElement Stationelement in objStation)
                {
                    Stations StationData = new Stations();
                    StationData.StationDesc = Stationelement.InnerText;
                    StationData.StationLatitude = Convert.ToDouble(Stationelement.NextSibling.NextSibling.InnerText);
                    StationData.StationLongitude = Convert.ToDouble(Stationelement.NextSibling.NextSibling.NextSibling.InnerText);
                    StationData.StationCode = Stationelement.NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
                    StationData.StationId = Convert.ToInt32(Stationelement.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText);
                    StationList.Add(StationData);
                }
                return StationList;
            }
        }

        public List<StationTrains> GetStationTrains(string code)
        { 
            XmlDocument maindoc = new XmlDocument();
            using (var httpClient = CreateClient())
            {
                string URL = "http://api.irishrail.ie/realtime/realtime.asmx/getStationDataByCodeXML?StationCode=" + code;
                maindoc.Load(URL);
                XmlNodeList objStation = maindoc.GetElementsByTagName("Traincode");
                List<StationTrains> StationTrain = new List<StationTrains>();
                foreach (XmlElement Stationelement in objStation)
                {
                    StationTrains TrainData = new StationTrains();
                    TrainData.Traincode = Stationelement.InnerText;
                    TrainData.Stationfullname = Stationelement.NextSibling.InnerText;
                    TrainData.StationCode = Stationelement.NextSibling.NextSibling.InnerText;
                    TrainData.Origin = Stationelement.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
                    TrainData.Destination = Stationelement.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
                    TrainData.Status = Stationelement.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
                    TrainData.Lastlocation = Stationelement.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
                    TrainData.Duein = Convert.ToInt32(Stationelement.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText);
                    TrainData.Late = Convert.ToInt32(Stationelement.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText);
                    TrainData.Exparrival = Stationelement.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
                    TrainData.Expdepart = Stationelement.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
                    TrainData.Scharrival = Stationelement.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
                    TrainData.Schdepart = Stationelement.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
                    TrainData.Direction = Stationelement.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
                    StationTrain.Add(TrainData);
                }
                return StationTrain;
            }
        }
    }
}
