using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ExecuteFormula
{
    class Program
    {
        static void Main(string[] args)
        {
            string formula = "(Omset -(Omset * 10/110)) * Tarif";
            decimal omset = decimal.Parse("50000");
            decimal tarif = decimal.Parse("0.1");
            ExecuteFormula(formula,omset,tarif);
        }

        public static void ExecuteFormula(string formula,decimal omset, decimal tarif)
        {
            DataTable table = new DataTable();
            // Create the first column.
            DataColumn omsetColumn = new DataColumn();
            omsetColumn.DataType = System.Type.GetType("System.Decimal");
            omsetColumn.ColumnName = "Omset";
            omsetColumn.DefaultValue = omset;
           

            // Create the first column.
            DataColumn tarifColumn = new DataColumn();
            tarifColumn.DataType = System.Type.GetType("System.Decimal");
            tarifColumn.ColumnName = "Tarif";
            tarifColumn.DefaultValue = tarif;

            // Create the second, calculated, column.
            DataColumn bagiHasilColumn = new DataColumn();
            bagiHasilColumn.DataType = System.Type.GetType("System.Decimal");
            bagiHasilColumn.ColumnName = "BagiHasil";
            bagiHasilColumn.Expression = formula;


            // Add columns to DataTable.
            table.Columns.Add(omsetColumn);
            table.Columns.Add(tarifColumn);
            table.Columns.Add(bagiHasilColumn);


            DataRow row = table.NewRow();
            table.Rows.Add(row);
            //DataView view = new DataView(table);
            // dataGrid1.DataSource = view;
            //Assert.AreEqual(909.09,row["BagiHasil"])
            Console.WriteLine("909.09 = {0}", row["BagiHasil"]);
            Console.Read();

        }
    }


}
