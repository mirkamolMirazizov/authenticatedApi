using MyLibraryProject;
using System.ComponentModel.DataAnnotations;
using Xunit;
public class ProductTests
{
    [Fact]
    public void Product_WithValidFields_ShouldBeValid()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Price = 9.99M,
            Description = "Test Description"
        };

        var validationContext = new ValidationContext(product);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(product, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void Product_WithNegativePrice_ShouldBeInvalid()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Price = -1M, // Invalid price
            Description = "Test Description"
        };

        var validationContext = new ValidationContext(product);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(product, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains("Price"));
    }
}