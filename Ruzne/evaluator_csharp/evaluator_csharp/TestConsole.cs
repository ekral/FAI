using System.Diagnostics.CodeAnalysis;
using System.IO;


[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public static class TestConsole
{
    private readonly static int maxLength = 128;
    private readonly static StringWriter writer = new();

    
    public static string Message
    {
        get
        {
            string message = writer.ToString();

            message = message.Length > maxLength ? message[..maxLength] : message;
            message = System.Net.WebUtility.HtmlEncode(message);

            return message;
        }
    }

    public static void Clear()
    {
        writer.GetStringBuilder().Clear();
    }

    public static void WriteLine(string message)
    {
        writer.Write(message);
    }
}
