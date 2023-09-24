using System.Security.Cryptography;
using System.Text;

namespace SampleWebAPI.Tools
{
    public class Encrypt
    {
        //CB-09242023 private function declaration
        public string GetHashPassword(string Password)
        {
            string returnVal = string.Empty;
            try
            {
                using (var sha256 = SHA256.Create())
                {
                    // Send a sample text to hash.  
                    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(Password));
                    // Get the hashed string.  
                    returnVal = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                    // Print the string.                   
                }
            }
            catch (Exception)
            {

            }
            return returnVal;
        }
    }
}
