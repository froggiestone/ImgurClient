using ImgurClient.ViewModels;
using ImgurClient.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ImgurClient
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel mainviewmodel = new MainViewModel();
        bool ViewModelInitialized = false;

        public MainPage()
        {
            
            this.InitializeComponent();
            
        }
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // page_loaded triggers whenever you navigate to/from, so check if the viewmodel has been 
            //initialized before proceeding
            if (!ViewModelInitialized)
            {
                // initialize viewmodel
                mainviewmodel.Initialize();

                ViewModelInitialized = true;

                //this is for the new design language fluent design, making the content extends into the titlebar
                ApplicationViewTitleBar formattableTitleBar = ApplicationView.GetForCurrentView().TitleBar;
                formattableTitleBar.ButtonBackgroundColor = Colors.Transparent;
                CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
                coreTitleBar.ExtendViewIntoTitleBar = true;
            }

            // Register a handler for BackRequested events and set the
            // visibility of the Back button
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                MainFrame.CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;

            MainFrame.Navigated += OnNavigated;

            MainFrame.Navigate(typeof(Gallery), mainviewmodel);

        }

        private void AcrylicBackDrop_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var panel = sender as Grid;
            ApplyPageAcrylic(panel);
        }

        Compositor PageAcrylicBackDrop;
        SpriteVisual PageHostSprite;
        private void ApplyPageAcrylic(Grid e)
        {
            PageAcrylicBackDrop = ElementCompositionPreview.GetElementVisual(this).Compositor;
            PageHostSprite = PageAcrylicBackDrop.CreateSpriteVisual();
            PageHostSprite.Size = new Vector2((float)e.ActualWidth, (float)e.ActualHeight);

            ElementCompositionPreview.SetElementChildVisual(e, PageHostSprite);
            PageHostSprite.Brush = PageAcrylicBackDrop.CreateHostBackdropBrush();

        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            // Each time a navigation event occurs, update the Back button's visibility
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                ((Frame)sender).CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
           // Frame rootFrame = Window.Current.Content as Frame;

            if (MainFrame.CanGoBack)
            {
                e.Handled = true;
                MainFrame.GoBack();
            }
        }
    }
}
