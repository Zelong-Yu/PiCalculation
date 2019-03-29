using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiCalculation
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAppLoop();
        }

            //Create a function which takes an x, y coordinate struct and returns a double 
            // corresponding to the hypotenuse of a triangle with sides of lengths x, y.
            //Func<XYCoord, double> hypotenuse = n => Math.Sqrt(Math.Pow(n.x, 2) + Math.Pow(n.y, 2));
        public static double hypotenuse(XYCoord n) => Math.Sqrt(Math.Pow(n.x, 2) + Math.Pow(n.y, 2));

        private static void MainAppLoop()
        {
            bool End = false;
            List<string> calculationMethods = new List<string>();
            calculationMethods.Add("Naïve approach (O(N) time, O(N) space: Array N created and iterated)");
            calculationMethods.Add("Space improved (O(N) time, O(1) space)");
            do
            {
                Console.Clear();
                int selected = UI.SelectionMenu(calculationMethods);
                End = HandleSelection(selected);

            } while (!End);
        }

        private static void NaiveApproach()
        {
            Console.Clear();
            //takes one int parameter from the command line and creates an array of that 
            //length containing randomly initialized coordinates.
            int numPoints = UI.AcceptValidInt("How many points to scatter?\n>", 1);

            Random rnd = new Random();
            XYCoord[] points = new XYCoord[numPoints];
            for (int i = 0; i < numPoints; ++i)
            {
                points[i] = new XYCoord(rnd);
            }
            //Iterate over the array, incrementing a counter for each coordinate which overlaps the unit circle.
            double estimatePi = points.Count(p => hypotenuse(p) < 1.0) / (double)numPoints * 4;
            Console.WriteLine($"Estimated Pi Value: {estimatePi}\n" +
                              $"Actual Pi Value:    {Math.PI}\n" +
                              $"Difference:         {Math.Abs(estimatePi - Math.PI)}");
        }

        private static bool HandleSelection(int selected)
        {
            switch (selected)
            {
                case -1:
                    return true;
                case 0:
                    NaiveApproach();
                    return false;
                    break;
                default:
                    return false;
                    break;
            }

        }
    }
}
