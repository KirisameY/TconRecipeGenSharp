// See https://aka.ms/new-console-template for more information

using System.Collections.Immutable;
using System.Text;

using KirisameLib.Extensions;

using TconRecipeGenSharp;

using Tomlyn;
using Tomlyn.Syntax;


#region Check & Parse

if (args.Length == 0)
{
    Console.WriteLine("Please drag and drop input files to this program.");
    Console.ReadKey();
    return 0;
}

var models = args.SelectExist(path =>
{
    if (!File.Exists(path))
    {
        Console.WriteLine($"Input file {path} does not exist.");
        return null;
    }
    var syntax = Toml.Parse(File.ReadAllBytes(path));
    if (syntax.HasErrors)
    {
        Console.WriteLine($"Failed to parse input file {path}. Errors:");
        syntax.Diagnostics.ForEach(Console.WriteLine);
        return null;
    }

    var succeed = syntax.TryToModel<Dictionary<string, InputModel>>(out var dict, out DiagnosticsBag diagnostics);
    Console.WriteLine($"Parsing input file {path} {(succeed ? "succeeded." : "failed.")}");
    if (diagnostics.Count > 0) Console.WriteLine("Diagnostics:");
    diagnostics.ForEach(Console.WriteLine);
    return dict?.Where(p => p.Value.Check());
}).Flatten().ToImmutableArray();

if (!models.Any())
{
    Console.WriteLine("No valid input.");
    Console.ReadKey();
    return 0;
}

Console.WriteLine();

#endregion

var outputPath = @$"{Directory.GetCurrentDirectory()}\recipes";
Console.WriteLine(outputPath);
if (!Directory.Exists(outputPath)) Directory.CreateDirectory(outputPath);

var outputs = models.SelectMany(model => model.Value.GenerateOutput(model.Key));
foreach (var (path, data) in outputs)
{
    var filePath = Path.Combine(outputPath, path);
    Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
    using var outFile = File.OpenWrite(filePath);
    outFile.Write(Encoding.UTF8.GetBytes(data));
    outFile.Flush();
}

Console.WriteLine($"Work done, generation files path: {outputPath}.");
Console.ReadKey();
return 0;