namespace BankingSystemDataAccess.Postgres.Services
{
    public class SimpleEncryptionService
    {
        private static readonly Dictionary<char, char> _encryptMap = new Dictionary<char, char>
        {
            {'а', 'я'}, {'б', 'ю'}, {'в', 'э'}, {'г', 'ь'}, {'д', 'ы'},
            {'е', 'ъ'}, {'ё', 'щ'}, {'ж', 'ш'}, {'з', 'ч'}, {'и', 'ц'},
            {'й', 'х'}, {'к', 'ф'}, {'л', 'у'}, {'м', 'т'}, {'н', 'с'},
            {'о', 'р'}, {'п', 'п'}, {'р', 'о'}, {'с', 'н'}, {'т', 'м'},
            {'у', 'л'}, {'ф', 'к'}, {'х', 'й'}, {'ц', 'и'}, {'ч', 'з'},
            {'ш', 'ж'}, {'щ', 'ё'}, {'ъ', 'е'}, {'ы', 'д'}, {'ь', 'г'},
            {'э', 'в'}, {'ю', 'б'}, {'я', 'а'},

            {'a', 'z'}, {'b', 'y'}, {'c', 'x'}, {'d', 'w'}, {'e', 'v'},
            {'f', 'u'}, {'g', 't'}, {'h', 's'}, {'i', 'r'}, {'j', 'q'},
            {'k', 'p'}, {'l', 'o'}, {'m', 'n'}, {'n', 'm'}, {'o', 'l'},
            {'p', 'k'}, {'q', 'j'}, {'r', 'i'}, {'s', 'h'}, {'t', 'g'},
            {'u', 'f'}, {'v', 'e'}, {'w', 'd'}, {'x', 'c'}, {'y', 'b'}, {'z', 'a'},

            {'0', '9'}, {'1', '8'}, {'2', '7'}, {'3', '6'}, {'4', '5'},
            {'5', '4'}, {'6', '3'}, {'7', '2'}, {'8', '1'}, {'9', '0'},

            {'.', ','}, {',', '.'},
            {'!', '?'}, {'?', '!'},
            {':', ';'}, {';', ':'},
            {'(', ')'}, {')', '('},
            {'[', ']'}, {']', '['},
            {'{', '}'}, {'}', '{'},
            {'<', '>'}, {'>', '<'},

            {'@', '#'}, {'#', '@'},
            {'$', '%'}, {'%', '$'},
            {'^', '&'}, {'&', '^'},
            {'*', '+'}, {'+', '*'},
            {'-', '='}, {'=', '-'},
            {'_', '/'}, {'/', '_'},
            {'\\', '|'}, {'|', '\\'},
            {'~', '`'}, {'`', '~'},

            {' ', ' '},
            {'"', '\''}, {'\'', '"'},

            {'№', '§'}, {'§', '№'},
            {'€', '£'}, {'£', '€'},
            {'¢', '¥'}, {'¥', '¢'}
        };

        private static readonly Dictionary<char, char> _decryptMap;

        static SimpleEncryptionService()
        {
            _decryptMap = _encryptMap.ToDictionary(x => x.Value, x => x.Key);
        }

        public static string Encrypt(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            char[] result = new char[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                result[i] = _encryptMap.ContainsKey(c) ? _encryptMap[c] : c;
            }

            return new string(result);
        }

        public static string Decrypt(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            char[] result = new char[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                result[i] = _decryptMap.ContainsKey(c) ? _decryptMap[c] : c;
            }

            return new string(result);
        }
    }
}
