using MongoDB.Bson;
using Realms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotosPage : ContentPage
    {
        public ObservableCollection<ImageSource> ImagesSources { get; set; } = new ObservableCollection<ImageSource>();
        public ICommand TakePhotoCommand { get; protected set; }
        Realm realm;
        PinObject PinObject;
        public PhotosPage(ObjectId pinId)
        {
            InitializeComponent();
            realm = Realm.GetInstance();
            InitPhotos(pinId);
            TakePhotoCommand = new Command(async () => await TakePhoto());
            BindingContext = this;
        }

        void InitPhotos(ObjectId pinId)
        {
            PinObject = realm.Find<PinObject>(pinId);
            foreach (var photoPath in PinObject.PhotosPaths)
            {
                try
                {
                    ImagesSources.Add(ImageSource.FromFile(photoPath));
                }
                catch { }
            }
        }

        async Task TakePhoto()
        {
            try
            {
                var fileResult = await MediaPicker.CapturePhotoAsync();

                if (fileResult != null)
                {
                    var imgSource = ImageSource.FromFile(fileResult.FullPath);
                    ImagesSources.Add(imgSource);
                    realm.Write(() =>
                    {
                        PinObject.PhotosPaths.Add(fileResult.FullPath);
                    });
                }
                else
                    await DisplayAlert("Уведомление", "Фотографирование было прервано", "Ок");
            }
            catch (PermissionException)
            {
                await DisplayAlert("Уведомление", "Не предоставлен доступ к камере", "Ок");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Уведомление", $"Не удалось сделать фото. {ex.Message}", "Ок");
            }

        }
    }
}