﻿using System;
using Foundation;
using UIKit;
using NewTestArKit.Connection;
namespace NewTestArKit.Delegate
{
    public class BoxViewDelegate : UITableViewDelegate
    {
        private UITableViewController Source { get; set; }
        private ItemDAO itemDAO;
        private BoxDAO boxDAO;

        public BoxViewDelegate(UITableViewController source)
        {
            Source = source;
            itemDAO = new ItemDAO();
            boxDAO = new BoxDAO();
        }

        public override UISwipeActionsConfiguration GetLeadingSwipeActionsConfiguration(UITableView tableView, NSIndexPath indexPath)
        {
            var insertAction = ContextualFlagAction(indexPath.Row);

            var leadingSwipe = UISwipeActionsConfiguration.FromActions(new UIContextualAction[] { insertAction });

            leadingSwipe.PerformsFirstActionWithFullSwipe = false;

            return leadingSwipe;
        }

        public UIContextualAction ContextualFlagAction(int row)
        {
            var action = UIContextualAction.FromContextualActionStyle(
                UIContextualActionStyle.Normal, "Svuota",
                (insertAction, view, success) => {
                    var source = Source as BoxViewController;
                    var boxSelected = source.Boxes[row];

                    var items = itemDAO.getAllItemInBox(boxSelected.Id);
                    foreach (var b in items)
                    {
                        b.Container = 0;
                        itemDAO.updateItem(b);
                        boxSelected.RemainVolume += b.Volume;
                    }
                    boxDAO.updateBox(boxSelected);

                    var alertController = UIAlertController.Create("Box Vuoto", "Il tuo box è stato svuotato", UIAlertControllerStyle.Alert);

                    alertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

                    Source.PresentViewController(alertController, true, null);

                    success(true);
                });

            action.BackgroundColor = UIColor.Blue;

            return action;
        }

        public override string TitleForDeleteConfirmation(UITableView tableView, NSIndexPath indexPath)
        {
            return "Elimina";
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            Source.PerformSegue("showDetailBoxController", null);
        }

    }
}
