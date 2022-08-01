using System;
using static Linez.Coords;

namespace Linez
{
    public class Box
    {
        
        public Coords position { get; set; }
        public Coords parent { get; set; }
        public int f { get; set; }
        public int h { get; set; }
        public int g { get; set; }
        
    }
}
