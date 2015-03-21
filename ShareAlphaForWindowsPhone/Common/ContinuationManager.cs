
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ShareAlpha.Common
{
#if WINDOWS_PHONE_APP
    /// <summary>
    /// ContinuationManager is used to detect if the most recent activation was due
    /// to a continuation such as the FileOpenPicker or WebAuthenticationBroker
    /// </summary>
    public class ContinuationManager
    {
        IContinuationActivatedEventArgs args = null;
        Guid id = Guid.Empty;

        /// <summary>
        /// Sets the ContinuationArgs for this instance. Using default Frame of current Window
        /// Should be called by the main activation handling code in App.xaml.cs
        /// </summary>
        /// <param name="args">The activation args</param>
        internal void Continue(IContinuationActivatedEventArgs args)
        {
            Continue(args, Window.Current.Content as Frame);
        }

        /// <summary>
        /// Sets the ContinuationArgs for this instance. Should be called by the main activation
        /// handling code in App.xaml.cs
        /// </summary>
        /// <param name="args">The activation args</param>
        /// <param name="rootFrame">The frame control that contains the current page</param>
        internal void Continue(IContinuationActivatedEventArgs args, Frame rootFrame)
        {
            if (args == null)
                throw new ArgumentNullException("args");

            if (this.args != null)
                throw new InvalidOperationException("Can't set args more than once");

            this.args = args;
            this.id = Guid.NewGuid();

            if (rootFrame == null)
                return;

            switch (args.Kind)
            {
                case ActivationKind.PickFileContinuation:
                    //var fileOpenPickerPage = rootFrame.Content as IFileOpenPickerContinuable;
                    FileOpenPickerContinuationEventArgs fileOpenPickerArg = args as FileOpenPickerContinuationEventArgs;

                    if (fileOpenPickerArg.Files.Any())
                    {
                        rootFrame.Navigate(typeof(ScaleAndCropPage), fileOpenPickerArg.Files.First());
                    }
                    //if (fileOpenPickerPage != null)
                    //{
                    //    fileOpenPickerPage.ContinueFileOpenPicker();
                    //}
                    break;

                case ActivationKind.PickSaveFileContinuation:

                    FileSavePickerContinuationEventArgs fileSavePickerArg = args as FileSavePickerContinuationEventArgs;

                    if (fileSavePickerArg.File != null)
                    {
                        rootFrame.Navigate(typeof(ShareYourPhotoPage), fileSavePickerArg.File);
                    }
                    //var fileSavePickerPage = rootFrame.Content as IFileSavePickerContinuable;
                    //if (fileSavePickerPage != null)
                    //{
                    //    fileSavePickerPage.ContinueFileSavePicker(args as FileSavePickerContinuationEventArgs);
                    //}
                    break;

                case ActivationKind.PickFolderContinuation:
                    //var folderPickerPage = rootFrame.Content as IFolderPickerContinuable;
                    //if (folderPickerPage != null)
                    //{
                    //    folderPickerPage.ContinueFolderPicker(args as FolderPickerContinuationEventArgs);
                    //}
                    break;

                case ActivationKind.WebAuthenticationBrokerContinuation:
                    //var wabPage = rootFrame.Content as IWebAuthenticationContinuable;
                    //if (wabPage != null)
                    //{
                    //    wabPage.ContinueWebAuthentication(args as WebAuthenticationBrokerContinuationEventArgs);
                    //}
                    break;
            }
        }

        /// <summary>
        /// Retrieves the continuation args, if they have not already been retrieved, and 
        /// prevents further retrieval via this property (to avoid accidentla double-usage)
        /// </summary>
        public IContinuationActivatedEventArgs ContinuationArgs
        {
            get
            {
                return args;
            }
        }

        /// <summary>
        /// Unique identifier for this particular continuation. Most useful for components that 
        /// retrieve the continuation data via <see cref="GetContinuationArgs"/> and need
        /// to perform their own replay check
        /// </summary>
        public Guid Id { get { return id; } }

    }
}
/// <summary>
/// Implement this interface if your page invokes the file open picker
/// API.
/// </summary>
interface IFileOpenPickerContinuable
{
    /// <summary>
    /// This method is invoked when the file open picker returns picked
    /// files
    /// </summary>
    /// <param name="args">Activated event args object that contains returned files from file open picker</param>
    void ContinueFileOpenPicker(FileOpenPickerContinuationEventArgs args);
}

/// <summary>
/// Implement this interface if your page invokes the file save picker
/// API
/// </summary>
interface IFileSavePickerContinuable
{
    /// <summary>
    /// This method is invoked when the file save picker returns saved
    /// files
    /// </summary>
    /// <param name="args">Activated event args object that contains returned file from file save picker</param>
    void ContinueFileSavePicker(FileSavePickerContinuationEventArgs args);
}

/// <summary>
/// Implement this interface if your page invokes the folder picker API
/// </summary>
interface IFolderPickerContinuable
{
    /// <summary>
    /// This method is invoked when the folder picker returns the picked
    /// folder
    /// </summary>
    /// <param name="args">Activated event args object that contains returned folder from folder picker</param>
    void ContinueFolderPicker(FolderPickerContinuationEventArgs args);
}

/// <summary>
/// Implement this interface if your page invokes the web authentication
/// broker
/// </summary>
interface IWebAuthenticationContinuable
{
    /// <summary>
    /// This method is invoked when the web authentication broker returns
    /// with the authentication result
    /// </summary>
    /// <param name="args">Activated event args object that contains returned authentication token</param>
    void ContinueWebAuthentication(WebAuthenticationBrokerContinuationEventArgs args);
}

#endif