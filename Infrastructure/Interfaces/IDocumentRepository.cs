using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IDocumentRepository : IRepositoryBase<Document>
{
    Task<bool> CheckDocumentExistsAsync(Document document);
}
