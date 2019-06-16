using Foundation;
using System;
using UIKit;
using NewTestArKit.Connection;
using NewTestArKit.Model;
using System.Collections.Generic;
using NewTestArKit.Packing.Algorithms;
using NewTestArKit.Packing.Entities;
using NewTestArKit.Packing;

namespace NewTestArKit
{
    public partial class ChosePackingBoxController : UITableViewController
    {
        private BoxDAO boxDAO;
        private ItemDAO itemDAO;
        private List<Box> Boxes;
        private UIImage boxImage;

        public List<Model.Item> Item { get; set; }

        private string cellIdentifier = "tableCell";

        private UIBarButtonItem insert;

        public ChosePackingBoxController(List<Model.Item> list)
        {
            boxDAO = new BoxDAO();
            itemDAO = new ItemDAO();
            Item = list;
        }

        public ChosePackingBoxController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            loadData();
            boxImage = UIImage.FromBundle("box_image.png");
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

            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, cellIdentifier);
            }

            cell.TextLabel.Text = boxName;
            cell.TextLabel.Text = boxName;
            cell.DetailTextLabel.Text = "H: " + box.Height + " W: " + box.Width + " D: " + box.Depth;
            cell.ImageView.Image = boxImage;

            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var selectedBoxId = Boxes[indexPath.Row].Id;
            checkPacking(Item, selectedBoxId);
        }

        private void setupNavigationItemButton()
        {


            insert = new UIBarButtonItem("Fine", UIBarButtonItemStyle.Done, (s, e) =>
            {
                TableView.SetEditing(false, true);

            })
            {
                Title = "Fine"
            };

            NavigationItem.RightBarButtonItem = insert;

        }

        private void CheckmarkNoneOtherBox()
        {
            nint section = boxView.NumberOfSections();

            for (var i = 0; i < section; i++)
            {
                nint rows = boxView.NumberOfRowsInSection(i);
                for (var z = 0; z < rows; z++)
                {
                    NSIndexPath path = NSIndexPath.FromRowSection(z, i);
                    var cell = boxView.CellAt(path);
                    if (cell.Accessory == UITableViewCellAccessory.Checkmark)
                        cell.Accessory = UITableViewCellAccessory.None;
                }
            }
        }

        private List<Packing.Item> convertListItem(List<Model.Item> items)
        {
            List<Packing.Item> tmp = new List<Packing.Item>();

            foreach (var p in items)
            {
                tmp.Add(convertItem(p));
            }

            return tmp;
        }

        private Packing.Item convertItem(Model.Item item)
        {
            return new Packing.Item(item.Id, (decimal)item.Height, (decimal)item.Width, (decimal)item.Depth, 1); ;
        }

        private Packing.Entities.Container convertBox(Box box)
        {
            return new Packing.Entities.Container(box.Id, (decimal)box.Depth, (decimal)box.Width, (decimal)box.Height);
        }

        private void checkPacking(List<Model.Item> list, int boxID)
        {
            var box = boxDAO.getBox(boxID);
            if (list.Count != 0)
            {
                boxDAO.resetBox(boxID);

                List<Packing.Item> itemsToPack = convertListItem(list);

                List<Packing.Entities.Container> containers = new List<Packing.Entities.Container>();

                containers.Add(convertBox(box));

                List<int> algorithms = new List<int>();
                algorithms.Add((int)AlgorithmType.EB_AFIT);

                List<ContainerPackingResult> result = PackingService.Pack(containers, itemsToPack, algorithms);

                List<Packing.Item> listPacked = new List<Packing.Item>();
                List<Packing.Item> listUnpacked = new List<Packing.Item>();

                var container = 0;
                foreach (var a in result)
                {
                    container = a.ContainerID;

                    foreach (var i in a.AlgorithmPackingResults)
                    {
                        listPacked = i.PackedItems;
                        listUnpacked = i.UnpackedItems;
                    }
                }

                foreach (var v in listPacked)
                {
                    var item = itemDAO.getItem(v.ID);
                    item.updateCoordinate(v.CoordX, v.CoordY, v.CoordZ, v.PackDimX, v.PackDimY, v.PackDimZ);
                    itemDAO.updateItem(item);
                }



                if (listUnpacked.Count == 0 && listPacked.Count == 0)
                {
                    alert("Impossibile completare", "Non è stato possibile inserire nessun oggetto nel pacco", "ok");
                }
                else
                {
                    if (listUnpacked.Count == 0)
                    {
                        alert("Completato", "Tutti gli oggetti sono stati inseriti", "ok");
                        insertPackedItem(listPacked, container);
                    }
                    else
                    {
                        alert("Non tutti gli oggetti sono stati inseriti", "Consulta gli oggetti nel pacco per vedere quelli inseriti", "ok");
                        insertPackedItem(listPacked, container);
                    }
                }
            }
        }

        private void alert(string title, string message, string button)
        {
            var alertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);

            alertController.AddAction(UIAlertAction.Create(button, UIAlertActionStyle.Default, (obj) => { DismissViewController(true, null); }));

            PresentViewController(alertController, true, null);
        }

        private void insertPackedItem(List<Packing.Item> listPacked, int container)
        {
            foreach (var i in listPacked)
            {
                var item = itemDAO.getItem(i.ID);
                item.Container = container;
                itemDAO.updateItem(item);
                updateBox(container, item);
            }
        }

        private void updateBox(int idBox, Model.Item item)
        {
            var box = boxDAO.getBox(idBox);
            var remainVolumeBoxSelexted = box.RemainVolume;

            box.RemainVolume = remainVolumeBoxSelexted - item.Volume;
            boxDAO.updateBox(box);
        }

    }
}
