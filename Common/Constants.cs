namespace Common;
public class Constants
{
    public static class ErrorMessages
    {
        public const string ErrorKey = "errorMessage";

        public const string NotFound = "Not found.";
        public const string NotValidModel = "Invalid model.";

        public const string PlayerCreateError = "Error while creating player";
        public const string PlayerNotFound = "Player not found.";
        public const string ExistsNicknane = "This nickname already exists.";
        public const string NotValidNickname = "Nickname must contain from 1 to 20 characters";

        public const string NotValidPassword = "The password must contain at least 6 characters, 1 uppercase letter, 1 lowercase letter, and 1 digit.";
        public const string PasswordMustMatch = "Passwords must match.";

        public const string NoEmailProvided = "Email is not provided.";
        public const string NotValidEmail = "Invalid email format.";
        public const string ExistsEmail = "The provided email is already in use.";
        public const string EmailNotConfirmed = "Email is not confirmed.";
    }
}
