using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Telegram Bot API", 
        Version = "v1",
        Description = "API для отправки сообщений в Telegram"
    });
});

// Регистрируем наш сервис
builder.Services.AddScoped<TelegramService>();

var app = builder.Build();

// Настройка Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Telegram Bot API v1"));
}

app.UseHttpsRedirection();

// Минимальный API эндпоинт
app.MapPost("/send", async (MessageRequest request, TelegramService service) => 
{
    var result = await service.SendMessageAsync(request.Text);
    return result ? Results.Ok("Сообщение отправлено!") : Results.Problem("Ошибка отправки");
})
.WithName("SendMessage")
.WithOpenApi();

app.MapGet("/debug", (IConfiguration config) => 
{
    var token = config["TelegramBotToken"];
    var chatId = config["ChatId"];
    return $"Token: {token}\nChatId: {chatId}";
});

app.Run();

// Модель запроса (добавить в конец файла)
public record MessageRequest(string Text);