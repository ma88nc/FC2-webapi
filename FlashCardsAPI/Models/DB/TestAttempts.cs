using System;
using System.Collections.Generic;

namespace FlashCardsAPI.Models.DB
{
    public partial class TestAttempts
    {
        public TestAttempts()
        {
            UserAnswers = new HashSet<UserAnswers>();
        }

        public Guid TestAttemptId { get; set; }
        public byte DomainId { get; set; }
        public string UserId { get; set; }
        public DateTime? TestDate { get; set; }

        public Domains Domain { get; set; }
        public ICollection<UserAnswers> UserAnswers { get; set; }
    }
}
