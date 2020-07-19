using System;
using System.Collections.Generic;
using System.Text;

namespace PHHTypes
{
    public class Recipe
    {
        public string Name { get; set; }
        public decimal ProcessTime { get; set; }
        public Dictionary<Resource, decimal> Inputs { get; set; }
        public Dictionary<Resource, decimal> Outputs { get; set; }
    }
}
