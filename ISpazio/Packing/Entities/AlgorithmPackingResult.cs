﻿using System;
using System.Collections.Generic;

namespace NewTestArKit.Packing.Entities
{
    public class AlgorithmPackingResult
    {
        #region Constructors

        public AlgorithmPackingResult()
        {
            this.PackedItems = new List<Item>();
            this.UnpackedItems = new List<Item>();
        }

        #endregion Constructors

        #region Public Properties

       
        public int AlgorithmID { get; set; }

       
        public string AlgorithmName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether all of the items are packed in the container.
        /// </summary>
        /// <value>
        /// True if all the items are packed in the container; otherwise, false.
        /// </value>
      
        public bool IsCompletePack { get; set; }

        /// <summary>
        /// Gets or sets the list of packed items.
        /// </summary>
        /// <value>
        /// The list of packed items.
        /// </value>
       
        public List<Item> PackedItems { get; set; }

        /// <summary>
        /// Gets or sets the elapsed pack time in milliseconds.
        /// </summary>
        /// <value>
        /// The elapsed pack time in milliseconds.
        /// </value>

        public long PackTimeInMilliseconds { get; set; }

        /// <summary>
        /// Gets or sets the percent of container volume packed.
        /// </summary>
        /// <value>
        /// The percent of container volume packed.
        /// </value>
  
        public decimal PercentContainerVolumePacked { get; set; }

        /// <summary>
        /// Gets or sets the percent of item volume packed.
        /// </summary>
        /// <value>
        /// The percent of item volume packed.
        /// </value>
  
        public decimal PercentItemVolumePacked { get; set; }

        /// <summary>
        /// Gets or sets the list of unpacked items.
        /// </summary>
        /// <value>
        /// The list of unpacked items.
        /// </value>
   
        public List<Item> UnpackedItems { get; set; }

        #endregion Public Properties
    }
}
