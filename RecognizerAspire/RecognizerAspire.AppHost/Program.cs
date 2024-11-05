using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

// IConfigurationSection containerParams = builder.Configuration.GetSection("ContainerParams");
// string bindMountDir = containerParams.GetSection("DbBindMountDir").Value;
// IConfigurationSection bindDirNames = builder.Configuration.GetSection("BindDirNames");

var grafana = builder.AddContainer("grafana", "grafana/grafana")
    .WithBindMount("../grafana/config", "/etc/grafana")
    .WithBindMount("../grafana/dashboards", "/var/lib/grafana/dashboards")
    .WithEndpoint(3000, 3000, "http", "grafana-http")
    ;
var prometheus = builder.AddContainer("prometheus", "prom/prometheus")
    .WithBindMount("../prometheus", "/etc/prometheus")
    .WithEndpoint(9090, 9090)
    ;


string brainDbName = "recognizer";
var brainDb = builder.AddPostgres("pgBrain", port: 55022)
                    .WithEnvironment("POSTGRES_DB", brainDbName)
                    // .WithDataBindMount(bindMountDir+ bindDirNames.GetSection("Brain").Value)
                    .AddDatabase(brainDbName)
                    .WithHealthCheck();
var brain = builder.AddProject<Projects.Brain>("svcbrain")
                .WithEnvironment("InfrastructureOptions__PostgresConnectionString", brainDb.Resource.ConnectionStringExpression)
                .WithEnvironment("GRAFANA_URL", grafana.GetEndpoint("grafana-http"))
                .WithReplicas(4)
                .WaitFor(brainDb)
                ;



string coversDbName = "covers";
var coversDb = builder.AddPostgres("pgCovers", port: 55023)
                    .WithEnvironment("POSTGRES_DB", coversDbName)
                    // .WithDataBindMount(bindMountDir + bindDirNames.GetSection("Covers").Value)
                    .AddDatabase(coversDbName)
                    .WithHealthCheck();
var covers = builder.AddProject<Projects.Covers>("svccovers")
                    .WithEnvironment("InfrastructureOptions__PostgresConnectionString", coversDb.Resource.ConnectionStringExpression)
                    .WithEnvironment("GRAFANA_URL", grafana.GetEndpoint("grafana-http"))
                    .WaitFor(coversDb);

string metadataDbName = "metadata";
var metadataDb = builder.AddPostgres("pgMetadata", port: 55021)
                    .WithEnvironment("POSTGRES_DB", metadataDbName)
                    // .WithDataBindMount(bindMountDir + bindDirNames.GetSection("Metadata").Value)
                    .AddDatabase(metadataDbName)
                    .WithHealthCheck();
var metadata = builder.AddProject<Projects.Metadata>("svcmetadata")
                    .WithEnvironment("InfrastructureOptions__PostgresConnectionString", metadataDb.Resource.ConnectionStringExpression)
                    .WithEnvironment("GRAFANA_URL", grafana.GetEndpoint("grafana-http"))
                    .WaitFor(metadataDb);

var gateway = builder.AddProject<Projects.Gateway>("gateway")
    .WithReference(brain)
    // .WaitFor(brain)
    // .WithEnvironment("MicroserviceAddresses__BrainAddress", "https://svcbrain")
    .WithReference(covers)
    // .WaitFor(covers)
    // .WithEnvironment("MicroserviceAddresses__CoverAddress", "https://svccover")    
    .WithReference(metadata)
    // .WaitFor(metadata)
    // .WithEnvironment("MicroserviceAddresses__MetadataAddress", "https://svcmetadata")
    .WithEnvironment("GRAFANA_URL", grafana.GetEndpoint("grafana-http"))
    .WaitFor(brain)
    .WaitFor(covers)
    .WaitFor(metadata)
    ;

prometheus.WaitFor(covers).WaitFor(metadata).WaitFor(brain).WaitFor(gateway);
grafana.WaitFor(prometheus);

builder.Build().Run();
