using ATM.Api.Configurations;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceConfigs(builder);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.Run();


