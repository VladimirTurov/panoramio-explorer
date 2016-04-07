using System;
using Newtonsoft.Json;

namespace PanoramioSDK
{
    public class Photo
    {
        public Photo(string title, Uri source)
        {
            Title = title;
            Source = source;
        }

        [JsonProperty(PropertyName = "photo_title")]
        public string Title { get; private set; }
        [JsonProperty(PropertyName = "photo_file_url")]
        public Uri Source { get; private set; }
    }
}