using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using InvoicesService.Extensions;
using InvoicesService.Models;
using LiczbyNaSlowaNETCore;
using Xceed.Words.NET;

namespace InvoicesService.WordGenerator
{
    public static class Generator
    {
        private const int DefaultSize = 8;
        private static Font Font = new Font("Arial");

        public static void GenerateDocument(Invoice invoice)
        {
            var path = Service.Settings.PathToDocuments;
            var vendor = invoice.Vendor;
            var customer = invoice.Customer;
            var consumer = invoice.Consumer;
            var documentData = invoice.DocumentData;
            var paymentData = invoice.PaymentData;
            var tableRows = invoice.Items;
            decimal sum = invoice.getSum();

            var type = typeof(DocumentType);
            var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);;
            foreach (var p in fields)
            {
                var documentType = p.GetValue(null).ToString();

                var rgx = new Regex($"^{path.Replace("\\","\\\\")}{documentData.Number.Replace('/', '-')}_v\\d{{2}}_oryginał\\.docx$");
                var tmp = Directory.GetFiles(path).Where(a => rgx.IsMatch(a)).ToList();
                tmp.Sort();
                var index = 1;
                if (tmp.Count > 0)
                {
                    var str = tmp.Last();
                    int pFrom = str.IndexOf("_v") + "_v".Length;
                    int pTo = str.LastIndexOf("_oryginał");

                    var result = str.Substring(pFrom, pTo - pFrom);
                    index = int.Parse(result);
                    if (documentType == DocumentType.ORIGINAL)
                    {
                        index++;
                    }
                }

                var doc = DocX.Create(path + $@"{documentData.Number.Replace('/', '-')}_v{index:D2}_{documentType}.docx");
                doc.MarginLeft = 50f;
                doc.MarginRight = 50f;
                doc.MarginTop = 50f;
                doc.MarginBottom = 50f;

                CreateMainData(doc, documentType, vendor, customer, consumer, documentData, paymentData);
                doc.InsertParagraph();
                CreateTable(doc, tableRows, sum);

                doc.InsertParagraph();
                doc.InsertParagraph();
                doc.InsertParagraph();
                doc.InsertParagraph();
                doc.InsertParagraph();

                CreateSignTable(doc);

                CreateFooter(doc);

                doc.Save();
            }
        }

        private static Footers CreateFooter(DocX doc)
        {
            var formattingBold = new Formatting { Bold = true, Size = DefaultSize, FontFamily = Font };
            var formattingWithoutBold = new Formatting { Bold = false, Size = DefaultSize, FontFamily = Font };

            doc.AddFooters();
            doc.Footers.Odd.InsertParagraph().Append("_______________________________________________________________________________________________________________", formattingWithoutBold);
            var p = doc.Footers.Odd.InsertParagraph();
            p.AppendPageNumber(PageNumberFormat.normal);
            p.Append("/");
            p.AppendPageCount(PageNumberFormat.normal);
            p.Bold();
            p.FontSize(8);
            p.Alignment = Alignment.right;

            return doc.Footers;
        }

        private static Table CreateSignTable(DocX doc)
        {
            var t = doc.AddTable(1, 2);
            t.SetWidthsPercentage(new[] { 50f, 50f }, doc.PageWidth);
            var border = new Border();
            border.Tcbs = BorderStyle.Tcbs_none;

            var formattingBold = new Formatting { Bold = true, Size = DefaultSize, FontFamily = Font };
            var formattingWithoutBold = new Formatting { Bold = false, Size = DefaultSize, FontFamily = Font };

            for (int i = 0; i < 2; i++)
            {
                t.Rows[0].Cells[i].SetBorder(TableCellBorderType.Left, border);
                t.Rows[0].Cells[i].SetBorder(TableCellBorderType.Top, border);
                t.Rows[0].Cells[i].SetBorder(TableCellBorderType.Right, border);
                t.Rows[0].Cells[i].SetBorder(TableCellBorderType.Bottom, border);
            }

            var p = t.Rows[0].Cells[0].Paragraphs.First().Append($"........................................................................\r\n{Resource.customerSign}", formattingWithoutBold);
            p.Alignment = Alignment.center;

            p = t.Rows[0].Cells[1].Paragraphs.First().Append($"........................................................................\r\n{Resource.vendorSign}", formattingWithoutBold);
            p.Alignment = Alignment.center;

            doc.InsertTable(t);
            return t;
        }

