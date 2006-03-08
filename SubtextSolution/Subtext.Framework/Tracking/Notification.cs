#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

#region Notes
 ///////////////////////////////////////////////////////////////////////////////////////////////////
 // The code in this file is freely distributable.
 // 
 // ASPNetWeblog isnot responsible for, shall have no liability for 
 // and disclaims all warranties whatsoever, expressed or implied, related to this code,
 // including without limitation any warranties related to performance, security, stability,
 // or non-infringement of title of the control.
 // 
 // If you have any questions, comments or concerns, please contact
 // Scott Watermasysk, Scott@TripleASP.Net.
 // 
 // For more information on this control, updates, and other tools to integrate blogging 
 // into your existing applications, please visit, http://aspnetweblog.com
 // 
 ///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Specialized;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Util;

namespace Subtext.Framework.Tracking
{
	/// <summary>
	/// Summary description for Notification.
	/// </summary>
	public class NotificationServices
	{
		public static void Run(Entry entry)
		{
			if(entry != null)
			{
				Notifier notify = new Notifier();

				notify.FullyQualifiedUrl = Config.CurrentBlog.RootUrl;
				notify.BlogName = Config.CurrentBlog.Title;

				notify.Title = entry.Title;

				notify.PostUrl = entry.Link;

				if(entry.HasDescription)
				{
					notify.Description = entry.Description;
				}
				else
				{
					entry.Description = entry.Title;	
				}

				notify.Text = entry.Body;
				
				//This could take a while, do it on another thread
//				Thread thread = new Thread(new ThreadStart(notify.Notify));
//				thread.Start();
				ManagedThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(notify.Notify));
			}
		}

	}

	public class Notifier
	{
		public Notifier(){}

		private string description;
		public string Description
		{
			get{return description;}
			set{description = value;}
		}

		private string blogName;
		public string BlogName
		{
			get{return blogName;}
			set{blogName = value;}
		}

		private string title;
		public string Title
		{
			get{return title;}
			set{title = value;}
		}

		private string fullyQualifiedUrl;
		public string FullyQualifiedUrl
		{
			get{return fullyQualifiedUrl;}
			set{fullyQualifiedUrl = value;}
		}

		private string postUrl;
		public string PostUrl
		{
			get{return postUrl;}
			set{postUrl = value;}
		}

		private string text;
		public string Text
		{
			get{return text;}
			set{text = value;}
		}


		public void Notify(object state)
		{
			Notify();
		}

		public void Notify()
		{
			Configuration.Tracking track= Config.Settings.Tracking;
			//First, ping weblogs.com
			try
			{
				
				if(track.PingWeblogs)
				{
					WeblogsNotificatinProxy weblogs = new WeblogsNotificatinProxy();
					weblogs.Ping(BlogName,FullyQualifiedUrl);
					weblogs.Dispose();
				}
			}
			catch{}

			//Get the links from the last post
			StringCollection links = TrackHelpers.GetLinks(Text);

			//Do we have links
			if(links != null && links.Count > 0)
			{
				int count = links.Count;
				//Create our notification Components
				PingBackNotificatinProxy pbnp = new PingBackNotificatinProxy();
				TrackBackNotificationProxy tbnp = new TrackBackNotificationProxy();

				//for each link, try to pingback and/or trackback
				for(int i = 0; i<count; i++)
				{
					string link = links[i];
					//get the page text
					string pageText = BlogRequest.GetPageText(link);
					if(pageText != null)
					{
						bool success = false;
						try
						{
							if(track.EnableTrackBacks)
							{
								success = tbnp.TrackBackPing(pageText,link,Title,PostUrl,BlogName,Description);
							}
			
							if(!success && track.EnablePingBacks)
							{
								pbnp.Ping(pageText,PostUrl,link);						
							}
							
						
						}
						catch(System.Exception exp)
						{
							string message  = exp.Message;
							message += "s";
							//Lots can go wrong here. Just eat the exception and keep going
						}
					}
				}
			}
			
		}
	}
}
