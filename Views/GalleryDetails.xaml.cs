using ImgurClient.DataModels;
using ImgurClient.ViewModels;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace ImgurClient.Views
{
    public sealed partial class GalleryDetails : Page
    {
        public MainViewModel mainviewmodel { get; set; }

        public GalleryDetails()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // viewmodel passed from previous page
            mainviewmodel = e.Parameter as MainViewModel;

            
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            var container = GalleryFlipView.ContainerFromItem(GalleryFlipView.SelectedItem) as FlipViewItem;
            if(container != null)
            {
                var root = (FrameworkElement)container.ContentTemplateRoot;
                var image = (Image)root.FindName("DetailImage");

                if(image != null)
                {
                    // set up a new animation for navigating back
                    ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ThumbImage", image);
                }
                
            }
            
        }

        private async void GalleryFlipView_Loaded(object sender, RoutedEventArgs e)
        {
            GalleryFlipView.SelectedItem = mainviewmodel.SelectedItem as GalleryItem;

            var galleryitem = mainviewmodel.SelectedItem as GalleryItem;

            await Task.Delay(50);

            var container = GalleryFlipView.ContainerFromItem(GalleryFlipView.SelectedItem) as FlipViewItem;
            if (container != null)
            {
                bool isplaceholder = false;
                var root = (FrameworkElement)container.ContentTemplateRoot;
                Image image = new Image();

                if(galleryitem.is_album)
                {
                   
                }
                else if(!string.IsNullOrEmpty(galleryitem.mp4))
                {
                    isplaceholder = true;
                    image = (Image)root.FindName("PlaceholderImage");

                    // set placeholder source
                    image.Source = new BitmapImage(new Uri("http://i.imgur.com/" + galleryitem.id + "h.jpg"));
                }
                else
                {
                    image = (Image)root.FindName("DetailImage");

                }

                //  var imagesource = "http://i.imgur.com/" + SelectedItem.id + ".jpg";

                // image.Source = new BitmapImage(new Uri(imagesource));

                // grab the connected animation
                var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("thumbnail");

                if (animation != null)
                {
                    // Wait for image opened. In future Insider Preview releases, this won't be necessary.
                    image.ImageOpened += (sender_, e_) =>
                    {
                        image.Opacity = 1;
                        animation.TryStart(image);

                        if (isplaceholder)
                        {
                            // TODO: clear the placeholder from the templatefile
                           // await Task.Delay(300);
                           // image.Source = null;
                        }
                    };
                }
            }
        }
    }
}
