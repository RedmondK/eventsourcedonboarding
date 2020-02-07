using Domain.Enums;
using EventStoreFramework;
using EventStoreFramework.Command;

namespace Domain.Commands
{
    public class Handlers : CommandHandler
    {
        public Handlers(EventStoreRepository repository)
        {
            Register<CreateEntity>(async c =>
            {
                var onboardingCase = new Case(CaseType.Onboarding, c.EntityId, c.EntityName);
                await repository.Save(onboardingCase);
            });
        }
    }
}
