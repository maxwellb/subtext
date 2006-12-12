using System;
using System.Web.Security;

namespace Subtext.Web.HostAdmin
{
	public partial class Login : System.Web.UI.Page
	{
		///<summary>
		///Raises the <see cref="E:System.Web.UI.Control.Init"></see> event to initialize the page.
		///</summary>
		///<param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnInit(EventArgs e)
		{
			//KLUDGE: We shouldn't need to do this.
			Membership.ApplicationName = Roles.ApplicationName = "/";
			Master.DestinationUrl = "~/HostAdmin/";
			base.OnInit(e);
		}
	}
}