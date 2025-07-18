﻿using System.ComponentModel;
using ModelContextProtocol.Server;

namespace Ba.Kuto.RankCalc;

[McpServerToolType, Description("ブルーアーカイブの戦術対抗戦における、1位までのルート（経由する順位）を計算するツールを提供します。")]
public static class RootCalculationTools
{
    public enum Strategy
    {
        Optimal, Compromise, MaxBattles
    }

    public readonly record struct Result(
        string Strategy,
        int BattleCount,
        List<int> Route
        );

    public readonly record struct BiStrategyResult(
        Result OptimalResult,
        Result? CompromiseResult);

    [McpServerTool(UseStructuredContent = true), Description("指定された順位からの最効率のルート、及び、対戦回数の変わらない範囲で妥協できるルートを計算します。妥協ルートは、最効率ルートと異なる場合のみ設定されます。特に戦略の指定がない場合は、この方法でルートを提示します。")]
    public static BiStrategyResult CalculateRoutes([Description("開始順位")] int rank)
    {
        var optimalRoute = RankCalculator.CalculateOptimalRoute(rank);
        var compromiseRoute = RankCalculator.CalculateCompromiseRoute(rank, optimalRoute);

        var optimalResult = new Result()
        {
            Strategy = Strategy.Optimal.ToString(),
            BattleCount = optimalRoute.Count - 1,
            Route = optimalRoute
        };
        Result? compromiseResult = compromiseRoute.SequenceEqual(optimalRoute)
            ? null
            : new Result()
            {
                Strategy = Strategy.Compromise.ToString(),
                BattleCount = compromiseRoute.Count - 1,
                Route = compromiseRoute
            };

        return new(optimalResult, compromiseResult);
    }

    [McpServerTool(UseStructuredContent = true), Description("指定された順位からの最効率のルートを計算します。")]
    public static Result CalculateOptimalRoute([Description("開始順位")] int rank)
    {
        var route = RankCalculator.CalculateOptimalRoute(rank);

        return new()
        {
            Strategy = Strategy.Optimal.ToString(),
            BattleCount = route.Count - 1,
            Route = route
        };
    }

    [McpServerTool(UseStructuredContent = true), Description("指定された順位からの対戦回数の変わらない範囲で妥協できるルートを計算します。")]
    public static Result CalculateCompromiseRoute([Description("開始順位")] int rank)
    {
        var route = RankCalculator.CalculateCompromiseRoute(rank);

        return new()
        {
            Strategy = Strategy.Compromise.ToString(),
            BattleCount = route.Count - 1,
            Route = route
        };
    }

    [McpServerTool(UseStructuredContent = true), Description("指定された順位からの最多対戦回数となるルートを計算します。")]
    public static Result CalculateMaxBattleRoute([Description("開始順位")] int rank)
    {
        var route = RankCalculator.CalculateMaxBattleRoute(rank);

        return new()
        {
            Strategy = Strategy.MaxBattles.ToString(),
            BattleCount = route.Count - 1,
            Route = route
        };
    }
}
