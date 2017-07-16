using System;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;

namespace DundalkOil
{
    class InvoiceSorter
    {
        private Excel.Application excelApplication;
        private ExcelFile debtorAllocFile;
        private ExcelFile debtorEntryFile;
        private ExcelFile saleDocFile;
        private ExcelFile saleDocItemsFile;
        private ExcelFile traderFile;
        private SkipList skipList;
        private string[] files;

        public InvoiceSorter(SkipList skipList, string[] files)
        {
            this.excelApplication = new Excel.Application();
            this.files = files;
            this.skipList = skipList;
        }

        public void AddIDToSkipFile(string id)
        {
            skipList.AddIDToSkip(id);
        }

        public void OpenExcelFiles()
        {
            this.debtorAllocFile = new ExcelFile(this.excelApplication, this.GetFile(files, "DebtorAlloc.xlsx"));
            this.debtorEntryFile = new ExcelFile(this.excelApplication, this.GetFile(files, "DebtorEntry.xlsx"));
            this.saleDocFile = new ExcelFile(this.excelApplication, this.GetFile(files, "SaleDoc.xlsx"));
            this.saleDocItemsFile = new ExcelFile(this.excelApplication, this.GetFile(files, "SaleDocItem.xlsx"));
            this.traderFile = new ExcelFile(this.excelApplication, this.GetFile(files, "Trader.xlsx"));
        }

        private string GetFile(string[] filenames, string filetype)
        {
            return @Array.Find(filenames, f => f.Contains(filetype));
        }

