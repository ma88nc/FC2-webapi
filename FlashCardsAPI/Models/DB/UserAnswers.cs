using System;
using System.Collections.Generic;

namespace FlashCardsAPI.Models.DB
{
    public partial class UserAnswers
    {
        public Guid TestAttemptId { get; set; }
        public Guid QuestionId { get; set; }
        public string UserAnswer { get; set; }
        public bool? IsCorrect { get; set; }
        public int UserAnswerId { get; set; }

        public Questions Question { get; set; }
        public TestAttempts TestAttempt { get; set; }
    }
}
