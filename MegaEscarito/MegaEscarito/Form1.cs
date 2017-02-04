using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MegaEscarito
{
    public enum Material { Oak, Pine, Laminate, Purpleheart, Zebrawood, Mahogany };

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Array valArray = Enum.GetValues(typeof(Material));

            foreach (Material materialValue in valArray)
            {
                material.Items.Add(materialValue);
            }

            material.SelectedIndex = 0;

            //Initialize Order Days options
            Dictionary<int, string> orderDayOptions = new Dictionary<int, string>();
            orderDayOptions.Add(14, "14 days (standard)");
            orderDayOptions.Add(3, "3 days (rush)");
            orderDayOptions.Add(5, "5 days (rush)");
            orderDayOptions.Add(7, "7 days (rush)");

            orderDays.DataSource = new BindingSource(orderDayOptions, null);
            orderDays.DisplayMember = "Value";
            orderDays.ValueMember = "Key";
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Material material = (Material)material.SelectedItem;
        }

        private void submit_Click(object sender, EventArgs e)
        {
            String errorList = "";
            int varWidth = 1;
            int varLength = 1;
            int varDrawerCount = 0;
            Material varMaterial = Material.Oak;
            int varOrderDays = 14;

            //**** VALIDATION SECTION ****

            //width
            if (width.Text.Trim() == "")
            {
                errorList += "Width is required.\n";
            }
            else
            {
                try
                {
                    varWidth = int.Parse(width.Text);
                }
                catch
                {
                    errorList += "Please enter a valid width (in inches).\n";
                }
            }

            //length
            if (length.Text.Trim() == "")
            {
                errorList += "Length is required.\n";
            }
            else
            {
                try
                {
                    varLength = int.Parse(length.Text);
                }
                catch
                {
                    errorList += "Please enter a valid length (in inches).\n";
                }
            }

            //material
            if (!Enum.IsDefined(typeof(Material), material.SelectedItem))
            {
                errorList += "Please select a valid material.\n";
            }
            else
            {
                varMaterial = (Material)material.SelectedItem;
            }

            //drawers
            if (drawerCount.Value < 0 || drawerCount.Value > 7)
            {
                errorList += "Please select number of drawers between 0 - 7.\n";
            }
            else
            {
                varDrawerCount = (int)drawerCount.Value;
            }

            //orderDays
            try
            {
                varOrderDays = ((KeyValuePair<int, string>)orderDays.SelectedItem).Key;
                if (varOrderDays != 3 && varOrderDays != 5 && varOrderDays != 7 && varOrderDays != 14)
                {
                    errorList += "Please select valid order days (3, 5, 7, or 14).\n";
                }
            }
            catch
            {
                errorList += "Please select valid order days (3, 5, 7, or 14).\n";
            }

            //if any errors then display them and return
            if (errorList.Length > 0)
            {
                MessageBox.Show(errorList);
                return;
            }

            DeskOrder myDeskOrder = new MegaEscarito.DeskOrder(varWidth, varLength, varDrawerCount, varMaterial, varOrderDays);

        }

    }
    public class DeskOrder
    {
        private Material material;

        public DeskOrder(int inWidth, int inLength, int inDrawerCount, Material inMaterial, int inOrderDays)
        {

        }

        public void SetMaterial(MegaEscarito.Material deskMaterial)
        {
            this.material = deskMaterial;
            MessageBox.Show(deskMaterial.ToString());
        }

    }
}
