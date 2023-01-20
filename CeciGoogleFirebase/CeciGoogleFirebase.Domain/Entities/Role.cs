using System;
using System.Collections.Generic;
using System.Text;

namespace CeciGoogleFirebase.Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<User> User { get; set; }
    }
}
