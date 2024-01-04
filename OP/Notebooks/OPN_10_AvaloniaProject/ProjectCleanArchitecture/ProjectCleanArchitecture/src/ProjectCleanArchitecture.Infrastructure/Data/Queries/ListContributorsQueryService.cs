﻿using Microsoft.EntityFrameworkCore;
using ProjectCleanArchitecture.UseCases.Contributors;
using ProjectCleanArchitecture.UseCases.Contributors.List;

namespace ProjectCleanArchitecture.Infrastructure.Data.Queries;
public class ListContributorsQueryService(AppDbContext _db) : IListContributorsQueryService
{
  // You can use EF, Dapper, SqlClient, etc. for queries - this is just an example

  public async Task<IEnumerable<ContributorDTO>> ListAsync()
  {
    // NOTE: This will fail if testing with EF InMemory provider
    var result = await _db.Contributors.FromSqlRaw("SELECT Id, Name FROM Contributors") // don't fetch other big columns
      .Select(c => new ContributorDTO(c.Id, c.Name))
      .ToListAsync();

    return result;
  }
}
