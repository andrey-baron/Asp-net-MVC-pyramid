using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tools
{
    public static class Transliteration
    {
        private static Dictionary<char, string> dictionary;

        static Transliteration()
        {
            dictionary = new Dictionary<char, string>();
            dictionary.Add('А', "a");
            dictionary.Add('Б', "b");
            dictionary.Add('В', "v");
            dictionary.Add('Г', "g");
            dictionary.Add('Д', "d");
            dictionary.Add('Е', "e");
            dictionary.Add('Ё', "e");
            dictionary.Add('Ж', "zh");
            dictionary.Add('З', "z");
            dictionary.Add('И', "i");
            dictionary.Add('Й', "j");
            dictionary.Add('К', "k");
            dictionary.Add('Л', "l");
            dictionary.Add('М', "m");
            dictionary.Add('Н', "n");
            dictionary.Add('О', "o");
            dictionary.Add('П', "p");
            dictionary.Add('Р', "r");
            dictionary.Add('С', "s");
            dictionary.Add('Т', "t");
            dictionary.Add('У', "u");
            dictionary.Add('Ф', "f");
            dictionary.Add('Х', "kh");
            dictionary.Add('Ц', "c");
            dictionary.Add('Ч', "ch");
            dictionary.Add('Ш', "sh");
            dictionary.Add('Щ', "shh");
            dictionary.Add('Ъ', "");
            dictionary.Add('Ы', "y");
            dictionary.Add('Ь', "");
            dictionary.Add('Э', "e");
            dictionary.Add('Ю', "yu");
            dictionary.Add('Я', "ya");
            dictionary.Add('а', "a");
            dictionary.Add('б', "b");
            dictionary.Add('в', "v");
            dictionary.Add('г', "g");
            dictionary.Add('д', "d");
            dictionary.Add('е', "e");
            dictionary.Add('ё', "e");
            dictionary.Add('ж', "zh");
            dictionary.Add('з', "z");
            dictionary.Add('и', "i");
            dictionary.Add('й', "j");
            dictionary.Add('к', "k");
            dictionary.Add('л', "l");
            dictionary.Add('м', "m");
            dictionary.Add('н', "n");
            dictionary.Add('о', "o");
            dictionary.Add('п', "p");
            dictionary.Add('р', "r");
            dictionary.Add('с', "s");
            dictionary.Add('т', "t");
            dictionary.Add('у', "u");
            dictionary.Add('ф', "f");
            dictionary.Add('х', "kh");
            dictionary.Add('ц', "c");
            dictionary.Add('ч', "ch");
            dictionary.Add('ш', "sh");
            dictionary.Add('щ', "shh");
            dictionary.Add('ъ', "");
            dictionary.Add('ы', "y");
            dictionary.Add('ь', "");
            dictionary.Add('э', "e");
            dictionary.Add('ю', "yu");
            dictionary.Add('я', "ya");
            dictionary.Add('a', "a");
            dictionary.Add('b', "b");
            dictionary.Add('c', "c");
            dictionary.Add('d', "d");
            dictionary.Add('e', "e");
            dictionary.Add('f', "f");
            dictionary.Add('g', "g");
            dictionary.Add('h', "h");
            dictionary.Add('i', "i");
            dictionary.Add('j', "j");
            dictionary.Add('k', "k");
            dictionary.Add('l', "l");
            dictionary.Add('m', "m");
            dictionary.Add('n', "n");
            dictionary.Add('o', "o");
            dictionary.Add('p', "p");
            dictionary.Add('q', "q");
            dictionary.Add('r', "r");
            dictionary.Add('s', "s");
            dictionary.Add('t', "t");
            dictionary.Add('u', "u");
            dictionary.Add('v', "v");
            dictionary.Add('w', "w");
            dictionary.Add('x', "x");
            dictionary.Add('y', "y");
            dictionary.Add('z', "z");
            dictionary.Add('A', "a");
            dictionary.Add('B', "b");
            dictionary.Add('C', "c");
            dictionary.Add('D', "d");
            dictionary.Add('E', "e");
            dictionary.Add('F', "f");
            dictionary.Add('G', "g");
            dictionary.Add('H', "h");
            dictionary.Add('I', "i");
            dictionary.Add('J', "j");
            dictionary.Add('K', "k");
            dictionary.Add('L', "l");
            dictionary.Add('M', "m");
            dictionary.Add('N', "n");
            dictionary.Add('O', "o");
            dictionary.Add('P', "p");
            dictionary.Add('Q', "q");
            dictionary.Add('R', "r");
            dictionary.Add('S', "s");
            dictionary.Add('T', "t");
            dictionary.Add('U', "u");
            dictionary.Add('V', "v");
            dictionary.Add('W', "w");
            dictionary.Add('X', "x");
            dictionary.Add('Y', "y");
            dictionary.Add('Z', "z");
            dictionary.Add('0', "0");
            dictionary.Add('1', "1");
            dictionary.Add('2', "2");
            dictionary.Add('3', "3");
            dictionary.Add('4', "4");
            dictionary.Add('5', "5");
            dictionary.Add('6', "6");
            dictionary.Add('7', "7");
            dictionary.Add('8', "8");
            dictionary.Add('9', "9");
            dictionary.Add(' ', "-");
            dictionary.Add('-', "-");
        }

        public static string Translit(string text, string missingCharactersReplacer = "")
        {
            if (text == null)
                return string.Empty;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < text.Length; ++i)
                if (dictionary.ContainsKey(text[i]))
                    builder.Append(dictionary[text[i]]);
                else
                {
                    builder.Append(missingCharactersReplacer ?? "");
                }
            var result = builder.ToString().Trim('-');
            result = new Regex(@"[-]{2,}").Replace(result, "-");
            return result;
        }
    }
}