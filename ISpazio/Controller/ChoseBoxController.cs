using Foundation;
using NewTestArKit.Connection;
using NewTestArKit.Model;
using System;
using System.Collections.Generic;
using UIKit;

namespace NewTestArKit
{
    public partial class ChoseBoxController : UITableViewController
    {
        private List<Box> boxes { get; set; }
        private BoxDAO BoxDAO { get; set; }
        private ItemDAO ItemDAO { get; set; }
        private string cellIdentifier = "tableCell";
        public Item Item { get; set; }

        public ChoseBoxController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            BoxDAO = new BoxDAO();
            boxes = BoxDAO.getAllBox();
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return boxes.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);

            var box = boxes[indexPath.Row];

            string name = box.Name;

            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
            }

            cell.TextLabel.Text = box.Name;

            if (box.Id == Item.Container)
                cell.Accessory = UITableViewCellAccessory.Checkmark;

            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var boxSelected = boxes[indexPath.Row];
            var idBoxSelected = boxSelected.Id;
            var remainVolumeBoxSelexted = boxSelected.RemainVolume;

            if (Item.Container != idBoxSelected)
            {
                if (Item.Container != 0)
                {
                    deleteVolumeFromPreviusBox();
                }

                if (boxSelected.insertIntoBox(Item))
                {
                    CheckmarkNoneOtherBox();

                    insertObjectInBox(indexPath, boxSelected);
                }
                else
                {
                    tableView.DeselectRow(indexPath, true);

                    alert("Spazio nel box insufficiente", "Scegliere un altro box", "OK");
                }
            }
            else
            {
                alert("L'oggetto è già nel box selezionato", "Scegliere un altro box", "OK");

            }

        }

        private void CheckmarkNoneOtherBox()
        {
            nint section = tableView.NumberOfSections();

            for (var i = 0; i < section; i++)
            {
                nint rows = tableView.NumberOfRowsInSection(i);
                for (var z = 0; z < rows; z++)
                {
                    NSIndexPath path = NSIndexPath.FromRowSection(z, i);
                    var cell = tableView.CellAt(path);
                    if (cell.Accessory == UITableViewCellAccessory.Checkmark)
                        cell.Accessory = UITableViewCellAccessory.None;
                }
            }
        }

        private void deleteVolumeFromPreviusBox()
        {
            var previusBox = BoxDAO.getBox(Item.Container);
         
            previusBox.RemainVolume += Item.Volume;
            BoxDAO.updateBox(previusBox);
        }

        private void alert(string title, string message, string button)
        {
            var alertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);

            alertController.AddAction(UIAlertAction.Create(button, UIAlertActionStyle.Default, null));

            PresentViewController(alertController, true, null);
        }

        private void insertObjectInBox(NSIndexPath indexPath, Box boxSelected)
        {
            var idBoxSelected = boxSelected.Id;
            var remainVolumeBoxSelexted = boxSelected.RemainVolume;
            tableView.CellAt(indexPath).Accessory = UITableViewCellAccessory.Checkmark;

            Item.Container = idBoxSelected;

            ItemDAO = new ItemDAO();
            ItemDAO.updateItem(Item);

            boxSelected.RemainVolume = remainVolumeBoxSelexted - Item.Volume;
            BoxDAO.updateBox(boxSelected);
            boxes = BoxDAO.getAllBox();
        }

    }

}
