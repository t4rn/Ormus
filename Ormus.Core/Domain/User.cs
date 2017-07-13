using System;

namespace Ormus.Core.Domain
{
    public class User : Entity<int>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
