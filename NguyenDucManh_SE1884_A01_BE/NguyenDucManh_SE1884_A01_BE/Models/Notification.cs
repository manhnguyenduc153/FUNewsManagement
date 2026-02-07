namespace NguyenDucManh_SE1884_A01_BE.Models;

public class Notification
{
    public int Id { get; set; }
    public string Message { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
}
