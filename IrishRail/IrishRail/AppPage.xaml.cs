using Android.App;
using Android.Content;
using Android.Locations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IrishRail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppPage : ContentPage
    {
        public AppPage()
        {
            InitializeComponent();

        }
        public static string latitude;
        public static string longitude;
        public string UserEntry;
        public bool CloseApp = true;

        public ObservableCollection<StationName> StationList;
        public static ArrayOfObjStationData TrainData = new ArrayOfObjStationData();


        private void btnFind_Clicked(object sender, EventArgs e)
        {
            GetResultAsync();
        }

        private async Task GetResultAsync()
        {
            await GetLocationCordsAsync();
            if (latitude != null || longitude != null)
            {
                this.BindingContext = this;
                GoogleMapsService GmapService = new GoogleMapsService();
                StationList = new ObservableCollection<StationName>();
                List<string> stations = GmapService.GetStationDetails(latitude, longitude);
                foreach (string station in stations)
                {
                    StationList.Add(new StationName() { TrainStationName = station });
                }
                StationNameList.ItemsSource = StationList;
            }
        }

        private async Task GetLocationCordsAsync()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location != null)
                {
                    latitude = location.Latitude.ToString();
                    longitude = location.Longitude.ToString();
                }
                else
                {
                    await DisplayAlert("GPS Access Issue", "Enable GPS from Settings to continue", "Ok").ConfigureAwait(true);
                }
            }

            catch (Exception ex)
            {
                await DisplayAlert("Warning", ex.ToString(), "Ok");
            }
        }

        public void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            StationData stationtraindata = new StationData();
            if (e.SelectedItem != null)
            {
                var SelectedStation = (StationName)e.SelectedItem;

                foreach (var station in MainPage.IrishStationList.ObjStation)
                {
                    if (station.StationDesc == SelectedStation.TrainStationName.ToString() || SelectedStation.TrainStationName.ToString().Contains(station.StationDesc,StringComparison.InvariantCultureIgnoreCase))
                    {
                        TrainData = StationData.GetStationTrains(station.StationCode);
                        if (TrainData.ObjStationData.Count > 0)
                        {
                            App.Current.MainPage = new TrainList();
                        }
                        else
                        {
                            DisplayAlert("No Train details are available", "Try a different station", "Ok");
                        }
                    }
                }
            }
        }

        class MyView : ContentView
        {
            string _text;

            public string Text
            {
                get => _text;
                set
                {
                    _text = value;
                    HandleTextChanged();
                }
            }

            private void HandleTextChanged()
            {
                var searchText = _text;
                if (searchText != null)
                {
                    List<string> names = null;
                    foreach (var desc in MainPage.IrishStationList.ObjStation)
                    {
                        names.Add(desc.StationDesc);
                    }
                    var FilterSearchCustomer = names.Where(x => x.Contains(searchText,StringComparison.OrdinalIgnoreCase)).ToList();
                }
            }

        }


        private void btnSearch_Clicked(object sender, EventArgs e)
        {
            if (this.txtStation.Text==null)
            {
                DisplayAlert("You must enter a search text", "", "Retry");
            }
            else
            {
                UserEntry = this.txtStation.Text;
                StationList = new ObservableCollection<StationName>();

                foreach(var stationData in MainPage.IrishStationList.ObjStation)
                {
                    if (stationData.StationDesc.Contains(UserEntry,StringComparison.OrdinalIgnoreCase))
                    {
                        StationList.Add(new StationName() { TrainStationName = stationData.StationDesc.ToString() });
                    }
                }
                if (StationList.Count == 0)
                {
                     DisplayAlert("No Station details are available for your search", "Try a different search", "Ok");
                }
                else
                {
                    StationNameList.ItemsSource = StationList;
                }
                
            }
        }

        private void btnShow_Clicked(object sender, EventArgs e)
        {

        }

        protected override bool OnBackButtonPressed()
        {
            if (!CloseApp)
                return false;

            // don't exit the app when back button is pressed
            Device.BeginInvokeOnMainThread(async () =>
            {
                CloseApp = await DisplayAlert("Irish Rail", "Do you want to exit the application?", "No", "Yes");
                if (!CloseApp)
                {
                    Process.GetCurrentProcess().CloseMainWindow();
                    Process.GetCurrentProcess().Close();
                }
                else
                {
                    base.OnBackButtonPressed();
                }
            });
            return CloseApp;
        }
    }

}