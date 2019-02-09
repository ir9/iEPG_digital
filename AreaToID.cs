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
				return 0;	// 全ての番組
			return INSTANCE[area];
		}

		public static readonly PlatformToID INSTANCE = new PlatformToID();
		private PlatformToID()
		{
			this["すべての番組"]   = 0;
			this["地上波"]         = 1;
			this["BSデジタル"]     = 2;
			this["スカパー！ｅ２"] = 5;
			this["スカパー！"]     = 4;
		}
	}

	class CategoryToID : Dictionary<string, int>
	{
		static public int get(string area)
		{
			if(!INSTANCE.ContainsKey(area))
				return 0;	// 全ての番組
			return INSTANCE[area];
		}

		public static readonly CategoryToID INSTANCE = new CategoryToID();
		private CategoryToID()
		{
			this["選択なし"]           = -1;
			this["ニュース／報道"]     = 100000;
			this["スポーツ"]           = 101000;
			this["情報／ワイドショー"] = 102000;
			this["ドラマ"]             = 103000;
			this["音楽"]               = 104000;
			this["バラエティー"]       = 105000;
			this["映画"]               = 106000;
			this["アニメ／特撮"]       = 107000;
			this["ドキュメンタリー／教養"] = 108000;
			this["劇場／公演"]         = 109000;
			this["趣味／教育"]         = 110000;
			this["福祉"]               = 111000;
			this["その他"]             = 115000;
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
			this["北海道（札幌）"] = 10;
			this["CATV北海道（札幌）エリア"] = 110;
			this["北海道（函館）"] = 11;
			this["北海道（旭川）"] = 12;
			this["北海道（帯広）"] = 13;
			this["北海道（釧路）"] = 14;
			this["北海道（北見）"] = 15;
			this["北海道（室蘭）"] = 16;
			this["青森"] = 22;
			this["岩手"] = 20;
			this["宮城"] = 17;
			this["CATV宮城エリア"] = 117;
			this["秋田"] = 18;
			this["山形"] = 19;
			this["福島"] = 21;
			this["茨城"] = 26;
			this["CATV茨城エリア"] = 126;
			this["栃木"] = 28;
			this["群馬"] = 25;
			this["CATV群馬エリア"] = 125;
			this["埼玉"] = 29;
			this["CATV埼玉エリア"] = 129;
			this["千葉"] = 27;
			this["CATV千葉エリア"] = 127;
			this["東京"] = 23;
			this["CATV東京エリア"] = 123;
			this["神奈川"] = 24;
			this["CATV神奈川エリア"] = 124;
			this["新潟"] = 31;
			this["山梨"] = 32;
			this["長野"] = 30;
			this["富山"] = 37;
			this["石川"] = 34;
			this["福井"] = 36;
			this["岐阜"] = 39;
			this["静岡"] = 35;
			this["愛知"] = 33;
			this["三重"] = 38;
			this["滋賀"] = 45;
			this["京都"] = 41;
			this["CATV京都エリア"] = 141;
			this["大阪"] = 40;
			this["CATV大阪エリア"] = 140;
			this["兵庫"] = 42;
			this["CATV兵庫エリア"] = 142;
			this["奈良"] = 44;
			this["和歌山"] = 43;
			this["CATV和歌山エリア"] = 143;
			this["鳥取"] = 49;
			this["島根"] = 48;
			this["岡山"] = 47;
			this["広島"] = 46;
			this["山口"] = 50;
			this["CATV山口エリア"] = 150;
			this["徳島"] = 53;
			this["香川"] = 52;
			this["愛媛"] = 51;
			this["高知"] = 54;
			this["福岡"] = 55;
			this["CATV福岡エリア"] = 155;
			this["佐賀"] = 61;
			this["長崎"] = 57;
			this["熊本"] = 56;
			this["大分"] = 60;
			this["宮崎"] = 59;
			this["鹿児島"] = 58;
			this["沖縄"] = 62;
			this["Nのフィールド"] = 23;
		}
	}
}
