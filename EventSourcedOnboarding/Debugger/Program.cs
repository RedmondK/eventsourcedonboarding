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
                ,new EntityProjection()
            };

            var p = new Projector(eventStoreConnection, projections, new MongoDAL.MongoDBRepository());

            p.Start();

            Console.WriteLine("Projector Running");
            Console.ReadLine();
        }
    }
}
