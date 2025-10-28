using Microsoft.AspNetCore.Identity;

namespace ServiceHelper.Dependencies.ErrorProb
{
    public class PasswordValidator
    {
        /// <summary>
        /// Validate whether the <paramref name="password"/> matches the required criteria.
        /// <para>
        /// Errors:
        /// <list type="bullet">
        /// <item><term><see cref="TooShortPasswordError"/></term><description>If the password is too short.</description></item>
        /// <item><term><see cref="TooLongPasswordError"/></term><description>If the password is too long.</description></item>
        /// <item><term><see cref="NoUniqueCharacterInPasswordError"/></term><description>If the password doesn't have any unique character.</description></item>
        /// <item><term><see cref="NoDigitInPasswordError"/></term><description>If the password doesn't have any digit.</description></item>
        /// <item><term><see cref="NoLowercaseLetterInPasswordError"/></term><description>If the password doesn't have any lowercase letter.</description></item>
        /// <item><term><see cref="NoUppercaseLetterInPasswordError"/></term><description>If the password doesn't have any uppercase letter.</description></item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="password">Password that needs to be validate.</param>
        /// <returns>Return <see cref="Result.Success()"/> if <paramref name="password"/> passes all the validation constraints, otherwise <see cref="Result.Failure(Error)"/> where all error <see cref="Error"/> are inherited from <see cref="PasswordValidationError"/>.</returns>
        public Result Validate(string password)
        {
            if (password.Length < 8) return Result.Failure(new TooShortPasswordError(8));
            if (password.Length > 100) return Result.Failure(new TooLongPasswordError(100));
            if (password.All(IsLetterOrDigit)) return Result.Failure(new NoUniqueCharacterInPasswordError());
            if (!password.Any(IsDigit)) return Result.Failure(new NoUniqueCharacterInPasswordError());
            if (!password.Any(IsLower)) return Result.Failure(new NoUniqueCharacterInPasswordError());
            if (!password.Any(IsUpper)) return Result.Failure(new NoUniqueCharacterInPasswordError());

            return Result.Success();
        }

        /// <summary>
        /// Returns a flag indicating whether the supplied character is a digit.
        /// </summary>
        /// <param name="c">The character to check if it is a digit.</param>
        /// <returns>True if the character is a digit, otherwise false.</returns>
        public virtual bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        /// <summary>
        /// Returns a flag indicating whether the supplied character is a lower case ASCII letter.
        /// </summary>
        /// <param name="c">The character to check if it is a lower case ASCII letter.</param>
        /// <returns>True if the character is a lower case ASCII letter, otherwise false.</returns>
        public virtual bool IsLower(char c)
        {
            return c >= 'a' && c <= 'z';
        }

        /// <summary>
        /// Returns a flag indicating whether the supplied character is an upper case ASCII letter.
        /// </summary>
        /// <param name="c">The character to check if it is an upper case ASCII letter.</param>
        /// <returns>True if the character is an upper case ASCII letter, otherwise false.</returns>
        public virtual bool IsUpper(char c)
        {
            return c >= 'A' && c <= 'Z';
        }

        /// <summary>
        /// Returns a flag indicating whether the supplied character is an ASCII letter or digit.
        /// </summary>
        /// <param name="c">The character to check if it is an ASCII letter or digit.</param>
        /// <returns>True if the character is an ASCII letter or digit, otherwise false.</returns>
        public virtual bool IsLetterOrDigit(char c)
        {
            return IsUpper(c) || IsLower(c) || IsDigit(c);
        }




        public class IdentityError : Error
        {
            public IdentityError(string message) : base(message)
            {

            }
        }

        public class PasswordValidationError : IdentityError
        {
            public PasswordValidationError(string message) : base(message)
            {
            }
        }

        public class TooShortPasswordError : PasswordValidationError
        {
            public TooShortPasswordError(int minCharacters) : base($"Password is too short, it should be atleast {minCharacters} characters long.")
            {

            }
        }

        public class TooLongPasswordError : PasswordValidationError
        {
            public TooLongPasswordError(int maxCharacters) : base($"Password is too long, it should upto {maxCharacters} characters long.")
            {
            }
        }

        public class NoUniqueCharacterInPasswordError : PasswordValidationError
        {
            public NoUniqueCharacterInPasswordError() : base("There is no unique character in the password.")
            {

            }
        }
    }
}
