﻿using Foundation;
using System;
using UIKit;
using NewTestArKit.Model;
using NewTestArKit.Connection;

namespace NewTestArKit
{
    public partial class DetailBoxController : UIViewController
    {
        private Box Box { get; set; }
        private BoxDAO boxDAO;
        public int IDBox { get; set; }


        public DetailBoxController(IntPtr handle) : base(handle)
        {
            boxDAO = new BoxDAO();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Box = boxDAO.getBox(IDBox);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            Box = boxDAO.getBox(IDBox);

            updateLabel();

        }

        private void updateLabel()
        {
            idLabel.Text = Box.Id.ToString();
            heightLabel.Text = Box.Height.ToString() + " cm";
            widthLabel.Text = Box.Width.ToString() + " cm";
            depthLabel.Text = Box.Depth.ToString() + " cm";
            volumeLabel.Text = Box.Volume.ToString() + " cm3";
            nameTextField.Text = Box.Name;
            descriptionTextField.Text = Box.Description;
            updateFreeSpace();
        }

        partial void SaveChanges_TouchUpInside(UIButton sender)
        {
            Box.Name = nameTextField.Text;
            Box.Description = descriptionTextField.Text;
            boxDAO.updateBox(Box);
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            nameTextField.ResignFirstResponder();
            descriptionTextField.ResignFirstResponder();
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            var identifier = segue.Identifier;
            var destination = segue.DestinationViewController;

            switch (identifier)
            {
                case "showShowObjectController":
                    var showObject = destination as ShowObjectController;
                    showObject.IDBox = IDBox;
                    break;
                case "showDrawBoxController":
                    var draw = destination as DrawBoxController;
                    draw.IDBox = IDBox;
                    break;
                case "showItemController":
                    Console.WriteLine("showItemController");
                    var itemController = destination as ShowItemController;
                    itemController.IDBox = IDBox;
                    break;
                default:
                    break;

            }
        }

        private void updateFreeSpace()
        {
            var value = (float)(Box.RemainVolume / Box.Volume);
            var percent = Math.Round(value * 100, 1);

            freeSpaceLabel.Text = percent + " %";
            if (value >= 1.0f)
                freeSpaceProgressBar.Progress = 0f;
            else
                freeSpaceProgressBar.Progress = 1 - value;
        }
    }
}