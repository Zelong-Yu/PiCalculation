using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

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

        //Since we are using a unit circle where r^2=1, we dont actually need to take the square root
        //when comparing hypotenuse and 1. As long as hypotenuseSquare < 1 the point is inside unit
        //circle.
        public static double hypotenuseSquare(XYCoord n) => n.x*n.x + n.y*n.y;
        private static void MainAppLoop()
        {
            bool End = false;
            List<string> calculationMethods = new List<string>();
            calculationMethods.Add("Naïve approach (O(N) time, O(N) space: Array N created and iterated)");
            calculationMethods.Add("Space improved (O(N) time, O(1) space)");
            calculationMethods.Add("Recursion approach (O(N) time, O(N) space, stackoverflow)");
            calculationMethods.Add("IEnumerable approach (O(N) time, O(1) space)");
            do
            {
                Console.Clear();
                int selected = UI.SelectionMenu(calculationMethods);
                End = HandleSelection(selected);

            } while (!End);
        }
        private static bool HandleSelection(int selected)
        {
            switch (selected)
            {
                case -1:
                    return true;
                case 0:
                    Console.Clear();
                    NaiveApproach();
                    return false;
                    break;
                case 1:
                    Console.Clear();
                    SpaceSavingApproach();
                    return false;
                    break;
                case 2:
                    Console.Clear();
                    RecursionApproach();
                    return false;
                    break;
                case 3:
                    Console.Clear();
                    IEnumerableApproach();
                    return false;
                    break;
                default:
                    return false;
                    break;
            }

        }

        private static void IEnumerableApproach()
        {
            do
            {
                //takes one int parameter from the command line and creates an array of that 
                //length containing randomly initialized coordinates.
                Console.Write("Enter how many points per step to print:  \n>");
                //long numPoints = long.Parse(Console.ReadLine());
                int step = int.Parse(Console.ReadLine());
                // Create new stopwatch.
                Stopwatch stopwatch = new Stopwatch();
                try
                {
                    Random rnd = new Random();
                    stopwatch.Start();

                    IEnumerable<long> GenerateInsideCount(long n)
                    {
                        long inside = 0;
                        for (long i=0; i<n; ++i)
                        {
                            if (hypotenuseSquare(new XYCoord(rnd)) < 1) inside++;
                            yield return inside;
                        }
                    }

                    IEnumerable<(long,long)> GenerateInsideCountAndTotal()
                    {
                        long inside = 0;
                        long total = 0;
                        while (true)
                        {
                            if (hypotenuse(new XYCoord(rnd)) < 1) inside++;
                            total++;
                            yield return (inside,total);
                        }
                    }

                    int tempcount = 0;
                    var enumerator = GenerateInsideCountAndTotal().GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        tempcount++;
                        if (tempcount==step)
                        {
                           decimal estimatePi = (decimal)enumerator.Current.Item1 / enumerator.Current.Item2 *4;
                            //Console.WriteLine(enumerator.Current);
                           Console.WriteLine($"Estimated Pi Value: {estimatePi}\n" +
                                             $"Actual Pi Value:    {Math.PI}\n" +
                                             $"Difference:         {Math.Abs(estimatePi - (decimal)Math.PI)}\n" +
                                             $"Elasped time:       {stopwatch.Elapsed.TotalMinutes} Minutes\n" +
                                             $"Points Generated:   {enumerator.Current.Item2}\n" +
                                             $"================================================================");
                           tempcount = 0;
                        }
                    }
                    //double ratio = GenerateInsideCount(numPoints).Last() / (double)numPoints;
                   
                    /*double estimatePi = ratio * 4;

                    //stop the timer
                    stopwatch.Stop();

                    Console.WriteLine($"Estimated Pi Value: {estimatePi}\n" +
                                      $"Actual Pi Value:    {Math.PI}\n" +
                                      $"Difference:         {Math.Abs(estimatePi - Math.PI)}\n" +
                                      $"Elasped time:       {stopwatch.Elapsed.TotalMilliseconds} ms");*/
                }
                catch (OutOfMemoryException e)
                {
                    Console.WriteLine(e.GetType().ToString());
                }
            } while (true);
        }

        private static void RecursionApproach()
        {
            do
            {
                //takes one int parameter from the command line and creates an array of that 
                //length containing randomly initialized coordinates.
                long numPoints = 5000;
                // Create new stopwatch.
                Stopwatch stopwatch = new Stopwatch();
                try
                {
                    Random rnd = new Random();
                    stopwatch.Start();

                    double getratio(long inside, long total, long index)
                    {
                        if (index == 0) return (double)inside / total;
                        else return getratio(inside += (hypotenuse(new XYCoord(rnd)) < 1 ? 1 : 0), total, --index);
                    }

                    //Iterate over the array, incrementing a counter for each coordinate which overlaps the unit circle.
                    double estimatePi = getratio(0,numPoints,numPoints) * 4;

                    //stop the timer
                    stopwatch.Stop();

                    Console.WriteLine($"Estimated Pi Value: {estimatePi}\n" +
                                      $"Actual Pi Value:    {Math.PI}\n" +
                                      $"Difference:         {Math.Abs(estimatePi - Math.PI)}\n" +
                                      $"Elasped time:       {stopwatch.Elapsed.TotalMilliseconds} ");
                    Console.ReadKey();
                }
                catch (OutOfMemoryException e)
                {
                    Console.WriteLine(e.GetType().ToString());
                }
            } while (false);
        }

        private static void NaiveApproach()
        {
            do
            {
                
                //takes one int parameter from the command line and creates an array of that 
                //length containing randomly initialized coordinates.
                int numPoints = UI.AcceptValidInt("How many points to scatter? (0 to quit) \n>", 0);
                if (numPoints == 0) break;
                // Create new stopwatch.
                Stopwatch stopwatch = new Stopwatch();
                try
                {
                    Random rnd = new Random();
                    stopwatch.Start();
                    XYCoord[] points = new XYCoord[numPoints];
                    for (int i = 0; i < numPoints; ++i)
                    {
                        points[i] = new XYCoord(rnd);
                    }
                    //Iterate over the array, incrementing a counter for each coordinate which overlaps the unit circle.
                    double estimatePi = points.Count(p => hypotenuse(p) < 1.0) / (double)numPoints * 4;

                    //stop the timer
                    stopwatch.Stop();

                    Console.WriteLine($"Estimated Pi Value: {estimatePi}\n" +
                                      $"Actual Pi Value:    {Math.PI}\n" +
                                      $"Difference:         {Math.Abs(estimatePi - Math.PI)}\n" +
                                      $"Elasped time:       {stopwatch.Elapsed.TotalMilliseconds} ");
                }
                catch (OutOfMemoryException e)
                {
                    Console.WriteLine(e.GetType().ToString());
                }
            } while (true);
        }

        private static void SpaceSavingApproach()
        {
            do
            {

                //takes one int parameter from the command line and creates an array of that 
                //length containing randomly initialized coordinates.
                int numPoints = UI.AcceptValidInt("How many points to scatter? (0 to quit) \n>", 0);

                if (numPoints == 0) break;
                // Create new stopwatch.
                Stopwatch stopwatch = new Stopwatch();
                try
                {
                    Random rnd = new Random();
                    stopwatch.Start();

                    //BigInteger bigIntFromInt64 = new BigInteger(934157136952);

                    double CountOverLap(int k) => Enumerable.Range(1, k).Aggregate(
                                    seed: 0,
                                    func: (count, index) => count + (hypotenuse(new XYCoord(rnd)) < 1 ? 1 : 0),
                                    resultSelector: count => (double)count );

                      double PointsInCircle = CountOverLap(numPoints);

                     double estimatedPi = PointsInCircle / numPoints * 4.0;
                    //stop the timer


                    //Console.WriteLine($"Estimated Pi Value: {estimatedPi}\n" +
                    //                  $"Actual Pi Value:    {Math.PI}\n" +
                    //                  $"Difference:         {Math.Abs(estimatedPi - Math.PI)}\n" +
                    //                  $"Elasped time:       {stopwatch.Elapsed.TotalMilliseconds} ");

                    Console.WriteLine($"Estimated Pi Value: {estimatedPi}\n" +
                                      $"Actual Pi Value:    {Math.PI}");
                    stopwatch.Stop();
                    Console.WriteLine($"Difference:         {Math.Abs(estimatedPi - Math.PI)}");
                    Console.WriteLine($"Elasped time:       {stopwatch.Elapsed.TotalMilliseconds} ");
                }
                catch (OutOfMemoryException e)
                {
                    Console.WriteLine(e.GetType().ToString());
                }
            } while (true);
        }

    }
}
