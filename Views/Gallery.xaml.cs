using ImgurClient.DataModels;
using ImgurClient.ViewModels;
using Marduk.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace ImgurClient.Views
{
    public sealed partial class Gallery : Page
    {
        public MainViewModel mainviewmodel { get; set; }

        public Gallery()
        {
            // require the page to be catched, so it doesn't reload when navigating to/from
            NavigationCacheMode = NavigationCacheMode.Required;

            this.InitializeComponent();
        }

        private bool PageLoaded = false;
        private bool _navigated = false;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(mainviewmodel == null)
            {
                base.OnNavigatedTo(e);

                // viewmodel passed from previous page
                mainviewmodel = e.Parameter as MainViewModel;
                DataContext = mainviewmodel;
            }
           
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(!PageLoaded)
            {
                loadingring.IsActive = true;
                bool success = await mainviewmodel.SerializeSection("hot", "viral");
                loadingring.IsActive = false;

                PageLoaded = true;
            }

           
        }
        
        private void GalleryGrid_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item;
            var container = e.Container as VirtualizingViewItem;

            var root = (FrameworkElement)container.ContentTemplateRoot;
            var image = (Image)root.FindName("thumbnail");
            mainviewmodel.SelectedItem = item;

            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("thumbnail", image);
            Globals.NavigatedTo = item;
            _navigated = true;

            Frame.Navigate(typeof(GalleryDetails), mainviewmodel);

        }

        private void GalleryGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if(_navigated)
            {
                _navigated = false;

                var animationService = ConnectedAnimationService.GetForCurrentView();
                var animation = animationService.GetAnimation("ThumbImage");

                if (animation != null)
                {
                    var item = Globals.NavigatedTo as GalleryItem;
/*
                    GalleryGrid.ScrollIntoView(item);

                    var container = GalleryGrid.ContainerFromItem(Globals.NavigatedTo) as GridViewItem;
                    if (container != null)
                    {
                        var root = (FrameworkElement)container.ContentTemplateRoot;
                        var image = (Image)root.FindName("thumbnail");

                        await GalleryGrid.TryStartConnectedAnimationAsync(animation, Globals.NavigatedTo, "thumbnail");
                        */
                    }
                    else
                    {
                        animation.Cancel();
                    }
                }

            }
        }

       
    }
