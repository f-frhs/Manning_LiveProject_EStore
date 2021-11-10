using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Driver;
using ShoppingCartService.Config;
using ShoppingCartService.Mapping;
using Xunit;

namespace ShoppingCartServiceTests.Fixtures
{
    public class DockerMongoFixtures : IDisposable
    {
        private Process _processRun;
        private readonly string _connectionString = "mongodb://localhost:1111";
        internal IMapper Mapper { get; }

        public ShoppingCartDatabaseSettings GetDatabaseSettings() => new()
        {
            CollectionName = "ShoppingCart",
            ConnectionString = _connectionString,
            DatabaseName = "ShoppingCartDb",
        };

        public DockerMongoFixtures()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            Mapper = config.CreateMapper();

            _processRun = Process.Start($"Docker", $" run --name mongo_test -p 1111:27017 mongo");

            var startAt = DateTime.Now;
            var isConnected = WaitForMongoDbConnection(_connectionString, "admin");
            var endAt = DateTime.Now;
            if (!isConnected)
            {
                var duration = endAt - startAt;
                throw new Exception(
                    $"Startup failed, could not get MongoDB connection after trying for '{duration}'");
            }
        }

        public void Dispose()
        {
            Console.Out.WriteLine("Dispose called");

            if (_processRun != null)
            {
                _processRun.Dispose();
                _processRun = null;
            }

            var processStop = Process.Start("docker", $"stop mongo_test");
            processStop?.WaitForExit();
            var processRm = Process.Start("docker", $"rm mongo_test");
            processRm?.WaitForExit();
        }



        private static bool WaitForMongoDbConnection(string connectionString, string dbName)
        {
            Console.Out.WriteLine("Waiting for Mongo to respond");
            var probeTask = Task.Run(() =>
            {
                var isAlive = false;
                var client = new MongoClient(connectionString);

                for (var i = 0; i < 3000; i++)
                {
                    client.GetDatabase(dbName);
                    var server = client.Cluster.Description.Servers.FirstOrDefault();
                    isAlive = server != null &&
                              server.HeartbeatException == null &&
                              server.State == MongoDB.Driver.Core.Servers.ServerState.Connected;

                    if (isAlive)
                    {
                        break;
                    }

                    Thread.Sleep(100);
                }

                return isAlive;
            });
            probeTask.Wait();

            return probeTask.Result;
        }
    }
}