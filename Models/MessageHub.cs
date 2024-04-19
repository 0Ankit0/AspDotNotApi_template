using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using ServiceApp_backend.Classes;

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
            var token = Context.GetHttpContext().Request.Query["access_token"].ToString();

            _connectionIdToTokenMap.TryAdd(Context.ConnectionId, token);

            await base.OnConnectedAsync();
        }
        public async Task Message(dynamic data)
        {
            var token = data.receiverId.ToString();
            var message = data.message.ToString();

            int receiverId = _jwtAuth.ExtractUserInfo(token);

            // For example, you can send a message back to the client like this:
            // await Clients.All.SendAsync("messageReceived", new { userId = userId, username = username });
        }
    }
}


