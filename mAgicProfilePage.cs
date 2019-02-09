using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using mAgicTvPlugIn;
using magicAnime;
using mAgicTvPlugIn.Properties;

namespace mAgicTvPlugIn
{
	public partial class TVOukokuConfigPage : Scheduler.ProfilePage
	{
		public TVOukokuConfigPage()
		{
			InitializeComponent();
		}

		public override void Load( Scheduler.Profile profile )
		{
			// insert combo item
			foreach (string key in AreaToID.INSTANCE.Keys)
				comboArea.Items.Add(key);
			foreach (string key in PlatformToID.INSTANCE.Keys)
				comboPlatform.Items.Add(key);
			foreach (string key in CategoryToID.INSTANCE.Keys)
				comboCategory.Items.Add(key);

			iPEG_Digital_Scheduler.TVOukokuConfig mprofile = profile as iPEG_Digital_Scheduler.TVOukokuConfig;

			if(mprofile != null)
			{
				textSearch.Text = mprofile.search_text;

				selectDefaultItem(comboArea,     mprofile.area,     Settings.Default.DefaultArea);
				selectDefaultItem(comboPlatform, mprofile.platform, Settings.Default.DefaultPlatform);
				selectDefaultItem(comboCategory, mprofile.category, Settings.Default.DefaultCategory);

				string platform = mprofile.platform;
				if(string.IsNullOrEmpty(platform))
					platform = Settings.Default.DefaultPlatform;
				comboPlatform.SelectedItem = platform;
			}
		}

		private static void selectDefaultItem(ComboBox combo, string value, string defaultValue)
		{
			if(string.IsNullOrEmpty(value))
				value = defaultValue;
			combo.SelectedItem = value;
		}

		public override void Save( Scheduler.Profile profile )
		{
			iPEG_Digital_Scheduler.TVOukokuConfig mprofile = profile as iPEG_Digital_Scheduler.TVOukokuConfig;
			if ( mprofile != null)
			{
				mprofile.area        = (string)comboArea.SelectedItem;	// ï∂éöóÒÇµÇ©ì¸ÇÍÇƒÇ»Ç¢ÇµÅAÇﬂÇ«Ç¢Ç©ÇÁÉRÉåÇ≈Ç¢Ç¢Ç≈ÇµÇÂÅ[
				mprofile.platform    = (string)comboPlatform.SelectedItem;
				mprofile.category    = (string)comboCategory.SelectedItem;
				mprofile.search_text = textSearch.Text;
			}
		}


		private void buttonCommonConfig_Click(object sender, EventArgs e)
		{
			CommonConfigDialog dlg = new CommonConfigDialog();
			dlg.ShowDialog(this);
		}
	}

}


