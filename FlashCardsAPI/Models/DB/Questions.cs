using System;
using System.Collections.Generic;

namespace FlashCardsAPI.Models.DB
{
    public partial class Questions
    {
        public Questions()
        {
            UserAnswers = new HashSet<UserAnswers>();
            QuestionTags = new HashSet<QuestionTags>();
        }

        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string Answer { get; set; }
        public int? TitleId { get; set; }
        public string Reference { get; set; } = string.Empty;
        public bool? HasImage { get; set; } = false;
        public string ImagePath { get; set; } = string.Empty;
        public byte? TimeMultiplier { get; set; } = 1;
        public bool? IsActive { get; set; } = false;
        public bool? IsVerified { get; set; } = true;
        public DateTime? DateAdded { get; set; } = DateTime.Now;
        public byte DomainId { get; set; }

        public Domains Domain { get; set; }
        public Titles Title { get; set; }
        public ICollection<UserAnswers> UserAnswers { get; set; }
        public HashSet<QuestionTags> QuestionTags { get; set; }
    }
}
