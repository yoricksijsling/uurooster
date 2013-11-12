<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<!DOCTYPE html>
<html>
<head runat="server">
	<title></title>
	<link href="<%= Url.Content("~/Content/Style.css") %>" rel="stylesheet" type="text/css" />
</head>
<body>
	<h1>UU Rooster</h1>
	<p>Om je rooster van <a href="https://www.osiris.universiteitutrecht.nl/">OSIRIS</a> te halen, zijn je inloggegevens nodig. Deze worden bewaard zolang je ingelogd bent.</p>
	<% using (Html.BeginForm("HandleLogin", "Session")) { %>
		<label for="username">Studentnummer</label> <%= Html.TextBox("username", null, new { autofocus="autofocus" }) %><br/>
		<label for="password">SOLIS-wachtwoord</label> <%= Html.Password("password") %><br/>
		<input type="submit" value="Log in" />
	<% } %>
	
	<p>LET OP: Deze site is niet verbonden aan de universiteit. Dit is een eigen initiatief, en je moet voor jezelf bepalen of je mij je inloggegevens toevertrouwd. Geeft nooit zonder nadenken je inloggegevens aan derden.</p> 
	<p><a href="https://github.com/yoricksijsling/uurooster">Open source</a></p>
	
	<script type="text/javascript">
		if (window.localStorage) {
			var cookie = localStorage.getItem("cookie");
			if (cookie) {
				localStorage.removeItem("cookie");
				if (document.cookie != cookie) {
					document.cookie = cookie;
					window.location.reload();
				}
			}
		}
	</script>
</body>
</html>
