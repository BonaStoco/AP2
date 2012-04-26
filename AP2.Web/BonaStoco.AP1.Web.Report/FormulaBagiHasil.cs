using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BonaStoco.AP1.Web.Report
{
    public static class FormulaBagiHasil
    {
        public static FormulaField ExecuteFormula(string formula, decimal omset, decimal tarif)
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
             string _Omset=omset.ToString("N2");
             string _Tarif = tarif.ToString("N2");
            string replaceFormula = FormulaReplace(formula,_Omset,_Tarif);
            FormulaField _formula = new FormulaField()
            {
                Tarif = (decimal)row["Tarif"],
                Omset = (decimal)row["Omset"],
                BagiHasil = (decimal)row["BagiHasil"],
                FormulaName = formula,
                FormulaProcess = replaceFormula
            };                    

            //DataView view = new DataView(table);
            // dataGrid1.DataSource = view;
            //Assert.AreEqual(909.09,row["BagiHasil"])         
          
            return _formula;
        }

        private static string FormulaReplace(string formula,string omset, string tarif)
        {           
            var changeomset = formula.Replace("omset", omset);
            var formulaProcess = changeomset.Replace("tarif", tarif);
            return formulaProcess;
        }
    }

    public class FormulaField
    {
        public decimal Tarif { get; set; }
        public decimal Omset { get; set; }
        public decimal BagiHasil { get; set; }
        public string FormulaName { get; set; }
        public string FormulaProcess { get; set; }
    }    
}
