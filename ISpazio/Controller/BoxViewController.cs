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

        public List<Box> Boxes { get; set; }

        private string cellIdentifier = "tableCell";

        private BoxViewDelegate boxViewDelegate;

        private BoxDAO boxDAO;

        private ItemDAO itemDAO;

        public BoxViewController() { }

        public BoxViewController (IntPtr handle) : base (handle)
        {
            boxDAO = new BoxDAO();
            itemDAO = new ItemDAO();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            boxViewDelegate = new BoxViewDelegate(this);
            boxView.Delegate = boxViewDelegate;

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
            BoxDAO boxDAO = new BoxDAO();
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

            string boxName = Boxes[indexPath.Row].Name;
            
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
            }

            cell.TextLabel.Text = boxName;

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

                    foreach(var i in items)
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

            switch (identifier)
            {
                case "showDetailBoxController":
                    var detail = destination as DetailBoxController;
                    var rowPath = boxView.IndexPathForSelectedRow;
                    detail.IDBox = Boxes[rowPath.Row].Id;
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

            done = new UIBarButtonItem("Fine", UIBarButtonItemStyle.Done, (s, e) => {
                TableView.SetEditing(false, true);
                NavigationItem.RightBarButtonItem = edit;
            });

            done.Title = "Fine";
        }
    }
}