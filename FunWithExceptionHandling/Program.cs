using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<OurExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.UseExceptionHandler(_ => { });


app.MapControllers();

app.MapGet("/api/fo", async (int id) =>
{
    throw new InvalidOperationException(message: "Bad things happen to good developer");
});
app.Run();



public class OurExceptionHandler : IExceptionHandler
{
    public  async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = 501;
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsync($"Exception thrown .... sorry! {exception.Message}");
        return true;
    }
}
