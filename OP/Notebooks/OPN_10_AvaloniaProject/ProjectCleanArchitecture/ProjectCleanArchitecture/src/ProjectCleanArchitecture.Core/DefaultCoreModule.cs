using Autofac;
using ProjectCleanArchitecture.Core.Interfaces;
using ProjectCleanArchitecture.Core.Services;

namespace ProjectCleanArchitecture.Core;
/// <summary>
/// An Autofac module that is responsible for wiring up services defined in the Core project.
/// </summary>
public class DefaultCoreModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<DeleteContributorService>()
        .As<IDeleteContributorService>().InstancePerLifetimeScope();
  }
}
