using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using NewTestArKit.Model;
using NewTestArKit.Connection;
using NewTestArKit.Delegate;

namespace NewTestArKit
{
    public partial class BoxViewController : UITableViewController
    {
        private UIBarButtonItem edit;
        private UIBarButtonItem done;

        private UIImage boxImage;

        public List<Box> Boxes { get; set; }

        private string cellIdentifier = "tableCell";

        private BoxViewDelegate BoxViewDelegate { get; set; }

        private BoxDAO boxDAO;

        private ItemDAO itemDAO;

        public BoxViewController()
        {
            boxDAO = new BoxDAO();
            itemDAO = new ItemDAO();
            BoxViewDelegate = new BoxViewDelegate(this);
        }

        public BoxViewController(IntPtr handle) : base(handle)
        {
            boxDAO = new BoxDAO();
            itemDAO = new ItemDAO();
            BoxViewDelegate = new BoxViewDelegate(this);
            boxImage = UIImage.FromBundle("box_image.png");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            boxView.Delegate = BoxViewDelegate;

            setupEditDoneButton();

            loadData();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            reloadData();
        }

        public void loadData()
        {
            Boxes = boxDAO.getAllBox();
        }

        public void reloadData()
        {
            loadData();
            boxView.ReloadData();
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return Boxes.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);

            var box = Boxes[indexPath.Row];
            string boxName = box.Name;
            var numberItem = itemDAO.getAllItemInBox(box.Id).Count;

            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, cellIdentifier);
            }

            cell.TextLabel.Text = boxName;
            cell.DetailTextLabel.Text = "H: " + box.Height + " W: " + box.Width + " D: " + box.Depth + " Oggetti: " + numberItem;
            cell.ImageView.Image = boxImage;
            return cell;
        }

        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        {
            switch (editingStyle)
            {
                case UITableViewCellEditingStyle.Delete:
                    var box = Boxes[indexPath.Row];
                    Boxes.Remove(box);
                    tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);

                    var items = itemDAO.getAllItemInBox(box.Id);

                    foreach (var i in items)
                    {
                        i.Container = 0;
                        itemDAO.updateItem(i);
                    }
                    boxDAO.deleteBox(box);

                    break;
                case UITableViewCellEditingStyle.None:
                    break;
                default:
                    break;
            }
        }

        public override string TitleForDeleteConfirmation(UITableView tableView, NSIndexPath indexPath)
        {
            return "Elimina";
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            return true;
        }

        public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableViewCellEditingStyle.Delete;
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {

            var identifier = segue.Identifier;
            var destination = segue.DestinationViewController;
            var rowPath = boxView.IndexPathForSelectedRow;
            switch (identifier)
            {
                case "showDrawBoxController":
                    var drawController = destination as DrawBoxController;
                    drawController.IDBox = Boxes[rowPath.Row].Id;
                    break;
                default:
                    break;
            }
        }

        private void setupEditDoneButton()
        {
            edit = new UIBarButtonItem("Modifica", UIBarButtonItemStyle.Plain, (s, e) =>
            {
                if (TableView.Editing)
                    TableView.SetEditing(false, true); // if we've half-swiped a row
                TableView.SetEditing(true, true);
                NavigationItem.LeftBarButtonItem = null;
                NavigationItem.RightBarButtonItem = done;
            });

            NavigationItem.RightBarButtonItem = edit;

            done = new UIBarButtonItem("Fine", UIBarButtonItemStyle.Done, (s, e) =>
            {
                TableView.SetEditing(false, true);
                NavigationItem.RightBarButtonItem = edit;
            });

            done.Title = "Fine";
        }
    }
}