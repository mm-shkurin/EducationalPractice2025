using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ScottPlot;

namespace task14
{
    public class PerformanceAnalyzer
    {
        private const double A = -100;
        private const double B = 100;
        private const double TARGET_ACCURACY = 1e-4;
        private static readonly Func<double, double> SinFunction = x => Math.Sin(x);
        
        public static double[] TestSteps = { 1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6 };
        public static int[] TestThreads = { 1, 2, 4, 8, 16, 32 };
        private const int MEASUREMENTS_COUNT = 5;
        
        public static double FindOptimalStep()
        {
            var results = new List<(double step, double time, double accuracy)>();
            
            foreach (var step in TestSteps)
            {
                var stopwatch = Stopwatch.StartNew();
                var result = DefiniteIntegral.Solve(A, B, SinFunction, step, 8);
                stopwatch.Stop();
                
                var singleThreadResult = SingleThreadedIntegral.Solve(A, B, SinFunction, step);
                var accuracy = Math.Abs(result - singleThreadResult);
                
                results.Add((step, stopwatch.ElapsedMilliseconds, accuracy));
            }
            
            var optimalStep = results
                .Where(r => r.accuracy <= TARGET_ACCURACY)
                .OrderBy(r => r.time)
                .First().step;
                
            return optimalStep;
        }
        
        public static (int optimalThreads, double time) FindOptimalThreads(double step)
        {
            var results = new List<(int threads, double time)>();
            
            foreach (var threadCount in TestThreads)
            {
                var times = new List<double>();
                
                for (int i = 0; i < MEASUREMENTS_COUNT; i++)
                {
                    var stopwatch = Stopwatch.StartNew();
                    DefiniteIntegral.Solve(A, B, SinFunction, step, threadCount);
                    stopwatch.Stop();
                    times.Add(stopwatch.ElapsedMilliseconds);
                }
                
                var avgTime = times.Average();
                results.Add((threadCount, avgTime));
            }
            
            var optimal = results.OrderBy(r => r.time).First();
            return (optimal.threads, optimal.time);
        }
        
        public static void GeneratePerformanceGraph(double step, string filename = "performance_graph.png")
        {
            var plot = new Plot();
            
            var threadCounts = TestThreads.Select(t => (double)t).ToArray();
            var times = new List<double>();
            
            foreach (var threadCount in TestThreads)
            {
                var measurements = new List<double>();
                
                for (int i = 0; i < MEASUREMENTS_COUNT; i++)
                {
                    var stopwatch = Stopwatch.StartNew();
                    DefiniteIntegral.Solve(A, B, SinFunction, step, threadCount);
                    stopwatch.Stop();
                    measurements.Add(stopwatch.ElapsedMilliseconds);
                }
                
                times.Add(measurements.Average());
            }
            
            var scatter = plot.Add.Scatter(threadCounts, times.ToArray());
            scatter.LineWidth = 2;
            scatter.MarkerSize = 8;
            
            plot.Axes.Left.Label.Text = "Время выполнения (мс)";
            plot.Axes.Bottom.Label.Text = "Количество потоков";
            plot.Title($"Производительность многопоточного вычисления интеграла sin(x) на [{A}, {B}]");
            
            plot.SavePng(filename, 800, 600);
        }
        
        public static (double multiThreadTime, double singleThreadTime, double improvement) CompareWithSingleThreaded(double step, int optimalThreads)
        {
            var multiThreadTimes = new List<double>();
            var singleThreadTimes = new List<double>();
            
            for (int i = 0; i < MEASUREMENTS_COUNT; i++)
            {
                var stopwatch = Stopwatch.StartNew();
                DefiniteIntegral.Solve(A, B, SinFunction, step, optimalThreads);
                stopwatch.Stop();
                multiThreadTimes.Add(stopwatch.ElapsedMilliseconds);
                
                stopwatch.Restart();
                SingleThreadedIntegral.Solve(A, B, SinFunction, step);
                stopwatch.Stop();
                singleThreadTimes.Add(stopwatch.ElapsedMilliseconds);
            }
            
            var avgMultiThread = multiThreadTimes.Average();
            var avgSingleThread = singleThreadTimes.Average();
            var improvement = ((avgSingleThread - avgMultiThread) / avgSingleThread) * 100;
            
            return (avgMultiThread, avgSingleThread, improvement);
        }
        
        public static void SaveResultsToFile(double optimalStep, int optimalThreads, double multiThreadTime, double singleThreadTime, double improvement)
        {
            var content = $@"Результаты оптимизации многопоточного вычисления интеграла sin(x) на отрезке [{A}, {B}]

1. Оптимальный размер шага: {optimalStep}
   - Выбран из тестирования шагов: {string.Join(", ", TestSteps)}
   - Обеспечивает точность не хуже {TARGET_ACCURACY}

2. Оптимальное количество потоков: {optimalThreads}
   - Протестированы варианты: {string.Join(", ", TestThreads)}
   - Время выполнения: {multiThreadTime:F2} мс

3. Сравнение с однопоточной версией:
   - Время однопоточной версии: {singleThreadTime:F2} мс
   - Время многопоточной версии: {multiThreadTime:F2} мс
   - Ускорение: {improvement:F2}%

4. Заключение:
   - Многопоточная версия работает {(improvement > 15 ? "значительно быстрее" : "медленнее или с незначительным ускорением")}
   - {(improvement > 15 ? "Требования выполнены" : "Требуется дополнительная оптимизация")}";
            
            System.IO.File.WriteAllText("optimization_results.txt", content);
        }
    }
} 