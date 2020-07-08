using System;
using System.Text;

namespace FlyshowVegetablesAPI.Utilities
{
    public class CommonUtilities
    {
        #region Common Method
        /// <summary>
        /// Encrypt Token
        /// </summary>
        /// <param name="account"></param>
        /// <param name="loginTime"></param>
        /// <returns></returns>
        public static string EncryptToken(string account, DateTime loginTime)
        {
            string _value = $"{account}_{loginTime.AddDays(30)}";
            return Encrypt(_value);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="encrypt">密碼</param>
        /// <returns>明碼</returns>
        public static string Decrypt(string encrypt)
        {
            encrypt = String.IsNullOrEmpty(encrypt) ? "" : encrypt.Replace(' ', '+');
            try
            {
                byte[] data = Convert.FromBase64String(encrypt);
                if (data == null)
                {
                    return null;
                }
                return Encoding.Unicode.GetString(data, 0x10, data.Length - 0x10);
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="value">明碼</param>
        /// <returns>密碼</returns>
        public static string Encrypt(string value)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(value);
            byte[] data = new byte[0x10];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(data);
            byte[] dst = new byte[data.Length + bytes.Length];
            Buffer.BlockCopy(data, 0, dst, 0, data.Length);
            Buffer.BlockCopy(bytes, 0, dst, data.Length, bytes.Length);
            return Convert.ToBase64String(dst);
        }

        public static string GetBase64String(string base64)
        {
            int dataTagIndex = base64.IndexOf(";base64,");
            if (dataTagIndex != -1)
            {
                base64 = base64.Substring(dataTagIndex + ";base64,".Length);
            }
            return base64;
        }

        public static string GetAdvertiseImageUrlFromBase64(string base64, string physicalPath, string virtualPath)
        {
            if (string.IsNullOrEmpty(base64) || string.IsNullOrEmpty(physicalPath) || string.IsNullOrEmpty(virtualPath))
            {
                throw new Exception("input null");
            }

            if (!System.IO.Directory.Exists(physicalPath))
            {
                System.IO.Directory.CreateDirectory(physicalPath);
            }

            base64 = GetBase64String(base64);

            string fileName = GetImageFileName();
            SaveBase64ToImage(base64, physicalPath + fileName);
            return virtualPath + fileName;
        }

        public static void SaveBase64ToImage(string base64, string filePath)
        {
            var bytes = Convert.FromBase64String(base64.Replace(" ", "+"));
            using (var imageFile = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
            }
        }

        public static string GetImageFileName()
        {
            return string.Format("ADIMG-{0}-{1}.jpg", DateTime.Now.ToString("yyyyMMdd"), Guid.NewGuid().ToString().ToUpper().Substring(0, 4));
        }

        public static void DeleteImageFromUrl(string url, string physicalPath, string rootPath)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(physicalPath) || string.IsNullOrEmpty(rootPath))
            {
                throw new Exception("input null");
            }

            rootPath = rootPath.Replace("~", string.Empty);
            string fileName = GetFileNameFromUrl(url, rootPath);
            string filePath = physicalPath + fileName;
            if (!System.IO.File.Exists(filePath))

            {
                throw new Exception("File not found.");
            }
            else
            {
                System.IO.File.Delete(filePath);
            }
        }

        public static string GetFileNameFromUrl(string url, string rootPath)
        {
            int index = url.IndexOf(rootPath);
            if (index == -1)
            {
                throw new Exception("Find file name error.");
            }

            return url.Substring(index + rootPath.Length);
        }

        #endregion
    }
}
