using Autofac;
using DigitalMarket.Data.UnitOfWork;

namespace DigitalMarket.Business.Infrastructure.DependencyResolvers
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(UnitOfWork<>)).As(typeof(IUnitOfWork<>)).InstancePerLifetimeScope();

        }
    }
}
