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
    public partial class Form1 : Form
    {
        static int ID;
        static string evismetlo = "";
        static int igennem;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Adattablafeltoltes();
        }

        private void Adattablafeltoltes()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.GetFullPath(
               Path.Combine(Assembly.GetExecutingAssembly().Location, @"..\..\..\Resources")));
            var conn = new SqlConnection(
                @"Server=(localdb)\MSSQLLocalDB;"+
                @"AttachDbFileName=|DataDirectory|Kozossegi_szolgalat.mdf");
            conn.Open();
            var cmd = new SqlCommand("Select * from gyak_tanulo", conn);
            var reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                dataGridView1.Rows.Add(reader[0], reader[1], reader[2], reader[3], reader[4]);
            }
            conn.Close();
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            IdKinyerese();
        }

        private void IdKinyerese()
        {
            DataGridViewRow sor = dataGridView1.SelectedRows[0];
            ID = int.Parse(sor.Cells[0].Value.ToString());
            textBox1.Text = sor.Cells[1].Value.ToString();
            textBox2.Text = sor.Cells[2].Value.ToString();
            textBox3.Text = sor.Cells[3].Value.ToString();
            comboBox1.Text= sor.Cells[4].Value.ToString();
            KozossegiAdatok();
            Oraszam();
        }

        private void Oraszam()
        {
            int sumora = 0;
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.GetFullPath(
             Path.Combine(Assembly.GetExecutingAssembly().Location, @"..\..\..\Resources")));
            var conn = new SqlConnection(
                @"Server=(localdb)\MSSQLLocalDB;" +
                @"AttachDbFileName=|DataDirectory|Kozossegi_szolgalat.mdf");
            conn.Open();
            var cmd = new SqlCommand("Select sum(gyak_munka.oraszam) As sumora from gyak_munka where tanuloID=" + ID + "", conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                textBox5.Text = reader[0].ToString();
                sumora = int.Parse(reader[0].ToString());
                if(sumora<50)
                {
                    textBox4.Text = "Sajnos nincs meg az 50 óra";
                }
                else { textBox4.Text = "Gratulálunk meg az 50 óra"; }
            }
            conn.Close();
        }

        private void KozossegiAdatok()
        {
            richTextBox1.Text = "A kiválasztott tanuló közösségi szolgálati adatai: \r\n";
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.GetFullPath(
              Path.Combine(Assembly.GetExecutingAssembly().Location, @"..\..\..\Resources")));
            var conn = new SqlConnection(
                @"Server=(localdb)\MSSQLLocalDB;" +
                @"AttachDbFileName=|DataDirectory|Kozossegi_szolgalat.mdf");
            conn.Open();
            var cmd = new SqlCommand("Select * from gyak_munka where tanuloID="+ID+"", conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                richTextBox1.Text += (
                    reader[2].ToString()+"\t"+
                    reader[3].ToString() + "\t" +
                    reader[4].ToString() + "\r\n");
            }
            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form X = new Form2();
            X.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nev = textBox1.Text;
            int evfolyam= int.Parse(textBox2.Text);
            string osztaly=textBox3.Text;
            
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.GetFullPath(
                          Path.Combine(Assembly.GetExecutingAssembly().Location, @"..\..\..\Resources")));
            var conn = new SqlConnection(
                @"Server=(localdb)\MSSQLLocalDB;" +
                @"AttachDbFileName=|DataDirectory|Kozossegi_szolgalat.mdf");
            conn.Open();
            var cmd = new SqlCommand("Update gyak_tanulo set  nev='"+nev+"', evfolyam="+evfolyam+",osztaly='"+osztaly+"',evismetlo='"+evismetlo+"' where ID=" + ID + "", conn);
            cmd.ExecuteNonQuery();
            if (cmd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Sikeres modosítás");
            }
            else { MessageBox.Show("Sikertelen modosítás"); }

            conn.Close();
            Adattablafeltoltes();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            igennem = comboBox1.SelectedIndex;
            
            if(igennem==0)
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
