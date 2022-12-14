using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using WinFormsApp_CTIKGT.Models;

namespace WinFormsApp_CTIKGT
{
    public partial class Form1 : Form
    {
        SeCocktailsContext context = new SeCocktailsContext();

        Excel.Application xlApp;
        Excel.Workbook xlWB;
        Excel.Worksheet xlSheet;
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonExcel_Click(object sender, EventArgs e)
        {
            try
            {
                xlApp = new Excel.Application();
                xlWB = xlApp.Workbooks.Add(Missing.Value);
                xlSheet = xlWB.ActiveSheet;
                CreateTable();
                xlApp.Visible = true;
                xlApp.UserControl = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                xlApp.Quit();
                xlWB.Close();
                xlApp = null;
                xlWB = null;
               
            }
        }

        private void CreateTable()
        {
            string[] fejléc = new string[]
            {
                "Koktélok", "Ár",
            };

            for (int i = 0; i < fejléc.Length; i++)
            {
                xlSheet.Cells[1, i+1] = fejléc[i];
            }

            var mindenkoktél = context.Cocktails.ToList();

            object[,] adattömb = new object[mindenkoktél.Count(),fejléc.Count()];

            for (int i = 0; i < mindenkoktél.Count(); i++)
            {
                adattömb[i, 0] = mindenkoktél[i].Name;
                adattömb[i, 1] = mindenkoktél[i].Price;

            }

            int sor = adattömb.GetLength(0);
            int oszlop = adattömb.GetLength(1);

            Excel.Range AdatRange = xlSheet.get_Range("A2", System.Type.Missing).get_Resize(sor,oszlop);
            AdatRange.Value2 = adattömb;

            AdatRange.Columns.AutoFit();

            Excel.Range FejlécRange = xlSheet.get_Range("A1", System.Type.Missing).get_Resize(1, 2);
            FejlécRange.Interior.Color = Color.Fuchsia;
            FejlécRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        }

        private void textBoxCocktails_TextChanged(object sender, EventArgs e)
        {
            CocktailSzûrés();
        }

        private void CocktailSzûrés()
        {
            var cocktails = from x in context.Cocktails
                            where x.Name.Contains(textBoxCocktails.Text)
                            select x;
            listBoxCocktails.DataSource = cocktails.ToList();
            listBoxCocktails.DisplayMember = "Name";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CocktailSzûrés();
            MaterialSzûrés();
        }

        private void textBoxMaterials_TextChanged(object sender, EventArgs e)
        {
            MaterialSzûrés();
        }

        private void MaterialSzûrés()
        {
            var material = from x in context.Materials
                           where x.Name.Contains(textBoxMaterials.Text)
                           select x;
            listBoxMaterial.DataSource = material.ToList();
            listBoxMaterial.DisplayMember = "Name";
        }

        private void listBoxCocktails_SelectedIndexChanged(object sender, EventArgs e)
        {
            Receptlistázás();
        }

        private void Receptlistázás()
        {
            var SelectedCocktail = (Cocktail)listBoxCocktails.SelectedItem;
            var recept = from x in context.Recipes
                         where x.CocktailFk == SelectedCocktail.CocktailSk
                         select new DetailedReceptItem
                         {
                             RecipeSk = x.RecipeSk,
                             MaterialName = x.MaterialFkNavigation.Name,
                             MaterialType = x.MaterialFkNavigation.TypeFkNavigation.Name,
                             Quantity = x.Quantity,
                         };

            detailedReceptItemBindingSource.DataSource = recept.ToList();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            if (f2.ShowDialog() == DialogResult.OK)
            {
                var SelectedCocktail = (Cocktail)f2.comboBoxCocktail.SelectedItem;
                var SelectedMaterial = (Material)f2.comboBoxMaterial.SelectedItem;
                var mennyiség = decimal.Parse(f2.textBoxMennyiség.Text);

                 Recipe newRecept = new Recipe()
                 {
                     CocktailFk = SelectedCocktail.CocktailSk,
                     MaterialFk = SelectedMaterial.MaterialId,
                     Quantity = mennyiség,
                 };
                context.Recipes.Add(newRecept);
                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                Receptlistázás();
            }
            
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            var SelectedRecept = (DetailedReceptItem)detailedReceptItemBindingSource.Current;
            var törlendõ = (from x in context.Recipes
                           where x.RecipeSk == SelectedRecept.RecipeSk
                           select x).FirstOrDefault();
            context.Recipes.Remove(törlendõ);
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }
            Receptlistázás();
        }
    }
    public class DetailedReceptItem
    {
        public int RecipeSk { get; set; }

        public string MaterialName { get; set; }

        public string MaterialType { get; set; }

        public decimal Quantity { get; set; }
    }
}