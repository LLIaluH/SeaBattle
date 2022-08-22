using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestWebForms.App;

namespace TestWebForms.Models
{
    public class SheepsUser
    {
        public string ConnectionId { get; set; }
        public string NameRoom { get; set; }
        public bool Ready { get; set; }
        public List<Cell> Cells { get; set; }
    }
}