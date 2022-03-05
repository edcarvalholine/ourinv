using Microsoft.EntityFrameworkCore;
using ourinv.WebAPI.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(op => op.UseInMemoryDatabase("teste"));
var app = builder.Build();

#region In Memory Database
using (var serviceScope = app.Services.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
    context.Categories.AddRange(
        new() { Name = "Unknown" },
        new() { Name = "Vegetais" },
        new() { Name = "Carnes" }
    );
    context.SaveChanges();
    context.Products.AddRange(
        new() { Category = context.Categories.Find(2), Id = 1, Name = "Bróculos", Quantity = 10 },
        new() { Category = context.Categories.Find(2), Id = 2, Name = "Batatas", Quantity = 3 },
        new() { Category = context.Categories.Find(3), Id = 3, Name = "Vaca", Quantity = 5 },
        new() { Category = context.Categories.Find(1), Id = 4, Name = "Porco", Quantity = 3 }
    );
    context.SaveChanges();

}
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
