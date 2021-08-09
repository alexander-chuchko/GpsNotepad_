using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GpsNotepad.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomNavBar : ContentView
    {
        public CustomNavBar()
        {
            InitializeComponent();
        }

        #region ---   Public properties   ---

        /*------SettingsTapCommand------*/

        public static readonly BindableProperty SettingsTapCommandProperty =
            BindableProperty.Create(nameof(SettingsTapCommand),
                typeof(ICommand),
                typeof(CustomNavBar),
                defaultValue: default(ICommand),
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: SettingsTapCommandPropertyChanged);


        public ICommand SettingsTapCommand
        {
            get => (ICommand)GetValue(SettingsTapCommandProperty);
            set => SetValue(SettingsTapCommandProperty, value);
        }

        /*----BackTapCommand----*/
        public static readonly BindableProperty BackTapCommandProperty =
        BindableProperty.Create(nameof(BackTapCommand),
                            typeof(ICommand),
                            typeof(CustomNavBar),
                            defaultValue: default(ICommand),
                            defaultBindingMode: BindingMode.TwoWay,
                            propertyChanged: BackTapCommandPropertyChanged);
        public ICommand BackTapCommand
        {
            get => (ICommand)GetValue(BackTapCommandProperty);
            set => SetValue(BackTapCommandProperty, value);
        }


        /*----ExitTapCommand---*/
        public static readonly BindableProperty ExitTapCommandProperty =
            BindableProperty.Create(nameof(ExitTapCommand),
                                    typeof(ICommand),
                                    typeof(CustomNavBar),
                                    defaultValue: default(ICommand),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: ExitTapCommandPropertyChanged);

        public ICommand ExitTapCommand
        {
            get => (ICommand)GetValue(ExitTapCommandProperty);
            set => SetValue(ExitTapCommandProperty, value);
        }

        /*-----SearchText-------*/
        public static readonly BindableProperty SearchTextProperty =
           BindableProperty.Create(nameof(SearchText),
                                   typeof(string),
                                   typeof(CustomNavBar),
                                   defaultValue: string.Empty,
                                   defaultBindingMode: BindingMode.OneWayToSource,
                                   propertyChanged: SearchTextPropertyChanged);

        public string SearchText
        {
            get => (string)GetValue(SearchTextProperty);
            set => SetValue(SearchTextProperty, value);
        }

        /*------ExitSearchProperty--------*/
        public static readonly BindableProperty ExitSearchProperty =
           BindableProperty.Create(nameof(ExitSearch),
                                   typeof(bool),
                                   typeof(CustomNavBar),
                                   defaultValue: true,
                                   defaultBindingMode: BindingMode.TwoWay,
                                   propertyChanged: ExitSearchPropertyChanged);

        public bool ExitSearch
        {
            get => (bool)GetValue(ExitSearchProperty);
            set => SetValue(ExitSearchProperty, value);
        }

        #endregion

        #region ---   Private helpers   ---

        private static void SettingsTapCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomNavBar customNavBar = bindable as CustomNavBar;
            if (customNavBar != null)
            {
                customNavBar.SettingsTapCommand = (ICommand)newValue;
            }
        }

        private static void BackTapCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomNavBar customNavBar = bindable as CustomNavBar;

            if (customNavBar != null)
            {
                customNavBar.BackTapCommand = (ICommand)newValue;
            }
        }

        private static void ExitTapCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomNavBar customNavBar = bindable as CustomNavBar;

            if (customNavBar != null)
            {
                customNavBar.ExitTapCommand = (ICommand)newValue;
            }
        }

        private static void SearchTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomNavBar customNavBar = bindable as CustomNavBar;

            if (customNavBar != null)
            {
                customNavBar.SearchText = (string)newValue;
            }
        }

        private static void ExitSearchPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomNavBar customNavBar = bindable as CustomNavBar;

            if (customNavBar != null)
            {
                if ((bool)newValue)
                {
                    customNavBar.logOutButton.IsVisible = true;
                    Grid.SetColumnSpan(customNavBar.searchBar1, 1);
                    customNavBar.grid.HorizontalOptions = LayoutOptions.FillAndExpand;
                    customNavBar.grid.Padding = new Thickness(0, 0, 0, 0);
                    customNavBar.imageClear.IsVisible = false;
                    customNavBar.searchEntry.Text = string.Empty;
                    customNavBar.backIcon.IsVisible = false;
                    customNavBar.settingsIcon.IsVisible = true;
                }
            }
        }


        private void ExitSearchBar(object sender, EventArgs e)
        {
        }

        private void SearchBar_Focused(object sender, FocusEventArgs e)
        {
            ExitSearch = false;
            logOutButton.IsVisible = false;
            Grid.SetColumnSpan(searchBar1, 2);
            grid.Padding = new Thickness(0, 0, 15, 0);
            imageClear.IsVisible = true;
            settingsIcon.IsVisible = false;
            backIcon.IsVisible = true;
        }

        private void ImageClear_Tapped(object sender, EventArgs e)
        {
            searchEntry.Text = string.Empty;
        }

        private void SearchEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchText == null)
            {
                SearchText = string.Empty;
            }
            SearchText = (string)e.NewTextValue;
        }

        #endregion
    }
}