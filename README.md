# Ba.Kuto.RankCalc

このプロジェクトは Gemini CLI と戦術対抗戦の順位ルートに関する対話してみようというものです。お試し用途。

![デモ](./images/image.png)

## 📝 概要

このリポジトリを `git clone` し、クローンしたディレクトリで Gemini CLI の対話モードを起動することで、戦術対抗戦の順位計算に関する対話を行うことができます。

背後のツール（MCPサーバー）をC#で作成してしまったので、.NET 9 プロジェクトを `dotnet run` できる環境が必要です。

## ✨ 特徴

例えば、以下のように話しかけることで、順位推移のルート（いわゆる「登頂ルート」）を提案させることができます。

*   `対抗戦で15001位からのルートを提案して`
*   `対抗戦で855位から最多対戦回数のルートを教えて`

## 🚀 使い方

### 🛠️ 必要環境

*   Gemini CLI
*   .NET 9.0 SDK

### 📦 セットアップ

```shell
git clone https://github.com/1m-lcei/Ba.Kuto.RankCalc.git
cd Ba.Kuto.RankCalc
node setup.js
```

`setup.js` はディレクトリに固有の設定（`.gemini/settings.json`）を生成します。Gemini CLI には現状、コマンドでMCPサーバーを追加する方法がないようなのでこの方法を取っています。

### ▶️ 実行

クローンしたディレクトリで、Gemini CLI の対話モードを開始します。

```shell
gemini
```

その後、登頂ルートの提案の指示などをしてください。
