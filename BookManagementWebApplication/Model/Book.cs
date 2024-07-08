namespace BookManagementWebApplication.Model;


/// <summary>
///     Class <c>Book</c>> for storing some book data (Title, Author, Publish Year, Genre)
///     Each Book has its unique identifier - Id that is auto-incremented value for database storage. 
/// </summary>
public class Book
{
    public int Id { get; set; }
    public String Title { get; set; } = "";
    public String Author { get; set; } = "";
    public int PublishedYear { get; set; }
    public String Genre { get; set; } = "";

    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is Book))
        {
            return false;
        }

        var other = (Book)obj;
        return other.Id == Id &&
               other.Author == Author &&
               other.Genre == Genre &&
               other.PublishedYear == PublishedYear &&
               other.Title == Title;
    }
}