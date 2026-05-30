using OpenGLExercises.Core;

// Launcher for the ported OpenGL exercises.
//
//   dotnet run                       interactive menu
//   dotnet run -- <id>               run one exercise (e.g. ex1, ex7_3, spiral)
//   dotnet run -- --list             list every exercise id
//   dotnet run -- <id> --capture out.png [--frames N]
//                                    render headlessly to a PNG (verification)

string? id = null;
string? capturePath = null;
int frames = 60;

for (int i = 0; i < args.Length; i++)
{
    switch (args[i])
    {
        case "--list":
            PrintList();
            return 0;
        case "--capture":
            capturePath = args[++i];
            break;
        case "--frames":
            frames = int.Parse(args[++i]);
            break;
        default:
            id ??= args[i];
            break;
    }
}

if (id == null)
{
    id = PromptForExercise();
    if (id == null) return 0;
}

var info = Catalog.Find(id);
if (info == null)
{
    Console.Error.WriteLine($"Unknown exercise '{id}'. Use --list to see the options.");
    return 1;
}

if (capturePath != null)
{
    CaptureRequest.Path = capturePath;
    CaptureRequest.AfterFrames = frames;
}

Console.WriteLine($"Running {info.Id}: {info.Title}");
using var window = info.Factory();
window.Run();
return 0;

void PrintList()
{
    foreach (var group in Catalog.All.GroupBy(e => e.Category))
    {
        Console.WriteLine($"\n{group.Key}");
        foreach (var e in group)
            Console.WriteLine($"  {e.Id,-10} {e.Title}");
    }
}

string? PromptForExercise()
{
    var all = Catalog.All;
    if (all.Count == 0)
    {
        Console.Error.WriteLine("No exercises registered yet.");
        return null;
    }

    string? current = null;
    for (int i = 0; i < all.Count; i++)
    {
        if (all[i].Category != current)
        {
            current = all[i].Category;
            Console.WriteLine($"\n{current}");
        }
        Console.WriteLine($"  [{i + 1,2}] {all[i].Id,-10} {all[i].Title}");
    }

    Console.Write("\nSelect an exercise (number or id), or blank to quit: ");
    var line = Console.ReadLine()?.Trim();
    if (string.IsNullOrEmpty(line)) return null;
    if (int.TryParse(line, out int n) && n >= 1 && n <= all.Count) return all[n - 1].Id;
    return line;
}
