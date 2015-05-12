using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Email;
using Windows.Graphics.Imaging;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Facebook;
using Facebook.Client;
using Facebook.Client.Controls;
using ShareAlpha.DataModel;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ShareAlpha
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShareYourPhotoPage : Page
    {
        public ShareYourPhotoPage()
        {
            this.InitializeComponent();

            this.backgroundGrid.Background = new ImageBrush()
            {
                ImageSource = ScaleAndCropPage.SelectedImage,
                Opacity = 0.3
            };

            this.shareImage.Source = ArrangePicturePage.ArrangedImage;
            this.alphaNameTextBox.DataContext = App.Invitation;
            this.locationTextBox.DataContext = App.Invitation;
            this.dateTextBlock.DataContext = App.Invitation;
            this.timeTextBlock.DataContext = App.Invitation;
            this.shortMessageTextBox.DataContext = App.Invitation;

            this.shareImage.Source = ArrangePicturePage.ArrangedImage;
            this.alphaNameTextBlock.DataContext = App.Invitation;
            this.locationTextBlock.DataContext = App.Invitation;
            this.alphaDatePicker.DataContext = App.Invitation;
            this.alphaTimePicker.DataContext = App.Invitation;
            this.shortMessageTextBlock.DataContext = App.Invitation;

            this.inviteTextPanel.Visibility = string.IsNullOrEmpty(App.Invitation.AlphaName) ? Windows.UI.Xaml.Visibility.Collapsed: Windows.UI.Xaml.Visibility.Visible;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                if (e.Parameter is StorageFile)
                {
                    var file = e.Parameter as StorageFile;

                    using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                    {

                        await SaveScreenshotToStream(inviteGrid, stream);
                    }

                    ContentDialog fileSavedDialog = new ContentDialog()
                    {
                        Content = "Your invitation file is saved",
                        PrimaryButtonText = "OK",
                    };

                    await fileSavedDialog.ShowAsync();

                }
            }
        }


        private async System.Threading.Tasks.Task SaveScreenshotToStream(UIElement targetScrenshotElement,  IRandomAccessStream stream)
        {
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(targetScrenshotElement);

            var pixels = await renderTargetBitmap.GetPixelsAsync();
            byte[] bytes = pixels.ToArray();

            BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);

            encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                BitmapAlphaMode.Ignore,
                (uint)renderTargetBitmap.PixelWidth,
                (uint)renderTargetBitmap.PixelHeight,
                96.0, 96.0, bytes);

            await encoder.FlushAsync();
        }

        private async void shareFacebook_Click(object sender, RoutedEventArgs e)
        {

            if (Facebook.Client.Session.ActiveSession != null && !
                string.IsNullOrEmpty(Facebook.Client.Session.ActiveSession.CurrentAccessTokenData.AccessToken))
            {

                await PublishImage();
            }
            else
            {

                ContentDialog photoPublishedDialog = new ContentDialog()
                {
                    Title = "You need to sign-in on Facebook before posting any pictures",
                    Content = "Do you want to sign-in with Facebook?",
                    PrimaryButtonText = "Yes",
                    SecondaryButtonText = "No",
                };

                photoPublishedDialog.PrimaryButtonClick += photoPublishedDialog_PrimaryButtonClick;

                await photoPublishedDialog.ShowAsync();
            }
        }

        async void photoPublishedDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            await this.loginButton.RequestNewPermissions("publish_actions");
        }

        private async System.Threading.Tasks.Task PublishImage()
        {
            byte[] resultingBuffer;

            using (IRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                await SaveScreenshotToStream(inviteGrid, stream);

                resultingBuffer = new byte[stream.Size];

                await stream.ReadAsync(resultingBuffer.AsBuffer(), (uint)resultingBuffer.Length, InputStreamOptions.None);
            }

            var facebookClient = new Facebook.FacebookClient(Facebook.Client.Session.ActiveSession.CurrentAccessTokenData.AccessToken);

            var mediaObject = new FacebookMediaObject
            {
                FileName = "sharealpha.png",
                ContentType = "image/png",
            };

            mediaObject.SetValue(resultingBuffer);

            var postParams = new
            {
                message = "Share Alpha",
                source = mediaObject,
                //no_story = true,
            };

            try
            {
                dynamic fbPostTaskResult = await facebookClient.PostTaskAsync("/me/photos", postParams);
                var result = (IDictionary<string, object>)fbPostTaskResult;

                ContentDialog photoPublishedDialog = new ContentDialog()
                {
                    Title = "Share on Facebook",
                    Content = "Your photo is now posted on Facebook.",
                    PrimaryButtonText = "Ok"
                };

                await photoPublishedDialog.ShowAsync();


            }
            catch (Exception ex)
            {
                ContentDialog exceptionMessageDialog = new ContentDialog()
                {
                    Title = "Share on Facebook",
                    Content = "Exception during post: " + ex.Message,
                    PrimaryButtonText = "Ok"
                };

                exceptionMessageDialog.ShowAsync();
            }
        }


        private void savePosterButton_Click(object sender, RoutedEventArgs e)
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("PNG image", new List<string>() { ".png" });
            savePicker.DefaultFileExtension = "png";
            savePicker.SuggestedFileName = "Share Alpha";

            savePicker.PickSaveFileAndContinue();
        }


        private void CreateInvite_Click(object sender, RoutedEventArgs e)
        {
            SwitchToEditView();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchToShareView();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            App.Invitation.Clear();
            SwitchToShareView();
        }

        private void SwitchToShareView()
        {
            inviteScrollArea.Visibility = Windows.UI.Xaml.Visibility.Visible;
            inviteTextPanel.Visibility = string.IsNullOrEmpty(App.Invitation.AlphaName) ? Windows.UI.Xaml.Visibility.Collapsed : Windows.UI.Xaml.Visibility.Visible;
            shareButtonsGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;

            editGridBorder.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            invitationEditGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            inviteEditButtonsGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void SwitchToEditView()
        {
            inviteScrollArea.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            inviteTextPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            shareButtonsGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            editGridBorder.Visibility = Windows.UI.Xaml.Visibility.Visible;
            invitationEditGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
            inviteEditButtonsGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private async void sendEmail_Click(object sender, RoutedEventArgs e)
        {

#if WINDOWS_PHONE_APP

            EmailMessage email = new EmailMessage()
            {
                Subject = "Share Alpha with you"
            };

            IRandomAccessStream stream = new InMemoryRandomAccessStream();

            await SaveScreenshotToStream(inviteGrid, stream);

            var streamReference = RandomAccessStreamReference.CreateFromStream(stream);

            EmailAttachment attachment = new EmailAttachment()
            {
                FileName = "Alpha.png",
                Data = streamReference
            };

            email.Attachments.Add(attachment);

            await EmailManager.ShowComposeNewEmailAsync(email);
#endif
        }


    }


}
