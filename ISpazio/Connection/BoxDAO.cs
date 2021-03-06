﻿using System;
using NewTestArKit.Model;
using System.Collections.Generic;
namespace NewTestArKit.Connection
{
    public class BoxDAO
    {
        public BoxDAO()
        {
        }

        public List<Box> getAllBox()
        {
            List<Box> boxes = new List<Box>();

            DatabaseConnection connection = new DatabaseConnection();

            var data = connection.Sqlite;

            var table = data.Table<Box>();

            foreach (var b in table)
                boxes.Add(b);

            return boxes;
        }

        public Box getBox(int id)
        {
            DatabaseConnection connection = new DatabaseConnection();

            var data = connection.Sqlite;

            return data.Get<Box>(id);
        }

        public void insertBox(Box box)
        {
            DatabaseConnection connection = new DatabaseConnection();

            var data = connection.Sqlite;

            data.Insert(box);
        }

        public void updateBox(Box box)
        {
            DatabaseConnection connection = new DatabaseConnection();

            var data = connection.Sqlite;

            data.Update(box);
        }

        public void deleteBox(Box box)
        {
            DatabaseConnection connection = new DatabaseConnection();

            var data = connection.Sqlite;

            data.Delete(box);
        }

        public void resetBox(int id)
        {
            var box = getBox(id);

            box.RemainVolume = box.Volume;

            ItemDAO itemDAO = new ItemDAO();

            var items = itemDAO.getAllItemInBox(id);

            foreach(var i in items)
            {
                var item = i;
                item.Container = 0;
                itemDAO.updateItem(item);
            }

            updateBox(box);
        }
    }
}
