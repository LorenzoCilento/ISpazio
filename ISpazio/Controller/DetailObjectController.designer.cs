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
    [Register ("DetailObjectController")]
    partial class DetailObjectController
    {
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
        UIKit.UILabel idLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField nameTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton saveChanges { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel volumeLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel widthLabel { get; set; }

        [Action ("SaveChanges_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void SaveChanges_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
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

            if (idLabel != null) {
                idLabel.Dispose ();
                idLabel = null;
            }

            if (nameTextField != null) {
                nameTextField.Dispose ();
                nameTextField = null;
            }

            if (saveChanges != null) {
                saveChanges.Dispose ();
                saveChanges = null;
            }

            if (volumeLabel != null) {
                volumeLabel.Dispose ();
                volumeLabel = null;
            }

            if (widthLabel != null) {
                widthLabel.Dispose ();
                widthLabel = null;
            }
        }
    }
}