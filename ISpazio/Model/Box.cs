using System;
using SQLite;
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

        public bool insertIntoBox(Item item)
        {
            var dimensionBox = getDimensionSortList();
            var dimensionItem = item.getDimensionSortList();

            Console.WriteLine("Volume box " + Volume + " Volume item " + item.Volume);

            if (item.Volume > Volume)
                return false;
           
            Console.WriteLine("Valori area box");
            foreach (var z in dimensionBox)
                Console.WriteLine(z);
            Console.WriteLine("Valori area item");
            foreach (var z in dimensionItem)
                Console.WriteLine(z);
            for (int i = 0; i < dimensionBox.Count; i++)
            {
                if (dimensionItem[i] > dimensionBox[i])
                    return false;
            }
            return true;
        }

        public override string ToString()
        {
            return base.ToString() + " Volume rimanente: " + RemainVolume;
        }
    }
}
