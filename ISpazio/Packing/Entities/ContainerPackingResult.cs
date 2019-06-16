using System;
using System.Collections.Generic;

namespace NewTestArKit.Packing.Entities
{
    /// <summary>
    /// The container packing result.
    /// </summary>
    public class ContainerPackingResult
    {
        #region Constructors

        public ContainerPackingResult()
        {
            this.AlgorithmPackingResults = new List<AlgorithmPackingResult>();
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the container ID.
        /// </summary>
        /// <value>
        /// The container ID.
        /// </value>
     
        public int ContainerID { get; set; }


        public List<AlgorithmPackingResult> AlgorithmPackingResults { get; set; }

        #endregion Public Properties
    }
}