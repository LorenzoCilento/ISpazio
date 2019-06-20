using System;
using SQLite;
namespace NewTestArKit.Model
{
    [Table("Item")]
    public class Item : MyObject
    {
        public int Container { get; set; }

        public decimal CoordX { get; set; }

        public decimal CoordY { get; set; }

        public decimal CoordZ { get; set; }

        public decimal PackDimX { get; set; }

        public decimal PackDimY { get; set; }

        public decimal PackDimZ { get; set; }

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

        public void updateCoordinate(decimal x, decimal y, decimal z, decimal pdx, decimal pdy, decimal pdz)
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