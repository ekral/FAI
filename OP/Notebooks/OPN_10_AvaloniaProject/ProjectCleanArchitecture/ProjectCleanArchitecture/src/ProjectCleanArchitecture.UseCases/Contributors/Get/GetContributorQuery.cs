using Ardalis.Result;
using Ardalis.SharedKernel;

namespace ProjectCleanArchitecture.UseCases.Contributors.Get;
public record GetContributorQuery(int ContributorId) : IQuery<Result<ContributorDTO>>;
