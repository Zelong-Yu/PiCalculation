using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Timers;

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
        //circle. This will help pretaining precision to 1E-6
        public static decimal hypotenuseSquare(XYCoordDecimal n) => n.x*n.x + n.y*n.y;
        public static double hypotenuseSquare(XYCoord n) => n.x * n.x + n.y * n.y;
        //Timer for use in Parallel approach
        private static System.Timers.Timer aTimer;
        private static void MainAppLoop()
        {
            bool End = false;
            List<string> calculationMethods = new List<string>();
            calculationMethods.Add("1. Original Approach with Array (O(N) time, O(N) space: Array N created and iterated)");
            calculationMethods.Add("2. Constant Space Approach with Enumerable.Range, Aggregate (O(N) time, O(1) space)");
            calculationMethods.Add("3. Recursion approach (O(N) time, O(N) space, easy stackoverflow)");
            calculationMethods.Add("4. Run-forever approach with IEnumerable yield return (O(N) time, O(1) space)");
            calculationMethods.Add("4a.Run-forever approach with user set start point from previous result");
            calculationMethods.Add("5. Parallel Approach (O(N) time, O(1) space)");
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
                case 4:
                    Console.Clear();
                    IEnumerableApproachReload();
                    return false;
                    break;
                case 5:
                    Console.Clear();
                    ParallelApproach();
                    return false;
                    break;
                default:
                    return false;
                    break;
            }

        }

        private static void ParallelApproach()
        {
            do
            {
                int numGenetarors = UI.AcceptValidInt("Enter how many point generators to run simultaneously: (0 to quit) \n>", 0);
                if (numGenetarors == 0) break;

                int eachstep = UI.AcceptValidInt("Enter how many points each generator is going to generate: (0 to quit) \n>", 0);
                if (eachstep == 0) break;


                List<int> stepsListToBeRunTogether = new List<int>();
                for (int i = 0; i < numGenetarors; i++)
                {
                    stepsListToBeRunTogether.Add(eachstep);
                }

                long totalInside = 0;
                long totalTotal = 0;
                int generatorsDone = 0;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                //parallel running: each generator should return inside and total after set num step
                Parallel.ForEach<int>(stepsListToBeRunTogether, (step) =>
                    {
                        (long, long) r = GenerateCountsWithSteps(step, new Random());
                        totalInside += r.Item1;
                        totalTotal += r.Item2;
                        decimal estimatePi = (decimal)totalInside / totalTotal * 4;
                        generatorsDone++;
                        Console.WriteLine($"Estimated Pi Value:  {estimatePi:f14}\n" +
                                                 $"Actual Pi Value:     {Math.PI}\n" +
                                                 $"Difference:          {Math.Abs(estimatePi - (decimal)Math.PI):f14}\n" +
                                                 $"Elasped time:        {stopwatch.Elapsed.TotalSeconds} Seconds\n" +
                                                 $"Inside Pts Generated:{totalInside}\n" +
                                                 $"Total  Pts Generated:{totalTotal}\n" +
                                                 $"Num Generators Done :{generatorsDone}\n" +
                                                 $"================================================================");
                    });
            } while (true);
        }


        //Generate a tuple (numPts inside unit circle, step) given step and random genereator
        private static (long,long) GenerateCountsWithSteps(int step, Random rnd)
        {
            IEnumerable<(long, long)> GenerateInsideCountAndTotal(long inside = 0, long total = 0)
            {
                while (true)
                {
                    if (hypotenuseSquare(new XYCoord(rnd)) < 1) inside++;
                    total++;
                    yield return (inside, total);
                }
            }

            int tempcount = 0;
            var enumerator = GenerateInsideCountAndTotal().GetEnumerator();

            while (tempcount < step)
            {
                enumerator.MoveNext();
                tempcount++;
            }
            return enumerator.Current;

        }

        private static void IEnumerableApproachReload()
        {
            do
            {
                long preinside = UI.AcceptValidLong("Enter Number of Points Inside previously generated: (-1 to quit, 0 to start fresh) \n>", -1);
                if (preinside == -1) break;
                long pretotal = UI.AcceptValidLong( "Enter Number of Points Total  previously generated: (-1 to quit, 0 to start fresh) \n>", -1);
                if (pretotal == -1) break;
                int step = UI.AcceptValidInt("Enter how many points per step to print (Put bigger steps >10000000 to avoid screen flash): (0 to quit) \n>", 0);
                if (step == 0) break;

                // Create new stopwatch.
                Stopwatch stopwatch = new Stopwatch();
                try
                {
                    Random rnd = new Random();
                    stopwatch.Start();

                    IEnumerable<(long, long)> GenerateInsideCountAndTotal(long inside = 0, long total = 0)
                    {
                        while (true)
                        {
                            if (hypotenuseSquare(new XYCoord(rnd)) < 1) inside++;
                            total++;
                            yield return (inside, total);
                        }
                    }

                    int tempcount = 0;
                    var enumerator = GenerateInsideCountAndTotal(preinside, pretotal).GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        tempcount++;
                        if (tempcount == step)
                        {
                            decimal estimatePi = (decimal)enumerator.Current.Item1 / enumerator.Current.Item2 * 4;
                            //Print out result of current batched step
                            Console.WriteLine($"Estimated Pi Value:  {estimatePi:f14}\n" +
                                              $"Actual Pi Value:     {Math.PI}\n" +
                                              $"Difference:          {Math.Abs(estimatePi - (decimal)Math.PI):f14}\n" +
                                              $"Elasped time:        {stopwatch.Elapsed.TotalMinutes:f2} Minutes\n" +
                                              $"Inside Pts Generated:{enumerator.Current.Item1}\n" +
                                              $"Total  Pts Generated:{enumerator.Current.Item2}\n" +
                                              $"================================================================");
                            //reset so the loop starts next batched step
                            tempcount = 0;
                        }
                    }
                }
                catch (OutOfMemoryException e)
                {
                    Console.WriteLine(e.GetType().ToString());
                }
            } while (true);
        }

        private static void IEnumerableApproach()
        {
            do
            {
                int step = UI.AcceptValidInt("Enter how many points per step to print (Put bigger steps >10000000 to avoid screen flash): (0 to quit) \n>", 0);
                if (step == 0) break;

                // Create new stopwatch.
                Stopwatch stopwatch = new Stopwatch();
                try
                {
                    Random rnd = new Random();
                    stopwatch.Start();
                  
                    IEnumerable<(long,long)> GenerateInsideCountAndTotal(long inside = 0, long total = 0)
                    {
                        while (true)
                        {
                            if (hypotenuseSquare(new XYCoord(rnd)) < 1) inside++;
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
                           Console.WriteLine($"Estimated Pi Value:  {estimatePi:f14}\n" +
                                             $"Actual Pi Value:     {Math.PI}\n" +
                                             $"Difference:          {Math.Abs(estimatePi - (decimal)Math.PI):f14}\n" +
                                             $"Elasped time:        {stopwatch.Elapsed.TotalMinutes:f2} Minutes\n" +
                                             $"Inside Pts Generated:{enumerator.Current.Item1}\n" +
                                             $"Total  Pts Generated:{enumerator.Current.Item2}\n" +
                                             $"================================================================");
                           tempcount = 0;
                        }
                    }
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
                int numPoints = UI.AcceptValidInt("Enter how many points to generate: (0 to quit) \n>", 0);
                if (numPoints == 0) break;
                //takes one int parameter from the command line and creates an array of that 
                //length containing randomly initialized coordinates.

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
                                      $"Elasped time:       {stopwatch.Elapsed.TotalMilliseconds} ms \n");
                    
                }
                catch (StackOverflowException e)
                {
                    Console.WriteLine(e.GetType().ToString());
                }
            } while (true);
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
                                      $"Elasped time:       {stopwatch.Elapsed.TotalMilliseconds} ms");
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


                    double CountOverLap(int k) => Enumerable.Range(1, k).Aggregate(
                                    seed: 0,
                                    func: (count, index) => count + (hypotenuse(new XYCoord(rnd)) < 1 ? 1 : 0),
                                    resultSelector: count => (double)count );

                      double PointsInCircle = CountOverLap(numPoints);

                     double estimatedPi = PointsInCircle / numPoints * 4.0;

                    //stop the timer
                    Console.WriteLine($"Estimated Pi Value: {estimatedPi}\n" +
                                      $"Actual Pi Value:    {Math.PI}");
                    stopwatch.Stop();
                    Console.WriteLine($"Difference:         {Math.Abs(estimatedPi - Math.PI)}");
                    Console.WriteLine($"Elasped time:       {stopwatch.Elapsed.TotalMilliseconds} ms\n");
                }
                catch (OutOfMemoryException e)
                {
                    Console.WriteLine(e.GetType().ToString());
                }
            } while (true);
        }

    }
}
