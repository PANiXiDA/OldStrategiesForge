using System.ComponentModel.DataAnnotations;
using Profile.Auth.Gen;
using Common;
using FluentValidation;

namespace APIGateway.Infrastructure.Requests.Auth;

public class RegistrationPlayerRequestDto
{
    [Required]
    public string Nickname { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
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
    public CreatePlayerDtoValidator()
    {
        RuleFor(player => player.Nickname)
            .Length(1, 20).WithMessage(Constants.ErrorMessages.NotValidNickname);

        RuleFor(player => player.Password)
            .Must(Helpers.IsPasswordValid).WithMessage(Constants.ErrorMessages.NotValidPassword);

        RuleFor(player => player.RepeatPassword)
            .Equal(player => player.Password).WithMessage(Constants.ErrorMessages.PasswordMustMatch);
    }
}
