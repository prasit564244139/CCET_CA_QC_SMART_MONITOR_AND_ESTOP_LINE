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
using iTextSharp.text.pdf;
using System.IO;

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

        private void button1_Click(object sender, EventArgs e)
        {
            // Load the PDF file
            PdfReader reader = new PdfReader("input.pdf");
            using (FileStream fs = new FileStream("output.pdf", FileMode.Create))
            {
                // Create a PDF stamper object to write changes to the PDF
                PdfStamper stamper = new PdfStamper(reader, fs);

                // Iterate through each page of the PDF
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    // Get the current page
                    PdfDictionary page = reader.GetPageN(i);

                    // Create a content stream to add text to the page
                    PdfContentByte cb = stamper.GetOverContent(i);

                    // Set the font and size for the text
                    BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb.SetFontAndSize(bf, 12);


                    // Set the position for the text
                    cb.BeginText();
                    float pageHeight = reader.GetPageSizeWithRotation(i).Height;

                    /////// TIME/DATE
                    cb.SetTextMatrix(147, pageHeight - 131);
                    DateTime now = DateTime.Now;
                    string formattedDate = now.ToString("yyyy-MM-dd");
                    string formattedTime = now.ToString("HH:mm:ss");
                    cb.ShowText(formattedTime.ToString() + " / " + formattedDate.ToString());

                    ////// MODEL
                    cb.SetTextMatrix(370, pageHeight - 131);
                    cb.ShowText("RSHH17RR");

                    ////// LINE NO
                    cb.SetTextMatrix(147, pageHeight - 146);
                    cb.ShowText("B2");

                    ////// LINE NO
                    cb.SetTextMatrix(370, pageHeight - 146);
                    cb.ShowText("B870339");



                    cb.SetTextMatrix(0, pageHeight - 40); // Set the x-coordinate to 100 and the y-coordinate to the top of the page minus 100

                    // End the text object
                    cb.EndText();
                }

                // Close the PDF stamper object
                stamper.Close();
            }

            try
            {
                System.Diagnostics.Process.Start("output.pdf");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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
