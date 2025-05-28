using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Services.Types.Threads;

public class CreateThreadRequestDto : IValidatableObject
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public IFormFile? Image { get; set; } = null!;
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validExtensions = new HashSet<string> { ".jpeg", ".jpg", ".png", ".gif", ".webp", ".svg" };
        
        if (Image is not null && !validExtensions.Contains(Path.GetExtension(Image.FileName)))
            return [new ValidationResult("Invalid image format", [nameof(Image)])];

        return [];
    }
}