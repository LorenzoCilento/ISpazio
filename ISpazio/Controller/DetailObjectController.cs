using Foundation;
using System;
using UIKit;
using NewTestArKit.Model;
using NewTestArKit.Connection;

namespace NewTestArKit
{
    public partial class DetailObjectController : UIViewController
    {
        public Item Item { get; set; }

        public int IDitem { get; set; }

        private ItemDAO itemDAO;
        private BoxDAO boxDAO;

        public DetailObjectController(IntPtr handle) : base(handle)
        {
            itemDAO = new ItemDAO();
            boxDAO = new BoxDAO();
        }

        public DetailObjectController() { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Item = itemDAO.getItem(IDitem);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            Item = itemDAO.getItem(IDitem);
            update();
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            descriptionTextField.ResignFirstResponder();
            nameTextField.ResignFirstResponder();
        }

        private void update()
        {
            idLabel.Text = Item.Id.ToString();
            heightLabel.Text = Item.Height.ToString() + " cm";
            widthLabel.Text = Item.Width.ToString() + " cm";
            depthLabel.Text = Item.Depth.ToString() + " cm";
            nameTextField.Text = Item.Name;
            descriptionTextField.Text = Item.Description;
            volumeLabel.Text = Item.Volume.ToString() + " cm3";
        }

        partial void SaveChanges_TouchUpInside(UIButton sender)
        {
            Item.Name = nameTextField.Text;
            Item.Description = descriptionTextField.Text;

            itemDAO.updateItem(Item);
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            var identifier = segue.Identifier;
            var destination = segue.DestinationViewController;

            switch (identifier)
            {
                case "showChoseBoxController":
                    var choseBox = destination as ChoseBoxController;
                    choseBox.Item = Item;
                    break;
                default:
                    break;
            }
        }
    }
}