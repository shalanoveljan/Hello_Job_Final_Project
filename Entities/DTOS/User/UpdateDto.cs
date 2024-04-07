using HelloJob.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public record UpdateDto
    {
        [Required(ErrorMessage = "İstifadəçi adı daxil edilməlidir.")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "İstifadəçi adı 2 ilə 25 simvol aralığında olmalıdır")]
        public string UserName { get; set; } = null!;
        [DataType(DataType.Password)]
        [StringLength(25, MinimumLength = 8, ErrorMessage = "Şifrə 8 ilə 25 simvol aralığında olmalıdır")]
        public string? OldPassword { get; set; }
        [DataType(DataType.Password)]
        [StringLength(25, MinimumLength = 8, ErrorMessage = "Şifrə 8 ilə 25 simvol aralığında olmalıdır")]
        public string? NewPassword { get; set; }




    }
}
