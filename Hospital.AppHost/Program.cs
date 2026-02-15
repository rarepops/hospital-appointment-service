var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Hospital_WebApi>("hospital-webapi");

builder.Build().Run();
