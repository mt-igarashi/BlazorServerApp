using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp.Models
{
    public class Author 
    {
        public int ID { get; set; }

        public int MovieID { get; set; }

       [ForeignKey("MovieID")]
        public Movie Movie { get; set; }

        [Display(Name = "名前")]
        [StringLength(60, ErrorMessage="名前は６０文字以内で入力してください")]
        [Required]
        public string Name { get; set; }
        
        [Display(Name = "性別")]
        [StringLength(1, MinimumLength = 1, ErrorMessage="性別を選択してください")]
        [Required]
        public string Sex { get; set; }
        
        [Display(Name = "年齢")]
        [Range(1, 100, ErrorMessage="年齢は1～100までの数値を入力してください")]
        public int Age { get; set; }
        
        [ConcurrencyCheck]
        public DateTime UpdateDate { get; set; }
    }
}