        private static Table CreateMainData(DocX doc, string documentType, Vendor vendor, Customer customer, Consumer consumer, DocumentData documentData, PaymentData paymentData)
        {
            var t = doc.AddTable(1, 2);
            t.SetWidthsPercentage(new[] { 50f, 50f }, doc.PageWidth);
            var border = new Border();
            border.Tcbs = BorderStyle.Tcbs_none;
            for (int i = 0; i < 2; i++)
            {
                t.Rows[0].Cells[i].SetBorder(TableCellBorderType.Left, border);
                t.Rows[0].Cells[i].SetBorder(TableCellBorderType.Top, border);
                t.Rows[0].Cells[i].SetBorder(TableCellBorderType.Right, border);
                t.Rows[0].Cells[i].SetBorder(TableCellBorderType.Bottom, border);
            }

            GenerateVendorText(t.Rows[0].Cells[0], vendor);
            GenerateCustomerText(t.Rows[0].Cells[0], customer);
            if (consumer != null)
            {
                GenerateConsumerText(t.Rows[0].Cells[0], consumer);
            }

            GenerateDocumentDataText(t.Rows[0].Cells[1], documentData, documentType);
            GeneratePaymentDataText(t.Rows[0].Cells[1], paymentData);
            doc.InsertTable(t);

            return t;
        }

        private static Paragraph GenerateVendorText(Cell cell, Vendor vendor)
        {
            cell.RemoveParagraphAt(0);
            var p = cell.InsertParagraph();
            p.Alignment = Alignment.left;

            var formattingBold = new Formatting { Bold = true, Size = DefaultSize, FontFamily = Font };
            var formattingWithoutBold = new Formatting { Bold = false, Size = DefaultSize, FontFamily = Font };

            p.Append("Sprzedawca:\r\n", formattingWithoutBold);
            AddStringItem(p, "", vendor.CompanyName.ToUpper(), DefaultSize);
            AddStringItem(p, "", $"{vendor.VendorName} {vendor.VendorLastName}".ToUpper(), DefaultSize);
            AddStringItem(p, "", vendor.Street.ToUpper(), DefaultSize);
            AddStringItem(p, "", vendor.PostCode.ToUpper(), DefaultSize);
            AddStringItem(p, "NIP", vendor.Nip, DefaultSize);
            AddStringItem(p, "bank", vendor.BankName.ToUpper(), DefaultSize);
            AddStringItem(p, "konto", vendor.BankAccount, DefaultSize);

            return p;
        }

        private static Paragraph GenerateCustomerText(Cell cell, Customer customer)
        {
            var p = cell.InsertParagraph();
            p.Alignment = Alignment.left;

            var formattingBold = new Formatting { Bold = true, Size = DefaultSize, FontFamily = Font };
            var formattingWithoutBold = new Formatting { Bold = false, Size = DefaultSize, FontFamily = Font };

            p.Append("Nabywca:\r\n", formattingWithoutBold);
            AddStringItem(p, "", customer.CompanyName.ToUpper(), DefaultSize);
            AddStringItem(p, "", $"{customer.CustomerName} {customer.CustomerLastName}".ToUpper(), DefaultSize);
            AddStringItem(p, "", customer.Street.ToUpper(), DefaultSize);
            AddStringItem(p, "", customer.PostCode.ToUpper(), DefaultSize);
            AddStringItem(p, "NIP", customer.Nip, DefaultSize);

            return p;
        }

        private static Paragraph GenerateConsumerText(Cell cell, Consumer consumer)
        {
            var p = cell.InsertParagraph();
            p.Alignment = Alignment.left;

            var formattingBold = new Formatting { Bold = true, Size = DefaultSize, FontFamily = Font };
            var formattingWithoutBold = new Formatting { Bold = false, Size = DefaultSize, FontFamily = Font };

            p.Append("Odbiorca:\r\n", formattingWithoutBold);
            AddStringItem(p, "", consumer.CompanyName.ToUpper(), DefaultSize);
            AddStringItem(p, "", $"{consumer.ConsumerName} {consumer.ConsumerLastName}".ToUpper(), DefaultSize);
            AddStringItem(p, "", consumer.Street.ToUpper(), DefaultSize);
            AddStringItem(p, "", consumer.PostCode.ToUpper(), DefaultSize);
            AddStringItem(p, "NIP", consumer.Nip, DefaultSize);

            return p;
        }

