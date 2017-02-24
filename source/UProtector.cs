using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace JclunaOficial
{
    /// <summary>
    /// Funciones (extension) para encriptar/desencriptar con Rijndael
    /// </summary>
    public static class UProtector
    {
        private const int PWD_MIN_LENGTH = 8;       // longitud mínima para la contraseña
        private const int PWD_MAX_LENGTH = 64;      // longitud máxima para la contraseña

        private const string PWD_ALLOWED_CHARS =    // caracteres permitidos para la contraseña
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        private const string PWD_SPECIAL_CHARS =    // caracteres especiales para la contraseña
            "%$#?!-"; // <- agregar los caracteres necesarios

        private static Encoding latin =             // uso interno de codificación latina ISO-8859-1
            Encoding.GetEncoding("ISO-8859-1");

        /// <summary>
        /// Generar una contraseña/password aleatorio
        /// </summary>
        /// <param name="length">Longitud máxima para la contraseña</param>
        /// <param name="specialChars">Determina si se incluyen caracteres especiales en la contraseña</param>
        /// <returns></returns>
        public static string RandomPassword(int length = PWD_MIN_LENGTH, bool specialChars = false)
        {
            // evaluar el rango de la longitud especificada
            if (length < PWD_MIN_LENGTH || length > PWD_MAX_LENGTH)
                length = PWD_MIN_LENGTH; // usar la longitud miníma cuando esta fuera del rango

            // definir caracteres que serán utilizados
            var pwdCharsToUse = PWD_ALLOWED_CHARS;
            if (specialChars == true)
                pwdCharsToUse += PWD_SPECIAL_CHARS;

            // extraer los caracteres aleatorios
            var pwdChars = new char[length];
            var rndPoll = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < length; i++)
                pwdChars[i] = pwdCharsToUse[rndPoll.Next(pwdCharsToUse.Length)];

            // regresar la contraseña generada
            return new string(pwdChars);
        }

        /// <summary>
        /// Ejecutar la acción de protección solicitada
        /// </summary>
        /// <param name="buffer">Buffer con los bytes a procesar</param>
        /// <param name="password">Contraseña de protección</param>
        /// <param name="decryptAction">Determina si la acción sera Desencriptar o Encriptar</param>
        /// <returns></returns>
        private static byte[] ExecuteCryptoAction(byte[] buffer, string password, bool decryptAction)
        {
            // evaluar los bytes a procesar
            if (buffer == null || buffer.Length == 0)
                return new byte[] { }; // no hay bytes para trabajar

            // evaluar la contraseña de protección
            var pwd = (password == null ?
                string.Empty : password.Trim());
            if (pwd.Length == 0)
                return new byte[] { }; // no hay contraseña de protección

            // usar el algoritmo Rijndael para procesar el buffer
            using (var alg = RijndaelManaged.Create())
            {
                // generar los bytes derivados usando la contraseña/password
                var pdb = new Rfc2898DeriveBytes(pwd, new byte[]
                    {
                        0x49, 0x76, 0x61, 0x6e,
                        0x20, 0x4d, 0x65, 0x64,
                        0x76, 0x65, 0x64, 0x65, 0x76
                    });
                alg.Key = pdb.GetBytes(32); // llave asimetrica
                alg.IV = pdb.GetBytes(16);  // inicialización del vector

                // ejecutar la acción en la memoria (no ocupa muchos recursos)
                // ademas de que evita la dependencia de una unidad de almacenamiento
                using (var memory = new MemoryStream())
                {
                    // crear el objeto de transformación segun la acción requerida
                    var transform = (decryptAction == false ?
                        alg.CreateEncryptor() : // <- encriptar el buffer
                        alg.CreateDecryptor()); // <- desencriptar el buffer

                    // proceder con la acción solicitada
                    using (var cs = new CryptoStream(memory, transform, CryptoStreamMode.Write))
                        cs.Write(buffer, 0, buffer.Length);

                    // regresar los bytes procesados
                    return memory.ToArray();
                }
            }
        }

        /// <summary>
        /// Encriptar un arreglo de bytes
        /// </summary>
        /// <param name="buffer">Arreglo de bytes a encriptar</param>
        /// <param name="password">Contraseña de protección</param>
        /// <returns>Regresa un arreglo de bytes encriptado</returns>
        public static byte[] Encrypt(this byte[] buffer, string password)
        {
            // aplicar encriptación de bytes
            return ExecuteCryptoAction(buffer, password, false);
        }

        /// <summary>
        /// Encriptar un string
        /// </summary>
        /// <param name="value">String a encriptar</param>
        /// <param name="password">Contraseña de protección</param>
        /// <returns>Regresa un string encriptado codificado en Base64</returns>
        public static string Encrypt(this string value, string password)
        {
            // evaluar el valor a procesar
            var textToEncrypt = (value == null ?
                string.Empty : value.Trim());
            if (textToEncrypt.Length == 0)
                return textToEncrypt; // no hay texto para trabajar

            // aplicar encriptación al string
            var buffer = Encrypt(latin.GetBytes(textToEncrypt), password);
            return (buffer.Length == 0 ? string.Empty :
                Convert.ToBase64String(buffer)); // <- convertir a Base64
        }

        /// <summary>
        /// Desencriptar un arreglo de bytes
        /// </summary>
        /// <param name="buffer">Arreglo de bytes a desencriptar</param>
        /// <param name="password">Contraseña de protección</param>
        /// <returns></returns>
        public static byte[] Decrypt(this byte[] buffer, string password)
        {
            // aplicar desencriptación de bytes
            return ExecuteCryptoAction(buffer, password, true);
        }

        /// <summary>
        /// Desencriptar un string
        /// </summary>
        /// <param name="value">String a desencriptar</param>
        /// <param name="password">Contraseña de protección</param>
        /// <returns>Regresa un string desencriptado con el texto original</returns>
        public static string Decrypt(this string value, string password)
        {
            // evaluar el valor a procesar
            var textToDecrypt = (value == null ?
                string.Empty : value.Trim());
            if (textToDecrypt.Length == 0)
                return textToDecrypt; // no hay texto para trabajar

            // aplicar desencriptación al string
            var buffer = Decrypt(Convert.FromBase64String(textToDecrypt), password);
            return (buffer.Length == 0 ? string.Empty :
                latin.GetString(buffer)); // <- convertir a texto original
        }
    }
}
