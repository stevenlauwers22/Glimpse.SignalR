<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Chat.aspx.cs" Inherits="Glimpse.SignalR.Sample.Chat" %>

<!DOCTYPE html>

<html>

<head>

    <title>Glimpse.SignalR.Sample</title>
    <style type="text/css">
        .container {
            background-color: #99CCFF;
            border: thick solid #808080;
            padding: 20px;
            margin: 20px;
        }
    </style>

</head>

<body>

    <script src="Scripts/jquery-2.1.1.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.signalR-2.1.1.min.js" type="text/javascript"></script>
    <script src="<%: ResolveClientUrl("~/signalr/hubs") %>" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            // Proxy created on the fly
            var chat = $.connection.chat;

            // Declare a function on the chat hub so the server can invoke it
            chat.client.addMessage = function (name, message) {
                var encodedName = $('<div />').text(name).html();
                var encodedMsg = $('<div />').text(message).html();
                // Add the message to the page. 
                $('#discussion').append('<li><strong>' + encodedName + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>');
            };

            // Get the user name and store it to prepend to messages.
            $('#displayname').val(prompt('Enter your name:', ''));
            // Set initial focus to message input box.  
            $('#message').focus();

            $("#sendmessage").click(function () {
                // Call the chat method on the server
                chat.server.send($('#displayname').val(), $('#message').val());
                $('#message').val('').focus();
            });

            // Start the connection
            $.connection.hub
                .start()
                .pipe(function () {
                    return chat.server.getMessages();
                })
                .done(function (messages) {
                    var messages1 = $.parseJSON(messages);
                    $.each(messages1, function (index, message) {
                        var name = message.split(':')[0],
                            text = message.split(':')[1];
                        $('#discussion').append('<li><strong>' + name + '</strong>:&nbsp;&nbsp;' + text + '</li>');
                    });
                });
        });
    </script>

    <div class="container">
        <input type="text" id="message" />
        <input type="button" id="sendmessage" value="Send" />
        <input type="hidden" id="displayname" />
        <ul id="discussion">
        </ul>
    </div>
</body>

</html>
