using Anthropic;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var agentMarkdownPath = Path.Combine("Agents", "code-reviewer.md");
var codePath = Path.Combine("Agents", "code.txt");
var apiKey = config["Anthropic:ApiKey"];

const string model = "claude-sonnet-4-6";
var client = new AnthropicClient { ApiKey = apiKey };

var instructions = File.ReadAllText(agentMarkdownPath);
var code = File.ReadAllText(codePath);

var codeReviewAgent = client.AsAIAgent(
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
