using System.Text.Json;

using KirisameLib.Extensions;

using Dict = System.Collections.Generic.Dictionary<string, object>;

namespace TconRecipeGenSharp;

public class InputModel
{
    public string Item { get; set; } = "";
    public string Material { get; set; } = "";
    public string Molten { get; set; } = "";

    public int Temperature { get; set; } = 6000;
    public int Time { get; set; } = 60;

    public bool Check() => !new[] { Item, Material, Molten }.Any(string.IsNullOrEmpty);

    public IEnumerable<(string path, string data)> GenerateOutput(string name) =>
        OutPutModels(name).Select(t => (t.path, JsonSerializer.Serialize(t.data, new JsonSerializerOptions
        {
            WriteIndented = true,
            IndentSize    = 4,
        })));

    private IEnumerable<(string path, object data)> OutPutModels(string name)
    {
        // smeltery\
        yield return (@$"smeltery\melting\{name}.json", new Dict
        {
            ["type"] = "tconstruct:melting",
            ["ingredient"] = new Dict
            {
                ["item"] = Item,
            },
            ["temperature"] = Temperature,
            ["time"]        = Time,
            ["result"] = new Dict
            {
                ["fluid"]  = Molten,
                ["amount"] = 90,
            },
        });

        yield return (@$"smeltery\casting\{name}.json", new Dict
        {
            ["type"] = "tconstruct:casting_table",
            ["cast"] = new Dict
            {
                ["tag"] = "tconstruct:casts/multi_use/ingot",
            },
            ["cast_consumed"] = false,
            ["fluid"] = new Dict
            {
                ["name"]   = Molten,
                ["amount"] = 90,
            },
            ["result"]       = Item,
            ["cooling_time"] = Time,
        });

        // materials\
        yield return (@$"materials\{name}\{name}_material_melting.json", new Dict
        {
            ["type"]        = "tconstruct:material_melting",
            ["input"]       = Item,
            ["temperature"] = Temperature,
            ["result"] = new Dict
            {
                ["fluid"]  = Molten,
                ["amount"] = 90,
            },
        });

        yield return (@$"materials\{name}\{name}_material_casting.json", new Dict
        {
            ["type"] = "tconstruct:material_fluid",
            ["fluid"] = new Dict
            {
                ["name"]   = Molten,
                ["amount"] = 90,
            },
            ["temperature"] = Temperature,
            ["output"]      = Material,
        });

        yield return (@$"materials\{name}\{name}_ingot_casting.json", new Dict
        {
            ["type"] = "tconstruct:casting_table",
            ["cast"] = new Dict
            {
                ["tag"] = "tconstruct:casts/multi_use/ingot",
            },
            ["cast_consumed"] = false,
            ["fluid"] = new Dict
            {
                ["name"]   = Molten,
                ["amount"] = 90,
            },
            ["result"]       = Item.Split(':', 2).Join(":item/"),
            ["cooling_time"] = Time,
        });

        yield return (@$"materials\{name}\{name}_fix.json", new Dict
        {
            ["type"] = "tconstruct:material",
            ["ingredient"] = new Dict
            {
                ["item"] = Item,
            },
            ["value"]    = 1,
            ["needed"]   = 1,
            ["material"] = Material,
        });
    }
}