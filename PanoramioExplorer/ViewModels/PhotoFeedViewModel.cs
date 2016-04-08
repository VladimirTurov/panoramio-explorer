using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        private bool isBusy;

        public PhotoFeedViewModel(PanoramioClient client, GeoArea geoArea, ViewModelFactory factory)
        {
            HasMoreItems = true;

            this.client = client;
            this.geoArea = geoArea;
            this.factory = factory;
        }

        public bool HasMoreItems { get; private set; }

        public bool IsBusy
        {
            get { return isBusy; }
            private set
            {
                if (value == isBusy) return;
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsBusy)));
            }
        }

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
                IsBusy = true;
                var photos = await client.GetPhotosAsync(geoArea, new PagingInfo(currentCount, (int)(currentCount + requestedCount)));
                foreach (var photo in photos)
                    Add(factory.CreatePhotoViewModel(photo));
            }
            catch
            {
                ItemsLoadingError?.Invoke(this, EventArgs.Empty);
            }

            var addedCount = Count - currentCount;
            HasMoreItems = addedCount != 0;
            IsBusy = false;
            return new LoadMoreItemsResult { Count = (uint) addedCount };
        }
    }
}