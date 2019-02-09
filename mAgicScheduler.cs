using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;

// mAgicManager�ƘA�����ė\��^����Ǘ�����

namespace magicAnime
{

	////////////////////////////////////////////////////////////////
	// CLASS	:	mAgicReservation
	// DESCRIPT	:	mAgicManager�̗\��G���g��
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

		const uint entrySize = 0x95c;			// �G���g���̃T�C�Y

		static public uint GetBytes() { return entrySize; }

		////////////////////////////////////////////////////////////////
		// MODULE	:	mAgicReservation::mAgicReservation
		// FUNCTION	:	mAgicReservation�̃R���X�g���N�^
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
	// DESCRIPT	:	�ԑg�^���\�񂷂�x�[�X�N���X
	//
	////////////////////////////////////////////////////////////////
	abstract class Scheduler
	{

		////////////////////////////////////////////////////////////////
		// MODULE	:	Scheduler::ReserveAll
		// FUNCTION	:	�w�肵���ԑg�̊e
		//
		////////////////////////////////////////////////////////////////
		public bool ReserveAll(AnimeProgram animeProgram)
		{
		
			return false;
		}

		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::MakeReservation
		// DESCRIPT	:	���Ԃ��w�肵�Ę^��\���V�K�o�^����
		// NOTE		:	�����^��ł�����ł́A���ԏd�����Ă�false��Ԃ��Ƃ͌���Ȃ��B
		//				�����^��^�C�g���ŌĂԂ�false��Ԃ��B
		// RETURN	:	true	- ����
		//			:	false	- ���s�i���ԏd�����j
		////////////////////////////////////////////////////////////////
		public abstract bool MakeReservation(string title,string tvStation,
			DateTime dateTime, uint minute, object extension);

		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::CancelReservation
		// DESCRIPT	:	�w�肳�ꂽ�\����L�����Z������
		////////////////////////////////////////////////////////////////
		public abstract void CancelReservation(string title);
	}




	////////////////////////////////////////////////////////////////
	// CLASS	:	mAgicScheduler
	// DESCRIPT	:	mAgicTV5�𗘗p���Ĕԑg�^���\�񂷂�h���N���X
	//
	////////////////////////////////////////////////////////////////
	class mAgicScheduler : Scheduler
	{

		const uint HeaderSize = 0x20;			// �w�b�_�̃T�C�Y
	

		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::MakeReservation
		// DESCRIPT	:	�\��o�^����B
		//				�e���v���[�g�̎w�肪�K�{�B�ǖ��͖��������B
		////////////////////////////////////////////////////////////////
		public override bool MakeReservation(
			string		title,			// �^��^�C�g��
			string		tvStation,		// �e���r�ǖ�
			DateTime	startDateTime,	// �^��J�n����
			uint		minute,			// �^�掞��(��)
			object		template )		// �e���v���[�g
		{

			// ���̃N���X�ŗ\�񂷂�ɂ̓e���v���[�g�̎w�肪�K�{
			if (template != null && template is mAgicReservation)
			{
				mAgicReservation newReserve;
				string managerExe;

				//
				// �e���v���[�g�����ɁA�V�����\��f�[�^���쐬����
				//

				newReserve = (mAgicReservation)((mAgicReservation)template).Clone();

				newReserve.title			= title;
				newReserve.startDateTime	= startDateTime;
				newReserve.endDateTime		= startDateTime.AddMinutes( minute );

				//
				// mAgicManager�̃X�P�W���[���f�[�^�ɃG���g����ǉ�����
				//

				managerExe = StopManager();		// �ꎞ�I��mAgicManager���~����
				
				string filePath = GetSettingFolder() + "\\schedule.dat";
				
				byte[] tableEntry = new byte[mAgicReservation.GetBytes()];

				using (FileStream file = new FileStream(filePath, FileMode.Append ))
				{
					byte[] temp = newReserve.ToBytes();
					file.Write( temp, 0, temp.GetLength(0) );
				}

				if ( managerExe != null )
					RestartManager( managerExe );	// mAgicManager���N��������

				return true;
			}

