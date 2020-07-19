using System;
using System.Collections.Generic;

namespace PHHTypes
{
    public class Node
    {
        public string Name { get; set; }
        public IEnumerable<Recipe> Recipes { get; set; }
    }
}
