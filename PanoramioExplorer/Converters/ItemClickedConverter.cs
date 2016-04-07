using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace PanoramioExplorer.Converters
{
    public class ItemClickedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var eventArgs = value as ItemClickEventArgs;
            return eventArgs?.ClickedItem;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
