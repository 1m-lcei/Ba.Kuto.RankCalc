const fs = require('fs');
const path = require('path');

const dirPath = path.join(process.cwd(), '.gemini');
const filePath = path.join(dirPath, 'settings.json');

if (!fs.existsSync(dirPath)) {
  fs.mkdirSync(dirPath);
}

const settings = {
  mcpServers: {
    "kivotos": {
      command: "dotnet",
      args: ["run", "--project", "src/Ba.Kuto.RankCalc/Ba.Kuto.RankCalc.csproj"],
      trust: true
    }
  }
};

fs.writeFileSync(filePath, JSON.stringify(settings, null, 2));
console.log("done.");
