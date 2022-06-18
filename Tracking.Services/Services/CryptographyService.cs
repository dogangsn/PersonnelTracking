using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SednaPersonel.Services.Services
{
    public class CryptographyService 
    {
        public string ConvertStringToMD5(string ClearText)
        {
            byte[] ByteData = Encoding.ASCII.GetBytes(ClearText);
            //MD5 nesnesi oluturalm.
            MD5 oMd5 = MD5.Create();
            //Hash deerini hesaplayalm.
            byte[] HashData = oMd5.ComputeHash(ByteData);
            //byte dizisini hex formatna evirelim
            StringBuilder oSb = new StringBuilder();
            for (int x = 0; x < HashData.Length; x++)
            {
                //hexadecimal string deeri
                oSb.Append(HashData[x].ToString("x2"));
            }
            return oSb.ToString();
        }
    }
}
