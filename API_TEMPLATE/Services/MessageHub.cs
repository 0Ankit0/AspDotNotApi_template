using API_TEMPLATE.CustomClass;
using API_TEMPLATE.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace API_TEMPLATE.Services
{
    public class MessageHub : Hub
    {
        private readonly JwtAuth _jwtAuth;
        private readonly AssymetricCryptography _cryptography;
        private readonly ConcurrentDictionary<string, MapToHubId> _connectionIdToGuidMap;

        public MessageHub(JwtAuth jwtAuth, ConcurrentDictionary<string, MapToHubId> connectionIdToGuidMap, AssymetricCryptography cryptography)
        {
            _jwtAuth = jwtAuth;
            _connectionIdToGuidMap = connectionIdToGuidMap;
            _cryptography = cryptography;
        }

        public async Task MapConnectionIdToGuid(MessageModel data)
        {
            try
            {
                var senderId = data.Sender;
                (string publicKey, string privateKey) = _cryptography.GenerateKeys();
                MapToHubId mapToHubId = new MapToHubId
                {
                    Publickey = publicKey,
                    Privatekey = privateKey,
                    UserId = senderId
                };
                _connectionIdToGuidMap.TryAdd(Context.ConnectionId, mapToHubId);
                await Clients.Caller.SendAsync("MappedSuccessfully", new { key = publicKey });
                return;
            }
            catch (System.Exception)
            {
                throw new Exception("Error in mapping connection id to guid");
            }
        }

        public async Task Message(MessageModel data)
        {
            try
            {
                var receiverId = data.Receiver;
                var senderId = data.Sender;
                var message = data.MessageText;
                var token = data.TokenNo;

                if (!_connectionIdToGuidMap.TryGetValue(Context.ConnectionId, out var senderMap))
                {
                    await Clients.Caller.SendAsync("errorMessage", new { error = "Sender not mapped." });
                    return;
                }

                if (!_connectionIdToGuidMap.Any(x => x.Value.UserId == receiverId))
                {
                    await Clients.Caller.SendAsync("errorMessage", new { error = "Receiver not mapped." });
                    return;
                }

                var receiverMap = _connectionIdToGuidMap.FirstOrDefault(x => x.Value.UserId == receiverId).Value;

                // Encrypt the message using the receiver's public key
                var encryptedMessage = _cryptography.Encrypt(message, receiverMap.Publickey);

                MessageModel msg = new MessageModel
                {
                    MessageText = encryptedMessage,
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

                if (_connectionIdToGuidMap.Any(x => x.Value.UserId == receiverId))
                {
                    var connectionId = _connectionIdToGuidMap.FirstOrDefault(x => x.Value.UserId == receiverId).Key;
                    await Clients.Client(connectionId).SendAsync("liveMessage", new { userId = msg.Sender, message = msg.MessageText });
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _connectionIdToGuidMap.TryRemove(Context.ConnectionId, out _);
            return base.OnDisconnectedAsync(exception);
        }

        // Helper method to check if a string is a valid Base64 string
        private bool IsBase64String(string base64)
        {
            if (string.IsNullOrEmpty(base64) || base64.Length % 4 != 0)
                return false;

            try
            {
                Convert.FromBase64String(base64);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
