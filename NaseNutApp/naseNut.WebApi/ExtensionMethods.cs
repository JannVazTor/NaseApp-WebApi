using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace naseNut.WebApi
{
    public static class ExtensionMethods
    {
        public static double RoundTwoDigitsDouble(this double number)
        {
            return Math.Round(number, 2);
        }
    }
}