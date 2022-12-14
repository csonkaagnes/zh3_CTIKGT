using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp_CTIKGT.Models;

namespace WinFormsApp_CTIKGT
{
    public partial class Form2 : Form
    {
        SeCocktailsContext context = new SeCocktailsContext();
        public Form2()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (ValidateChildren() == true)
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            var cocktails = from x in context.Cocktails
                            select x;
            comboBoxCocktail.DisplayMember = "Name";
            cocktailBindingSource.DataSource = cocktails.ToList();

            var materials = from x in context.Materials
                            select x;
            comboBoxMaterial.DisplayMember = "Name";
            materialBindingSource.DataSource = materials.ToList();
        }

        private bool Check(string menny) 
        {
            Regex r = new Regex("^[0-9]*$");
            return r.IsMatch(menny);
        }

        private void textBoxMennyiség_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(textBoxMennyiség,"");
        }

        private void textBoxMennyiség_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty((textBoxMennyiség.Text).ToString()) || !Check((textBoxMennyiség.Text).ToString()))
            {
                e.Cancel = true;
                errorProvider1.SetError(textBoxMennyiség, "Érvénytelen");
            }
        
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
