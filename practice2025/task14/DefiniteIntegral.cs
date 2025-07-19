using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace task14
{
    public class DefiniteIntegral
    {
        public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
        {
            double segmentLength = (b - a) / threadsNumber;
            
            double totalResult = 0.0;
            
            using (var barrier = new Barrier(threadsNumber + 1))
            {
                var tasks = Enumerable.Range(0, threadsNumber)
                    .Select(i =>
                    {
                        double segmentStart = a + i * segmentLength;
                        double segmentEnd = (i == threadsNumber - 1) ? b : a + (i + 1) * segmentLength;
                        
                        return Task.Run(() =>
                        {
                            double segmentResult = CalculateSegmentIntegral(segmentStart, segmentEnd, function, step);
                            
                            Interlocked.Exchange(ref totalResult, totalResult + segmentResult);
                            
                            barrier.SignalAndWait();
                        });
                    })
                    .ToArray();
                
                barrier.SignalAndWait();
                
                Task.WaitAll(tasks);
            }
            
            return totalResult;
        }
        
        private static double CalculateSegmentIntegral(double start, double end, Func<double, double> function, double step)
        {
            double result = 0.0;
            
            var points = Enumerable.Range(0, (int)((end - start) / step) + 1)
                .Select(i => start + i * step)
                .Where(x => x <= end)
                .ToList();
            
            if (points.Count < 2)
                return 0.0;
            
            result = points.Zip(points.Skip(1), (x1, x2) =>
            {
                double y1 = function(x1);
                double y2 = function(x2);
                return (y1 + y2) * (x2 - x1) / 2.0;
            }).Sum();
            
            return result;
        }
    }
}
