// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace NewTestArKit
{
    [Register ("OptionViewController")]
    partial class OptionViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISlider accuracyValue { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton backButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScreenEdgePanGestureRecognizer backGesture { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch switchFindRectangle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch switchMeasureAccuracy { get; set; }

        [Action ("accuracyChanged:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void accuracyChanged (UIKit.UISlider sender);

        [Action ("switchMeasureChanged:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void switchMeasureChanged (UIKit.UISwitch sender);

        [Action ("switchRectagleChanged:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void switchRectagleChanged (UIKit.UISwitch sender);

        void ReleaseDesignerOutlets ()
        {
            if (accuracyValue != null) {
                accuracyValue.Dispose ();
                accuracyValue = null;
            }

            if (backButton != null) {
                backButton.Dispose ();
                backButton = null;
            }

            if (backGesture != null) {
                backGesture.Dispose ();
                backGesture = null;
            }

            if (switchFindRectangle != null) {
                switchFindRectangle.Dispose ();
                switchFindRectangle = null;
            }

            if (switchMeasureAccuracy != null) {
                switchMeasureAccuracy.Dispose ();
                switchMeasureAccuracy = null;
            }
        }
    }
}