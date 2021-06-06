using System;
using System.ComponentModel.DataAnnotations;

namespace HallOfValhalla.Domain
{
    public class Talk
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Speaker { get; set; }
    }
}
