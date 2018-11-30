using UnityEngine;
using UnityEditor;
using System.IO;

public class HandleTextFile
{
    public static void WriteString(string file, string value, bool append = true)
    {
        if (!File.Exists(file))
            File.Create(file).Dispose();

        StreamWriter writer = new StreamWriter(file, append);
        writer.Write(value);
        writer.Close();
    }

    /// <summary>Replace crt file text by value</summary>
    /// <param name="file">path of the file</param>
    /// <param name="value">value to set in the file</param>
    public static void WriteRString(string file, string value)
    {
        WriteString(file, value, false);
    }

    public static string ReadString(string file)
    {
        if (!File.Exists(file)) return "";

        StreamReader reader = new StreamReader(file);
        string value = reader.ReadToEnd();
        reader.Close();
        return value;
    }
}