        private static Paragraph GenerateDocumentDataText(Cell cell, DocumentData data, string type)
        {
            cell.RemoveParagraphAt(0);
            var p = cell.InsertParagraph();
            p.Alignment = Alignment.right;

            var formattingBold = new Formatting { Bold = true, Size = DefaultSize + 4, FontFamily = Font };
            var formattingWithoutBold = new Formatting { Bold = false, Size = DefaultSize, FontFamily = Font };

            p.Append($"Rachunek nr {data.Number}\r\n", formattingBold);
            p.Append($"{type}\r\n\r\n", formattingWithoutBold);
            AddStringItem(p, "Miejsce wystawienia", data.Place.ToUpper(), DefaultSize);
            AddStringItem(p, "Data wystawienia", $"{data.Date: dd.MM.yyyy}", DefaultSize);

            return p;
        }

        private static Paragraph GeneratePaymentDataText(Cell cell, PaymentData data)
        {
            var p = cell.InsertParagraph();
            p.Alignment = Alignment.right;
            AddStringItem(p, "Forma płatności",  data.PaymentMethod.Name.ToUpper(), DefaultSize);
            AddStringItem(p, "Data płatności", $"{data.PaymentDate: dd.MM.yyyy}", DefaultSize);

            return p;
        }

        private static Paragraph AddStringItem(Paragraph p, string tag, string text, int size)
        {
            var formattingBold = new Formatting { Bold = true, Size = size, FontFamily = Font };
            var formattingWithoutBold = new Formatting { Bold = false, Size = size, FontFamily = Font };
            if (text.Trim()=="")
            {
                return p;
            }

            if (tag == "")
            {
                p.Append($"{text}\r\n", formattingBold);
                return p;
            }

            p.Append($"{tag}: ", formattingWithoutBold);
            p.Append($"{text}\r\n", formattingBold);
            return p;
        }

