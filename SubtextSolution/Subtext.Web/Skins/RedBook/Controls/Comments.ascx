<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Comments" %>
<hr/>
<a name="feedback" />
<div id="moreinfo">
	<h2>Feedback</h2>
	<p><asp:Literal ID = "NoCommentMessage" Runat ="server" /></p>
	<asp:Repeater id="CommentList" runat="server" OnItemCreated="CommentsCreated" OnItemCommand="RemoveComment_ItemCommand">
		<ItemTemplate>
			<div class="CommentEntry">
					<h4>
						<asp:Literal Runat = "server" ID = "Title" />
					</h4>
					<asp:Literal id = "PostText" Runat = "server" />
					<span><asp:Literal id = "PostDate" Runat = "server" /> | <cite><asp:HyperLink Target="_blank" Runat="server" ID="NameLink" /></cite></span>
					<asp:LinkButton Runat="server" ID="EditLink" CausesValidation="False" />
			</div>
		</ItemTemplate>
	</asp:Repeater>
</div>
<hr/>