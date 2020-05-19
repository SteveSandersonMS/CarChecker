using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarChecker.Shared
{
    public class Vehicle
    {
        [Key]
        public string LicenseNumber { get; set; }

        [Required]
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        [Range(1, 1000000)]
        public int Mileage { get; set; }

        [Required]
        public FuelLevel Tank { get; set; }

        public List<InspectionNote> Notes { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; }
    }
}
