using System;

namespace JclunaOficial
{
    /// <summary>
    /// Funciones (extension) de utilidad para encriptar/desencriptar 
    /// </summary>
    public static class UProtector
    {
        private const int PWD_MIN_LENGTH = 8;       // longitud mínima para la contraseña
        private const int PWD_MAX_LENGTH = 64;      // longitud máxima para la contraseña
        private const string PWD_ALLOWED_CHARS =    // caracteres permitidos para la contraseña
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private const string PWD_SPECIAL_CHARS =    // caracteres especiales para la contraseña
            "%$#?!-"; // <- agregar los caracteres necesarios

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
    }
}
