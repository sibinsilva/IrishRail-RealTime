using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IrishRail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainDetails : ContentPage
    {
        public TrainDetails()
        {
            InitializeComponent();
            App.CheckInternetConnectivity(this.lbl_NoInternet, this);
            TrainDetail();
        }

        public void TrainDetail()
        {
            var time = "";
            var Train = TrainList.SelectedTrain;
            this.Origin.Text = Origin.Text + Train.Origin.ToString();
            this.Destination.Text = Destination.Text + Train.Destination.ToString();
            if(Convert.ToInt32(Train.Exparrival.Substring(0,2))==00 || Convert.ToInt32(Train.Exparrival.Substring(0, 2))<13)
            {
                time = "AM";
            }
            else
            {
                time = "PM";
            }
            if (Convert.ToInt32(Train.Exparrival.Substring(0, 2)) == 00)
            {
                this.ExpArrival.Text = "Expected Departure in: " + Train.Expdepart.ToString() + " " + time;
            }
            else
            {
                this.ExpArrival.Text = ExpArrival.Text + Train.Exparrival.ToString() + " " + time;
            }
            if (Convert.ToInt32(Train.Scharrival.Substring(0, 2)) == 00)
            {
                this.SchArrival.Text = "Scheduled Departure in: " + Train.Schdepart.ToString() + " " + time;
            }
            else
            {
                this.SchArrival.Text = SchArrival.Text + Train.Scharrival.ToString() + " " + time;
            }
            this.DueIn.Text = DueIn.Text + Train.Duein.ToString() +  " minutes";
            if (Train.Late == "0")
            {
                this.Late.IsVisible = false;
            }
            else
            {
                this.Late.Text = Late.Text + Train.Late.ToString() + " minutes";
            }
            
            if (Train.Lastlocation == null || Train.Lastlocation =="")
            {
                this.Status.Text = Status.Text + "Status Unavailable";
            }
            else
            {
                this.Status.Text = Status.Text + Train.Lastlocation.ToString();
            }
            
        }

        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new TrainList();
            return true;
        }

        private void BookTicket_Clicked(object sender, EventArgs e)
        {
            Launcher.TryOpenAsync("https://booking.cf.irishrail.ie/en-IE/mys3/login");
        }

        
    }
}