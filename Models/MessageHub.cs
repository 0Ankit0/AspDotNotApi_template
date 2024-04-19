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
            try
            {
                var token = Context.GetHttpContext().Request.Query["token"].ToString();
                int userId = _jwtAuth.ExtractUserInfo(token);
                _connectionIdToTokenMap.TryAdd(Context.ConnectionId, userId.ToString());
                await base.OnConnectedAsync();
            }
            catch (System.Exception)
            {

                throw;
            }

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
                string receiver = receiverId.ToString();
                if (_connectionIdToTokenMap.Any(x => x.Value == receiver))
                {
                    var connectionId = _connectionIdToTokenMap.FirstOrDefault(x => x.Value == receiver).Key;
                    await Clients.Client(connectionId).SendAsync("liveMessage", new { userId = msg.Sender, message = msg.MessageText });
                }
            }
            catch (System.Exception)
            {

                throw;
            }


        }
    }
}


