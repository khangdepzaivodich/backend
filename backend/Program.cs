var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<backend.Services.ChatService.ChatMongoService>();

builder.Services.AddSingleton<backend.Services.ChatService.ChatRedisService>();

builder.Services.AddControllers();

// [TÍNH NĂNG MỚI] KÍCH HOẠT SignalR WebSockets! ---
builder.Services.AddSignalR().AddStackExchangeRedis(builder.Configuration.GetConnectionString("Redis")!);
// -------------------------------------------------

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// [SignalR] Cắm cái đuôi cho trạm phát sóng (Đây là link mà Blazor Frontend sẽ gõ vào để connect WebSocket)
app.MapHub<backend.Services.ChatService.ChatHub>("/chat-hub");

app.Run();
