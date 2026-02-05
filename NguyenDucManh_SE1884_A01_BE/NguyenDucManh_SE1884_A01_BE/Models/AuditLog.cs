namespace NguyenDucManh_SE1884_A01_BE.Models;

public class AuditLog
{
    public int Id { get; set; }
    public string User { get; set; } = null!;
    public string Action { get; set; } = null!;
    public string Entity { get; set; } = null!;
    public string? BeforeData { get; set; }
    public string? AfterData { get; set; }
    public DateTime Timestamp { get; set; }
}
