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
    public enum Material {Oak, Pine, Laminate, Purpleheart, Zebrawood, Mahogany };

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Array valArray = Enum.GetValues(typeof(Material));

            foreach (Material materialValue in valArray)
            {
                listBox1.Items.Add(materialValue);
            }

            listBox1.SelectedIndex = 0;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Material material = (Material)listBox1.SelectedItem;
        }

        private void submit_Click(object sender, EventArgs e)
        {
            //Validations
            if (listBox1.SelectedItem == null)
            {
               
                MessageBox.Show("Please select a desktop material");
               
                
                return;
            }

            MessageBox.Show(this.width.Text);
            MessageBox.Show(this.length.Text);
            MessageBox.Show(this.days.Text);
            MessageBox.Show(this.drawers.Text);
            DeskOrder myDeskOrder = new MegaEscarito.DeskOrder();
            MessageBox.Show(listBox1.SelectedItem.ToString());
            //myDeskOrder.SetMaterial(listBox1.SelectedItem);
        }

    }
    public class DeskOrder
    {
        private Material material;

        public DeskOrder()
        {

        }

        public void SetMaterial(MegaEscarito.Material deskMaterial)
        {
            this.material = deskMaterial;
            MessageBox.Show(deskMaterial.ToString());
        }
    }
}
