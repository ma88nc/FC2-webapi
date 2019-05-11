using System;
using System.Collections.Generic;

namespace FlashCardsAPI.Models.DB
{
    public partial class Titles
    {
        public Titles()
        {
            Questions = new HashSet<Questions>();
        }

        public int TitleId { get; set; }
        public string Description { get; set; }
        public byte DomainId { get; set; }

        public Domains Domain { get; set; }
        public ICollection<Questions> Questions { get; set; }
    }
}
