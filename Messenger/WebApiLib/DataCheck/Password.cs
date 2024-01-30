using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebApiLib.DataCheck;

public static class Password
{
    public static bool Check(string password)
    {
        string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$";
        Regex regex = new Regex(pattern);
        Match match = regex.Match(password);
        return match.Success;
    }

}
