using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DSB_SCO_RMS
{
    class clsImportant
    {
        string strMySqlConnectionString = "";
        string strMySqlConnectionStringBulk = "";
        string strRMSConnectionString = "";
        string strSCOConnectionString = "";

        public void Get_from_config()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(@"C:\Rms_Bulk_Upload_Template\Cyclo_Connection_Details.xml");

                XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Configuration");

                foreach (XmlNode node in nodeList)
                {
                    strMySqlConnectionString = node.SelectSingleNode("MySqlConnectionString").InnerText;
                    strMySqlConnectionStringBulk = node.SelectSingleNode("MySqlConnectionStringBulk").InnerText;
                    strRMSConnectionString = node.SelectSingleNode("RMS_Connection_String").InnerText;
                    strSCOConnectionString = node.SelectSingleNode("SCO_Connection_String").InnerText;
                }

            }
            catch (Exception ex)
            {
                SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - Get_from_config", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }
        }


        //string strRMSConnectionString = "Data Source=10.61.0.33;Initial Catalog=ReportingManagementSystem;Persist Security Info=True;User ID=sa;Password=rmsblindspot";
        //string strSCOConnectionString = "Data Source=10.28.27.26;Initial Catalog=sco;Persist Security Info=True;User ID=SCOLEITURA;Password=SCOLEITURA";

        //Function to Send Email
        public void SendMailMessage(string from, string to, string bcc, string cc, string subject, string body)
        {
            try
            {
                bool bMessageSent = false;

                // Instantiate a new instance of MailMessage
                MailMessage mMailMessage = new MailMessage();

                // Set the sender address of the mail message
                mMailMessage.From = new MailAddress(from);

                // Set the recepient address of the mail message
                mMailMessage.To.Add(new MailAddress(to));

                // Check if the bcc value is null or an empty string
                if ((bcc != null) && (bcc != string.Empty))
                {
                    // Set the Bcc address of the mail message
                    mMailMessage.Bcc.Add(new MailAddress(bcc));
                }

                // Check if the cc value is null or an empty value
                if ((cc != null) && (cc != string.Empty))
                {
                    // Set the CC address of the mail message
                    mMailMessage.CC.Add(new MailAddress(cc));
                }

                // Set the subject of the mail message
                mMailMessage.Subject = subject;

                // Set the body of the mail message
                mMailMessage.Body = body;

                // Set the format of the mail message body as HTML
                mMailMessage.IsBodyHtml = true;

                // Set the priority of the mail message to normal
                mMailMessage.Priority = MailPriority.Normal;

                // Instantiate a new instance of SmtpClient
                SmtpClient mSmtpClient = new SmtpClient("mail.sisprosegur.com", 587);

                //Enable SSl Certificate
                mSmtpClient.EnableSsl = true;

                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("support@sisprosegur.com", "India@321");

                //Assigning credentials in mSmtpClient
                mSmtpClient.Credentials = credentials;

                // Send the mail message
                mSmtpClient.Send(mMailMessage);

                bMessageSent = true;

                //return bMessageSent;
            }
            catch (Exception exSendMailMessage)
            {
                throw;
            }
        }

        public DataSet GetDSBSCOClientMaster()
        {
            Get_from_config();
            DataSet dsData = new DataSet();

            try
            {
                string strSQL = "select CODCLI,NOMFAN,RAZSOC from SCO_TTABCLI";
                
                SqlConnection conSCO = new SqlConnection(strSCOConnectionString);

                conSCO.Open();

                SqlCommand cmd = new SqlCommand(strSQL, conSCO);

                cmd.CommandTimeout = 1200;

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dsData);

                conSCO.Close();
            }
            catch (Exception)
            {
                //SendMailMessage("support@sisprosegur.com", "rrajupradhan@gmail.com", "", "", "Error -GetDSBSCOClientMaster ", ex.Message.ToString() + " || " + ex.StackTrace.ToString());
            }

            return dsData;
        }

        public DataSet GetDSBSCO(string strDate)
        {
            Get_from_config();
            DataSet dsData = new DataSet();

            try
            {
//                string strSQL = "Select " +
//"TPOT.DESTIPPTOATE AS CustomerType,TREG.DESREG AS HUB,  " +
//"TBL_CASH_PICKUP.*,TUF.DESUF,TMUN.NOMMUN, TPON.REFPONATE AS Cust_Code, TPON.BARPONATE , " +
//" TPON.OBSPONATE as [Account_No_New],TPON.CEPPONATE as [PinCode] FROM   " +
//"(  " +
//"SELECT   " +
//"  " +
//"CASE WHEN ROTA.[SERVICE SITUATION] = '1'  THEN 'PENDING'  " +
//"            WHEN ROTA.[SERVICE SITUATION] = '2'  THEN 'TO BE CANCELLED' " +
//"            WHEN ROTA.[SERVICE SITUATION] = '3'  THEN 'CANCELLED'  " +
//"            WHEN ROTA.[SERVICE SITUATION] = '4'  THEN 'TO BE INCLUDED'  " +
//"            WHEN ROTA.[SERVICE SITUATION] = '5'  THEN 'CONCLUDED'  " +
//"            WHEN ROTA.[SERVICE SITUATION] = 'A'  THEN 'PERFORMING'  " +
//"            WHEN ROTA.[SERVICE SITUATION] = 'B'  THEN 'ON THE WAY'  " +
//"            WHEN ROTA.[SERVICE SITUATION] = 'C'  THEN 'PENDING SOLN'  " +
//"            WHEN ROTA.[SERVICE SITUATION] = 'V'  THEN 'POINT CLOSED'  " +
//"            WHEN ROTA.[SERVICE SITUATION] = '9'  THEN 'NO CASH'  " +
//"            WHEN ROTA.[SERVICE SITUATION] = '6'  THEN 'ROUTE CLOSED'  " +
//"            WHEN ROTA.[SERVICE SITUATION] = '7'  THEN 'FINAL CLOSED'  " +
//"      ELSE 'UNKNOWN SITUATION' END AS [SERVICE SITUATION TYPE],  " +
//"      ROTA.[S.NO],  " +
//"      ROTA.[BRANCH CODE],  " +
//"      ROTA.[BRANCH NAME],  " +
//"      ROTA.[ROUTE NUMBER],  " +
//"      ROTA.[ROUTE NAME],  " +
//"      ROTA.[DEVICE ID],  " +
//"      ROTA.[DATE],  " +
//"      ROTA.[ROUTE START TIME-TM],  " +
//"      ROTA.[ROUTE END TIME-TM],  " +
//"      ROTA.[VEHICLE NUMBER],  " +
//"      ROTA.CUSTODIAN_1,  " +
//"      ROTA.CUSTODIAN_1_REG,  " +
//"      ROTA.CUSTODIAN_2,  " +
//"      ROTA.CUSTODIAN_2_REG,  " +
//"      ROTA.GUNMAN_1,  " +
//"      ROTA.GUNMAN_1_REG,  " +
//"      ROTA.GUNMAN_2,  " +
//"      ROTA.GUNMAN_2_REG,  " +
//"      ROTA.DRIVER,  " +
//"      ROTA.DRIVER_REG,  " +
//"      ROTA.[SERVICE SITUATION],  " +
//"      ROTA.[BILLING CLIENT CODE],  " +
//"      ROTA.[BILING CLIENT NAME],  " +
//"      ROTA.[SERVICE NUMBER],  " +
//"      ROTA.[SERVICE TYPE],  " +
//"      ROTA.[CUSTOMER CODE-ORIGIN],  " +
//"      ROTA.[CUSTOMER NAME-ORIGIN],  " +
//"      ROTA.[SERVICE POINT NUMBER-ORIGIN],  " +
//"      ROTA.[SERVICE POINT NAME-ORIGIN],  " +
//"      RTRIM(ROTA.[SERVICE POINT ADDRESS-ORIGIN1]) AS [SERVICE POINT ADDRESS-ORIGIN1],  " +
//"      ROTA.[SERVICE POINT ADDRESS-DELIVERY1],  " +
//"      ROTA.CODCLASER,  " +
//"      CASE WHEN CHARINDEX('//',ROTA.[SERVICE POINT ADDRESS-ORIGIN]) > 0 THEN SUBSTRING(ROTA.[SERVICE POINT ADDRESS-ORIGIN],0,CHARINDEX('//',ROTA.[SERVICE POINT ADDRESS-ORIGIN])) ELSE 'NA' END AS CUSTOMER_CODE,  " +
//"        CASE WHEN CHARINDEX('//',ROTA.[SERVICE POINT ADDRESS-ORIGIN]) > 0 THEN SUBSTRING(SUBSTRING(ROTA.[SERVICE POINT ADDRESS-ORIGIN],CHARINDEX('//',ROTA.[SERVICE POINT ADDRESS-ORIGIN]) + 2,  " +
//"      LEN(ROTA.[SERVICE POINT ADDRESS-ORIGIN])),0,CHARINDEX('//',SUBSTRING(ROTA.[SERVICE POINT ADDRESS-ORIGIN],  " +
//"      CHARINDEX('//',ROTA.[SERVICE POINT ADDRESS-ORIGIN]) + 2,LEN(ROTA.[SERVICE POINT ADDRESS-ORIGIN])))) ELSE 'NA' END AS CRN_No,  " +
//"        CASE WHEN CHARINDEX('//',ROTA.[SERVICE POINT ADDRESS-ORIGIN]) > 0 THEN SUBSTRING(SUBSTRING(ROTA.[SERVICE POINT ADDRESS-ORIGIN],CHARINDEX('//',ROTA.[SERVICE POINT ADDRESS-ORIGIN]) + 2,  " +
//"            LEN(ROTA.[SERVICE POINT ADDRESS-ORIGIN])),CHARINDEX('//',SUBSTRING(ROTA.[SERVICE POINT ADDRESS-ORIGIN],  " +
//"            CHARINDEX('//',ROTA.[SERVICE POINT ADDRESS-ORIGIN]) + 2,LEN(ROTA.[SERVICE POINT ADDRESS-ORIGIN]))) + 2,  " +
//"            LEN(SUBSTRING(ROTA.[SERVICE POINT ADDRESS-ORIGIN],  " +
//"            CHARINDEX('//',ROTA.[SERVICE POINT ADDRESS-ORIGIN]),  " +
//"            LEN(ROTA.[SERVICE POINT ADDRESS-ORIGIN])))) ELSE 'NA' END AS Account_No,  " +
//"      ROTA.[CUSTOMER CODE-DELIVERY],  " +
//"      ROTA.[CUSTOMER NAME-DELIVERY],  " +
//"      ROTA.[SERVICE POINT NUMBER-DELIVERY],  " +
//"      ROTA.[SERVICE POINT NAME-DELIVERY],  " +
//"      ROTA.[ACTUAL KM READING],  " +
//"      ROTA.[TIME OF ARRIVAL],  " +
//"      ROTA.[TIME OF DEPARTURE],  " +
//"      ROTA.[TIME OF LAST ROUTE UPDATE],  " +
//"      ROTA.[REMARKS],  " +
//"      CASE WHEN (ROTA.DELIVERY_DAY = 'NEXT' OR ROTA.[CUSTOMER CODE-DELIVERY] = '0' OR ROTA.[CUSTOMER CODE-DELIVERY] = '999') THEN 'VAULT' ELSE 'BANK' END AS CASH_DEPOSIT_IN,  " +
//"      CASE WHEN ROTA.[CUSTOMER CODE-ORIGIN] = 0 AND ROTA.[CUSTOMER CODE-DELIVERY] = 0 THEN 'ADMINISTRATIVE'  " +
//"            WHEN ROTA.[CUSTOMER CODE-ORIGIN] = 0 AND ROTA.[CUSTOMER CODE-DELIVERY] <> 0 THEN 'DELIVERY' " +
//"            WHEN ROTA.[CUSTOMER CODE-ORIGIN] <> 0 AND ROTA.[CUSTOMER CODE-DELIVERY] = 0 THEN 'PICKUP'  " +
//"      ELSE 'TRANSIT' END AS [TYPE OF SERVICE],  " +
//"            CASE WHEN ROTA.[CUSTOMER CODE-ORIGIN] = 0 AND ROTA.[CUSTOMER CODE-DELIVERY] = 0 THEN '0'  " +
//"            WHEN ROTA.[CUSTOMER CODE-ORIGIN] = 0 AND ROTA.[CUSTOMER CODE-DELIVERY] <> 0 THEN '1'  " +
//"            WHEN ROTA.[CUSTOMER CODE-ORIGIN] <> 0 AND ROTA.[CUSTOMER CODE-DELIVERY] = 0 THEN '2'  " +
//"      ELSE '3' END AS [CODE TYPE OF SERVICE],  " +
//"  " +
//"  " +
//"ISNULL(GUICOL.NUMGUI, GUIENT.NUMGUI) AS [SLIP NUMBER],  " +
//"ISNULL(GUICOL.NUMLAC1, GUIENT.NUMLAC1) AS [SEAL NUMBER],  " +
//"ISNULL(SUM(ISNULL(GUICOL.[C_INR_0.1],GUIENT.[C_INR_0.1])),0) AS [M_INR_0.1],  " +
//"ISNULL(SUM(ISNULL(GUICOL.[C_INR_0.50],GUIENT.[C_INR_0.50])),0) AS [M_INR_0.50],  " +
//"ISNULL(SUM(ISNULL(GUICOL.C_INR_1,GUIENT.C_INR_1)),0) AS C_INR_1,  " +
//"ISNULL(SUM(ISNULL(GUICOL.C_INR_2,GUIENT.C_INR_2)),0) AS C_INR_2,  " +
//"ISNULL(SUM(ISNULL(GUICOL.C_INR_5,GUIENT.C_INR_5)),0) AS C_INR_5,  " +
//"ISNULL(SUM(ISNULL(GUICOL.C_INR_10,GUIENT.C_INR_10)),0) AS C_INR_10,  " +
//"ISNULL(ISNULL(GUICOL.VALMOE,GUIENT.VALMOE),0) AS [COINS-AMOUNT],  " +
//"ISNULL(SUM(ISNULL(GUICOL.INR_10,GUIENT.INR_10)),0) AS INR_10,   " +
//"ISNULL(SUM(ISNULL(GUICOL.INR_20,GUIENT.INR_20)),0) AS INR_20,   " +
//"ISNULL(SUM(ISNULL(GUICOL.INR_50,GUIENT.INR_50)),0) AS INR_50,   " +
//"ISNULL(SUM(ISNULL(GUICOL.INR_100,GUIENT.INR_100)),0) AS INR_100,   " +
//"ISNULL(SUM(ISNULL(GUICOL.INR_500,GUIENT.INR_500)),0) AS INR_500,   " +
//"ISNULL(SUM(ISNULL(GUICOL.INR_1000,GUIENT.INR_1000)),0) AS INR_1000,  " +
//"ISNULL(SUM(ISNULL(GUICOL.INR_2000,GUIENT.INR_2000)),0) AS INR_2000,  " +
//"ISNULL((ISNULL(GUICOL.VALDIN,GUIENT.VALDIN) - ISNULL(GUICOL.VALMOE,GUIENT.VALMOE)),0) AS [CASH-AMOUNT],  " +
//"ISNULL(SUM(ISNULL(GUICOL.ITERTV10,GUIENT.ITERTV10)),0) AS [BULLION],  " +
//"ISNULL(SUM(ISNULL(GUICOL.ITERTV3,GUIENT.ITERTV3)),0) AS [INTRUMENTS],  " +
//"ISNULL(SUM(ISNULL(GUICOL.ITERTV9,GUIENT.ITERTV9)),0) AS [OTHERS],  " +
//"ISNULL(ISNULL(GUICOL.VALDIN, GUIENT.VALDIN),0) AS [TOTAL AMOUNT/QUANTITY],  " +
//"ISNULL(SUM(ISNULL(GUICOL.INR_200,GUIENT.INR_200)),0) AS INR_200,  " +
//"ISNULL(GUICOL.OBSNAOATE,GUIENT.OBSNAOATE) AS MANUAL_REMARK, " +
//"ISNULL(GUICOL.DESJUSNAOATE,GUIENT.DESJUSNAOATE) AS REASON, " +
//"ISNULL(GUICOL.SCRCAR,GUIENT.SCRCAR) AS SCRCAR, " +
//"ROTA.ITEFOLROTID  " +
//"  " +
//"FROM   " +
//"(  " +
//"( " +
//" " +
//"SELECT DISTINCT  " +
//"ITE.itefolrotid as [ITEFOLROTID]," +
//"ITE.SITITEFOLROT AS [SERVICE SITUATION],  " +
//"ITE.SEQFOLROT AS [S.NO],  " +
//"ITE.CODFIL AS [BRANCH CODE],  " +
//"FIL.NOMFIL AS [BRANCH NAME],  " +
//"ITE.NUMROT AS [ROUTE NUMBER],  " +
//"FOL.DESROT AS [ROUTE NAME],  " +
//"TM.COD_SERIE AS [DEVICE ID],  " +
//"ITE.DATSER AS [DATE],  " +
//"FOL.HORINIREA AS [ROUTE START TIME-TM],  " +
//"FOL.HORFIMREA AS [ROUTE END TIME-TM],  " +
//"VEH.PLAVEI AS [VEHICLE NUMBER],  " +
//"ITE.DES_SOLUCAO AS [REMARKS],  " +
//"  " +
//"(SELECT MAX(FUN.NOMCOM)  " +
//"      " +
//"  FROM SCO_TCADEQU EQ WITH (NOLOCK)   " +
//"  INNER JOIN SCO_TCADFUC AS FUC WITH (NOLOCK) ON EQ.CODFUC = FUC.CODFUC   " +
//"  INNER JOIN SCO_TCADFUN AS FUN WITH (NOLOCK) ON EQ.MATFUN = FUN.MATFUN   " +
//"  " +
//"  WHERE EQ.CODFIL = ITE.CODFIL AND EQ.DATSERABE = ITE.DATSER AND EQ.NUMROTABE = ITE.NUMROT  " +
//"  AND FUC.CODFUC IN (3,8,11)) AS CUSTODIAN_1,  " +
//"  " +
//"  (SELECT MAX(FUN.REGNOERP)  " +
//"      " +
//"  FROM SCO_TCADEQU EQ WITH (NOLOCK)   " +
//"  INNER JOIN SCO_TCADFUC AS FUC WITH (NOLOCK) ON EQ.CODFUC = FUC.CODFUC   " +
//"  INNER JOIN SCO_TCADFUN AS FUN WITH (NOLOCK) ON EQ.MATFUN = FUN.MATFUN   " +
//"  " +
//"  WHERE EQ.CODFIL = ITE.CODFIL AND EQ.DATSERABE = ITE.DATSER AND EQ.NUMROTABE = ITE.NUMROT  " +
//"  AND FUC.CODFUC IN (3,8,11)) AS CUSTODIAN_1_REG,  " +
//"  " +
//"   (SELECT MIN(FUN.NOMCOM)  " +
//"    " +
//"    " +
//"  FROM SCO_TCADEQU EQ WITH (NOLOCK)   " +
//"  INNER JOIN SCO_TCADFUC AS FUC WITH (NOLOCK) ON EQ.CODFUC = FUC.CODFUC   " +
//"  INNER JOIN SCO_TCADFUN AS FUN WITH (NOLOCK) ON EQ.MATFUN = FUN.MATFUN   " +
//"  " +
//"  WHERE EQ.CODFIL = ITE.CODFIL AND EQ.DATSERABE = ITE.DATSER AND EQ.NUMROTABE = ITE.NUMROT  " +
//"  AND FUC.CODFUC IN (3,8,11)) AS CUSTODIAN_2,  " +
//"  " +
//"     (SELECT MIN(FUN.REGNOERP)  " +
//"    " +
//"    " +
//"  FROM SCO_TCADEQU EQ WITH (NOLOCK)   " +
//"  INNER JOIN SCO_TCADFUC AS FUC WITH (NOLOCK) ON EQ.CODFUC = FUC.CODFUC   " +
//"  INNER JOIN SCO_TCADFUN AS FUN WITH (NOLOCK) ON EQ.MATFUN = FUN.MATFUN   " +
//"  " +
//"  WHERE EQ.CODFIL = ITE.CODFIL AND EQ.DATSERABE = ITE.DATSER AND EQ.NUMROTABE = ITE.NUMROT  " +
//"  AND FUC.CODFUC IN (3,8,11)) AS CUSTODIAN_2_REG,  " +
//"    " +
//"   (SELECT MAX(FUN.NOMCOM)  " +
//"    " +
//"    " +
//"  FROM SCO_TCADEQU EQ WITH (NOLOCK)   " +
//"  INNER JOIN SCO_TCADFUC AS FUC WITH (NOLOCK) ON EQ.CODFUC = FUC.CODFUC   " +
//"  INNER JOIN SCO_TCADFUN AS FUN WITH (NOLOCK) ON EQ.MATFUN = FUN.MATFUN   " +
//"  " +
//"  WHERE EQ.CODFIL = ITE.CODFIL AND EQ.DATSERABE = ITE.DATSER AND EQ.NUMROTABE = ITE.NUMROT  " +
//"  AND FUC.CODFUC IN (2,9)) AS GUNMAN_1,  " +
//"  " +
//"     (SELECT MAX(FUN.REGNOERP)  " +
//"    " +
//"    " +
//"  FROM SCO_TCADEQU EQ WITH (NOLOCK)   " +
//"  INNER JOIN SCO_TCADFUC AS FUC WITH (NOLOCK) ON EQ.CODFUC = FUC.CODFUC   " +
//"  INNER JOIN SCO_TCADFUN AS FUN WITH (NOLOCK) ON EQ.MATFUN = FUN.MATFUN   " +
//"  " +
//"  WHERE EQ.CODFIL = ITE.CODFIL AND EQ.DATSERABE = ITE.DATSER AND EQ.NUMROTABE = ITE.NUMROT  " +
//"  AND FUC.CODFUC IN (2,9)) AS GUNMAN_1_REG,  " +
//"  " +
//"   (SELECT MIN(FUN.NOMCOM)  " +
//"    " +
//"    " +
//"  FROM SCO_TCADEQU EQ WITH (NOLOCK)   " +
//"  INNER JOIN SCO_TCADFUC AS FUC WITH (NOLOCK) ON EQ.CODFUC = FUC.CODFUC   " +
//"  INNER JOIN SCO_TCADFUN AS FUN WITH (NOLOCK) ON EQ.MATFUN = FUN.MATFUN   " +
//"  " +
//"  WHERE EQ.CODFIL = ITE.CODFIL AND EQ.DATSERABE = ITE.DATSER AND EQ.NUMROTABE = ITE.NUMROT  " +
//"  AND FUC.CODFUC IN (2,9)) AS GUNMAN_2,  " +
//"  " +
//"     (SELECT MIN(FUN.REGNOERP)  " +
//"    " +
//"    " +
//"  FROM SCO_TCADEQU EQ WITH (NOLOCK)   " +
//"  INNER JOIN SCO_TCADFUC AS FUC WITH (NOLOCK) ON EQ.CODFUC = FUC.CODFUC   " +
//"  INNER JOIN SCO_TCADFUN AS FUN WITH (NOLOCK) ON EQ.MATFUN = FUN.MATFUN   " +
//"  " +
//"  WHERE EQ.CODFIL = ITE.CODFIL AND EQ.DATSERABE = ITE.DATSER AND EQ.NUMROTABE = ITE.NUMROT  " +
//"  AND FUC.CODFUC IN (2,9)) AS GUNMAN_2_REG,  " +
//"  " +
//"(SELECT MIN(FUN.NOMCOM)  " +
//"    " +
//"    " +
//"  FROM SCO_TCADEQU EQ WITH (NOLOCK)   " +
//"  INNER JOIN SCO_TCADFUC AS FUC WITH (NOLOCK) ON EQ.CODFUC = FUC.CODFUC   " +
//"  INNER JOIN SCO_TCADFUN AS FUN WITH (NOLOCK) ON EQ.MATFUN = FUN.MATFUN   " +
//"  " +
//"  WHERE EQ.CODFIL = ITE.CODFIL AND EQ.DATSERABE = ITE.DATSER AND EQ.NUMROTABE = ITE.NUMROT  " +
//"  AND FUC.CODFUC = 1 ) AS DRIVER,  " +
//"  " +
//"  (SELECT MIN(FUN.REGNOERP)  " +
//"    " +
//"    " +
//"  FROM SCO_TCADEQU EQ WITH (NOLOCK)   " +
//"  INNER JOIN SCO_TCADFUC AS FUC WITH (NOLOCK) ON EQ.CODFUC = FUC.CODFUC   " +
//"  INNER JOIN SCO_TCADFUN AS FUN WITH (NOLOCK) ON EQ.MATFUN = FUN.MATFUN   " +
//"  " +
//"  WHERE EQ.CODFIL = ITE.CODFIL AND EQ.DATSERABE = ITE.DATSER AND EQ.NUMROTABE = ITE.NUMROT  " +
//"  AND FUC.CODFUC = 1 ) AS DRIVER_REG,  " +
//"  " +
//"SERORI.CODCLI AS [BILLING CLIENT CODE],  " +
//"CLIORISER.NOMFAN AS [BILING CLIENT NAME],  " +
//"ITE.NUMSER AS [SERVICE NUMBER],  " +
//"TIP.DESTIPSER AS [SERVICE TYPE],  " +
//"PARORI.CODCLI AS [CUSTOMER CODE-ORIGIN],  " +
//"CLIORI.NOMFAN AS [CUSTOMER NAME-ORIGIN],  " +
//"ISNULL(PONATEORI.NUMPONATE,PONATEATMORI.NUMPONATE) AS [SERVICE POINT NUMBER-ORIGIN],  " +
//"ISNULL(PONATEORI.NOMPONATE,PONATEATMORI.NOMPONATE) AS [SERVICE POINT NAME-ORIGIN],  " +
//"ISNULL(PONATEORI.OBSPONATE,PONATEATMORI.OBSPONATE) AS [SERVICE POINT ADDRESS-ORIGIN],  " +
//"ISNULL(PONATEORI.ENDPON,PONATEATMORI.ENDPON) AS [SERVICE POINT ADDRESS-ORIGIN1],  " +
//"SERORI.CODCLASER,  " +
//"PARDES.CODCLI AS [CUSTOMER CODE-DELIVERY],  " +
//"CLIDES.NOMFAN AS [CUSTOMER NAME-DELIVERY],  " +
//"ISNULL(PONATEDES.NUMPONATE,PONATEATMDES.NUMPONATE ) AS [SERVICE POINT NUMBER-DELIVERY],  " +
//"ISNULL(PONATEDES.NOMPONATE,PONATEATMDES.NOMPONATE) AS [SERVICE POINT NAME-DELIVERY],  " +
//"ISNULL(PONATEDES.OBSPONATE,PONATEATMDES.OBSPONATE) AS [SERVICE POINT ADDRESS-DELIVERY],  " +
//"  ISNULL(PONATEDES.ENDPON,PONATEATMDES.ENDPON) AS [SERVICE POINT ADDRESS-DELIVERY1],  " +
//"CASE WHEN SERORI.FLGENTPRODIA = '1' THEN 'NEXT' ELSE 'SAME' END AS DELIVERY_DAY,  " +
//"ITE.ODOVEI AS [ACTUAL KM READING],  " +
//"ITE.HORCHEREA AS [TIME OF ARRIVAL],  " +
//"ITE.HORSAIREA AS [TIME OF DEPARTURE],  " +
//"CONVERT(NVARCHAR(8), FOL.DATULTALT,114 ) AS [TIME OF LAST ROUTE UPDATE]  " +
//"       " +
//"  " +
//"FROM   SCO_TGRARISSEG RISATM (NOLOCK) RIGHT OUTER JOIN  " +
//"        SCO_TPONATE PONATM  (NOLOCK)  ON RISATM.CODGRARISSEG = PONATM.CODGRARISSEG RIGHT OUTER JOIN  " +
//"        SCO_TGRARISSEG RISPON  (NOLOCK) RIGHT OUTER JOIN  " +
//"        SCO_TPONATE PON  (NOLOCK) ON RISPON.CODGRARISSEG = PON.CODGRARISSEG RIGHT OUTER JOIN  " +
//"        SCO_TCADATM ATM  (NOLOCK) RIGHT OUTER JOIN  " +
//"        SCO_TITEFOLROT ITE  (NOLOCK) LEFT OUTER JOIN  " +
//"        SCO_TCLASER CLA  (NOLOCK) ON ITE.CODCLASER = CLA.CODCLASER LEFT OUTER JOIN  " +
//"        SCO_TTABCLI CLI  (NOLOCK) ON ITE.CODCLI = CLI.CODCLI ON ATM.CODCLI = ITE.CODCLI AND ATM.CODFIL = ITE.CODFIL AND  " +
//"        ATM.NUMATM = ITE.NUMATM LEFT OUTER JOIN  " +
//"        SCO_TTIPSER TIP ON ITE.CODTIPSER = TIP.CODTIPSER ON PON.NUMPONATE = ITE.NUMPON AND PON.CODFIL = ITE.CODFIL AND  " +
//"        PON.CODCLI = ITE.CODCLI ON PONATM.CODPONATEID = ATM.CODPONATEIDLOC   " +
//"        LEFT OUTER JOIN  SCO_TFOLROT  FOL (NOLOCK)  " +
//"            ON      ITE.CODFIL = FOL.CODFIL  " +
//"                AND ITE.DATSER = FOL.DATSER  " +
//"                AND ITE.NUMROT = FOL.NUMROT  " +
//"        LEFT OUTER JOIN SCO_TPONATE PONORI (NOLOCK)   " +
//"        ON ITE.CODFIL = PONORI.CODFIL AND ITE.CODCLI = PONORI.CODCLI AND ITE.NUMPON = PONORI.NUMPONATE   " +
//"        LEFT OUTER JOIN TPBR_TPDA PDA (NOLOCK) ON FOL.OID_PDA = PDA.OID_PDA   " +
//" LEFT OUTER JOIN SCO_TPAREXT EXT (NOLOCK)   " +
//"   ON EXT.CODDOM = 234   " +
//"       AND EXT.CODFIL = ITE.CODFIL   " +
//"       AND EXT.CODCLI = ITE.CODCLI  " +
//"       AND EXT.CODVALDOM = CASE WHEN ITE.NUMATM IS NULL THEN 2 ELSE 1 END  " +
//"INNER JOIN SCO_TCADFIL FIL (NOLOCK) ON ITE.CODFIL = FIL.CODFIL  " +
//"  " +
//"LEFT OUTER JOIN   " +
//"      SCO_TCADSER SERORI (NOLOCK)  " +
//"            ON SERORI.CODFIL = ITE.CODFIL   " +
//"                  AND SERORI.NUMPED = ITE.NUMSER   " +
//"INNER JOIN   " +
//"      SCO_TTABCLI CLIORISER  " +
//"            ON CLIORISER.CODCLI = SERORI.CODCLI  " +
//"INNER JOIN   " +
//"      SCO_TPARSER PARORI (NOLOCK)  " +
//"            ON PARORI.CODFIL = SERORI.CODFIL  " +
//"                  AND PARORI.NUMSER = SERORI.NUMPED   " +
//"                  AND PARORI.SEQSER = 1  " +
//"LEFT OUTER JOIN  " +
//"      SCO_TPONATE PONATEORI  " +
//"            ON PONATEORI.CODFIL = PARORI.CODFIL   " +
//"                  AND PONATEORI.NUMPONATE = PARORI.NUMPONATE  " +
//"                  AND PONATEORI.CODCLI = PARORI.CODCLI  " +
//"LEFT OUTER JOIN  " +
//"      SCO_TCADATM ATMORI  " +
//"            ON ATMORI.CODFIL = PARORI.CODFIL  " +
//"                  AND ATMORI.CODCLI = PARORI.CODCLI  " +
//"                  AND ATMORI.NUMATM = PARORI.NUMATM   " +
//"LEFT OUTER JOIN  " +
//"      SCO_TPONATE PONATEATMORI  " +
//"            ON PONATEATMORI.CODFIL = ATMORI.CODFIL   " +
//"                  AND PONATEATMORI.NUMPONATE = ATMORI.NUMPONATERES  " +
//"                  AND PONATEATMORI.CODCLI = ATMORI.CODCLI  " +
//"INNER JOIN   " +
//"      SCO_TTABCLI CLIORI  " +
//"            ON CLIORI.CODCLI = PARORI.CODCLI  " +
//"  " +
//"LEFT OUTER JOIN   " +
//"      SCO_TCADSER SERDES (NOLOCK)  " +
//"            ON SERDES.CODFIL = ITE.CODFIL   " +
//"                  AND SERDES.NUMPED = ITE.NUMSER   " +
//"INNER JOIN   " +
//"      SCO_TPARSER PARDES (NOLOCK)  " +
//"            ON PARDES.CODFIL = SERDES.CODFIL  " +
//"                  AND PARDES.NUMSER = SERDES.NUMPED   " +
//"                  AND PARDES.SEQSER = 2  " +
//"LEFT OUTER JOIN  " +
//"      SCO_TPONATE PONATEDES  " +
//"            ON PONATEDES.CODFIL = PARDES.CODFIL   " +
//"                  AND PONATEDES.NUMPONATE = PARDES.NUMPONATE  " +
//"                  AND PONATEDES.CODCLI = PARDES.CODCLI  " +
//"LEFT OUTER JOIN  " +
//"      SCO_TCADATM ATMDES  " +
//"            ON ATMDES.CODFIL = PARDES.CODFIL  " +
//"                  AND ATMDES.CODCLI = PARDES.CODCLI  " +
//"                  AND ATMDES.NUMATM = PARDES.NUMATM   " +
//"LEFT OUTER JOIN  " +
//"      SCO_TPONATE PONATEATMDES  " +
//"            ON PONATEATMDES.CODFIL = ATMDES.CODFIL   " +
//"                  AND PONATEATMDES.NUMPONATE = ATMDES.NUMPONATERES  " +
//"                  AND PONATEATMDES.CODCLI = ATMDES.CODCLI  " +
//"INNER JOIN   " +
//"      SCO_TTABCLI CLIDES  " +
//"            ON CLIDES.CODCLI = PARDES.CODCLI  " +
//"  " +
//"LEFT OUTER JOIN   " +
//"      TPBR_TPDA TM (NOLOCK)  " +
//"            ON TM.OID_PDA = FOL.OID_PDA  " +
//"LEFT OUTER JOIN   " +
//"      SCO_TCADVEI VEH  " +
//"      ON FOL.NUMVEIABE = VEH.NUMVEI  " +
//"WHERE   " +
//"      ITE.DATSER >= '" + strDate + "' AND ITE.DATSER <= '" + strDate + "'  " +
//"AND ((((CAST(CONVERT(VARCHAR(5), HORCHEPRE, 8) AS DATETIME) BETWEEN '00:00:00' AND '23:59:59') OR HORCHEPRE IS NULL))   " +
//"OR ((DATEDIFF(N, CAST(CONVERT(VARCHAR(5), HORSAIPRE, 8) AS DATETIME), '06:00:00' ) > 0) " +
//"AND ((CAST(CONVERT(VARCHAR(5), HORCHEPRE, 8) AS DATETIME) BETWEEN '00:00:00' AND '23:59:59') OR HORCHEPRE IS NULL) )) " +
//"/*AND  ITE.CODMACREG IN (2,1,8) AND   ITE.NUMROT NOT IN(201,202)*/   " +
//") AS ROTA    " +
//"LEFT OUTER JOIN  " +
//"(  " +
//"SELECT   " +
//"      ISNULL(GUI.SLIPNUM, GUI.NUMGUI) as NUMGUI, GUI.NUMROTCOL, GUI.SEQROTCOL, MAL.NUMLAC1,GUI.DATCOL,GUI.NUMPED,GUI.CODFIL, " +
//"      ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS) * (ISNULL(COMP1.PES_COMP, COMP.PES_COMP)) AS VALORGTV,  " +
//"      SUM(ITEGTVENT9.QTDITERTV) AS ITERTV9,  " +
//"      SUM(ITEGTVENT2.QTDITERTV) AS ITERTV2,  " +
//"      SUM(ITEGTVENT10.QTDITERTV) AS ITERTV10,  " +
//"      SUM(ITEGTVENT3.QTDITERTV) AS ITERTV3,  " +
//"      MAL.VALDIN AS VALDIN,  " +
//"      MAL.VALMOE AS VALMOE,  " +
//"  " +
//"      CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP) WHEN  12 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS INR_10,     " +
//"      CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP) WHEN  1 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS INR_20,    " +
//"      CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP) WHEN  2 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS INR_50,     " +
//"      CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP) WHEN  3 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS INR_100,    " +
//"      CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP) WHEN  5 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS INR_500,  " +
//"      CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP) WHEN  6 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS INR_1000,  " +
//"      CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP) WHEN  13 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS INR_2000,  " +
//"      CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP) WHEN  14 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS INR_200,  " +
//"      CASE ISNULL(CONT.COD_ESP,PEDMAL.COD_ESP)  WHEN  2 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS MOEDAS,  " +
//"      CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  WHEN  15 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS [C_INR_0.1],  " +
//"      CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  WHEN  9 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS [C_INR_0.50],  " +
//"      CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  WHEN  7 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS C_INR_1,  " +
//"      CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  WHEN  8 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS C_INR_2,  " +
//"      CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  WHEN  10 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS C_INR_5,  " +
//"      CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  WHEN  11 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS C_INR_10,  " +
//" REMRK.OBSNAOATE, " +
//"CASE WHEN REMRK.CODJUSNAOATE = 1 THEN 'Due to BANK'  " +
//"     WHEN REMRK.CODJUSNAOATE = 2 THEN 'Due to MSP'  " +
//"     WHEN REMRK.CODJUSNAOATE = 3 THEN 'Due to SIS-Prosegur'  " +
//"     else 'The Pickup Point is closed' end as DESJUSNAOATE, " +
//"GUI.SCRCAR " +
//"FROM   " +
//"      SCO_TCADPOOGUI GUI (NOLOCK)  " +
//" LEFT OUTER JOIN SCO_TCADJUSNAOATELOG REMRK (NOLOCK) ON (GUI.RTVID = REMRK.RTVID)" +
//"LEFT OUTER JOIN   " +
//"      SCO_TCADMAL MAL (NOLOCK)  " +
//"            ON GUI.CODFIL = MAL.CODFIL   " +
//"                  AND GUI.DATCOL = MAL.DATCOL  " +
//"                  AND GUI.NUMGUI = MAL.NUMGUI  " +
//"                  AND GUI.SERGUI = MAL.SERGUI  " +
//"                  AND GUI.CODORIRTV = MAL.CODORIRTV  " +
//"LEFT OUTER JOIN   " +
//"      SCO_TPLANCONTAGEM CONT (NOLOCK)  " +
//"            ON CONT.COD_FIL = MAL.CODFIL  " +
//"                  AND CONT.NUM_GUI = MAL.NUMGUI  " +
//"                  AND CONT.SER_GUI = MAL.SERGUI  " +
//"                  AND CONT.COD_ORI_RTV = MAL.CODORIRTV  " +
//"                  AND CONT.DATCOL = MAL.DATCOL  " +
//"                  AND CONT.NUM_MAL = MAL.NUMMAL  " +
//"LEFT OUTER JOIN  " +
//"      SCO_TCADCOMPOSICAO COMP1   " +
//"            ON COMP1.COD_COMP = CONT.COD_COM  " +
//"LEFT OUTER JOIN   " +
//"      SCO_TCADMAL MALPED (NOLOCK)  " +
//"            ON MALPED.CODFIL = GUI.CODFIL   " +
//"                  AND MALPED.DATCOL = GUI.DATCOL  " +
//"                  AND MALPED.NUMGUI = GUI.NUMGUI  " +
//"                  AND MALPED.SERGUI = GUI.SERGUI  " +
//"                  AND MALPED.CODORIRTV = GUI.CODORIRTV  " +
//"LEFT OUTER JOIN     " +
//"      SCO_TCOMPMALOTE PEDMAL (NOLOCK)  " +
//"            ON PEDMAL.COD_FIL = MALPED.CODFIL  " +
//"                  AND PEDMAL.NUM_GUI = MALPED.NUMGUI  " +
//"                  AND PEDMAL.SER_GUI = MALPED.SERGUI  " +
//"                  AND PEDMAL.COD_ORI_RTV = MALPED.CODORIRTV  " +
//"                  AND PEDMAL.DATCOL = MALPED.DATCOL  " +
//"                  AND PEDMAL.NUM_MAL = MALPED.NUMMAL   " +
//"LEFT OUTER JOIN  " +
//"      SCO_TCADCOMPOSICAO COMP (NOLOCK)  " +
//"            ON COMP.COD_COMP = PEDMAL.COD_COMP  " +
//"                  AND COMP.COD_ESP = PEDMAL.COD_ESP  " +
//"  " +
//"            LEFT OUTER JOIN  " +
//"                  SCO_TITERTV ITEGTVENT9 (NOLOCK)  " +
//"                        ON ITEGTVENT9.CODFIL = GUI.CODFIL  " +
//"                              AND ITEGTVENT9.NUMGUI = GUI.NUMGUI  " +
//"                              AND ITEGTVENT9.SERGUI = GUI.SERGUI  " +
//"                              AND ITEGTVENT9.CODORIRTV = GUI.CODORIRTV  " +
//"                              AND ITEGTVENT9.DATCOL = GUI.DATCOL  " +
//"                              AND ITEGTVENT9.CODTIPITERTV = 12  " +
//"              " +
//"            LEFT OUTER JOIN  " +
//"                  SCO_TITERTV ITEGTVENT2 (NOLOCK)  " +
//"                        ON ITEGTVENT2.CODFIL = GUI.CODFIL  " +
//"                              AND ITEGTVENT2.NUMGUI = GUI.NUMGUI  " +
//"                              AND ITEGTVENT2.SERGUI = GUI.SERGUI  " +
//"                              AND ITEGTVENT2.CODORIRTV = GUI.CODORIRTV  " +
//"                              AND ITEGTVENT2.DATCOL = GUI.DATCOL  " +
//"                              AND ITEGTVENT2.CODTIPITERTV = 2  " +
//"  " +
//"            LEFT OUTER JOIN  " +
//"                  SCO_TITERTV ITEGTVENT10 (NOLOCK)  " +
//"                        ON ITEGTVENT10.CODFIL = GUI.CODFIL  " +
//"                              AND ITEGTVENT10.NUMGUI = GUI.NUMGUI  " +
//"                              AND ITEGTVENT10.SERGUI = GUI.SERGUI  " +
//"                              AND ITEGTVENT10.CODORIRTV = GUI.CODORIRTV  " +
//"                              AND ITEGTVENT10.DATCOL = GUI.DATCOL  " +
//"                              AND ITEGTVENT10.CODTIPITERTV = 10  " +
//"  " +
//"            LEFT OUTER JOIN  " +
//"                  SCO_TITERTV ITEGTVENT3 (NOLOCK)  " +
//"                        ON ITEGTVENT3.CODFIL = GUI.CODFIL  " +
//"                              AND ITEGTVENT3.NUMGUI = GUI.NUMGUI  " +
//"                              AND ITEGTVENT3.SERGUI = GUI.SERGUI  " +
//"                              AND ITEGTVENT3.CODORIRTV = GUI.CODORIRTV  " +
//"                              AND ITEGTVENT3.DATCOL = GUI.DATCOL  " +
//"                              AND ITEGTVENT3.CODTIPITERTV = 11  " +
//"                          " +
//"  " +
//"WHERE   " +
//"     GUI.DATCOL >= '" + strDate + "' AND   GUI.DATCOL <= '" + strDate + "'  " +
//"GROUP BY GUI.DATCOL,GUI.NUMPED,GUI.CODFIL,CONT.COD_COM, PEDMAL.COD_COMP, ISNULL(GUI.SLIPNUM, GUI.NUMGUI), GUI.NUMROTCOL, GUI.SEQROTCOL,CONT.COD_ESP,PEDMAL.COD_ESP,MAL.NUMLAC1,  " +
//"CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS,COMP1.PES_COMP, COMP.PES_COMP, MAL.VALDIN, MAL.VALMOE, " +
//"REMRK.OBSNAOATE,REMRK.CODJUSNAOATE," +
//" GUI.SCRCAR " +
//"  " +
//") AS GUICOL  " +
//"ON  " +
//"      GUICOL.NUMROTCOL = ROTA.[ROUTE NUMBER]  " +
//"      AND   GUICOL.SEQROTCOL = ROTA.[S.NO]  " +
//"	and ROTA.[DATE] = GUICOL.DATCOL " +
//"	and ROTA.[SERVICE NUMBER] = GUICOL.NUMPED " +
//"	and ROTA.[BRANCH CODE] = GUICOL.CODFIL " +
//"  " +
//"LEFT OUTER JOIN   " +
//"  (  " +
//"      SELECT   " +
//"           ISNULL(GUI.SLIPNUM, GUI.NUMGUI) as NUMGUI, GUI.NUMROTENT, GUI.SEQROTENT, MAL.NUMLAC1, GUI.DATCOL,GUI.NUMPED,GUI.CODFIL, " +
//"  " +
//"            ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS) * (ISNULL(COMP1.PES_COMP, COMP.PES_COMP)) AS VALORGTV,  " +
//"            SUM(ITEGTVENT9.QTDITERTV) AS ITERTV9,  " +
//"            SUM(ITEGTVENT2.QTDITERTV) AS ITERTV2,  " +
//"            SUM(ITEGTVENT10.QTDITERTV) AS ITERTV10,  " +
//"            SUM(ITEGTVENT3.QTDITERTV) AS ITERTV3,  " +
//"            MAL.VALDIN AS VALDIN,     " +
//"            MAL.VALMOE AS VALMOE,  " +
//"  " +
//"            CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP) WHEN  12 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS INR_10,     " +
//"            CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP) WHEN  1 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS INR_20,    " +
//"            CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP) WHEN  2 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS INR_50,     " +
//"            CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP) WHEN  3 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS INR_100,    " +
//"            CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP) WHEN  5 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS INR_500,  " +
//"            CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP) WHEN  6 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS INR_1000,  " +
//"            CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP) WHEN  13 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS INR_2000,  " +
//"            CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP) WHEN  14 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS INR_200,  " +
//"            CASE ISNULL(CONT.COD_ESP,PEDMAL.COD_ESP)  WHEN  2 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS MOEDAS,  " +
//"            CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  WHEN  15 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS [C_INR_0.1],  " +
//"            CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  WHEN  9 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS [C_INR_0.50],  " +
//"            CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  WHEN  7 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS C_INR_1,  " +
//"            CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  WHEN  8 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS C_INR_2,  " +
//"            CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  WHEN  10 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS C_INR_5,  " +
//"            CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  WHEN  11 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) END AS C_INR_10, " +
//" REMRK.OBSNAOATE, " +
//"CASE WHEN REMRK.CODJUSNAOATE = 1 THEN 'Due to BANK'  " +
//"     WHEN REMRK.CODJUSNAOATE = 2 THEN 'Due to MSP'  " +
//"     WHEN REMRK.CODJUSNAOATE = 3 THEN 'Due to SIS-Prosegur'  " +
//"     else 'The Pickup Point is closed' end as DESJUSNAOATE, " +
//"GUI.SCRCAR " +
//"  " +
//"  " +
//"            FROM   " +
//"                  SCO_TCADPOOGUI GUI (NOLOCK)  " +
//" LEFT OUTER JOIN SCO_TCADJUSNAOATELOG REMRK (NOLOCK) ON (GUI.RTVID = REMRK.RTVID)" +
//"            LEFT OUTER JOIN   " +
//"                  SCO_TCADMAL MAL (NOLOCK)  " +
//"                        ON GUI.CODFIL = MAL.CODFIL   " +
//"                              AND GUI.DATCOL = MAL.DATCOL  " +
//"                              AND GUI.NUMGUI = MAL.NUMGUI  " +
//"                              AND GUI.SERGUI = MAL.SERGUI  " +
//"                              AND GUI.CODORIRTV = MAL.CODORIRTV  " +
//"            LEFT OUTER JOIN   " +
//"                  SCO_TPLANCONTAGEM CONT (NOLOCK)  " +
//"                        ON CONT.COD_FIL = MAL.CODFIL  " +
//"                              AND CONT.NUM_GUI = MAL.NUMGUI  " +
//"                              AND CONT.SER_GUI = MAL.SERGUI  " +
//"                              AND CONT.COD_ORI_RTV = MAL.CODORIRTV  " +
//"                              AND CONT.DATCOL = MAL.DATCOL  " +
//"                              AND CONT.NUM_MAL = MAL.NUMMAL  " +
//"            LEFT OUTER JOIN  " +
//"                  SCO_TCADCOMPOSICAO COMP1 (NOLOCK)  " +
//"                        ON COMP1.COD_COMP = CONT.COD_COM " +
//"            LEFT OUTER JOIN   " +
//"                  SCO_TCADMAL MALPED (NOLOCK)  " +
//"                        ON MALPED.CODFIL = GUI.CODFIL   " +
//"                              AND MALPED.DATCOL = GUI.DATCOL  " +
//"                              AND MALPED.NUMGUI = GUI.NUMGUI  " +
//"                              AND MALPED.SERGUI = GUI.SERGUI  " +
//"                              AND MALPED.CODORIRTV = GUI.CODORIRTV  " +
//"            LEFT OUTER JOIN     " +
//"                  SCO_TCOMPMALOTE PEDMAL (NOLOCK)  " +
//"                        ON PEDMAL.COD_FIL = MALPED.CODFIL  " +
//"                              AND PEDMAL.NUM_GUI = MALPED.NUMGUI  " +
//"                              AND PEDMAL.SER_GUI = MALPED.SERGUI  " +
//"                              AND PEDMAL.COD_ORI_RTV = MALPED.CODORIRTV  " +
//"                              AND PEDMAL.DATCOL = MALPED.DATCOL  " +
//"                              AND PEDMAL.NUM_MAL= MALPED.NUMMAL   " +
//"            LEFT OUTER JOIN  " +
//"                  SCO_TCADCOMPOSICAO COMP (NOLOCK)  " +
//"                        ON COMP.COD_COMP = PEDMAL.COD_COMP  " +
//"                              AND COMP.COD_ESP = PEDMAL.COD_ESP  " +
//"  " +
//"            LEFT OUTER JOIN  " +
//"                  SCO_TITERTV ITEGTVENT9 (NOLOCK)  " +
//"                        ON ITEGTVENT9.CODFIL = GUI.CODFIL  " +
//"                              AND ITEGTVENT9.NUMGUI = GUI.NUMGUI  " +
//"                              AND ITEGTVENT9.SERGUI = GUI.SERGUI  " +
//"                              AND ITEGTVENT9.CODORIRTV = GUI.CODORIRTV  " +
//"                              AND ITEGTVENT9.DATCOL = GUI.DATCOL  " +
//"                              AND ITEGTVENT9.CODTIPITERTV = 12  " +
//"              " +
//"            LEFT OUTER JOIN  " +
//"                  SCO_TITERTV ITEGTVENT2 (NOLOCK)  " +
//"                        ON ITEGTVENT2.CODFIL = GUI.CODFIL  " +
//"                              AND ITEGTVENT2.NUMGUI = GUI.NUMGUI  " +
//"                              AND ITEGTVENT2.SERGUI = GUI.SERGUI  " +
//"                              AND ITEGTVENT2.CODORIRTV = GUI.CODORIRTV  " +
//"                              AND ITEGTVENT2.DATCOL = GUI.DATCOL  " +
//"                              AND ITEGTVENT2.CODTIPITERTV = 2  " +
//"  " +
//"            LEFT OUTER JOIN  " +
//"                  SCO_TITERTV ITEGTVENT10 (NOLOCK)  " +
//"                        ON ITEGTVENT10.CODFIL = GUI.CODFIL  " +
//"                              AND ITEGTVENT10.NUMGUI = GUI.NUMGUI  " +
//"                              AND ITEGTVENT10.SERGUI = GUI.SERGUI  " +
//"                              AND ITEGTVENT10.CODORIRTV = GUI.CODORIRTV  " +
//"                              AND ITEGTVENT10.DATCOL = GUI.DATCOL  " +
//"                              AND ITEGTVENT10.CODTIPITERTV = 10  " +
//"  " +
//"            LEFT OUTER JOIN  " +
//"                  SCO_TITERTV ITEGTVENT3 (NOLOCK)  " +
//"                        ON ITEGTVENT3.CODFIL = GUI.CODFIL  " +
//"                              AND ITEGTVENT3.NUMGUI = GUI.NUMGUI  " +
//"                              AND ITEGTVENT3.SERGUI = GUI.SERGUI  " +
//"                              AND ITEGTVENT3.CODORIRTV = GUI.CODORIRTV  " +
//"                              AND ITEGTVENT3.DATCOL = GUI.DATCOL  " +
//"                              AND ITEGTVENT3.CODTIPITERTV = 11  " +
//"  " +
//"            WHERE   " +
//"            GUI.DATENT >= '" + strDate + "' AND GUI.DATENT <= '" + strDate + "'  " +
//"      GROUP BY GUI.DATCOL,GUI.NUMPED,GUI.CODFIL,CONT.COD_COM, PEDMAL.COD_COMP, ISNULL(GUI.SLIPNUM, GUI.NUMGUI), GUI.NUMROTENT, GUI.SEQROTENT,CONT.COD_ESP, PEDMAL.COD_ESP,MAL.NUMLAC1,  " +
//"            CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS,COMP1.PES_COMP, COMP.PES_COMP,MAL.VALDIN, MAL.VALMOE, " +
//"REMRK.OBSNAOATE,REMRK.CODJUSNAOATE, " + 
//"GUI.SCRCAR  " +
//"  " +
//") AS GUIENT   " +
//" ON  " +
//"      GUIENT.NUMROTENT = ROTA.[ROUTE NUMBER]  " +
//"      AND   GUIENT.SEQROTENT = ROTA.[S.NO]  " +
//"	 and ROTA.[DATE] = GUIENT.DATCOL " +
//"	and ROTA.[SERVICE NUMBER] = GUIENT.NUMPED " +
//"	and ROTA.[BRANCH CODE] = GUIENT.CODFIL " +
//"  " +
//")  " +
//"GROUP BY   " +
//"      ROTA.[S.NO],  " +
//"      ROTA.[BRANCH CODE],  " +
//"      ROTA.[BRANCH NAME],  " +
//"      ROTA.[ROUTE NUMBER],  " +
//"      ROTA.[ROUTE NAME],  " +
//"      ROTA.[DEVICE ID],  " +
//"      ROTA.[DATE],  " +
//"      ROTA.[ROUTE START TIME-TM],  " +
//"      ROTA.[ROUTE END TIME-TM],  " +
//"      ROTA.[VEHICLE NUMBER],  " +
//"      ROTA.CUSTODIAN_1,  " +
//"      ROTA.CUSTODIAN_1_REG,  " +
//"      ROTA.CUSTODIAN_2,  " +
//"      ROTA.CUSTODIAN_2_REG,  " +
//"      ROTA.GUNMAN_1,  " +
//"      ROTA.GUNMAN_1_REG,  " +
//"      ROTA.GUNMAN_2,  " +
//"      ROTA.GUNMAN_2_REG,  " +
//"      ROTA.DRIVER,  " +
//"      ROTA.DRIVER_REG,  " +
//"      GUICOL.NUMGUI,   " +
//"      GUIENT.NUMGUI,  " +
//"      GUICOL.NUMLAC1,  " +
//"      GUIENT.NUMLAC1,  " +
//"      ROTA.[SERVICE SITUATION],  " +
//"      ROTA.[BILLING CLIENT CODE],  " +
//"      ROTA.[BILING CLIENT NAME],  " +
//"      ROTA.[SERVICE NUMBER],  " +
//"      ROTA.[SERVICE TYPE],  " +
//"      ROTA.[CUSTOMER CODE-ORIGIN],  " +
//"      ROTA.[CUSTOMER NAME-ORIGIN],  " +
//"      ROTA.[SERVICE POINT NUMBER-ORIGIN],  " +
//"      ROTA.[SERVICE POINT NAME-ORIGIN],  " +
//"      ROTA.[SERVICE POINT ADDRESS-ORIGIN],  " +
//"      ROTA.DELIVERY_DAY,  " +
//"      ROTA.[CODCLASER],  " +
//"      ROTA.[CUSTOMER CODE-DELIVERY],  " +
//"      ROTA.[CUSTOMER NAME-DELIVERY],  " +
//"      ROTA.[SERVICE POINT NUMBER-DELIVERY],  " +
//"      ROTA.[SERVICE POINT NAME-DELIVERY],  " +
//"                ROTA.[SERVICE POINT ADDRESS-DELIVERY],  " +
//"                        ROTA.[SERVICE POINT ADDRESS-ORIGIN1],  " +
//"      ROTA.[SERVICE POINT ADDRESS-DELIVERY1],  " +
//"      ROTA.[ACTUAL KM READING],  " +
//"      ROTA.[TIME OF ARRIVAL],  " +
//"      ROTA.[TIME OF DEPARTURE],  " +
//"      ROTA.[TIME OF LAST ROUTE UPDATE],  " +
//"      ROTA.[REMARKS],  " +
//"      ROTA.[ITEFOLROTID]," +
//"      GUICOL.ITERTV9,  " +
//"      GUIENT.ITERTV9,  " +
//"      GUICOL.ITERTV2,  " +
//"      GUIENT.ITERTV2,  " +
//"      GUICOL.ITERTV10,  " +
//"      GUIENT.ITERTV10,  " +
//"      GUICOL.ITERTV3,  " +
//"      GUIENT.ITERTV3,  " +
//"      GUICOL.VALDIN,  " +
//"      GUICOL.VALMOE,  " +
//"      GUIENT.VALMOE,  " +
//"      GUIENT.VALDIN, " +
//" GUICOL.OBSNAOATE," +
//"GUIENT.OBSNAOATE, " +
//"GUICOL.DESJUSNAOATE," +
//"GUIENT.DESJUSNAOATE, " +
//"GUICOL.SCRCAR," +
//"GUIENT.SCRCAR " +
//"      )TBL_CASH_PICKUP   " +
//"      LEFT JOIN dbo.SCO_TPONATE TPON  " +
//"      ON (TBL_CASH_PICKUP.[SERVICE POINT NUMBER-ORIGIN] = TPON.NUMPONATE) AND (TBL_CASH_PICKUP.[BRANCH CODE] =  TPON.CODFIL) AND (TBL_CASH_PICKUP.[CUSTOMER CODE-ORIGIN] = TPON.CODCLI)  " +
//"      INNER JOIN dbo.SCO_TUF TUF  " +
//"	  ON TPON.CODUF = TUF.CODUF  " +
//"	  INNER JOIN dbo.SCO_TTABMUN TMUN  " +
//"	  ON TPON.CODMUN = TMUN.CODMUN  " +
//"      INNER JOIN dbo.SCO_TTIPPTOATE TPOT  " +
//"      ON TPOT.codtipptoate = TPON.codtipptoate  " +
//"      LEFT JOIN dbo.SCO_TTABREG TREG  " +
//"      ON (TREG.codreg = TPON.codreg) and (TREG.codfil = TPON.codfil) AND (TBL_CASH_PICKUP.[CUSTOMER CODE-ORIGIN] = TPON.CODCLI)   " +
//"      WHERE TBL_CASH_PICKUP.[SERVICE NUMBER] NOT IN (21,22)  " +
//"AND TBL_CASH_PICKUP.[SERVICE SITUATION TYPE] IN ('CONCLUDED','CANCELLED','POINT CLOSED','NO CASH','ROUTE CLOSED','FINAL CLOSED') " +
////" and TBL_CASH_PICKUP.[CUSTOMER NAME-ORIGIN] not like '%ICICI%' " +
//"order by TBL_CASH_PICKUP.[BRANCH CODE] asc,TBL_CASH_PICKUP.[TIME OF DEPARTURE] desc";

                //" AND TBL_CASH_PICKUP.[SERVICE NUMBER] = 4169 and [ROUTE NUMBER] = 1975 and TBL_CASH_PICKUP.[BRANCH CODE]= 13 " +
                string strSQL = "SELECT TPOT.DESTIPPTOATE AS CUSTOMERTYPE," +
"	TREG.DESREG AS HUB,   " +
"	TBL_CASH_PICKUP.*, " +
"	TUF.DESUF,TMUN.NOMMUN, " +
"	TPON.REFPONATE AS CUST_CODE,  " +
"	TPON.BARPONATE ,   " +
"	TPON.OBSPONATE AS [ACCOUNT_NO_NEW], " +
"	TPON.CEPPONATE AS [PINCODE]  " +
"FROM   (SELECT  " +
"			CASE WHEN ROTA.[SERVICE SITUATION] = '1'  THEN 'PENDING'     " +
"				WHEN ROTA.[SERVICE SITUATION] = '2'  THEN 'TO BE CANCELLED'          " +
"				WHEN ROTA.[SERVICE SITUATION] = '3'  THEN 'CANCELLED'        " +
"				WHEN ROTA.[SERVICE SITUATION] = '4'  THEN 'TO BE INCLUDED' " +
"				WHEN ROTA.[SERVICE SITUATION] = '5'  THEN 'CONCLUDED'   " +
"				WHEN ROTA.[SERVICE SITUATION] = 'A'  THEN 'PERFORMING'     " +
"				WHEN ROTA.[SERVICE SITUATION] = 'B'  THEN 'ON THE WAY' " +
"				WHEN ROTA.[SERVICE SITUATION] = 'C'  THEN 'PENDING SOLN' " +
"				WHEN ROTA.[SERVICE SITUATION] = 'V'  THEN 'POINT CLOSED'   " +
"				WHEN ROTA.[SERVICE SITUATION] = '9'  THEN 'NO CASH'     " +
"				WHEN ROTA.[SERVICE SITUATION] = '6'  THEN 'ROUTE CLOSED'    " +
"				WHEN ROTA.[SERVICE SITUATION] = '7'  THEN 'FINAL CLOSED'    " +
"				ELSE 'UNKNOWN SITUATION'  " +
"			END AS [SERVICE SITUATION TYPE],  " +
"			ROTA.[S.NO],  " +
"			ROTA.[BRANCH CODE],   " +
"			ROTA.[BRANCH NAME],    " +
"			ROTA.[ROUTE NUMBER],    " +
"			ROTA.[ROUTE NAME],    " +
"			ROTA.[DEVICE ID],    " +
"			ROTA.[DATE],      " +
"			ROTA.[ROUTE START TIME-TM],         " +
"			ROTA.[ROUTE END TIME-TM],         " +
"			ROTA.[VEHICLE NUMBER],         " +
"			ROTA.CUSTODIAN_1,        " +
"			ROTA.CUSTODIAN_1_REG,      " +
"			ROTA.CUSTODIAN_2,       " +
"			ROTA.CUSTODIAN_2_REG,   " +
"			ROTA.GUNMAN_1,       " +
"			ROTA.GUNMAN_1_REG,    " +
"			ROTA.GUNMAN_2,    " +
"			ROTA.GUNMAN_2_REG,  " +
"			ROTA.DRIVER,    " +
"			ROTA.DRIVER_REG, " +
"			ROTA.[SERVICE SITUATION], " +
"			ROTA.[BILLING CLIENT CODE],      " +
"			ROTA.[BILING CLIENT NAME], " +
"			ROTA.[SERVICE NUMBER],    " +
"			ROTA.[SERVICE TYPE],    " +
"			ROTA.[CUSTOMER CODE-ORIGIN],  " +
"			ROTA.[CUSTOMER NAME-ORIGIN],  " +
"			ROTA.[SERVICE POINT NUMBER-ORIGIN],  " +
"			ROTA.[SERVICE POINT NAME-ORIGIN],      " +
"			RTRIM(ROTA.[SERVICE POINT ADDRESS-ORIGIN1]) AS [SERVICE POINT ADDRESS-ORIGIN1],   " +
"			ROTA.[SERVICE POINT ADDRESS-DELIVERY1],     " +
"			ROTA.CODCLASER,       " +
"			CASE WHEN CHARINDEX('//',ROTA.[SERVICE POINT ADDRESS-ORIGIN]) > 0  " +
"				THEN SUBSTRING(ROTA.[SERVICE POINT ADDRESS-ORIGIN],0,CHARINDEX('//',ROTA.[SERVICE POINT ADDRESS-ORIGIN]))  " +
"				ELSE 'NA'  " +
"			END AS CUSTOMER_CODE,           " +
"			CASE WHEN CHARINDEX('//',ROTA.[SERVICE POINT ADDRESS-ORIGIN]) > 0  " +
"				THEN SUBSTRING(SUBSTRING(ROTA.[SERVICE POINT ADDRESS-ORIGIN],CHARINDEX('//',ROTA.[SERVICE POINT ADDRESS-ORIGIN]) + 2, LEN(ROTA.[SERVICE POINT ADDRESS-ORIGIN])),0,CHARINDEX('//',SUBSTRING(ROTA.[SERVICE POINT ADDRESS-ORIGIN],     " +
"				CHARINDEX('//',ROTA.[SERVICE POINT ADDRESS-ORIGIN]) + 2,LEN(ROTA.[SERVICE POINT ADDRESS-ORIGIN]))))  " +
"				ELSE 'NA'  " +
"			END AS CRN_NO,           " +
"			CASE WHEN CHARINDEX('//',ROTA.[SERVICE POINT ADDRESS-ORIGIN]) > 0  " +
"				THEN SUBSTRING(SUBSTRING(ROTA.[SERVICE POINT ADDRESS-ORIGIN], CHARINDEX('//',ROTA.[SERVICE POINT ADDRESS-ORIGIN]) + 2, LEN(ROTA.[SERVICE POINT ADDRESS-ORIGIN])),CHARINDEX('//',SUBSTRING(ROTA.[SERVICE POINT ADDRESS-ORIGIN],              " +
"				 CHARINDEX('//',ROTA.[SERVICE POINT ADDRESS-ORIGIN]) + 2,LEN(ROTA.[SERVICE POINT ADDRESS-ORIGIN]))) + 2, LEN(SUBSTRING(ROTA.[SERVICE POINT ADDRESS-ORIGIN], CHARINDEX('//',ROTA.[SERVICE POINT ADDRESS-ORIGIN]), LEN(ROTA.[SERVICE POINT ADDRESS-ORIGIN]))))  " +
"				 ELSE 'NA'  " +
"			END AS ACCOUNT_NO,         " +
"			ROTA.[CUSTOMER CODE-DELIVERY],         " +
"			ROTA.[CUSTOMER NAME-DELIVERY],         " +
"			ROTA.[SERVICE POINT NUMBER-DELIVERY],         " +
"			ROTA.[SERVICE POINT NAME-DELIVERY],        " +
"			ROTA.[ACTUAL KM READING],         " +
"			ROTA.[TIME OF ARRIVAL],         " +
"			ROTA.[TIME OF DEPARTURE],         " +
"			ROTA.[TIME OF LAST ROUTE UPDATE],         " +
"			ROTA.[REMARKS],         " +
"			CASE WHEN (ROTA.DELIVERY_DAY = 'NEXT' OR ROTA.[CUSTOMER CODE-DELIVERY] = '0' OR ROTA.[CUSTOMER CODE-DELIVERY] = '999')  " +
"				THEN 'VAULT'  " +
"				ELSE 'BANK'  " +
"			END AS CASH_DEPOSIT_IN,         " +
"			CASE WHEN ROTA.[CUSTOMER CODE-ORIGIN] = 0 AND ROTA.[CUSTOMER CODE-DELIVERY] = 0 THEN 'ADMINISTRATIVE'               " +
"				WHEN ROTA.[CUSTOMER CODE-ORIGIN] = 0 AND ROTA.[CUSTOMER CODE-DELIVERY] <> 0 THEN 'DELIVERY'              " +
"				WHEN ROTA.[CUSTOMER CODE-ORIGIN] <> 0 AND ROTA.[CUSTOMER CODE-DELIVERY] = 0 THEN 'PICKUP'         " +
"				ELSE 'TRANSIT'  " +
"			END AS [TYPE OF SERVICE],               " +
"			CASE WHEN ROTA.[CUSTOMER CODE-ORIGIN] = 0 AND ROTA.[CUSTOMER CODE-DELIVERY] = 0 THEN '0'               " +
"				WHEN ROTA.[CUSTOMER CODE-ORIGIN] = 0 AND ROTA.[CUSTOMER CODE-DELIVERY] <> 0 THEN '1'               " +
"				WHEN ROTA.[CUSTOMER CODE-ORIGIN] <> 0 AND ROTA.[CUSTOMER CODE-DELIVERY] = 0 THEN '2'         " +
"				ELSE '3'  " +
"			END AS [CODE TYPE OF SERVICE],       " +
"			ISNULL(GUICOL.NUMGUI, GUIENT.NUMGUI) AS [SLIP NUMBER],   " +
"			ISNULL(GUICOL.NUMLAC1, GUIENT.NUMLAC1) AS [SEAL NUMBER],   " +
"			ISNULL(SUM(ISNULL(GUICOL.[C_INR_0.1],GUIENT.[C_INR_0.1])),0) AS [M_INR_0.1],   " +
"			ISNULL(SUM(ISNULL(GUICOL.[C_INR_0.50],GUIENT.[C_INR_0.50])),0) AS [M_INR_0.50],   " +
"			ISNULL(SUM(ISNULL(GUICOL.C_INR_1,GUIENT.C_INR_1)),0) AS C_INR_1,   " +
"			ISNULL(SUM(ISNULL(GUICOL.C_INR_2,GUIENT.C_INR_2)),0) AS C_INR_2,   " +
"			ISNULL(SUM(ISNULL(GUICOL.C_INR_5,GUIENT.C_INR_5)),0) AS C_INR_5,   " +
"			ISNULL(SUM(ISNULL(GUICOL.C_INR_10,GUIENT.C_INR_10)),0) AS C_INR_10,   " +
"			ISNULL(ISNULL(GUICOL.VALMOE,GUIENT.VALMOE),0) AS [COINS-AMOUNT],   " +
"			ISNULL(SUM(ISNULL(GUICOL.INR_10,GUIENT.INR_10)),0) AS INR_10,    " +
"			ISNULL(SUM(ISNULL(GUICOL.INR_20,GUIENT.INR_20)),0) AS INR_20,    " +
"			ISNULL(SUM(ISNULL(GUICOL.INR_50,GUIENT.INR_50)),0) AS INR_50,    " +
"			ISNULL(SUM(ISNULL(GUICOL.INR_100,GUIENT.INR_100)),0) AS INR_100,    " +
"			ISNULL(SUM(ISNULL(GUICOL.INR_500,GUIENT.INR_500)),0) AS INR_500,    " +
"			ISNULL(SUM(ISNULL(GUICOL.INR_1000,GUIENT.INR_1000)),0) AS INR_1000,   " +
"			ISNULL(SUM(ISNULL(GUICOL.INR_2000,GUIENT.INR_2000)),0) AS INR_2000,   " +
"			ISNULL((ISNULL(GUICOL.VALDIN,GUIENT.VALDIN) - ISNULL(GUICOL.VALMOE,GUIENT.VALMOE)),0) AS [CASH-AMOUNT],   " +
"			ISNULL(SUM(ISNULL(GUICOL.ITERTV10,GUIENT.ITERTV10)),0) AS [BULLION],   " +
"			ISNULL(SUM(ISNULL(GUICOL.ITERTV3,GUIENT.ITERTV3)),0) AS [INTRUMENTS],   " +
"			ISNULL(SUM(ISNULL(GUICOL.ITERTV9,GUIENT.ITERTV9)),0) AS [OTHERS],   " +
"			ISNULL(ISNULL(GUICOL.VALDIN, GUIENT.VALDIN),0) AS [TOTAL AMOUNT/QUANTITY],   " +
"			ISNULL(SUM(ISNULL(GUICOL.INR_200,GUIENT.INR_200)),0) AS INR_200,   " +
"			ISNULL(GUICOL.OBSNAOATE,GUIENT.OBSNAOATE) AS MANUAL_REMARK,  " +
"			ISNULL(GUICOL.DESJUSNAOATE,GUIENT.DESJUSNAOATE) AS REASON,  " +
"			ISNULL(GUICOL.SCRCAR,GUIENT.SCRCAR) AS SCRCAR,  " +
"			ROTA.ITEFOLROTID   " +
"FROM (( SELECT DISTINCT  ITE.ITEFOLROTID AS [ITEFOLROTID]," +
"					ITE.SITITEFOLROT AS [SERVICE SITUATION],   " +
"					ITE.SEQFOLROT AS [S.NO],   " +
"					ITE.CODFIL AS [BRANCH CODE],   " +
"					FIL.NOMFIL AS [BRANCH NAME],   " +
"					ITE.NUMROT AS [ROUTE NUMBER],   " +
"					FOL.DESROT AS [ROUTE NAME],   " +
"					TM.COD_SERIE AS [DEVICE ID],   " +
"					ITE.DATSER AS [DATE],   " +
"					FOL.HORINIREA AS [ROUTE START TIME-TM],   " +
"					FOL.HORFIMREA AS [ROUTE END TIME-TM],   " +
"					VEH.PLAVEI AS [VEHICLE NUMBER],   " +
"					ITE.DES_SOLUCAO AS [REMARKS],     " +
"					(SELECT MAX(FUN.NOMCOM)           " +
"					 FROM SCO_TCADEQU EQ WITH (NOLOCK)      " +
"						INNER JOIN SCO_TCADFUC AS FUC WITH (NOLOCK)  " +
"							ON EQ.CODFUC = FUC.CODFUC      " +
"						INNER JOIN SCO_TCADFUN AS FUN WITH (NOLOCK)  " +
"							ON EQ.MATFUN = FUN.MATFUN        " +
"					 WHERE EQ.CODFIL = ITE.CODFIL  " +
"						AND EQ.DATSERABE = ITE.DATSER  " +
"						AND EQ.NUMROTABE = ITE.NUMROT     " +
"						AND FUC.CODFUC IN (3,8,11)) AS CUSTODIAN_1,       " +
"					(SELECT MAX(FUN.REGNOERP)           " +
"					 FROM SCO_TCADEQU EQ WITH (NOLOCK)      " +
"						INNER JOIN SCO_TCADFUC AS FUC WITH (NOLOCK)  " +
"							ON EQ.CODFUC = FUC.CODFUC      " +
"						INNER JOIN SCO_TCADFUN AS FUN WITH (NOLOCK)  " +
"							ON EQ.MATFUN = FUN.MATFUN        " +
"					 WHERE EQ.CODFIL = ITE.CODFIL  " +
"						AND EQ.DATSERABE = ITE.DATSER  " +
"						AND EQ.NUMROTABE = ITE.NUMROT     " +
"						AND FUC.CODFUC IN (3,8,11)) AS CUSTODIAN_1_REG,        " +
"					(SELECT MIN(FUN.NOMCOM)             " +
"					 FROM SCO_TCADEQU EQ WITH (NOLOCK)      " +
"						INNER JOIN SCO_TCADFUC AS FUC WITH (NOLOCK)  " +
"							ON EQ.CODFUC = FUC.CODFUC      " +
"						INNER JOIN SCO_TCADFUN AS FUN WITH (NOLOCK)  " +
"							ON EQ.MATFUN = FUN.MATFUN        " +
"					 WHERE EQ.CODFIL = ITE.CODFIL  " +
"						AND EQ.DATSERABE = ITE.DATSER  " +
"						AND EQ.NUMROTABE = ITE.NUMROT     " +
"						AND FUC.CODFUC IN (3,8,11)) AS CUSTODIAN_2,          " +
"					(SELECT MIN(FUN.REGNOERP)             " +
"					 FROM SCO_TCADEQU EQ WITH (NOLOCK)      " +
"						INNER JOIN SCO_TCADFUC AS FUC WITH (NOLOCK)  " +
"							ON EQ.CODFUC = FUC.CODFUC      " +
"						INNER JOIN SCO_TCADFUN AS FUN WITH (NOLOCK)  " +
"							ON EQ.MATFUN = FUN.MATFUN        " +
"					 WHERE EQ.CODFIL = ITE.CODFIL  " +
"						AND EQ.DATSERABE = ITE.DATSER  " +
"						AND EQ.NUMROTABE = ITE.NUMROT     " +
"						AND FUC.CODFUC IN (3,8,11)) AS CUSTODIAN_2_REG,          " +
"					(SELECT MAX(FUN.NOMCOM)             " +
"					 FROM SCO_TCADEQU EQ WITH (NOLOCK)      " +
"						INNER JOIN SCO_TCADFUC AS FUC WITH (NOLOCK)  " +
"							ON EQ.CODFUC = FUC.CODFUC      " +
"						INNER JOIN SCO_TCADFUN AS FUN WITH (NOLOCK)  " +
"							ON EQ.MATFUN = FUN.MATFUN        " +
"					 WHERE EQ.CODFIL = ITE.CODFIL  " +
"						AND EQ.DATSERABE = ITE.DATSER  " +
"						AND EQ.NUMROTABE = ITE.NUMROT     " +
"						AND FUC.CODFUC IN (2,9)) AS GUNMAN_1,          " +
"					(SELECT MAX(FUN.REGNOERP)             " +
"					 FROM SCO_TCADEQU EQ WITH (NOLOCK)      " +
"						INNER JOIN SCO_TCADFUC AS FUC WITH (NOLOCK)  " +
"							ON EQ.CODFUC = FUC.CODFUC      " +
"						INNER JOIN SCO_TCADFUN AS FUN WITH (NOLOCK)  " +
"							ON EQ.MATFUN = FUN.MATFUN        " +
"					 WHERE EQ.CODFIL = ITE.CODFIL  " +
"						AND EQ.DATSERABE = ITE.DATSER  " +
"						AND EQ.NUMROTABE = ITE.NUMROT     " +
"						AND FUC.CODFUC IN (2,9)) AS GUNMAN_1_REG,        " +
"					(SELECT MIN(FUN.NOMCOM)             " +
"					 FROM SCO_TCADEQU EQ WITH (NOLOCK)      " +
"						INNER JOIN SCO_TCADFUC AS FUC WITH (NOLOCK)  " +
"							ON EQ.CODFUC = FUC.CODFUC      " +
"						INNER JOIN SCO_TCADFUN AS FUN WITH (NOLOCK)  " +
"							ON EQ.MATFUN = FUN.MATFUN        " +
"					 WHERE EQ.CODFIL = ITE.CODFIL  " +
"						AND EQ.DATSERABE = ITE.DATSER  " +
"						AND EQ.NUMROTABE = ITE.NUMROT     " +
"						AND FUC.CODFUC IN (2,9)) AS GUNMAN_2,          " +
"					 (SELECT MIN(FUN.REGNOERP)             " +
"					  FROM SCO_TCADEQU EQ WITH (NOLOCK)      " +
"						INNER JOIN SCO_TCADFUC AS FUC WITH (NOLOCK)  " +
"							ON EQ.CODFUC = FUC.CODFUC      " +
"						INNER JOIN SCO_TCADFUN AS FUN WITH (NOLOCK)  " +
"							ON EQ.MATFUN = FUN.MATFUN        " +
"					 WHERE EQ.CODFIL = ITE.CODFIL  " +
"						AND EQ.DATSERABE = ITE.DATSER  " +
"						AND EQ.NUMROTABE = ITE.NUMROT     " +
"						AND FUC.CODFUC IN (2,9)) AS GUNMAN_2_REG,     " +
"					(SELECT MIN(FUN.NOMCOM)             " +
"					 FROM SCO_TCADEQU EQ WITH (NOLOCK)      " +
"						INNER JOIN SCO_TCADFUC AS FUC WITH (NOLOCK)  " +
"							ON EQ.CODFUC = FUC.CODFUC      " +
"						INNER JOIN SCO_TCADFUN AS FUN WITH (NOLOCK)  " +
"							ON EQ.MATFUN = FUN.MATFUN        " +
"					 WHERE EQ.CODFIL = ITE.CODFIL  " +
"						AND EQ.DATSERABE = ITE.DATSER  " +
"						AND EQ.NUMROTABE = ITE.NUMROT     " +
"						AND FUC.CODFUC = 1 ) AS DRIVER,       " +
"					(SELECT MIN(FUN.REGNOERP)             " +
"					 FROM SCO_TCADEQU EQ WITH (NOLOCK)      " +
"						INNER JOIN SCO_TCADFUC AS FUC WITH (NOLOCK)  " +
"							ON EQ.CODFUC = FUC.CODFUC      " +
"						INNER JOIN SCO_TCADFUN AS FUN WITH (NOLOCK)  " +
"							ON EQ.MATFUN = FUN.MATFUN        " +
"					 WHERE EQ.CODFIL = ITE.CODFIL  " +
"						AND EQ.DATSERABE = ITE.DATSER  " +
"						AND EQ.NUMROTABE = ITE.NUMROT     " +
"						AND FUC.CODFUC = 1 ) AS DRIVER_REG,     " +
"					SERORI.CODCLI AS [BILLING CLIENT CODE],   " +
"					CLIORISER.NOMFAN AS [BILING CLIENT NAME],   " +
"					ITE.NUMSER AS [SERVICE NUMBER],   " +
"					TIP.DESTIPSER AS [SERVICE TYPE],   " +
"					PARORI.CODCLI AS [CUSTOMER CODE-ORIGIN],   " +
"					CLIORI.NOMFAN AS [CUSTOMER NAME-ORIGIN],   " +
"					ISNULL(PONATEORI.NUMPONATE,PONATEATMORI.NUMPONATE) AS [SERVICE POINT NUMBER-ORIGIN],   " +
"					ISNULL(PONATEORI.NOMPONATE,PONATEATMORI.NOMPONATE) AS [SERVICE POINT NAME-ORIGIN],   " +
"					ISNULL(PONATEORI.OBSPONATE,PONATEATMORI.OBSPONATE) AS [SERVICE POINT ADDRESS-ORIGIN],   " +
"					ISNULL(PONATEORI.ENDPON,PONATEATMORI.ENDPON) AS [SERVICE POINT ADDRESS-ORIGIN1],   " +
"					SERORI.CODCLASER,   " +
"					PARDES.CODCLI AS [CUSTOMER CODE-DELIVERY],   " +
"					CLIDES.NOMFAN AS [CUSTOMER NAME-DELIVERY],   " +
"					ISNULL(PONATEDES.NUMPONATE,PONATEATMDES.NUMPONATE ) AS [SERVICE POINT NUMBER-DELIVERY],   " +
"					ISNULL(PONATEDES.NOMPONATE,PONATEATMDES.NOMPONATE) AS [SERVICE POINT NAME-DELIVERY],   " +
"					ISNULL(PONATEDES.OBSPONATE,PONATEATMDES.OBSPONATE) AS [SERVICE POINT ADDRESS-DELIVERY],     " +
"					ISNULL(PONATEDES.ENDPON,PONATEATMDES.ENDPON) AS [SERVICE POINT ADDRESS-DELIVERY1],   " +
"					CASE WHEN SERORI.FLGENTPRODIA = '1'  " +
"						THEN 'NEXT'  " +
"						ELSE 'SAME'  " +
"					END AS DELIVERY_DAY,   " +
"					ITE.ODOVEI AS [ACTUAL KM READING],   " +
"					ITE.HORCHEREA AS [TIME OF ARRIVAL],   " +
"					ITE.HORSAIREA AS [TIME OF DEPARTURE],   " +
"					CONVERT(NVARCHAR(8), FOL.DATULTALT,114 ) AS [TIME OF LAST ROUTE UPDATE]            " +
"				FROM   SCO_TGRARISSEG RISATM (NOLOCK)  " +
"					RIGHT OUTER JOIN SCO_TPONATE PONATM (NOLOCK)   " +
"						ON RISATM.CODGRARISSEG = PONATM.CODGRARISSEG  " +
"					RIGHT OUTER JOIN SCO_TGRARISSEG RISPON (NOLOCK)  " +
"					RIGHT OUTER JOIN SCO_TPONATE PON  (NOLOCK)  " +
"						ON RISPON.CODGRARISSEG = PON.CODGRARISSEG  " +
"					RIGHT OUTER JOIN SCO_TCADATM ATM  (NOLOCK)  " +
"					RIGHT OUTER JOIN SCO_TITEFOLROT ITE (NOLOCK)  " +
"					LEFT OUTER JOIN  SCO_TCLASER CLA (NOLOCK)  " +
"						ON ITE.CODCLASER = CLA.CODCLASER  " +
"					LEFT OUTER JOIN  SCO_TTABCLI CLI  (NOLOCK)  " +
"						ON ITE.CODCLI = CLI.CODCLI  " +
"						ON ATM.CODCLI = ITE.CODCLI  " +
"							AND ATM.CODFIL = ITE.CODFIL  " +
"							AND  ATM.NUMATM = ITE.NUMATM  " +
"					LEFT OUTER JOIN  SCO_TTIPSER TIP  " +
"						ON ITE.CODTIPSER = TIP.CODTIPSER  " +
"						ON PON.NUMPONATE = ITE.NUMPON  " +
"							AND PON.CODFIL = ITE.CODFIL  " +
"							AND PON.CODCLI = ITE.CODCLI  " +
"						ON PONATM.CODPONATEID = ATM.CODPONATEIDLOC  " +
"					LEFT OUTER JOIN  SCO_TFOLROT  FOL (NOLOCK)               " +
"						ON ITE.CODFIL = FOL.CODFIL  " +
"							AND ITE.DATSER = FOL.DATSER                   " +
"							AND ITE.NUMROT = FOL.NUMROT           " +
"					LEFT OUTER JOIN SCO_TPONATE PONORI (NOLOCK)            " +
"						ON ITE.CODFIL = PONORI.CODFIL  " +
"							AND ITE.CODCLI = PONORI.CODCLI  " +
"							AND ITE.NUMPON = PONORI.NUMPONATE            " +
"					LEFT OUTER JOIN TPBR_TPDA PDA (NOLOCK)  " +
"						ON FOL.OID_PDA = PDA.OID_PDA     " +
"					LEFT OUTER JOIN SCO_TPAREXT EXT (NOLOCK)       " +
"						ON EXT.CODDOM = 234           " +
"							AND EXT.CODFIL = ITE.CODFIL           " +
"							AND EXT.CODCLI = ITE.CODCLI          " +
"							AND EXT.CODVALDOM = CASE WHEN ITE.NUMATM IS NULL THEN 2 ELSE 1 END   " +
"					INNER JOIN SCO_TCADFIL FIL (NOLOCK)  " +
"						ON ITE.CODFIL = FIL.CODFIL     " +
"					LEFT OUTER JOIN SCO_TCADSER SERORI (NOLOCK)               " +
"						ON SERORI.CODFIL = ITE.CODFIL                      " +
"						AND SERORI.NUMPED = ITE.NUMSER    " +
"					INNER JOIN SCO_TTABCLI CLIORISER               " +
"						ON CLIORISER.CODCLI = SERORI.CODCLI   " +
"					INNER JOIN SCO_TPARSER PARORI (NOLOCK)               " +
"						ON PARORI.CODFIL = SERORI.CODFIL                     " +
"							AND PARORI.NUMSER = SERORI.NUMPED                      " +
"							AND PARORI.SEQSER = 1   " +
"					LEFT OUTER JOIN SCO_TPONATE PONATEORI               " +
"						ON PONATEORI.CODFIL = PARORI.CODFIL                      " +
"							AND PONATEORI.NUMPONATE = PARORI.NUMPONATE                     " +
"							AND PONATEORI.CODCLI = PARORI.CODCLI   " +
"					LEFT OUTER JOIN SCO_TCADATM ATMORI               " +
"						ON ATMORI.CODFIL = PARORI.CODFIL                     " +
"							AND ATMORI.CODCLI = PARORI.CODCLI                     " +
"							AND ATMORI.NUMATM = PARORI.NUMATM    " +
"					LEFT OUTER JOIN SCO_TPONATE PONATEATMORI               " +
"						ON PONATEATMORI.CODFIL = ATMORI.CODFIL                      " +
"							AND PONATEATMORI.NUMPONATE = ATMORI.NUMPONATERES                     " +
"							AND PONATEATMORI.CODCLI = ATMORI.CODCLI   " +
"					INNER JOIN SCO_TTABCLI CLIORI               " +
"						ON CLIORI.CODCLI = PARORI.CODCLI     " +
"					LEFT OUTER JOIN SCO_TCADSER SERDES (NOLOCK)               " +
"						ON SERDES.CODFIL = ITE.CODFIL                      " +
"							AND SERDES.NUMPED = ITE.NUMSER    " +
"					INNER JOIN SCO_TPARSER PARDES (NOLOCK)               " +
"						ON PARDES.CODFIL = SERDES.CODFIL                     " +
"							AND PARDES.NUMSER = SERDES.NUMPED                      " +
"							AND PARDES.SEQSER = 2   " +
"					LEFT OUTER JOIN SCO_TPONATE PONATEDES               " +
"						ON PONATEDES.CODFIL = PARDES.CODFIL                      " +
"							AND PONATEDES.NUMPONATE = PARDES.NUMPONATE                     " +
"							AND PONATEDES.CODCLI = PARDES.CODCLI   " +
"					LEFT OUTER JOIN SCO_TCADATM ATMDES               " +
"						ON ATMDES.CODFIL = PARDES.CODFIL                     " +
"							AND ATMDES.CODCLI = PARDES.CODCLI                     " +
"							AND ATMDES.NUMATM = PARDES.NUMATM    " +
"					LEFT OUTER JOIN SCO_TPONATE PONATEATMDES              " +
"						ON PONATEATMDES.CODFIL = ATMDES.CODFIL                      " +
"							AND PONATEATMDES.NUMPONATE = ATMDES.NUMPONATERES                     " +
"							AND PONATEATMDES.CODCLI = ATMDES.CODCLI   " +
"					INNER JOIN SCO_TTABCLI CLIDES               " +
"						ON CLIDES.CODCLI = PARDES.CODCLI     " +
"					LEFT OUTER JOIN TPBR_TPDA TM (NOLOCK)               " +
"						ON TM.OID_PDA = FOL.OID_PDA   " +
"					LEFT OUTER JOIN SCO_TCADVEI VEH         " +
"						ON FOL.NUMVEIABE = VEH.NUMVEI   " +
"				WHERE ITE.DATSER >= '"+ strDate +"'" +
"					AND ITE.DATSER <= '" + strDate + "'  " +
"					AND ((((CAST(CONVERT(VARCHAR(5), HORCHEPRE, 8) AS DATETIME) BETWEEN '00:00:00' AND '23:59:59') OR HORCHEPRE IS NULL))    " +
"					OR ((DATEDIFF(N, CAST(CONVERT(VARCHAR(5), HORSAIPRE, 8) AS DATETIME), '06:00:00' ) > 0) " +
"					 AND ((CAST(CONVERT(VARCHAR(5), HORCHEPRE, 8) AS DATETIME) BETWEEN '00:00:00' AND '23:59:59') OR HORCHEPRE IS NULL) )) /*AND  ITE.CODMACREG IN (2,1,8) AND   ITE.NUMROT NOT IN(201,202)*/   ) AS ROTA     " +
"LEFT OUTER JOIN  (SELECT ISNULL(GUI.SLIPNUM, GUI.NUMGUI) AS NUMGUI, " +
"						GUI.NUMROTCOL,  " +
"						GUI.SEQROTCOL,  " +
"						MAL.NUMLAC1, " +
"						GUI.DATCOL, " +
"						GUI.NUMPED, " +
"						GUI.CODFIL,        " +
"						ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS) * (ISNULL(COMP1.PES_COMP, COMP.PES_COMP)) AS VALORGTV,         " +
"						SUM(ITEGTVENT9.QTDITERTV) AS ITERTV9,    " +
"						SUM(ITEGTVENT2.QTDITERTV) AS ITERTV2,         " +
"						SUM(ITEGTVENT10.QTDITERTV) AS ITERTV10,         " +
"						SUM(ITEGTVENT3.QTDITERTV) AS ITERTV3,         " +
"						MAL.VALDIN AS VALDIN,         " +
"						MAL.VALMOE AS VALMOE,           " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  " +
"							WHEN  12 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS INR_10,            " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  " +
"							WHEN  1 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS INR_20,           " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  " +
"							WHEN  2 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS INR_50,            " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  " +
"							WHEN  3 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS INR_100,           " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  " +
"							WHEN  5 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS INR_500,         " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  " +
"							WHEN  6 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS INR_1000,         " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  " +
"							WHEN  13 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS INR_2000,         " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  " +
"							WHEN  14 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS INR_200,         " +
"						CASE ISNULL(CONT.COD_ESP,PEDMAL.COD_ESP)   " +
"							WHEN  2 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS MOEDAS,         " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)   " +
"							WHEN  15 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS [C_INR_0.1],         " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)   " +
"							WHEN  9 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS [C_INR_0.50],         " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)   " +
"							WHEN  7 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS C_INR_1,         " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)   " +
"							WHEN  8 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS C_INR_2,         " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)   " +
"							WHEN  10 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS C_INR_5,         " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)   " +
"							WHEN  11 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS C_INR_10,    " +
"						REMRK.OBSNAOATE,  " +
"						CASE WHEN REMRK.CODJUSNAOATE = 1 THEN 'DUE TO BANK'        " +
"							WHEN REMRK.CODJUSNAOATE = 2 THEN 'DUE TO MSP'        " +
"							WHEN REMRK.CODJUSNAOATE = 3 THEN 'DUE TO SIS-PROSEGUR'        " +
"							ELSE 'THE PICKUP POINT IS CLOSED'  " +
"						END AS DESJUSNAOATE,  " +
"						GUI.SCRCAR  " +
"					FROM SCO_TCADPOOGUI GUI (NOLOCK)    " +
"						LEFT OUTER JOIN SCO_TCADJUSNAOATELOG REMRK (NOLOCK)  " +
"							ON (GUI.RTVID = REMRK.RTVID) " +
"						LEFT OUTER JOIN  SCO_TCADMAL MAL (NOLOCK)              " +
"							 ON GUI.CODFIL = MAL.CODFIL                      " +
"								AND GUI.DATCOL = MAL.DATCOL                     " +
"								AND GUI.NUMGUI = MAL.NUMGUI                     " +
"								AND GUI.SERGUI = MAL.SERGUI                     " +
"								AND GUI.CODORIRTV = MAL.CODORIRTV   " +
"						LEFT OUTER JOIN SCO_TPLANCONTAGEM CONT (NOLOCK)               " +
"							ON CONT.COD_FIL = MAL.CODFIL                     " +
"								AND CONT.NUM_GUI = MAL.NUMGUI                     " +
"								AND CONT.SER_GUI = MAL.SERGUI                     " +
"								AND CONT.COD_ORI_RTV = MAL.CODORIRTV                     " +
"								AND CONT.DATCOL = MAL.DATCOL                     " +
"								AND CONT.NUM_MAL = MAL.NUMMAL   " +
"						LEFT OUTER JOIN SCO_TCADCOMPOSICAO COMP1                " +
"							ON COMP1.COD_COMP = CONT.COD_COM " +
"								AND COMP1.COD_ESP = CONT.COD_ESP   " +
"						LEFT OUTER JOIN SCO_TCADMAL MALPED (NOLOCK)               " +
"							ON MALPED.CODFIL = GUI.CODFIL                      " +
"								AND MALPED.DATCOL = GUI.DATCOL                     " +
"								AND MALPED.NUMGUI = GUI.NUMGUI                     " +
"								AND MALPED.SERGUI = GUI.SERGUI                     " +
"								AND MALPED.CODORIRTV = GUI.CODORIRTV   " +
"						LEFT OUTER JOIN SCO_TCOMPMALOTE PEDMAL (NOLOCK)               " +
"							ON PEDMAL.COD_FIL = MALPED.CODFIL                     " +
"								AND PEDMAL.NUM_GUI = MALPED.NUMGUI                     " +
"								AND PEDMAL.SER_GUI = MALPED.SERGUI                     " +
"								AND PEDMAL.COD_ORI_RTV = MALPED.CODORIRTV                    " +
"								AND PEDMAL.DATCOL = MALPED.DATCOL                     " +
"								AND PEDMAL.NUM_MAL = MALPED.NUMMAL    " +
"						LEFT OUTER JOIN SCO_TCADCOMPOSICAO COMP (NOLOCK)               " +
"							ON COMP.COD_COMP = PEDMAL.COD_COMP                     " +
"								AND COMP.COD_ESP = PEDMAL.COD_ESP                 " +
"						LEFT OUTER JOIN SCO_TITERTV ITEGTVENT9 (NOLOCK)                           " +
"							ON ITEGTVENT9.CODFIL = GUI.CODFIL                                 " +
"								AND ITEGTVENT9.NUMGUI = GUI.NUMGUI                                 " +
"								AND ITEGTVENT9.SERGUI = GUI.SERGUI                                 " +
"								AND ITEGTVENT9.CODORIRTV = GUI.CODORIRTV                                 " +
"								AND ITEGTVENT9.DATCOL = GUI.DATCOL                                 " +
"								AND ITEGTVENT9.CODTIPITERTV = 12                             " +
"						LEFT OUTER JOIN SCO_TITERTV ITEGTVENT2 (NOLOCK)                           " +
"							ON ITEGTVENT2.CODFIL = GUI.CODFIL                                 " +
"								AND ITEGTVENT2.NUMGUI = GUI.NUMGUI                                 " +
"								AND ITEGTVENT2.SERGUI = GUI.SERGUI                                 " +
"								AND ITEGTVENT2.CODORIRTV = GUI.CODORIRTV                                 " +
"								AND ITEGTVENT2.DATCOL = GUI.DATCOL                                 " +
"								AND ITEGTVENT2.CODTIPITERTV = 2                 " +
"						LEFT OUTER JOIN SCO_TITERTV ITEGTVENT10 (NOLOCK)                           " +
"							ON ITEGTVENT10.CODFIL = GUI.CODFIL                                 " +
"								AND ITEGTVENT10.NUMGUI = GUI.NUMGUI                                 " +
"								AND ITEGTVENT10.SERGUI = GUI.SERGUI                                 " +
"								AND ITEGTVENT10.CODORIRTV = GUI.CODORIRTV                                 " +
"								AND ITEGTVENT10.DATCOL = GUI.DATCOL                                 " +
"								AND ITEGTVENT10.CODTIPITERTV = 10                 " +
"						LEFT OUTER JOIN SCO_TITERTV ITEGTVENT3 (NOLOCK)                           " +
"							ON ITEGTVENT3.CODFIL = GUI.CODFIL                                 " +
"								AND ITEGTVENT3.NUMGUI = GUI.NUMGUI                                 " +
"								AND ITEGTVENT3.SERGUI = GUI.SERGUI                                 " +
"								AND ITEGTVENT3.CODORIRTV = GUI.CODORIRTV                                 " +
"								AND ITEGTVENT3.DATCOL = GUI.DATCOL                                 " +
"								AND ITEGTVENT3.CODTIPITERTV = 11                               " +
"					WHERE GUI.DATCOL >= '" + strDate + "' " +
"						AND   GUI.DATCOL <= '" + strDate + "'  " +
"					GROUP BY GUI.DATCOL, " +
"						GUI.NUMPED, " +
"						GUI.CODFIL, " +
"						CONT.COD_COM,  " +
"						PEDMAL.COD_COMP,  " +
"						ISNULL(GUI.SLIPNUM, GUI.NUMGUI),  " +
"						GUI.NUMROTCOL,  " +
"						GUI.SEQROTCOL, " +
"						CONT.COD_ESP, " +
"						PEDMAL.COD_ESP, " +
"						MAL.NUMLAC1,   " +
"						CONT.QTD_CED_CONT,  " +
"						PEDMAL.QTD_ITENS, " +
"						COMP1.PES_COMP,  " +
"						COMP.PES_COMP,  " +
"						MAL.VALDIN,  " +
"						MAL.VALMOE,  " +
"						REMRK.OBSNAOATE, " +
"						REMRK.CODJUSNAOATE, GUI.SCRCAR  ) AS GUICOL   " +
"		ON GUICOL.NUMROTCOL = ROTA.[ROUTE NUMBER]         " +
"			AND   GUICOL.SEQROTCOL = ROTA.[S.NO]  	 " +
"			AND ROTA.[DATE] = GUICOL.DATCOL 	 " +
"			AND ROTA.[SERVICE NUMBER] = GUICOL.NUMPED 	 " +
"			AND ROTA.[BRANCH CODE] = GUICOL.CODFIL    " +
"	LEFT OUTER JOIN ( SELECT ISNULL(GUI.SLIPNUM, GUI.NUMGUI) AS NUMGUI,  " +
"						GUI.NUMROTENT,  " +
"						GUI.SEQROTENT,  " +
"						MAL.NUMLAC1,  " +
"						GUI.DATCOL, " +
"						GUI.NUMPED, " +
"						GUI.CODFIL,                " +
"						ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS) * (ISNULL(COMP1.PES_COMP, COMP.PES_COMP)) AS VALORGTV,               " +
"						SUM(ITEGTVENT9.QTDITERTV) AS ITERTV9,               " +
"						SUM(ITEGTVENT2.QTDITERTV) AS ITERTV2,               " +
"						SUM(ITEGTVENT10.QTDITERTV) AS ITERTV10,               " +
"						SUM(ITEGTVENT3.QTDITERTV) AS ITERTV3,               " +
"						MAL.VALDIN AS VALDIN,                  " +
"						MAL.VALMOE AS VALMOE,                 " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  " +
"							WHEN  12 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS INR_10,                  " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  " +
"							WHEN  1 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS INR_20,                 " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  " +
"							WHEN  2 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS INR_50,                  " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  " +
"							WHEN  3 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS INR_100,                 " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  " +
"							WHEN  5 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS INR_500,               " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  " +
"							WHEN  6 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS INR_1000,               " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  " +
"							WHEN  13 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS INR_2000,               " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  " +
"							WHEN  14 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS INR_200,               " +
"						CASE ISNULL(CONT.COD_ESP,PEDMAL.COD_ESP)   " +
"							WHEN  2 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS MOEDAS,               " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)  " +
"							 WHEN  15 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS [C_INR_0.1],               " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)   " +
"							WHEN  9 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS [C_INR_0.50],               " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)   " +
"							WHEN  7 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS C_INR_1,               " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)   " +
"							WHEN  8 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS C_INR_2,               " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)   " +
"							WHEN  10 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT) " +
"						END AS C_INR_5,               " +
"						CASE ISNULL(CONT.COD_COM,PEDMAL.COD_COMP)   " +
"							WHEN  11 THEN CAST(MAX(ISNULL(CONT.QTD_CED_CONT, PEDMAL.QTD_ITENS)) AS INT)  " +
"						END AS C_INR_10,  REMRK.OBSNAOATE,  " +
"						CASE WHEN REMRK.CODJUSNAOATE = 1 THEN 'DUE TO BANK'        " +
"							WHEN REMRK.CODJUSNAOATE = 2 THEN 'DUE TO MSP'        " +
"							WHEN REMRK.CODJUSNAOATE = 3 THEN 'DUE TO SIS-PROSEGUR'        " +
"							ELSE 'THE PICKUP POINT IS CLOSED'  " +
"						END AS DESJUSNAOATE, GUI.SCRCAR                  " +
"					FROM SCO_TCADPOOGUI GUI (NOLOCK)    " +
"						LEFT OUTER JOIN SCO_TCADJUSNAOATELOG REMRK (NOLOCK)  " +
"							ON (GUI.RTVID = REMRK.RTVID)            " +
"						LEFT OUTER JOIN SCO_TCADMAL MAL (NOLOCK)                           " +
"							ON GUI.CODFIL = MAL.CODFIL                                  " +
"								AND GUI.DATCOL = MAL.DATCOL                                 " +
"								AND GUI.NUMGUI = MAL.NUMGUI                                 " +
"								AND GUI.SERGUI = MAL.SERGUI                                 " +
"								AND GUI.CODORIRTV = MAL.CODORIRTV               " +
"						LEFT OUTER JOIN SCO_TPLANCONTAGEM CONT (NOLOCK)                           " +
"							ON CONT.COD_FIL = MAL.CODFIL                                 " +
"								AND CONT.NUM_GUI = MAL.NUMGUI                                 " +
"								AND CONT.SER_GUI = MAL.SERGUI                                 " +
"								AND CONT.COD_ORI_RTV = MAL.CODORIRTV                                 " +
"								AND CONT.DATCOL = MAL.DATCOL                                 " +
"								AND CONT.NUM_MAL = MAL.NUMMAL               " +
"						LEFT OUTER JOIN SCO_TCADCOMPOSICAO COMP1 (NOLOCK)                          " +
"							ON COMP1.COD_COMP = CONT.COD_COM " +
"								AND COMP1.COD_ESP = CONT.COD_ESP               " +
"						LEFT OUTER JOIN SCO_TCADMAL MALPED (NOLOCK)                           " +
"							ON MALPED.CODFIL = GUI.CODFIL                                  " +
"								AND MALPED.NUMGUI = GUI.NUMGUI                                 " +
"								AND MALPED.SERGUI = GUI.SERGUI                                 " +
"								AND MALPED.CODORIRTV = GUI.CODORIRTV               " +
"						LEFT OUTER JOIN SCO_TCOMPMALOTE PEDMAL (NOLOCK)                           " +
"							ON PEDMAL.COD_FIL = MALPED.CODFIL                                 " +
"								AND PEDMAL.NUM_GUI = MALPED.NUMGUI                                 " +
"								AND PEDMAL.SER_GUI = MALPED.SERGUI                                 " +
"								AND PEDMAL.COD_ORI_RTV = MALPED.CODORIRTV                                 " +
"								AND PEDMAL.DATCOL = MALPED.DATCOL                                " +
"								AND PEDMAL.NUM_MAL= MALPED.NUMMAL                " +
"						LEFT OUTER JOIN SCO_TCADCOMPOSICAO COMP (NOLOCK)                           " +
"							ON COMP.COD_COMP = PEDMAL.COD_COMP                                 " +
"								AND COMP.COD_ESP = PEDMAL.COD_ESP                 " +
"						LEFT OUTER JOIN SCO_TITERTV ITEGTVENT9 (NOLOCK)                           " +
"							ON ITEGTVENT9.CODFIL = GUI.CODFIL                                 " +
"								AND ITEGTVENT9.NUMGUI = GUI.NUMGUI                                 " +
"								AND ITEGTVENT9.SERGUI = GUI.SERGUI								 " +
"								AND ITEGTVENT9.CODORIRTV = GUI.CODORIRTV                                 " +
"								AND ITEGTVENT9.DATCOL = GUI.DATCOL                                 " +
"								AND ITEGTVENT9.CODTIPITERTV = 12                             " +
"						LEFT OUTER JOIN SCO_TITERTV ITEGTVENT2 (NOLOCK)                           " +
"							ON ITEGTVENT2.CODFIL = GUI.CODFIL                                 " +
"								AND ITEGTVENT2.NUMGUI = GUI.NUMGUI                                 " +
"								AND ITEGTVENT2.SERGUI = GUI.SERGUI                                 " +
"								AND ITEGTVENT2.CODORIRTV = GUI.CODORIRTV                                 " +
"								AND ITEGTVENT2.DATCOL = GUI.DATCOL                                 " +
"								AND ITEGTVENT2.CODTIPITERTV = 2                 " +
"						LEFT OUTER JOIN SCO_TITERTV ITEGTVENT10 (NOLOCK)                           " +
"							ON ITEGTVENT10.CODFIL = GUI.CODFIL                                 " +
"								AND ITEGTVENT10.NUMGUI = GUI.NUMGUI                                 " +
"								AND ITEGTVENT10.SERGUI = GUI.SERGUI                                 " +
"								AND ITEGTVENT10.CODORIRTV = GUI.CODORIRTV                                 " +
"								AND ITEGTVENT10.DATCOL = GUI.DATCOL                                 " +
"								AND ITEGTVENT10.CODTIPITERTV = 10                 " +
"						LEFT OUTER JOIN SCO_TITERTV ITEGTVENT3 (NOLOCK)                           " +
"							ON ITEGTVENT3.CODFIL = GUI.CODFIL                                 " +
"								AND ITEGTVENT3.NUMGUI = GUI.NUMGUI                                 " +
"								AND ITEGTVENT3.SERGUI = GUI.SERGUI                                 " +
"								AND ITEGTVENT3.CODORIRTV = GUI.CODORIRTV                                 " +
"								AND ITEGTVENT3.DATCOL = GUI.DATCOL                                 " +
"								AND ITEGTVENT3.CODTIPITERTV = 11                 " +
"					WHERE  GUI.DATENT >= '" + strDate + "'  " +
"						AND GUI.DATENT <= '" + strDate + "'     " +
"					GROUP BY GUI.DATCOL, " +
"						GUI.NUMPED, " +
"						GUI.CODFIL, " +
"						CONT.COD_COM,  " +
"						PEDMAL.COD_COMP,  " +
"						ISNULL(GUI.SLIPNUM, GUI.NUMGUI),  " +
"						GUI.NUMROTENT,  " +
"						GUI.SEQROTENT, " +
"						CONT.COD_ESP,  " +
"						PEDMAL.COD_ESP, " +
"						MAL.NUMLAC1,               " +
"						CONT.QTD_CED_CONT,  " +
"						PEDMAL.QTD_ITENS, " +
"						COMP1.PES_COMP,  " +
"						COMP.PES_COMP, " +
"						MAL.VALDIN,  " +
"						MAL.VALMOE,  " +
"						REMRK.OBSNAOATE, " +
"						REMRK.CODJUSNAOATE,  " +
"						GUI.SCRCAR    ) AS GUIENT     " +
"		ON GUIENT.NUMROTENT = ROTA.[ROUTE NUMBER]    " +
"			AND   GUIENT.SEQROTENT = ROTA.[S.NO]  	  " +
"			AND ROTA.[DATE] = GUIENT.DATCOL 	 " +
"			AND ROTA.[SERVICE NUMBER] = GUIENT.NUMPED 	 " +
"			AND ROTA.[BRANCH CODE] = GUIENT.CODFIL   )   " +
"	GROUP BY ROTA.[S.NO],         " +
"		ROTA.[BRANCH CODE],         " +
"		ROTA.[BRANCH NAME],         " +
"		ROTA.[ROUTE NUMBER],         " +
"		ROTA.[ROUTE NAME],         " +
"		ROTA.[DEVICE ID],         " +
"		ROTA.[DATE],         " +
"		ROTA.[ROUTE START TIME-TM],         " +
"		ROTA.[ROUTE END TIME-TM],         " +
"		ROTA.[VEHICLE NUMBER],         " +
"		ROTA.CUSTODIAN_1,         " +
"		ROTA.CUSTODIAN_1_REG,         " +
"		ROTA.CUSTODIAN_2,         " +
"		ROTA.CUSTODIAN_2_REG,         " +
"		ROTA.GUNMAN_1,         " +
"		ROTA.GUNMAN_1_REG,         " +
"		ROTA.GUNMAN_2,         " +
"		ROTA.GUNMAN_2_REG,         " +
"		ROTA.DRIVER,         " +
"		ROTA.DRIVER_REG,         " +
"		GUICOL.NUMGUI,          " +
"		GUIENT.NUMGUI,         " +
"		GUICOL.NUMLAC1,         " +
"		GUIENT.NUMLAC1,         " +
"		ROTA.[SERVICE SITUATION],         " +
"		ROTA.[BILLING CLIENT CODE],         " +
"		ROTA.[BILING CLIENT NAME],         " +
"		ROTA.[SERVICE NUMBER],        " +
"		ROTA.[SERVICE TYPE],         " +
"		ROTA.[CUSTOMER CODE-ORIGIN],         " +
"		ROTA.[CUSTOMER NAME-ORIGIN],         " +
"		ROTA.[SERVICE POINT NUMBER-ORIGIN],         " +
"		ROTA.[SERVICE POINT NAME-ORIGIN],         " +
"		ROTA.[SERVICE POINT ADDRESS-ORIGIN],         " +
"		ROTA.DELIVERY_DAY,         " +
"		ROTA.[CODCLASER],         " +
"		ROTA.[CUSTOMER CODE-DELIVERY],         " +
"		ROTA.[CUSTOMER NAME-DELIVERY],         " +
"		ROTA.[SERVICE POINT NUMBER-DELIVERY],         " +
"		ROTA.[SERVICE POINT NAME-DELIVERY],                   " +
"		ROTA.[SERVICE POINT ADDRESS-DELIVERY],                           " +
"		ROTA.[SERVICE POINT ADDRESS-ORIGIN1],         " +
"		ROTA.[SERVICE POINT ADDRESS-DELIVERY1],         " +
"		ROTA.[ACTUAL KM READING],         " +
"		ROTA.[TIME OF ARRIVAL],         " +
"		ROTA.[TIME OF DEPARTURE],         " +
"		ROTA.[TIME OF LAST ROUTE UPDATE],         " +
"		ROTA.[REMARKS],         " +
"		ROTA.[ITEFOLROTID],       " +
"		GUICOL.ITERTV9,         " +
"		GUIENT.ITERTV9,         " +
"		GUICOL.ITERTV2,         " +
"		GUIENT.ITERTV2,         " +
"		GUICOL.ITERTV10,         " +
"		GUIENT.ITERTV10,         " +
"		GUICOL.ITERTV3,         " +
"		GUIENT.ITERTV3,         " +
"		GUICOL.VALDIN,         " +
"		GUICOL.VALMOE,         " +
"		GUIENT.VALMOE,         " +
"		GUIENT.VALDIN,   " +
"		GUICOL.OBSNAOATE, " +
"		GUIENT.OBSNAOATE,  " +
"		GUICOL.DESJUSNAOATE, " +
"		GUIENT.DESJUSNAOATE,  " +
"		GUICOL.SCRCAR,GUIENT.SCRCAR )TBL_CASH_PICKUP          " +
"	LEFT JOIN DBO.SCO_TPONATE TPON         " +
"		ON (TBL_CASH_PICKUP.[SERVICE POINT NUMBER-ORIGIN] = TPON.NUMPONATE)  " +
"			AND (TBL_CASH_PICKUP.[BRANCH CODE] =  TPON.CODFIL)  " +
"			AND (TBL_CASH_PICKUP.[CUSTOMER CODE-ORIGIN] = TPON.CODCLI)         " +
"	INNER JOIN DBO.SCO_TUF TUF  	   " +
"		ON TPON.CODUF = TUF.CODUF  	   " +
"	INNER JOIN DBO.SCO_TTABMUN TMUN  	   " +
"		ON TPON.CODMUN = TMUN.CODMUN         " +
"	INNER JOIN DBO.SCO_TTIPPTOATE TPOT         " +
"		ON TPOT.CODTIPPTOATE = TPON.CODTIPPTOATE         " +
"	LEFT JOIN DBO.SCO_TTABREG TREG         " +
"		ON (TREG.CODREG = TPON.CODREG)	 " +
"			AND (TREG.CODFIL = TPON.CODFIL)  " +
"			AND (TBL_CASH_PICKUP.[CUSTOMER CODE-ORIGIN] = TPON.CODCLI)          " +
"WHERE TBL_CASH_PICKUP.[SERVICE NUMBER] NOT IN (21,22)   " +
"	AND TBL_CASH_PICKUP.[SERVICE SITUATION TYPE] IN ('CONCLUDED','CANCELLED','POINT CLOSED','NO CASH','ROUTE CLOSED','FINAL CLOSED')  " +
"			 " +
"ORDER BY TBL_CASH_PICKUP.[BRANCH CODE] ASC, " +
"TBL_CASH_PICKUP.[TIME OF DEPARTURE] DESC ";
                SqlConnection conSCO = new SqlConnection(strSCOConnectionString);

                conSCO.Open();

                SqlCommand cmd = new SqlCommand(strSQL, conSCO);

                cmd.CommandTimeout = 1200;

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dsData);

                conSCO.Close();
            }
            catch (Exception ex)
            {
                //SendMailMessage("support@sisprosegur.com", "rrajupradhan@gmail.com", "", "", "Error -GetATMMasterSCO ", ex.Message.ToString() + " || " + ex.StackTrace.ToString());
            }

            return dsData;
        }

        public int InsertDetailsRMS(DataSet dsData)
        {
            Get_from_config();
            int iReturn = 0;
            int icounter = 0;

            try
            {
                SqlConnection con = new SqlConnection(strRMSConnectionString);
                con.Open();

                for (int i = 0; i <= dsData.Tables[0].Rows.Count - 1; i++)
                {
                    
                    using (SqlCommand cmd = new SqlCommand("[dbo].[DSB_SCO_RMSInsert]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 12000;
                        cmd.Parameters.AddWithValue("@CustomerType", dsData.Tables[0].Rows[i]["CustomerType"].ToString());
                        cmd.Parameters.AddWithValue("@HUB", dsData.Tables[0].Rows[i]["HUB"].ToString());
                        cmd.Parameters.AddWithValue("@SERVICE_SITUATION_TYPE", dsData.Tables[0].Rows[i]["SERVICE SITUATION TYPE"].ToString());
                        cmd.Parameters.AddWithValue("@S_NO", dsData.Tables[0].Rows[i]["S.NO"].ToString());
                        cmd.Parameters.AddWithValue("@BRANCH_CODE", dsData.Tables[0].Rows[i]["BRANCH CODE"].ToString());
                        cmd.Parameters.AddWithValue("@BRANCH_NAME", dsData.Tables[0].Rows[i]["BRANCH NAME"].ToString());
                        cmd.Parameters.AddWithValue("@ROUTE_NUMBER", dsData.Tables[0].Rows[i]["ROUTE NUMBER"].ToString());
                        cmd.Parameters.AddWithValue("@ROUTE_NAME", dsData.Tables[0].Rows[i]["ROUTE NAME"].ToString());
                        cmd.Parameters.AddWithValue("@DEVICE_ID", dsData.Tables[0].Rows[i]["DEVICE ID"].ToString());
                        cmd.Parameters.AddWithValue("@DATE", Convert.ToDateTime(dsData.Tables[0].Rows[i]["DATE"].ToString()));

                        try
                        {
                            cmd.Parameters.AddWithValue("@ROUTE_START_TIME_TM", Convert.ToDateTime(dsData.Tables[0].Rows[i]["ROUTE START TIME-TM"].ToString()));
                        }
                        catch (Exception)
                        {
                            cmd.Parameters.AddWithValue("@ROUTE_START_TIME_TM", DBNull.Value);
                        }

                        try
                        {
                            cmd.Parameters.AddWithValue("@ROUTE_END_TIME_TM", Convert.ToDateTime(dsData.Tables[0].Rows[i]["ROUTE END TIME-TM"].ToString()));
                        }
                        catch (Exception)
                        {
                            cmd.Parameters.AddWithValue("@ROUTE_END_TIME_TM", DBNull.Value);
                        }

                        cmd.Parameters.AddWithValue("@VEHICLE_NUMBER", dsData.Tables[0].Rows[i]["VEHICLE NUMBER"].ToString());
                        cmd.Parameters.AddWithValue("@CUSTODIAN_1", dsData.Tables[0].Rows[i]["CUSTODIAN_1"].ToString());
                        cmd.Parameters.AddWithValue("@CUSTODIAN_1_REG", dsData.Tables[0].Rows[i]["CUSTODIAN_1_REG"].ToString());
                        cmd.Parameters.AddWithValue("@CUSTODIAN_2", dsData.Tables[0].Rows[i]["CUSTODIAN_2"].ToString());
                        cmd.Parameters.AddWithValue("@CUSTODIAN_2_REG", dsData.Tables[0].Rows[i]["CUSTODIAN_2_REG"].ToString());
                        cmd.Parameters.AddWithValue("@GUNMAN_1", dsData.Tables[0].Rows[i]["GUNMAN_1"].ToString());
                        cmd.Parameters.AddWithValue("@GUNMAN_1_REG", dsData.Tables[0].Rows[i]["GUNMAN_1_REG"].ToString());
                        cmd.Parameters.AddWithValue("@GUNMAN_2", dsData.Tables[0].Rows[i]["GUNMAN_2"].ToString());
                        cmd.Parameters.AddWithValue("@GUNMAN_2_REG", dsData.Tables[0].Rows[i]["GUNMAN_2_REG"].ToString());
                        cmd.Parameters.AddWithValue("@DRIVER", dsData.Tables[0].Rows[i]["DRIVER"].ToString());
                        cmd.Parameters.AddWithValue("@DRIVER_REG", dsData.Tables[0].Rows[i]["DRIVER_REG"].ToString());
                        cmd.Parameters.AddWithValue("@SERVICE_SITUATION", dsData.Tables[0].Rows[i]["SERVICE SITUATION"].ToString());
                        cmd.Parameters.AddWithValue("@BILLING_CLIENT_CODE", dsData.Tables[0].Rows[i]["BILLING CLIENT CODE"].ToString());
                        cmd.Parameters.AddWithValue("@BILING_CLIENT_NAME", dsData.Tables[0].Rows[i]["BILING CLIENT NAME"].ToString());
                        cmd.Parameters.AddWithValue("@SERVICE_NUMBER", dsData.Tables[0].Rows[i]["SERVICE NUMBER"].ToString());
                        cmd.Parameters.AddWithValue("@SERVICE_TYPE", dsData.Tables[0].Rows[i]["SERVICE TYPE"].ToString());
                        cmd.Parameters.AddWithValue("@CUSTOMER_CODE_ORIGIN", dsData.Tables[0].Rows[i]["CUSTOMER CODE-ORIGIN"].ToString());
                        cmd.Parameters.AddWithValue("@CUSTOMER_NAME_ORIGIN", dsData.Tables[0].Rows[i]["CUSTOMER NAME-ORIGIN"].ToString());
                        cmd.Parameters.AddWithValue("@SERVICE_POINT_NUMBER_ORIGIN", dsData.Tables[0].Rows[i]["SERVICE POINT NUMBER-ORIGIN"].ToString());
                        cmd.Parameters.AddWithValue("@SERVICE_POINT_NAME_ORIGIN", dsData.Tables[0].Rows[i]["SERVICE POINT NAME-ORIGIN"].ToString());
                        cmd.Parameters.AddWithValue("@SERVICE_POINT_ADDRESS_ORIGIN1", dsData.Tables[0].Rows[i]["SERVICE POINT ADDRESS-ORIGIN1"].ToString());
                        cmd.Parameters.AddWithValue("@SERVICE_POINTADDRESS_DELIVERY1", dsData.Tables[0].Rows[i]["SERVICE POINT ADDRESS-DELIVERY1"].ToString());
                        cmd.Parameters.AddWithValue("@CODCLASER", dsData.Tables[0].Rows[i]["CODCLASER"].ToString());
                        cmd.Parameters.AddWithValue("@CUSTOMER_CODE", dsData.Tables[0].Rows[i]["CUSTOMER_CODE"].ToString());
                        cmd.Parameters.AddWithValue("@CRN_No", dsData.Tables[0].Rows[i]["CRN_No"].ToString());
                        cmd.Parameters.AddWithValue("@Account_No", dsData.Tables[0].Rows[i]["Account_No"].ToString());
                        cmd.Parameters.AddWithValue("@CUSTOMER_CODE_DELIVERY", dsData.Tables[0].Rows[i]["CUSTOMER CODE-DELIVERY"].ToString());
                        cmd.Parameters.AddWithValue("@CUSTOMER_NAME_DELIVERY", dsData.Tables[0].Rows[i]["CUSTOMER NAME-DELIVERY"].ToString());
                        cmd.Parameters.AddWithValue("@SERVICE_POINT_NUMBER_DELIVERY", dsData.Tables[0].Rows[i]["SERVICE POINT NUMBER-DELIVERY"].ToString());
                        cmd.Parameters.AddWithValue("@SERVICE_POINT_NAME_DELIVERY", dsData.Tables[0].Rows[i]["SERVICE POINT NAME-DELIVERY"].ToString());
                        cmd.Parameters.AddWithValue("@ACTUAL_KM_READING", dsData.Tables[0].Rows[i]["ACTUAL KM READING"].ToString());

                        try
                        {
                            cmd.Parameters.AddWithValue("@TIME_OF_ARRIVAL", Convert.ToDateTime(dsData.Tables[0].Rows[i]["TIME OF ARRIVAL"].ToString()));
                        }
                        catch
                        {
                            cmd.Parameters.AddWithValue("@TIME_OF_ARRIVAL", DBNull.Value);
                        }

                        try
                        {
                            cmd.Parameters.AddWithValue("@TIME_OF_DEPARTURE", Convert.ToDateTime(dsData.Tables[0].Rows[i]["TIME OF DEPARTURE"].ToString()));
                        }
                        catch
                        {
                            cmd.Parameters.AddWithValue("@TIME_OF_DEPARTURE", DBNull.Value);
                        }

                        try
                        {
                            cmd.Parameters.AddWithValue("@TIME_OF_LAST_ROUTE_UPDATE", Convert.ToDateTime(dsData.Tables[0].Rows[i]["TIME OF LAST ROUTE UPDATE"].ToString()));
                        }
                        catch
                        {
                            cmd.Parameters.AddWithValue("@TIME_OF_LAST_ROUTE_UPDATE", DBNull.Value);
                        }

                        cmd.Parameters.AddWithValue("@REMARKS", dsData.Tables[0].Rows[i]["MANUAL_REMARK"].ToString());

                        cmd.Parameters.AddWithValue("@CASH_DEPOSIT_IN", dsData.Tables[0].Rows[i]["CASH_DEPOSIT_IN"].ToString());
                        cmd.Parameters.AddWithValue("@TYPE_OF_SERVICE", dsData.Tables[0].Rows[i]["TYPE OF SERVICE"].ToString());
                        cmd.Parameters.AddWithValue("@CODE_TYPE_OF_SERVICE", dsData.Tables[0].Rows[i]["CODE TYPE OF SERVICE"].ToString());
                        cmd.Parameters.AddWithValue("@SLIP_NUMBER", dsData.Tables[0].Rows[i]["SLIP NUMBER"].ToString());
                        cmd.Parameters.AddWithValue("@SEAL_NUMBER", dsData.Tables[0].Rows[i]["SEAL NUMBER"].ToString());
                        cmd.Parameters.AddWithValue("@M_INR_0_50", Convert.ToDouble(dsData.Tables[0].Rows[i]["M_INR_0.50"].ToString()));
                        cmd.Parameters.AddWithValue("@M_INR_0_1", Convert.ToInt32(dsData.Tables[0].Rows[i]["M_INR_0.1"].ToString()));
                        cmd.Parameters.AddWithValue("@C_INR_1", dsData.Tables[0].Rows[i]["C_INR_1"].ToString());
                        cmd.Parameters.AddWithValue("@C_INR_2", dsData.Tables[0].Rows[i]["C_INR_2"].ToString());
                        cmd.Parameters.AddWithValue("@C_INR_5", dsData.Tables[0].Rows[i]["C_INR_5"].ToString());
                        cmd.Parameters.AddWithValue("@C_INR_10", dsData.Tables[0].Rows[i]["C_INR_10"].ToString());
                        cmd.Parameters.AddWithValue("@COINS_AMOUNT", Convert.ToDouble(dsData.Tables[0].Rows[i]["COINS-AMOUNT"].ToString()));
                        cmd.Parameters.AddWithValue("@INR_10", dsData.Tables[0].Rows[i]["INR_10"].ToString());
                        cmd.Parameters.AddWithValue("@INR_20", dsData.Tables[0].Rows[i]["INR_20"].ToString());
                        cmd.Parameters.AddWithValue("@INR_50", dsData.Tables[0].Rows[i]["INR_50"].ToString());
                        cmd.Parameters.AddWithValue("@INR_100", dsData.Tables[0].Rows[i]["INR_100"].ToString());

                        cmd.Parameters.AddWithValue("@INR_500", dsData.Tables[0].Rows[i]["INR_500"].ToString());
                        cmd.Parameters.AddWithValue("@INR_1000", dsData.Tables[0].Rows[i]["INR_1000"].ToString());
                        cmd.Parameters.AddWithValue("@INR_2000", dsData.Tables[0].Rows[i]["INR_2000"].ToString());
                        cmd.Parameters.AddWithValue("@CASH_AMOUNT", Convert.ToDouble(dsData.Tables[0].Rows[i]["CASH-AMOUNT"].ToString()));
                        cmd.Parameters.AddWithValue("@BULLION", dsData.Tables[0].Rows[i]["BULLION"].ToString());
                        cmd.Parameters.AddWithValue("@INTRUMENTS", dsData.Tables[0].Rows[i]["INTRUMENTS"].ToString());
                        cmd.Parameters.AddWithValue("@OTHERS", dsData.Tables[0].Rows[i]["OTHERS"].ToString());
                        cmd.Parameters.AddWithValue("@TOTAL_AMOUNT_QUANTITY", Convert.ToDouble(dsData.Tables[0].Rows[i]["TOTAL AMOUNT/QUANTITY"].ToString()));
                        cmd.Parameters.AddWithValue("@INR_200", dsData.Tables[0].Rows[i]["INR_200"].ToString());
                        cmd.Parameters.AddWithValue("@DESUF", dsData.Tables[0].Rows[i]["DESUF"].ToString());
                        cmd.Parameters.AddWithValue("@NOMMUN", dsData.Tables[0].Rows[i]["NOMMUN"].ToString());
                        cmd.Parameters.AddWithValue("@Cust_Code", dsData.Tables[0].Rows[i]["Cust_Code"].ToString());
                        cmd.Parameters.AddWithValue("@BARPONATE", dsData.Tables[0].Rows[i]["BARPONATE"].ToString());
                        cmd.Parameters.AddWithValue("@SCRCAR", dsData.Tables[0].Rows[i]["SCRCAR"].ToString());
                        cmd.Parameters.AddWithValue("@ITEFOLROTID", dsData.Tables[0].Rows[i]["ITEFOLROTID"].ToString());

                        cmd.Parameters.AddWithValue("@REASON", dsData.Tables[0].Rows[i]["REASON"].ToString());

                        cmd.Parameters.AddWithValue("@AccountNoNew", dsData.Tables[0].Rows[i]["Account_No_New"].ToString());
                        cmd.Parameters.AddWithValue("@PinCode", dsData.Tables[0].Rows[i]["PinCode"].ToString());

                        iReturn = cmd.ExecuteNonQuery();

                        if (iReturn == 1)
                        {
                            icounter = icounter + 1;
                        }
                    }
                }

                con.Close();
            }
            catch (Exception)
            {
                //SendMailMessage("support@sisprosegur.com", "rrajupradhan@gmail.com", "", "", "Error -InsertDetailsRMS ", ex.Message.ToString() + " || " + ex.StackTrace.ToString());
            }

            return icounter;
        }

        public void InsertActivityLog(string strActivity,int intTotalRows)
        {
            Get_from_config();
            try
            {
                SqlConnection con = new SqlConnection(strRMSConnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("insert into DSB_MIS_DATA_SCHEDULER_LOG (Activity,Rows,Last_Modified_Time) values ('" + strActivity + "','" + intTotalRows.ToString() + "',getdate())", con);

                cmd.ExecuteNonQuery();

                con.Close();
            }
            catch
            {

            }
            finally
            {
               // SendMailMessage("support@sisprosegur.com", "rrajupradhan@gmail.com", "", "", "DSB Scheduler Run ","InsertDetailsRMS " + DateTime.Now.ToString());
            }
        }


        public int InsertClientDetailsRMS(DataSet dsData)
        {
            Get_from_config();
            int iReturn = 0;
            int icounter = 0;

            try
            {
                SqlConnection con = new SqlConnection(strRMSConnectionString);
                con.Open();

                for (int i = 0; i <= dsData.Tables[0].Rows.Count - 1; i++)
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[DSB_SCO_RMS_ClientInsert]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CODCLI", dsData.Tables[0].Rows[i]["CODCLI"].ToString());
                        cmd.Parameters.AddWithValue("@NOMFAN", dsData.Tables[0].Rows[i]["NOMFAN"].ToString());
                        cmd.Parameters.AddWithValue("@RAZSOC", dsData.Tables[0].Rows[i]["RAZSOC"].ToString());                                               

                        iReturn = cmd.ExecuteNonQuery();

                        if (iReturn == 1)
                        {
                            icounter = icounter + 1;
                        }
                    }
                }

                con.Close();
            }
            catch (Exception ex)
            {
                SendMailMessage("support@sisprosegur.com", "rrajupradhan@gmail.com", "", "", "Error -InsertClientDetailsRMS ", ex.Message.ToString() + " || " + ex.StackTrace.ToString());
            }

            return icounter;
        }
    }
}
