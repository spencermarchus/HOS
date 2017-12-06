using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Web;
using System.Web.UI;
using HOS;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

public partial class Account_Login : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterHyperLink.NavigateUrl = "Register";
        OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
        var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
        if (!String.IsNullOrEmpty(returnUrl))
        {
            RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
        }
    }

    HOSDatabaseEntities dbcon = new HOSDatabaseEntities();

    protected void LogIn(object sender, EventArgs e)
    {
        if (IsValid)
        {
            //clear cookies
            Session.Clear();

            // Validate the user password
            var manager = new UserManager();
            ApplicationUser user = manager.Find(UserName.Text, Password.Text);
            if (user != null)
            {
                IdentityHelper.SignIn(manager, user, RememberMe.Checked);
                IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);

                string name = UserName.Text;

                //determine if user is type doctor or patient
                dbcon.PatientTables.Load();
                string type = "patient";

                //load list of patients & doctors
                var patients =
                    from patient in dbcon.PatientTables
                    select patient;
                //doctors
                var doctors =
                    from doctor in dbcon.DoctorTables
                    select doctor;

                //loop to check type
                foreach (PatientTable p in patients)
                {
                    if (p.PatientUserName == name)
                    {
                        //do nothing, type is already equal to patient
                        break; //exit loop
                    }
                }

                foreach (DoctorTable d in doctors)
                {
                    if (d.DoctorUserName == name)
                    {
                        type = "doctor"; //user is doctor
                        break; //exit loop
                    }
                }

                Session.Add("name", name);
                Session.Add("type", type);
                string username = (string)Session[name];
            }
            else
            {
                FailureText.Text = "Invalid username or password.";
                ErrorMessage.Visible = true;
            }

        }

    }
}