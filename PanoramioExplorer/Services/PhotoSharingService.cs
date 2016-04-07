using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage.Streams;

namespace PanoramioExplorer.Services
{
    public class PhotoSharingService : IPhotoSharingService
    {
        public void Share(Uri imageSource, string description)
        {
            var dataTransferManager = DataTransferManager.GetForCurrentView();

            TypedEventHandler<DataTransferManager, DataRequestedEventArgs> shareDelegate = null;
            shareDelegate = delegate (DataTransferManager sender, DataRequestedEventArgs e)
            {
                dataTransferManager.DataRequested -= shareDelegate;
                var dataContainer = e.Request.Data;
                var imageStreamReference = RandomAccessStreamReference.CreateFromUri(imageSource);

                dataContainer.SetBitmap(imageStreamReference);
                dataContainer.Properties.Title = description;
            };

            dataTransferManager.DataRequested += shareDelegate;
            DataTransferManager.ShowShareUI();
        }
    }
}
