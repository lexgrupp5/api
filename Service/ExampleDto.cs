namespace Service;

#nullable disable

public record ExampleDto
{
    public Guid Id { get; init; }

    public string Name { get; init; }
    
    public string Value { get; init; }    
}