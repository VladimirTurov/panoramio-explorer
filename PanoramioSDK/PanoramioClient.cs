using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PanoramioSDK
{
    public class PanoramioClient
    {
        private readonly HttpClient httpClient;

        public PanoramioClient()
        {
            httpClient = new HttpClient();
        }

        public async Task<IEnumerable<Photo>> GetPhotosAsync(GeoArea geoArea, PagingInfo paging)
        {
            if (geoArea == null) throw new ArgumentNullException(nameof(geoArea));
            if (paging == null) throw new ArgumentNullException(nameof(paging));

            var uri = ComposeUri(geoArea, paging);
            var response = await httpClient.GetStringAsync(uri);
            var jsonObject = JObject.Parse(response);
            var photosField = jsonObject["photos"];
            var photos = photosField.ToObject<IEnumerable<Photo>>();

            return photos;
        }

        private Uri ComposeUri(GeoArea geoArea, PagingInfo paging)
        {
            const string baseUri = "http://www.panoramio.com/map/get_panoramas.php?set=public&size=medium&";
            var parameters = 
                $"from={paging.From}&to={paging.To}" +
                $"&minx={geoArea.MinimalLongitude.ToString(CultureInfo.InvariantCulture)}" +
                $"&miny={geoArea.MinimalLatitude.ToString(CultureInfo.InvariantCulture)}" +
                $"&maxx={geoArea.MaximalLongitude.ToString(CultureInfo.InvariantCulture)}" +
                $"&maxy={geoArea.MaximalLatitude.ToString(CultureInfo.InvariantCulture)}";

            return new Uri(baseUri + parameters);
        }
    }
}