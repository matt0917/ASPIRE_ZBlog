using Ganss.Xss;

namespace ZBlogApp.Library;

public static class Helper
{
    public static string SanitizeHtml(string input)
    {
        var sanitizer = new HtmlSanitizer();
        return sanitizer.Sanitize(input);
    }


    //return only 100 characters from the string input
    public static string TruncateString(string input, int length)
    {
        if (string.IsNullOrEmpty(input) || input.Length <= length)
        {
            return input;
        }
        return input.Substring(0, length);
    }
}
