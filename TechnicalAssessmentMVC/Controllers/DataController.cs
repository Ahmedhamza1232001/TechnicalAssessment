using Microsoft.AspNetCore.Mvc;
using TechnicalAssessmentMVC.Models;

namespace TechnicalAssessmentMVC.Controllers;
public class DataController : Controller
{
    private readonly ExcelService _excelService;
    private readonly IWebHostEnvironment _env;
    public DataController(ExcelService excelService, IWebHostEnvironment env)
    {
        _excelService = excelService;
        _env = env;
    }
    [HttpGet]
    public IActionResult Index(string fileName)
    {
        var filePath = Path.Combine(_env.WebRootPath, "uploads", fileName);
        List<Invoice> invoices = _excelService.ReadCsvFile(filePath);
        ViewBag.FileName = fileName;
        return View(invoices);
    }

    [HttpGet]
    public IActionResult Edit(string fileName)
    {
        var filePath = Path.Combine(_env.WebRootPath, "uploads", fileName);
        List<Invoice> invoices = _excelService.ReadCsvFile(filePath);
        ViewBag.FileName = fileName;
        return View(invoices);
    }

    [HttpPost]
    public IActionResult Edit(string fileName, List<Invoice> invoices)
    {
        var filePath = Path.Combine(_env.WebRootPath, "uploads", fileName);
        _excelService.SaveCsvFile(filePath, invoices);
        return RedirectToAction("Index", new { fileName = fileName });
    }
}
