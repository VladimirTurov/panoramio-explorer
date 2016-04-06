using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace PanoramioExplorer.Controls
{
    [TemplatePart(Name = ImageName, Type = typeof(Image))]
    [TemplatePart(Name = LoaderName, Type = typeof(ProgressRing))]
    public class PhotoImage : Control
    {
        #region Dependency properties boilerplate

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(Uri), typeof(PhotoImage),
                                        new PropertyMetadata(default(Uri), OnSourceChanged));

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(PhotoImage),
                                        new PropertyMetadata(default(string)));

        private static void OnSourceChanged(DependencyObject target,
                                            DependencyPropertyChangedEventArgs e)
        {
            (target as PhotoImage).UpdateSource(e.NewValue as Uri);
        }

        #endregion

        private const string ImageName = "Image_PART";
        private const string LoaderName = "Loader_PART";

        private Image image;
        private ProgressRing loader;

        public PhotoImage()
        {
            DefaultStyleKey = typeof(PhotoImage);
        }

        public Uri ImageSource
        {
            get { return (Uri)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        protected override void OnApplyTemplate()
        {
            image = GetTemplateChild(ImageName) as Image;
            loader = GetTemplateChild(LoaderName) as ProgressRing;

            if (image == null || loader == null)
                throw new InvalidOperationException("Template is corrupted, loader and/or image not found");

            base.OnApplyTemplate();
        }

        private void UpdateSource(Uri newSource)
        {
            image.Source = null;

            if (newSource == null)
                return;

            var bitmap = new BitmapImage(newSource);
            bitmap.ImageOpened += OnBitmapLoaded;
            bitmap.ImageFailed += OnBitmapLoadingFailed;

            image.Source = bitmap;
            loader.IsActive = true;
        }

        private void OnBitmapLoaded(object sender, RoutedEventArgs e)
        {
            (sender as BitmapImage).ImageOpened -= OnBitmapLoaded;

            loader.IsActive = false;
        }

        private void OnBitmapLoadingFailed(object sender, ExceptionRoutedEventArgs e)
        {
            (sender as BitmapImage).ImageFailed -= OnBitmapLoadingFailed;

            loader.IsActive = false;
        }
    }
}
