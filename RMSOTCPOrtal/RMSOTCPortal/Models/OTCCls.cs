using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSOTCPortal.Models
{
    public class OTCCls
    {
        
    }
    public class OTCReturnData
    {
        public string OTC { get; set; }
        public string CallerNo { get; set; }
        public string Msg { get; set; }
    }
    public class OTCAuthCode
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class OTCAuthCodeToken
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthCode { get; set; }
    }

    public class OpenLockNow1A
    {
        public string AtmId { get; set; }
        public string UserId { get; set; }
    }

    public class OTC_Open_LOCK_A
    {
        public string AtmId { get; set; }
        public string UserId { get; set; }
        public string TouchKeyId { get; set; }
        public string Date { get; set; }
        public int Hour { get; set; }
        public int TimeBlock { get; set; }
        public int LockStatus { get; set; }
    }

    public class RESET_TAMPER_NOW_A
    {
        public string AtmId { get; set; }
        public string UserId { get; set; }
    }

    public class RESET_USER_KEY_A
    {
        public string AtmId { get; set; }
        public string UserId { get; set; }
        public string TouchKeyId { get; set; }
        public string Date { get; set; }
        public int Hour { get; set; }
        public int TimeBlock { get; set; }
        public int LockStatus { get; set; }
    }

    public static class clsSISCredentials
    {
        public static string ClientId = "6ekoct0kedwz1shxj2zzmkfa1izxhbi1";
        public static string ClientSecret = "NmVrb2N0MGtlZHd6MXNoeGoyenpta2ZhMWl6eGhiaTE=";
        public static string Username = "AMAR";
        public static string Password = "1234";
    }

    public static class clsSISCOCredentials
    {
        public static string ClientId = "5xybou6sdoztzyig5qocgwogkdke2gea";
        public static string ClientSecret = "NXh5Ym91NnNkb3p0enlpZzVxb2Nnd29na2RrZTJnZWE=";
        public static string Username = "AMAR";
        public static string Password = "1234";
    }
}