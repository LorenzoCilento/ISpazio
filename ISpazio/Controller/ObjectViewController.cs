using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using NewTestArKit.Model;
using NewTestArKit.Connection;
using NewTestArKit.Delegate;
using System.ComponentModel;

namespace NewTestArKit
{
    public partial class ObjectViewController : UITableViewController
    {
        private ItemDAO itemDAO = new ItemDAO();
        private BoxDAO boxDAO = new BoxDAO();

        public List<Item> Items { get; set; }

        private ObjectViewDelegate objectViewDelegate;

        private string cellIdentifier = "tableCell";

        private UIBarButtonItem done;
        private UIBarButtonItem edit;
        private UIBarButtonItem insert;
        private UIBarButtonItem delete;

        public ObjectViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            objectViewDelegate = new ObjectViewDelegate(this);
            TableView.Delegate = objectViewDelegate;
            TableView.AllowsMultipleSelectionDuringEditing = true;

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

            Items = itemDAO.getAllItem();
        }

        public void reloadData()
        {
            loadData();
            itemView.ReloadData();
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);

            string itemName = Items[indexPath.Row].Name;

            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
            }

            cell.TextLabel.Text = itemName;
            cell.Accessory = UITableViewCellAccessory.DetailDisclosureButton;
            return cell;
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return Items.Count;
        }

        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        {
            switch (editingStyle)
            {
                case UITableViewCellEditingStyle.Delete:
                    deleteItem(indexPath);
                    break;

                case UITableViewCellEditingStyle.None:
                    Console.WriteLine("CommitEditingStyle:None called");
                    break;
                default:
                    break;
            }
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
                case "showDetailObjectController":
                    var detail = destination as DetailObjectController;
                    var rowPath = TableView.IndexPathForSelectedRow;
                    detail.IDitem = Items[rowPath.Row].Id;
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
                  NavigationItem.LeftBarButtonItem = insert;
                  NavigationItem.SetRightBarButtonItems(new UIBarButtonItem[] { done,delete }, true);
                  objectViewDelegate.SelectedItems.Clear();
              });

            NavigationItem.RightBarButtonItem = edit;

            done = new UIBarButtonItem("Fine", UIBarButtonItemStyle.Done, (s, e) =>
            {
                TableView.SetEditing(false, true);
                NavigationItem.RightBarButtonItems = null;
                NavigationItem.RightBarButtonItem = edit;
                NavigationItem.LeftBarButtonItem = null;
            });

            insert = new UIBarButtonItem("Inserisci in", UIBarButtonItemStyle.Plain, (sender, e) =>
            {
                TableView.SetEditing(false, true);
                NavigationItem.RightBarButtonItem = edit;
                NavigationItem.LeftBarButtonItem = null;
            });

            delete = new UIBarButtonItem("Elimina",UIBarButtonItemStyle.Plain, (sender, e) =>
            {
                TableView.SetEditing(false, true);
                NavigationItem.RightBarButtonItem = null;
                NavigationItem.RightBarButtonItem = edit;
                NavigationItem.LeftBarButtonItem = null;
                deleteMoreItem(objectViewDelegate.SelectedIndexPaths);
                objectViewDelegate.SelectedIndexPaths.Clear();
            });
        }

        private void deleteMoreItem(List<NSIndexPath> indexPaths)
        {
            for (var i = indexPaths.Count - 1; i >= 0; i--)
                deleteItem(indexPaths[i]);
        }

        private void deleteItem(NSIndexPath indexPath)
        {
            var index = indexPath.Row;
            var item = Items[index];
            if (item.Container != 0)
            {
                var box = boxDAO.getBox(item.Container);
                box.RemainVolume += item.Volume;
                boxDAO.updateBox(box);
            }
            Items.Remove(item);
            TableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
            itemDAO.deleteItem(item);
        }
    }
}