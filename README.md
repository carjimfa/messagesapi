# MessagesAPI

This project is built using Net Core 3.1, make sure you have the Net Core 3.1 runtime installed to execute it.

## Cloning and running it

Open a Terminal in your computer.

Clone this repository `git clone https://github.com/carjimfa/messagesapi.git`.

Access the downloaded folder `cd messagesapi`.

Run `dotnet restore` to restore all packages.

Navigate to the main project foler `cd MessagesApi/` and execute `dotnet run` to run the API.

Open a browser window and navigate to `https://localhost:5001`.

## Swagger

This project has Swagger installed to explore the main endpoints exposed.

Navigate to `https://localhost:5001/swagger/index.html` to see the API documentation.

## WebSocket

The Swagger documentation will only provide documentation for the login process but not for sending and receiving messages, communication made by SignalR Websocket exposed in `/ws`. The MessagesApp that uses this API establishes a connection with this websocket to interchange messages.

## Workflow

### 1 - Connect to Websocket
This will provide a GUID with the socket Id where the client has connected but this Guid is not valid to interchange messages. User need to be logged in to send messages. The user will be able to see all messages with no restriction, like a public feed but won't be able to participate until he logs in.
### 2 - Login with Username
Login is made through API. `POST /api/Users/login` will provide a User with Id, Username and older messages.
### 3 - Send Message
Sending a message is made through WebSocket running in `/ws` where you can send and receive messages. You can send a Mesage to that webSocket and all users connected to the API (all sockets registered) will receive it. Messages sent will have the UserId provided by login to identify user's messages.

## Data Structure

For testing only purposes, this approach uses a Singleton Pattern with all messages and users in memory using singleton services. In the future it would be great to use a DbContext injection to save messages and retrieve them after the app shuts down. In this approach it is not included.

In this future scenario where we persist users and messages, WebSocket would retrieve the latest 100 messages and would only maintain those 100 messages. 

When someone is loading more and more messages (older messages), client app should call an API endpoint to retrieve older messages instead of communicate through the WebSocket that is intended for using only live information.

## Contact

Feel free to send a message to carjimfa@gmail.com for further details, questions and more.