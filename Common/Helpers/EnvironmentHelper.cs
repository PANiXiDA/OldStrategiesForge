namespace Common.Helpers;

public static class EnvironmentHelper
{
    public static bool IsDevelopment =>
        Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
}

