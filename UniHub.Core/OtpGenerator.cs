namespace UniHub.Core;

public static class OtpGenerator
{
    public static string GenerateOtp(int length = 6)
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }
}