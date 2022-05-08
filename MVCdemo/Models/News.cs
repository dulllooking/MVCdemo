using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVCdemo.Models
{
    public class News
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(200)]
        [Display(Name = "聯絡人")]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "內容")]
        public string Article { get; set; }

        [Required]
        [Display(Name = "內容1")]
        public string Articles { get; set; }

        [Display(Name = "開始時間")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "結束時間")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")] /*想自訂格式時*/
        public DateTime? EndDate { get; set; }

        [Display(Name = "建立時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime initDate { get; set; }


        //ForeignKey
        [Display(Name = "類別")]
        public int CtaegoryId { get; set; }
        [ForeignKey("CtaegoryId")]
        public virtual NewsCatalog Catalog { get; set; }


        public NewsClass NewsClass { get; set; }
    }
}