using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

namespace FlashCardsAPI.Models.DB
{
    public class QuestionsDTO
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string Answer { get; set; }
        public int? TitleId { get; set; }
        public string TitleDescription { get; set; }
        public string Reference { get; set; }
        public bool? HasImage { get; set; }
        public string ImagePath { get; set; }
        public byte? TimeMultiplier { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsVerified { get; set; }
     //   public DateTime? DateAdded { get; set; }
        public byte DomainId { get; set; }
    }
}
