using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;

internal class Program
{
    private static async Task Main(string[] args)
    {
        string endpoint = Environment.GetEnvironmentVariable("AZURE_ENDPOINT");
        string key = Environment.GetEnvironmentVariable("AZURE_KEY");

        var credential = new AzureKeyCredential(key);
        var client = new DocumentAnalysisClient(new Uri(endpoint), credential);


        Uri fileUri = new Uri("https://henriquestoragetest1.blob.core.windows.net/containertest1/Nubank_2024-1211test.pdf");


        var operation = await client.AnalyzeDocumentFromUriAsync(WaitUntil.Completed, "prebuilt-document", fileUri);
        var result = operation.Value;


        Console.WriteLine("Extraindo informações da fatura...");

        string totalAPagar = null;

        foreach (var kvp in result.KeyValuePairs)
        {
            if (kvp.Key.Content.Contains("Total a pagar", StringComparison.OrdinalIgnoreCase))
            {
                totalAPagar = kvp.Value?.Content;
            }
        }


        Console.WriteLine($"Valor Total: {totalAPagar ?? "Não encontrado"}");
    }
}