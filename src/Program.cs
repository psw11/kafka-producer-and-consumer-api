using Microsoft.Extensions.DependencyInjection;
using Kafka.Workers;


// Dependencies
var serviceProvider = Kafka.ServiceProvider.Create();
var workerForAutoCommit = serviceProvider.GetService<WorkerForAutoCommit>();
var workerForManualCommit = serviceProvider.GetService<WorkerForManualCommit>();
var workerForManyConsume = serviceProvider.GetService<WorkerForManyConsumers>();
var workerForManualReadPartition = serviceProvider.GetService<WorkerForManualReadPartition>();
var workerWithConfigTopic = serviceProvider.GetService<WorkerWithConfigTopic>();
var workerWithJsonSerialization = serviceProvider.GetService<WorkerWithJsonSerialization>();
var workerWithAvroSerialization = serviceProvider.GetService<WorkerWithAvroSerialization>();
var workerWithProtoSerialization = serviceProvider.GetService<WorkerWithProtoSerialization>();


// Use cases
if (workerForAutoCommit != null)
{
    //workerForAutoCommit.RunProduce();
    //Task.Delay(4000).Wait();
    //workerForAutoCommit.RunConsume();
}

if (workerForManualCommit != null)
{
    //workerForManualCommit.RunProduce();
    //Task.Delay(4000).Wait();
    //workerForManualCommit.RunConsume();
}

if (workerForManyConsume != null)
{
    //workerForManyConsume.RunProduce();
    //Task.Delay(4000).Wait();

    //workerForManyConsume.RunAutoConsume();
    //workerForManyConsume.RunManualConsume();
}

if (workerForManualReadPartition != null)
{
    //workerForManualReadPartition.RunProduce();
    //Task.Delay(4000).Wait();
    //workerForManualReadPartition.RunConsume();
}

if (workerWithConfigTopic != null)
{
    workerWithConfigTopic.RunProduce();
    Task.Delay(4000).Wait();
    workerWithConfigTopic.RunConsume();
}

if (workerWithJsonSerialization != null)
{
    //workerWithJsonSerialization.RunProduce();
    //Task.Delay(4000).Wait();
    //workerWithJsonSerialization.RunConsume();
}

if (workerWithAvroSerialization != null)
{
    //workerWithAvroSerialization.RunProduce();
    //Task.Delay(4000).Wait();
    //workerWithAvroSerialization.RunConsume();
}

if (workerWithProtoSerialization != null)
{
    //workerWithProtoSerialization.RunProduce();
    //Task.Delay(4000).Wait();
    //workerWithProtoSerialization.RunConsume();
}

// Waiting for any key
Console.ReadLine();
