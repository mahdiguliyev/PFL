using System;
using System.ComponentModel.DataAnnotations;

namespace PFL.Models.DTO
{
    public class UserBaseModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "İstifadəçi adı daxil edilməyib.")]
        [StringLength(50)]
        [Display(Name = "İstifadəçi adı")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Şifrə daxil edilməyib")]
        [StringLength(50)]
        [Display(Name = "Şifrə")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required(ErrorMessage = "Ad daxil edilməyib.")]
        [StringLength(50)]
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [StringLength(50)]
        [Display(Name = "Ata adı")]
        public string FatherName { get; set; }

        [Required(ErrorMessage = "Email daxil edilməyib.")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Yalnış email formatı")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "İstifadəçinin statusunu seçilməlidir.")]
        [Display(Name = "İstifadəçinin statusu")]
        public bool IsActive { get; set; }
    }

    public class UserAdminDto : UserBaseModel
    {

    }

    public class UserClubAdminDto : UserBaseModel
    {
        [Required(ErrorMessage = "Klub seçilməyib.")]
        [Display(Name = "Klub")]
        public int ClubId { get; set; }
    }

    public class UserRefereeDto : UserBaseModel
    {
        [Required(ErrorMessage = "Hakim seçilməyib.")]
        [Display(Name = "Hakim")]
        public int RefereeId { get; set; }
    }
}