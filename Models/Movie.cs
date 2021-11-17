using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp.Models
{
    public class Movie
    {
        public int ID { get; set; }
        
        [Display(Name = "タイトル")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "{0}は{2}文字以上{1}文字以内で入力してください")]
        [Required(ErrorMessage = "{0}は必須項目です")]
        public string Title { get; set; }

        [Display(Name = "公開日")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; } = DateTime.Today;

        [Display(Name = "ジャンル")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "{0}は半角ローマ字で入力してください")]
        [Required(ErrorMessage = "{0}は必須入力です")]
        [StringLength(30, ErrorMessage = "{0}は{1}文字以内で入力してください")]
        public string Genre { get; set; }

        [Display(Name = "価格")]
        [Range(1, 100, ErrorMessage = "{0}は{1}と{2}の間で入力してください")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; } = 100;

        [Display(Name = "評価")]
        [Range(1, 5, ErrorMessage = "{0}は{1}と{2}の間で入力してください")]
        [Required(ErrorMessage = "{0}は必須項目です")]
        public int Rating { get; set; }

        public int Order { get; set; }
    }
}