        private static Table CreateTable(DocX doc, List<InvoiceItem> rows, decimal sum)
        {
            var t = doc.AddTable(rows.Count + 2, 6);
            t.Alignment = Alignment.center;
            t.Design = TableDesign.Custom;
            t.SetWidthsPercentage(new[] { 5f, 50f, 10f, 10f, 10f, 15f }, 1000);

            var formattingBold = new Formatting { Bold = true, Size = DefaultSize, FontFamily = Font };
            var formattingWithoutBold = new Formatting { Bold = false, Size = DefaultSize, FontFamily = Font };

            //Fill cells by adding text.  
            var p = t.Rows[0].Cells[0].Paragraphs.First().Append("Lp.", formattingBold);
            p.Alignment = Alignment.center;
            p = t.Rows[0].Cells[1].Paragraphs.First().Append("Nazwa towaru lub usługi", formattingBold);
            p.Alignment = Alignment.center;
            p = t.Rows[0].Cells[2].Paragraphs.First().Append("Cena [zł]", formattingBold);
            p.Alignment = Alignment.center;
            p = t.Rows[0].Cells[3].Paragraphs.First().Append("Ilość", formattingBold);
            p.Alignment = Alignment.center;
            p = t.Rows[0].Cells[4].Paragraphs.First().Append("JM", formattingBold);
            p.Alignment = Alignment.center;
            p = t.Rows[0].Cells[5].Paragraphs.First().Append("Wartość [zł]", formattingBold);
            p.Alignment = Alignment.center;

            foreach (var cell in t.Rows[0].Cells)
            {
                cell.FillColor = Color.DarkGray;
                cell.VerticalAlignment = VerticalAlignment.Center;
            }
            t.Rows[0].MinHeight = 20;

            for (var index = 0; index < rows.Count; index++)
            {
                var row = rows[index];
                FillRow(t, index + 1, row);
                for (var i = 0; i < 6; i++)
                {
                    t.Rows[index + 1].Cells[i].VerticalAlignment = VerticalAlignment.Center;
                    t.Rows[index + 1].Cells[i].VerticalAlignment = VerticalAlignment.Center;
                    t.Rows[index + 1].Cells[i].VerticalAlignment = VerticalAlignment.Center;
                    t.Rows[index + 1].Cells[i].VerticalAlignment = VerticalAlignment.Center;
                }
                t.Rows[index + 1].MinHeight = 20;
            }

            var border = new Border();
            border.Tcbs = BorderStyle.Tcbs_none;
            for (var i = 0; i < 5; i++)
            {
                t.Rows[rows.Count + 1].Cells[i].SetBorder(TableCellBorderType.Left, border);
                t.Rows[rows.Count + 1].Cells[i].SetBorder(TableCellBorderType.Top, border);
                t.Rows[rows.Count + 1].Cells[i].SetBorder(TableCellBorderType.Right, border);
                t.Rows[rows.Count + 1].Cells[i].SetBorder(TableCellBorderType.Bottom, border);
            }

            t.Rows[rows.Count + 1].MinHeight = 20;
            t.Rows[rows.Count + 1].Cells[4].VerticalAlignment = VerticalAlignment.Center;
            t.Rows[rows.Count + 1].Cells[4].Paragraphs.First().Append("Razem:", formattingBold);
            t.Rows[rows.Count + 1].Cells[5].VerticalAlignment = VerticalAlignment.Center;
            p = t.Rows[rows.Count + 1].Cells[5].Paragraphs.First().Append($"{sum:0.00}", formattingBold);
            p.Alignment = Alignment.right;

            doc.InsertTable(t);

            p = doc.InsertParagraph();
            p.Append("Sprzedawca zwolniony podmiotowo z podatku VAT (podatku od towarów i usług)", formattingWithoutBold).SpacingBefore(30);
            p.Alignment = Alignment.center;
            doc.InsertParagraph();
            p = doc.InsertParagraph().Append($"Do zapłaty: ", formattingWithoutBold);
            p.Append($"{sum:0.00} zł\r\n", formattingBold);
            p.Append("Słownie: ", formattingWithoutBold);
            p.Append($"{ NumberToText.Convert(sum, LiczbyNaSlowaNETCore.Currency.PLN, true)}", formattingBold);

            return t;
        }

        private static void FillRow(Table table, int i, InvoiceItem row)
        {
            var formattingBold = new Formatting { Bold = true, Size = DefaultSize, FontFamily = Font };
            var formattingWithoutBold = new Formatting { Bold = false, Size = DefaultSize, FontFamily = Font };

            var p = table.Rows[i].Cells[0].Paragraphs.First().Append($"{i}", formattingWithoutBold);
            p.Alignment = Alignment.right;
            p = table.Rows[i].Cells[1].Paragraphs.First().Append(row.Name, formattingWithoutBold);
            p.Alignment = Alignment.left;
            p = table.Rows[i].Cells[2].Paragraphs.First().Append($"{row.Price:0.00}", formattingWithoutBold);
            p.Alignment = Alignment.right;
            p = table.Rows[i].Cells[3].Paragraphs.First().Append($"{row.Amount:0.00}", formattingWithoutBold);
            p.Alignment = Alignment.right;
            if (row.Unit.Name.Any(char.IsDigit))
            {
                var sub = row.Unit.Name.SplitAndKeep(new[] {'2', '3'}, StringSplitOptions.RemoveEmptyEntries);
                p = table.Rows[i].Cells[4].Paragraphs.First().Append(sub[0], formattingWithoutBold)
                    .Append(sub[1], formattingWithoutBold).Script(Script.superscript);
            }
            else
            {
                p = table.Rows[i].Cells[4].Paragraphs.First().Append(row.Unit.Name, formattingWithoutBold);
            }
            p.Alignment = Alignment.left;
            p = table.Rows[i].Cells[5].Paragraphs.First().Append($"{row.Total:0.00}", formattingWithoutBold);
            p.Alignment = Alignment.right;
        }
    }
}
