using System;
using System.Collections.Generic;

namespace FlashCardsAPI.Models.DB
{
    public partial class Tags
    {
        public Tags()
        {
            QuestionTags = new HashSet<QuestionTags>();
        }

        public int TagId { get; set; }
        public string TagDescription { get; set; }
        public int? ParentTagId { get; set; }
        public bool? IsActive { get; set; }
        public byte DomainId { get; set; }

        public Domains Domain { get; set; }
        public ICollection<QuestionTags> QuestionTags { get; set; }
    }
}
