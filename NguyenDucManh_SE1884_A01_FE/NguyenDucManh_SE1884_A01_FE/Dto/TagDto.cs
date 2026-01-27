using Assignmen_PRN232_1.DTOs.Common;

namespace Assignmen_PRN232__.Dto
{
    public class TagDto
    {
        public int TagId { get; set; }

        public string? TagName { get; set; }

        public string? Note { get; set; }
    }

    public class TagSaveDto
    {
        public int TagId { get; set; }

        public string? TagName { get; set; }

        public string? Note { get; set; }
    }

    public class TagSearchDto : BaseSearchDto
    {
        // Nếu sau này Tag có filter riêng thì thêm ở đây
    }
}
