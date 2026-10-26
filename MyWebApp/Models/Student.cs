using System.ComponentModel.DataAnnotations;

namespace MyWebApp.Models
{
    public class Student
    {
        [Display(Name = "Mã sinh viên")]
        public int Id { get; set; }               // Mã sinh viên

        // Name: tối thiểu 4 ký tự, tối đa 100 ký tự
        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "Họ tên bắt buộc phải được nhập")]
        [StringLength(100, MinimumLength = 4,
            ErrorMessage = "Họ tên phải có từ 4 đến 100 ký tự")]
        public string? Name { get; set; }         // Họ tên

        // Email: bắt buộc, phải có đuôi gmail.com
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email bắt buộc phải được nhập")]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@gmail\.com$",
            ErrorMessage = "Email phải có đuôi @gmail.com")]
        public string? Email { get; set; }        // Email

        // Password: >= 8 ký tự, có chữ hoa, chữ thường, chữ số và ký tự đặc biệt
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Mật khẩu bắt buộc phải được nhập")]
        [StringLength(100, MinimumLength = 8,
            ErrorMessage = "Mật khẩu phải có từ 8 đến 100 ký tự")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
            ErrorMessage = "Mật khẩu phải có chữ hoa, chữ thường, chữ số và ký tự đặc biệt")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }     // Mật khẩu

        [Display(Name = "Ngành học")]
        [Required(ErrorMessage = "Vui lòng chọn ngành học")]
        public Branch? Branch { get; set; }       // Ngành học

        [Display(Name = "Giới tính")]
        [Required(ErrorMessage = "Vui lòng chọn giới tính")]
        public Gender? Gender { get; set; }       // Giới tính

        [Display(Name = "Hệ chính quy")]
        public bool IsRegular { get; set; }       // Hệ: true - chính quy, false - phi chính quy

        [Display(Name = "Địa chỉ")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Địa chỉ bắt buộc phải được nhập")]
        public string? Address { get; set; }      // Địa chỉ

        [Display(Name = "Ngày sinh")]
        [Range(typeof(DateTime), "1963-01-01", "2005-12-31",
            ErrorMessage = "Ngày sinh phải từ 01/01/1963 đến 31/12/2005")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Ngày sinh bắt buộc phải được nhập")]
        public DateTime DateOfBorth { get; set; } // Ngày sinh

        // Điểm: kiểu số thực, miền giá trị từ 0.0 đến 10.0
        [Display(Name = "Điểm")]
        [Required(ErrorMessage = "Điểm bắt buộc phải được nhập")]
        [Range(0.0, 10.0, ErrorMessage = "Điểm phải nằm trong khoảng từ 0.0 đến 10.0")]
        public double? Score { get; set; }

        public string? Avatar { get; set; }       // Ảnh đại diện
    }
}