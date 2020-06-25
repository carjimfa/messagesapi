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
    public class MessagesApiWebSocketService
    {
        private Dictionary<Guid, WebSocket> _usersSockets = new Dictionary<Guid, WebSocket>();
        private readonly IMessagesService _messagesService;
        private readonly IUsersService _usersService;

        public MessagesApiWebSocketService(IMessagesService messagesService,
            IUsersService usersService)
        {
            _messagesService = messagesService;
            _usersService = usersService;
        }

        public async Task AddUser(WebSocket socket)
        {
            try
            {
                var userConnectedId = Guid.NewGuid();

                _usersSockets.Add(userConnectedId, socket);
                SendSocketUserInfo(socket, userConnectedId);
                AnnounceNewUser(userConnectedId).Wait();
                SendMessages(socket).Wait();

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
                        //LOGIN AND MESSAGES HERE
                        // var changeRequest = SquareChangeRequest.FromJson(bufferAsString);
                        // await HandleSquareChangeRequest(changeRequest);
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
            await SendAll(JsonConvert.SerializeObject(socketMessageResponseDto));
        }

        private void SendSocketUserInfo(WebSocket socket, Guid userConnectedId)
        {
            var connectedUserResponse = new SocketMessageResponseDto<Guid>()
            {
                Content = userConnectedId,
                Type = MessageType.SocketUserInfo
            };

            var connectedUserInfoPayload = JsonConvert.SerializeObject(connectedUserResponse);
            Send(connectedUserInfoPayload, socket);
        }

        private async Task SendMessages(WebSocket socket)
        {
            var messages = (await _messagesService.GetAll()).ToList();
            if (messages.Any())
            {
                var response = new SocketMessageResponseDto<List<Message>>()
                {
                    Content = messages,
                    Type = MessageType.AllMessages
                }; 
                await Send(JsonConvert.SerializeObject(response), socket);
            }

        }

        private async Task AnnounceNewUser(Guid userId)
        {
            var message = new SocketMessageResponseDto<string>
            {
                Type = MessageType.UserConnected,
                Content = userId.ToString()
            };
            await SendAll(JsonConvert.SerializeObject(message));
        }
        
        private async Task SendAll(string message)
        {
            await Send(message, _usersSockets.Values.ToArray());
        }
        
        private async Task Send(string message, params WebSocket[] socketsToSendTo)
        {
            var sockets = socketsToSendTo.Where(s => s.State == WebSocketState.Open);
            foreach (var theSocket in sockets)
            {
                var stringAsBytes = System.Text.Encoding.ASCII.GetBytes(message);
                var byteArraySegment = new ArraySegment<byte>(stringAsBytes, 0, stringAsBytes.Length);
                await theSocket.SendAsync(byteArraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}