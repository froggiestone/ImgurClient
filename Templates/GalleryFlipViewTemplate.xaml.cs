using ImgurClient.DataModels;
using ImgurClient.ViewModels;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace ImgurClient.Templates
{
    public sealed partial class GalleryFlipViewTemplate : UserControl
    {
        public AlbumViewModel albumviewmodel { get; set; }

        public GalleryFlipViewTemplate()
        {

            albumviewmodel = new AlbumViewModel();

            this.InitializeComponent();
        }

        private bool _loaded = false;

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(!_loaded)
            {
                var c = sender as UserControl;
                var galleryitem = c.DataContext as GalleryItem;

                if (galleryitem.is_album)
                {
                    await albumviewmodel.GetAlbum(galleryitem.albumid);
                }
                else
                {
                    albumviewmodel.SingleItem(galleryitem);
                }

                // since there are multiple instances running in the flipview, check if it match the item we clicked, then run the animation
                if (Globals.NavigatedTo == galleryitem)
                {
                    // set placeholder source
                    Cover.Source = new BitmapImage(new Uri(galleryitem.thumbnail));

                    // grab the connected animation
                    var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("thumbnail");

                    if (animation != null)
                    {
                        // Wait for image opened. In future Insider Preview releases, this won't be necessary.
                        Cover.ImageOpened += (sender_, e_) =>
                        {
                            Cover.Opacity = 1;
                            animation.TryStart(Cover);


                        };
                    }
                }

                _loaded = true;
            }
            
        }

        private async void UserControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if(_loaded)
            {
                Cover.Source = null;

                var c = sender as UserControl;
                var galleryitem = c.DataContext as GalleryItem;

                Cover.Source = new BitmapImage(new Uri(galleryitem.thumbnail));

                if (galleryitem.is_album)
                {
                    await albumviewmodel.GetAlbum(galleryitem.albumid);
                }
                else
                {
                    albumviewmodel.SingleItem(galleryitem);
                }
            }
            
        }

        private void DetailVideo_MediaOpened(object sender, RoutedEventArgs e)
        {

        }
    }
}
