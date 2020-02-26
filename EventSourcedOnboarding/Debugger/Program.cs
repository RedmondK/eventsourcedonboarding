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

            var mongoRepository = new MongoDAL.MongoDBRepository();

            var projections = new List<IProjection>
            {
                new CaseProjection(mongoRepository)
                ,new EntityProjection(mongoRepository)
            };

            var p = new Projector(eventStoreConnection, projections, mongoRepository);

            p.Start();

            Console.WriteLine("Projector Running");
            Console.ReadLine();
        }
    }
}
