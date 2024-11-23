namespace Common;
public class Constants
{
    public static class ErrorMessages
    {
        public const string ErrorKey = "errorMessage.";
        public const string Unavailable = "The service is unavailable";

        public const string NotFound = "Not found.";
        public const string NotValidModel = "Invalid model.";
        public const string NoActiveSessions = "No active sessions.";

        public const string Unauthorized = "Unauthorized.";
        public const string BadRefreshToken = "Refresh token not found.";

        public const string PlayerCreateError = "Error while creating player.";
        public const string ExistsNicknane = "This nickname already exists.";
        public const string NotValidNickname = "Nickname must contain from 1 to 20 characters.";

        public const string PlayerNotFound = "Player with this email not found.";
        public const string PlayerNotConfirm = "Account not confirm.";
        public const string PlayerBlocked = "Accout was blocked.";
        public const string PlayerPasswordIncorrect = "Password incorrect.";
        public const string PlayerAlreadyConfirmed = "Email already confirmed.";

        public const string NotValidPassword = "The password must contain at least 6 characters, 1 uppercase letter, 1 lowercase letter, and 1 digit.";
        public const string PasswordMustMatch = "Passwords must match.";

        public const string NoEmailProvided = "Email is not provided.";
        public const string NotValidEmail = "Invalid email format.";
        public const string ExistsEmail = "The provided email is already in use.";
        public const string EmailNotConfirmed = "Email is not confirmed.";

        public const string EmailServiceUnavailable = "Failed to send confirmation email.";
        public const string EmailTimeoutError = "Timeout while waiting for email confirmation.";
    }
}
