using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using PanoramioExplorer.Commands;
using PanoramioSDK;

namespace PanoramioExplorer.ViewModels
{
    public class MapViewModel : Screen
    {
        private readonly ViewModelFactory factory;

        private PhotoFeedViewModel photos;
        private bool isGalleryModeEnabled;
        private PhotoViewModel galleryPhoto;

        private CancellationTokenSource cts;

        public MapViewModel(ViewModelFactory factory)
        {
            this.factory = factory;

            ShowInGalleryModeCommand = new SimpleCommand<PhotoViewModel>(ShowInGalleryMode);
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

        public bool IsGalleryModeEnabled
        {
            get { return isGalleryModeEnabled; }
            private set
            {
                if (value == isGalleryModeEnabled) return;
                isGalleryModeEnabled = value;
                NotifyOfPropertyChange();
            }
        }

        public PhotoViewModel GalleryPhoto
        {
            get { return galleryPhoto; }
            private set
            {
                if (Equals(value, galleryPhoto)) return;
                galleryPhoto = value;
                NotifyOfPropertyChange();
            }
        }

        public ICommand ShowInGalleryModeCommand { get; private set; }

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

        private void ShowInGalleryMode(PhotoViewModel photo)
        {
            IsGalleryModeEnabled = photo != null;
            GalleryPhoto = photo;
        }
    }
}