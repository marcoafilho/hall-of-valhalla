using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HallOfValhalla.Domain
{
    public class Convention
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public HashSet<Talk> Talks { get; set; }
    }
}
