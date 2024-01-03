using ProjectCleanArchitecture.Web.ContributorEndpoints;

namespace ProjectCleanArchitecture.Web.Endpoints.ContributorEndpoints;
public class ContributorListResponse
{
  public List<ContributorRecord> Contributors { get; set; } = new();
}
