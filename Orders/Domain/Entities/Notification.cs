namespace Domain.Entities
{
    public abstract class Notification
    {
        public Guid Id { get; set; }
        public required string Text { get; set; }
        public DateTime NotificationTimestamp { get; set; }
    }
}
