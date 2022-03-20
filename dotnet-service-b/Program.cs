using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Context.Propagation;

var serviceName = "dotnet-service-b";
var serviceVersion = "1.0.0";

var builder = WebApplication.CreateBuilder(args);

var tracerProvider = Sdk.CreateTracerProviderBuilder()
                        .AddConsoleExporter()
                        .AddSource(serviceName)
                        .SetResourceBuilder(
                            ResourceBuilder.CreateDefault()
                                .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
                        .AddHttpClientInstrumentation()
                        .AddAspNetCoreInstrumentation()
                        .AddJaegerExporter(o =>
                        {
                            o.AgentHost = Environment.GetEnvironmentVariable("JAEGER_HOST") ?? "127.0.0.1";
                            o.AgentPort = int.Parse(Environment.GetEnvironmentVariable("JAEGER_PORT") ?? "6831");
                        })
                        .Build();
Sdk.SetDefaultTextMapPropagator(new CompositeTextMapPropagator(new TextMapPropagator[]
{
    new TraceContextPropagator(),
    new BaggagePropagator(),
    new B3Propagator()
}));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
