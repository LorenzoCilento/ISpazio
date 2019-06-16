using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using NewTestArKit.Model;
using NewTestArKit.Connection;
using NewTestArKit.Delegate;
using System.ComponentModel;
using NewTestArKit.Packing;
using NewTestArKit.Packing.Entities;
using NewTestArKit.Packing.Algorithms;

namespace NewTestArKit
{
    public partial class ObjectViewController : UITableViewController
    {
        private ItemDAO itemDAO = new ItemDAO();
        private BoxDAO boxDAO = new BoxDAO();

        private UIImage itemImage;

        public List<Model.Item> Items { get; set; }

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

            itemImage = UIImage.FromBundle("item_image.png");
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

            var item = Items[indexPath.Row];
            string itemName = item.Name;
            var container = item.Container;


            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, cellIdentifier);
            }

            cell.TextLabel.Text = itemName;
            cell.Accessory = UITableViewCellAccessory.DetailDisclosureButton;
            cell.ImageView.Image = itemImage;

            if (container != 0)
            {
                var box = boxDAO.getBox(container);
                cell.DetailTextLabel.Text = box.Name + "\tH: " + item.Height + " W: " + item.Width + " D: " + item.Depth;
            }
            else
            {
                cell.DetailTextLabel.Text = "Nessun pacco" + "\tH: " + item.Height + " W: " + item.Width + " D: " + item.Depth;
            }
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
                  NavigationItem.SetRightBarButtonItems(new UIBarButtonItem[] { done, delete }, true);
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
                var list = objectViewDelegate.SelectedItems;

                if (list.Count != 0)
                {
                    okCancelAlert("Attenzione", "il pacco che si selezionerà verra svuotato", list);
                }
                else
                {
                    alert("Nessun oggetto selezionato", "Seleziona qualche oggetto", "Ok");
                }

                TableView.SetEditing(false, true);
                NavigationItem.RightBarButtonItem = null;
                NavigationItem.RightBarButtonItem = edit;
                NavigationItem.LeftBarButtonItem = null;
            });

            delete = new UIBarButtonItem("Elimina", UIBarButtonItemStyle.Plain, (sender, e) =>
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

        private void alert(string title, string message, string button)
        {
            var alertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);

            alertController.AddAction(UIAlertAction.Create(button, UIAlertActionStyle.Default, null));

            PresentViewController(alertController, true, null);
        }

        private void okCancelAlert(string title, string message, List<Model.Item> list)
        {
            //Create Alert
            var okCancelAlertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);

            //Add Actions
            okCancelAlertController.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, (obj) => { presentCheckController(obj, list); }));
            okCancelAlertController.AddAction(UIAlertAction.Create("Annulla", UIAlertActionStyle.Cancel, alert => Console.WriteLine("Cancel was clicked")));

            //Present Alert
            PresentViewController(okCancelAlertController, true, null);
        }

        void presentCheckController(UIAlertAction obj, List<Model.Item> list)
        {
            var vc = new ChosePackingBoxController(list);

            PresentViewController(vc, true, null);
        }

    }
}