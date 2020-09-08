using System;
using System.Collections.Generic;
using rentasgt.Domain.Common;
using rentasgt.Domain.Enums;

namespace rentasgt.Domain.Entities
{

    /// <summary>
    /// Information about the rent process
    /// </summary>
    public class Rent : AuditableEntity
    {

        public Rent()
        { }

        public Rent(RentStatus status, DateTime? startDate = null, DateTime? endDate = null)
        {
            Status = status;
            StartDate = startDate;
            EndDate = endDate;
        }

        /// <summary>
        /// Accepted request that started the rent process
        /// </summary>
        public long RequestId { get; set; }
        public RentRequest Request { get; set; }

        public RentStatus Status { get; set; }

        /// <summary>
        /// Date and time when the owner gives the product to the requestor
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Date and time when the requetor gives back the product to its owner
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Cost of the rent, calculated based on the StartDate and EndDate 
        /// and includes fines for delays, etc.
        /// </summary>
        public decimal? TotalCost { get; set; }

        public List<RentEvent> Events { get; set; }

    }
}
