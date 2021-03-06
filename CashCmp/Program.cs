
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json.Linq;
using System.Globalization;

var cash = ReadCash();
var bill = ReadBill();
var ez = ReadEz();

foreach (var item in cash)
{
    JObject o = JObject.Parse(item.Data);

    var type = o["tr"].ToString();

    if (type.Contains("paybill") || type.Contains("datapay"))
    {
        string accNo = o["con"][0]["n"].Value<string>();

        if(accNo.StartsWith('0'))
        {
            accNo = accNo.Substring(1);
        }

        var qu = bill.Where(t => t.Account == accNo)
                     .Select(t => t.Account)
                     .FirstOrDefault();

        if (qu == null)
        {
            Console.WriteLine("0b- " + item.Date + "  " + o.ToString());
            Console.WriteLine("--------------------------------------");
        }
    }
    else if (type.Contains("ez"))
    {
        var accNo = o["con"].Value<string>();

        if (accNo.StartsWith('0'))
        {
            accNo = accNo.Substring(1);
        }

        var qu = ez.Where(t => t.Account == accNo)
                   .Select(t => t.Account)
                   .FirstOrDefault();
        if (qu == null)
        { 
            Console.WriteLine("0e- " + item.Date + "  " + o.ToString());
            Console.WriteLine("--------------------------------------");
        }

    }
    else
    {
        Console.WriteLine("0e- " + item.Date + "  " + o.ToString());
        Console.WriteLine("--------------------------------------");
    }
}

Console.WriteLine("P");
Console.Read();

static List<Tr> ReadBill() 
{
    using (var reader = new StreamReader(@"D:\c3\dialogkiosk6\test\CashCmpF\bill.csv"))
    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
    {
        var records = csv.GetRecords<Tr>().ToList();
        return records;
    }
}

static List<Ez> ReadEz()
{
    using (var reader = new StreamReader(@"D:\c3\dialogkiosk6\test\CashCmpF\ez.csv"))
    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
    {
        var records = csv.GetRecords<Ez>().ToList();
        return records;
    }
}

static List<Cash> ReadCash()
{
    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        Delimiter = "|",
        BadDataFound = null
    };

    using (var reader = new StreamReader(@"D:\c3\dialogkiosk6\test\CashCmpF\cash.csv"))
    using (var csv = new CsvReader(reader, config))
    {
        var records = csv.GetRecords<Cash>().ToList();
        return records;
    }
}


public class Tr
{
    public string TrID { get; set; }
    public string Date { get; set; }
    public string Account { get; set; }
    public string LOB { get; set; }
    public string Amount { get; set; }

}

public class Ez
{
    public string Date { get; set; }
    public string Account { get; set; }
    public string Amount { get; set; }
    public string Type { get; set; }
    public string EZCashRefId { get; set; }

}

public class Cash
{
    public string Date { get; set; }
    public string Direction { get; set; }
    public int Amount { get; set; }
    public string Status { get; set; }
    public string Data { get; set; }
}

