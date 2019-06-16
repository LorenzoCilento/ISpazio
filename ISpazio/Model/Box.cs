using System;
using System.Collections.Generic;
using SQLite;
using System.Linq;
namespace NewTestArKit.Model
{
    [Table("Box")]
    public class Box : MyObject
    {
        public double RemainVolume { get; set; }

        public Box()
        {
        }

        public Box(MyObject obj)
        {
            Name = obj.Name;
            Height = obj.Height;
            Width = obj.Width;
            Depth = obj.Depth;
            Description = obj.Description;
            RemainVolume = Math.Round(Volume, 2);
        }

        public override string ToString()
        {
            return base.ToString() + " Volume rimanente: " + RemainVolume;
        }
    }
}
