using System.Text;

namespace cat;

class Program
{
    private static readonly TextWriter _stdout = Console.Out;

    static void Main(string[] args)
    {
        if (args.Length == 0 || args[0] == "-")
        {
            Cat(Console.OpenStandardInput());
        }
        else
        {
            var files = GetFiles(args);
            bool isConsole = Console.IsOutputRedirected == false;

            foreach (string file in files)
            {
                try
                {
                    using var stream = File.OpenRead(file);
                    Cat(stream);               
                }
                catch (FileNotFoundException)
                {
                    string errorMessage = $"cat: {file}: no such file or directory";

                    if (isConsole)
                    {
                        Console.Error.WriteLine(errorMessage);
                    }
                    else
                    {
                        Console.WriteLine(errorMessage);
                    }
                }                
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }

    private static string[] GetFiles(string[] args) =>
        args.Where(a => a != "-").ToArray();

    private static void Cat(Stream stream)
    {
        ProcessStream(stream, s => _stdout.Write(s));
    }

    private static void ProcessStream(Stream stream, Action<string> process)
    {
        var buffer = new byte[4096];
        int bytesRead;

        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            var chunk = Encoding.Default.GetString(buffer, 0, bytesRead);
            process(chunk);
        }
    }
}
