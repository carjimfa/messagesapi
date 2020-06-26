using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using MessagesApi.Domain;
using MessagesApi.Dtos;
using MessagesApi.Services.Interfaces;
using Newtonsoft.Json;

namespace MessagesApi.Services
{
    public class WebSocketService
    {
        private Dictionary<Guid, WebSocket> _usersSockets = new Dictionary<Guid, WebSocket>();
        private readonly IMessagesService _messagesService;

        public WebSocketService(IMessagesService messagesService)
        {
            _messagesService = messagesService;
        }

        public async Task AddUserSocket(WebSocket socket)
        {
            try
            {
                var userConnectedId = Guid.NewGuid();
                _usersSockets.Add(userConnectedId, socket);
                SendSocketUserInfo(socket, userConnectedId);
                AnnounceNewUser(userConnectedId).Wait();
                SendMessagesToSingleSocket(socket).Wait();
                while (socket.State == WebSocketState.Open)
                {
                    var buffer = new byte[1024 * 4];
                    WebSocketReceiveResult socketResponse;
                    var package = new List<byte>();
                    do
                    {
                        socketResponse =
                            await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        package.AddRange(new ArraySegment<byte>(buffer, 0, socketResponse.Count));
                    } while (!socketResponse.EndOfMessage);

                    var bufferAsString = System.Text.Encoding.ASCII.GetString(package.ToArray());

                    if (!string.IsNullOrEmpty(bufferAsString))
                    {
                        Console.WriteLine(bufferAsString);
                        await HandleNewMessage(bufferAsString);
                    }
                }

                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task HandleNewMessage(string bufferAsString)
        {
            var socketMessageRequestDto =
                JsonConvert.DeserializeObject<SocketMessageResponseDto<PostMessageRequestDto>>(
                    bufferAsString);
            var newMessage = await _messagesService.PostNewMessage(socketMessageRequestDto.Content);
            var socketMessageResponseDto = new SocketMessageResponseDto<Message>()
            {
                Content = newMessage,
                Type = MessageType.NewMessage
            };
            await SendToAllSockets(JsonConvert.SerializeObject(socketMessageResponseDto));
        }

        private void SendSocketUserInfo(WebSocket socket, Guid userConnectedId)
        {
            var connectedUserResponse = new SocketMessageResponseDto<Guid>()
            {
                Content = userConnectedId,
                Type = MessageType.SocketUserInfo
            };

            var connectedUserInfoPayload = JsonConvert.SerializeObject(connectedUserResponse);
            SendToSocket(connectedUserInfoPayload, socket);
        }

        private async Task SendMessagesToSingleSocket(WebSocket socket)
        {
            var messages = (await _messagesService.GetAll()).ToList();
            if (messages.Any())
            {
                var response = new SocketMessageResponseDto<List<Message>>()
                {
                    Content = messages,
                    Type = MessageType.AllMessages
                }; 
                await SendToSocket(JsonConvert.SerializeObject(response), socket);
            }

        }

        private async Task AnnounceNewUser(Guid userId)
        {
            var message = new SocketMessageResponseDto<string>
            {
                Type = MessageType.UserConnected,
                Content = userId.ToString()
            };
            await SendToAllSockets(JsonConvert.SerializeObject(message));
        }
        
        private async Task SendToAllSockets(string content)
        {
            await SendToSocket(content, _usersSockets.Values.ToArray());
        }
        
        private async Task SendToSocket(string content, params WebSocket[] socketsToSendTo)
        {
            var sockets = socketsToSendTo.Where(s => s.State == WebSocketState.Open);
            foreach (var theSocket in sockets)
            {
                var stringAsBytes = System.Text.Encoding.ASCII.GetBytes(content);
                var byteArraySegment = new ArraySegment<byte>(stringAsBytes, 0, stringAsBytes.Length);
                await theSocket.SendAsync(byteArraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}