using System;
using System.Collections.Generic;

namespace FlashCardsAPI.Models.DB
{
    public partial class Domains
    {
        public Domains()
        {
            Questions = new HashSet<Questions>();
            Tags = new HashSet<Tags>();
            TestAttempts = new HashSet<TestAttempts>();
            Titles = new HashSet<Titles>();
        }

        public byte DomainId { get; set; }
        public string Description { get; set; }

        public ICollection<Questions> Questions { get; set; }
        public ICollection<Tags> Tags { get; set; }
        public ICollection<TestAttempts> TestAttempts { get; set; }
        public ICollection<Titles> Titles { get; set; }
    }
}
