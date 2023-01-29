using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Kafka.Workers;
using Kafka.CustomConfig;
using Kafka.CustomApi;
using Kafka.BaseApi;
using Kafka.CustomSerializers;

namespace Kafka
{
    internal static class ServiceProvider
    {
        public static IServiceProvider Create()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton(GetConfiguration())

                .AddSingleton<KafkaAutoCommitConfig>()
                .AddSingleton<KafkaManualCommitConfig>()
                .AddSingleton<IKafkaProducerClient<KafkaAutoCommitConfig>, KafkaProducerClient<KafkaAutoCommitConfig>>()
                .AddSingleton<IKafkaProducerClient<KafkaManualCommitConfig>, KafkaProducerClient<KafkaManualCommitConfig>>()

                .AddTransient<KafkaStringAutoProducer>()    // Producers and consumers for different config
                .AddTransient<KafkaStringAutoConsumer>()
                .AddTransient<KafkaStringManualProducer>()
                .AddTransient<KafkaStringManualConsumer>()

                .AddTransient<KafkaJsonSerializer>()
                .AddTransient<KafkaAvroSerializer>()
                .AddTransient<KafkaProtoSerializer>()

                .AddTransient<KafkaJsonAutoProducer>()
                .AddTransient<KafkaJsonAutoConsumer>()
                .AddTransient<KafkaAvroAutoProducer>()
                .AddTransient<KafkaAvroAutoConsumer>()
                .AddTransient<KafkaProtoAutoProducer>()
                .AddTransient<KafkaProtoAutoConsumer>()

                .AddSingleton<WorkerForAutoCommit>()
                .AddSingleton<WorkerForManualCommit>()
                .AddSingleton<WorkerForManyConsumers>()
                .AddSingleton<WorkerForManualReadPartition>()
                .AddSingleton<WorkerWithConfigTopic>()
                .AddSingleton<WorkerWithJsonSerialization>()
                .AddSingleton<WorkerWithAvroSerialization>()
                .AddSingleton<WorkerWithProtoSerialization>()
                .BuildServiceProvider();

            return serviceProvider;
        }

        private static IConfiguration GetConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            return configuration;
        }
    }
}
