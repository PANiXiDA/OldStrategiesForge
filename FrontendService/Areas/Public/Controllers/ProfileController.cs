using Microsoft.AspNetCore.Mvc;

namespace FrontendService.Areas.Public.Controllers;

[Area("Public")]
public class ProfileController : Controller
{
    public IActionResult Confirm(string nickname)
    {
        return View((object)nickname);
    }
}
