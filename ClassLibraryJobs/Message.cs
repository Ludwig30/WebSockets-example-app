using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryJobs
{
    public class Message
    {        
        public required string Action { get; set; }
        public int Duration { get; set; }
        public string Id { get; set; }
    }
}
