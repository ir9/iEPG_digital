// ABSTRACT	: mAgicTV5録画プラグイン 番組毎設定用クラス
//
//
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Xml;
using mAgicTvPlugIn.Properties;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using magicAnime;

namespace mAgicTvPlugIn
{
	public partial class iPEG_Digital_Scheduler : Scheduler
	{
		private static string KEY_SEARCH_TEXT = "SearchText";
		private static string KEY_AREA        = "Area";
		private static string KEY_PLATFORM    = "Platform";
		private static string KEY_CATEGORY    = "Category";

		// ABSTRACT : 番組ごとの個別設定
		public class TVOukokuConfig : Scheduler.Profile
		{
			public override void Import( XmlReader xr )
			{
				while( xr.Read() )
				{
					if( xr.NodeType == System.Xml.XmlNodeType.Element )
					{
						if( xr.LocalName.Equals( KEY_SEARCH_TEXT) )
							search_text = xr.ReadElementContentAsString();
						else if( xr.LocalName.Equals( KEY_AREA ) )
							area        = xr.ReadElementContentAsString();
						else if( xr.LocalName.Equals( KEY_PLATFORM ) )
							platform    = xr.ReadElementContentAsString();
						else if( xr.LocalName.Equals( KEY_CATEGORY ) )
							category    = xr.ReadElementContentAsString();
					}
					else if( xr.NodeType == System.Xml.XmlNodeType.EndElement )
						if( xr.LocalName.Equals( "SchedulerProfile" ) )
							return;
				}
			}
			public override void Export( XmlWriter xw )
			{
				xw.WriteElementString( KEY_SEARCH_TEXT, search_text);
				xw.WriteElementString( KEY_AREA,       area);
				xw.WriteElementString( KEY_PLATFORM,   platform);
				xw.WriteElementString( KEY_CATEGORY,   category);
			}

			internal string search_text = "";
			internal string area        = "";	// 東京？ 新潟？ CATV(茨城) とか？
			internal string platform    = "";	// 地上波 とか すべての番組 とか スカパー とか
			internal string category    = "";	// アニメ／特撮 とか 情報／ワイドショー とか
		}

	}
}
