using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MegaEscarito
{
    public enum Material {Oak, Pine, Laminate, Purpleheart, Zebrawood, Mahogany};

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //Initialize materials list box
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
            if (errorList.Length > 0) {
                MessageBox.Show(errorList);
                return;
            }

            //Create new DeskOrder instance and run calculations
            DeskOrder myDeskOrder = new MegaEscarito.DeskOrder(varWidth, varLength, varDrawerCount, varMaterial, varOrderDays);

            //Save Desk Order to file
            myDeskOrder.SaveToFile("orders.txt");

            //Show Desk Order on screen
            //myDeskOrder.ShowOrder();
        }

    }
    public class DeskOrder
    {
        private int width;
        private int length;
        private int surfaceArea;
        private int drawers;
        private Material material;
        private int days;

        //material price list
        private Dictionary<Material, int> materialPriceList = new Dictionary<Material, int>();

        //rush order price array
        private string[] rushOrderPrices;

        //base desk price
        private const int baseDeskPrice = 200;

        //calculated variables
        private int surfaceAreaPrice;
        private int drawerPrice;
        private int materialPrice;
        private int rushOrderPrice;
        private int totalPrice;

        public DeskOrder(int inWidth, int inLength, int inDrawerCount, Material inMaterial, int inOrderDays)
        {
            //assign values to instance variables
            width = inWidth;
            length = inLength;
            surfaceArea = width * length;
            drawers = inDrawerCount;
            material = inMaterial;
            days = inOrderDays;
            
            //populate material price list
            materialPriceList.Add(Material.Oak, 200);
            materialPriceList.Add(Material.Laminate, 100);
            materialPriceList.Add(Material.Pine, 50);
            materialPriceList.Add(Material.Purpleheart, 350);
            materialPriceList.Add(Material.Zebrawood, 300);
            materialPriceList.Add(Material.Mahogany, 250);

            //rush order Array read in
            rushOrderPrices = File.ReadAllLines("rushPrices.txt");

            //Calculate Prices
            CalculatePrices();
            
        }

        private void CalculatePrices()
        {

            //Initialize then Calculate Surface Area Price
            surfaceAreaPrice = 0;
            if(surfaceArea > 1000)
            {
                surfaceAreaPrice = (surfaceArea - 1000) * 5;
            }


            //Calculate Drawer Price
            drawerPrice = drawers * 50;


            //Initialize then Calculate Material Price
            materialPrice = 0;
            if (materialPriceList.ContainsKey(material))
            {
                materialPrice = materialPriceList[material];
            }


            //Calculate Rush Order Price
            switch (days)
            {
                case 3:
                    if (surfaceArea < 1000) {
                        rushOrderPrice = int.Parse(rushOrderPrices[0]);
                    }
                    else if (surfaceArea < 2000)
                    {
                        rushOrderPrice = int.Parse(rushOrderPrices[1]);
                    }
                    else
                    {
                        rushOrderPrice = int.Parse(rushOrderPrices[2]);
                    }
                    break;
                case 5:
                    if (surfaceArea < 1000)
                    {
                        rushOrderPrice = int.Parse(rushOrderPrices[3]);
                    }
                    else if (surfaceArea < 2000)
                    {
                        rushOrderPrice = int.Parse(rushOrderPrices[4]);
                    }
                    else
                    {
                        rushOrderPrice = int.Parse(rushOrderPrices[5]);
                    }
                    break;
                case 7:
                    if (surfaceArea < 1000)
                    {
                        rushOrderPrice = int.Parse(rushOrderPrices[6]);
                    }
                    else if (surfaceArea < 2000)
                    {
                        rushOrderPrice = int.Parse(rushOrderPrices[7]);
                    }
                    else
                    {
                        rushOrderPrice = int.Parse(rushOrderPrices[8]);
                    }
                    break;
                default:
                    rushOrderPrice = 0;
                    break;
            }

            //Calculate Total Price
            totalPrice = baseDeskPrice + surfaceAreaPrice + drawerPrice + materialPrice + rushOrderPrice;
            MessageBox.Show(totalPrice.ToString());
        }

        public bool SaveToFile(string filePath)
        {
            string desktopOrderAsJSON = CreateJSONstring();
            MessageBox.Show(desktopOrderAsJSON);
            try
            {
                StreamWriter writer = new StreamWriter(filePath);
                writer.WriteLine(desktopOrderAsJSON);
                writer.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }

        private string CreateJSONstring()
        {
            string jsonOrder = "";

            // Form JSON
            jsonOrder += "{\"deskOrders\":[";
            jsonOrder += "{";
            jsonOrder += "\"width\":\"" + width.ToString() + "\",";
            jsonOrder += "\"length\":\"" + length.ToString() + "\",";
            jsonOrder += "\"surfaceArea\":\"" + surfaceArea.ToString() + "\",";
            jsonOrder += "\"drawers\":\"" + drawers.ToString() + "\",";
            jsonOrder += "\"material\":\"" + material + "\",";
            jsonOrder += "\"days\":\"" + days.ToString() + "\",";
            jsonOrder += "\"baseDeskPrice\":\"" + baseDeskPrice.ToString() + "\",";
            jsonOrder += "\"surfaceAreaPrice\":\"" + surfaceAreaPrice.ToString() + "\",";
            jsonOrder += "\"drawerPrice\":\"" + drawerPrice.ToString() + "\",";
            jsonOrder += "\"materialPrice\":\"" + materialPrice.ToString() + "\",";
            jsonOrder += "\"rushOrderPrice\":\"" + rushOrderPrice.ToString() + "\",";
            jsonOrder += "\"totalPrice\":\"" + totalPrice.ToString() + "\"";
            jsonOrder += "}";
            jsonOrder += "]}";

            return jsonOrder;
        }
    }
}
