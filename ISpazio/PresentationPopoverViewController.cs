using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using CoreGraphics;

namespace NewTestArKit
{
    public partial class PresentationPopoverViewController : UIViewController
    {

        public List<Slide> Slides { get; set; }

        public UIView SlideView { get => slideView; }

        public UIPageControl PageControl { get => pageControl; }

        public UIImageView ImageView { get => imageView; }

        public PresentationPopoverViewController() { }

        public PresentationPopoverViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            scrollView.Delegate = new ScrollViewDelegate(this);

            Slides = createSlide();
            setupScrollView(Slides);

            pageControl.Pages = Slides.Count;
            pageControl.CurrentPage = 0;
        }

        private void setupScrollView(List<Slide> slides)
        {
            scrollView.Frame = new CGRect(0, 0, slideView.Frame.Width, slideView.Frame.Height);
            scrollView.ContentSize = new CGSize(slideView.Frame.Width * slides.Count, slideView.Frame.Height);
            scrollView.PagingEnabled = true;

            foreach (var i in Slides)
            {
                scrollView.AddSubview(setupSlide(i));
            }
        }

        private Slide setupSlide(Slide slide)
        {
            var slideTmp = new Slide();

            slideTmp.TextLabel.Text = slide.TextLabel.Text;
            slideTmp.TitleLabel.Text = slide.TitleLabel.Text;

            return slideTmp;
        }

        private List<Slide> createSlide()
        {
            List<Slide> slides = new List<Slide>();

            var slide = new Slide();
            slide.TitleLabel.Text = "Slide 1";
            slide.TextLabel.Text = "ciccio";
            slides.Add(slide);

            slide = new Slide();
            slide.TitleLabel.Text = "Slide 2";
            slide.TextLabel.Text = "pasticcio";
            slides.Add(slide);

            slide = new Slide();
            slide.TitleLabel.Text = "Slide 3";
            slide.TextLabel.Text = "miao";
            slides.Add(slide);

            return slides;
        }
    }
}