using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using ServiceApp_backend.Classes;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;

namespace ServiceApp_backend.Models
{

    public class MessageHub : Hub
    {
        private readonly JwtAuth _jwtAuth;
        private readonly ConcurrentDictionary<string, string> _connectionIdToTokenMap;

        public MessageHub(JwtAuth jwtAuth, ConcurrentDictionary<string, string> connectionIdToTokenMap)
        {
            _jwtAuth = jwtAuth;
            _connectionIdToTokenMap = connectionIdToTokenMap;
        }

        public override async Task OnConnectedAsync()
        {
            var token = Context.GetHttpContext().Request.Query["token"].ToString();

            _connectionIdToTokenMap.TryAdd(Context.ConnectionId, token);

            await base.OnConnectedAsync();
        }
        public async Task Message(Message data)
        {
            try
            {
                var receiverId = data.Receiver;
                var message = data.MessageText;
                var token = data.TokenNo;

                Message msg = new Message
                {
                    MessageText = message,
                    Sender = _jwtAuth.ExtractUserInfo(_connectionIdToTokenMap[Context.ConnectionId]),
                    Receiver = receiverId
                };
                var json = JsonSerializer.Serialize(msg);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await httpClient.PostAsync("https://localhost:44348/api/Home/message", content);
                response.EnsureSuccessStatusCode();
                await Clients.Caller.SendAsync("messageSent", new { userId = msg.Sender, message = msg.MessageText });
                await Clients.Client(_connectionIdToTokenMap.FirstOrDefault(x => x.Value == token).Key).SendAsync("liveMessage", new { userId = msg.Sender, message = msg.MessageText });
            }
            catch (System.Exception)
            {

                throw;
            }

            // For example, you can send a message back to the client like this:
            // await Clients.All.SendAsync("messageReceived", new { userId = userId, username = username });
        }
    }
}


