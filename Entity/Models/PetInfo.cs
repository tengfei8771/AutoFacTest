using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class PetInfo
    {
        public string PetId { get; set; }
        public string PetName { get; set; }
        public int? PetAge { get; set; }
        public int? PetSex { get; set; }
        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }
    }
}
