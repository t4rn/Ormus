using System;

namespace Ormus.Core.Domain
{
    public class UserRole : Entity<int>
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public bool Ghost { get; set; }
    }
}
