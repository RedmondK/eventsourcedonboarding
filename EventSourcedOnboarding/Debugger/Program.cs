using EventStore.ClientAPI;
using ProjectionFramework;
using System;
using System.Collections.Generic;
using System.Net;

namespace Debugger
{
    class Program
    {
        static void Main(string[] args)
        {
            var eventStoreConnection = EventStoreConnection.Create(
               ConnectionSettings.Default,
               new IPEndPoint(IPAddress.Loopback, 1113));

            eventStoreConnection.ConnectAsync().Wait();

            var projections = new List<IProjection>
            {
                new CaseProjection()
            };

            var p = new Projector(eventStoreConnection, projections, new MongoDAL.MongoDBRepository());

            var events = eventStoreConnection.ReadStreamEventsForwardAsync("Case+073dce4d-5693-4e1a-9635-1a83ab12236c", 0, 10, true).Result;

            p.Start();

            Console.WriteLine("Projector Running");
            Console.ReadLine();
        }
    }
}
