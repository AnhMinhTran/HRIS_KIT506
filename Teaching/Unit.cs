﻿using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS_KIT506.Teaching
{
    class Unit
    {
        public int Coordinator { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public List<Class> ClassList { get; set; }
        public override string ToString()
        {
            return Code + " " + Title;
        }
    }
}
