using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            TrainsListing();
        }
        public ObservableCollection<StationTrains> TrainsList;
        public static ObjStationData SelectedTrain;
        public void TrainsListing()
        {
            TrainsList = new ObservableCollection<StationTrains>();
            foreach (var DepartingTrain in AppPage.TrainData.ObjStationData)
            {
                this.TrSource.Text = DepartingTrain.Stationfullname;
                TrainsList.Add(new StationTrains() { Destination = DepartingTrain.Destination, Traincode=DepartingTrain.Traincode, Duein=DepartingTrain.Duein }) ;
                
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
                var Destination = (StationTrains)e.SelectedItem;
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
    }
}