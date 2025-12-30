using Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.AddControllers();


builder.Services.AddDbContext<UserContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("GamerHistory")));
builder.Services.AddScoped<UserContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
const string corsName = "MyOrigins"; 
 
builder.Services.AddCors(options => 
{ 
    options.AddPolicy(corsName, cpb => 
    {  

        cpb.AllowAnyOrigin() 
            .AllowAnyMethod() 
            .AllowAnyHeader(); 
    }); 
}); 



var app = builder.Build();
app.MapControllers();
app.UseCors(corsName); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
    
}

app.UseHttpsRedirection();

app.Run();  