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
    [Register ("DetailViewController")]
    partial class DetailViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton addButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton backButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScreenEdgePanGestureRecognizer backGesture { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel depthLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView descriptionTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel heightLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField nameTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISegmentedControl SegmentChoiceTypeObject { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel widthLabel { get; set; }

        [Action ("addButtonPressed:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void addButtonPressed (UIKit.UIButton sender);

        [Action ("BackButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BackButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (addButton != null) {
                addButton.Dispose ();
                addButton = null;
            }

            if (backButton != null) {
                backButton.Dispose ();
                backButton = null;
            }

            if (backGesture != null) {
                backGesture.Dispose ();
                backGesture = null;
            }

            if (depthLabel != null) {
                depthLabel.Dispose ();
                depthLabel = null;
            }

            if (descriptionTextField != null) {
                descriptionTextField.Dispose ();
                descriptionTextField = null;
            }

            if (heightLabel != null) {
                heightLabel.Dispose ();
                heightLabel = null;
            }

            if (nameTextField != null) {
                nameTextField.Dispose ();
                nameTextField = null;
            }

            if (SegmentChoiceTypeObject != null) {
                SegmentChoiceTypeObject.Dispose ();
                SegmentChoiceTypeObject = null;
            }

            if (widthLabel != null) {
                widthLabel.Dispose ();
                widthLabel = null;
            }
        }
    }
}