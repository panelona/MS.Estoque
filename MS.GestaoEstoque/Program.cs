using Microsoft.EntityFrameworkCore;
using MS.GestaoEstoque.Events;
using MS.GestaoEstoque.Interface;
using MS.GestaoEstoque.RabbitMqClient;
using MS.GestaoEstoque.Repository;
using MS.GestaoEstoque.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region build service e repository e Rabbit
builder.Services.AddScoped<IEstoqueService, EstoqueService>();
builder.Services.AddScoped<IEstoqueRepository, EstoqueRepository>();

builder.Services.AddSingleton<IProcessaEvento, ProcessaEvento>();
builder.Services.AddSingleton<IRabbitMqClient, RabbitMqClient>();
builder.Services.AddHostedService<RabbitMqSubscriber>();
#endregion

#region Context
builder.Services.AddDbContextFactory<TransientDbContextFactory>(options =>
{
    var connectionString = builder.Configuration["MS_ESTOQUE_CONNSTRING"];
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
#endregion

//#region CORS
//var corsDomains = builder.Configuration["MS_ESTOQUE_CORS_DOMAINS"];
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("CorsPolicy",
//        builder => builder
//            .WithOrigins(corsDomains)
//            .AllowAnyMethod()
//            .AllowAnyHeader());
//});
//#endregion

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseCors("CorsPolicy");
app.UseAuthorization();
app.MapControllers();
app.Run();