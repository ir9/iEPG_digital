using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;

// mAgicManagerと連動して予約録画を管理する

namespace magicAnime
{

	////////////////////////////////////////////////////////////////
	// CLASS	:	mAgicReservation
	// DESCRIPT	:	mAgicManagerの予約エントリ
	////////////////////////////////////////////////////////////////
	public class mAgicReservation : object, ICloneable
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct mAgicDateTime
		{
			public ushort year;
			public ushort month;
			public ushort dayOfWeek;
			public ushort day;
			public ushort hour;
			public ushort minute;
			public ushort dummy;
			public ushort dummy2;
		};

		const uint entrySize = 0x95c;			// エントリのサイズ

		static public uint GetBytes() { return entrySize; }

		////////////////////////////////////////////////////////////////
		// MODULE	:	mAgicReservation::mAgicReservation
		// FUNCTION	:	mAgicReservationのコンストラクタ
		////////////////////////////////////////////////////////////////
		public mAgicReservation(byte[] rawData)
		{
			this.rawData = rawData;

			if (rawData.GetLength(0) != entrySize)
				throw new NotImplementedException();

			//callBack(title, tableEntry);
		}

		private mAgicReservation()
		{
			this.rawData = null;
		}

		public object Clone()
		{
			mAgicReservation cloneObject = new mAgicReservation();

			cloneObject.rawData = (byte[])rawData.Clone();

			return cloneObject;
		}

		public byte[] ToBytes()
		{
			return rawData;
		}

		public DateTime startDateTime
		{
			get
			{
				mAgicDateTime tempDateTime = new mAgicDateTime();
				DateTime dateTime;

				tempDateTime = QuarryDateTime(0x98);

				dateTime = new DateTime(
					tempDateTime.year, tempDateTime.month, tempDateTime.day,
					tempDateTime.hour, tempDateTime.minute, 0);

				return dateTime;
			}
			set
			{
				mAgicDateTime dateTime = new mAgicDateTime();

				dateTime.year = (ushort)value.Year;
				dateTime.month = (ushort)value.Month;
				dateTime.day = (ushort)value.Day;
				dateTime.hour = (ushort)value.Hour;
				dateTime.minute = (ushort)value.Minute;
				dateTime.dayOfWeek = (ushort)value.DayOfWeek;

				InscribeDateTime(0x98, dateTime);
			}

		}
		public DateTime endDateTime
		{
			get
			{
				mAgicDateTime tempDateTime = new mAgicDateTime();
				DateTime dateTime;

				tempDateTime = QuarryDateTime(0xA8);

				dateTime = new DateTime(
					tempDateTime.year, tempDateTime.month, tempDateTime.day,
					tempDateTime.hour, tempDateTime.minute, 0);

				return dateTime;
			}
			set
			{
				mAgicDateTime dateTime = new mAgicDateTime();

				dateTime.year = (ushort)value.Year;
				dateTime.month = (ushort)value.Month;
				dateTime.day = (ushort)value.Day;
				dateTime.hour = (ushort)value.Hour;
				dateTime.minute = (ushort)value.Minute;
				dateTime.dayOfWeek = (ushort)value.DayOfWeek;

				InscribeDateTime(0xA8, dateTime);
			}

		}

		public string title
		{
			get
			{
				uint length;

				for (length = 0; rawData[length + 0xbc] != 0x00; length++) ;

				return Encoding.GetEncoding(932).GetString(rawData, 0xBC, (int)length);
			}
			set
			{
				byte[] temp;

				temp = Encoding.GetEncoding(932).GetBytes(value + '\0');

				Buffer.BlockCopy(temp, 0, rawData, 0xBC, temp.GetLength(0));
			}

		}

		private mAgicDateTime QuarryDateTime(int offset)
		{
			mAgicDateTime dateTime;
			byte[] temp = new byte[Marshal.SizeOf(typeof(mAgicDateTime))];

			Buffer.BlockCopy(rawData, offset, temp, 0, Marshal.SizeOf(typeof(mAgicDateTime)));

			GCHandle gcChandle = GCHandle.Alloc(temp, GCHandleType.Pinned);

			dateTime = (mAgicDateTime)Marshal.PtrToStructure(
				gcChandle.AddrOfPinnedObject(),
				typeof(mAgicDateTime));

			gcChandle.Free();

			return dateTime;
		}

		private void InscribeDateTime(int offset, mAgicDateTime dateTime)
		{
			byte[] temp = new byte[Marshal.SizeOf(typeof(mAgicDateTime))];

			GCHandle gcChandle = GCHandle.Alloc(temp, GCHandleType.Pinned);

			Marshal.StructureToPtr(dateTime, gcChandle.AddrOfPinnedObject(), false);

			gcChandle.Free();

			Buffer.BlockCopy(temp, 0, rawData, offset, Marshal.SizeOf(typeof(mAgicDateTime)));

		}

		public void Write(string fileName)
		{
			using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
			{
				fileStream.Write(rawData, 0, (int)entrySize);

				fileStream.Close();
			}
		}

