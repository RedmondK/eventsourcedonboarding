﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CaseManagementAPI.Requests;
using Domain.Commands;
using EventStore.ClientAPI;
using EventStoreFramework;
using EventStoreFramework.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectionFramework;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CaseManagementAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CaseController : Controller
    {
        private readonly ILogger<CaseController> _logger;
        private readonly Dispatcher _commandDispatcher;
        private readonly Projector _projector;

        public CaseController(ILogger<CaseController> logger)
        {
            _logger = logger;
            _commandDispatcher = SetupDispatcher().GetAwaiter().GetResult();
            _projector = SetupProjector().GetAwaiter().GetResult();
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            _projector.Start();

            return "";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]CreateEntityRequest request)
        {
            var createEntityCommand = new CreateEntity(request.EntityName);

            _commandDispatcher.Dispatch(createEntityCommand);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
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

        private static async Task<Projector> SetupProjector()
        {
            var eventStoreConnection = EventStoreConnection.Create(
                ConnectionSettings.Default,
                new IPEndPoint(IPAddress.Loopback, 1113));

            await eventStoreConnection.ConnectAsync();

            var projections = new List<IProjection>
            {
                new CaseProjection()
            };

            return new Projector(eventStoreConnection, projections, new MongoDAL.MongoDBRepository());
        }
    }
}
