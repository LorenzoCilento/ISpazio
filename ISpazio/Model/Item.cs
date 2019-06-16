using System;
using SQLite;
namespace NewTestArKit.Model
{
    [Table("Item")]
    public class Item : MyObject
    {
        public int Container { get; set; }

        public int CoordX { get; set; }

        public int CoordY { get; set; }

        public int CoordZ { get; set; }

        public int PackDimX { get; set; }

        public int PackDimY { get; set; }

        public int PackDimZ { get; set; }

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
            CoordX = CoordY = CoordZ = PackDimX = PackDimY = PackDimZ = 0;
        }

        public void updateCoordinate(int x, int y, int z, int pdx, int pdy, int pdz)
        {
            CoordX = x;
            CoordY = y;
            CoordZ = z;
            PackDimX = pdx;
            PackDimY = pdy;
            PackDimZ = pdz;
        }

        public override string ToString()
        {
            return base.ToString() + " Container: " + Container;
        }
    }
}
