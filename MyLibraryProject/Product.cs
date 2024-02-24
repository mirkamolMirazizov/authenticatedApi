namespace MyLibraryProject;

public class Product
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Category Category { get; set; } // Navigation property
}
