using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgurClient.DataModels
{
    public class DescriptionAnnotations
    {
    }

    public class Tag
    {
        public string name { get; set; }
        public string display_name { get; set; }
        public int followers { get; set; }
        public int total_items { get; set; }
        public bool following { get; set; }
        public string background_hash { get; set; }
        public bool background_is_animated { get; set; }
        public bool is_promoted { get; set; }
        public string description { get; set; }
        public object logo_hash { get; set; }
        public object logo_destination_url { get; set; }
        public DescriptionAnnotations description_annotations { get; set; }
    }

    public class AlbumItem
    {
        public string id { get; set; }
        public object title { get; set; }
        public string description { get; set; }
        public int datetime { get; set; }
        public string type { get; set; }
        public bool animated { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int size { get; set; }
        public int views { get; set; }
        public long bandwidth { get; set; }
        public object vote { get; set; }
        public bool favorite { get; set; }
        public object nsfw { get; set; }
        public string mp4 { get; set; }
        public object section { get; set; }
        public object account_url { get; set; }
        public object account_id { get; set; }
        public bool is_ad { get; set; }
        public bool in_most_viral { get; set; }
        public List<object> tags { get; set; }
        public int ad_type { get; set; }
        public string ad_url { get; set; }
        public bool in_gallery { get; set; }
        public string link { get; set; }
        public object comment_count { get; set; }
        public object ups { get; set; }
        public object downs { get; set; }
        public object points { get; set; }
        public object score { get; set; }
    }

    public class Album
    {
        public string id { get; set; }
        public string title { get; set; }
        public object description { get; set; }
        public int datetime { get; set; }
        public string cover { get; set; }
        public int cover_width { get; set; }
        public int cover_height { get; set; }
        public string account_url { get; set; }
        public int account_id { get; set; }
        public string privacy { get; set; }
        public string layout { get; set; }
        public int views { get; set; }
        public string link { get; set; }
        public int ups { get; set; }
        public int downs { get; set; }
        public int points { get; set; }
        public int score { get; set; }
        public bool is_album { get; set; }
        public object vote { get; set; }
        public bool favorite { get; set; }
        public bool nsfw { get; set; }
        public string section { get; set; }
        public int comment_count { get; set; }
        public string topic { get; set; }
        public int topic_id { get; set; }
        public int images_count { get; set; }
        public bool in_gallery { get; set; }
        public bool is_ad { get; set; }
        public List<Tag> tags { get; set; }
        public int ad_type { get; set; }
        public string ad_url { get; set; }
        public bool in_most_viral { get; set; }
        public List<AlbumItem> images { get; set; }
    }

    public class AlbumDataModel
    {
        public Album data { get; set; }
        public bool success { get; set; }
        public int status { get; set; }
    }
}
