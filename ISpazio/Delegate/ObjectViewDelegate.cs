using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using NewTestArKit.Model;
namespace NewTestArKit.Delegate
{
    public class ObjectViewDelegate : UITableViewDelegate
    {
        private ObjectViewController Source { get; set; }

        private List<Item> selectedItems = new List<Item>();
        public List<Item> SelectedItems { get => selectedItems; }

        public List<NSIndexPath> SelectedIndexPaths { get; set; }

        public ObjectViewDelegate(ObjectViewController source)
        {
            Source = source;
            SelectedIndexPaths = new List<NSIndexPath>();
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
                UIContextualActionStyle.Normal, "Inserisci",
                (insertAction, view, success) =>
                {
                    Console.WriteLine("inserisci");

                    BoxViewController boxView = new BoxViewController();


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
            if (Source.TableView.Editing != true)
                Source.PerformSegue("showDetailObjectController", null);
            else
            {
                var item = Source.Items[indexPath.Row];
                if (!selectedItems.Contains(item))
                {
                    selectedItems.Add(item);
                    SelectedIndexPaths.Add(indexPath);
                }
            }
        }

    }
}
