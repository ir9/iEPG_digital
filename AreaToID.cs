using System;
using System.Collections.Generic;
using System.Text;

namespace mAgicTvPlugIn
{
	class PlatformToID : Dictionary<string, int>
	{
		static public int get(string area)
		{
			if(!INSTANCE.ContainsKey(area))
				return 0;	// �S�Ă̔ԑg
			return INSTANCE[area];
		}

		public static readonly PlatformToID INSTANCE = new PlatformToID();
		private PlatformToID()
		{
			this["���ׂĂ̔ԑg"]   = 0;
			this["�n��g"]         = 1;
			this["BS�f�W�^��"]     = 2;
			this["�X�J�p�[�I���Q"] = 5;
			this["�X�J�p�[�I"]     = 4;
		}
	}

	class CategoryToID : Dictionary<string, int>
	{
		static public int get(string area)
		{
			if(!INSTANCE.ContainsKey(area))
				return 0;	// �S�Ă̔ԑg
			return INSTANCE[area];
		}

		public static readonly CategoryToID INSTANCE = new CategoryToID();
		private CategoryToID()
		{
			this["�I���Ȃ�"]           = -1;
			this["�j���[�X�^��"]     = 100000;
			this["�X�|�[�c"]           = 101000;
			this["���^���C�h�V���["] = 102000;
			this["�h���}"]             = 103000;
			this["���y"]               = 104000;
			this["�o���G�e�B�["]       = 105000;
			this["�f��"]               = 106000;
			this["�A�j���^���B"]       = 107000;
			this["�h�L�������^���[�^���{"] = 108000;
			this["����^����"]         = 109000;
			this["��^����"]         = 110000;
			this["����"]               = 111000;
			this["���̑�"]             = 115000;
		}
	}

	class AreaToID : Dictionary<string, int>
	{
		static public int get(string area)
		{
			if(!INSTANCE.ContainsKey(area))
				return 23;	// TOKYO
			return INSTANCE[area];
		}

		public static readonly AreaToID INSTANCE = new AreaToID();
		private AreaToID()
		{
			this["�k�C���i�D�y�j"] = 10;
			this["CATV�k�C���i�D�y�j�G���A"] = 110;
			this["�k�C���i���فj"] = 11;
			this["�k�C���i����j"] = 12;
			this["�k�C���i�эL�j"] = 13;
			this["�k�C���i���H�j"] = 14;
			this["�k�C���i�k���j"] = 15;
			this["�k�C���i�����j"] = 16;
			this["�X"] = 22;
			this["���"] = 20;
			this["�{��"] = 17;
			this["CATV�{��G���A"] = 117;
			this["�H�c"] = 18;
			this["�R�`"] = 19;
			this["����"] = 21;
			this["���"] = 26;
			this["CATV���G���A"] = 126;
			this["�Ȗ�"] = 28;
			this["�Q�n"] = 25;
			this["CATV�Q�n�G���A"] = 125;
			this["���"] = 29;
			this["CATV��ʃG���A"] = 129;
			this["��t"] = 27;
			this["CATV��t�G���A"] = 127;
			this["����"] = 23;
			this["CATV�����G���A"] = 123;
			this["�_�ސ�"] = 24;
			this["CATV�_�ސ�G���A"] = 124;
			this["�V��"] = 31;
			this["�R��"] = 32;
			this["����"] = 30;
			this["�x�R"] = 37;
			this["�ΐ�"] = 34;
			this["����"] = 36;
			this["��"] = 39;
			this["�É�"] = 35;
			this["���m"] = 33;
			this["�O�d"] = 38;
			this["����"] = 45;
			this["���s"] = 41;
			this["CATV���s�G���A"] = 141;
			this["���"] = 40;
			this["CATV���G���A"] = 140;
			this["����"] = 42;
			this["CATV���ɃG���A"] = 142;
			this["�ޗ�"] = 44;
			this["�a�̎R"] = 43;
			this["CATV�a�̎R�G���A"] = 143;
			this["����"] = 49;
			this["����"] = 48;
			this["���R"] = 47;
			this["�L��"] = 46;
			this["�R��"] = 50;
			this["CATV�R���G���A"] = 150;
			this["����"] = 53;
			this["����"] = 52;
			this["���Q"] = 51;
			this["���m"] = 54;
			this["����"] = 55;
			this["CATV�����G���A"] = 155;
			this["����"] = 61;
			this["����"] = 57;
			this["�F�{"] = 56;
			this["�啪"] = 60;
			this["�{��"] = 59;
			this["������"] = 58;
			this["����"] = 62;
			this["N�̃t�B�[���h"] = 23;
		}
	}
}
