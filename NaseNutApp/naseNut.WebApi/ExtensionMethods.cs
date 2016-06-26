using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace naseNut.WebApi
{
    public static class ExtensionMethods
    {
        public static DateTime ConvertToDate(this string dateString)
        {
            return new DateTime(Convert.ToInt32(dateString.Substring(6, 4)), 
                Convert.ToInt32(dateString.Substring(3, 2)), 
                Convert.ToInt32(dateString.Substring(0, 2)), 
                Convert.ToInt32(dateString.Substring(11, 2)),
                Convert.ToInt32(dateString.Substring(14, 2)),0);
        }
    }
}