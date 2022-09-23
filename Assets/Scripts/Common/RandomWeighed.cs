using System.Collections.Generic;
using Core;
using Core.Logs;
using Extensions;
using UnityEngine;

namespace Common
{
    public class RandomWeighed
    {
        private int CalculateCDFWeightedIndexIn(ICollection<int> weights)
        {
            var cdf = weights.ComputeCumulativeDensityFunction(out var sumOfWeights);
            var random = Random.Range(0, sumOfWeights + 1);
            Log.Info($"SumOfWeights: {sumOfWeights}, random: {random}");

            var intResultIndex = cdf.FindClosestIndexWithBinarySearch(random);
            Log.Info($"IntResultIndex: {intResultIndex}".Colorize(Color.green));

            return intResultIndex;
        }

        private int CalculateCDFWeightedIndexIn(ICollection<float> weights)
        {
            var cdfFloat = weights.ComputeCumulativeDensityFunction(out var sumOfWeightsFloat);
            var randomFloat = Random.Range(0, sumOfWeightsFloat);
            Log.Info($"SumOfWeightsFloat: {sumOfWeightsFloat}, randomFloat: {randomFloat}");

            var floatResultIndex = cdfFloat.FindClosestIndexWithBinarySearch(randomFloat);
            Log.Info($"FloatResultIndex: {floatResultIndex}".Colorize(Color.green));

            return floatResultIndex;
        }
    }
}