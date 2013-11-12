<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<!DOCTYPE html>
<html>
<head runat="server">
	<title></title>
</head>
<body>
	Bezig met uitloggen...
	<script type="text/javascript">
		if (window.localStorage) {
			localStorage.removeItem("cookie");
		}
		window.location.href = "<%= Url.Action("Index", "Schedule")%>";
	</script>
</body>
</html>
