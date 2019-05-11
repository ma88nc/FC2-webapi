using System;
using System.Collections.Generic;

namespace FlashCardsAPI.Models.DB
{
    public partial class QuestionTags
    {
        public int TagId { get; set; }
        public Guid QuestionId { get; set; }
        public int QuestionTagId { get; set; }

        public Tags Tag { get; set; }
    }
}