        public Dictionary<String, Invoice> BuildInvoices()
        {
            Dictionary<String, Invoice> result = new Dictionary<string, Invoice>();
            Dictionary<String, ArrayList> customerIdsToInvoiceIds = new Dictionary<string, ArrayList>();
            CultureInfo info = new CultureInfo("en-IE");

            var invoiceRows = this.saleDocFile.RowCount();
            var saleDocData = GetDataArray(this.saleDocFile);
            string[] fields = { "ID", "ACCIDDEBTOR", "CUSTOMERID", "CUSTOMERTEXT", "DELIVERYTEXT", "DESCRIPTION", "NUMBER", "REMARKS", "CONSIGNEEID", "CONSIGNEETEXT", "REFERENCE" };
            for (int i = 1; i < invoiceRows; i++)
            {
                Invoice invoice = new Invoice();
                foreach (string field in fields)
                {
                    invoice.Set(field, GetValues(i, saleDocData, this.saleDocFile, field));
                }
                invoice.Set("DUEDATE", GetDataValues(i, saleDocData, this.saleDocFile, "DUEDATE").ToString("d", info));
                invoice.Set("POSTDATE", GetDataValues(i, saleDocData, this.saleDocFile, "POSTDATE").ToString("d", info));

                if (!skipList.HasID(invoice.GetID()))
                {
                    result[invoice.GetID()] = invoice;
                    ArrayList invoices;
                    if (!customerIdsToInvoiceIds.TryGetValue(invoice.GetCustomerID(), out invoices))
                    {
                        ArrayList list = new ArrayList();
                        list.Add(invoice.GetID());
                        customerIdsToInvoiceIds[invoice.GetCustomerID()] = list;
                    }
                    else
                    {
                        invoices.Add(invoice.GetID());
                    }
                }
            }
            saleDocData = null;
            this.saleDocFile.Close();

            var saleDocItemsData = GetDataArray(this.saleDocItemsFile);
            int saleDocItemsRows = this.saleDocItemsFile.RowCount();
            string[] saleDocItemsFields = {"ID", "ACCIDSALES", "CODE", "DISCOUNTRATE", "FRGAMOUNTVATEXC", "FRGCOSTAMOUNT", "FRGBMUCOSTPRICE", "FRGDISCOUNTAMOUNT", "FRGSALEPRICE", "FRGVATAMOUNT", "LOCATIONID", "NUMBERBYORDER", "SALEDOCID", "SALEPRODUCTID", "VATID", "QUANTITY", "QTYDELIVERED", "QTYWEIGHED", "QTYORDERED", "NAME", "FRGSALEPRICEVATINC", "FRGDISCOUNTAMOUNTVATINC", "BMUQUANTITY", "FRGBMUSALEPRICE", "FRGBMUSALEPRICEVATINC", "R$POSTDATE", "NETWEIGHTKG"};
            for (int i = 1; i < saleDocItemsRows; i++)
            {
                string id = saleDocItemsData[i, this.saleDocItemsFile.Headers()["SALEDOCID"]].ToString();
                Invoice invoice;
                if (result.TryGetValue(id, out invoice))
                {
                    DocItem item = new DocItem();
                    foreach (string field in saleDocItemsFields)
                    {
                        item.Set(field, GetValues(i, saleDocItemsData, this.saleDocItemsFile, field));
                    }
                    invoice.AddItem(item);
                }
            }
            saleDocItemsData = null;
            this.saleDocItemsFile.Close();

            var traderData = GetDataArray(this.traderFile);
            int tradeDataRows = this.traderFile.RowCount();
            string[] tradeFields = {"ID", "ADDRLINE01", "ADDRLINE02", "ADDRLINE03", "ADDRLINE04", "ADDRLINE05", "ADDRLINE06", "CODE", "CONTACTEMAIL", "CONTACTNAME", "NAME","ADDRPOSTALCODE"};
            for (int i = 1; i < tradeDataRows; i++)
            {
                Customer customer = new Customer();
                foreach (string field in tradeFields)
                {
                    customer.Set(field, GetValues(i, traderData, this.traderFile, field));
                }

                ArrayList invoices;
                if (customerIdsToInvoiceIds.TryGetValue(customer.GetID(), out invoices))
                {
                    foreach (String invoiceId in invoices)
                    {
                        result[invoiceId].SetCustomer(customer);
                    }
                }
            }
            traderData = null;
            this.traderFile.Close();

            var debtorEntryData = GetDataArray(this.debtorEntryFile);
            int debtorEntryRows = this.debtorEntryFile.RowCount();
            string[] debtoryEntryFields = { "DOCUMENTID", "DOCITEMID", "CUSTOMERID", "CURRENCYID", "DOCKIND", "POSTDATE", "FRGAMOUNT", "R$FRGAMOUNTALLOCATED", "R$FRGAMOUNTFREE", "CONTRACTID", "R$CLOSEDATE", "DUEDATE", "R$DUECLOSEDATE" };
            for (int i = 1; i < debtorEntryRows; i++)
            {
                string id = debtorEntryData[i, this.debtorEntryFile.Headers()["DOCUMENTID"]].ToString();
                Invoice invoice;
                if (result.TryGetValue(id, out invoice))
                {
                    DebtorEntry debtorEntry = new DebtorEntry();
                    foreach (string field in debtoryEntryFields)
                    {
                        debtorEntry.Set(field, GetValues(i, debtorEntryData, this.debtorEntryFile, field));
                    }
                    invoice.AddDebtorEntry(debtorEntry);
                }
            }
            debtorEntryData = null;
            this.debtorEntryFile.Close();

            var debtorAllocData = GetDataArray(this.debtorAllocFile);
            int debtorAllocRows = this.debtorAllocFile.RowCount();
            string[] debtorAllocFields = { "DEBITDOCUMENTID", "DEBITDOCITEMID", "CREDITDOCUMENTID", "CREDITDOCITEMID", "FRGAMOUNT" };
            for (int i = 1; i < debtorAllocRows; i++)
            {
                string id = debtorAllocData[i, this.debtorAllocFile.Headers()["DEBITDOCUMENTID"]].ToString();
                Invoice invoice;
                if (result.TryGetValue(id, out invoice)) 
                {
                    DebtorAlloc debtorAlloc = new DebtorAlloc();
                    foreach (string field in debtorAllocFields)
                    {
                        debtorAlloc.Set(field, GetValues(i, debtorAllocData, this.debtorAllocFile, field));
                    }
                    invoice.AddDebtorAlloc(debtorAlloc);
                }
            }
            debtorAllocData = null;
            this.debtorAllocFile.Close();
            this.excelApplication.Quit();
            Marshal.ReleaseComObject(this.excelApplication);
            return result;
        }

        String GetValues(int index, object[,] data, ExcelFile file, string columnName)
        {
            object value = data[index, file.Headers()[columnName]];
            return value != null ? value.ToString() : "";
        }

        DateTime GetDataValues(int index, object[,] data, ExcelFile file, string columnName)
        {
            object valueObject = data[index, file.Headers()[columnName]];
            if (valueObject != null)
            {
                return DateTime.FromOADate((double)valueObject);
            }
            else
            {
                return DateTime.MinValue;
            }
        }


        public object[,] GetDataArray(ExcelFile file)
        {
            int rows = file.RowCount();
            int columns = file.ColumnCount();
            var range = file.sheet().Range[file.sheet().Cells[2, 1], file.sheet().Cells[rows, columns]];
            object[,] data = range.Cells.Value2;
            range = null;
            return data;
        }

        public void CleanUp()
        {
            this.skipList.CleanUp();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
