using System.ComponentModel.DataAnnotations;

namespace FC_Application.Models
{
    public class Finance
    {
        public int SrNo { get; set; } = 0;
        [Required]
        public string SONumber { get; set; }
        [Required]
        public string Client { get; set; }
        [Required]
        public string Customer { get; set; }
        [Required]
        public string DateReceived { get; set; }
        [Required]
        public string DateDue { get; set; }
        [Required]
        public string DateSubmitted { get; set; }
        [Required]
        public string ExpirationDate { get; set; }
        [Required]
        public string POCName { get; set; }
        [Required]
        public string POCEmail { get; set; }
        [Required]
        public string POCPhone { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string ServiceType { get; set; }
        [Required]
        public int UnitQuantity { get; set; }
        [Required]
        public decimal ProposalTotal { get; set; }
    }
}
