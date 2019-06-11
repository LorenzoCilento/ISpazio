using Foundation;
using System;
using UIKit;

namespace NewTestArKit
{
    public partial class OptionViewController : UIViewController
    {

        public bool IsActiveFindRectangle { get; set; }
        public bool IsActiveMeasureAccuracy { get; set; }
        public int AccuracyValue { get; set; }

        public OptionViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            backGesture.AddTarget(addBackGesture);

            switchFindRectangle.On = IsActiveFindRectangle;

            switchMeasureAccuracy.On = IsActiveMeasureAccuracy;

            if (AccuracyValue == 0)
                AccuracyValue = (int)accuracyValue.Value;
            else
                accuracyValue.Value = AccuracyValue;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.PerformSegue("unwindFromOptionViewControllerToCameraViewController", null);
        }

        private void addBackGesture()
        {
            if (backGesture.State == UIGestureRecognizerState.Ended)
            {
                NavigationController.PopViewController(true);
                this.PerformSegue("unwindFromOptionViewControllerToCameraViewController", null);
            }
        }

        partial void switchRectagleChanged(UISwitch sender)
        {
            IsActiveFindRectangle = switchFindRectangle.On;
        }

        partial void switchMeasureChanged(UISwitch sender)
        {
            IsActiveMeasureAccuracy = switchMeasureAccuracy.On;
        }

        partial void accuracyChanged(UISlider sender)
        {
            AccuracyValue = (int)accuracyValue.Value;
        }
    }
}