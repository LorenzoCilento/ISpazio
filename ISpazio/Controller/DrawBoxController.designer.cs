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
    [Register ("DrawBoxController")]
    partial class DrawBoxController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel depthLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel freeSpaceLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIProgressView freeSpaceProgressBar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel heightLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView infoView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel nameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SceneKit.SCNView sceneView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel widthLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (depthLabel != null) {
                depthLabel.Dispose ();
                depthLabel = null;
            }

            if (freeSpaceLabel != null) {
                freeSpaceLabel.Dispose ();
                freeSpaceLabel = null;
            }

            if (freeSpaceProgressBar != null) {
                freeSpaceProgressBar.Dispose ();
                freeSpaceProgressBar = null;
            }

            if (heightLabel != null) {
                heightLabel.Dispose ();
                heightLabel = null;
            }

            if (infoView != null) {
                infoView.Dispose ();
                infoView = null;
            }

            if (nameLabel != null) {
                nameLabel.Dispose ();
                nameLabel = null;
            }

            if (sceneView != null) {
                sceneView.Dispose ();
                sceneView = null;
            }

            if (widthLabel != null) {
                widthLabel.Dispose ();
                widthLabel = null;
            }
        }
    }
}