<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Chat.aspx.cs" Inherits="Glimpse.SignalR.Sample.Chat" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN">

<html xmlns="http://www.w3.org/1999/xhtml">
    
    <head>
        
        <title>Glimpse.SignalR.Sample</title>

    </head>

    <body>

        <script src="Scripts/jquery-1.8.3.min.js" type="text/javascript"></script>
        <script src="Scripts/jquery.signalR-1.0.0-rc1.min.js" type="text/javascript"></script>
        <script src="/signalr/hubs" type="text/javascript"></script>
        <script type="text/javascript">
            $(function () {
                // Proxy created on the fly
                var chat = $.connection.chat;

                // Declare a function on the chat hub so the server can invoke it
                chat.client.addMessage = function (message) {
                    $('#messages').append('<li>' + message + '</li>');
                };

                $("#broadcast").click(function () {
                    // Call the chat method on the server
                    chat.server.send($('#msg').val());
                });

                // Start the connection
                $.connection.hub
                    .start()
                    .pipe(function() {
                         return chat.server.getMessages();
                    })
                    .done(function (messages) {
                        $.each(messages, function(index, message) {
                            $('#messages').append('<li>' + message + '</li>');
                        });
                    });
            });
        </script>
        
        <input id="msg" type="text" />
        <input id="broadcast" type="button" value="broadcast" />

        <ul id="messages"></ul>

    </body>

</html>