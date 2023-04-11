namespace JsonTransformerApi
{
    public interface ITokenProvider
    {
        /// <summary>
        /// Generates a JWT token for the specified password
        /// </summary>
        /// <param name="password">The password to use for token generation</param>
        /// <returns>A tuple indicating if the operation was successful and the generated token string</returns>
        (bool isSuccess, string token) GetToken(string password);
    }
}