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
using Windows.UI.Xaml.Shapes;

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
            Globals.NavigatedTo = GalleryFlipView.SelectedItem;

            if (container != null)
            {
                var root = (FrameworkElement)container.ContentTemplateRoot;
                var image = (Image)root.FindName("Cover");

                if(image != null)
                {
                    // set up a new animation for navigating back
                    ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ThumbImage", image);
                }
                
            }
            
        }

        private void GalleryFlipView_Loaded(object sender, RoutedEventArgs e)
        {
            GalleryFlipView.SelectedItem = mainviewmodel.SelectedItem as GalleryItem;

        }
    }
}
 