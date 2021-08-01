using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GpsNotepad.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomAnimationForImage : ContentView
    {
        public CustomAnimationForImage()
        {
            InitializeComponent();
        }
        public static readonly BindableProperty ImageSourceProperty =
            BindableProperty.Create(nameof(ImageSource),
                            typeof(string),
                            typeof(CustomAnimationForImage),
                            defaultValue: string.Empty,
                            propertyChanged: ImageSourcePropertyChanged);

        public string ImageSource
        {
            get => (string)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        private static async void ImageSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomAnimationForImage customImage = bindable as CustomAnimationForImage;

            if (customImage != null)
            {
                if (!string.IsNullOrWhiteSpace((string)newValue))
                { 
                    var customImageControl = customImage.animationImage;

                    await Task.WhenAny<bool>(customImageControl.FadeTo(0, 500),customImageControl.ScaleTo(0,500));

                    customImage.animationImage.Source = (string)newValue;

                    await Task.WhenAny<bool>(customImageControl.ScaleTo(1,500), customImageControl.FadeTo(1, 500));
                }
            }
        }
    }
}