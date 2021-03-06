﻿using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using NewTestArKit.Model;
using NewTestArKit.Connection;
namespace NewTestArKit.Delegate
{
    public class ObjectViewDelegate : UITableViewDelegate
    {
        private ObjectViewController Source { get; set; }

        private List<Item> selectedItems = new List<Item>();
        public List<Item> SelectedItems { get => selectedItems; }

        private ItemDAO itemDAO;

        public List<NSIndexPath> SelectedIndexPaths { get; set; }

        public ObjectViewDelegate(ObjectViewController source)
        {
            Source = source;
            SelectedIndexPaths = new List<NSIndexPath>();
            itemDAO = new ItemDAO();
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
                UIContextualActionStyle.Normal, "Duplica",
                (insertAction, view, success) =>
                {
                    Console.WriteLine("duplica");

                    var item = Source.Items[row];
                    var tmp = new Item(new MyObject(item.Name, item.Height, item.Width, item.Depth, item.Description));

                    itemDAO.insertItem(tmp);

                    Source.reloadData();
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

        public override void RowDeselected(UITableView tableView, NSIndexPath indexPath)
        {
            var item = Source.Items[indexPath.Row];
            if (selectedItems.Contains(item))
            {
                selectedItems.Remove(item);
                SelectedIndexPaths.Remove(indexPath);
            }
        }

        private void removeItem(Item item)
        {
            foreach (var i in selectedItems)
            {
                if (i.Id.Equals(item.Id))
                {
                    selectedItems.Remove(i);
                    break;
                }
            }
        }

        private void removeIndexPath(NSIndexPath indexPath)
        {
            foreach (var i in SelectedIndexPaths)
            {
                if (i.GetIndexes().Equals(indexPath.GetIndexes()))
                {
                    SelectedIndexPaths.Remove(i);
                    break;
                }
            }
        }
    }
}
