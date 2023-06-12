using FatecLibrary.BookAPI.Context;
using FatecLibrary.BookAPI.Models.Entities;
using FatecLibrary.BookAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FatecLibrary.BookAPI.Repositories.Entities;
public class PublishingRepository : IPublishingRepository
{

    // o que que os repositories fazem?
    // Fazem o acesso dos métodos ao banco de dados

    private readonly AppDBContext _dbContext;

    public PublishingRepository(AppDBContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<IEnumerable<Publishing>> GetAll()
    {
        return await _dbContext.Publishers.ToListAsync();
    }

    public async Task<IEnumerable<Publishing>> GetPublishersBooks()
    {
        return await _dbContext.Publishers.Include(p => p.Books).ToListAsync();
    }

    public async Task<Publishing> GetById(int id)
    {
        return await _dbContext.Publishers.Where(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Publishing> Create(Publishing publishing)
    {
        _dbContext.Publishers.Add(publishing);
        await _dbContext.SaveChangesAsync();
        return publishing;
    }

    public async Task<Publishing> Update(Publishing publishing)
    {
        _dbContext.Entry(publishing).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return publishing;
    }

    public async Task<Publishing> Delete(int id)
    {
        var publishing = await GetById(id);
        _dbContext.Publishers.Remove(publishing);
        await _dbContext.SaveChangesAsync();
        return publishing;
    }
}
