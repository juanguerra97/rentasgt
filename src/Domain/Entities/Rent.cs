﻿using System;
using rentasgt.Domain.Enums;

namespace rentasgt.Domain.Entities
{

    /// <summary>
    /// Information about the rent process
    /// </summary>
    public class Rent
    {

        public Rent()
        { }

        public Rent(RentStatus status, ChatRoom? chatRoom = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            Status = status;
            ChatRoom = chatRoom;
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
        /// Chat room where the owner and the requestor can send messages to each other
        /// </summary>
        public ChatRoom? ChatRoom { get; set; }

    }
}
