using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace mAgicTvPlugIn
{
	partial class ReserveWizard : Form
	{
		public ReserveWizard()
		{
			InitializeComponent();
		}

		public DialogResult ShowDialog(
			string descript,
			string tvStation)
		{

			descriptLabel.Text	= descript;
			tvStationLabel.Text	= tvStation;

			return ShowDialog();
		}

		private void nextButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			Close();
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;

			Close();
		}

	}

}


