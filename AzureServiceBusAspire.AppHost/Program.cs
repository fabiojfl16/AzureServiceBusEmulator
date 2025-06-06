var builder = DistributedApplication.CreateBuilder(args);

var serviceBus = builder.AddAzureServiceBus("messaging")
    .RunAsEmulator(emulator => emulator.WithHostPort(7777))
    .AddServiceBusTopic("topic")
    .AddServiceBusSubscription("mysubscription");

builder.AddProject<Projects.PublisherApi>("publisher-api")
    .WithReference(serviceBus)
    .WaitFor(serviceBus);

builder.AddProject<Projects.ConsumerApi>("consumer-api")
    .WithReference(serviceBus)
    .WaitFor(serviceBus);

builder.Build().Run();
