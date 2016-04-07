using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace PanoramioExplorer.Services
{
    public class FileSavingService : IFileSavingService
    {
        public async Task<SavingOperationResult> SaveImageAsync(Uri imageSource)
        {
            var filename = imageSource.Segments.Last();
            var fileType = new FileInfo(filename).Extension;
            var filePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                SuggestedFileName = filename
            };

            filePicker.FileTypeChoices.Add("Изображение", new[] { fileType });

            try
            {
                var storageFile = await filePicker.PickSaveFileAsync();
                if (storageFile == null)
                    return SavingOperationResult.DissmissedByUser;

                using (var stream = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var imageStreamReference = RandomAccessStreamReference.CreateFromUri(imageSource);
                    using (var imageStream = await imageStreamReference.OpenReadAsync())
                    {
                        await RandomAccessStream.CopyAsync(imageStream, stream);
                    }
                }

                return SavingOperationResult.Succeed;
            }
            catch
            {
                return SavingOperationResult.Failed;
            }
        }
    }
}