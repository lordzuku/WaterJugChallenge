using WaterJugChallenge.Models;
using System.Collections.Generic;
using System;

namespace WaterJugChallenge.Services
{
    public interface IWaterJugService
    {
        WaterJugResponse SolveWaterJugProblem(WaterJugRequest request);
    }

    public class WaterJugService : IWaterJugService
    {
        private readonly HashSet<(int, int)> _visitedStatesX = new HashSet<(int, int)>();
        private readonly HashSet<(int, int)> _visitedStatesY = new HashSet<(int, int)>();

        public WaterJugResponse SolveWaterJugProblem(WaterJugRequest request)
        {
            var response = new WaterJugResponse();
            int x = request.X_capacity;
            int y = request.Y_capacity;
            int z = request.Z_amount_wanted;

            if (x <= 0 || y <= 0 || z <= 0)
            {
                throw new ArgumentException("all values must be positive integers");
            }

            if (z > Math.Max(x, y))
            {
                throw new ArgumentException("target amount cannot be greater than the larger jug");
            }

            // Using GCD to check if solution is possible - if z is not divisible by GCD(x,y), no solution exists
            if (z % GCD(x, y) != 0)
            {
                throw new ArgumentException("no solution exists for the given values");
            }

            var pathX = new List<Step>();
            var pathY = new List<Step>();
            int bucketX1 = 0, bucketY1 = 0;
            int bucketX2 = 0, bucketY2 = 0;
            int step = 1;
            const int MAX_STEPS = 1000;

            while (step <= MAX_STEPS)
            {
                if (bucketX1 != z && bucketY1 != z)
                {
                    if (!_visitedStatesX.Add((bucketX1, bucketY1)))
                    {
                        throw new ArgumentException("No solution exists - cycle detected in path starting with X");
                    }

                    if (bucketX1 == 0)
                    {
                        bucketX1 = x;
                        pathX.Add(new Step
                        {
                            StepNumber = step,
                            BucketX = bucketX1,
                            BucketY = bucketY1,
                            Action = "Fill bucket X"
                        });
                    }
                    else if (bucketY1 == y)
                    {
                        bucketY1 = 0;
                        pathX.Add(new Step
                        {
                            StepNumber = step,
                            BucketX = bucketX1,
                            BucketY = bucketY1,
                            Action = "Empty bucket Y"
                        });
                    }
                    else
                    {
                        int transfer = Math.Min(bucketX1, y - bucketY1);
                        bucketX1 -= transfer;
                        bucketY1 += transfer;
                        pathX.Add(new Step
                        {
                            StepNumber = step,
                            BucketX = bucketX1,
                            BucketY = bucketY1,
                            Action = "Transfer from bucket X to Y"
                        });
                    }

                    if (bucketX1 == z || bucketY1 == z)
                    {
                        pathX.Last().Status = "Solved";
                        response.Solution = pathX;
                        return response;
                    }
                }

                if (bucketX2 != z && bucketY2 != z)
                {
                    if (!_visitedStatesY.Add((bucketX2, bucketY2)))
                    {
                        throw new ArgumentException("No solution exists - cycle detected in path starting with Y");
                    }

                    if (bucketY2 == 0)
                    {
                        bucketY2 = y;
                        pathY.Add(new Step
                        {
                            StepNumber = step,
                            BucketX = bucketX2,
                            BucketY = bucketY2,
                            Action = "Fill bucket Y"
                        });
                    }
                    else if (bucketX2 == x)
                    {
                        bucketX2 = 0;
                        pathY.Add(new Step
                        {
                            StepNumber = step,
                            BucketX = bucketX2,
                            BucketY = bucketY2,
                            Action = "Empty bucket X"
                        });
                    }
                    else
                    {
                        int transfer = Math.Min(bucketY2, x - bucketX2);
                        bucketY2 -= transfer;
                        bucketX2 += transfer;
                        pathY.Add(new Step
                        {
                            StepNumber = step,
                            BucketX = bucketX2,
                            BucketY = bucketY2,
                            Action = "Transfer from bucket Y to X"
                        });
                    }

                    if (bucketX2 == z || bucketY2 == z)
                    {
                        pathY.Last().Status = "Solved";
                        response.Solution = pathY;
                        return response;
                    }
                }

                step++;
            }

            throw new ArgumentException($"Solution requires more than {MAX_STEPS} steps, which exceeds the maximum allowed");
        }

        private int GCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
    }
} 