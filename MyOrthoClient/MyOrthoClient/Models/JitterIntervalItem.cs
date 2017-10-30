using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOrthoClient.Models
{
    public class JitterIntervalItem
    {
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public double Jitter { get; set; }
    }
}
