using System.ComponentModel.DataAnnotations;
namespace FC_Application.Models
{


    public class Location
    {
        public string? SurveyorName { get; set; }

        [Required]
        public string LocationID { get; set; }

        [Required]
        public string SalesOrderID { get; set; }

        public string? ClientLocationIdentifier { get; set; }

        [Required]
        public string Status { get; set; }

        public string? ServiceDueDate { get; set; }
        public string? ServiceDate { get; set; }

        [Required]
        public string Client { get; set; }

        public string? Customer { get; set; }

        [Required]
        public string BrandName { get; set; }

        public string? LocationNumber { get; set; }
        public string? LocationNickname { get; set; }

        [Required]
        public string Service { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Zip { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? ManagerName { get; set; }
        public string? L1ManagerName { get; set; }
        public string? L1ManagerEmail { get; set; }
        public string? L1ManagerPhone { get; set; }
        public string? L2ManagerName { get; set; }
        public string? L2ManagerEmail { get; set; }
        public string? L2ManagerPhone { get; set; }
        public string? AssetsVerified { get; set; }
        public string? AssetCount { get; set; }

        [Required]
        public string SqFt { get; set; }

        public string? Value { get; set; }
        public string? Notes { get; set; }
        public string? Verifier { get; set; }
        public string? DateVerified { get; set; }
    }



}
