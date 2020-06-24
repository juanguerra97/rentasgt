using rentasgt.Domain.Enums;
using System;
using System.Collections.Generic;

namespace rentasgt.Domain.Entities
{
    /// <summary>
    /// Class to store information about a rent request made by a requestor 
    /// to the owner of the product they want to rent
    /// </summary>
    public class RentRequest
    {

        public static readonly int MAX_PLACE_LENGTH = 128;

        public RentRequest()
        {
            Events = new List<RequestEvent>();
        }

        public RentRequest(DateTime requestDate, Product product, AppUser requestor,
            DateTime startDate, DateTime endDate, string? place = null, Rent? rent = null)
            : this()
        {
            RequestDate = requestDate;
            Product = product;
            Requestor = requestor;
            StartDate = startDate;
            EndDate = endDate;
            Place = place;
            Rent = rent;
        }

        public long Id { get; set; }

        public RequestStatus Status { get; set; }

        /// <summary>
        /// Date and time of the request creation
        /// </summary>
        public DateTime RequestDate { get; set; }

        /// <summary>
        /// Product that the requestor wants to rent
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// User that wants to rent the product from the owner
        /// </summary>
        public string RequestorId { get; set; }
        public AppUser Requestor { get; set; }

        /// <summary>
        /// Date and time when the requestor wants to start renting the product
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Date and time when the requestor wants to finish renting the product
        /// </summary>
        public DateTime EndDate { get; set; }


        /// <summary>
        /// Place where the requestor wants to receive the product.
        /// </summary>
        public string? Place { get; set; }

        /// <summary>
        /// List of events related to the request 
        /// </summary>
        public List<RequestEvent> Events { get; set; }

        /// <summary>
        /// If the request is accepted by the owner there will be a rent instance 
        /// with the information about the rent process
        /// </summary>
        public Rent? Rent { get; set; }

    }
}
