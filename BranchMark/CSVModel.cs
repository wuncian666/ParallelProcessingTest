using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchMark
{
    //"18,Josias,Consterdine,jconsterdineh@scribd.com,Male,149.190.102.195"
    public class CSVModel
    {
        public String ID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public String Gender { get; set; }
        public String IP { get; set; }
    }
}