using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using mAgicTvPlugIn;

namespace testapp
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			iPEG_Digital_Scheduler digi = new iPEG_Digital_Scheduler();


		}

		private void button2_Click(object sender, EventArgs e)
		{
			TVOukoku o = new TVOukoku();
			o.ParserTest();
		}
	}
}