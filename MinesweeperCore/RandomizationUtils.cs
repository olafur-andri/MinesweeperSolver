namespace MinesweeperCore;

/// <summary>Utility functions that help with constructing randomized values</summary>
public static class RandomizationUtils
{
    /// <summary>
    /// Returns a copy of the given <paramref name="values"/> shuffled according to the Fisher-Yates
    /// algorithm. This method does not change the given <paramref name="values"/> at all
    /// </summary>
    public static IEnumerable<T> FisherYatesShuffle<T>(IEnumerable<T> values, int seed)
    {
        var shuffledList = new List<T>(values);
        var rng = new Random(seed);

        var rightIndex = shuffledList.Count;

        while (rightIndex > 1)
        {
            rightIndex -= 1;

            var leftIndex = rng.Next(rightIndex + 1);
            
            (shuffledList[leftIndex], shuffledList[rightIndex]) =
                (shuffledList[rightIndex], shuffledList[leftIndex]);
        }

        return shuffledList;
    }
}