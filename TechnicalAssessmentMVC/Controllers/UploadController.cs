using Microsoft.AspNetCore.Mvc;
using TechnicalAssessmentMVC.Services;

namespace TechnicalAssessmentMVC.Controllers;
public class UploadController : Controller
{
    private readonly UploadService _uploadService;

    public UploadController(UploadService uploadService)
    {
        _uploadService = uploadService;
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (!_uploadService.IsValidFile(file))
        {
            ViewBag.Message = "Please upload a valid file.";
            return View("Index");
        }

        var filePath = await _uploadService.SaveFileAsync(file);
        var invoices = _uploadService.ReadCsvFile(filePath);
        ViewBag.FilePath = filePath;

        return View("Index", invoices);
    }

    [HttpPost]
    public IActionResult Save(string filePath)
    {
        try
        {
            var invoices = _uploadService.ReadCsvFile(filePath);
            var fileName = Path.GetFileName(filePath);
            filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);
            _uploadService.SaveCsvFile(filePath, invoices);
            ViewBag.Message = "File saved successfully!";
            return View("Index", invoices);
        }
        catch (Exception ex)
        {
            ViewBag.Message = $"Error saving file: {ex.Message}";
            return View("Index");
        }
    }
}