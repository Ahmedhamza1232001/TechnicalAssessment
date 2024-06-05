using Microsoft.AspNetCore.Mvc;

namespace TechnicalAssessmentMVC.Controllers;

public class HomeController : Controller
{
    private readonly ExcelService _excelService;
    private readonly string _uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

    public HomeController(ExcelService excelService)
    {
        _excelService = excelService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            ViewBag.Message = "Please upload a valid file.";
            return View("Index");
        }

        var fileExtension = Path.GetExtension(file.FileName).ToLower();

        if (fileExtension == ".csv" || fileExtension == ".xlsx")
        {
            string filePath = Path.Combine(_uploadDirectory, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            if (fileExtension == ".xlsx")
            {
                string csvFilePath = Path.Combine(_uploadDirectory, Path.GetFileNameWithoutExtension(file.FileName) + ".csv");
                _excelService.ConvertXlsxToCsv(filePath, csvFilePath);
                ViewBag.FilePath = csvFilePath;
            }
            else
            {
                ViewBag.FilePath = filePath;
            }

            var invoices = _excelService.ReadCsvFile(ViewBag.FilePath);
            return View("Index", invoices);
        }
        else
        {
            ViewBag.Message = "Please upload a valid CSV or XLSX file.";
            return View("Index");
        }
    }

    [HttpPost]
    public IActionResult Save(string filePath)
    {
        try
        {
            var invoices = _excelService.ReadCsvFile(filePath);
            var fileName = Path.GetFileName(filePath);
            filePath = Path.Combine(_uploadDirectory, fileName);
            _excelService.SaveCsvFile(filePath, invoices);
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

