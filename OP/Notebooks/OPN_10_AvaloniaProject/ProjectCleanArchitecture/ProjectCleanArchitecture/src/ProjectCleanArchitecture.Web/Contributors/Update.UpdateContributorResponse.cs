using ProjectCleanArchitecture.Web.ContributorEndpoints;

namespace ProjectCleanArchitecture.Web.Endpoints.ContributorEndpoints;
public class UpdateContributorResponse
{
  public UpdateContributorResponse(ContributorRecord contributor)
  {
    Contributor = contributor;
  }
  public ContributorRecord Contributor { get; set; }
}
