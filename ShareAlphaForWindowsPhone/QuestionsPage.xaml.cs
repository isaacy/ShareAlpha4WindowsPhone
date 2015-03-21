using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ShareAlpha
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class QuestionsPage : Page
    {

        public static string FirstQuestion { get; private set; }

        public static string SecondQuestion { get; private set; }

        public static string ThirdQuestion { get; private set; }


        public QuestionsPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            firstQuestionTextBox.Text = string.Empty;
            secondQuestionTextBox.Text = string.Empty;
            thirdQuestionTextBox.Text = string.Empty;
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            FirstQuestion = firstQuestionTextBox.Text;
            SecondQuestion = secondQuestionTextBox.Text;
            ThirdQuestion = thirdQuestionTextBox.Text;

            bool gotoNextPage = true;

            if (string.IsNullOrEmpty(FirstQuestion))
            {
                message.Text = "Please complete first question";
                gotoNextPage = false;
            }

            if (string.IsNullOrEmpty(SecondQuestion))
            {
                message.Text = "Please complete second question";
                gotoNextPage = false;

            }

            if (string.IsNullOrEmpty(ThirdQuestion))
            {
                message.Text = "Please complete third question";
                gotoNextPage = false;

            }

            if (gotoNextPage)
            {
                this.Frame.Navigate(typeof(ArrangePicturePage));
            }
        }

    }
}
