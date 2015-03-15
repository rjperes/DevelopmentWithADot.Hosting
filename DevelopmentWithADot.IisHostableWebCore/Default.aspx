<%@ Page Language="C#" %>
<!DOCTYPE html>
<html>
<head runat="server">
	<title></title>
</head>
<body>
	<script runat="server" language="C#">

		protected override void OnLoad(EventArgs e)
		{
			this.label.Text = "AQUI";
		}

	</script>
	<h1>Hello, World!</h1>
	<form runat="server">
		<asp:Label runat="server" ID="label"/>
	</form>
</body>
</html>
