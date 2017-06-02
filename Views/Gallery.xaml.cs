using ImgurClient.ViewModels;
using Marduk.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        private object _container;

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
            
            var container = e.Container as VirtualizingViewItem;

            // used for holding the object used for connected backanimation
            _container = container;

            var root = (FrameworkElement)container.ContentTemplateRoot;
            var image = (Image)root.FindName("thumbnail");
            mainviewmodel.SelectedItem = e.Item;

            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("thumbnail", image);
            _navigated = true;

            Frame.Navigate(typeof(GalleryDetails), mainviewmodel);
        }

        private void GalleryGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if(_navigated)
            {
                _navigated = false;

                if (_container != null)
                {
                    var container = _container as VirtualizingViewItem;
                    var root = (FrameworkElement)container.ContentTemplateRoot;
                    var image = (Image)root.FindName("thumbnail");

                    // set up a new animation for navigating back
                   var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("ThumbImage");
                
                    if (animation != null)
                    {
                        // Wait for image opened. In future Insider Preview releases, this won't be necessary.
                        image.ImageOpened += (sender_, e_) =>
                        {
                            image.Opacity = 1;
                            animation.TryStart(image);
                        };
                    }
                }
                
            }
        }
    }
}
