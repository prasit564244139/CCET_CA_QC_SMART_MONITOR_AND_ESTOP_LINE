using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Json2DataTable;

namespace CCET_CA_QC_SMART_MONITOR_AND_ESTOP_LINE
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        JsonToDataTable j2dt = new JsonToDataTable();
        String SQL, SQLDATA, SQL_SELECT;
        DataTable DT = new DataTable();

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void Query_DT(String DB, String SQL)
        {
            try
            {
                //dataGridView1.DataSource = null;
                j2dt.Url = "http://10.51.64.63:8085/service/query";
                j2dt.db = DB;
                j2dt.cmd = SQL;
                //dataGridView1.DataSource = j2dt.getData();
                DT = j2dt.getData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                SQL = "select distinct MODEL from DQC310 order by model";
                Query_DT("CRDB",SQL);
                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow dtRow in DT.Rows)
                    {
                        foreach (DataColumn dc in DT.Columns)
                        {
                            comboBox1.Items.Add(dtRow[dc].ToString());
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
