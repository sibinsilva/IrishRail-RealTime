﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IrishRail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainList : ContentPage
    {
        public TrainList()
        {
            InitializeComponent();
            App.CheckInternetConnectivity(this.lbl_NoInternet, this);
            TrainsListing();
        }
        public ObservableCollection<clsTrainList> TrainsList;
        public static ObjStationData SelectedTrain;
        public void TrainsListing()
        {
            TrainsList = new ObservableCollection<clsTrainList>();
            foreach (var DepartingTrain in AppPage.TrainData.ObjStationData)
            {
                this.TrSource.Text = DepartingTrain.Stationfullname;
                TrainsList.Add(new clsTrainList() { Destination = DepartingTrain.Destination, Traincode=DepartingTrain.Traincode, Duein = DepartingTrain.Duein });

            }
            depTrainsList.ItemsSource = TrainsList;
        }

        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new AppPage();
            return true;
        }
        private void StationList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if(e.SelectedItem!=null)
            {
                var Destination = (clsTrainList)e.SelectedItem;
                foreach(var train in AppPage.TrainData.ObjStationData)
                {
                    if(train.Traincode==Destination.Traincode)
                    {
                        SelectedTrain = train;
                    }
                }
                App.Current.MainPage = new TrainDetails();
            }
        }

        private void Directions_Clicked(object sender, EventArgs e)
        {
            var location = new Location(AppPage.PickedStationLatitude, AppPage.PickedStationLongitude);
            var options = new MapLaunchOptions { Name = AppPage.PickedStation, NavigationMode = NavigationMode.Default };
            string Source = AppPage.latitude.ToString() + "," + AppPage.longitude.ToString();
            string Destination = AppPage.PickedStationLatitude.ToString() + "," + AppPage.PickedStationLongitude.ToString();
            try
            {
                Map.OpenAsync(location, options);
            }
            catch (Exception)
            {
                clsGeneric.OpenBrowser(new Uri($"http://maps.google.com/maps?saddr={Source}&daddr={Destination}")).ConfigureAwait(true);
                // No map application available to open so try using a web browser
            }

        }
    }
}