using System;

namespace PanoramioExplorer.Services
{
  public interface IPhotoSharingService
  {
      void Share(Uri imageSource, string description);
  }
}
