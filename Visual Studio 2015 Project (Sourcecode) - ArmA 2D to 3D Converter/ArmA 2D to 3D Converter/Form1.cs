using System;
using System.IO;
using System.Windows.Forms;

namespace ArmA_2D_to_3D_Converter
{
    public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		Mission CurrentMission;

		private void button1_Click(object sender, EventArgs e)
		{

			OpenFileDialog openConfigDialog = new OpenFileDialog();
			openConfigDialog.Title = "Select the SQM file";
			openConfigDialog.Filter = "SQM|*.sqm";
			if (textBox1.TextLength > 3)
			{
				openConfigDialog.InitialDirectory = Path.GetDirectoryName(textBox1.Text);
			}
			else
			{
				openConfigDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			}
			DialogResult objResult = openConfigDialog.ShowDialog(this);
			if (objResult == DialogResult.OK)
			{
				textBox1.Text = openConfigDialog.FileName;
			}
			//-------------------------------------------------------------------------------------------------------------
			try
			{
				button1.Enabled = false;
				CurrentMission = new Mission();
				CurrentMission.ReadSQM(openConfigDialog.FileName);
				CurrentMission.ProcessCode();
				string BIEdiPath = openConfigDialog.FileName.Substring(0, openConfigDialog.FileName.Length - 3) + "biedi";
				if (File.Exists(BIEdiPath))
				{
					File.Delete(BIEdiPath);
				}
				CurrentMission.WriteBIEdi(BIEdiPath);
				textBox1.Text = string.Empty;
				MessageBox.Show("3D-Missionfile successfully created");
				button1.Enabled = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				button1.Enabled = true;
			}
		}
	}
}