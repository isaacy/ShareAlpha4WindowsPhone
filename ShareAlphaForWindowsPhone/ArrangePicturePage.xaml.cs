using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Text;
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
    public sealed partial class ArrangePicturePage : Page
    {
        private Point pressedPoint;
        private double pressedSenderX;
        private double pressedSenderY;
        private object currentTargetSender;

        private TextBlock firstQuestionTextBlock;
        private TextBlock secondQuestionTextBlock;
        private TextBlock thirdQuestionTextBlock;

        private static SolidColorBrush blackBrush = new SolidColorBrush(Windows.UI.Colors.Black);
        private static SolidColorBrush whiteBrush = new SolidColorBrush(Windows.UI.Colors.White);


        public ArrangePicturePage()
        {
            this.InitializeComponent();

            arrangePictureImageBrush.ImageSource = ScaleAndCropPage.SelectedImage;

            firstQuestionTextBlock = new TextBlock()
            {
                Text = QuestionsPage.FirstQuestion,
                FontSize = 24,
                FontFamily = new FontFamily("Arial Black"),
                FontWeight = FontWeights.ExtraBold,
            };

            secondQuestionTextBlock = new TextBlock()
            {
                Text = QuestionsPage.SecondQuestion,
                FontSize = 24,
                FontFamily = new FontFamily("Arial Black"),
                FontWeight = FontWeights.ExtraBold,
            };

            thirdQuestionTextBlock = new TextBlock()
            {
                Text = QuestionsPage.ThirdQuestion,
                FontSize = 24,
                FontFamily = new FontFamily("Arial Black"),
                FontWeight = FontWeights.ExtraBold,
            };

            firstQuestionTextBlock.SetValue(Canvas.LeftProperty, "30");
            firstQuestionTextBlock.SetValue(Canvas.TopProperty, "10");
            firstQuestionTextBlock.PointerPressed += firstQuestionTextBlock_PointerPressed;
            firstQuestionTextBlock.PointerReleased += firstQuestionTextBlock_PointerReleased;
            firstQuestionTextBlock.PointerMoved += TextBlock_PointerMoved;
            firstQuestionTextBlock.PointerCanceled += firstQuestionTextBlock_PointerCanceled;
            firstQuestionTextBlock.PointerCaptureLost += firstQuestionTextBlock_PointerCaptureLost;

            secondQuestionTextBlock.SetValue(Canvas.LeftProperty, "30");
            secondQuestionTextBlock.SetValue(Canvas.TopProperty, "200");
            secondQuestionTextBlock.PointerMoved += TextBlock_PointerMoved;
            secondQuestionTextBlock.PointerPressed += firstQuestionTextBlock_PointerPressed;
            secondQuestionTextBlock.PointerReleased += firstQuestionTextBlock_PointerReleased;
            secondQuestionTextBlock.PointerCanceled += firstQuestionTextBlock_PointerCanceled;
            secondQuestionTextBlock.PointerCaptureLost += firstQuestionTextBlock_PointerCaptureLost;
            

            thirdQuestionTextBlock.SetValue(Canvas.LeftProperty, "30");
            thirdQuestionTextBlock.SetValue(Canvas.TopProperty, "300");
            thirdQuestionTextBlock.PointerMoved += TextBlock_PointerMoved;
            thirdQuestionTextBlock.PointerPressed += firstQuestionTextBlock_PointerPressed;
            thirdQuestionTextBlock.PointerReleased += firstQuestionTextBlock_PointerReleased;
            thirdQuestionTextBlock.PointerCanceled += firstQuestionTextBlock_PointerCanceled;
            thirdQuestionTextBlock.PointerCaptureLost += firstQuestionTextBlock_PointerCaptureLost;
            
            arrangePictureCanvas.Children.Add(firstQuestionTextBlock);
            arrangePictureCanvas.Children.Add(secondQuestionTextBlock);
            arrangePictureCanvas.Children.Add(thirdQuestionTextBlock);
        }

        void firstQuestionTextBlock_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            ClearTargetSender();
        }

        void firstQuestionTextBlock_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            ClearTargetSender();
        }


        public static RenderTargetBitmap ArrangedImage{ get; private set; }

        
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {


        }

        void firstQuestionTextBlock_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            pointerPressed(sender, e);
        }

        void firstQuestionTextBlock_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            pointerReleased(sender, e);
            
        }

        private void TextBlock_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            pointerMoved(sender, e);
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(arrangePictureCanvas);
            ArrangedImage = renderTargetBitmap;

            this.Frame.Navigate(typeof(ShareYourPhotoPage));
        }

        private void whiteButton_Click(object sender, RoutedEventArgs e)
        {
            firstQuestionTextBlock.Foreground = whiteBrush;
            secondQuestionTextBlock.Foreground = whiteBrush;
            thirdQuestionTextBlock.Foreground = whiteBrush;
        }

        private void blackButton_Click(object sender, RoutedEventArgs e)
        {
            firstQuestionTextBlock.Foreground = blackBrush;
            secondQuestionTextBlock.Foreground = blackBrush;
            thirdQuestionTextBlock.Foreground = blackBrush;
        }

        private void Image_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            pointerMoved(sender, e);
        }

        private void Image_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            pointerPressed(sender, e);
        }

        private void Image_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            pointerReleased(sender, e);
        }


        private void pointerMoved(object sender, PointerRoutedEventArgs e)
        {

            if (sender == currentTargetSender)
            {
                Point p = e.GetCurrentPoint(arrangePictureCanvas).Position;

                double deltaX = p.X - pressedPoint.X;
                double deltaY = p.Y - pressedPoint.Y;

                ((UIElement)sender).SetValue(Canvas.LeftProperty, pressedSenderX + deltaX);
                ((UIElement)sender).SetValue(Canvas.TopProperty, pressedSenderY + deltaY);
            }
        }

        private void pointerPressed(object sender, PointerRoutedEventArgs e)
        {
            currentTargetSender = sender;
            pressedPoint = e.GetCurrentPoint(arrangePictureCanvas).Position;
            pressedSenderX = (double)((UIElement)sender).GetValue(Canvas.LeftProperty);
            pressedSenderY = (double)((UIElement)sender).GetValue(Canvas.TopProperty);
        }

        private void pointerReleased(object sender, PointerRoutedEventArgs e)
        {
            currentTargetSender = null;
            pressedPoint = new Point(0, 0);
            pressedSenderX = 0;
            pressedSenderY = 0;
        }

        private void ClearTargetSender()
        {
            currentTargetSender = null;
        }

        private void Image_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            ClearTargetSender();
        }

        private void Image_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            ClearTargetSender();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            questionMarkImage.Source = orangeQuestionMarkImage.Source;
        }

        private void redQuestionMarkButton_Click(object sender, RoutedEventArgs e)
        {
            questionMarkImage.Source = redQuestionMarkImage.Source;
        }

        private void pinkQuestionMarkButton_Click(object sender, RoutedEventArgs e)
        {
            questionMarkImage.Source = pinkQuestionMarkImage.Source;
        }
    }
}
