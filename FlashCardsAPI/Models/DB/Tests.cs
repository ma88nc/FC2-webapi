using System;
using System.Collections.Generic;

namespace FlashCardsAPI.Models.DB
{
    public partial class Tests
    {
        public Guid AttemptId { get; set; }
        public byte DomainId { get; set; }
        public string UserId { get; set; }
    }
}
