using System;
using SQLite;
using System.Collections.Generic;
using System.Security.Principal;
using System.Linq;

namespace NewTestArKit.Model
{
    public class MyObject
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }

        public string Name { get; set; }

        public double Height { get; set; }

        public double Width { get; set; }

        public double Depth { get; set; }

        public double Volume { get { return Math.Round(Height * Width * Depth, 2); } }

        public string Description { get; set; }

        public MyObject() { }

        public MyObject(string name, double height, double width, double depth, string description)
        {
            Name = name;
            Height = height;
            Width = width;
            Depth = depth;
            Description = description;
        }

        public List<double> getAllSortArea()
        {
            List<double> tmp = new List<double>();

            double[] value = new double[] { Height, Width, Depth };

            for (int i = 0; i < value.Length - 1; i++)
            {
                var a = value[i];
                for (int z = i + 1; z < value.Length; z++)
                {
                    tmp.Add(a * value[z]);
                }
            }

            tmp.Sort();

            return tmp;
        }

        public List<double> getDimensionSortList()
        {
            List<double> dimension = new List<double>() { Height, Width, Depth };

            dimension.Sort();

            return dimension;
        }

        public bool allDistanceNotZero()
        {
            if (Height.Equals(0) || Width.Equals(0) || Depth.Equals(0))
                return false;
            else
                return true;
        }


        public override string ToString()
        {
            return Name + ", " + Description + ". H: " + Height + " W: " + Width + " D: " + Depth + " V: " + Volume;
        }
    }
}
