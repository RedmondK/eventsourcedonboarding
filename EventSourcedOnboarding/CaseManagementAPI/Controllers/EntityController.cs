using CaseManagementAPI.Requests;
using Domain.Commands;
using EventStore.ClientAPI;
using EventStoreFramework;
using EventStoreFramework.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CaseManagementAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EntityController
    {
        private readonly ILogger<EntityController> _logger;
        private readonly Dispatcher _commandDispatcher;

        public EntityController(ILogger<EntityController> logger)
        {
            _logger = logger;
            _commandDispatcher = SetupDispatcher().GetAwaiter().GetResult();
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]AddBasicDetailsRequest request)
        {
            var addBasicDetailsCommand = new AddBasicDetails(request.CaseID, request.EntityID, request.DateOfBirth, request.CountryOfResidence);

            _commandDispatcher.Dispatch(addBasicDetailsCommand);
        }

        private static async Task<Dispatcher> SetupDispatcher()
        {
            var eventStoreConnection = EventStoreConnection.Create(
                ConnectionSettings.Default,
                new IPEndPoint(IPAddress.Loopback, 1113));

            await eventStoreConnection.ConnectAsync();

            var repository = new EventStoreRepository(eventStoreConnection);

            var commandHandlerMap = new CommandHandlerMap(new Handlers(repository));

            return new Dispatcher(commandHandlerMap);
        }
    }
}
