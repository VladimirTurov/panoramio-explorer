using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using PanoramioExplorer.Commands;
using PanoramioExplorer.Services;
using PanoramioSDK;

namespace PanoramioExplorer.ViewModels
{
    public class MapViewModel : Screen
    {
        private readonly ViewModelFactory factory;
        private readonly IPhotoSharingService sharingService;
        private readonly IFileSavingService fileSavingService;

        private GeoArea currentArea;

        private PhotoFeedViewModel photos;
        private bool isGalleryModeEnabled;
        private PhotoViewModel galleryPhoto;
        private string errorDetails;
        private bool isErrorShown;

        private CancellationTokenSource cts;

        public MapViewModel(ViewModelFactory factory,
                            IPhotoSharingService sharingService,
                            IFileSavingService fileSavingService)
        {
            this.factory = factory;
            this.sharingService = sharingService;
            this.fileSavingService = fileSavingService;

            ShowInGalleryModeCommand = new SimpleCommand<PhotoViewModel>(ShowInGalleryMode);
            ExitGalleryModeCommand = new SimpleCommand<object>(ExitGalleryMode);

            ShareCommand = new SimpleCommand<PhotoViewModel>(Share);
            SaveCommand = new SimpleCommand<PhotoViewModel>(Save);

            RetryCommand = new SimpleCommand<object>(Retry);
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

        public string ErrorDetails
        {
            get { return errorDetails; }
            private set
            {
                if (value == errorDetails) return;
                errorDetails = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsErrorShown
        {
            get { return isErrorShown; }
            private set
            {
                if (value == isErrorShown) return;
                isErrorShown = value;
                NotifyOfPropertyChange();
            }
        }

        public ICommand ShowInGalleryModeCommand { get; private set; }
        public ICommand ExitGalleryModeCommand { get; private set; }

        public ICommand ShareCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }

        public ICommand RetryCommand { get; private set; }

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

            if (Photos != null) Photos.ItemsLoadingError -= OnItemsLoadingError;
            Photos = factory.CreatePhotoFeedViewModel(visibleArea);
            if (Photos != null) Photos.ItemsLoadingError += OnItemsLoadingError;

            currentArea = visibleArea;
        }

        private void ShowInGalleryMode(PhotoViewModel photo)
        {
            IsGalleryModeEnabled = photo != null;
            GalleryPhoto = photo;
        }

        private void ExitGalleryMode(object parameter)
        {
            IsGalleryModeEnabled = false;
            GalleryPhoto = null;
        }

        private void Share(PhotoViewModel photo)
        {
            sharingService.Share(photo.Source, photo.Title);
        }

        private async void Save(PhotoViewModel photo)
        {
            await fileSavingService.SaveImageAsync(photo.Source);
        }

        private void OnItemsLoadingError(object sender, EventArgs e)
        {
            IsErrorShown = true;
            ErrorDetails = "Оу, все сломалось!"
                + Environment.NewLine
                + Environment.NewLine
                + "В следующий раз постарайся скроллить не так быстро, "
                + "и убедись, что присутствует соединение к сети";
        }

        private void Retry(object parameter)
        {
            IsErrorShown = false;
            ErrorDetails = null;

            ChangeVisibleArea(currentArea);
        }
    }
}