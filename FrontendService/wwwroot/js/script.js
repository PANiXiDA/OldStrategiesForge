$(document).ready(function () {
    $('#newsletterForm').submit(function (event) {
        event.preventDefault();

        var email = $('#email').val();

        $.ajax({
            type: 'POST',
            url: '/Email/SubscribeToNotifications',
            data: { email: email },
            success: function (response) {
                if (response.ok) {
                    Toastify({
                        text: 'You have successfully subscribed.',
                        duration: 3000,
                        style: {
                            background: "green",
                            color: "white"
                        }
                    }).showToast();
                } else {
                    Toastify({
                        text: 'We are sorry, but the service is currently unavailable.',
                        duration: 3000
                    }).showToast();
                }
            },
            error: function () {
                Toastify({
                    text: 'We are sorry, but the service is currently unavailable.',
                    duration: 3000
                }).showToast();
            }
        });
    });
});