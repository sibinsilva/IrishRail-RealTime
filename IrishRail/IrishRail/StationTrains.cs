using System;
using System.Collections.Generic;
using System.Text;

namespace IrishRail
{
    public class StationTrains
    {
        public string Traincode { get; set; }
        public string Stationfullname { get; set; }
        public string StationCode { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public int Duein { get; set; }
        public string Lastlocation { get; set; }
        public int Late { get; set; }
        public string Status { get; set; }
        public string Exparrival { get; set; }
        public string Scharrival { get; set; }
        public string Direction { get; set; }
        public string Schdepart { get; set; }
        public string Expdepart { get; set; }
        public StationTrains()
        {
            this.Traincode = Traincode;
            this.Stationfullname = Stationfullname;
            this.StationCode = StationCode;
            this.Origin = Origin;
            this.Destination = Destination;
            this.Duein = Duein;
            this.Lastlocation = Lastlocation;
            this.Late = Late;
            this.Status = Status;
            this.Exparrival = Exparrival;
            this.Scharrival = Scharrival;
            this.Direction = Direction;
            this.Schdepart = Schdepart;
            this.Expdepart = Expdepart;
        }
    }
}
