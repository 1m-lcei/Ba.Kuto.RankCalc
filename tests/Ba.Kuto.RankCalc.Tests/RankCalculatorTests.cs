namespace Ba.Kuto.RankCalc.Tests;

public class RankCalculatorTests
{
    [Fact]
    public void CalculateOptimalRoute_From100_ShouldReturnCorrectRoute()
    {
        var expectedRoute = new List<int> { 100, 70, 49, 34, 23, 16, 11, 7, 4, 1 };
        var actualRoute = RankCalculator.CalculateOptimalRoute(100);
        Assert.Equal(expectedRoute, actualRoute);
    }

    [Fact]
    public void CalculateOptimalRoute_From1_ShouldReturnSingleRank()
    {
        var expectedRoute = new List<int> { 1 };
        var actualRoute = RankCalculator.CalculateOptimalRoute(1);
        Assert.Equal(expectedRoute, actualRoute);
    }

    [Fact]
    public void CalculateOptimalRoute_From13_ShouldReturnCorrectRoute()
    {
        var expectedRoute = new List<int> { 13, 9, 6, 3, 1 };
        var actualRoute = RankCalculator.CalculateOptimalRoute(13);
        Assert.Equal(expectedRoute, actualRoute);
    }

    [Theory]
    [InlineData(100, 9)]
    [InlineData(13, 4)]
    [InlineData(1, 0)]
    public void GetOptimalBattleCount_ShouldReturnCorrectCount(int startRank, int expectedCount)
    {
        Assert.Equal(expectedCount, RankCalculator.GetOptimalBattleCount(startRank));
    }

    [Fact]
    public void CalculateCompromiseRoute_From100_ShouldReturnCorrectRoute()
    {
        var expectedRoute = new List<int> { 100, 78, 54, 37, 25, 17, 11, 7, 4, 1 };
        var actualRoute = RankCalculator.CalculateCompromiseRoute(100);
        Assert.Equal(expectedRoute, actualRoute);
    }

    [Fact]
    public void CalculateCompromiseRoute_From13_ShouldReturnCorrectRoute()
    {
        // Optimal: 13 -> 9 -> 6 -> 3 -> 1 (4 battles)
        // Available from 13: 9, 10, 11
        // GetOptimalBattleCount(9) = 3
        // GetOptimalBattleCount(10) = 4 (10->7->4->1)
        // GetOptimalBattleCount(11) = 4 (11->7->4->1)
        // So, for 13, next rank can be 9, 10, or 11 to reach 1st place in 4 battles.
        // Compromise should pick the highest rank (lowest value) among 9, 10, 11 that leads to 4 battles.
        // In this case, 11 is the highest rank (lowest value) that still allows 4 battles.
        var expectedRoute = new List<int> { 13, 11, 7, 4, 1 };
        var actualRoute = RankCalculator.CalculateCompromiseRoute(13);
        Assert.Equal(expectedRoute, actualRoute);
    }