		private byte[] rawData;
	};



	////////////////////////////////////////////////////////////////
	// CLASS	:	Scheduler
	// DESCRIPT	:	番組録画を予約するベースクラス
	//
	////////////////////////////////////////////////////////////////
	abstract class Scheduler
	{

		////////////////////////////////////////////////////////////////
		// MODULE	:	Scheduler::ReserveAll
		// FUNCTION	:	指定した番組の各
		//
		////////////////////////////////////////////////////////////////
		public bool ReserveAll(AnimeProgram animeProgram)
		{
		
			return false;
		}

		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::MakeReservation
		// DESCRIPT	:	時間を指定して録画予約を新規登録する
		// NOTE		:	同時録画できる環境では、時間重複してもfalseを返すとは限らない。
		//				同じ録画タイトルで呼ぶとfalseを返す。
		// RETURN	:	true	- 成功
		//			:	false	- 失敗（時間重複等）
		////////////////////////////////////////////////////////////////
		public abstract bool MakeReservation(string title,string tvStation,
			DateTime dateTime, uint minute, object extension);

		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::CancelReservation
		// DESCRIPT	:	指定された予約をキャンセルする
		////////////////////////////////////////////////////////////////
		public abstract void CancelReservation(string title);
	}




	////////////////////////////////////////////////////////////////
	// CLASS	:	mAgicScheduler
	// DESCRIPT	:	mAgicTV5を利用して番組録画を予約する派生クラス
	//
	////////////////////////////////////////////////////////////////
	class mAgicScheduler : Scheduler
	{

		const uint HeaderSize = 0x20;			// ヘッダのサイズ
	

		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::MakeReservation
		// DESCRIPT	:	予約登録する。
		//				テンプレートの指定が必須。局名は無視される。
		////////////////////////////////////////////////////////////////
		public override bool MakeReservation(
			string		title,			// 録画タイトル
			string		tvStation,		// テレビ局名
			DateTime	startDateTime,	// 録画開始時間
			uint		minute,			// 録画時間(分)
			object		template )		// テンプレート
		{

			// このクラスで予約するにはテンプレートの指定が必須
			if (template != null && template is mAgicReservation)
			{
				mAgicReservation newReserve;
				string managerExe;

				//
				// テンプレートを元に、新しい予約データを作成する
				//

				newReserve = (mAgicReservation)((mAgicReservation)template).Clone();

				newReserve.title			= title;
				newReserve.startDateTime	= startDateTime;
				newReserve.endDateTime		= startDateTime.AddMinutes( minute );

				//
				// mAgicManagerのスケジュールデータにエントリを追加する
				//

				managerExe = StopManager();		// 一時的にmAgicManagerを停止する
				
				string filePath = GetSettingFolder() + "\\schedule.dat";
				
				byte[] tableEntry = new byte[mAgicReservation.GetBytes()];

				using (FileStream file = new FileStream(filePath, FileMode.Append ))
				{
					byte[] temp = newReserve.ToBytes();
					file.Write( temp, 0, temp.GetLength(0) );
				}

				if ( managerExe != null )
					RestartManager( managerExe );	// mAgicManagerを起動し直す

				return true;
			}

			return false;
		}

		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::CancelReservation
		////////////////////////////////////////////////////////////////
		public override void CancelReservation(
			string title)	// キャンセルする録画タイトル
		{
		
		}

		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::StopManager
		// DESCRIPT	:	mAgicManagerを終了する
		////////////////////////////////////////////////////////////////
		static string StopManager()
		{
			System.IntPtr hWindow;
			string filePath = null;

			//
			// mAgicManagerの隠しメインウィンドウを取得する
			//
			hWindow = (System.IntPtr)KernelAPI.Forms.FindWindow("mAgicマネージャ", null);

			if ( KernelAPI.Forms.IsWindow(hWindow) )
			{
				//
				// mAgicManagerを終了させる
				//
				Process tvManProcess;
				System.IntPtr hTvWindow;
				int processId;

				KernelAPI.Forms.GetWindowThreadProcessId( hWindow, out processId );

				tvManProcess = Process.GetProcessById(processId);

				filePath = tvManProcess.MainModule.FileName;

				// メモ：
				// 予約録画中・予約録画直前でないことをチェックする
				
				//
				// mAgicPlayerが起動している場合は終了するかここで問い合わせ
				//

				hTvWindow = (System.IntPtr)KernelAPI.Forms.FindWindow("mAgicTVWindow", null);
				
				if (KernelAPI.Forms.IsWindow(hTvWindow))
				{
					if ( MessageBox.Show(
						"mAgicTVが起動していると予約データを更新できません。\n終了して続けますか？",
						"確認", MessageBoxButtons.YesNo,MessageBoxIcon.Question) != DialogResult.Yes)
					{
						throw new NotImplementedException(
							"予約データの更新はユーザーによりキャンセルされました");
					}

				}

				//
				// mAgicManagerに"終了"メニューのコマンドメッセージを送信する
				//
				KernelAPI.Forms.SendMessage(
					hWindow,KernelAPI.Forms.WM_COMMAND,
					(System.IntPtr)32772, (System.IntPtr)0);

				
				//
				// 終了を待機する
				//
				if (!tvManProcess.WaitForExit(3000))
				{
					throw new NotImplementedException(
						"mAgicManagerの終了に失敗しました。\n予約データを更新できません。");
				}

			}

			return filePath;
		}

		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::RestartManager
		// DESCRIPT	:	mAgicManagerを起動し直す
		////////////////////////////////////////////////////////////////
		static void RestartManager(string filePath)
		{
			try
			{
				Process.Start(filePath);

			}catch (Exception)
			{
				// もし起動できなくとも問題はない(?)
			}

		}


		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::GetScheduledEntries
		// DESCRIPT	:	mAgicTV5の全予約エントリのラベルリストを返す
		////////////////////////////////////////////////////////////////
		/*static public List<string> GetScheduleEntries()
		{
			List<string> entries;
			
			entries = new List<string>();

			CallbackEnumScheduleEntries callBack = delegate(mAgicReservation reservation)
			{
				entries.Add(title);
			};

			EnumScheduleEntries(callBack);

			return entries;
		}*/

		public delegate bool CallbackEnumScheduleEntries(mAgicReservation reservation);

		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::EnumScheduleEntries
		// DESCRIPT	:	mAgicTVに登録された全予約エントリを列挙する
		////////////////////////////////////////////////////////////////
		static public uint EnumScheduleEntries(CallbackEnumScheduleEntries callBack)
		{
			string filePath = GetSettingFolder() + "\\schedule.dat";
			uint n;

			int entrySize = (int)mAgicReservation.GetBytes();
			
			using (FileStream file = new FileStream(filePath, FileMode.Open))
			{

				for (n = 0; ; ++n)
				{
					uint sectionTop = (uint)(HeaderSize + n * entrySize);

					file.Seek(sectionTop, SeekOrigin.Begin);

					if (file.Position >= file.Length) break;

					byte[] tableEntry = new byte[entrySize];

					file.Read(tableEntry, 0, entrySize);	// エントリを1つ読み込む

					mAgicReservation reservation;

					reservation = new mAgicReservation(tableEntry);

					if (callBack(reservation))
					{
						//
						// 変更された予約エントリを書き戻す
						//
						file.Seek(sectionTop, SeekOrigin.Begin);
						file.Write(tableEntry, 0, entrySize);
					}

				}

			}

			return n;
		}

		////////////////////////////////////////////////////////////////
		// MODULE	:	mAgicScheduler::SetCaptureTime
		// FUNCTION	:	指定された予約の予約時間をセットする
		//
		////////////////////////////////////////////////////////////////
		static private void SetCaptureTime(string captureName, DateTime captureTime)
		{
		
		}

		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::GetCaptureFolder
		// DESCRIPT	:	mAgicTVの録画フォルダを取得する
		////////////////////////////////////////////////////////////////
		public static string GetCaptureFolder()
		{
			Microsoft.Win32.RegistryKey regKey;
			
			regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
				"Software\\I-O DATA\\Kilimanjaro\\TVmanager\\Record", false);
		
			if(regKey!=null)
			{
				return (string)regKey.GetValue( "RecordFolder" );
				
			}else{
				throw new NotImplementedException("mAgicTVのレジストリが見つかりません");
			}
		
		}

		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::GetSettingFolder
		// DESCRIPT	:	mAgicTVの設定フォルダを取得する
		////////////////////////////////////////////////////////////////
		public static string GetSettingFolder()
		{
#if DEBUG
	return "D:\\Temp";
#else
			string mAgicDataFolder;
			
			mAgicDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			mAgicDataFolder += "\\I-O DATA\\mAgicTV\\mAgicManager";

			//if (!System.IO.Directory.Exists(mAgicDataFolder))
			
			return mAgicDataFolder;
#endif
}


		public enum ByteSize { One, Two };

		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::ConvertBytesToStr
		// DESCRIPT	:	バイト列をstring形式に変換する
		////////////////////////////////////////////////////////////////
		public static string ConvertBytesToStr(byte[] bytes, ByteSize byteSize)
		{
			char[] dummyChars;
			string str;
			//1文字=1バイトの場合
			if (byteSize == ByteSize.One)
			{
				dummyChars = new char[bytes.Length];
				for (int i = 0; i < bytes.Length; i++)
				{
					dummyChars[i] = Convert.ToChar(bytes[i]);
				}
			}
			//1文字=2バイトの場合
			else
			{
				dummyChars = new char[(int)(bytes.Length / 2)];
				int allLength = 0;
				for (int i = 0; i < dummyChars.Length; i++)
				{
					dummyChars[i] = BitConverter.ToChar(bytes, allLength);
					allLength += 2;
				}
			}
			str = new String(dummyChars);
			return str;
		}	
	
	}
}
