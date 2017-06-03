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

        public DataTemplate Image { get; set; }
        public DataTemplate Video { get; set; }


        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var albumitem = item as AlbumItem;
            
            if(string.IsNullOrEmpty(albumitem.mp4))
            {
                return Image;
            }

           else
            {
                return Video;
            }

           // return base.SelectTemplateCore(item, container);
        }


    }



    


}