    [Fact]
    public void CalculateMaxBattleRoute_From100_ShouldReturnCorrectRoute()
    {
        var expectedRoute = new List<int> { 100, 95, 90, 85, 80, 76, 72, 68, 64, 60, 57, 54, 51, 48, 45, 42, 39, 37, 35, 33, 31, 29, 27, 25, 23, 21, 19, 18, 17, 16, 15, 14, 13, 11, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
        var actualRoute = RankCalculator.CalculateMaxBattleRoute(100);
        Assert.Equal(expectedRoute, actualRoute);
    }

    [Fact]
    public void CalculateMaxBattleRoute_From13_ShouldReturnCorrectRoute()
    {
        var expectedRoute = new List<int> { 13, 11, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
        var actualRoute = RankCalculator.CalculateMaxBattleRoute(13);
        Assert.Equal(expectedRoute, actualRoute);
    }

    [Fact]
    public void CalculateCompromiseRoute_WithPrecalculatedOptimalRoute_ShouldReturnCorrectRoute()
    {
        var precalculatedOptimalRoute = new List<int> { 100, 70, 49, 34, 23, 16, 11, 7, 4, 1 };
        var expectedRoute = new List<int> { 100, 78, 54, 37, 25, 17, 11, 7, 4, 1 };
        var actualRoute = RankCalculator.CalculateCompromiseRoute(100, precalculatedOptimalRoute);
        Assert.Equal(expectedRoute, actualRoute);
    }

    [Fact]
    public void CalculateCompromiseRoute_WithEmptyPrecalculatedOptimalRoute_ShouldThrowArgumentException()
    {
        var emptyRoute = new List<int>();
        Assert.Throws<ArgumentException>(() => RankCalculator.CalculateCompromiseRoute(100, emptyRoute));
    }

    [Fact]
    public void CalculateCompromiseRoute_WithMismatchedPrecalculatedOptimalRoute_ShouldThrowArgumentException()
    {
        var mismatchedRoute = new List<int> { 50, 35, 24, 16, 11, 7, 4, 1 };
        Assert.Throws<ArgumentException>(() => RankCalculator.CalculateCompromiseRoute(100, mismatchedRoute));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CalculateOptimalRoute_InvalidRank_ShouldThrowArgumentOutOfRangeException(int invalidRank)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => RankCalculator.CalculateOptimalRoute(invalidRank));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CalculateCompromiseRoute_InvalidRank_ShouldThrowArgumentOutOfRangeException(int invalidRank)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => RankCalculator.CalculateCompromiseRoute(invalidRank));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CalculateMaxBattleRoute_InvalidRank_ShouldThrowArgumentOutOfRangeException(int invalidRank)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => RankCalculator.CalculateMaxBattleRoute(invalidRank));
    }

    [Theory]
    [InlineData(100, new int[] { 100, 70, 49, 34, 23, 16, 11, 7, 4, 1 })]
    [InlineData(13, new int[] { 13, 9, 6, 3, 1 })]
    [InlineData(1, new int[] { 1 })]
    public void OptimalRouteEnumerable_ShouldGenerateCorrectSequence(int startRank, int[] expectedSequence)
    {
        var enumerable = new RankCalculator.OptimalRouteEnumerable(startRank);
        var actualSequence = new List<int> { startRank };
        while (enumerable.MoveNext())
        {
            actualSequence.Add(enumerable.Current);
        }
        Assert.Equal(expectedSequence, actualSequence);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void OptimalRouteEnumerable_InvalidRank_ShouldThrowArgumentOutOfRangeException(int invalidRank)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new RankCalculator.OptimalRouteEnumerable(invalidRank));
    }

    [Theory]
    [InlineData(100, new int[] { 100, 95, 90, 85, 80, 76, 72, 68, 64, 60, 57, 54, 51, 48, 45, 42, 39, 37, 35, 33, 31, 29, 27, 25, 23, 21, 19, 18, 17, 16, 15, 14, 13, 11, 9, 8, 7, 6, 5, 4, 3, 2, 1 })]
    [InlineData(13, new int[] { 13, 11, 9, 8, 7, 6, 5, 4, 3, 2, 1 })]
    [InlineData(1, new int[] { 1 })]
    public void MaxBattleRouteEnumerable_ShouldGenerateCorrectSequence(int startRank, int[] expectedSequence)
    {
        var enumerable = new RankCalculator.MaxBattleRouteEnumerable(startRank);
        var actualSequence = new List<int>() { startRank };
        while (enumerable.MoveNext())
        {
            actualSequence.Add(enumerable.Current);
        }
        Assert.Equal(expectedSequence, actualSequence);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void MaxBattleRouteEnumerable_InvalidRank_ShouldThrowArgumentOutOfRangeException(int invalidRank)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new RankCalculator.MaxBattleRouteEnumerable(invalidRank));
    }

    [Theory]
    [InlineData(100, new int[] { 95, 94, 93, 92, 91, 90, 89, 88, 87, 86, 85, 84, 83, 82, 81, 80, 79, 78, 77, 76, 75, 74, 73, 72, 71, 70 })]
    [InlineData(13, new int[] { 11, 10, 9 })]
    [InlineData(10, new int[] { 9, 8, 7 })]
    [InlineData(4, new int[] { 3, 2, 1 })]
    [InlineData(1, new int[] { })]
    public void AvailableRanksEnumerable_ShouldGenerateCorrectSequence(int startRank, int[] expectedSequence)
    {
        var enumerable = new RankCalculator.AvailableRanksEnumerable(startRank);
        var actualSequence = new List<int>();
        while (enumerable.MoveNext())
        {
            actualSequence.Add(enumerable.Current);
        }
        Assert.Equal(expectedSequence, actualSequence);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AvailableRanksEnumerable_InvalidRank_ShouldThrowArgumentOutOfRangeException(int invalidRank)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new RankCalculator.AvailableRanksEnumerable(invalidRank));
    }
}