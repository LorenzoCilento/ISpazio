// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace NewTestArKit
{
    [Register ("CameraViewController")]
    partial class CameraViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISegmentedControl dimensionChoise { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton eraseButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel measureLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel messageLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView messageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton okButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton optionButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScreenEdgePanGestureRecognizer optionGesture { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton resetButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        ARKit.ARSCNView sceneView { get; set; }

        [Action ("dimensionChoiseValueChangedPressed:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void dimensionChoiseValueChangedPressed (UIKit.UISegmentedControl sender);

        [Action ("EraseButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void EraseButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("OkButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OkButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("resetButtonPressed:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void resetButtonPressed (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (dimensionChoise != null) {
                dimensionChoise.Dispose ();
                dimensionChoise = null;
            }

            if (eraseButton != null) {
                eraseButton.Dispose ();
                eraseButton = null;
            }

            if (measureLabel != null) {
                measureLabel.Dispose ();
                measureLabel = null;
            }

            if (messageLabel != null) {
                messageLabel.Dispose ();
                messageLabel = null;
            }

            if (messageView != null) {
                messageView.Dispose ();
                messageView = null;
            }

            if (okButton != null) {
                okButton.Dispose ();
                okButton = null;
            }

            if (optionButton != null) {
                optionButton.Dispose ();
                optionButton = null;
            }

            if (optionGesture != null) {
                optionGesture.Dispose ();
                optionGesture = null;
            }

            if (resetButton != null) {
                resetButton.Dispose ();
                resetButton = null;
            }

            if (sceneView != null) {
                sceneView.Dispose ();
                sceneView = null;
            }
        }
    }
}