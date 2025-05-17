// See https://aka.ms/new-console-template for more information

List<string> errors = [];

if (args is not [var path]) errors.Add("Please drag and drop a input file to this program.");

if (errors.Count > 0)
{
    Console.WriteLine("Errors:");
    foreach (var error in errors)
    {
        Console.WriteLine(error);
    }
    return 0;
}

Console.WriteLine("Hello, World!");
return 0;