using Microsoft.AspNetCore.Mvc;

namespace TechnicalAssessmentMVC.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }


}

