//
// ABSTRACT	: mAgicTV5録画プラグイン for mAgicAnime
// UPDATE	: テンプレート録画機能を廃止
// UPDATE	: 画質・音声多重を選択できるようにした(2007/03/20)
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

// mAgicManagerと連動して予約録画を管理する

namespace mAgicTvPlugIn
{
	////////////////////////////////////////////////////////////////
	// CLASS		:	mAgicScheduler
	// ABSTRACT	:	mAgicTV5を利用して番組録画を予約する派生クラス
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

		// プラグイン名称を返す
		public override string Name { get { return "くんくん探偵を地デジで録画予約したいのだわ"; } }

		// 録画ソフト規定の録画先フォルダを返す
		public override string Folder { get{ return null; } }
		public override string Extension { get { return ".ts"; } }
		public override bool SubDirectory { get { return true; } }
		public override AbilityFlag Ability { get { return AbilityFlag.MakeReservation; } }

		// ver 1.8.01
		public override Type ProfileType { get { return typeof(TVOukokuConfig); } }
		public override Type ProfilePageType { get { return typeof(TVOukokuConfigPage); } }

		/*-------------------------------------*
		 *	プラグイン機構に「初期化イベント」が存在しないので
		 *	コンストラクタでごにょごにょする事にすゆ。
		 *-------------------------------------*/
		public iPEG_Digital_Scheduler()
		{

		}

		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::MakeReservation
		// ABSTRACT	:	予約登録する。
		//				テンプレートの指定が必須。局名は無視される。->廃止
		// UPDATE	: ireserve.exeを直接呼び出すように変更(070506)
		////////////////////////////////////////////////////////////////
		public override void MakeReservation(
			string		title			,	// 録画タイトル
			string		uniqueID		,	// ユニークID
			string		tvStation		,	// テレビ局名
			DateTime	startDateTime	,	// 録画開始時間
			int			minute			,	// 録画時間(分)
			string		descript		,	// 番組の説明
			uint		groupCode		,	// グループ番号
			Profile		param			)	// 番組ごとの設定
		{
			TraceListener debugListener = CreateDebugListener();
			if(debugListener != null)
				Debug.Listeners.Add(debugListener);

			try
			{
				TitleUniqID title_id = ParseTitleID(title, uniqueID);

				TVOukokuConfig conf = (iPEG_Digital_Scheduler.TVOukokuConfig)param;

				// 色々変換
				int    areaID     = AreaToID.get(conf.area);
				int    platformID = PlatformToID.get(conf.platform);
				int    categoryID = CategoryToID.get(conf.category);
				string searchText = string.IsNullOrEmpty(conf.search_text) ? title_id.title : conf.search_text;

				ReservationModel model    = new ReservationModel(searchText, areaID, platformID, categoryID, startDateTime, tvStation, debugListener != null);
				TVOukoku         oukoku   = new TVOukoku();
				string           iEPGFile = oukoku.SearchEntry(model);

				if(title_id.id != null)
					iEPGFile = ReplaceIEPGProgramTitle(iEPGFile, title_id.id);

				// tvpid 登録処理
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
		 *	謎の定数が色々あるけど…まぁこういう仕様なのだよ…
		 */
		private TitleUniqID ParseTitleID(string title, string uniqID) {
			string[] a = title.Split(new String[] { uniqID + "_" }, StringSplitOptions.RemoveEmptyEntries);
			Regex titleOpt = new Regex(@"\s*\d+話\s*$", RegexOptions.Compiled);

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
		// UPDATE	:	ChangeReservationの機能代替として実装(070508)
		////////////////////////////////////////////////////////////////
		public override void CancelReservation(
			string title,
			string uniqueID)
		{
			throw new NotImplementedException("CancelReservationは実装されていません。");
		}


		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::ChangeReservation
		// ABSTRACT	:	予約時間を変更する
		// UPDATE	:	方法を変更(070418)
		// UPDATE	:	廃止(070508)
		//
		////////////////////////////////////////////////////////////////
		public override void ChangeReservation(
			string title,
			string uniqueID,
			DateTime newDateTime,	// 新しい放送時間
			uint groupCode,
			Profile	param)
		{
			throw new NotImplementedException("ChangeReservationは実装されていません。");
		}

		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::Flush
		// ABSTRACT	:	予約データをフラッシュする
		////////////////////////////////////////////////////////////////
		public override void Flush()
		{

		}

		//
		// ABSTRACT	: 登録されたテレビ局を全て取得する
		//
		public override List<string> GetStations()
		{
			List<string> list = new List<string>();
			return list;
		}


		// ABSTRACT	: 予約の存在を確認する
		// 
		public override bool ExistReservation(
			string title,		// 録画タイトル
			string uniqueID)	// ユニークID
		{
			throw new NotImplementedException("ExistReservationは実装されていません。");
		}
	}

}
