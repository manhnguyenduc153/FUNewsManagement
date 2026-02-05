namespace Assignmen_PRN232__.Dto
{
    public class AuditLogDto
    {
        public int Id { get; set; }
        public string User { get; set; } = null!;
        public string Action { get; set; } = null!;
        public string Entity { get; set; } = null!;
        public string? BeforeData { get; set; }
        public string? AfterData { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class AuditLogSearchDto
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Entity { get; set; }
        public string? Action { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
