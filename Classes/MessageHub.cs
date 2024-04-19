using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using ServiceApp_backend.Models;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;

namespace ServiceApp_backend.Classes
{

    public class MessageHub : Hub
    {
        private readonly JwtAuth _jwtAuth;
        private readonly ConcurrentDictionary<string, string> _connectionIdToGuidMap;

        public MessageHub(JwtAuth jwtAuth, ConcurrentDictionary<string, string> connectionIdToGuidMap)
        {
            _jwtAuth = jwtAuth;
            _connectionIdToGuidMap = connectionIdToGuidMap;
        }

        public Task MapConnectionIdToGuid(Message data)
        {
            try
            {
                var senderId = data.Sender;
                _connectionIdToGuidMap.TryAdd(Context.ConnectionId, senderId);
                return Task.CompletedTask;
            }
            catch (System.Exception)
            {

                throw new Exception("Error in mapping connection id to guid");
            }

        }
        public async Task Message(Message data)
        {
            try
            {
                var receiverId = data.Receiver;
                var senderId = data.Sender;
                var message = data.MessageText;
                var token = data.TokenNo;

                Message msg = new Message
                {
                    MessageText = message,
                    Sender = senderId,
                    Receiver = receiverId
                };
                var json = JsonSerializer.Serialize(msg);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await httpClient.PostAsync("https://localhost:44348/api/Home/message", content);
                response.EnsureSuccessStatusCode();
                await Clients.Caller.SendAsync("messageSent", new { userId = msg.Sender, message = msg.MessageText });
                if (_connectionIdToGuidMap.Any(x => x.Value == receiverId))
                {
                    var connectionId = _connectionIdToGuidMap.FirstOrDefault(x => x.Value == receiverId).Key;
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


