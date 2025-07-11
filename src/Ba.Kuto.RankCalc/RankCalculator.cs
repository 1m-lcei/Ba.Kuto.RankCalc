using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

namespace Ba.Kuto.RankCalc;

public static class RankCalculator
{
    /// <summary>
    /// 最効率ルートを計算します。
    /// </summary>
    public static List<int> CalculateOptimalRoute(int startRank)
    {
        var route = new List<int>() { startRank };
        foreach (var next in GetOptimalRouteEnumerable(startRank))
        {
            route.Add(next);
        }

        return route;
    }

    /// <summary>
    /// ある順位から1位になるまでの最短戦闘回数を計算します。
    /// </summary>
    public static int GetOptimalBattleCount(int startRank)
    {
        var count = 0;
        foreach (var _ in GetOptimalRouteEnumerable(startRank))
        {
            ++count;
        }

        return count;
    }

    /// <summary>
    /// 妥協ルートを計算します。
    /// </summary>
    public static List<int> CalculateCompromiseRoute(int startRank, List<int>? calculatedOptimalRoute = default)
    {
        // まず最効率ルートを取得
        List<int> optimalRoute;
        if(calculatedOptimalRoute is null)
        {
            optimalRoute = CalculateOptimalRoute(startRank);
        }
        else
        {
            if (calculatedOptimalRoute.Count < 1) throw new ArgumentException($"{nameof(calculatedOptimalRoute)} is empty.");
            if (calculatedOptimalRoute[0] != startRank) throw new ArgumentException($"{nameof(startRank)} is NOT equal to top value of {nameof(calculatedOptimalRoute)}.");

            optimalRoute = calculatedOptimalRoute;
        }

        var route = new List<int> { startRank };

        if (startRank is 1) return route; // If 1, returns early.

        var current = startRank;

        // 各ステップで、残りの戦闘回数で1位に到達できる最も低い順位を探す
        for (int remainingBattleCount = optimalRoute.Count - 2; remainingBattleCount > 0; remainingBattleCount--)
        {
            int next = -1;
            foreach (var candidateRank in GetAvailableRanksEnumerable(current))
            {
                var count = GetOptimalBattleCount(candidateRank);
                // 残りの戦闘回数が、本来のルートで残っている戦闘回数と一致するかをチェック
                if (count == remainingBattleCount)
                {
                    next = candidateRank;
                    break; // 最も低い順位が見つかったのでループを抜ける
                }
            }

            if (next is -1) throw new InvalidOperationException();

            route.Add(next);
            current = next;
        }

        route.Add(1);
        return route;
    }

    /// <summary>
    /// 最多対戦回数ルートを計算します。
    /// </summary>
    public static List<int> CalculateMaxBattleRoute(int startRank)
    {
        var route = new List<int>() { startRank };
        foreach (var next in GetMaxBattleRouteEnumerable(startRank))
        {
            route.Add(next);
        }

        return route;
    }

    /// <summary>
    /// ユーザー提供のロジックに基づき、最も高い順位を経由するルートにおける順位を列挙可能な構造体を取得します。
    /// </summary>
    public static OptimalRouteEnumerable GetOptimalRouteEnumerable(int startRank) => new(startRank);

    public ref struct OptimalRouteEnumerable
    {
        public int Current { get; private set; }

        public OptimalRouteEnumerable() => Current = 1;

        public OptimalRouteEnumerable(int startRank)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(startRank);
            Current = startRank;
        }

        public bool MoveNext()
        {
            [DoesNotReturn]
            [MethodImpl(MethodImplOptions.NoInlining)]
            static int Throw() => throw new InvalidOperationException("Invalid enumeration.");

            // If 1, enumeration is done.
            if (Current is 1) return false;

            Current = Current switch
            {
                > 13 => Current * 7 / 10,
                > 10 => Current - 4,
                > 4 => Current - 3,
                > 1 => 1,
                _ => Throw() // Throws exception on invalid Current value.
            };

            return true;
        }

        public readonly OptimalRouteEnumerable GetEnumerator() => this;
    }

    /// <summary>
    /// ユーザー提供のロジックに基づき、最も低い順位を経由するルートにおける順位を列挙可能な構造体を取得します。
    /// </summary>
    public static MaxBattleRouteEnumerable GetMaxBattleRouteEnumerable(int startRank) => new(startRank);

    public ref struct MaxBattleRouteEnumerable
    {
        public int Current { get; private set; }

        public MaxBattleRouteEnumerable() => Current = 1;

        public MaxBattleRouteEnumerable(int startRank)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(startRank);
            Current = startRank;
        }

        public bool MoveNext()
        {
            [DoesNotReturn]
            [MethodImpl(MethodImplOptions.NoInlining)]
            static int Throw() => throw new InvalidOperationException("Invalid enumeration.");

            // If 1, enumeration is done.
            if (Current is 1) return false;

            Current = Current switch
            {
                > 13 => Current * 95 / 100,
                > 10 => Current - 2,
                > 1 => Current - 1,
                _ => Throw() // Throws exception on invalid Current value.
            };

            return true;
        }

        public readonly MaxBattleRouteEnumerable GetEnumerator() => this;
    }

    /// <summary>
    /// ユーザー提供のロジックに基づき、指定順位から挑戦可能な順位を列挙可能な構造体を取得します。
    /// </summary>
    public static AvailableRanksEnumerable GetAvailableRanksEnumerable(int startRank) => new(startRank);

    public ref struct AvailableRanksEnumerable
    {
        private readonly int bound;
        public int Current { get; private set; }

        public AvailableRanksEnumerable() => (Current, bound) = (1, 1);

        public AvailableRanksEnumerable(int startRank)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(startRank);

            // Note: Current is decremented before the first access, since MoveNext() is called first in foreach.
            (Current, bound) = startRank switch
            {
                > 13 => (startRank * 95 / 100 + 1, startRank * 7 / 10),
                > 10 => (startRank - 2 + 1, startRank - 4),
                > 4  => (startRank, startRank - 3),
                > 1  => (startRank, 1),
                _ => (1, 1)
            };
        }

        public bool MoveNext()
        {
            // If Current is ≦ bound, enumeration is done.
            if (Current <= bound) return false;

            --Current;
            return true;
        }

        public readonly AvailableRanksEnumerable GetEnumerator() => this;
    }
}
