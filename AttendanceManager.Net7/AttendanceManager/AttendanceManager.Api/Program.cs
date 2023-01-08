using AttendanceManager.Api.Middleware;
using AttendanceManager.Application.Models.Authentication;
using AttendanceManager.Application.Models.Mail;
using AttendanceManager.Application.Modules.Seed;
using AttendanceManager.Infrastructure;
using AttendanceManager.Persistance;
using MediatR;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// setup serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Host.UseSerilog(Log.Logger);

// Settup EF core
builder.Services.Configure<AdminSeedSetting>(builder.Configuration.GetSection("Admin"));
builder.Services.AddPersistanceServices(builder.Configuration);

// Settup automapper and mediator
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

// Settup contracts
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailKit"));
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddControllers();

//add this to offer access to other apps to access the API without any restrictions
builder.Services.AddCors(o => o.AddPolicy("CORSPolicy", b =>
{
    b.WithOrigins(builder.Configuration.GetValue<string>("CORSOrigin")!)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials();
}));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Seed();

var app = builder.Build();

//add this to can use APIs in any application
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomExceptionHandler();

app.UseHttpsRedirection();

//second step in setting up the JWT authentication: this should always be placed above UseMvc
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
