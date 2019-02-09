namespace mAgicTvPlugIn
{
	partial class CommonConfigDialog
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナで生成されたコード

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.comboDefaultArea = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.buttonCanel = new System.Windows.Forms.Button();
			this.buttonOK = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.labelVersion = new System.Windows.Forms.Label();
			this.label_output_debug_log = new System.Windows.Forms.Label();
			this.textBox_debug_log = new System.Windows.Forms.TextBox();
			this.button_select_debug_log_path = new System.Windows.Forms.Button();
			this.saveFileDialog_select_debug_log = new System.Windows.Forms.SaveFileDialog();
			this.toolTip_debug_output_log = new System.Windows.Forms.ToolTip(this.components);
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// comboDefaultArea
			// 
			this.comboDefaultArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDefaultArea.FormattingEnabled = true;
			this.comboDefaultArea.Location = new System.Drawing.Point(148, 13);
			this.comboDefaultArea.Margin = new System.Windows.Forms.Padding(4);
			this.comboDefaultArea.Name = "comboDefaultArea";
			this.comboDefaultArea.Size = new System.Drawing.Size(229, 23);
			this.comboDefaultArea.TabIndex = 7;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(8, 17);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(122, 15);
			this.label3.TabIndex = 6;
			this.label3.Text = "デフォルト検索地域";
			// 
			// buttonCanel
			// 
			this.buttonCanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCanel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCanel.Location = new System.Drawing.Point(278, 173);
			this.buttonCanel.Name = "buttonCanel";
			this.buttonCanel.Size = new System.Drawing.Size(99, 23);
			this.buttonCanel.TabIndex = 8;
			this.buttonCanel.Text = "やめるのだわ";
			this.buttonCanel.UseVisualStyleBackColor = true;
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.Location = new System.Drawing.Point(173, 173);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(99, 23);
			this.buttonOK.TabIndex = 9;
			this.buttonOK.Text = "いいのだわ";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(88, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(271, 15);
			this.label1.TabIndex = 10;
			this.label1.Text = "くんくん探偵を地デジで録画予約したいのだわ";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.labelVersion);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(12, 83);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(365, 83);
			this.groupBox1.TabIndex = 11;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "プラグイン情報";
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 38);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(71, 15);
			this.label4.TabIndex = 15;
			this.label4.Text = "バージョン：";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 18);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(85, 15);
			this.label2.TabIndex = 14;
			this.label2.Text = "プラグイン名：";
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 59);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(132, 15);
			this.label5.TabIndex = 13;
			this.label5.Text = "どこかの第２プログラマ";
			// 
			// labelVersion
			// 
			this.labelVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelVersion.AutoSize = true;
			this.labelVersion.Location = new System.Drawing.Point(88, 38);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new System.Drawing.Size(104, 15);
			this.labelVersion.TabIndex = 11;
			this.labelVersion.Text = "Version: 0.9.1.0";
			// 
			// label_output_debug_log
			// 
			this.label_output_debug_log.AutoSize = true;
			this.label_output_debug_log.Location = new System.Drawing.Point(8, 48);
			this.label_output_debug_log.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label_output_debug_log.Name = "label_output_debug_log";
			this.label_output_debug_log.Size = new System.Drawing.Size(128, 15);
			this.label_output_debug_log.TabIndex = 6;
			this.label_output_debug_log.Text = "Debug log 出力パス";
			// 
			// textBox_debug_log
			// 
			this.textBox_debug_log.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBox_debug_log.Location = new System.Drawing.Point(148, 45);
			this.textBox_debug_log.Name = "textBox_debug_log";
			this.textBox_debug_log.Size = new System.Drawing.Size(195, 22);
			this.textBox_debug_log.TabIndex = 12;
			// 
			// button_select_debug_log_path
			// 
			this.button_select_debug_log_path.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button_select_debug_log_path.Location = new System.Drawing.Point(349, 45);
			this.button_select_debug_log_path.Name = "button_select_debug_log_path";
			this.button_select_debug_log_path.Size = new System.Drawing.Size(28, 23);
			this.button_select_debug_log_path.TabIndex = 13;
			this.button_select_debug_log_path.Text = "...";
			this.button_select_debug_log_path.UseVisualStyleBackColor = true;
			this.button_select_debug_log_path.Click += new System.EventHandler(this.button_select_debug_log_path_Click);
			// 
			// saveFileDialog_select_debug_log
			// 
			this.saveFileDialog_select_debug_log.DefaultExt = "log";
			this.saveFileDialog_select_debug_log.Filter = "log file (*.log)|*.log";
			// 
			// CommonConfigDialog
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCanel;
			this.ClientSize = new System.Drawing.Size(389, 208);
			this.Controls.Add(this.button_select_debug_log_path);
			this.Controls.Add(this.textBox_debug_log);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.buttonCanel);
			this.Controls.Add(this.comboDefaultArea);
			this.Controls.Add(this.label_output_debug_log);
			this.Controls.Add(this.label3);
			this.Font = new System.Drawing.Font("MS UI Gothic", 11.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CommonConfigDialog";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "共通設定";
			this.Load += new System.EventHandler(this.CommonConfigDialog_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox comboDefaultArea;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button buttonCanel;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label labelVersion;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label_output_debug_log;
		private System.Windows.Forms.TextBox textBox_debug_log;
		private System.Windows.Forms.Button button_select_debug_log_path;
		private System.Windows.Forms.SaveFileDialog saveFileDialog_select_debug_log;
		private System.Windows.Forms.ToolTip toolTip_debug_output_log;
	}
}