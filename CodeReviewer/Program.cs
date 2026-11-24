using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OllamaSharp;

var agentMarkdownPath = Path.Combine("Agents", "code-reviewer.md");
var codePath = Path.Combine("Agents", "code.txt");
const string ollamaUrl = "http://localhost:11434";
const string model = "qwen2.5-coder:7b";

var handler = new HttpClientHandler();
var httpClient = new HttpClient(handler)
{
    BaseAddress = new Uri(ollamaUrl),
    Timeout = TimeSpan.FromMinutes(2)
};

var ollamaClient = new OllamaApiClient(httpClient);

var instructions = File.ReadAllText(agentMarkdownPath);
var code = File.ReadAllText(codePath);

var codeReviewAgent = ollamaClient.AsAIAgent(
    new ChatClientAgentOptions
    {
        Name = "CodeReviewerAgent",
        Description = "Agente especialista em revisão de código C# .NET",
        ChatOptions = new ChatOptions
        {
            ModelId = model,
            Temperature = 0.7f,
            Instructions = instructions
        }
    }
);

await foreach (var token in codeReviewAgent.RunStreamingAsync(code))
    Console.Write(token);
