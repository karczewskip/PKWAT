using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PKWAT.ScoringPoker.Server.BackgroundServices;
using PKWAT.ScoringPoker.Server.Data;
using PKWAT.ScoringPoker.Server.Factories;
using PKWAT.ScoringPoker.Server.Hubs;
using PKWAT.ScoringPoker.Server.Stores;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddHostedService<TaskChangesNotifier>();

builder.Services.AddCors();

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtIssuer"],
            ValidAudience = builder.Configuration["JwtAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSecurityKey"]!))
        };

        options.Events = new JwtBearerEvents()
        {
            OnAuthenticationFailed = c =>
            {
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddSingleton<ILiveEstimationObserversInMemoryStore, LiveEstimationObserversInMemoryStore>();
builder.Services.AddTransient<ILiveEstimationScoringTaskStatusFactory, LiveEstimationScoringTaskStatusFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<LiveEstimationHub>("liveEstimation");

app.Run();
