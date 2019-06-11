using System;
using UIKit;
using CoreGraphics;
namespace NewTestArKit
{
    public class ScrollViewDelegate : UIScrollViewDelegate
    {
        private PresentationPopoverViewController Source { get; set; }

        public ScrollViewDelegate(PresentationPopoverViewController source)
        {
            Source = source;
        }

        public override void DidChangeAdjustedContentInset(UIScrollView scrollView)
        {
            var pageIndex = Math.Round(scrollView.ContentOffset.X / Source.SlideView.Frame.Width);
            Source.PageControl.CurrentPage = (int)pageIndex;

            Console.WriteLine(pageIndex);

            var maximumHorizontalOffset = scrollView.ContentSize.Width - scrollView.Frame.Width;
            var currentHorizontalOffset = scrollView.ContentOffset.X;


            var maximumVerticalOffset = scrollView.ContentSize.Height - scrollView.Frame.Height;
            var currentVerticalOffset = scrollView.ContentOffset.Y;

            var percentageHorizontalOffset = currentHorizontalOffset / maximumHorizontalOffset;
            var percentageVerticalOffset = currentVerticalOffset / maximumVerticalOffset;

            var percentOffset = new CGPoint(percentageHorizontalOffset, percentageVerticalOffset);

            if (percentOffset.X > 0 && percentOffset.X <= 0.25)
            {
                Source.Slides[0].ImageView.Transform = CGAffineTransform.MakeScale((nfloat)((0.25 - percentOffset.X) / 0.25), (nfloat)((0.25 - percentOffset.X) / 0.25));
                Source.Slides[1].ImageView.Transform = CGAffineTransform.MakeScale((nfloat)(percentOffset.X / 0.25), (nfloat)(percentOffset.X / 0.25));
            }
            else if (percentOffset.X > 0.25 && percentOffset.X <= 0.50)
            {
                Source.Slides[1].ImageView.Transform = CGAffineTransform.MakeScale((nfloat)((0.50 - percentOffset.X) / 0.25), (nfloat)((0.50 - percentOffset.X) / 0.25));
                Source.Slides[2].ImageView.Transform = CGAffineTransform.MakeScale((nfloat)(percentOffset.X / 0.50), (nfloat)(percentOffset.X / 0.50));
            }
        }

        public override void DidZoom(UIScrollView scrollView)
        {
            var pageIndex = Math.Round(scrollView.ContentOffset.X / Source.SlideView.Frame.Width);
            Source.PageControl.CurrentPage = (int)pageIndex;

            Console.WriteLine(pageIndex);

            var maximumHorizontalOffset = scrollView.ContentSize.Width - scrollView.Frame.Width;
            var currentHorizontalOffset = scrollView.ContentOffset.X;


            var maximumVerticalOffset = scrollView.ContentSize.Height - scrollView.Frame.Height;
            var currentVerticalOffset = scrollView.ContentOffset.Y;

            var percentageHorizontalOffset = currentHorizontalOffset / maximumHorizontalOffset;
            var percentageVerticalOffset = currentVerticalOffset / maximumVerticalOffset;

            var percentOffset = new CGPoint(percentageHorizontalOffset, percentageVerticalOffset);

            if (percentOffset.X > 0 && percentOffset.X <= 0.25)
            {
                Source.Slides[0].ImageView.Transform = CGAffineTransform.MakeScale((nfloat)((0.25 - percentOffset.X) / 0.25), (nfloat)((0.25 - percentOffset.X) / 0.25));
                Source.Slides[1].ImageView.Transform = CGAffineTransform.MakeScale((nfloat)(percentOffset.X / 0.25), (nfloat)(percentOffset.X / 0.25));
            }
            else if (percentOffset.X > 0.25 && percentOffset.X <= 0.50)
            {
                Source.Slides[1].ImageView.Transform = CGAffineTransform.MakeScale((nfloat)((0.50 - percentOffset.X) / 0.25), (nfloat)((0.50 - percentOffset.X) / 0.25));
                Source.Slides[2].ImageView.Transform = CGAffineTransform.MakeScale((nfloat)(percentOffset.X / 0.50), (nfloat)(percentOffset.X / 0.50));
            }
        }
    }
}
