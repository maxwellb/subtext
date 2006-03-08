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

using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Web.Admin.WebUI;

namespace Subtext.Web.Admin.Pages
{
	/// <summary>
	/// Admin Page used to set syndication settings.
	/// </summary>
	public class Syndication : AdminOptionsPage
	{
		// abstract out at a future point for i18n
		private const string RES_SUCCESS = "Your syndication settings were successfully updated.";
		private const string RES_FAILURE = "Syndication settings update failed.";

		protected MessagePanel Messages;
		protected CheckBox chkEnableSyndication;
		protected CheckBox chkUseSyndicationCompression;
		protected CheckBox chkUseDeltaEncoding;
		protected TextBox txtLicenseUrl;
		protected Button lkbPost;
		protected AdvancedPanel Edit;
		protected HtmlImage helpImg;
		protected HtmlImage Img1;
		protected HtmlImage Img2;
		protected Subtext.Web.Controls.HelpToolTip HelpToolTip1;
		protected Subtext.Web.Controls.HelpToolTip HelpToolTip2;
		protected Subtext.Web.Controls.HelpToolTip Helptooltip4;
		protected Subtext.Web.Controls.HelpToolTip HelpToolTip3;

		protected HtmlImage Img3;
		
		private void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				PopulateForm();
			}
			ManageHiddenSettings();
		}
		
		private void PopulateForm()
		{
			BlogInfo info = Config.CurrentBlog;
			
			this.chkEnableSyndication.Checked = info.IsAggregated;
			this.chkUseDeltaEncoding.Checked = info.RFC3229DeltaEncodingEnabled;
			this.chkUseSyndicationCompression.Checked = info.UseSyndicationCompression;
			this.txtLicenseUrl.Text = info.LicenseUrl;
		}

		private void ManageHiddenSettings()
		{
			this.chkEnableSyndication.Attributes["onclick"] = "toggleHideOnCheckbox(this, 'otherSettings');";
	
			string startupScript = "<script type=\"text/javascript\">"
				+  Environment.NewLine + "var checkbox = document.getElementById('" + this.chkEnableSyndication.ClientID + "');"
				+  Environment.NewLine + " toggleHideOnCheckbox(checkbox, 'otherSettings');"
				+  Environment.NewLine +  "</script>";
	
			Page.RegisterStartupScript("startupScript", startupScript);
		}

		private void SaveSettings()
		{
			try
			{
				UpdateConfiguration();
				this.Messages.ShowMessage(RES_SUCCESS);
			}
			catch(Exception ex)
			{
				this.Messages.ShowError(String.Format(Constants.RES_EXCEPTION, RES_FAILURE, ex.Message));
			}
		}

		private void UpdateConfiguration()
		{
			BlogInfo info = Config.CurrentBlog;
			
			info.IsAggregated = this.chkEnableSyndication.Checked;
			info.UseSyndicationCompression = this.chkUseSyndicationCompression.Checked;
			info.RFC3229DeltaEncodingEnabled = this.chkUseDeltaEncoding.Checked;
			info.LicenseUrl = this.txtLicenseUrl.Text;

			Config.UpdateConfigData(info);
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.lkbPost.Click += new System.EventHandler(this.lkbPost_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void lkbPost_Click(object sender, EventArgs e)
		{
			SaveSettings();
		}
	}
}