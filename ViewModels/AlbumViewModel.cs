using ImgurClient.DataModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class AlbumViewModel
    {
        public ObservableCollection<AlbumItem> Album = new ObservableCollection<AlbumItem>();

        public AlbumViewModel()
        {

        }

        public void SingleItem(GalleryItem galleryitem)
        {

        }

        public async Task<bool> GetAlbum(string id)
        {
            try
            {

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
