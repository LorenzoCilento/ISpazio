using System;
using UIKit;
namespace NewTestArKit
{
    public class Slide : UIView
    {
        public UILabel TextLabel { get; set; }
        public UILabel TitleLabel { get; set; }
        public UIImageView ImageView { get; set; }

        public Slide()
        {
            TextLabel = new UILabel();
            TitleLabel = new UILabel();
            ImageView = new UIImageView();
        }
    }
}
