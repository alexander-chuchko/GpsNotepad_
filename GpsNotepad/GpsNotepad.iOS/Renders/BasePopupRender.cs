using GpsNotepad.iOS.Renders;
using GpsNotepad.View;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BasePopupPage), typeof(BasePopupRender))]
namespace GpsNotepad.iOS.Renders
{
    public class BasePopupRender: PageRenderer
    {

        private UIViewController _parentModalViewController;

        #region -- Overrides --

        public override void DidMoveToParentViewController(UIViewController parent)
        {
            base.DidMoveToParentViewController(parent);
            _parentModalViewController = parent;
            parent.ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            _parentModalViewController.View.BackgroundColor = UIColor.Clear;
            View.BackgroundColor = UIColor.Clear;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            _parentModalViewController.View.BackgroundColor = UIColor.Clear;
            View.BackgroundColor = UIColor.Clear;
        }

        #endregion

    }
}