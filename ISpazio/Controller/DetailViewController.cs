using Foundation;
using System;
using UIKit;
using NewTestArKit.Model;
using NewTestArKit.Connection;


namespace NewTestArKit
{
    public partial class DetailViewController : UIViewController
    {

        private MyObject obj;
        public MyObject Obj
        {
            get { return obj; }
            set { obj = value; }
        }


        public DetailViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            backGesture.AddTarget(addBackGesture);

            updateLabel();

            setupTextField();

        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            NavigationController.PopViewController(true);
        }



        partial void BackButton_TouchUpInside(UIButton sender)
        {
            NavigationController.PopViewController(true);

        }

        private void setupTextField()
        {
            nameTextField.ShouldReturn = (textField) =>
            {
                Obj.Name = textField.Text;
                nameTextField.ResignFirstResponder();
                return true;
            };

            nameTextField.ShouldEndEditing = (textField) =>
            {
                Obj.Name = textField.Text;
                return true;
            };

            nameTextField.EditingDidBegin += (sender, e) => { nameTextField.Text = ""; };

            descriptionTextField.ShouldBeginEditing = (textView) =>
            {
                descriptionTextField.Text = "";
                return true;
            };

            descriptionTextField.ShouldEndEditing = (textView) =>
            {
                Obj.Description = textView.Text;
                return true;
            };


        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            descriptionTextField.ResignFirstResponder();
            nameTextField.ResignFirstResponder();
        }

        private void addBackGesture()
        {
            if (backGesture.State == UIGestureRecognizerState.Ended)
            {
                NavigationController.PopViewController(true);
                this.PerformSegue("unwindFromDetailViewControllerToCameraViewController", null);
            }
        }

        partial void addButtonPressed(UIButton sender)
        {
            if (obj.allDistanceNotZero())
            {
                switch (SegmentChoiceTypeObject.SelectedSegment)
                {
                    case 0:
                        Item item = new Item(obj);

                        ItemDAO itemDAO = new ItemDAO();
                        itemDAO.insertItem(item);

                        break;
                    case 1:
                        Box box = new Box(obj);

                        BoxDAO boxDAO = new BoxDAO();
                        boxDAO.insertBox(box);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                alert("Non è possibile aggiungere l'oggetto", "Una delle dimensioni è uguale a 0");
            }
        }


        private void updateLabel()
        {
            heightLabel.Text = obj.Height.ToString() + " cm";
            widthLabel.Text = obj.Width.ToString() + " cm";
            depthLabel.Text = obj.Depth.ToString() + " cm";
        }

        private void alert(string title, string message)
        {
            var alertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);

            alertController.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));
            PresentViewController(alertController, true, null);
        }

    }
}
