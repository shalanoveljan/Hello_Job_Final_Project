using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public record RegisterDto
    {
        [Required(ErrorMessage = "İstifadəçi adı daxil edilməlidir.")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "İstifadəçi adı 2 ilə 25 simvol aralığında olmalıdır")]
        public string Username { get; set; } = null!;
        [Required(ErrorMessage = "E-poçt vacibdir")]
        [StringLength(255, MinimumLength = 10, ErrorMessage = "E-poçt ünvanı 10 ilə 255 simvol aralığında olmalıdır")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Yanlış e-poçt ünvanı")]
        //[RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Yanlış e-poçt formatı")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Şifrə vacibdir")]
        [StringLength(25, MinimumLength = 8, ErrorMessage = "Şifrə 8 ilə 25 simvol aralığında olmalıdır")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "Şifrənin təsdiqi vacibdir")]
        [StringLength(25, MinimumLength = 8, ErrorMessage = "Şifrə 8 ilə 25 simvol aralığında olmalıdır")]
        [Compare(nameof(Password), ErrorMessage = "Şifrə eyni olmalıdır")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;

        public string? Role { get; init; }
        [Range(typeof(bool), "true", "true", ErrorMessage = "Şərtləri qəbul etməlisiniz.")]
        public bool Terms { get; set; }

    }
}
