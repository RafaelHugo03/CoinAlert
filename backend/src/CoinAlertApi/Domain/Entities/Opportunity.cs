using CoinAlertApi.Domain.Enums;

namespace CoinAlertApi.Domain.Entities
{
    public class Opportunity
    {
        public string Id { get; set; }

        public string CryptoId { get; set; }

        public OpportunityType Type { get; set; }

        public decimal TargetPrice { get; set; }

        public string Currency { get; set; }

        public OpportunityStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? TriggeredAt { get; set; }
    }
}