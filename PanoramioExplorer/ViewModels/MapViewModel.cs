using System;
using AutoMapper;
using Caliburn.Micro;
using PanoramioSDK;

namespace PanoramioExplorer.ViewModels
{
    public class MapViewModel : Screen
    {
        private readonly ViewModelFactory factory;
        private PhotoFeedViewModel photos;

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

        public void ChangeVisibleArea(GeoArea visibleArea)
        {
            Photos = factory.CreatePhotoFeedViewModel(visibleArea);
        }
    }
}