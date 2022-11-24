using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSB_SCO_RMS
{
    public partial class Form1 : Form
    {
        clsImportant objcls = new clsImportant();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            try
            {
                //Get Master Client Table Data (SCO_TTABCLI)
                //DataSet dsSCOClient = new DataSet();

                //dsSCOClient = objcls.GetDSBSCOClientMaster();

                //int iReturnClient = objcls.InsertClientDetailsRMS(dsSCOClient);

                //Activity Log
                //objcls.InsertActivityLog("DSB SCO Data Transfer Client Master", iReturnClient);

                //if time is 7am till next day 
                //if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour <= 10)
                //{
                //    Get Data
                //    DataSet dsSCO = new DataSet();

                //    string strDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                //    string strDate = "2022-04-30";
                //    dsSCO = objcls.GetDSBSCO(strDate);

                //    int iReturn = objcls.InsertDetailsRMS(dsSCO);

                //    Activity Log
                //    objcls.InsertActivityLog("DSB SCO Data Transfer", iReturn);
                //}

                if (DateTime.Now.Hour >= 11 && DateTime.Now.Hour <= 23)
                {
                    //Get Data
                    DataSet dsSCO = new DataSet();

                    string strDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                    //string strDate = "2022-04-30";
                    //git check
                    //string strDate = "2022-04-30";
                    //string strDate = "2022-04-30";
                    //end check

                    dsSCO = objcls.GetDSBSCO(strDate);

                    int iReturn = objcls.InsertDetailsRMS(dsSCO);

                    //Activity Log
                    objcls.InsertActivityLog("DSB SCO Data Transfer", iReturn);
                }

                //objcls.SendMailMessage("DSB_Sco_RMS@sisprosegur.com", "rrajupradhan@sisprosegur.com", "", "", "DSB-SCO to RMS Transfer Date : " + strDate, "DSB-SCO to RMS Transfer " + iReturn.ToString() + " row(s).");

            }
            catch (Exception)
            {
                //objcls.SendMailMessage("support@sisprosegur.com", "rrajupradhan@sisprosegur.com", "", "", "Error - btnGetData_Click ", ex.Message.ToString() + " || " + ex.StackTrace.ToString());
                //return;
            }
            finally
            {
                System.Diagnostics.Process.Start(Application.ExecutablePath);
                Process.GetCurrentProcess().Kill();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Now.Hour >= 1 && DateTime.Now.Hour <= 23)
                {
                    //if (DateTime.Now.Minute % 30 == 0)
                    //{
                        timer1.Enabled = false;
                        btnGetData_Click(null, null);
                        timer1.Enabled = true;
                    //}
                }
            }
            catch (Exception ex)
            {
                //objcls.SendMailMessage("support@sisprosegur.com", "rrajupradhan@sisprosegur.com", "", "", "Error - timer1_Tick ", ex.Message.ToString() + " || " + ex.StackTrace.ToString());
                return;
            }
        }

      
    }
}
