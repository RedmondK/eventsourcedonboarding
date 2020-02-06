using EventStoreFramework;
using EventStoreFramework.Command;

namespace Domain.Commands
{
    public class Handlers : CommandHandler
    {
        public Handlers(EventStoreRepository repository)
        {
            Register<CreateEntityCommand>(async c =>
            {
                var onboardingCase = new Case(c.EntityId, c.EntityName);
                await repository.Save(onboardingCase);
            });
        }
    }
}
