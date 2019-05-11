using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCardsAPI.Models
{
    public class Node
    {
        public Node Parent { get; set; }
        public int ParentId { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
     //   public int Position { get; private set; }
        private readonly List<Node> children = new List<Node>();
        public List<Node> Children { get { return children; } }
    }
}
