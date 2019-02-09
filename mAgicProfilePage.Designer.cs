namespace mAgicTvPlugIn
{
	partial class TVOukokuConfigPage
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
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.buttonCommonConfig = new System.Windows.Forms.Button();
			this.textSearch = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.comboPlatform = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.comboArea = new System.Windows.Forms.ComboBox();
			this.comboCategory = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.buttonCommonConfig);
			this.groupBox2.Controls.Add(this.textSearch);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.comboCategory);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.comboPlatform);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.comboArea);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(14, 11);
			this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
			this.groupBox2.Size = new System.Drawing.Size(418, 185);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "iEPGデジタル検索設定";
			// 
			// buttonCommonConfig
			// 
			this.buttonCommonConfig.Location = new System.Drawing.Point(10, 155);
			this.buttonCommonConfig.Name = "buttonCommonConfig";
			this.buttonCommonConfig.Size = new System.Drawing.Size(232, 23);
			this.buttonCommonConfig.TabIndex = 4;
			this.buttonCommonConfig.Text = "iEPGデジタルプラグイン 共通設定";
			this.buttonCommonConfig.UseVisualStyleBackColor = true;
			this.buttonCommonConfig.Click += new System.EventHandler(this.buttonCommonConfig_Click);
			// 
			// textSearch
			// 
			this.textSearch.Location = new System.Drawing.Point(113, 24);
			this.textSearch.Name = "textSearch";
			this.textSearch.Size = new System.Drawing.Size(298, 22);
			this.textSearch.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(7, 90);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(52, 15);
			this.label3.TabIndex = 2;
			this.label3.Text = "放送波";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 58);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(67, 15);
			this.label2.TabIndex = 2;
			this.label2.Text = "検索地域";
			// 
			// comboPlatform
			// 
			this.comboPlatform.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPlatform.FormattingEnabled = true;
			this.comboPlatform.Location = new System.Drawing.Point(112, 87);
			this.comboPlatform.Name = "comboPlatform";
			this.comboPlatform.Size = new System.Drawing.Size(173, 23);
			this.comboPlatform.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 28);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(95, 15);
			this.label1.TabIndex = 1;
			this.label1.Text = "検索キーワード";
			// 
			// comboArea
			// 
			this.comboArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboArea.FormattingEnabled = true;
			this.comboArea.Location = new System.Drawing.Point(113, 55);
			this.comboArea.Name = "comboArea";
			this.comboArea.Size = new System.Drawing.Size(173, 23);
			this.comboArea.TabIndex = 0;
			// 
			// comboCategory
			// 
			this.comboCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboCategory.FormattingEnabled = true;
			this.comboCategory.Location = new System.Drawing.Point(112, 119);
			this.comboCategory.Name = "comboCategory";
			this.comboCategory.Size = new System.Drawing.Size(173, 23);
			this.comboCategory.TabIndex = 0;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(7, 122);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(67, 15);
			this.label4.TabIndex = 2;
			this.label4.Text = "大ジャンル";
			// 
			// TVOukokuConfigPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(445, 207);
			this.Controls.Add(this.groupBox2);
			this.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "TVOukokuConfigPage";
			this.Text = "Form1";
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox textSearch;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboArea;
		private System.Windows.Forms.Button buttonCommonConfig;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox comboPlatform;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox comboCategory;

	}
}