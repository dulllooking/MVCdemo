using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVCdemo.Models
{
    public class Permissions
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(200)]
        [Display(Name = "權限名稱")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(200)]
        [Display(Name = "Value")]
        public string Value { get; set; }

        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public virtual Permissions Parent { get; set; }

        //[JsonIgnore]
        public virtual ICollection<Permissions> PermissionsCollection { get; set; } //children
    }
}