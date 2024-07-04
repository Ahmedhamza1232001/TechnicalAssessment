using TechnicalAssessmentMVC.Models;

namespace TechnicalAssessmentMVC.Services;

public class UploadService
{
    private readonly string _uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
    private readonly ExcelService _excelService;

    public UploadService(ExcelService excelService)
    {
        _excelService = excelService;
    }

    public bool IsValidFile(IFormFile file)
    {
        return file != null && file.Length > 0;
    }

    public async Task<string> SaveFileAsync(IFormFile file)
    {
        var fileExtension = Path.GetExtension(file.FileName).ToLower();
        string filePath = Path.Combine(_uploadDirectory, file.FileName);

        if (fileExtension == ".xlsx")
        {
            string csvFilePath = Path.Combine(_uploadDirectory, Path.GetFileNameWithoutExtension(file.FileName) + ".csv");
            await ConvertAndSaveXlsxFileAsync(file, csvFilePath);
            return csvFilePath;
        }
        else
        {
            await SaveFileToPathAsync(file, filePath);
            return filePath;
        }
    }

    private async Task SaveFileToPathAsync(IFormFile file, string filePath)
    {
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
    }

    private async Task ConvertAndSaveXlsxFileAsync(IFormFile file, string csvFilePath)
    {
        string tempXlsxPath = Path.Combine(Path.GetTempPath(), file.FileName);
        await SaveFileToPathAsync(file, tempXlsxPath);
        _excelService.ConvertXlsxToCsv(tempXlsxPath, csvFilePath);
        File.Delete(tempXlsxPath); // Delete the temporary XLSX file
    }

    public List<Invoice> ReadCsvFile(string filePath)
    {
        return _excelService.ReadCsvFile(filePath);
    }

    public void SaveCsvFile(string filePath, List<Invoice> invoices)
    {
        _excelService.SaveCsvFile(filePath, invoices);
    }
}