			return false;
		}

		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::CancelReservation
		////////////////////////////////////////////////////////////////
		public override void CancelReservation(
			string title)	// �L�����Z������^��^�C�g��
		{
		
		}

		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::StopManager
		// DESCRIPT	:	mAgicManager���I������
		////////////////////////////////////////////////////////////////
		static string StopManager()
		{
			System.IntPtr hWindow;
			string filePath = null;

			//
			// mAgicManager�̉B�����C���E�B���h�E���擾����
			//
			hWindow = (System.IntPtr)KernelAPI.Forms.FindWindow("mAgic�}�l�[�W��", null);

			if ( KernelAPI.Forms.IsWindow(hWindow) )
			{
				//
				// mAgicManager���I��������
				//
				Process tvManProcess;
				System.IntPtr hTvWindow;
				int processId;

				KernelAPI.Forms.GetWindowThreadProcessId( hWindow, out processId );

				tvManProcess = Process.GetProcessById(processId);

				filePath = tvManProcess.MainModule.FileName;

				// �����F
				// �\��^�撆�E�\��^�撼�O�łȂ����Ƃ��`�F�b�N����
				
				//
				// mAgicPlayer���N�����Ă���ꍇ�͏I�����邩�����Ŗ₢���킹
				//

				hTvWindow = (System.IntPtr)KernelAPI.Forms.FindWindow("mAgicTVWindow", null);
				
				if (KernelAPI.Forms.IsWindow(hTvWindow))
				{
					if ( MessageBox.Show(
						"mAgicTV���N�����Ă���Ɨ\��f�[�^���X�V�ł��܂���B\n�I�����đ����܂����H",
						"�m�F", MessageBoxButtons.YesNo,MessageBoxIcon.Question) != DialogResult.Yes)
					{
						throw new NotImplementedException(
							"�\��f�[�^�̍X�V�̓��[�U�[�ɂ��L�����Z������܂���");
					}

				}

				//
				// mAgicManager��"�I��"���j���[�̃R�}���h���b�Z�[�W�𑗐M����
				//
				KernelAPI.Forms.SendMessage(
					hWindow,KernelAPI.Forms.WM_COMMAND,
					(System.IntPtr)32772, (System.IntPtr)0);

				
				//
				// �I����ҋ@����
				//
				if (!tvManProcess.WaitForExit(3000))
				{
					throw new NotImplementedException(
						"mAgicManager�̏I���Ɏ��s���܂����B\n�\��f�[�^���X�V�ł��܂���B");
				}

			}

			return filePath;
		}

		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::RestartManager
		// DESCRIPT	:	mAgicManager���N��������
		////////////////////////////////////////////////////////////////
		static void RestartManager(string filePath)
		{
			try
			{
				Process.Start(filePath);

			}catch (Exception)
			{
				// �����N���ł��Ȃ��Ƃ����͂Ȃ�(?)
			}

		}


		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::GetScheduledEntries
		// DESCRIPT	:	mAgicTV5�̑S�\��G���g���̃��x�����X�g��Ԃ�
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
		// DESCRIPT	:	mAgicTV�ɓo�^���ꂽ�S�\��G���g����񋓂���
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

					file.Read(tableEntry, 0, entrySize);	// �G���g����1�ǂݍ���

					mAgicReservation reservation;

					reservation = new mAgicReservation(tableEntry);

					if (callBack(reservation))
					{
						//
						// �ύX���ꂽ�\��G���g���������߂�
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
		// FUNCTION	:	�w�肳�ꂽ�\��̗\�񎞊Ԃ��Z�b�g����
		//
		////////////////////////////////////////////////////////////////
		static private void SetCaptureTime(string captureName, DateTime captureTime)
		{
		
		}

		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::GetCaptureFolder
		// DESCRIPT	:	mAgicTV�̘^��t�H���_���擾����
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
				throw new NotImplementedException("mAgicTV�̃��W�X�g����������܂���");
			}
		
		}

		////////////////////////////////////////////////////////////////
		// FUNCTION	:	mAgicScheduler::GetSettingFolder
		// DESCRIPT	:	mAgicTV�̐ݒ�t�H���_���擾����
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
		// DESCRIPT	:	�o�C�g���string�`���ɕϊ�����
		////////////////////////////////////////////////////////////////
		public static string ConvertBytesToStr(byte[] bytes, ByteSize byteSize)
		{
			char[] dummyChars;
			string str;
			//1����=1�o�C�g�̏ꍇ
			if (byteSize == ByteSize.One)
			{
				dummyChars = new char[bytes.Length];
				for (int i = 0; i < bytes.Length; i++)
				{
					dummyChars[i] = Convert.ToChar(bytes[i]);
				}
			}
			//1����=2�o�C�g�̏ꍇ
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
