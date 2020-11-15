using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IrishRail
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public static ArrayOfObjStation IrishStationList = null;
        public MainPage()
        {
            InitializeComponent();
            App.CheckInternetConnectivity(this.lbl_NoInternet, this);
            image.Source = ImageSource.FromResource("IrishRail.Images.AppIcon.jpg");
        }
        private async void btnLogin_ClickedAsync(object sender, EventArgs e)
        {
            IrishStationList = StationData.GetAllStationCodes();
            App.Current.MainPage = new AppPage();
        }

        private async Task<bool> CheckInternetConnection()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                return true;
            }
            else
            {
                await DisplayAlert("No Internet Connection Detected", "Application Cannot Start without Internet Access", "Retry").ConfigureAwait(true);
                return false;
            }
        }
    }
}
