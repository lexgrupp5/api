using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class DocumentRepository : RepositoryBase<Document>, IDocumentRepository
{
    public DocumentRepository(AppDbContext context) : base(context)
    {
        _db = context;
    }

    public async Task<bool> CheckDocumentExistsAsync(Document document)
    {
        return await _db.Documents.AnyAsync(d => d.Name == document.Name);
    }
}