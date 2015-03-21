using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ShareAlpha
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScaleAndCropPage : Page

        
    {

        public static RenderTargetBitmap SelectedImage { get; private set; }
        public ScaleAndCropPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            StorageFile file = e.Parameter as StorageFile;

            if (file != null)
            {
                using (Windows.Storage.Streams.IRandomAccessStream fileStream =
                         await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    // Set the image source to the selected bitmap.
                    Windows.UI.Xaml.Media.Imaging.BitmapImage bitmapImage =
                        new Windows.UI.Xaml.Media.Imaging.BitmapImage();

                    bitmapImage.SetSource(fileStream);
                    scaleChopImage.Source = bitmapImage;
                }
            }
        }

        void ContinueFileOpenPicker(Windows.ApplicationModel.Activation.FileOpenPickerContinuationEventArgs args)
        {
            throw new NotImplementedException();
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(scaleChopImageScroll);
            SelectedImage = renderTargetBitmap;

            this.Frame.Navigate(typeof(QuestionsPage));
        }
    }
}
