var builder = WebApplication.CreateBuilder(args);

// If you keep service registrations in Startup, invoke it so they are applied
var startup = new StoreHub.API.Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// Run Startup.Configure to apply middleware and endpoint mappings
startup.Configure(app, app.Environment);

app.Run();