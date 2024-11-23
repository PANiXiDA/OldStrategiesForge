using Microsoft.AspNetCore.Mvc;

namespace FrontendService.Controllers;

public class ProfileController : Controller
{
    public IActionResult Confirm(string nickname)
    {
        return View((object)nickname);
    }

    public IActionResult Recovery()
    {
        return View();
    }
}
