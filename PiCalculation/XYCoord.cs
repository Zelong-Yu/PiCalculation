using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extreme.Mathematics;
using Extreme.Mathematics.Random;

namespace PiCalculation
{
    //1.	Create a struct to hold x, y coordinates as doubles. Provide a 2-parameter constructor
    // which takes a new set of coordinates and a 1-parameter constructor which takes a Random object
    //    and uses the NextDouble method to initialize X and Y
    struct XYCoord
    {
        public double x;
        public double y;

        public XYCoord(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public XYCoord(Random r)
        {
            this.x = r.NextDouble();
            this.y = r.NextDouble();
        }

        public XYCoord(IEnumerator<Vector<double>> Enumerator)
        {
            //takes a 2D vector Enumerator and populate it 
            Enumerator.MoveNext();
            this.x = Enumerator.Current[0];
            this.y = Enumerator.Current[1];
        }


        public XYCoord((double x, double y) t) : this(t.x, t.y) { }

        public void Deconstruct(out double x, out double y)
        {
            x = this.x;
            y = this.y;
        }
        
    }

    struct XYCoordDecimal
    {
        public decimal x;
        public decimal y;

        public XYCoordDecimal(decimal x, decimal y)
        {
            this.x = x;
            this.y = y;
        }

        public XYCoordDecimal(Random r)
        {
            this.x = (decimal)r.NextDouble();
            this.y = (decimal)r.NextDouble();
        }

    }

}
