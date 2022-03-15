using API.Middleware;
using Aplication.Activities;
using Aplication.Core;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddFluentValidation(config=>
{
    config.RegisterValidatorsFromAssemblyContaining<Create>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors(opt=>
opt.AddPolicy("CorsPolicy",policy =>
{
    policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:3000");
})
);

builder.Services.AddMediatR(typeof(List.Handler).Assembly);

builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    // use context
    await Seed.SeedData(context);
}

app.UseMiddleware<ExceptionMiddleware>(); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
