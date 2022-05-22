using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
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
            App.CheckInternetConnectivity(this.lbl_NoInternet, this);

        }
        public static string latitude ;
        public static string longitude;
        public static string PickedStation;
        public static double PickedStationLatitude;
        public static double PickedStationLongitude;
        public string UserEntry;
        public bool CloseApp = true;

        public ObservableCollection<clsStationName> StationList;
        public static ArrayOfObjStationData TrainData = new ArrayOfObjStationData();


        private void btnFind_Clicked(object sender, EventArgs e)
        {
            GetResultAsync().ConfigureAwait(true);
        }

        private async Task GetResultAsync()
        {
            await GetLocationCordsAsync().ConfigureAwait(true);
            if (latitude != null || longitude != null)
            {
                this.BindingContext = this;
                GoogleMapsService GmapService = new GoogleMapsService();
                StationList = new ObservableCollection<clsStationName>();
                List<string> stations = GmapService.GetStationDetails(latitude, longitude);
                foreach (string station in stations)
                {
                    StationList.Add(new clsStationName() { TrainStationName = station });
                }
                StationNameList.ItemsSource = StationList;
            }
        }

        private async Task GetLocationCordsAsync()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync().ConfigureAwait(true);
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

            catch(Exception ex)
            {
                await DisplayAlert("Warning", ex.ToString(), "Ok").ConfigureAwait(true);
            }
        }

        public void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem != null)
                {
                    var SelectedStation = (clsStationName)e.SelectedItem;

                    foreach (var station in MainPage.IrishStationList.ObjStation)
                    {
                        if (station.StationDesc == SelectedStation.TrainStationName.ToString() || SelectedStation.TrainStationName.ToString().Contains(station.StationDesc, StringComparison.InvariantCultureIgnoreCase))
                        {
                            PickedStation = station.StationDesc;
                            PickedStationLatitude = Convert.ToDouble(station.StationLatitude);
                            PickedStationLongitude = Convert.ToDouble(station.StationLongitude);
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
            catch(Exception ex)
            {
                
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
            try
            {
                if (this.txtStation.Text == null)
                {
                    DisplayAlert("You must enter a search text", "", "Retry");
                }
                else
                {
                    UserEntry = this.txtStation.Text;
                    StationList = new ObservableCollection<clsStationName>();
                    if (MainPage.IrishStationList == null)
                    {
                        DisplayAlert("No Station details are available for your search", "Try a different search", "Ok");
                    }

                    foreach (var stationData in MainPage.IrishStationList.ObjStation)
                    {
                        if (stationData.StationDesc.Contains(UserEntry, StringComparison.OrdinalIgnoreCase))
                        {
                            StationList.Add(new clsStationName() { TrainStationName = stationData.StationDesc.ToString() });
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
            catch(Exception ex)
            {
                
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
                CloseApp = await DisplayAlert("Irish Rail", "Do you want to exit the application?", "No", "Yes").ConfigureAwait(true);
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