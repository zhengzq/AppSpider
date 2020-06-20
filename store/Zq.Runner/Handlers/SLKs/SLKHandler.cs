using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Zq.Runner.Handlers
{
    public class SLKHandler : IHandler
   {
        private readonly IServiceProvider _rootServiceProvider;
        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;
        private readonly IConfiguration _configuration;

        public SLKHandler(IServiceProvider rootServiceProvider, IConfiguration configuration)
        {
            this._rootServiceProvider = rootServiceProvider;
            this._factory = new ConnectionFactory()
            {
                Uri = new Uri(configuration["RabbitMQ:Connection"])
            };
            this._connection = this._factory.CreateConnection();
            this._channel = this._connection.CreateModel();
            this._configuration = configuration;
        }

        public Task Handle()
        {
            _channel.QueueDeclare("36k", false, false, false, null);

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            Console.WriteLine(" [*] Waiting for 36k messages.");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                using (var conn = new SqlConnection(_configuration["ConnectionStrings:SqlConnection"]))
                {
                    var message = Encoding.UTF8.GetString(ea.Body);
                    var obj = JsonConvert.DeserializeObject<JArray>(message);
                    using (var scope = _rootServiceProvider.CreateScope())
                    {

                        foreach (var item in obj)
                        {
                            try
                            {
                                conn.Execute(@"INSERT INTO Slk(Data,Code) VALUES (@Data,@Code)", new
                                {
                                    Data = item.ToString(),
                                    Code = item.Value<long>("id")
                                });

                                conn.Execute
                                (
                                    @"INSERT INTO [Summary](Title,Content,Link,Source,PublishedTime) VALUES (@Title,@Content,@Link,'36k',@PublishedTime)",
                                    new
                                    {
                                        Title = item.Value<string>("title"),
                                        Content = item.Value<string>("description"),
                                        Link = item.Value<string>("news_url"),
                                        PublishedTime = item.Value<DateTime>("published_at")
                                    }
                                );
                            }
                            catch (SqlException)
                            {
                                //Console.WriteLine($"{ item.Value<long>("id") } 已存在");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"{ DateTime.Now.ToString() } 36k { e.ToString() }");
                                Console.WriteLine($"{ DateTime.Now.ToString() } 36k { item.ToString() }");
                            }
                        }
                    }

                    Console.WriteLine($"{ DateTime.Now.ToString() } 36k store Done");

                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
            };

            _channel.BasicConsume("36k", false, consumer);

            return Task.Delay(0);
        }

        public void Dispose()
        {
            this._channel.Close();
            this._connection.Close();
        }
    }
}
