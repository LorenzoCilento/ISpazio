using System;
using SQLite;
namespace NewTestArKit.Model
{
    [Table("Item")]
    public class Item : MyObject
    {
        public int Container { get; set; }

        public Item()
        {
        }

        public Item(MyObject obj)
        {
            Name = obj.Name;
            Height = obj.Height;
            Width = obj.Width;
            Depth = obj.Depth;
            Description = obj.Description;
        }

        public override string ToString()
        {
            return base.ToString() + " Container: " + Container;
        }
    }
}
