﻿namespace Domain.Entities
{
    public class Shipment
    {
        public Guid Id { get; set; }
        public Order Order { get; set; } = null!;
        public Guid ProviderId { get; set; }
        public Guid? PackageId { get; set; }

        public decimal ShipmentPrice { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Country { get; set; }
        public required string City { get; set; }
        public required string Street { get; set; }
        public required string PostalCode { get; set; }
        public int HomeNumber { get; set; }
        public required string PhoneNumber { get; set; }
        public required string AreaCode { get; set; }
        public required string TrackingLink { get; set; }
    }
}
