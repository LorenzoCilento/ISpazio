using Foundation;
using System;
using UIKit;
using NewTestArKit.Connection;
using System.Collections.Generic;
using NewTestArKit.Model;

namespace NewTestArKit
{
    public partial class ShowObjectController : UITableViewController
    {
        public int IDBox { get; set; }
        private ItemDAO itemDAO;
        private BoxDAO boxDAO;
        private List<Item> Items { get; set; }
        private string cellIdentifier = "tableCell";

        public ShowObjectController(IntPtr handle) : base(handle)
        {
            itemDAO = new ItemDAO();
            boxDAO = new BoxDAO();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Items = itemDAO.getAllItemInBox(IDBox);
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return Items.Count;
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
            
            return cell;
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            return true;
        }

        public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableViewCellEditingStyle.Delete;
        }

        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        {
            switch (editingStyle)
            {
                case UITableViewCellEditingStyle.Delete:
                    var item = Items[indexPath.Row];
                    var box = boxDAO.getBox(item.Container);
                    box.RemainVolume += item.Volume;
                    boxDAO.updateBox(box);
                    item.Container = 0;
                    itemDAO.updateItem(item);
                    Items.Remove(item);
                    tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
                    break;
                default:
                    break;
            }
        }
    }
}