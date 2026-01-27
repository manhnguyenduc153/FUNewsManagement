using NguyenDucManh_SE1884_A01_BE.Models;
using NguyenDucManh_SE1884_A01_BE.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace NguyenDucManh_SE1884_A01_BE.Dto
{
    public class CategoryDto
    {
        public short CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public string CategoryDesciption { get; set; } = null!;

        public short? ParentCategoryId { get; set; }

        public string ParentCategoryName { get; set; } = null!;

        public bool? IsActive { get; set; }

        public int ArticleCount { get; set; }
    }

    public class CategorySaveDto
    {
        public short CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
        public string CategoryName { get; set; } = null!;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string CategoryDesciption { get; set; } = null!;

        public short? ParentCategoryId { get; set; }

        public bool? IsActive { get; set; }
    }

    public class CategorySearchDto : BaseSearchDto
    {
        
    }
}
