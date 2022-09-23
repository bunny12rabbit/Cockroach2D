using System.Linq;
using UnityEngine;

namespace Common
{
    public static class RandomWeighed
    {
        public static int GetRandomWeightedIndex(int[] weights)
        {
            // Get the total sum of all the weights.
            var weightSum = weights.Sum();

            // Step through all the possibilities, one by one, checking to see if each one is selected.
            var index = 0;
            var lastIndex = weights.Length - 1;

            while (index < lastIndex)
            {
                // Do a probability check with a likelihood of weights[index] / weightSum.
                var random = Random.Range(0, weightSum);

                if (random < weights[index])
                    return index;

                // Remove the last item from the sum of total untested weights and try again.
                weightSum -= weights[index++];
            }

            // No other item was selected, so return very last index.
            return index;
        }

        public static int GetRandomWeightedIndex(float[] weights)
        {
            if (weights == null || weights.Length == 0)
                return -1;

            float weight;
            float weightsSum = 0;

            for (var i = 0; i < weights.Length; i++)
            {
                weight = weights[i];

                if (float.IsPositiveInfinity(weight))
                    return i;

                if (weight >= 0f && !float.IsNaN(weight))
                    weightsSum += weights[i];
            }

            var random = Random.value;
            var sum = 0f;

            for (var i = 0; i < weights.Length; i++)
            {
                weight = weights[i];

                if (float.IsNaN(weight) || weight <= 0f)
                    continue;

                sum += weight / weightsSum;

                if (sum >= random)
                    return i;
            }

            return -1;
        }
    }
}