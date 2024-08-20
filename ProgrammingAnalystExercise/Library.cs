using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using ProgrammingAnalystExercise;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

// Interface defining the operations that the Library class should implement
public interface ILibrary
{
    void AddBook(Book book);
    void BorrowBook(string title);
    void ReturnBook(string title);
    void RemoveBook(string title);
}

// Library class that manages a collection of books and implements the ILibrary interface
public class Library : ILibrary
{
    private readonly ILogger<Library> _logger;  // Logger to log information and errors
    private List<Book> books = new List<Book>();  // Collection of books in the library

    // Constructor to initialize the library and the logger
    public Library(ILogger<Library> logger)
    {
        _logger = logger;

        try
        {
            // Initialize the library with a set of default books
            books.Add(new Book("The Great Gatsby"));
            books.Add(new Book("1984"));
            books.Add(new Book("To Kill a Mockingbird"));

            _logger.LogInformation("Library initialized with default books.");
        }
        catch (Exception ex)
        {
            // Log any errors that occur during initialization
            _logger.LogError(ex, "Error initializing the library.");
            throw;
        }
    }

    // Method to add a new book to the library
    public void AddBook(Book book)
    {
        // Validate the book parameter
        if (book == null)
        {
            throw new ArgumentNullException(nameof(book), "Book cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(book.Title))
        {
            throw new ArgumentException("Book title cannot be empty or whitespace.", nameof(book));
        }

        // Check if the book already exists in the library
        if (FindBook(book.Title) != null)
        {
            throw new InvalidOperationException($"A book with the title '{book.Title}' already exists in the library.");
        }

        try
        {
            books.Add(book); // Add the book to the collection
            _logger.LogInformation($"The book '{book.Title}' has been added to the Library.");
        }
        catch (Exception ex)
        {
            // Log any errors that occur while adding the book
            _logger.LogError(ex, $"Error adding the book '{book.Title}': {ex.Message}");
            throw;
        }
    }

    // Method to borrow a book from the library by its title
    public void BorrowBook(string title)
    {
        ValidateTitle(title); // Validate the book title

        try
        {
            var book = FindBook(title); // Find the book by its title
            if (book != null)
            {
                book.Borrow(); // Mark the book as borrowed
                _logger.LogInformation($"The book '{title}' has been borrowed.");
            }
            else
            {
                _logger.LogWarning($"The book '{title}' was not found in the library.");
            }
        }
        catch (Exception ex)
        {
            // Log any errors that occur while borrowing the book
            _logger.LogError(ex, $"Error borrowing the book '{title}': {ex.Message}");
            throw;
        }
    }

    // Method to return a book to the library by its title
    public void ReturnBook(string title)
    {
        ValidateTitle(title); // Validate the book title

        try
        {
            var book = FindBook(title); // Find the book by its title
            if (book != null)
            {
                book.Return(); // Mark the book as returned
                _logger.LogInformation($"The book '{title}' has been returned.");
            }
            else
            {
                _logger.LogWarning($"The book '{title}' was not found in the library.");
            }
        }
        catch (Exception ex)
        {
            // Log any errors that occur while returning the book
            _logger.LogError(ex, $"Error returning the book '{title}': {ex.Message}");
            throw;
        }
    }

    // Method to remove a book from the library by its title
    public void RemoveBook(string title)
    {
        ValidateTitle(title); // Validate the book title

        try
        {
            var book = FindBook(title); // Find the book by its title
            if (book != null)
            {
                books.Remove(book); // Remove the book from the collection
                _logger.LogInformation($"The book '{title}' has been removed from the library.");
            }
            else
            {
                _logger.LogWarning($"The book '{title}' was not found in the library.");
            }
        }
        catch (Exception ex)
        {
            // Log any errors that occur while removing the book
            _logger.LogError(ex, $"Error removing the book '{title}': {ex.Message}");
            throw;
        }
    }

    // Private method to validate the book title
    private void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be null or empty.", nameof(title));
        }
    }

    // Private method to find a book by its title
    private Book? FindBook(string title)
    {
        try
        {
            // Search for the book in the collection
            foreach (var book in books)
            {
                if (book.Title == title)
                {
                    return book; // Return the book if found
                }
            }

            // Log the message when the book is not found
            _logger.LogWarning($"The book '{title}' was not found in the library.");
            return null; // Return null if the book is not found
        }
        catch (Exception ex)
        {
            // Log any errors that occur while finding the book
            _logger.LogError(ex, $"Error finding the book '{title}': {ex.Message}");
            throw;
        }
    }
}
