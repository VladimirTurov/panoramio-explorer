using System;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;
using PanoramioExplorer.ViewModels;
using PanoramioSDK;

namespace PanoramioExplorer.Views
{
    public sealed partial class MapView
    {
        public MapView()
        {
            InitializeComponent();

            Map.Center = new Geopoint(new BasicGeoposition { Latitude = 51.564355278387666, Longitude = 32.751946086063981 });
        }

        private void OnMapCameraMoved(MapControl map, object e)
        {
            var visibleArea = CalculateVisibleGeoArea(map);
            (DataContext as MapViewModel).ChangeVisibleArea(visibleArea);
        }

        private static GeoArea CalculateVisibleGeoArea(MapControl map)
        {
            Geopoint topLeft;
            Geopoint bottomRight;

            map.GetLocationFromOffset(new Point(0, 0), out topLeft);
            map.GetLocationFromOffset(new Point(map.RenderSize.Width, map.RenderSize.Height), out bottomRight);

            var leftPoint = topLeft.Position;
            var rightPoint = bottomRight.Position;

            var minLongitude = Math.Min(leftPoint.Longitude, rightPoint.Longitude);
            var maxLongitude = Math.Max(leftPoint.Longitude, rightPoint.Longitude);
            var minLatitude = Math.Min(leftPoint.Latitude, rightPoint.Latitude);
            var maxLatitude = Math.Max(leftPoint.Latitude, rightPoint.Latitude);

            return new GeoArea(minLongitude, minLatitude,
                               maxLongitude, maxLatitude);
        }

        private void MenuToggleButtonTapped(object sender, RoutedEventArgs e)
        {
            SplitView.IsPaneOpen = !SplitView.IsPaneOpen;
        }
    }
}