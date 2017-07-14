using System;

namespace Ormus.Core.Domain
{
    public class User : Entity<int>
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public UserRole Role { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool Ghost { get; set; }
    }
}
