using CsvHelper.Configuration.Attributes;

namespace TechnicalAssessmentMVC.Models;

public class Invoice
{

    [Name("Inv_No")]
    public int InvNo { get; set; }

    [Name("Inv_CURNo")]
    public string InvCURNo { get; set; }

    [Name("Inv_Date")]
    public string InvDate { get; set; }

    [Name("Customer Code")]
    public string CustomerCode { get; set; }

    [Name("Customer Name")]
    public string CustomerName { get; set; }

    [Name("REG_COUNTRY_APREV")]
    public string RegCountryAPrev { get; set; }

    [Name("Total Value After Taxing")]
    public decimal TotalValueAfterTaxing { get; set; }

    [Name("Taxing Value")]
    public decimal TaxingValue { get; set; }

    public decimal TotalValueBeforeTaxing => TotalValueAfterTaxing - TaxingValue;
}
