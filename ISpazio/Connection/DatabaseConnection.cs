using System;
using System.IO;
using SQLite;
using NewTestArKit.Model;
namespace NewTestArKit
{
    public class DatabaseConnection
    {
        private SQLiteConnection sqlite;
        public SQLiteConnection Sqlite
        {
            get { return sqlite; }
        }

        private string dbpath;

        public DatabaseConnection()
        {
            dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ISpazioDatabase.db");

            var exist = File.Exists(dbpath);

            try
            {
                sqlite = new SQLiteConnection(dbpath);
                sqlite.CreateTable<Item>();
                sqlite.CreateTable<Box>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
        }


    }
}
