using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using NewTestArKit.Model;
using NewTestArKit.Delegate;

namespace NewTestArKit
{
    public partial class TabBarViewController : UITabBarController
    {

        private List<MyObject> myObjects = new List<MyObject>();
        public List<MyObject> MyObjects
        {
            get { return myObjects; }
            set { myObjects = value; }
        }

        public TabBarViewController (IntPtr handle) : base (handle)
        {
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

        }


        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            Console.WriteLine("PREPARE TAB CONTROLLER");
        }


    }
}