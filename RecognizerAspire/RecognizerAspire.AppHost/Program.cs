using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

IConfigurationSection containerParams = builder.Configuration.GetSection("ContainerParams");
string bindMountDir = containerParams.GetSection("DbBindMountDir").Value;
IConfigurationSection bindDirNames = builder.Configuration.GetSection("BindDirNames");



string brainDbName = "recognizer";
var brainDb = builder.AddPostgres("pgBrain")
                    .WithEnvironment("POSTGRES_DB", brainDbName)
                    // .WithDataBindMount(bindMountDir+ bindDirNames.GetSection("Brain").Value)
                    .AddDatabase(brainDbName)
                    .WithHealthCheck();
var brain = builder.AddProject<Projects.Brain>("brain-svc")
                .WithEnvironment("InfrastructureOptions__PostgresConnectionString", brainDb.Resource.ConnectionStringExpression)
                .WaitFor(brainDb);


string coversDbName = "covers";
var coversDb = builder.AddPostgres("pgCovers")
                    .WithEnvironment("POSTGRES_DB", coversDbName)
                    // .WithDataBindMount(bindMountDir + bindDirNames.GetSection("Covers").Value)
                    .AddDatabase(coversDbName)
                    .WithHealthCheck();
var covers = builder.AddProject<Projects.Covers>("covers-svc")
                    .WithEnvironment("InfrastructureOptions__PostgresConnectionString", coversDb.Resource.ConnectionStringExpression)
                    .WaitFor(coversDb);

string metadataDbName = "metadata";
var metadataDb = builder.AddPostgres("pgMetadata")
                    .WithEnvironment("POSTGRES_DB", metadataDbName)
                    // .WithDataBindMount(bindMountDir + bindDirNames.GetSection("Metadata").Value)
                    .AddDatabase(metadataDbName)
                    .WithHealthCheck();
var metadata = builder.AddProject<Projects.Metadata>("metadata-svc")
                    .WithEnvironment("InfrastructureOptions__PostgresConnectionString", metadataDb.Resource.ConnectionStringExpression)
                    .WaitFor(metadataDb);

builder.AddProject<Projects.Gateway>("gateway")
    .WithReference(brain)
    .WithReference(covers)
    .WithReference(metadata);

builder.Build().Run();
