﻿
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json.Linq;
using System.Globalization;

var cash = ReadCash();
var tr = ReadTr();

foreach (var item in cash)
{
    JObject o = JObject.Parse(item.Data);
    Console.WriteLine(o["con"]);
}

Console.WriteLine("P");
Console.Read();

static List<Tr> ReadTr() 
{
    using (var reader = new StreamReader(@"D:\c3\dialogkiosk6\test\CashCmpF\tr.csv"))
    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
    {
        var records = csv.GetRecords<Tr>().ToList();
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

public class Cash
{
    public string Date { get; set; }
    public string Direction { get; set; }
    public int Amount { get; set; }
    public string Status { get; set; }
    public string Data { get; set; }
}
