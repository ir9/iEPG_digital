// ABSTRACT	: mAgicTV5�^��v���O�C�� �ԑg���ݒ�p�N���X
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

		// ABSTRACT : �ԑg���Ƃ̌ʐݒ�
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
			internal string area        = "";	// �����H �V���H CATV(���) �Ƃ��H
			internal string platform    = "";	// �n��g �Ƃ� ���ׂĂ̔ԑg �Ƃ� �X�J�p�[ �Ƃ�
			internal string category    = "";	// �A�j���^���B �Ƃ� ���^���C�h�V���[ �Ƃ�
		}

	}
}
