using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPMExamPodgot.Models
{
    public enum UserRole
    {
        Client,
        Admin
    }

    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = "";
        public string Password { get; set; } = "";
        public UserRole Role { get; set; }
    }
}
