using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using ServiceApp_backend.Classes;
using System.Net.Http;
using System.Text;
using System.Text.Json;

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
        public async Task Message(dynamic data)
        {
            try
            {
                var token = data.receiverId.ToString();
                var message = data.message.ToString();

                int receiverId = _jwtAuth.ExtractUserInfo(token);
                Message msg = new Message
                {
                    MessageText = message,
                    Sender = _jwtAuth.ExtractUserInfo(_connectionIdToTokenMap[Context.ConnectionId]),
                    Receiver = receiverId,
                    TokenNo = token
                };
                var json = JsonSerializer.Serialize(msg);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using var httpClient = new HttpClient();
                var response = await httpClient.PostAsync("https://localhost:44348/api/home/insertmessage", content);
                response.EnsureSuccessStatusCode();

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


