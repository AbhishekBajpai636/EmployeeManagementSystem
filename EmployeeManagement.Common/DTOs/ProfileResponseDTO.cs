using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Common.DTOs
{
    public class ProfileResponseDTO
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
