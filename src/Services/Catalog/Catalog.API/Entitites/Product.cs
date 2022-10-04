using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Api.Entities;

public sealed record Product
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; init; }
    [BsonElement("Name")]
    public string Name { get; init; }
    public string Category { get; init; }
    public string Summary { get; init; }
    public string Description { get; init; }
    public string ImageFile { get; init; }
    public decimal Price { get; init; }

    public Product(string id, string name, string category, string summary, string description, string imageFile, decimal price)
    {
        Id = id;
        Name = name;
        Category = category;
        Summary = summary;
        Description = description;
        ImageFile = imageFile;
        Price = price;
    }
    public Product(){}
}