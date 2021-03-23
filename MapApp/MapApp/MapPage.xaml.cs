using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace MapApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        Realm realm;
        public MapPage()
        {
            InitializeComponent();
            realm = Realm.GetInstance();
            var pins = realm.All<PinObject>();
            foreach(var pin in pins)
            {
                var newPin = new Pin
                {
                    Label = "",
                    Position = new Position(pin.Latitude, pin.Longitude),
                };
                newPin.MarkerClicked += async (s, args) =>
                {
                    await Navigation.PushAsync(new PhotosPage(pin.Id));
                };
                map.Pins.Add(newPin);
            }
        }

        private void map_MapClicked(object sender, Xamarin.Forms.Maps.MapClickedEventArgs e)
        {
            var newPin = new Pin
            {
                Label = "",
                Position = e.Position,
            };
            PinObject pinObject = new PinObject();
            realm.Write(() => 
            {
                pinObject.Longitude = newPin.Position.Longitude;
                pinObject.Latitude = newPin.Position.Latitude;
                realm.Add(pinObject);
            });
            
            newPin.MarkerClicked += async (s, args) =>
            {
                await Navigation.PushAsync(new PhotosPage(pinObject.Id));
            };
            map.Pins.Add(newPin);
        }
    }
}