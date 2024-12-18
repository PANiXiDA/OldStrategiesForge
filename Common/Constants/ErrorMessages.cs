namespace Common.Constants;

public static class ErrorMessages
{
    #region Global errors
    public const string ErrorKey = "ErrorMessage.";
    public const string Unavailable = "The service is unavailable.";
    public const string TooManyRequests = "Too many requests. Wait 15 min.";
    #endregion

    #region Validation errors
    public const string NotFound = "Not found.";
    public const string NotValidModel = "Invalid model.";
    #endregion

    #region Profile service errors

    #region Player errors

    #region Registration errors
    public const string NotValidPassword = "The password must contain at least 6 characters, 1 uppercase letter, 1 lowercase letter, and 1 digit.";
    public const string PasswordMustMatch = "Passwords must match.";

    public const string NoEmailProvided = "Email is not provided.";
    public const string NotValidEmail = "Invalid email format.";
    public const string ExistsEmail = "The provided email is already in use.";
    #endregion

    #region Auth errors
    public const string Unauthorized = "Unauthorized.";
    public const string BadRefreshToken = "Refresh token not found.";
    public const string NoActiveSessions = "No active sessions.";

    public const string PlayerNotFound = "Player with this email not found.";
    public const string PlayerNotConfirm = "Account not confirm.";
    public const string PlayerBlocked = "Accout was blocked.";
    public const string PlayerPasswordIncorrect = "Password incorrect.";
    #endregion

    #region Create player errors
    public const string PlayerCreateError = "Error while creating player.";
    public const string ExistsNicknane = "This nickname already exists.";
    public const string NotValidNickname = "Nickname must contain from 1 to 20 characters.";
    #endregion

    public const string PlayerAlreadyConfirmed = "Email already confirmed.";

    #endregion

    #region Avatars errors
    public const string AvatarNotFound = "Avatar not found";
    public const string AvatarNotAvailable = "In current time this avatar not available";
    public const string NotEnoughMmr = "To use this avatar, your Mmr must be:";
    public const string NotEnoughGames = "To use this avatar, your number of games must be greater:";
    public const string NotEnoughWins = "To use this avatar, your number of wins must be greater:";
    #endregion

    #endregion

    #region Email service errors
    public const string EmailServiceUnavailable = "Failed to send confirmation email.";
    public const string EmailTimeoutError = "Timeout while waiting for email confirmation.";
    #endregion
}
