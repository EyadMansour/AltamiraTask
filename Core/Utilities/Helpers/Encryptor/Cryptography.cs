using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using Core.Utilities.Attributes;

namespace Core.Utilities.Helpers.Cryptograpies
{
    public static class Cryptography
    {
        //Şifreleme için kullanılan private key.
        //İlerleyen süreçte geliştirilecektir.
        private static readonly string _key= "b14ca5898a4e4133bbce2ea2315a1916";

        //Dışarıdan classı alıp işleyici fonksiyona gönderiyor
        public static T Encrypt<T> (T data)
        {
            return HandleClass(data, true);
        }
         //Dışarıdan List alıp alıp işleyici fonksiyona gönderiyor
        public static List<T> Decrypt<T>(List<T> data)
        {
            if (!isClass(data.FirstOrDefault()))// Listenin içindeki elemanların sınıf olması lazım. 
                throw new Exception(data.ToString());
            List<T> list = new List<T>();
            foreach (var item in data)
            {
                list.Add(HandleClass(item,false));
            }
            return list;
        }
        //Dışarıdan classı alıp işleyici fonksiyona gönderiyor
        public static T Decrypt<T>(T data)
        {
            return HandleClass(data, false);
        }
        //Encryptable olan alanları bulup, encryp veya decrypt yapmak için ilgili fonksiyonlara gönderiyor
        private static T HandleClass<T>(T data,bool isEncrypt) 
        {
            var recordType = data.GetType();
            //Property'leri tek tek dönüyor
            foreach (var item in recordType.GetProperties().ToList())
            {
                var attrs = Attribute.GetCustomAttributes(item);//Propertinin başındaki atributları alıyoruz
                var itemValue = recordType.GetProperty(item?.Name).GetValue(data);//Propertinin değerini alıyoruz
                if(itemValue==null)
                {
                    continue;//Değer null ise bir sonraki elemana geçiyoruz
                }
                //Propertin değeri string mi ve başında encryptable yazıyormu 
                if(CheckType(itemValue, typeof(string)) && isItEncryptable(attrs))
                {
                    //İlerilyen süreçte privateKey ile SecurityStamp merge edilip ortak key olarak kullanılacaktır.
                    string securityStamp = recordType.GetProperty("PrivateKey").GetValue(data).ToString();
                    if(isEncrypt)//Decrypt mi yapılmak isteniyor yoksa encryptmi
                        //Propertiye encrypt edilmiş değer set ediliyor
                        recordType.GetProperty(item.Name).SetValue(data, EncryptString(itemValue.ToString(),_key /*+ securityStamp*/));
                    else
                        //Propertiye decrypt edilmiş değer set ediliyor
                        recordType.GetProperty(item.Name).SetValue(data, DecryptString(itemValue.ToString(), _key /*+ securityStamp*/));
                }   
                else
                {
                    //Eğer propğerty kendisi bir class ise işeyici fonksiyona yeniden gönderiliyor
                    //C# ın kendi class'ları ile karışmaması için namespace'in "System" ile başlıyor mu kontrolüde yapıyoruz
                    if(!(itemValue.GetType().Namespace.StartsWith("System")) && isClass(itemValue) )
                    {
                        HandleClass(itemValue,isEncrypt);
                    }
                }
            }
            return data;
        }
        public static string EncryptString(string plainText,string key)
        {
            //Stringi encrypt ediyoruz
            byte[] iv = new byte[16];
            byte[] array;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream= new MemoryStream())
                {
                    using (CryptoStream cryptoStream= new CryptoStream((Stream)memoryStream,encryptor,CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter= new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }
                        array = memoryStream.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array);
        }
        public static string DecryptString(string chiperText, string key)
        {
            //Stringi decrypt ediyoruz
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(chiperText);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader= new StreamReader((Stream)cryptoStream))
                        {
                          return  streamReader.ReadToEnd();
                        }
                        
                    }
                }
            }
        }
        private static bool isItEncryptable(Attribute[] attrs)
        {
            //Atributları tek tek dönüp "Encryptable" atributu varmı diye kontrol ediyoruz
            foreach (var item in attrs)
            {
                if (item is Encryptable)
                {
                    return true;
                }
            }
            return false;
        }
        private static bool isClass<Class>(Class data)
        {
            //Gelen value'nun bir class olup olmadığı kontrol ediliyor
            return typeof(Class).IsClass;
        }
        private static bool CheckType<Z>(Z data,Type type)
        {
            //Dışarıdan gelen tip ile value'yu  karşılaştırıp tipi aynı mı diye kontrol ediyoruz
            if (data==null)
            {
                return false;
            }
            return data.GetType() == type;
        }
    }
}
