namespace UserValidation
{
    internal sealed class StringValidator
    {
        private static StringValidator? _instance;

        public static StringValidator GetStringValidator() => _instance ?? (_instance = new StringValidator());
        // Function that confirm validation of user input data.
        public bool Validate(string data)
        {
            string validsymb = @"1234567890qwertyuiopasdfghjklzxcvbnm!#$%&()*+,-./;<=>?@[\]^_{|}~";
            if (data.Length < 4 || data.Length > 32) return false;
            string datalower = data.ToLower();
            if (!char.IsLetter(datalower[0])) return false;
            if (datalower.Except(validsymb).Count() > 0) return false;          
            return true;
        }
    }
}
