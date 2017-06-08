using ImgurClient.DataModels;
using ImgurClient.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;

namespace ImgurClient.ViewModels
{
   public class MainViewModel :NotificationBase
    {
        public MainViewModel()
        {

        }

        public ObservableCollection<GalleryItem> GalleryItems = new ObservableCollection<GalleryItem>();
        
        private object _SelectedItem;
        public object SelectedItem
        {
            get { return _SelectedItem; }
            set { SetProperty(_SelectedItem, value, () => _SelectedItem = value); }
        }

        public void Initialize()
        {
            // load local data and user info
        }

        public async Task<bool> SerializeSection(string section, string sort)
        {
            try
            {
                // https://api.imgur.com/3/gallery/{section}/{sort}/{page}?showViral=bool
                string url = Config.Endpoint + "gallery/" + section + "/" + sort + "/1";
                
                string jsonMessage = await HttpRequestAsync(url);

                if (!string.IsNullOrEmpty(jsonMessage))
                {
                   var serializer = new DataContractJsonSerializer(typeof(GalleryDataModel));
                   var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonMessage));
                   var response = (GalleryDataModel)serializer.ReadObject(ms);
                    
                    ms.Dispose();
                    serializer = null;

                    foreach (var item in response.data)
                    {
                        /*
                               s	Small Square	90x90	No
                               b	Big Square	160x160	No
                               t	Small Thumbnail	160x160	Yes
                               m	Medium Thumbnail	320x320	Yes
                               l	Large Thumbnail	640x640	Yes
                               h	Huge Thumbnail	1024x1024	Yes
                            */

                        // build the thumbnail

                        double thumbwidth = Window.Current.Bounds.Width / 5;

                        if (item.is_album)
                        {
                            item.albumid = item.id;
                            item.id = item.cover;
                            
                            // original height / original width * new width = new height
                            item.thumbnail_height = (item.cover_height / item.cover_width) * thumbwidth;
                            item.thumbnail_width = thumbwidth;

                           
                        }
                        else
                        {
                            item.thumbnail_height = (item.height / item.width) * thumbwidth;
                            item.thumbnail_width = thumbwidth;
                        }

                        if (item.thumbnail_height == 0)
                        {
                            item.thumbnail_height = thumbwidth;
                        }


                        else if (item.thumbnail_height > 500)
                        {
                            item.thumbnail_height = 500;
                        }
                        // use the huge thumbnail, so we can make the connected animation smoother
                        item.thumbnail = "http://i.imgur.com/" + item.id + "l.jpg";
                        
                        GalleryItems.Add(item);
                    }

                    if(Config.EnableCache)
                        await SaveGalleryCache();

                    return true;
                }

                else
                {
                    
                    return false;
                }
            }

            catch (Exception) { return false; }
        }

        private async Task<string> HttpRequestAsync(string url)
        {
            try
            {
                var AuthHeaderName = "Client-ID";
                var AuthHeaderValue = Config.Client_ID;
                
                if (!string.IsNullOrEmpty(Config.UserToken))
                {
                    AuthHeaderName = "Bearer";
                    AuthHeaderValue = Config.UserToken;
                }

                HttpClient http = new HttpClient();
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthHeaderName, AuthHeaderValue);
                var timeout = new TimeSpan(0, 0, 5);
                http.Timeout = timeout;
                var response = await http.GetAsync(url);

                string jsonmessage = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(jsonmessage))
                {
                    http.Dispose();
                    return jsonmessage;
                }
                else
                {
                    http.Dispose();
                    return null;
                }

            }
            catch (Exception)
            {
                // assume no connection, so load the cache
                await LoadGalleryCache();
                return null;
            }

        }

        public async Task<bool> SaveGalleryCache()
        {
            var filename = "gallery.json";

            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);

                IRandomAccessStream raStream = await file.OpenAsync(FileAccessMode.ReadWrite);

                using (IOutputStream outStream = raStream.GetOutputStreamAt(0))
                {
                    // Serialize the Session State. 
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<GalleryItem>));

                    serializer.WriteObject(outStream.AsStreamForWrite(), GalleryItems);

                    await outStream.FlushAsync();
                    outStream.Dispose();
                    raStream.Dispose();

                    Debug.WriteLine("gallery cache saved");
                    return true;
                }
            }
            catch (Exception) { Debug.WriteLine("gallery cahce failed to save"); return false; }
        }

        // Loads the movie database
        public async Task<bool> LoadGalleryCache()
        {
            var filename = "gallery.json";

            try
            {
                var Serializer = new DataContractJsonSerializer(typeof(List<GalleryItem>));
                var filecheck = await ApplicationData.Current.LocalFolder.TryGetItemAsync(filename);

                if (filecheck != null)
                {
                    var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(filename);

                    if (stream.Length > 0)
                    {
                        var gallerycatche = (List<GalleryItem>)Serializer.ReadObject(stream);

                        foreach (var galleryitem in gallerycatche)
                        {
                            GalleryItems.Add(galleryitem);
                        }

                        stream.Dispose();
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }

            }
            catch (Exception) { Debug.WriteLine("Failed loading achievement data"); return false; }

        }

    }
}
