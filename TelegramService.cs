using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

public class TelegramService
{
    private readonly HttpClient _httpClient;
    private readonly string _botToken;
    private readonly string _chatId;

    public TelegramService(IConfiguration config)
    {
        _httpClient = new HttpClient();
        _botToken = config["TelegramBotToken"]!;
        _chatId = config["ChatId"]!;
    }

    public async Task<bool> SendMessageAsync(string text)
    {
        try
        {
            var url = $"https://api.telegram.org/bot{_botToken}/sendMessage";
            var response = await _httpClient.PostAsJsonAsync(url, new { chat_id = _chatId, text });
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}