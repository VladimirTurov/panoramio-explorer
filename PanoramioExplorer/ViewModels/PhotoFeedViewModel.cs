using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using PanoramioSDK;

namespace PanoramioExplorer.ViewModels
{
    public class PhotoFeedViewModel : ObservableCollection<PhotoViewModel>,
                                      ISupportIncrementalLoading
    {
        private readonly PanoramioClient client;
        private readonly GeoArea geoArea;
        private readonly ViewModelFactory factory;

        public PhotoFeedViewModel(PanoramioClient client, GeoArea geoArea, ViewModelFactory factory)
        {
            HasMoreItems = true;

            this.client = client;
            this.geoArea = geoArea;
            this.factory = factory;
        }

        public bool HasMoreItems { get; private set; }

        public event EventHandler ItemsLoadingError;

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return FetchMoreItemsAsync(count).AsAsyncOperation();
        }

        private async Task<LoadMoreItemsResult> FetchMoreItemsAsync(uint requestedCount)
        {
            if (requestedCount < 10)
                requestedCount = 10;

            var currentCount = Count;
            try
            {
                var photos = await client.GetPhotosAsync(geoArea, new PagingInfo(currentCount, (int)(currentCount + requestedCount)));
                foreach (var photo in photos)
                    Add(factory.CreatePhotoViewModel(photo));
            }
            catch
            {
                ItemsLoadingError?.Invoke(this, EventArgs.Empty);
            }

            HasMoreItems = currentCount != Count;
            return new LoadMoreItemsResult { Count = requestedCount };
        }
    }
}