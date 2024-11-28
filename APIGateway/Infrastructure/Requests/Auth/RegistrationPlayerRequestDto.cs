using System.ComponentModel.DataAnnotations;
using Profile.Auth.Gen;
using Common;
using FluentValidation;
using Tools.Redis;
using APIGateway.Extensions.Exceptions;

namespace APIGateway.Infrastructure.Requests.Auth;

public class RegistrationPlayerRequestDto
{
    [Required(ErrorMessage = "Nickname is required.")]
    public string Nickname { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Repeat password is required.")]
    public string RepeatPassword { get; set; } = string.Empty;

    public RegistrationPlayerRequest RegistrationPlayerRequestDtoToProto()
    {
        return new RegistrationPlayerRequest()
        {
            Nickname = Nickname,
            Email = Email,
            Password = Password
        };
    }
}

public class CreatePlayerDtoValidator : AbstractValidator<RegistrationPlayerRequestDto>
{
    private readonly IRedisCache _redisCache;

    public CreatePlayerDtoValidator(IRedisCache redisCache)
    {
        _redisCache = redisCache;

        RuleFor(player => player.Nickname)
            .Length(1, 20).WithMessage(Constants.ErrorMessages.NotValidNickname)
            .MustAsync(IsNicknameUniqueAsync).WithMessage(Constants.ErrorMessages.ExistsNicknane);

        RuleFor(player => player.Email)
            .EmailAddress().WithMessage(Constants.ErrorMessages.NotValidEmail)
            .MustAsync(IsEmailUniqueAsync).WithMessage(Constants.ErrorMessages.ExistsEmail);

        RuleFor(player => player.Password)
            .Must(Helpers.IsPasswordValid).WithMessage(Constants.ErrorMessages.NotValidPassword);

        RuleFor(player => player.RepeatPassword)
            .Equal(player => player.Password).WithMessage(Constants.ErrorMessages.PasswordMustMatch);
    }

    private async Task<bool> IsNicknameUniqueAsync(string nickname, CancellationToken cancellationToken)
    {
        if (await _redisCache.ExistsAsync($"nickname:{nickname}"))
        {
            throw new ValidationConflictException(Constants.ErrorMessages.ExistsNicknane, "Nickname");
        }

        return true;
    }

    private async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken)
    {
        if (await _redisCache.ExistsAsync($"email:{email}"))
        {
            throw new ValidationConflictException(Constants.ErrorMessages.ExistsEmail, "Email");
        }

        return true;
    }
}

