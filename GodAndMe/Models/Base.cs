using System;
using System.Collections.Generic;

namespace GodAndMe.Models
{
    public class Base
    {
        public List<Intention> intention { get; set; }
        public List<Lent> lent { get; set; }
        public List<Diary> diary { get; set; }
        public List<Sins> sins { get; set; }
        public List<Prayers> prayers { get; set; }
    }
}
