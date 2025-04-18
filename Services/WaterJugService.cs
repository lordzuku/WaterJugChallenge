using WaterJugChallenge.Models;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;

namespace WaterJugChallenge.Services
{
    public interface IWaterJugService
    {
        WaterJugResponse SolveWaterJugProblem(WaterJugRequest request);
    }

    public class WaterJugService : IWaterJugService
    {
        private readonly ILogger<WaterJugService> _logger;

        public WaterJugService(ILogger<WaterJugService> logger)
        {
            _logger = logger;
        }

        public WaterJugResponse SolveWaterJugProblem(WaterJugRequest request)
        {
            _logger.LogInformation("Solving water jug problem for X={X}, Y={Y}, Z={Z}", 
                request.X_capacity, request.Y_capacity, request.Z_amount_wanted);

            var response = new WaterJugResponse();
            int x = request.X_capacity;
            int y = request.Y_capacity;
            int z = request.Z_amount_wanted;

            if (x <= 0 || y <= 0 || z <= 0)
            {
                _logger.LogWarning("Invalid input: non-positive values detected");
                throw new ArgumentException("all values must be positive integers");
            }

            if (z > Math.Max(x, y))
            {
                _logger.LogWarning("Invalid input: target amount {Z} exceeds larger jug capacity", z);
                throw new ArgumentException("target amount cannot be greater than the larger jug");
            }

            if (z % GCD(x, y) != 0)
            {
                _logger.LogWarning("No solution exists: Z={Z} is not divisible by GCD({X},{Y})", z, x, y);
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
                        _logger.LogInformation("Solution found in {Steps} steps starting with X", step);
                        return response;
                    }
                }

                if (bucketX2 != z && bucketY2 != z)
                {
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
                        _logger.LogInformation("Solution found in {Steps} steps starting with Y", step);
                        return response;
                    }
                }

                step++;
            }

            _logger.LogWarning("Maximum steps ({MaxSteps}) exceeded", MAX_STEPS);
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