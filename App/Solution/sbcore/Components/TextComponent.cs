using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace sbcore.Components
{
    public class TextComponent
    {
        /// <summary>
        /// Remove acentos de uma string
        /// </summary>
        /// <param name="term">Termo original</param>
        /// <returns>Termo sem acentos</returns>
        public static string slug(string term)
        {
            string semAcentos = string.Empty;
            semAcentos = Regex.Replace(term, "[àáâäã]", "a", RegexOptions.IgnoreCase);
            semAcentos = Regex.Replace(semAcentos, "[èéêë]", "e", RegexOptions.IgnoreCase);
            semAcentos = Regex.Replace(semAcentos, "[ìíîï]", "i", RegexOptions.IgnoreCase);
            semAcentos = Regex.Replace(semAcentos, "[òóôöõ]", "o", RegexOptions.IgnoreCase);
            semAcentos = Regex.Replace(semAcentos, "[ùúûü]", "u", RegexOptions.IgnoreCase);
            semAcentos = Regex.Replace(semAcentos, "[ç]", "c", RegexOptions.IgnoreCase);

            return semAcentos;
        }
    }
}
