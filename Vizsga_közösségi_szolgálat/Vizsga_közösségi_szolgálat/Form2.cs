using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Reflection;

using System.Windows.Forms;

namespace Vizsga_közösségi_szolgálat
{
    
    public partial class Form2 : Form
    {
        static string evismetlo = "";
        static int igennem;
        public Form2()
        {
            InitializeComponent();
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            AdatokRogzitese();

        }

        private void AdatokRogzitese()
        {
            string nev = textBox1.Text;
            int evfolyam = int.Parse(textBox2.Text);
            string osztaly = textBox3.Text;

            AppDomain.CurrentDomain.SetData("DataDirectory", Path.GetFullPath(
                          Path.Combine(Assembly.GetExecutingAssembly().Location, @"..\..\..\Resources")));
            var conn = new SqlConnection(
                @"Server=(localdb)\MSSQLLocalDB;" +
                @"AttachDbFileName=|DataDirectory|Kozossegi_szolgalat.mdf");
            conn.Open();
            var cmd = new SqlCommand("Insert into gyak_tanulo(nev, evfolyam, osztaly, evismetlo) values('"+nev+"',"+evfolyam+",'"+osztaly+"','"+evismetlo+"')", conn);
            cmd.ExecuteReader();
            {
                MessageBox.Show("Sikeres mentes");
            }
            

            conn.Close();
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            igennem = comboBox1.SelectedIndex;

            if (igennem == 0)
            {
                evismetlo = "igen";
            }
            else
            {
                evismetlo = "nem";
            }

        }
    }
}
