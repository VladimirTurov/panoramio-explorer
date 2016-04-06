using System;
using AutoMapper;
using Caliburn.Micro;
using PanoramioSDK;

namespace PanoramioExplorer.ViewModels
{
    public class MapViewModel : Screen
    {
        private PhotoFeedViewModel _photos;

        public PhotoFeedViewModel Photos
        {
            get { return _photos; }
            private set
            {
                if (Equals(value, _photos)) return;
                _photos = value;
                NotifyOfPropertyChange();
            }
        }

        public void ChangeVisibleArea(GeoArea visibleArea)
        {
        }
    }
}