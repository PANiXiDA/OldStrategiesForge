namespace Common.Constants;

public static class TimeoutSettings
{
    public const int ConfirmationEmailTokenLifeTimeMin = 15;
    public const int RecoveryPasswordTokenLifeTimeMin = 15;

    public const int RabbitMqTimeoutSec = 30;

    public const int RedisEmailLifeTimeDays = 30;
    public const int RedisNicknameLifeTimeDays = 30;
}
