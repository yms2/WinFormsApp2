
using System.Data;
using System.Runtime.InteropServices;

namespace WinFormsApp2
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		List<String> x = new List<string>();
		List<String> y = new List<string>();

		private void timer1_Tick(object sender, EventArgs e)
		{
			label1.Text = DateTime.Now.ToString("F");
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			timer1.Interval = 100;
			timer1.Start();
		}


		private void button1_Click(object sender, EventArgs e)
		{
			StreamReader file = new StreamReader("data.csv");

			DataTable table = new DataTable();
			table.Columns.Add("number");
			table.Columns.Add("count");

			while (!file.EndOfStream)
			{
				string line = file.ReadLine();
				string[] data = line.Split(',');
				table.Rows.Add(data[0], data[1]);

				x.Add(data[0]);
				y.Add(data[1]);
			}
			dataGridView1.DataSource = table;
			dataGridView1.ReadOnly = true; //수정 불가능
			dataGridView1.AllowUserToOrderColumns = false;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Title = "Save as Excel File";
			sfd.Filter = " Excel Files(2018)|*.xlsx";
			sfd.FileName = "";
			if (sfd.ShowDialog() == DialogResult.OK)
			{
				dataGridView_ExportToExcel(sfd.FileName, dataGridView1);
			}
		}

		private void dataGridView_ExportToExcel(string fileName, DataGridView dataGridView1)
		{
			Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();
			Microsoft.Office.Interop.Excel.Workbook workbook = application.Workbooks.Add(true);
			Microsoft.Office.Interop.Excel.Worksheet worksheet1 = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
			application.Visible = false;
			try
			{
				int rowcnt = dataGridView1.RowCount;
				int colcnt = dataGridView1.ColumnCount;

				String[,] data = new string[rowcnt, colcnt];

				for (int i = 0; i < rowcnt; i++)
				{
					for (int j = 0; j < colcnt; j++)
					{
						if (dataGridView1.Rows[i].Cells[j].Value != null)
						{
							worksheet1.Cells[i + 1, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
						}

					}
				}
				workbook.SaveAs(fileName);
				// Char Endchar = Convert.ToChar(Convert.ToInt32('A') + colcnt);
				// String EndStr = Endchar.ToString() + rowcnt.ToString(); //마지막 범위 위치 지정
				// worksheet1.get_Range()

				workbook.Close();
				application.Quit();
			}
			finally
			{
				ReleaseObject((Microsoft.Office.Interop.Excel.Application)worksheet1);
				ReleaseObject((Microsoft.Office.Interop.Excel.Application)workbook);
				ReleaseObject(application);
			}
		}

		private void ReleaseObject(Object obj)
		{
			try
			{
				if (obj != null)
				{
					Marshal.ReleaseComObject(obj); // 액셀 객체 해제 
					obj = null;
				}
			}
			catch (Exception ex)
			{
				obj = null;
				throw ex;
			}
			finally
			{
				GC.Collect(); // 가비지 수집 
			}
		}
	}
}