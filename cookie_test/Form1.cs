using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Net;
using System.Web;
using System.IO;
using System.Windows.Forms;

namespace cookie_test
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			string         url     = "http://www.yahoo.co.jp";
			WebRequest     req     = WebRequest.Create(url);
			HttpWebRequest httpReq = (HttpWebRequest)req;
			httpReq.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; MyIE2; Maxthon; .NET CLR 1.1.4322)";
			httpReq.Accept    = "Accept: image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight, */*";

			httpReq.CookieContainer = new CookieContainer();

			WebResponse  res = req.GetResponse();
			StreamReader r = new StreamReader(res.GetResponseStream());
			string a = r.ReadToEnd();

			HttpWebResponse httpRes = (HttpWebResponse)res;


			CookieCollection coll = httpReq.CookieContainer.GetCookies(new Uri(url));
			o(coll.Count.ToString());
			foreach(Cookie cookie in httpRes.Cookies)
			{
				o(cookie.ToString());
			}

			r.Close();
			res.Close();
		}


		private static void o(object s) {
			System.Console.Out.WriteLine(s);
		}

		private void https_Click(object sender, EventArgs e)
		{
			string     url = "https://myepg.so-net.ne.jp/member/pc/lis/lisLoginComplete.action";
			WebRequest req = WebRequest.Create(url);

			string sendData = "mailAddress=test01@ir9.jp&password";
			
			
			WebResponse res = req.GetResponse();

			string f = (new StreamReader(res.GetResponseStream())).ReadToEnd();

			o(f.Length);
			res.Close();
		}
	}
}


