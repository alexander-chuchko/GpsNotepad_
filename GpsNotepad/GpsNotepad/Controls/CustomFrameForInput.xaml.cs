using GpsNotepad.Helpers;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GpsNotepad.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomFrameForInput : ContentView
    {
        public CustomFrameForInput()
        {
            InitializeComponent();
        }

        #region --- Public properties ---

        public static readonly BindableProperty TextEntryProperty =
            BindableProperty.Create(nameof(TextEntry),
                            typeof(string),
                            typeof(CustomFrameForInput),
                            defaultValue: string.Empty,
                            defaultBindingMode: BindingMode.TwoWay,
                            propertyChanged: TextEntryPropertyChanged);

        public string TextEntry
        {
            get => (string)GetValue(TextEntryProperty);
            set => SetValue(TextEntryProperty, value);
        }

        public static readonly BindableProperty LabelInfoProperty =
            BindableProperty.Create(nameof(LabelInfo),
                typeof(string),
                typeof(CustomFrameForInput),
                defaultValue: string.Empty,
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: LabelInfoChanged);

        public string LabelInfo
        {
            get => (string)GetValue(LabelInfoProperty);
            set => SetValue(LabelInfoProperty, value);
        }

        public static readonly BindableProperty EntryBorderColorProperty =
            BindableProperty.Create(nameof(EntryBorderColor),
                typeof(Color),
                typeof(CustomFrameForInput),
                defaultValue: Color.FromHex("#858E9E"),
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: EntryBorderColorChanged);

        public Color EntryBorderColor
        {
            get => (Color)GetValue(EntryBorderColorProperty);
            set => SetValue(EntryBorderColorProperty, value);
        }

        public static readonly BindableProperty ImageSourceProperty =
            BindableProperty.Create(nameof(ImageSource),
                            typeof(string),
                            typeof(CustomFrameForInput),
                            defaultValue: string.Empty,
                            propertyChanged: ImageSourcePropertyChanged);

        public string ImageSource
        {
            get => (string)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public static readonly BindableProperty IsPasswordEntryProperty =
            BindableProperty.Create(nameof(IsPasswordEntry),
                typeof(bool),
                typeof(CustomFrameForInput),
                defaultBindingMode: BindingMode.TwoWay,
                defaultValue: default,
                propertyChanged: IsPasswordEntryPropertyChanged);

        public bool IsPasswordEntry
        {
            get => (bool)GetValue(IsPasswordEntryProperty);
            set => SetValue(IsPasswordEntryProperty, value);
        }

        private static void IsPasswordEntryPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomFrameForInput customEntry = bindable as CustomFrameForInput;
            bool value = (bool)newValue;
            if (customEntry != null)
            {
                customEntry.entry.IsPassword = value;
            }
        }

        public static readonly BindableProperty PlaceholderTextProperty =
            BindableProperty.Create(nameof(PlaceholderText),
                            typeof(string),
                            typeof(CustomFrameForInput),
                            defaultBindingMode: BindingMode.TwoWay,
                            defaultValue: default,
                            propertyChanged: PlaceholderTextPropertyChanged);


        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        public static readonly BindableProperty TapImageProperty =
            BindableProperty.Create(nameof(TapImage),
                            typeof(bool),
                            typeof(CustomFrameForInput),
                            defaultValue: false,
                            defaultBindingMode: BindingMode.TwoWay);

        public bool TapImage
        {
            get => (bool)GetValue(TapImageProperty);
            set => SetValue(TapImageProperty, value);
        }

        #endregion

        #region -- Private helpers --

        private static void TextEntryPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomFrameForInput customEntry = bindable as CustomFrameForInput;
            if (customEntry != null)
            {
                customEntry.entry.Text = (string)newValue;
            }
        }

        private static void EntryBorderColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomFrameForInput customEntry = bindable as CustomFrameForInput;
            if (customEntry != null)
            {
                customEntry.frame.BorderColor = (Color)newValue;
            }
        }

        private static void ImageSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomFrameForInput customEntry = bindable as CustomFrameForInput;

            if (customEntry != null)
            {
                if (!string.IsNullOrWhiteSpace((string)newValue))
                {
                    customEntry.clearButton.Source = (string)newValue;
                }
            }
        }

        private static void LabelInfoChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomFrameForInput customEntry = bindable as CustomFrameForInput;
            if(customEntry != null)
            {
                customEntry.Error.Text = (string)newValue;
            }
        }

        private static void PlaceholderTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomFrameForInput customEntry = bindable as CustomFrameForInput;

            if (customEntry != null)
            {
                customEntry.entry.Placeholder = (string)newValue;
            }
        }

        private void ImageButton_Tapped(object sender, EventArgs e)
        {
            TapImage = TapImage == false? true : false;
        }
        private void Entry_ChangedText(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(entry.Text))
            {
                clearButton.IsVisible = true;
                entry.Placeholder = string.Empty;
                TextEntry = e.NewTextValue;
            }
            else
            {
                clearButton.IsVisible = false;
                entry.Placeholder = PlaceholderText;
                TextEntry = e.NewTextValue;
            }
        }

        #endregion
    }
}