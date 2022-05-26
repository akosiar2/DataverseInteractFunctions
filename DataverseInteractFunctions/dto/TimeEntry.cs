using System;
using System.Collections.Generic;
using System.Text;

namespace DataverseInteractFunctions.Dto
{
    public class TimeEntry
    {
        public string schema {get;set;}
        public string type { get; set; }
        public Property properties { get; set; }
        public string[] required { get; set; }
    }

    public class Property
    {
        public Attribute StartOn { get; set; }
        public Attribute EndOn { get; set; }
    }

    public class Attribute 
    { 
        public string type { get; set; }
        public string format { get; set; }
    }
}
