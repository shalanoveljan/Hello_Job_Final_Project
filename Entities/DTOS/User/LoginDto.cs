using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public record LoginDto
    {
        [Required(ErrorMessage = "İstifadəçi adı və ya E-poçt ünvanı daxil edilməlidir.")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "İstifadəçi adı və ya E-poçt ünvanı 2 ilə 255 simvol aralığında olmalıdır.")]
        [RegularExpression(@"^(?:(?![@._])[a-zA-Z0-9@._-]{3,})$|^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Yanlış istifadəçi adı və ya E-poçt formatı.")]
        public string UserNameOrEmail { get; set; } = null!;
        [Required(ErrorMessage = "Parol daxil edilməlidir.")]
        [DataType(DataType.Password)]
        [StringLength(25, MinimumLength = 8, ErrorMessage = "Parol 8 ilə 25 simvol aralığında olmalıdır.")]
        public string Password { get; set; } = null!;
        [Required]
        public bool RememberMe { get; set; }


    }
}
