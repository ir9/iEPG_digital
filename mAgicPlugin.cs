//
// ABSTRACT	: mAgicTV5�^��v���O�C�� for mAgicAnime
// UPDATE	: �e���v���[�g�^��@�\��p�~
// UPDATE	: �掿�E�������d��I���ł���悤�ɂ���(2007/03/20)
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
using System.ComponentModel;
using mAgicTvPlugIn.Properties;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using magicAnime;
using KernelAPI;
using User32API;

// mAgicManager�ƘA�����ė\��^����Ǘ�����

namespace mAgicTvPlugIn
{
	////////////////////////////////////////////////////////////////
	// CLASS		:	mAgicScheduler
	// ABSTRACT	:	mAgicTV5�𗘗p���Ĕԑg�^���\�񂷂�h���N���X
	//
	////////////////////////////////////////////////////////////////
	public partial class iPEG_Digital_Scheduler : Scheduler
	{
		private class TitleUniqID
		{
			public string title;
			public string id;
			public TitleUniqID(string title_, string id_) {
				title = title_;
				id    = id_;
			}
		};

		// �v���O�C�����̂�Ԃ�
		public override string Name { get { return "���񂭂�T���n�f�W�Ř^��\�񂵂����̂���"; } }

		// �^��\�t�g�K��̘^���t�H���_��Ԃ�
		public override string Folder { get{ return null; } }
		public override string Extension { get { return ".ts"; } }
		public override bool SubDirectory { get { return true; } }
		public override AbilityFlag Ability { get { return AbilityFlag.MakeReservation; } }

		// ver 1.8.01
		public override Type ProfileType { get { return typeof(TVOukokuConfig); } }
		public override Type ProfilePageType { get { return typeof(TVOukokuConfigPage); } }

		/*-------------------------------------*
		 *	�v���O�C���@�\�Ɂu�������C�x���g�v�����݂��Ȃ��̂�
		 *	�R���X�g���N�^�ł��ɂ傲�ɂ傷�鎖�ɂ���B
		 *-------------------------------------*/
		public iPEG_Digital_Scheduler()
		{

		}

		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::MakeReservation
		// ABSTRACT	:	�\��o�^����B
		//				�e���v���[�g�̎w�肪�K�{�B�ǖ��͖��������B->�p�~
		// UPDATE	: ireserve.exe�𒼐ڌĂяo���悤�ɕύX(070506)
		////////////////////////////////////////////////////////////////
		public override void MakeReservation(
			string		title			,	// �^��^�C�g��
			string		uniqueID		,	// ���j�[�NID
			string		tvStation		,	// �e���r�ǖ�
			DateTime	startDateTime	,	// �^��J�n����
			int			minute			,	// �^�掞��(��)
			string		descript		,	// �ԑg�̐���
			uint		groupCode		,	// �O���[�v�ԍ�
			Profile		param			)	// �ԑg���Ƃ̐ݒ�
		{
			TraceListener debugListener = CreateDebugListener();
			if(debugListener != null)
				Debug.Listeners.Add(debugListener);

			try
			{
				TitleUniqID title_id = ParseTitleID(title, uniqueID);

				TVOukokuConfig conf = (iPEG_Digital_Scheduler.TVOukokuConfig)param;

				// �F�X�ϊ�
				int    areaID     = AreaToID.get(conf.area);
				int    platformID = PlatformToID.get(conf.platform);
				int    categoryID = CategoryToID.get(conf.category);
				string searchText = string.IsNullOrEmpty(conf.search_text) ? title_id.title : conf.search_text;

				ReservationModel model    = new ReservationModel(searchText, areaID, platformID, categoryID, startDateTime, tvStation, debugListener != null);
				TVOukoku         oukoku   = new TVOukoku();
				string           iEPGFile = oukoku.SearchEntry(model);

				if(title_id.id != null)
					iEPGFile = ReplaceIEPGProgramTitle(iEPGFile, title_id.id);

				// tvpid �o�^����
				string iEPGFileName = WriteIEPGFile(iEPGFile);
				using(Process p = Process.Start(iEPGFileName)) {
					p.WaitForExit(10 * 1000);
				}
				if(File.Exists(iEPGFileName))
					File.Delete(iEPGFileName);
			}
			finally
			{
				if(debugListener != null)
				{
					Debug.Listeners.Remove(debugListener);
					debugListener.Close();
				}
			}
		}

		/**
		 *	��̒萔���F�X���邯�ǁc�܂����������d�l�Ȃ̂���c
		 */
		private TitleUniqID ParseTitleID(string title, string uniqID) {
			string[] a = title.Split(new String[] { uniqID + "_" }, StringSplitOptions.RemoveEmptyEntries);
			Regex titleOpt = new Regex(@"\s*\d+�b\s*$", RegexOptions.Compiled);

			if(a.Length == 2) {
				return new TitleUniqID(titleOpt.Replace(a[1], ""), a[0] + uniqID);
			} else {
				return new TitleUniqID(titleOpt.Replace(a[0], ""), null);
			}
		}


		static Regex IEPG_TITLE = new Regex(@"(program-title\s*:\s*.+?)[\r\n]+", RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
		private string ReplaceIEPGProgramTitle(string iEPG, string uniq_id)
		{
			return IEPG_TITLE.Replace(iEPG, @"$1_" + uniq_id + "\r\n");
		}


		private string WriteIEPGFile(string iEPGFile)
		{
			string tmpFileName = Path.GetTempFileName() + ".tvpid";
			using(TextWriter w = new StreamWriter(tmpFileName, false, Encoding.GetEncoding("shift_jis"))) {
				w.Write(iEPGFile);
			}
			return tmpFileName;
		}

		private TraceListener CreateDebugListener()
		{
			string path = Settings.Default.DebugOutputLog;
			if(string.IsNullOrEmpty(path))
				return null;
			FileInfo info = new FileInfo(path);
			if(!Directory.Exists(info.DirectoryName))
				return null;
			return new DebugListener(path);
		}

		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::CancelReservation
		// UPDATE	:	ChangeReservation�̋@�\��ւƂ��Ď���(070508)
		////////////////////////////////////////////////////////////////
		public override void CancelReservation(
			string title,
			string uniqueID)
		{
			throw new NotImplementedException("CancelReservation�͎�������Ă��܂���B");
		}


		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::ChangeReservation
		// ABSTRACT	:	�\�񎞊Ԃ�ύX����
		// UPDATE	:	���@��ύX(070418)
		// UPDATE	:	�p�~(070508)
		//
		////////////////////////////////////////////////////////////////
		public override void ChangeReservation(
			string title,
			string uniqueID,
			DateTime newDateTime,	// �V������������
			uint groupCode,
			Profile	param)
		{
			throw new NotImplementedException("ChangeReservation�͎�������Ă��܂���B");
		}

		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::Flush
		// ABSTRACT	:	�\��f�[�^���t���b�V������
		////////////////////////////////////////////////////////////////
		public override void Flush()
		{

		}

		//
		// ABSTRACT	: �o�^���ꂽ�e���r�ǂ�S�Ď擾����
		//
		public override List<string> GetStations()
		{
			List<string> list = new List<string>();
			return list;
		}


		// ABSTRACT	: �\��̑��݂��m�F����
		// 
		public override bool ExistReservation(
			string title,		// �^��^�C�g��
			string uniqueID)	// ���j�[�NID
		{
			throw new NotImplementedException("ExistReservation�͎�������Ă��܂���B");
		}
	}

}
