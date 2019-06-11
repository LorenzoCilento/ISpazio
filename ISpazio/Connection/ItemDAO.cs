using System;
using NewTestArKit.Model;
using SQLite;
using System.Collections.Generic;

namespace NewTestArKit.Connection
{
    public class ItemDAO
    {
        public ItemDAO()
        {
        }

        public List<Item> getAllItem()
        {
            DatabaseConnection connection = new DatabaseConnection();
            List<Item> items = new List<Item>();

            var data = connection.Sqlite;

            var table = data.Table<Item>();

            foreach (var i in table)
                items.Add(i);

            return items;
        }

        public Item getItem(int id)
        {
            DatabaseConnection connection = new DatabaseConnection();

            var data = connection.Sqlite;

            return data.Get<Item>(id);
        }

        public void insertItem(Item item)
        {
            DatabaseConnection connection = new DatabaseConnection();

            var data = connection.Sqlite;

            data.Insert(item);
        }

        public void deleteItem(Item item)
        {
            DatabaseConnection connection = new DatabaseConnection();

            var data = connection.Sqlite;

            data.Delete(item);
        }

        public void updateItem(Item item)
        {
            DatabaseConnection connection = new DatabaseConnection();

            var data = connection.Sqlite;

            data.Update(item);
        }

        public List<Item> getAllItemInBox(int id)
        {
            List<Item> items = new List<Item>();

            DatabaseConnection connection = new DatabaseConnection();

            var data = connection.Sqlite;

            items = data.Query<Item>("SELECT * FROM Item WHERE Container=?",id);

            return items;
        }
    }
}
