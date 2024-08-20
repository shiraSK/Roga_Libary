using System;
using System.IO;
using Microsoft.Extensions.Configuration;

public static class FileLogger
{
    // This field holds the path to the log file. It is initialized in the static constructor.
    private static readonly string logFilePath;

    // Static constructor to initialize the configuration and log file path.
    static FileLogger()
    {
        // Create a configuration builder to read from appsettings.json.
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Set the base path to the current directory of the executable.
            .AddJsonFile("appsettings.json") // Add the JSON configuration file to the configuration builder.
            .Build(); // Build the configuration.

        // Read the log file path from the configuration.
        logFilePath = configuration["Logging:LogFilePath"];

        //Check if the logFilePath is set correctly
        if (string.IsNullOrEmpty(logFilePath))
        {
            throw new InvalidOperationException("Log file path is not configured correctly.");
        }
    }

    // Method to log messages to the log file.
    public static void Log(string message)
    {
        try
        {
            // Create or append to the log file using StreamWriter.
            using (StreamWriter writer = new StreamWriter(logFilePath, append: true))
            {
                // Write the log message with a timestamp.
                writer.WriteLine($"{DateTime.Now}|{message}");
            }
        }
        catch (Exception ex)
        {
            // If an error occurs while writing to the log file, output the error to the console.
            Console.WriteLine($"Error writing to log file: {ex.Message}");
        }
    }
}
