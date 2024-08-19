using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace ProgrammingAnalystExercise
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Configure Serilog to log to a file
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day) // Logs will be stored in "logs/log.txt", with a new file created daily
                .CreateLogger(); // Create and configure the logger

            var serviceProvider = new ServiceCollection() // Create a new service collection for dependency injection
                .AddLogging(config =>
                {
                    config.ClearProviders(); // Remove any default logging providers
                    config.AddSerilog(); // Add Serilog as the logging provider
                })
                .AddSingleton<Library>() // Register the Library class as a singleton service
                .BuildServiceProvider(); // Build the service provider to resolve dependencies

            // Get an instance of Library with ILogger<Library> injected
            var library = serviceProvider.GetService<Library>();

            var newBook = new Book("Great Expectations"); // Create a new book instance

            // Use the Library methods to add, borrow, return, and remove a book
            library.AddBook(newBook); 
            library.BorrowBook("Great Expectations"); 
            library.ReturnBook("Great Expectations"); 
            library.RemoveBook("Great Expectations"); 
        }
    }
}
