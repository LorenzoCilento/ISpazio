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
    [Register ("ChoseBoxController")]
    partial class ChoseBoxController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (tView != null) {
                tView.Dispose ();
                tView = null;
            }
        }
    }
}