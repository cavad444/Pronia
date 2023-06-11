using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;


namespace Pronia.Models
{
    public class Setting
    {
        [System.ComponentModel.DataAnnotations.Required]
        [MaxLength(30)]
        public string Key { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [MaxLength(300)]
        public string Value { get; set; }
    }
}
