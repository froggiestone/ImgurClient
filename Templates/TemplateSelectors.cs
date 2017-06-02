using ImgurClient.DataModels;
using ImgurClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ImgurClient.Templates
{
    public class AlbumListViewTemplateSelecter : DataTemplateSelector
    {

        public DataTemplate Album { get; set; }
        public DataTemplate Image { get; set; }
        public DataTemplate Video { get; set; }


        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var galleryitem = item as GalleryItem;

            if(galleryitem.is_album)
            {
                return Album;
            }

            else if (string.IsNullOrEmpty(galleryitem.mp4))
            {
                return Image;
            }

           else if(!string.IsNullOrEmpty(galleryitem.mp4))
            {
                return Video;
            }

            return base.SelectTemplateCore(item, container);
        }


    }



    


}
