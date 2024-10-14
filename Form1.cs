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

namespace Football
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                panel1.Enabled = true;
                App.DataTable1.AddDataTable1Row(App.DataTable1.NewDataTable1Row());
                dataTable1BindingSource.MoveLast();
                txtMatchName.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Message",MessageBoxButtons.OK, MessageBoxIcon.Error);
                App.DataTable1.RejectChanges();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            panel1.Enabled = true;
            txtMatchName.Focus();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            dataTable1BindingSource.ResetBindings(false);
            panel1.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                dataTable1BindingSource.EndEdit();
                App.DataTable1.AcceptChanges();
                App.DataTable1.WriteXml(String.Format("{0}//data.dat", Application.StartupPath));
                panel1.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                App.DataTable1.RejectChanges();
            }
        }



        static AppData db;
        protected static AppData App
        {
            get
            { 
                if(db==null)
                    db = new AppData();
                return db;
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            string fileName = string.Format("{0}//data.dat", Application.StartupPath);
            if (File.Exists(fileName))
                App.DataTable1.ReadXml(fileName);
            dataTable1BindingSource.DataSource = App.DataTable1;
            panel1.Enabled = false;
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Delete)
            {   
                if (MessageBox.Show("Are you sure you want to delete this record?", "Message",MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    dataTable1BindingSource.RemoveCurrent();
                
            }
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    var query = from o in App.DataTable1
                                where o.MatchName == txtSearch.Text || o.WhoVsWho.Contains(txtSearch.Text) || o.Goals == txtSearch.Text
                                select o;
                    dataGridView1.DataSource = query.ToList();
                }
                else
                    dataGridView1.DataSource = dataTable1BindingSource;
            }
        }
    }
}
