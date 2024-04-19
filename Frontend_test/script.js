
const form = $("#loginForm");

form.submit(function (event) {
    event.preventDefault();

    var data = {
        UserEmail: $("#Email").val(),
        Password: $("#Password").val()
    }
    $.ajax({
        url: "https://localhost:44348/api/login/LoginValidation",
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            if (response.status === 404) {
                alert(response.message);
                return;
            }
            sessionStorage.setItem("token", response.data.tokenNo);
            location.href = "./index.html";
            const signalr = new signalr.HubConnectionBuilder()
                .withUrl("https://localhost:44348/messagehub?token=" + encodeURIComponent(response.data.tokenNo))
                .build();
            signalr.start().then(function () {
                console.log("Connection started");
            }).catch(function (err) {
                return console.error(err.toString());
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error(jqXHR.responseText);
        }
    });
});
$(document).ready(function () {
    const token = sessionStorage.getItem("token");

    if (token) {

        const connection = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:44348/messagehub?token=" + encodeURIComponent(token))
            .build();
        connection.start().then(function () {
            console.log("Connection started");
        }).catch(function (err) {
            return console.error(err.toString());
        });
        const navbar = $("#narbarBtn");
        const logoutButton = $(
            `<button class='btn btn-danger'  id='logoutButton'>Logout</button>`
        );

        navbar.html(logoutButton);

        $("#logoutButton").click(function () {
            sessionStorage.removeItem("token");
            connection.stop();
            window.location.href = "./login.html";
        });

        connection.on("liveMessage", (data) => {
            $(".chat-history ul").append(`<li class="clearfix">
            <div class="message other-message">
              ${data.message}
            </div>
          </li>`);
        });

        $(document).on("click", "#sendMessageBtn", function () {
            const message = $("#messageInput").val();
            connection.invoke("Message", { Receiver: parseInt($(this).data("userid")), MessageText: message, TokenNo: token }).catch(function (err) {
                return console.error(err.toString());
            });

        });
        connection.on("messageSent", (data) => {
            $(".chat-history ul").append(`<li class="clearfix">
            <div class="message my-message float-right">
              ${data.message}
            </div>
          </li>`);
            $("#messageInput").val("")
        });
        connection.onclose((error) => {
            console.log(`Connection closed due to error: ${error}`);
            // You can add code here to handle the disconnection, like trying to reconnect
        });
    }
});

