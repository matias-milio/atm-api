using ATM.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddServiceConfigs(builder);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

app.MapControllers();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();
app.Run();


