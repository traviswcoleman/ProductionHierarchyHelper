using System;
using System.Collections.Generic;
using System.Text;

namespace PHHTypes
{
    public class Recipe
    {
        public string Name { get; set; }
        public decimal ProcessTime { get; set; }
        public List<Resource> Inputs { get; set; }
        public List<Resource> Outputs { get; set; }
    }
}
