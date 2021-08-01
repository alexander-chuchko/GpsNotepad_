using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GpsNotepad.Controls
{
    public class CustomMap:Map
    {
        private Position _cameraMovingPosition;

        public CustomMap()
        {
            MyLocationEnabled = false;
            UiSettings.MyLocationButtonEnabled = false;
            UiSettings.ZoomControlsEnabled = true;
        }

        public static readonly BindableProperty IsEnableIconZoomProperty =
            BindableProperty.Create(nameof(IsEnableIconZoom),
                typeof(bool),
                typeof(CustomMap),
                defaultValue: false,
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: OnIsEnableIconZoomPropertyChanged);

        public bool IsEnableIconZoom
        {
            get => (bool)GetValue(IsEnableIconZoomProperty);
            set => SetValue(IsEnableIconZoomProperty, value);
        }

        
        private static void OnIsEnableIconZoomPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomMap map = bindable as CustomMap;
            bool value = (bool)newValue;
            if (map != null)
            {
                map.UiSettings.ZoomControlsEnabled = value;
            }
        }
        

        /* Display list of pins */
        public static readonly BindableProperty PinsListProperty =
            BindableProperty.Create(nameof(PinsList),
                                    typeof(List<Pin>),
                                    typeof(CustomMap),
                                    defaultValue: default(List<Pin>),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: OnPinsListPropertyChanged);

        public List<Pin> PinsList
        {
            get => (List<Pin>)GetValue(PinsListProperty);
            set => SetValue(PinsListProperty, value);
        }
        private static void OnPinsListPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomMap map = (CustomMap)bindable;
            if ((List<Pin>)newValue != null)
            {
                map.Pins.Clear();
                foreach (Pin pin in (List<Pin>)newValue)
                {
                    map.Pins.Add(pin);
                }
            }
        }
        /*Checking button is enabled  */
        public static readonly BindableProperty IsMyLocationButtonVisibleProperty =
             BindableProperty.Create(nameof(IsMyLocationButtonVisible),
                                     typeof(bool),
                                     typeof(CustomMap),
                                     defaultValue: false,
                                     propertyChanged: OnIsMyLocationButtonVisiblePropertyChanged);
        public bool IsMyLocationButtonVisible
        {
            get => (bool)GetValue(IsMyLocationButtonVisibleProperty);
            set => SetValue(IsMyLocationButtonVisibleProperty, value);
        }
        private static void OnIsMyLocationButtonVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomMap map = bindable as CustomMap;
            bool value = (bool)newValue;
            if (map != null)
            {
                map.UiSettings.MyLocationButtonEnabled = value;
                map.MyLocationEnabled = value;
            }
        }

        public static readonly BindableProperty CameraMovingPositionProperty =
            BindableProperty.Create(nameof(CameraMovingPosition),
                                    typeof(Position),
                                    typeof(CustomMap),
                                    defaultValue: default,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: OnCameraMovingPositionPropertyChanged);

        public Position CameraMovingPosition
        {
            get => (Position)GetValue(CameraMovingPositionProperty);
            set => SetValue(CameraMovingPositionProperty, value);
        }
        private static void OnCameraMovingPositionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomMap map = (CustomMap)bindable;
            if (map != null)
            {
                if (newValue is Position)
                {
                    map._cameraMovingPosition = (Position)newValue;
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(map._cameraMovingPosition.Latitude, map._cameraMovingPosition.Longitude), Distance.FromMiles(1)));
                }
            }
        }
    }
}
