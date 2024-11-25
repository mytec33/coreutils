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
            string[] files = GetFiles(args);
            bool isConsole = Console.IsOutputRedirected == false;

            foreach (string file in files)
            {
                try
                {
                    using var stream = File.OpenRead(file);
                    CatFiles(stream);
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

    private static void CatFiles(Stream stream)
    {
        var buffer = new byte[4];
        int bytesRead;
        var stdout = Console.OpenStandardOutput();

        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            var lineStartIndex = 0;

            for (int i = 0; i < buffer.Length; i++ )
            {
                if (buffer[i] == 0x00 || i > bytesRead)
                    continue;

                if (buffer[i] == 0xa)
                {
                    stdout.Write(buffer, lineStartIndex, (i - lineStartIndex) + 1);
                    lineStartIndex = i + 1;
                }
            }
        }
    }

    private static void ProcessStream(Stream stream, Action<string> process)
    {
        var buffer = new byte[64*1024];
        int bytesRead;
        var encoding = Encoding.UTF8;

        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            var chunk = encoding.GetString(buffer.AsSpan(0, bytesRead));
            process(chunk);
        }
    }
}
