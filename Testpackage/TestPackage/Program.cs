using Dpk.Observability;
using TestPackage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCompanyObservability();
builder.Services.AddScoped<PaymentMetrics>();
var app = builder.Build();
app.UseCompanyObservability();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGet("/pay", (PaymentMetrics metrics) =>
{
    metrics.PaymentRejected();
    return Results.Ok("payment success");
});
app.Run();


