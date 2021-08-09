using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GpsNotepad.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomFrame : ContentView
    {
        public CustomFrame()
        {
            
            InitializeComponent();
            customFrame.IsVisible = true;
        }



        public static readonly BindableProperty IsVisibleFrameProperty =
    BindableProperty.Create(nameof(IsVisibleFrame),
                    typeof(bool),
                    typeof(CustomFrame),
                    defaultValue: default,
                    BindingMode.TwoWay,
                    propertyChanged: IsVisibleFramePropertyChanged);

        public bool IsVisibleFrame
        {
            get => (bool)GetValue(IsVisibleFrameProperty);
            set => SetValue(IsVisibleFrameProperty, value);
        }

        private static async void IsVisibleFramePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomFrame customFrame1 = bindable as CustomFrame;

            if (customFrame1 != null)
            {

                var myFrameControl = customFrame1.customFrame;
                
                await Task.WhenAny<bool>(myFrameControl.FadeTo(0, 500), myFrameControl.ScaleTo(0, 500));

                customFrame1.customFrame.IsVisible = (bool)newValue;
                customFrame1.IsVisibleFrame = (bool)newValue;
                await Task.WhenAny<bool>(myFrameControl.ScaleTo(1, 500), myFrameControl.FadeTo(1, 500));
                
            }
        }
    }
}