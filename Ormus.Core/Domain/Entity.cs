using System;

namespace Ormus.Core.Domain
{
    public class Entity<T>
    {
        public T Id { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
