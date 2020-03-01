using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IrishRail
{
    public class GoogleMapsService : IGoogleMapsApiService
    {
        static string _googleMapsKey = Settings.Default.GMapApi;

        private const string ApiBaseAddress = "https://maps.googleapis.com/maps/";
        private HttpClient CreateClient()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(ApiBaseAddress)
            };

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }
        public static void Initialize(string googleMapsKey)
        {
            _googleMapsKey = googleMapsKey;
        }


        public async Task<GooglePlaceAutoCompleteResult> GetPlaces(string text)
        {
            GooglePlaceAutoCompleteResult results = null;

            using (var httpClient = CreateClient())
            {
                var response = await httpClient.GetAsync($"api/place/autocomplete/json?input={Uri.EscapeUriString(text)}&key={_googleMapsKey}").ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json) && json != "ERROR")
                    {
                        results = await Task.Run(() =>
                           JsonConvert.DeserializeObject<GooglePlaceAutoCompleteResult>(json)
                        ).ConfigureAwait(false);

                    }
                }
            }

            return results;
        }

        public async Task<GooglePlaces> GetPlaceDetails(string placeId)
        {
            GooglePlaces result = null;
            using (var httpClient = CreateClient())
            {
                var response = await httpClient.GetAsync($"api/place/details/json?placeid={Uri.EscapeUriString(placeId)}&key={_googleMapsKey}").ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json) && json != "ERROR")
                    {
                        result = new GooglePlaces(JObject.Parse(json));
                    }
                }
            }

            return result;
        }

        public async Task<List<string>> GetStationDetails(string latitude,string longitude)
        {
            List<string> result = new List<string>();
            XmlDocument doc = new XmlDocument();
            using (var httpClient = CreateClient())
            {
                string URL = ApiBaseAddress+"api/place/nearbysearch/xml?location=" +latitude+","+ longitude+"&rankby=distance&type=train_station&key="+_googleMapsKey;
                doc.Load(URL);
                XmlNode status = doc.SelectSingleNode("//PlaceSearchResponse/status");

                if (status.InnerText == "ZERO_RESULTS")
                {
                    return null;
                }
                else
                    {
                        XmlNodeList elements = doc.SelectNodes("//PlaceSearchResponse/result");
                        foreach (XmlNode element in elements)
                        {
                            XmlNode Singleelement = element.ChildNodes.Item(0);
                            result.Add(Singleelement.InnerText);
                        }
                }
            }

            return result ;
        }
    }
}