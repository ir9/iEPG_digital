using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using mAgicTvPlugIn.Properties;

namespace mAgicTvPlugIn
{
	public partial class CommonConfigDialog : Form
	{
		public CommonConfigDialog()
		{
			InitializeComponent();
		}

		private void CommonConfigDialog_Load(object sender, EventArgs e)
		{
			LoadConfig();

			Assembly     asm     = Assembly.GetAssembly(this.GetType());
			AssemblyName asmName = asm.GetName();
			labelVersion.Text    = asmName.Version.ToString();
			toolTip_debug_output_log.SetToolTip(textBox_debug_log, Properties.Resources.output_debug_log_description);
			toolTip_debug_output_log.SetToolTip(label_output_debug_log, Properties.Resources.output_debug_log_description);
			toolTip_debug_output_log.SetToolTip(button_select_debug_log_path, Properties.Resources.output_debug_log_description);
		}

		private void LoadConfig()
		{
			// comboセッティング
			foreach(string key in AreaToID.INSTANCE.Keys)
				comboDefaultArea.Items.Add(key);

			// comboBox
			string defaultArea = Settings.Default.DefaultArea;
			if(defaultArea == null || defaultArea.Length == 0)
				defaultArea = "Nのフィールド";
			comboDefaultArea.SelectedItem = defaultArea;

			// debug log
			string debugLog = Settings.Default.DebugOutputLog;
			if(debugLog == null)
				debugLog = "";
			textBox_debug_log.Text = debugLog;
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			string debugLog = textBox_debug_log.Text;
			if(debugLog == null)
				debugLog = "";

			// 共通設定も保存
			Settings.Default.DefaultArea = (string)comboDefaultArea.SelectedItem;
			Settings.Default.DebugOutputLog = debugLog;
			Settings.Default.Save();

			this.Close();
		}

		private void button_select_debug_log_path_Click(object sender, EventArgs e)
		{
			saveFileDialog_select_debug_log.FileName = textBox_debug_log.Text;
			if(saveFileDialog_select_debug_log.ShowDialog(this) == DialogResult.Cancel)
				return;
			textBox_debug_log.Text = saveFileDialog_select_debug_log.FileName;
		}
	}
}


