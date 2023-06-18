using GrpcMessagingService.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
// ��������� ��������� gRPC ��������:
builder.Services
    .AddGrpc()
    .AddJsonTranscoding();

// ��������� ��������� gRPC ��� ��������������� ����������� ���������� ����� (gRPCurl ��� Postman):
builder.Services.AddGrpcReflection();

// ������� Swagger ��� gRPC:
builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "gRPC transcoding",
            Version = "v1"
        });
});

// ������� ����������� � �������:
builder.Services.AddLogging(logging => logging.AddConsole());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // ���������� �������� ����� ������ ���������:
    app.MapGrpcReflectionService();
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "gRPC API v1"));

// Configure the HTTP request pipeline.
// ���������� ��� gRPC ������  � ������� ������������� ��� ��������� ��������:
app.MapGrpcService<MessagingService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
