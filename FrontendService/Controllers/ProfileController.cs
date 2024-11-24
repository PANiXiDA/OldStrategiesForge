using FrontendService.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FrontendService.Controllers;

public class ProfileController : Controller
{
    public IActionResult Confirm(bool isExpired, string nickname)
    {
        var model = new ConfirmViewModel
        {
            IsExpired = isExpired,
            Nickname = nickname
        };

        return View(model);
    }

    public IActionResult Recovery(bool isExpired)
    {
        var model = new RecoveryViewModel { IsExpired = isExpired };

        return View(model);
    }
}
