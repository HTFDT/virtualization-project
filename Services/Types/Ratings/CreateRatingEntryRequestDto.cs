using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Services.Types.Ratings;

public class CreateRatingEntryRequestDto : IValidatableObject
{
    [Range(0d, 10d)]
    public double Rating { get; set; }
    public string? Comment { get; set; }
    public IFormFile? Image { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validExtensions = new HashSet<string> { ".jpeg", ".jpg", ".png", ".gif", ".webp", ".svg" };
        
        if (Image is not null && !validExtensions.Contains(Path.GetExtension(Image.FileName)))
            return [new ValidationResult("Invalid image format", [nameof(Image)])];

        return [];
    }
}