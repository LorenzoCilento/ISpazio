using System;
using System.Collections.Generic;
using NewTestArKit.Packing.Entities;

namespace NewTestArKit.Packing.Algorithms
{
    public abstract class AlgorithmBase
    {
        public abstract ContainerPackingResult Run(Container container, List<Item> items);
    }
}
