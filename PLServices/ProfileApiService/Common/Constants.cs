namespace ProfileApiService.Common;

public class Constants
{
    public static class ErrorMessages
    {
        public const string ErrorKey = "errorMessage";

        public const string NotFound = "Не найдено.";
        public const string NotValidModel = "Не валидная модель.";

        public const string PlayerCreateError = "Ошибка при создании пользователя";
        public const string PlayerNotFound = "Пользователь не найден.";
        public const string ExistsLogin = "Такой логин уже существует.";

        public const string NotValidPassword = "Пароль должен содержать не менее 6 символов, 1 прописную, 1 строчную буквы и 1 цифру";
        public const string PasswordMustMatch = "Пароли должны совпадать.";

        public const string NoEmailProvided = "Не указана почта.";
        public const string NotValidEmail = "Неверно введена почта.";
        public const string ExistsEmail = "Указанная почта уже используется.";
        public const string EmailNotConfirmed = "Почта не подтверждена.";
    }
}
