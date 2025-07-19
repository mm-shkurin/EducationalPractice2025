using System;
using System.Linq;

namespace task14
{
    public class SingleThreadedIntegral
    {
        public static double Solve(double a, double b, Func<double, double> function, double step)
        {
            var points = Enumerable.Range(0, (int)((b - a) / step) + 1)
                .Select(i => a + i * step)
                .Where(x => x <= b)
                .ToList();
            
            if (points.Count < 2)
                return 0.0;
            
            return points.Zip(points.Skip(1), (x1, x2) =>
            {
                double y1 = function(x1);
                double y2 = function(x2);
                return (y1 + y2) * (x2 - x1) / 2.0;
            }).Sum();
        }
    }
} 