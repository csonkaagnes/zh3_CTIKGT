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
            string[] fejl�c = new string[]
            {
                "Kokt�lok", "�r",
            };

            for (int i = 0; i < fejl�c.Length; i++)
            {
                xlSheet.Cells[1, i+1] = fejl�c[i];
            }

            var mindenkokt�l = context.Cocktails.ToList();

            object[,] adatt�mb = new object[mindenkokt�l.Count(),fejl�c.Count()];

            for (int i = 0; i < mindenkokt�l.Count(); i++)
            {
                adatt�mb[i, 0] = mindenkokt�l[i].Name;
                adatt�mb[i, 1] = mindenkokt�l[i].Price;

            }

            int sor = adatt�mb.GetLength(0);
            int oszlop = adatt�mb.GetLength(1);

            Excel.Range AdatRange = xlSheet.get_Range("A2", System.Type.Missing).get_Resize(sor,oszlop);
            AdatRange.Value2 = adatt�mb;

            AdatRange.Columns.AutoFit();

            Excel.Range Fejl�cRange = xlSheet.get_Range("A1", System.Type.Missing).get_Resize(1, 2);
            Fejl�cRange.Interior.Color = Color.Fuchsia;
            Fejl�cRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        }

        private void textBoxCocktails_TextChanged(object sender, EventArgs e)
        {
            CocktailSz�r�s();
        }

        private void CocktailSz�r�s()
        {
            var cocktails = from x in context.Cocktails
                            where x.Name.Contains(textBoxCocktails.Text)
                            select x;
            listBoxCocktails.DataSource = cocktails.ToList();
            listBoxCocktails.DisplayMember = "Name";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CocktailSz�r�s();
            MaterialSz�r�s();
        }

        private void textBoxMaterials_TextChanged(object sender, EventArgs e)
        {
            MaterialSz�r�s();
        }

        private void MaterialSz�r�s()
        {
            var material = from x in context.Materials
                           where x.Name.Contains(textBoxMaterials.Text)
                           select x;
            listBoxMaterial.DataSource = material.ToList();
            listBoxMaterial.DisplayMember = "Name";
        }

        private void listBoxCocktails_SelectedIndexChanged(object sender, EventArgs e)
        {
            Receptlist�z�s();
        }

        private void Receptlist�z�s()
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
                var mennyis�g = decimal.Parse(f2.textBoxMennyis�g.Text);

                 Recipe newRecept = new Recipe()
                 {
                     CocktailFk = SelectedCocktail.CocktailSk,
                     MaterialFk = SelectedMaterial.MaterialId,
                     Quantity = mennyis�g,
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
                Receptlist�z�s();
            }
            
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            var SelectedRecept = (DetailedReceptItem)detailedReceptItemBindingSource.Current;
            var t�rlend� = (from x in context.Recipes
                           where x.RecipeSk == SelectedRecept.RecipeSk
                           select x).FirstOrDefault();
            context.Recipes.Remove(t�rlend�);
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }
            Receptlist�z�s();
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