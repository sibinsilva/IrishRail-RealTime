using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace IrishRail
{
    public class Stations
    {
        public string StationDesc { get; set; }
        public string StationCode { get; set; }
        public double StationLatitude { get; set; }
        public double StationLongitude { get; set; }
        public Int32 StationId { get; set; }

        public Stations()
        {
            this.StationDesc = StationDesc;
            this.StationCode = StationCode;
            this.StationLatitude = StationLatitude;
            this.StationLongitude = StationLongitude;
            this.StationId = StationId;
        }
    }
}
