using Microsoft.AspNetCore.Mvc;
using TechnicalAssessmentMVC.Models;

namespace TechnicalAssessmentMVC.Controllers;
public class InvoicesController : Controller
{

    private readonly string _uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

    [HttpGet]
    public IActionResult ListCsvFiles()
    {
        try
        {
            var csvFiles = Directory.GetFiles(_uploadDirectory, "*.csv").Select(Path.GetFileName).ToList();
            return View(csvFiles);
        }
        catch (Exception ex)
        {
            ViewBag.Message = $"Error listing CSV files: {ex.Message}";
            return View();
        }
    }

    public IActionResult Edit(int index, List<Invoice> invoices)
    {
        var invoice = invoices[index];
        return View("EditInvoice", invoice);
    }

    [HttpPost]
    public IActionResult Delete(string fileName)
    {
        try
        {
            var filePath = Path.Combine(_uploadDirectory, fileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                return Json(new { success = true, message = "File deleted successfully!" });
            }
            else
            {
                return Json(new { success = false, message = "File not found!" });
            }
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"Error deleting file: {ex.Message}" });
        }
    }


}
