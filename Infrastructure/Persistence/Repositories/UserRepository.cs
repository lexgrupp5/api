using AutoMapper;
using AutoMapper.QueryableExtensions;

using Domain.DTOs;
using Domain.Entities;

using Infrastructure.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    private readonly IMapper _mapper;
    public UserRepository(AppDbContext context, IMapper mapper) : base(context)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> CheckUsernameExistsAsync(User user)
    {
        return await _context.Users.AnyAsync(u => u.Name == user.Name);
    }

    public async Task<IEnumerable<UserDto>> GetAllStudentsAsync()
    {
        var queryResults = await GetAll()
            .Where(u => u.Role.Name == "Student")
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider, new[] { nameof(UserDto.Name) })
            .ToListAsync();

        return queryResults;
    }
}