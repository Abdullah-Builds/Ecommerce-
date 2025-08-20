public class Order
{
    public int Id { get; set; }
    public required string CustomerEmail { get; set; } 
    public required string ConfirmationToken { get; set; } 
    public bool IsConfirmed { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
