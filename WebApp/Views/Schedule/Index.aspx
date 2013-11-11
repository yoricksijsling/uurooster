<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<OsirisClient.Schedule>" %>
<!DOCTYPE html>
<html>
<head runat="server">
	<title></title>
	<link href="<%= Url.Content("~/Content/Style.css") %>" rel="stylesheet" type="text/css" />
</head>
<body>
	<div class="topbar">
		Rooster op
		<%: Html.ActionLink(String.Format("{0} {1}", Model.CreatedAtCET.ToShortDateString(), Model.CreatedAtCET.ToShortTimeString()), "Reload", "Schedule") %> | 
		<%: Html.ActionLink("Log uit", "Logout", "Session") %>
	</div>
	<% foreach (var month in Model.Months) { %>
		<div class="month">
			<div class="monthname"><%: month.Name %></div>
			<div class="day-list">
				<% foreach (var day in month) { %>
					<div class="day">
						<div class="dayindicator"><%: day.DayOfWeek %> <%: day.DayOfMonth %></div>
						<div class="row-list">
							<% foreach (var row in day) { %>
								<div class="row <%: row.Highlight ? "highlight" : ""%>">
									<div>
										<div class="time"><%: row.Time.Replace("-", " - ") %></div>
										<div class="location"><%: row.Building %> <%: row.Room %></div>
									</div>
									<div class="course"><%: row.Course %> <%: row.Kind %></div>
								</div>
							<% } %>
						</div>
					</div>
				<% } %>
			</div>
		</div>
	<% } %>
	
	<script type="text/javascript">
		if (window.localStorage) {
			localStorage.setItem("cookie", document.cookie);
		}
	</script>
</body>
	