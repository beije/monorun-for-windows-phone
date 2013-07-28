using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace monorun
{
    public class Highscore
    {
        public int id { get; set; }
        public int sourceid { get; set; }
        public int dateline { get; set; }
        public int score { get; set; }
        public int position { get; set; }
        public string secretkey { get; set; }
        public string username { get; set; }
    }
}
