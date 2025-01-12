namespace NurseryMart.Utility
{
    public class ErrorConstant
    {
        public const string NotFound = "the resourse you trying to access is not found,{0}";
        public static string NotAuthorized = "You are not authorized to access this resource.";
        public static string InvalidInput = "Invalid inputs.";
        public static string Conflict = "The entity exist already, problems {0}. Please try with different options.";
        public static string InvalidCredentials = "Invalid credentials.";
        public static string CreationFailed = "The {0} you are trying to create has these problems {1}.";
    }
}
