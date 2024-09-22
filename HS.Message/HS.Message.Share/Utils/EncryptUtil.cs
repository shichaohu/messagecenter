using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace HS.Message.Share.Utils
{
    internal class EncryptUtil
    {
        public static string MD5By16(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            try
            {
                MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
                string text2 = BitConverter.ToString(mD5CryptoServiceProvider.ComputeHash(Encoding.Default.GetBytes(text)), 4, 8);
                mD5CryptoServiceProvider.Clear();
                return text2.Replace("-", "");
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string MD5By32(string text, Encoding encoding)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            try
            {
                MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
                byte[] array = mD5CryptoServiceProvider.ComputeHash(encoding.GetBytes(text));
                mD5CryptoServiceProvider.Clear();
                string text2 = "";
                int i = 0;
                for (int num = array.Length; i < num; i++)
                {
                    text2 += array[i].ToString("x").PadLeft(2, '0');
                }

                return text2;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string MD5By32(string text)
        {
            return MD5By32(text, Encoding.UTF8);
        }

        public static string MD5ByAlipay(string text)
        {
            return MD5By32(text, Encoding.GetEncoding("gb2312"));
        }

        public static string EncodeBase64(string text)
        {
            return EncodeBase64(text, Encoding.Default);
        }

        public static string EncodeBase64(string text, Encoding encoding)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            try
            {
                char[] array = new char[65]
                {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
                'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
                'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd',
                'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
                'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
                'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7',
                '8', '9', '+', '/', '='
                };
                byte b = 0;
                ArrayList arrayList = new ArrayList(encoding.GetBytes(text));
                int count = arrayList.Count;
                int num = count / 3;
                int num2 = 0;
                if ((num2 = count % 3) > 0)
                {
                    for (int i = 0; i < 3 - num2; i++)
                    {
                        arrayList.Add(b);
                    }

                    num++;
                }

                StringBuilder stringBuilder = new StringBuilder(num * 4);
                for (int j = 0; j < num; j++)
                {
                    byte[] array2 = new byte[3]
                    {
                    (byte)arrayList[j * 3],
                    (byte)arrayList[j * 3 + 1],
                    (byte)arrayList[j * 3 + 2]
                    };
                    int[] array3 = new int[4]
                    {
                    array2[0] >> 2,
                    ((array2[0] & 3) << 4) ^ (array2[1] >> 4),
                    0,
                    0
                    };
                    if (!array2[1].Equals(b))
                    {
                        array3[2] = ((array2[1] & 0xF) << 2) ^ (array2[2] >> 6);
                    }
                    else
                    {
                        array3[2] = 64;
                    }

                    if (!array2[2].Equals(b))
                    {
                        array3[3] = array2[2] & 0x3F;
                    }
                    else
                    {
                        array3[3] = 64;
                    }

                    stringBuilder.Append(array[array3[0]]);
                    stringBuilder.Append(array[array3[1]]);
                    stringBuilder.Append(array[array3[2]]);
                    stringBuilder.Append(array[array3[3]]);
                }

                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string DecodeBase64(string text)
        {
            return DecodeBase64(text, Encoding.Default);
        }

        public static string DecodeBase64(string text, Encoding encoding)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            text = text.Replace(" ", "+");
            try
            {
                if (text.Length % 4 != 0)
                {
                    return "包含不正确的BASE64编码";
                }

                if (!Regex.IsMatch(text, "^[A-Z0-9/+=]*$", RegexOptions.IgnoreCase))
                {
                    return "包含不正确的BASE64编码";
                }

                string text2 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
                int num = text.Length / 4;
                ArrayList arrayList = new ArrayList(num * 3);
                char[] array = text.ToCharArray();
                for (int i = 0; i < num; i++)
                {
                    byte[] array2 = new byte[4]
                    {
                    (byte)text2.IndexOf(array[i * 4]),
                    (byte)text2.IndexOf(array[i * 4 + 1]),
                    (byte)text2.IndexOf(array[i * 4 + 2]),
                    (byte)text2.IndexOf(array[i * 4 + 3])
                    };
                    byte[] array3 = new byte[3]
                    {
                    (byte)((array2[0] << 2) ^ ((array2[1] & 0x30) >> 4)),
                    0,
                    0
                    };
                    if (array2[2] != 64)
                    {
                        array3[1] = (byte)((array2[1] << 4) ^ ((array2[2] & 0x3C) >> 2));
                    }
                    else
                    {
                        array3[2] = 0;
                    }

                    if (array2[3] != 64)
                    {
                        array3[2] = (byte)((array2[2] << 6) ^ array2[3]);
                    }
                    else
                    {
                        array3[2] = 0;
                    }

                    arrayList.Add(array3[0]);
                    if (array3[1] != 0)
                    {
                        arrayList.Add(array3[1]);
                    }

                    if (array3[2] != 0)
                    {
                        arrayList.Add(array3[2]);
                    }
                }

                byte[] bytes = (byte[])arrayList.ToArray(Type.GetType("System.Byte"));
                return encoding.GetString(bytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static byte[] DecodeBase64BackBytes(string text)
        {
            return DecodeBase64BackBytes(text, Encoding.Default);
        }

        public static byte[] DecodeBase64BackBytes(string text, Encoding encoding)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new byte[0];
            }

            text = text.Replace(" ", "+");
            try
            {
                if (text.Length % 4 != 0)
                {
                    throw new Exception("包含不正确的BASE64编码");
                }

                if (!Regex.IsMatch(text, "^[A-Z0-9/+=]*$", RegexOptions.IgnoreCase))
                {
                    throw new Exception("包含不正确的BASE64编码");
                }

                string text2 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
                int num = text.Length / 4;
                ArrayList arrayList = new ArrayList(num * 3);
                char[] array = text.ToCharArray();
                for (int i = 0; i < num; i++)
                {
                    byte[] array2 = new byte[4]
                    {
                    (byte)text2.IndexOf(array[i * 4]),
                    (byte)text2.IndexOf(array[i * 4 + 1]),
                    (byte)text2.IndexOf(array[i * 4 + 2]),
                    (byte)text2.IndexOf(array[i * 4 + 3])
                    };
                    byte[] array3 = new byte[3]
                    {
                    (byte)((array2[0] << 2) ^ ((array2[1] & 0x30) >> 4)),
                    0,
                    0
                    };
                    if (array2[2] != 64)
                    {
                        array3[1] = (byte)((array2[1] << 4) ^ ((array2[2] & 0x3C) >> 2));
                    }
                    else
                    {
                        array3[2] = 0;
                    }

                    if (array2[3] != 64)
                    {
                        array3[2] = (byte)((array2[2] << 6) ^ array2[3]);
                    }
                    else
                    {
                        array3[2] = 0;
                    }

                    arrayList.Add(array3[0]);
                    if (array3[1] != 0)
                    {
                        arrayList.Add(array3[1]);
                    }

                    if (array3[2] != 0)
                    {
                        arrayList.Add(array3[2]);
                    }
                }

                return (byte[])arrayList.ToArray(Type.GetType("System.Byte"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string HMAC32(string text, string key)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            string pad = "6666666666666666666666666666666666666666666666666666666666666666";
            string pad2 = "\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\";
            string hexStr = fun_MD5(strXor(key, pad) + text);
            string text2 = strXor(key, pad2);
            byte[] bytes = hexstr2array(hexStr);
            string text3 = "";
            char[] chars = Encoding.GetEncoding(1252).GetChars(bytes);
            for (int i = 0; i < chars.Length; i++)
            {
                text3 += chars[i];
            }

            text3 = text2 + text3;
            return fun_MD5(text3).ToLower();
        }

        private static string fun_MD5(string str)
        {
            byte[] bytes = Encoding.GetEncoding(1252).GetBytes(str);
            bytes = new MD5CryptoServiceProvider().ComputeHash(bytes);
            string text = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                text += bytes[i].ToString("x").PadLeft(2, '0');
            }

            return text;
        }

        private static byte[] hexstr2array(string HexStr)
        {
            string text = "0123456789ABCDEF";
            string text2 = HexStr.ToUpper();
            int length = text2.Length;
            byte[] array = new byte[length / 2];
            for (int i = 0; i < length / 2; i++)
            {
                int num = text.IndexOf(text2[i * 2]);
                int num2 = text.IndexOf(text2[i * 2 + 1]);
                array[i] = Convert.ToByte(num * 16 + num2);
            }

            return array;
        }

        private static string strXor(string password, string pad)
        {
            string text = "";
            int length = password.Length;
            for (int i = 0; i < 64; i++)
            {
                text = ((i >= length) ? (text + Convert.ToChar(pad[i])) : (text + Convert.ToChar(pad[i] ^ password[i])));
            }

            return text;
        }

        public static string EncodeDES(string text, string key)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (key.Length != 24)
            {
                return string.Empty;
            }

            try
            {
                ICryptoTransform cryptoTransform = new TripleDESCryptoServiceProvider
                {
                    Key = Encoding.UTF8.GetBytes(key),
                    Mode = CipherMode.ECB
                }.CreateEncryptor();
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                byte[] inArray = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
                cryptoTransform.Dispose();
                return Convert.ToBase64String(inArray);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string DecodeDES(string text, string key)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (key.Length != 24)
            {
                return string.Empty;
            }

            ICryptoTransform cryptoTransform = new TripleDESCryptoServiceProvider
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            }.CreateDecryptor();
            try
            {
                byte[] array = Convert.FromBase64String(text);
                byte[] bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                cryptoTransform.Dispose();
            }
        }

        public static void GenerateMachineKey(out string validationKey, out string decryptionKey)
        {
            validationKey = CreateKey(20);
            decryptionKey = CreateKey(24);
        }

        private static string CreateKey(int size)
        {
            byte[] array = new byte[size];
            new RNGCryptoServiceProvider().GetBytes(array);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.AppendFormat("{0:X2}", array[i]);
            }

            return stringBuilder.ToString();
        }

        public static string EncryptString(string source, string key)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            RSAParameters parameters = ConvertFromPemPublicKey(key);
            RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
            rSACryptoServiceProvider.ImportParameters(parameters);
            return Convert.ToBase64String(rSACryptoServiceProvider.Encrypt(bytes, fOAEP: false));
        }

        private static RSAParameters ConvertFromPemPublicKey(string pemFileConent)
        {
            if (string.IsNullOrEmpty(pemFileConent))
            {
                throw new ArgumentNullException("pemFileConent", "This arg cann't be empty.");
            }

            byte[] array = Convert.FromBase64String(pemFileConent);
            bool flag = array.Length == 162;
            bool flag2 = array.Length == 294;
            if (!(flag || flag2))
            {
                throw new ArgumentException("pem file content is incorrect, Only support the key size is 1024 or 2048");
            }

            byte[] array2 = (flag ? new byte[128] : new byte[256]);
            byte[] array3 = new byte[3];
            Array.Copy(array, flag ? 29 : 33, array2, 0, flag ? 128 : 256);
            Array.Copy(array, flag ? 159 : 291, array3, 0, 3);
            RSAParameters result = default(RSAParameters);
            result.Modulus = array2;
            result.Exponent = array3;
            return result;
        }
    }
}
