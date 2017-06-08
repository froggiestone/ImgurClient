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
using Windows.UI.Xaml;

namespace ImgurClient.ViewModels
{
    public class AlbumViewModel : NotificationBase
    {
        public ObservableCollection<AlbumItem> Album = new ObservableCollection<AlbumItem>();

        public AlbumViewModel()
        {

        }

        private string _AlbumTitle;
        public string AlbumTitle
        {
            get { return _AlbumTitle; }
            set { SetProperty(_AlbumTitle, value, () => _AlbumTitle = value); }
        }

        public void SingleItem(GalleryItem galleryitem)
        {
            Album.Clear();

            var item = new AlbumItem();
            item.title = galleryitem.title;
            item.link = galleryitem.link;
            item.mp4 = galleryitem.mp4;
            item.width = galleryitem.width;

            Album.Add(item);
        }

        public async Task<bool> GetAlbum(string id)
        {
            try
            {
                // Album = new ObservableCollection<AlbumItem>();
                Album.Clear();
                // https://api.imgur.com/3/gallery/album/{id}
                string url = Config.Endpoint + "gallery/album/" + id;

                string jsonMessage = await HttpRequestAsync(url);

                if (!string.IsNullOrEmpty(jsonMessage))
                {
                    var serializer = new DataContractJsonSerializer(typeof(AlbumDataModel));
                    var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonMessage));
                    var response = (AlbumDataModel)serializer.ReadObject(ms);

                    ms.Dispose();
                    serializer = null;

                    AlbumTitle = response.data.title;

                    foreach (var item in response.data.images)
                    {
                        Album.Add(item);
                    }

                    Debug.WriteLine("Album loaded: " + Album.Count);
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
                return null;
            }

        }

    }
}
