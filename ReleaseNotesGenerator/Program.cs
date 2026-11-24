using Microsoft.Agents.AI;
using OpenAI;
using OpenAI.Chat;

// Caminhos dos arquivos de configuração do agente
const string agentMarkdownPath = "Agents/release_notes_writer.md";
const string changelogPath = "Agents/changelog.txt";

// Modelo a ser utilizado
const string model = "gpt-4o-mini";

// Leitura da chave da API a partir de variável de ambiente
var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
    ?? throw new InvalidOperationException("Variável OPENAI_API_KEY não encontrada.");

// Leitura dos arquivos em tempo de execução
var instructions = File.ReadAllText(agentMarkdownPath);
var changelog = File.ReadAllText(changelogPath);

// Criação do cliente OpenAI e obtenção do chat client para o modelo desejado
var client = new OpenAIClient(apiKey);
var chatClient = client.GetChatClient(model);

// Criação do agente MAF usando o chat client da OpenAI como provedor
var releaseNotesAgent = chatClient.AsAIAgent(
    instructions: instructions,
    name: "ReleaseNotesAgent");

// Execução do agente com o changelog como entrada
Console.WriteLine("Gerando release notes...\n");

var result = await releaseNotesAgent.RunAsync(
    $"Transforme o changelog abaixo em release notes seguindo o formato JSON definido nas suas instruções.\n\n{changelog}");

Console.WriteLine(result);