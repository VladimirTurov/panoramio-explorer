using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using PanoramioSDK;

namespace PanoramioExplorer.ViewModels
{
    public class MapViewModel : Screen
    {
        private readonly ViewModelFactory factory;
        private PhotoFeedViewModel photos;
        private CancellationTokenSource cts;

        public MapViewModel(ViewModelFactory factory)
        {
            this.factory = factory;
        }

        public PhotoFeedViewModel Photos
        {
            get { return photos; }
            private set
            {
                if (Equals(value, photos)) return;
                photos = value;
                NotifyOfPropertyChange();
            }
        }

        public async void ChangeVisibleArea(GeoArea visibleArea)
        {
            try
            {
                cts?.Cancel();
                cts = new CancellationTokenSource();
                await Task.Delay(500, cts.Token);
            }
            catch (TaskCanceledException)
            {
                // throttling ftw
                return;
            }

            Photos = factory.CreatePhotoFeedViewModel(visibleArea);
        }
    }
}