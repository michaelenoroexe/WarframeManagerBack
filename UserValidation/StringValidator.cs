namespace UserValidation
{
    /// <summary>
    /// Validator of string.
    /// </summary>
    internal sealed class StringValidator
    {
        private static StringValidator? _instance;
        /// <summary>
        /// Return instance of validator.
        /// </summary>
        public static StringValidator GetStringValidator() => _instance ??= new StringValidator();
        /// <summary>
        /// Confirm validation of user input data.
        /// </summary>
        /// <param name="data">Inputed data.</param>
        /// <returns>True if data valid oterwith false.</returns>
        public bool Validate(string data)
        {
            string validsymb = @"1234567890qwertyuiopasdfghjklzxcvbnm!#$%&()*+,-./;<=>?@[\]^_{|}~";
            if (data.Length < 4 || data.Length > 32) return false;
            string datalower = data.ToLower();
            if (!char.IsLetter(datalower[0])) return false;
            if (datalower.Except(validsymb).Any()) return false;
            return true;
        }
    }
}
