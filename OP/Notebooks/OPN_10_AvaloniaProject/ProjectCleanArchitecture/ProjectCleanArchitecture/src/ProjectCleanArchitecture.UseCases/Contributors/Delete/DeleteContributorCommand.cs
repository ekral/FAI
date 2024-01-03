using Ardalis.Result;
using Ardalis.SharedKernel;

namespace ProjectCleanArchitecture.UseCases.Contributors.Delete;
public record DeleteContributorCommand(int ContributorId) : ICommand<Result>;
