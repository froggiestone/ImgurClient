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

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var c = sender as UserControl;
            var galleryitem = c.DataContext as GalleryItem;

            if(galleryitem.is_album)
            {
               await albumviewmodel.GetAlbum(galleryitem.albumid);
            }
            else
            {
                albumviewmodel.SingleItem(galleryitem);
            }
        }
    }
}
