using Ardalis.Result;
using Ardalis.SharedKernel;
using FastEndpoints;
using MediatR;
using ProjectCleanArchitecture.Core.ContributorAggregate;
using ProjectCleanArchitecture.UseCases.Contributors.Create;
using ProjectCleanArchitecture.UseCases.Contributors.Get;
using ProjectCleanArchitecture.UseCases.Contributors.Update;
using ProjectCleanArchitecture.Web.Endpoints.ContributorEndpoints;

namespace ProjectCleanArchitecture.Web.ContributorEndpoints;
/// <summary>
/// Update an existing Contributor.
/// </summary>
/// <remarks>
/// Update an existing Contributor by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator _mediator)
  : Endpoint<UpdateContributorRequest, UpdateContributorResponse>
{
  public override void Configure()
  {
    Put(UpdateContributorRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(
    UpdateContributorRequest request,
    CancellationToken cancellationToken)
  {
    var result = await _mediator.Send(new UpdateContributorCommand(request.Id, request.Name!));

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var query = new GetContributorQuery(request.ContributorId);

    var queryResult = await _mediator.Send(query);

    if (queryResult.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    if (queryResult.IsSuccess)
    {
      var dto = queryResult.Value;
      Response = new UpdateContributorResponse(new ContributorRecord(dto.Id, dto.Name));
      return;
    }
  }
}
