namespace UserValidation
{
    internal class StringValidator
    {
        private static StringValidator? _instance;

        public static StringValidator GetStringValidator() => _instance ?? (_instance = new StringValidator());
        // Function that confirm validation of user input data.
        public bool Validate(string data)
        {
            string validsymb = @"1234567890qwertyuiopasdfghjklzxcvbnm!#$%&()*+,-./;<=>?@[\]^_{|}~";
            string datalower = data.ToLower();
            if (!char.IsLetter(datalower[0])) return false;
            if (datalower.Except(validsymb).Count() > 0) return false;
            if (datalower.Length < 4 || datalower.Length > 32) return false;
            return true;
        }
    }
}
