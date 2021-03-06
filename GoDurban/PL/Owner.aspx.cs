using GoDurban.BL;
using GoDurban.Models;
using GoDurban.SMSService;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class Owner : System.Web.UI.Page
    {
        private GoDurbanEntities db = new GoDurbanEntities();

        string currentDate = DateTime.Now.ToString("dd MMM yyyy");
        public static int accID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null || string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                Response.Redirect("~/Index.aspx?ShowDialog=yes");
            }
            else
            {
                string type = Session["UserRole"].ToString();

                if (type != "Admin" && type != "User" && type != "Supervisor")
                {
                    Response.Redirect("~/Error.aspx");
                }
            }

            if (ddlStatus.SelectedItem.Text == "Approved")
            {
                divReason.Style.Add("display", "none");
                divReason1.Style.Add("display", "none");
            }
            else
            {
                divReason.Style.Add("display", "inline");
                divReason1.Style.Add("display", "inline");
            }

            txtAccNo.Style.Add("border", "");
            lblError.Visible = false;
            RemoveBorder();

            if (!IsPostBack)
            {
                LoadGender();
                LoadRace();
                LoadData();
                LoadReason();
                LoadBanks();
                LoadBranches();
                LoadStatusAll();
                //RemoveDuplicateItems(ddlStatus);
            }
        }

        public void RemoveDuplicateItems(DropDownList ddl)
        {
            for (int i = 0; i < ddl.Items.Count; i++)
            {
                ddl.SelectedIndex = i;
                string str = ddl.SelectedItem.ToString();
                for (int counter = i + 1; counter < ddl.Items.Count; counter++)
                {
                    ddl.SelectedIndex = counter;
                    string compareStr = ddl.SelectedItem.ToString();
                    if (str == compareStr)
                    {
                        ddl.Items.RemoveAt(counter);
                        counter = counter - 1;
                    }
                }
            }
        }

        public void DisableForm(ControlCollection ctrls)
        {
            foreach (Control ctrl in ctrls)
            {
                if (ctrl is TextBox)
                    ((TextBox)ctrl).Enabled = false;
                if (ctrl is Button)
                    ((Button)ctrl).Enabled = false;
                else if (ctrl is DropDownList)
                    ((DropDownList)ctrl).Enabled = false;
                else if (ctrl is CheckBox)
                    ((CheckBox)ctrl).Enabled = false;
                else if (ctrl is RadioButton)
                    ((RadioButton)ctrl).Enabled = false;
                else if (ctrl is RadioButtonList)
                    ((RadioButtonList)ctrl).Enabled = false;
                else if (ctrl is LinkButton)
                    ((LinkButton)ctrl).Enabled = false;
                else if (ctrl is FileUpload)
                    ((FileUpload)ctrl).Enabled = false;
                else if (ctrl is GridView)
                    ((GridView)ctrl).Enabled = false;

                DisableForm(ctrl.Controls);
            }
        }

        public void LoadBanks()
        {
            BankBL BL = new BankBL();
            ddlBank.DataSource = BL.LoadBanks();
            ddlBank.DataTextField = "BankName";
            ddlBank.DataValueField = "BankID";
            ddlBank.DataBind();
        }

        public void LoadBranches()
        {
            BankBL BL = new BankBL();
            ddlBranch.DataSource = BL.LoadBranchs();
            ddlBranch.DataTextField = "BranchCode";
            ddlBranch.DataValueField = "BankID";
            ddlBranch.DataBind();
        }

        public void LoadStatusAll()
        {
            StatusBL BL = new StatusBL();
            ddlStatus.DataSource = BL.LoadStatus();
            ddlStatus.DataTextField = "StatusDescription";
            ddlStatus.DataValueField = "StatusID";
            ddlStatus.DataBind();
            ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Pending"));
        }

        public void LoadStatusNotAll()
        {
            StatusBL BL = new StatusBL();
            ddlStatus.DataSource = BL.LoadStatusNotAll();
            ddlStatus.DataTextField = "StatusDescription";
            ddlStatus.DataValueField = "StatusID";
            ddlStatus.DataBind();
            ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Pending"));
        }

        //public DataTable GetCertainStatuses()
        //{
        //    using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
        //    using (var cmd = new SqlCommand("SELECT StatusID, StatusDescription FROM tbl_Status WHERE StatusDescription <>'Approved'", conn))
        //    using (var adapter = new SqlDataAdapter(cmd))
        //    {
        //        var products = new DataTable();
        //        adapter.Fill(products);
        //        return products;
        //    }
        //}

        public void LoadReason()
        {
            ReasonBL BL = new ReasonBL();
            ddlReason.DataSource = BL.LoadReason();
            ddlReason.DataTextField = "ReasonDescription";
            ddlReason.DataValueField = "ReasonID";
            ddlReason.DataBind();
        }

        public void LoadGender()
        {
            GenderBL BL = new GenderBL();
            ddlGender.DataSource = BL.LoadGender();
            ddlGender.DataTextField = "GenderDescription";
            ddlGender.DataValueField = "GenderID";
            ddlGender.DataBind();
        }

        public void LoadRace()
        {
            RaceBL BL = new RaceBL();
            ddlRace.DataSource = BL.LoadRace();
            ddlRace.DataTextField = "RaceDescription";
            ddlRace.DataValueField = "RaceID";
            ddlRace.DataBind();
        }

        protected void RemoveBorder()
        {
            txtName.Style.Add("border", "");
            txtSurname.Style.Add("border", "");
            txtIDNo.Style.Add("border", "");
            txtCellNo.Style.Add("border", "");
            txtOfficeNo.Style.Add("border", "");
            txtEmail.Style.Add("border", "");
            txtStreetNo.Style.Add("border", "");
            txtSuburb.Style.Add("border", "");
            txtCity.Style.Add("border", "");
            txtCode.Style.Add("border", "");
            txtCO.Style.Add("border", "");
            ddlRace.Style.Add("border", "");
            ddlGender.Style.Add("border", "");
            ddlStatus.Style.Add("border", "");
            ddlReason.Style.Add("border", "");
            chkTermsConditions.Style.Add("border", "");
            chkReviewContract.Style.Add("border", "");
            txtAccNo.Style.Add("border", "");
            ddlBank.Style.Add("border", "");
            ddlBranch.Style.Add("border", "");

            divwarning1.Style.Add("display", "none");
        }

        private bool ValidateOwner()
        {
            bool valid = true;

            divsuccess.Style.Add("display", "none");
            divdanger.Style.Add("display", "none");
            divinfo.Style.Add("display", "none");
            divwarning.Style.Add("display", "none");

            if ((txtName.Text == string.Empty))
            {
                txtName.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtName.Style.Add("border", "");
            }
            if ((txtStreetNo.Text == string.Empty))
            {
                txtStreetNo.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtStreetNo.Style.Add("border", "");
            }
            if ((txtSuburb.Text == string.Empty))
            {
                txtSuburb.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtSuburb.Style.Add("border", "");
            }
            if ((txtCity.Text == string.Empty))
            {
                txtCity.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtCity.Style.Add("border", "");
            }
            if ((txtCode.Text == string.Empty))
            {
                txtCode.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtCode.Style.Add("border", "");
            }
            if ((txtSurname.Text == string.Empty))
            {
                txtSurname.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtSurname.Style.Add("border", "");
            }
            if ((txtIDNo.Text == string.Empty))
            {
                txtIDNo.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtIDNo.Style.Add("border", "");
            }

            if ((txtCellNo.Text == string.Empty))
            {
                txtCellNo.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtCellNo.Style.Add("border", "");
            }

            if ((txtAccNo.Text == string.Empty))
            {
                txtAccNo.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtAccNo.Style.Add("border", "");
            }

            if ((ddlBank.SelectedValue == "0"))
            {
                ddlBank.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                ddlBank.Style.Add("border", "");
            }

            if ((ddlBranch.SelectedValue == "0"))
            {
                ddlBranch.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                ddlBranch.Style.Add("border", "");
            }

            if ((ddlGender.SelectedValue == "0"))
            {
                ddlGender.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                ddlGender.Style.Add("border", "");
            }

            if ((ddlRace.SelectedValue == "0"))
            {
                ddlRace.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                ddlRace.Style.Add("border", "");
            }

            if ((ddlStatus.SelectedValue == "0"))
            {
                ddlStatus.Style.Add("border", "1px solid red");
                valid = false;
            }
            else if (ddlStatus.SelectedItem.Text == "Approved")
            {
                ddlStatus.Style.Add("border", "");
                ddlReason.Style.Add("border", "");

                divReason.Style.Add("display", "none");
                divReason1.Style.Add("display", "none");
            }
            else if ((ddlStatus.SelectedItem.Text == "Inactive") || (ddlStatus.SelectedItem.Text == "Cancelled") || (ddlStatus.SelectedItem.Text == "Pending"))
            {
                if ((ddlReason.SelectedValue == "0"))
                {
                    ddlReason.Style.Add("border", "1px solid red");
                    valid = false;
                }
                else
                {
                    ddlReason.Style.Add("border", "");
                }
            }
            else
            {
                ddlStatus.Style.Add("border", "");
                ddlReason.Style.Add("border", "");
            }

            if ((!chkReviewContract.Checked))
            {
                chkReviewContract.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                chkReviewContract.Style.Add("border", "");
            }
            if ((!chkTermsConditions.Checked))
            {
                chkTermsConditions.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                chkTermsConditions.Style.Add("border", "");
            }

            //var acc = db.Accounts.ToList().FindAll(x => String.Compare(x.AccountNo, (string)txtAccNo.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);
            //if (acc.Count > 0 || string.IsNullOrWhiteSpace(txtAccNo.Text))
            //{
            //    divsuccess.Style.Add("display", "none");
            //    divdanger.Style.Add("display", "none");
            //    divinfo.Style.Add("display", "none");
            //    divwarning.Style.Add("display", "none");
            //    divwarning1.Style.Add("display", "inline");

            //    //txtAccNo.Text = string.Empty;
            //    txtAccNo.Focus();

            //    txtAccNo.Style.Add("border", "1px solid red");
            //    valid = false;
            //}
            //else
            //{
            //    divsuccess.Style.Add("display", "none");
            //    divdanger.Style.Add("display", "none");
            //    divinfo.Style.Add("display", "none");
            //    divwarning.Style.Add("display", "none");
            //    divwarning1.Style.Add("display", "none");

            //    txtAccNo.Style.Add("border", "");
            //}

            return valid;
        }

        protected void GetPrimaryKey()
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;

            SqlConnection csdb = new SqlConnection(constr);

            //SqlDataAdapter date = new SqlDataAdapter("SELECT max(AccountID)  FROM[Account]", csdb);
            //SqlDataAdapter adap = new SqlDataAdapter("SELECT AccountID  FROM[Account] where AccountNo = '"+txtAccNo.Text+"' )", csdb);
            SqlDataAdapter adap = new SqlDataAdapter("SELECT AccountID FROM [Account] WHERE (AccountNo LIKE'" + txtAccNo.Text + "')", csdb);

            //  SqlDataAdapter adap = new SqlDataAdapter("SELECT max(ApplicationID) as ApplicationID FROM Application", csdb);
            DataSet MyDataSet = new DataSet();

            adap.Fill(MyDataSet);
            int vall = 0;
            for (int i = 0; i < MyDataSet.Tables[0].Rows.Count; i++)
            {
                vall = Convert.ToInt16(MyDataSet.Tables[0].Rows[i]["AccountID"].ToString());
            }
            accID = vall;
        }

        private void LoadData()
        {
            var list = (from a in db.tbl_Owner
                        join b in db.tbl_Gender on a.GenderID equals b.GenderID
                        join d in db.tbl_Race on a.RaceID equals d.RaceID
                        join e in db.tbl_Status on a.StatusID equals e.StatusID
                        join f in db.Accounts on a.AccountID equals f.AccountID
                        join c in db.Banks on f.BankID equals c.BankID
                        //join f in db.tbl_Reason on a.ReasonID equals f.ReasonID

                        select new
                        {
                            a.OwnerID,
                            a.AddressCity,
                            a.AddressCode,
                            a.AddressStreet,
                            a.AddressSuburb,
                            a.CalcBirthDate,
                            a.CellNo,
                            a.Email,
                            b.GenderDescription,
                            a.IDNo,
                            a.Name,
                            a.OfficeNo,
                            d.RaceDescription,
                            a.Surname,
                            a.CompanyName,
                            e.StatusDescription,
                            //f.ReasonDescription,
                            c.BranchCode,
                            c.BankName,
                            f.AccountID,
                            f.AccountNo
                        });
            gvOwner.DataSource = list.ToList().OrderByDescending(x => x.OwnerID);
            gvOwner.DataBind();
        }

        public void Clear()
        {
            txtName.Text = string.Empty;
            txtSurname.Text = string.Empty;
            txtIDNo.Text = string.Empty;
            txtCellNo.Text = string.Empty;
            txtOfficeNo.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtStreetNo.Text = string.Empty;
            txtSuburb.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtCode.Text = string.Empty;
            txtCO.Text = string.Empty;
            ddlRace.SelectedValue = "0";
            ddlGender.SelectedValue = "0";
            ddlStatus.SelectedItem.Text = "Pending";
            ddlReason.SelectedValue = "0";
            chkReviewContract.Checked = false;
            chkTermsConditions.Checked = false;
            txtAccNo.Text = string.Empty;
            ddlBank.SelectedValue = "0";
            ddlBranch.SelectedValue = "0";
        }

        public string GenerateLineNo()
        {
            int NumberofDrivers = db.Payments.ToList().Count;
            string refno = "LN";
            if (NumberofDrivers < 10)
            {
                int NewNumberofDrivers = NumberofDrivers + 1;
                if (NewNumberofDrivers < 10)
                {
                    refno += "000" + NewNumberofDrivers;
                }
                else
                {
                    refno += "00" + NewNumberofDrivers;
                }
            }
            else if (NumberofDrivers < 100)
            {
                int NewNumberofProjects = NumberofDrivers + 1;
                if (NewNumberofProjects < 10)
                {
                    refno += "000" + NewNumberofProjects;
                }
                else
                {
                    refno += "00" + NewNumberofProjects;
                }
            }
            else
            {
                int NewNumberofProjects = NumberofDrivers + 1;
                refno += NewNumberofProjects;
            }
            return refno;
        }

        public string GenerateBatchNo()
        {
            int NumberofDrivers = db.Payments.ToList().Count;
            string refno = "BN";
            if (NumberofDrivers < 10)
            {
                int NewNumberofDrivers = NumberofDrivers + 1;
                if (NewNumberofDrivers < 10)
                {
                    refno += "000" + NewNumberofDrivers;
                }
                else
                {
                    refno += "00" + NewNumberofDrivers;
                }
            }
            else if (NumberofDrivers < 100)
            {
                int NewNumberofProjects = NumberofDrivers + 1;
                if (NewNumberofProjects < 10)
                {
                    refno += "000" + NewNumberofProjects;
                }
                else
                {
                    refno += "00" + NewNumberofProjects;
                }
            }
            else
            {
                int NewNumberofProjects = NumberofDrivers + 1;
                refno += NewNumberofProjects;
            }
            return refno;
        }

        public string GenerateDocumentNo()
        {
            int NumberofDrivers = db.Payments.ToList().Count;
            string refno = "DN";
            if (NumberofDrivers < 10)
            {
                int NewNumberofDrivers = NumberofDrivers + 1;
                if (NewNumberofDrivers < 10)
                {
                    refno += "000" + NewNumberofDrivers;
                }
                else
                {
                    refno += "00" + NewNumberofDrivers;
                }
            }
            else if (NumberofDrivers < 100)
            {
                int NewNumberofProjects = NumberofDrivers + 1;
                if (NewNumberofProjects < 10)
                {
                    refno += "000" + NewNumberofProjects;
                }
                else
                {
                    refno += "00" + NewNumberofProjects;
                }
            }
            else
            {
                int NewNumberofProjects = NumberofDrivers + 1;
                refno += NewNumberofProjects;
            }
            return refno;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateOwner())
                {
                    OwnerBL BL = new OwnerBL();
                    tbl_Owner TBL = new tbl_Owner();

                    AccountBL aBL = new AccountBL();
                    Models.Account aTBL = new Models.Account();

                    //Check button text to add or update
                    string temp = btnAdd.Text;

                    if (temp.Contains("Add"))
                    {
                        var owner = db.tbl_Owner.ToList().FindAll(x => String.Compare(x.IDNo, (string)txtIDNo.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);
                        var acc = db.Accounts.ToList().FindAll(x => String.Compare(x.AccountNo, (string)txtAccNo.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);

                        if (owner.Count > 0 || string.IsNullOrWhiteSpace(txtIDNo.Text))
                        {
                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "inline");

                            txtIDNo.Text = string.Empty;
                            txtIDNo.Focus();
                        }
                        else if (acc.Count > 0 || string.IsNullOrWhiteSpace(txtAccNo.Text))
                        {
                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "none");
                            divwarning1.Style.Add("display", "inline");

                            txtAccNo.Text = string.Empty;
                            txtAccNo.Focus();

                            txtAccNo.Style.Add("border", "");
                        }
                        else
                        {
                            aTBL.AccountNo = txtAccNo.Text.Trim();
                            aTBL.BankID = Convert.ToInt16(ddlBank.SelectedValue);
                            aBL.CreateAccount(aTBL);

                            TBL.AddressCity = txtCity.Text;
                            TBL.AddressCode = txtCode.Text;
                            TBL.AddressStreet = txtStreetNo.Text;
                            TBL.AddressSuburb = txtSuburb.Text;
                            TBL.CompanyName = txtCO.Text;
                            //TBL.CalcBirthDate = Convert.ToDateTime(txtDOB.Text);
                            TBL.CellNo = txtCellNo.Text;
                            TBL.Email = txtEmail.Text;
                            TBL.GenderID = Convert.ToInt16(ddlGender.SelectedValue);
                            TBL.IDNo = txtIDNo.Text.Trim();
                            TBL.Name = txtName.Text;
                            TBL.OfficeNo = txtOfficeNo.Text;
                            TBL.RaceID = Convert.ToInt16(ddlRace.SelectedValue);
                            TBL.Surname = txtSurname.Text;
                            TBL.ReviewContract = chkReviewContract.Checked;
                            TBL.TermsAndConditions = chkTermsConditions.Checked;

                            GetPrimaryKey();
                            TBL.AccountID = accID;

                            //TBL.BankBranch = ddlBranch.SelectedItem.Text;
                            //TBL.BankName = ddlBank.SelectedItem.Text;
                            //TBL.AccountNo = txtAccNo.Text.Trim();

                            if ((ddlStatus.SelectedItem.Text == "Approved"))
                            {
                                TBL.StatusID = Convert.ToInt16(ddlStatus.SelectedValue);
                                divReason.Style.Add("display", "none");
                                divReason1.Style.Add("display", "none");
                            }
                            else if ((ddlStatus.SelectedItem.Text == "Inactive") || (ddlStatus.SelectedItem.Text == "Cancelled") || (ddlStatus.SelectedItem.Text == "Pending"))
                            {
                                TBL.StatusID = Convert.ToInt16(ddlStatus.SelectedValue);
                                TBL.ReasonID = Convert.ToInt16(ddlReason.SelectedValue);
                                divReason.Style.Add("display", "inline");
                                divReason1.Style.Add("display", "inline");
                            }

                            if (ddlStatus.SelectedItem.Text == "Pending")
                            {
                                BL.CreateOwner(TBL);

                                PaymentBL pBL = new PaymentBL();
                                Models.Payment pTBL = new Models.Payment();
                                //pTBL.LineNo = GenerateLineNo();
                                //pTBL.BatchNo = GenerateBatchNo();
                                //pTBL.DocNo = GenerateDocumentNo();
                                pTBL.SP = "P";
                                pTBL.PaymentMethod = "T";
                                pTBL.Date = DateTime.Now;
                                pTBL.IsPaymentSent = "No";
                                pTBL.EntityType = "BNO";
                                //pTBL.VoidedPayment = false;

                                var ownid = db.tbl_Owner.ToList().FirstOrDefault(x => x.IDNo == txtIDNo.Text.Trim());
                                pTBL.OwnerID = ownid.OwnerID;

                                string ownername = ownid.Name;
                                string ownersurname = ownid.Surname;
                                pTBL.AccountName = ownername + " " + ownersurname;
                                pTBL.PhoneNo = ownid.CellNo;
                                pTBL.AccountNo = ownid.IDNo;
                                pTBL.AddressLine1 = ownid.AddressStreet;
                                pTBL.AddressLine2 = ownid.AddressSuburb;
                                pTBL.AddressLine3 = ownid.AddressCode;
                                pTBL.City = ownid.AddressCity;
                                pTBL.PostalCode = ownid.AddressCode;

                                var accno = db.Accounts.ToList().FirstOrDefault(x => x.AccountID == ownid.AccountID);
                                pTBL.BankAccNo = accno.AccountNo;

                                var bnkno = db.Banks.ToList().FirstOrDefault(x => x.BankID == accno.BankID);
                                pTBL.BankName = bnkno.BankName;
                                pTBL.BankTransit = bnkno.BranchCode;
                                pBL.CreatePayment(pTBL);

                                SendEmailPending();

                                string own = "Owner";
                                SendSmsPending(own);

                                Clear();

                                divsuccess.Style.Add("display", "inline");
                                divdanger.Style.Add("display", "none");
                                divinfo.Style.Add("display", "none");
                                divwarning.Style.Add("display", "none");

                                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Pending"));
                                ddlStatus.Enabled = false;
                            }
                            else
                            {
                                BL.CreateOwner(TBL);

                                Clear();

                                divsuccess.Style.Add("display", "inline");
                                divdanger.Style.Add("display", "none");
                                divinfo.Style.Add("display", "none");
                                divwarning.Style.Add("display", "none");

                                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Pending"));
                                ddlStatus.Enabled = false;
                            }
                        }
                    }
                    else if (temp.Contains("Update"))
                    {
                        //var owner = db.tbl_Owner.ToList().FindAll(x => String.Compare(x.IDNo, (string)txtIDNo.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);

                        //if (owner.Count > 0 || string.IsNullOrWhiteSpace(txtIDNo.Text))
                        //{
                        //    divsuccess.Style.Add("display", "none");
                        //    divdanger.Style.Add("display", "none");
                        //    divinfo.Style.Add("display", "none");
                        //    divwarning.Style.Add("display", "inline");

                        //    Clear();
                        //    txtName.Focus();
                        //}
                        //else                       


                        int Id = int.Parse(ViewState["OwnerID"].ToString());
                        TBL.OwnerID = Id;
                        TBL.AddressCity = txtCity.Text;
                        TBL.AddressCode = txtCode.Text;
                        TBL.AddressStreet = txtStreetNo.Text;
                        TBL.AddressSuburb = txtSuburb.Text;
                        TBL.CompanyName = txtCO.Text;
                        //TBL.CalcBirthDate = Convert.ToDateTime(txtDOB.Text);
                        TBL.CellNo = txtCellNo.Text;
                        TBL.Email = txtEmail.Text;
                        TBL.GenderID = Convert.ToInt16(ddlGender.SelectedValue);
                        TBL.IDNo = txtIDNo.Text.Trim();
                        TBL.Name = txtName.Text;
                        TBL.OfficeNo = txtOfficeNo.Text;
                        TBL.RaceID = Convert.ToInt16(ddlRace.SelectedValue);
                        TBL.Surname = txtSurname.Text;
                        TBL.ReviewContract = chkReviewContract.Checked;
                        TBL.TermsAndConditions = chkTermsConditions.Checked;

                        if ((ddlStatus.SelectedItem.Text == "Approved"))
                        {
                            TBL.StatusID = Convert.ToInt16(ddlStatus.SelectedValue);
                        }
                        else if ((ddlStatus.SelectedItem.Text == "Inactive") || (ddlStatus.SelectedItem.Text == "Cancelled") || (ddlStatus.SelectedItem.Text == "Pending"))
                        {
                            TBL.StatusID = Convert.ToInt16(ddlStatus.SelectedValue);
                            TBL.ReasonID = Convert.ToInt16(ddlReason.SelectedValue);
                        }

                        var o = db.tbl_Owner.ToList().Find(x => x.IDNo == txtIDNo.Text.ToString());
                        var id = db.tbl_Owner.ToList().FirstOrDefault(x => x.OwnerID == Id);

                        #region account
                        var aa = db.Accounts.ToList().Find(x => x.AccountNo == txtAccNo.Text.ToString());
                        var a = db.Accounts.FirstOrDefault(x => x.AccountID == id.AccountID);
                        if ((aa == null) || (a.AccountNo == txtAccNo.Text.Trim()))
                        {
                            a.AccountNo = txtAccNo.Text.Trim();
                            a.BankID = Convert.ToInt16(ddlBank.SelectedValue);
                            aBL.UpdateAccount(a);

                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "none");
                            divwarning1.Style.Add("display", "none");

                            if ((o == null) || (id.IDNo == txtIDNo.Text.Trim()))
                            {
                                if (ddlStatus.SelectedItem.Text == "Pending")
                                {
                                    BL.UpdateOwner(TBL);

                                    PaymentBL pBL = new PaymentBL();
                                    Models.Payment pTBL = new Models.Payment();
                                    var payId = db.Payments.ToList().FirstOrDefault(x => x.OwnerID == Id);
                                    if(payId != null)
                                    {
                                        payId.AccountName = txtName.Text + " " + txtSurname.Text;
                                        payId.PhoneNo = txtCellNo.Text;
                                        payId.AccountNo = txtIDNo.Text;
                                        payId.AddressLine1 = txtStreetNo.Text; ;
                                        payId.AddressLine2 = txtSuburb.Text;
                                        payId.AddressLine3 = txtCode.Text;
                                        payId.PostalCode = txtCode.Text;
                                        payId.AddressLine4 = txtCity.Text;
                                        payId.BankAccNo = txtAccNo.Text;
                                        payId.BankName = ddlBank.SelectedItem.Text;
                                        payId.BankTransit = ddlBranch.SelectedItem.Text;
                                        pBL.UpdatePayment(payId);
                                    }
                                    else
                                    {
                                        pTBL.SP = "P";
                                        pTBL.PaymentMethod = "T";
                                        pTBL.Date = DateTime.Now;
                                        pTBL.IsPaymentSent = "No";
                                        pTBL.EntityType = "BNO";
                                        //pTBL.VoidedPayment = false;

                                        var ownid = db.tbl_Owner.ToList().FirstOrDefault(x => x.IDNo == txtIDNo.Text.Trim());
                                        pTBL.OwnerID = ownid.OwnerID;

                                        string ownername = ownid.Name;
                                        string ownersurname = ownid.Surname;
                                        pTBL.AccountName = ownername + " " + ownersurname;
                                        pTBL.PhoneNo = ownid.CellNo;
                                        pTBL.AccountNo = ownid.IDNo;
                                        pTBL.AddressLine1 = ownid.AddressStreet;
                                        pTBL.AddressLine2 = ownid.AddressSuburb;
                                        pTBL.AddressLine3 = ownid.AddressCode;
                                        pTBL.City = ownid.AddressCity;
                                        pTBL.PostalCode = ownid.AddressCode;

                                        var accno = db.Accounts.ToList().FirstOrDefault(x => x.AccountID == ownid.AccountID);
                                        pTBL.BankAccNo = accno.AccountNo;

                                        var bnkno = db.Banks.ToList().FirstOrDefault(x => x.BankID == accno.BankID);
                                        pTBL.BankName = bnkno.BankName;
                                        pTBL.BankTransit = bnkno.BranchCode;
                                        pBL.CreatePayment(pTBL);
                                    }

                                    SendEmailPendingEdited();

                                    string own = "Owner";
                                    SendSmsPending(own);

                                    btnAdd.Text = "Add";

                                    Clear();

                                    divsuccess.Style.Add("display", "none");
                                    divdanger.Style.Add("display", "none");
                                    divinfo.Style.Add("display", "inline");
                                    divwarning.Style.Add("display", "none");
                                    divwarning1.Style.Add("display", "none");

                                    ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Pending"));
                                    ddlStatus.Enabled = false;
                                }
                                else
                                {
                                    BL.UpdateOwner(TBL);

                                    btnAdd.Text = "Add";

                                    Clear();

                                    divsuccess.Style.Add("display", "none");
                                    divdanger.Style.Add("display", "none");
                                    divinfo.Style.Add("display", "inline");
                                    divwarning.Style.Add("display", "none");
                                    divwarning1.Style.Add("display", "none");

                                    ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Pending"));
                                    ddlStatus.Enabled = false;
                                }
                            }
                            else
                            {
                                divsuccess.Style.Add("display", "none");
                                divdanger.Style.Add("display", "none");
                                divinfo.Style.Add("display", "none");
                                divwarning.Style.Add("display", "inline");
                                divwarning1.Style.Add("display", "none");

                                txtIDNo.Text = string.Empty;
                                txtIDNo.Focus();

                                //lblError.Visible = true;
                                ////lblError.Text = "Connection Problem: " + ex.Message.ToString();
                                //lblError.Text = "Id number exists.";
                            }
                        }
                        else
                        {
                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "none");
                            divwarning1.Style.Add("display", "inline");

                            txtAccNo.Text = string.Empty;
                            txtAccNo.Focus();
                        }
                        #endregion
                    }
                }
                LoadData();
            }

            catch (Exception ex)
            {
                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");
                divwarning1.Style.Add("display", "none");

                lblError.Visible = true;
                //lblError.Text = "Connection Problem: " + ex.Message.ToString();
                lblError.Text = "Network problem or services are currently down, please contact support team.";
            }
        }
        protected void gvOwner_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            divsuccess.Style.Add("display", "none");
            divdanger.Style.Add("display", "none");
            divinfo.Style.Add("display", "none");
            divwarning.Style.Add("display", "none");

            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gvOwner.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                string Source = gvOwner.Rows[myRow.RowIndex].Cells[0].Text;

                ViewState["OwnerID"] = Id;
                //int Id = Convert.ToInt32(e.CommandArgument);

                var temp = db.tbl_Owner.ToList().FirstOrDefault(x => x.OwnerID == Convert.ToInt16(Id));

                if (temp != null)
                {
                    RemoveBorder();
                    Session["OwnerID"] = Id;
                    txtName.Text = temp.Name;
                    txtSurname.Text = temp.Surname;
                    txtIDNo.Text = temp.IDNo;
                    txtCellNo.Text = temp.CellNo;
                    txtOfficeNo.Text = temp.OfficeNo;
                    txtEmail.Text = temp.Email;
                    txtStreetNo.Text = temp.AddressStreet;
                    txtSuburb.Text = temp.AddressSuburb;
                    txtCity.Text = temp.AddressCity;
                    txtCode.Text = temp.AddressCode;
                    txtCO.Text = temp.CompanyName;
                    ddlRace.SelectedValue = temp.RaceID.ToString();
                    ddlGender.SelectedValue = temp.GenderID.ToString();
                    chkReviewContract.Checked = temp.ReviewContract.Value;
                    chkTermsConditions.Checked = temp.TermsAndConditions.Value;

                    var acc = db.Accounts.FirstOrDefault(x => x.AccountID == temp.AccountID);
                    txtAccNo.Text = acc.AccountNo.ToString();

                    var bank = db.Banks.FirstOrDefault(x => x.BankID == acc.BankID);
                    ddlBranch.SelectedValue = bank.BankID.ToString();
                    ddlBank.SelectedValue = bank.BankID.ToString();

                    //txtAccNo.Text = temp.AccountNo;
                    //var bank = db.tbl_Bank.FirstOrDefault(x => x.BankName == temp.BankName);
                    //ddlBranch.SelectedValue = bank.BankId.ToString();
                    //ddlBank.SelectedValue = bank.BankId.ToString();

                    if (temp.tbl_Status.StatusDescription == "Approved")
                    {
                        ddlStatus.Items.Clear();
                        LoadStatusAll();
                        RemoveDuplicateItems(ddlStatus);
                        ddlStatus.SelectedValue = temp.StatusID.ToString();
                        divReason.Style.Add("display", "inline");
                        divReason1.Style.Add("display", "inline");
                    }
                    else if ((temp.tbl_Status.StatusDescription == "Inactive") || (temp.tbl_Status.StatusDescription == "Cancelled") || (temp.tbl_Status.StatusDescription == "Pending"))
                    {
                        ddlStatus.Items.Clear();
                        LoadStatusNotAll();
                        //LoadReason();
                        RemoveDuplicateItems(ddlStatus);
                        ddlStatus.SelectedValue = temp.StatusID.ToString();
                        ddlReason.SelectedValue = temp.ReasonID.ToString();
                        divReason.Style.Add("display", "inline");
                        divReason1.Style.Add("display", "inline");
                    }

                    ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Pending"));
                    ddlStatus.Enabled = false;

                    btnAdd.Text = "Update";
                }
            }
            else if (commandName == "Print")
            {
                LoadStatusAll();

                //string Source = gvOwner.Rows[myRow.RowIndex].Cells[0].Text;

                //ViewState["OwnerID"] = Id;

                //var temp = db.tbl_Owner.ToList().FirstOrDefault(x => x.OwnerID == Convert.ToInt16(Id));

                //db.tbl_Owner.Remove(temp);
                //db.SaveChanges();
                //RemoveBorder();
                //LoadData();
                //Clear();
                //BtnAdd.Text = "Add";

                //divsuccess.Style.Add("display", "none");
                //divdanger.Style.Add("display", "inline");
                //divinfo.Style.Add("display", "none");
                //divwarning.Style.Add("display", "none");

                OwnerBL BL = new OwnerBL();
                tbl_Owner TBL = new tbl_Owner();

                ViewState["OwnerID"] = Id;

                var temp = db.tbl_Owner.ToList().FirstOrDefault(x => x.OwnerID == Convert.ToInt16(Id));

                if (temp != null)
                {
                    RemoveBorder();
                    Session["OwnerID"] = Id;
                    txtName.Text = temp.Name;
                    txtSurname.Text = temp.Surname;
                    txtIDNo.Text = temp.IDNo;
                    txtCellNo.Text = temp.CellNo;
                    txtOfficeNo.Text = temp.OfficeNo;
                    txtEmail.Text = temp.Email;
                    txtStreetNo.Text = temp.AddressStreet;
                    txtSuburb.Text = temp.AddressSuburb;
                    txtCity.Text = temp.AddressCity;
                    txtCode.Text = temp.AddressCode;
                    txtCO.Text = temp.CompanyName;
                    ddlRace.SelectedValue = temp.RaceID.ToString();
                    ddlGender.SelectedValue = temp.GenderID.ToString();
                    chkReviewContract.Checked = temp.ReviewContract.Value;
                    chkTermsConditions.Checked = temp.TermsAndConditions.Value;

                    var acc = db.Accounts.FirstOrDefault(x => x.AccountID == temp.AccountID);
                    txtAccNo.Text = acc.AccountNo.ToString();

                    var bank = db.Banks.FirstOrDefault(x => x.BankID == acc.BankID);
                    ddlBranch.SelectedValue = bank.BankID.ToString();
                    ddlBank.SelectedValue = bank.BankID.ToString();

                    //txtAccNo.Text = temp.AccountNo;
                    //var bank = db.tbl_Bank.FirstOrDefault(x => x.BankName == temp.BankName);
                    //ddlBranch.SelectedValue = bank.BankId.ToString();
                    //ddlBank.SelectedValue = bank.BankId.ToString();

                    ddlStatus.SelectedValue = temp.StatusID.ToString();

                    if (temp.ReasonID != null)
                    {
                        ddlReason.SelectedValue = temp.ReasonID.ToString();
                    }
                    else
                    {
                        ddlReason.SelectedItem.Text = "N/A";
                    }
                }

                CreatePDF();
                Clear();

                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");
            }
        }

        protected void gvOwner_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadData();

            if (gvOwner.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvOwner.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvOwner.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvOwner.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PL/Owner.aspx");
        }

        public void SendEmailPending()
        {
            string email, name, surname;
            var data = db.tbl_User.ToList().FindAll(x => x.IsSupervisor == true);

            for (int i = 0; i < data.Count; i++)
            {
                email = data[i].Email;
                name = data[i].Name;
                surname = data[i].Surname;

                try
                {
                    String URL = HttpContext.Current.Request.Url.AbsoluteUri;
                    Uri uri = new Uri(URL);

                    EmailBL EML = new EmailBL();
                    EML.Email("Moja Cruise System - Owner Registration", "Hello " + name + " " + surname + "<br/> <br/>"
                        + "Please check the pending record of Owner: " + txtName.Text + " " + txtSurname.Text + ", " + "ID No: " + txtIDNo.Text + "<br/> <br/>"
                        + "<a href = '" + uri + "'>Please click the link to login.</a>" + " " + "<br/> <br/>"
                         + "For more details feel free to contact the Ethekwini Transport Authority." + "<br/>" +
                            "Regards" + "<br/>" +
                            "Ethekwini Transport Authority", email, "Ethekwini Transport Authority", "ethekwini@oneconnectgroup.com");
                }
                catch (Exception ex)
                {
                    divsuccess.Style.Add("display", "none");
                    divdanger.Style.Add("display", "none");
                    divinfo.Style.Add("display", "none");
                    divwarning.Style.Add("display", "none");

                    lblError.Visible = true;
                    //lblError.Text = "Connection Problem: " + ex.Message.ToString();
                    lblError.Text = "Can't send email. Network problem or services are currently down, please contact support team.";
                }

                db.SaveChanges();
            }
        }

        public void SendEmailPendingEdited()
        {
            string email, name, surname;
            var data = db.tbl_User.ToList().FindAll(x => x.IsSupervisor == true);

            for (int i = 0; i < data.Count; i++)
            {
                email = data[i].Email;
                name = data[i].Name;
                surname = data[i].Surname;

                try
                {
                    String URL = HttpContext.Current.Request.Url.AbsoluteUri;
                    Uri uri = new Uri(URL);

                    EmailBL EML = new EmailBL();
                    EML.Email("Moja Cruise System - Owner Edited", "Hello " + name + " " + surname + "<br/> <br/>"
                        + "Please check the pending record of Owner: " + txtName.Text + " " + txtSurname.Text + ", " + "ID No: " + txtIDNo.Text + "<br/> <br/>"
                        + "<a href = '" + uri + "'>Please click the link to login.</a>" + " " + "<br/> <br/>"
                         + "For more details feel free to contact the Ethekwini Transport Authority." + "<br/>" +
                            "Regards" + "<br/>" +
                            "Ethekwini Transport Authority", email, "Ethekwini Transport Authority", "ethekwini@oneconnectgroup.com");
                }
                catch (Exception ex)
                {
                    divsuccess.Style.Add("display", "none");
                    divdanger.Style.Add("display", "none");
                    divinfo.Style.Add("display", "none");
                    divwarning.Style.Add("display", "none");

                    lblError.Visible = true;
                    //lblError.Text = "Connection Problem: " + ex.Message.ToString();
                    lblError.Text = "Can't send email. Network problem or services are currently down, please contact support team.";
                }

                db.SaveChanges();
            }
        }

        public void SendSmsPending(string owner)
        {
            try
            {
                string status = null;

                SMSService.PortTypeClient smsserv = new SMSService.PortTypeClient();
                SMSService.SendSMSReq_T req = new SMSService.SendSMSReq_T();

                req.MobileNo = txtCellNo.Text;
                req.TemplateId = "GDOwner";

                Parameters_T objp = new Parameters_T();

                req.Parameters = objp;
                req.Parameters.Parameter1 = new Parameter1_T();
                req.Parameters.Parameter1.Name = "Parameter1";
                req.Parameters.Parameter1.Value = owner;

                req.Parameters = objp;
                req.Parameters.Parameter2 = new Parameter2_T();
                req.Parameters.Parameter2.Name = "Parameter2";
                req.Parameters.Parameter2.Value = "Pending";

                SMSService.SendSMSResp_T obj = smsserv.SendSMSOperation(req);

                status = obj.Status.StatusMessage;
            }
            catch (Exception ex)
            {
                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");

                lblError.Visible = true;
                //lblError.Text = "Connection Problem: " + ex.Message.ToString();
                lblError.Text = "Can't send sms. Network problem or services are currently down, please contact support team.";
            }
        }

        public void SendSmsApproved(string owner)
        {
            try
            {
                string status = null;

                SMSService.PortTypeClient smsserv = new SMSService.PortTypeClient();
                SMSService.SendSMSReq_T req = new SMSService.SendSMSReq_T();

                req.MobileNo = txtCellNo.Text;
                req.TemplateId = "GDOwner";

                Parameters_T objp = new Parameters_T();

                req.Parameters = objp;
                req.Parameters.Parameter1 = new Parameter1_T();
                req.Parameters.Parameter1.Name = "Parameter1";
                req.Parameters.Parameter1.Value = owner;

                req.Parameters = objp;
                req.Parameters.Parameter2 = new Parameter2_T();
                req.Parameters.Parameter2.Name = "Parameter2";
                req.Parameters.Parameter2.Value = "Approved";

                SMSService.SendSMSResp_T obj = smsserv.SendSMSOperation(req);

                status = obj.Status.StatusMessage;
            }
            catch (Exception ex)
            {
                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");

                lblError.Visible = true;
                //lblError.Text = "Connection Problem: " + ex.Message.ToString();
                lblError.Text = "Can't send sms. Network problem or services are currently down, please contact support team.";
            }
        }

        public void CreatePDF()
        {
            // Create a Document object
            var document = new Document(PageSize.A4, 50, 50, 25, 25);

            // Create a new PdfWrite object, writing the output to a MemoryStream
            var output = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, output);

            // Open the Document for writing
            document.Open();

            var titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
            var subTitleFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
            var boldTableFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
            var endingMessageFont = FontFactory.GetFont("Arial", 10, Font.ITALIC);
            var bodyFont = FontFactory.GetFont("Arial", 12, Font.NORMAL);

            document.Add(new Paragraph("", titleFont));
            document.Add(Chunk.NEWLINE);

            document.Add(new Paragraph("Owner", titleFont));

            document.Add(new Paragraph("Personal Details", subTitleFont));
            var personaldetails = new PdfPTable(2);
            personaldetails.HorizontalAlignment = 0;
            personaldetails.SpacingBefore = 5;
            personaldetails.SpacingAfter = 5;
            personaldetails.DefaultCell.Border = 0;

            //personaldetails.SetWidths(new int[] { 1, 4 });
            personaldetails.AddCell(new Phrase("Name:", bodyFont));
            personaldetails.AddCell(txtName.Text);
            personaldetails.AddCell(new Phrase("Surname:", bodyFont));
            personaldetails.AddCell(txtSurname.Text);
            personaldetails.AddCell(new Phrase("ID No:", bodyFont));
            personaldetails.AddCell(txtIDNo.Text);
            personaldetails.AddCell(new Phrase("Cell No:", bodyFont));
            personaldetails.AddCell(txtCellNo.Text);
            personaldetails.AddCell(new Phrase("Office No:", bodyFont));
            personaldetails.AddCell(txtOfficeNo.Text);
            personaldetails.AddCell(new Phrase("Email:", bodyFont));
            personaldetails.AddCell(txtEmail.Text);
            personaldetails.AddCell(new Phrase("Gender:", bodyFont));
            personaldetails.AddCell(ddlGender.SelectedItem.Text);
            personaldetails.AddCell(new Phrase("Race:", bodyFont));
            personaldetails.AddCell(ddlRace.SelectedItem.Text);
            document.Add(personaldetails);

            // Add the "Address" subtitle

            document.Add(new Paragraph("Address", subTitleFont));
            var address = new PdfPTable(2);
            address.HorizontalAlignment = 0;
            address.SpacingBefore = 5;
            address.SpacingAfter = 5;
            address.DefaultCell.Border = 0;

            address.AddCell(new Phrase("Street No & Name:", bodyFont));
            address.AddCell(txtStreetNo.Text);
            address.AddCell(new Phrase("City:", bodyFont));
            address.AddCell(txtCity.Text);
            address.AddCell(new Phrase("Suburb:", bodyFont));
            address.AddCell(txtSuburb.Text);
            address.AddCell(new Phrase("Code:", bodyFont));
            address.AddCell(txtCode.Text);
            document.Add(address);

            document.Add(new Paragraph("Bank Details", subTitleFont));
            var bank = new PdfPTable(2);
            bank.HorizontalAlignment = 0;
            bank.SpacingBefore = 5;
            bank.SpacingAfter = 5;
            bank.DefaultCell.Border = 0;

            bank.AddCell(new Phrase("Bank Name:", bodyFont));
            bank.AddCell(ddlBank.SelectedItem.Text);
            bank.AddCell(new Phrase("Branch Code:", bodyFont));
            bank.AddCell(ddlBranch.SelectedItem.Text);
            bank.AddCell(new Phrase("Account No:", bodyFont));
            bank.AddCell(txtAccNo.Text);
            document.Add(bank);

            // Add the "Process Info" subtitle

            document.Add(new Paragraph("Process Info", subTitleFont));
            var process = new PdfPTable(2);
            process.HorizontalAlignment = 0;
            process.SpacingBefore = 5;
            process.SpacingAfter = 5;
            process.DefaultCell.Border = 0;

            process.AddCell(new Phrase("Status:", bodyFont));
            process.AddCell(ddlStatus.SelectedItem.Text);
            process.AddCell(new Phrase("Reason:", bodyFont));
            process.AddCell(ddlReason.SelectedItem.Text);
            process.AddCell(new Phrase("Accept Terms & Conditions:", bodyFont));
            process.AddCell("Yes");
            process.AddCell(new Phrase("Review Contract:", bodyFont));
            process.AddCell("Yes");
            process.AddCell(new Phrase("Print Date:", bodyFont));
            process.AddCell(currentDate);
            document.Add(process);

            //// Add ending message
            //var endingMessage = new Paragraph("Thank you for your business! If you have any questions about your order, please contact us at 800-555-NORTH.", endingMessageFont);
            //endingMessage.SetAlignment("Center");
            //document.Add(endingMessage);

            var godbn = iTextSharp.text.Image.GetInstance(Server.MapPath("~/img/godurban.png"));
            godbn.SetAbsolutePosition(10, 800);
            godbn.ScaleAbsolute(200f, 40f);
            godbn.ScaleToFit(500f, 40f);
            godbn.ScaledHeight.ToString();
            godbn.ScaledWidth.ToString();

            //godbn.ScaleToFit(105f, 105f);
            //godbn.Alignment = Element.ALIGN_LEFT;

            document.Add(godbn);

            var muvo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/img/moja.png"));
            muvo.SetAbsolutePosition(415, 800);
            muvo.ScaleAbsolute(170f, 40f);
            godbn.ScaleToFit(300f, 40f);
            godbn.ScaledHeight.ToString();
            godbn.ScaledWidth.ToString();

            //muvo.ScaleToFit(105f, 105f);
            //muvo.Alignment = Element.ALIGN_RIGHT;

            document.Add(muvo);

            document.Close();

            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=Owner.pdf");
            Response.BinaryWrite(output.ToArray());
        }
        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((ddlStatus.SelectedItem.Text == "Inactive") || (ddlStatus.SelectedItem.Text == "Cancelled") || (ddlStatus.SelectedItem.Text == "Pending"))
            {
                divReason.Style.Add("display", "inline");
                divReason1.Style.Add("display", "inline");
                //LoadReason();
            }
            else
            {
                divReason.Style.Add("display", "none");
                divReason1.Style.Add("display", "none");
            }
        }

        protected void ddlBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlBranch.SelectedValue = ddlBank.SelectedValue;
            ddlBranch.DataBind();
        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlBank.SelectedValue = ddlBranch.SelectedValue;
            ddlBank.DataBind();
        }

        protected void gvOwner_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string st = e.Row.Cells[12].Text.ToString();
                if (st == "Approved")
                {
                    LinkButton lnkPrint = (LinkButton)e.Row.Cells[2].FindControl("lnkPrint");
                    lnkPrint.Visible = true;
                }
                else
                {
                    LinkButton lnkPrint = (LinkButton)e.Row.Cells[2].FindControl("lnkPrint");
                    lnkPrint.Visible = false;
                }
            }
        }
    }
}