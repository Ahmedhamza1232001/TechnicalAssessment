using CsvHelper;
using ExcelDataReader;
using System.Data;
using System.Globalization;
using System.Text;
using TechnicalAssessmentMVC.Models;

namespace TechnicalAssessmentMVC;

public class ExcelService
{
    public List<Invoice> ReadCsvFile(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<Invoice>().ToList();
        return records;
    }

    public void SaveCsvFile(string filePath, List<Invoice> invoices)
    {
        using var writer = new StreamWriter(filePath);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords(invoices);
    }

    public bool ValidCsvFileHeader(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Read();
        csv.ReadHeader();
        var headers = csv.HeaderRecord;

        var requiredHeaders = new List<string>
            {
                "Inv_No",
                "Inv_CURNo",
                "Inv_Date",
                "Customer Code",
                "Customer Name",
                "REG_COUNTRY_APREV",
                "Total Value After Taxing",
                "Taxing Value"
            };
        return requiredHeaders.All(header => headers.Contains(header));
    }


    public void ConvertXlsxToCsv(string xlsxFilePath, string csvFilePath)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        using var stream = File.Open(xlsxFilePath, FileMode.Open, FileAccess.Read);
        using var reader = ExcelReaderFactory.CreateReader(stream);
        var result = reader.AsDataSet(new ExcelDataSetConfiguration
        {
            ConfigureDataTable = _ => new ExcelDataTableConfiguration
            {
                UseHeaderRow = true
            }
        });

        var dataTable = result.Tables[0];


        using var writer = new StreamWriter(csvFilePath);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        // Write the header
        foreach (DataColumn column in dataTable.Columns)
        {
            csv.WriteField(column.ColumnName);
        }
        csv.NextRecord();

        // Write the data
        foreach (DataRow row in dataTable.Rows)
        {
            foreach (var item in row.ItemArray)
            {
                csv.WriteField(item);
            }
            csv.NextRecord();
        }
    }
}