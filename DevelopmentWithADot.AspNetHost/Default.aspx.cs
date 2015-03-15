using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DevelopmentWithADot.ApplicationHost
{
	public partial class Default : Page
	{
		protected Label label;

		protected override void OnLoad(EventArgs e)
		{
			this.label.Text = DateTime.Now.ToString();

			base.OnLoad(e);
		}
	}
}