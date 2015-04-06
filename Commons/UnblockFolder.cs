using System;
using System.Globalization;
using System.IO;
using Trinet.Core.IO.Ntfs;

public static class FileInfoExtensions
{
    private const string ZoneIdentifierStreamName = "Zone.Identifier";

    public static void Unblock(this FileInfo file)
    {
        if (file == null)
        {
            throw new ArgumentNullException("file");
        }

        if (!file.Exists)
        {
            throw new FileNotFoundException("Unable to find the specified file.", file.FullName);
        }

        if (file.Exists && file.AlternateDataStreamExists(ZoneIdentifierStreamName))
        {
            file.DeleteAlternateDataStream(ZoneIdentifierStreamName);
        }
    }
}

public static class DirectoryInfoExtensions
{
    private const string ZoneIdentifierStreamName = "Zone.Identifier";

    public static void Unblock(this DirectoryInfo directory)
    {
        directory.Unblock(false);
    }

    public static void Unblock(this DirectoryInfo directory, bool isRecursive)
    {
        if (directory == null)
        {
            throw new ArgumentNullException("file");
        }

        if (!directory.Exists)
        {
            throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, "The specified directory '{0}' cannot be found.", directory.FullName));
        }

        if (directory.AlternateDataStreamExists(ZoneIdentifierStreamName))
        {
            directory.DeleteAlternateDataStream(ZoneIdentifierStreamName);
        }

        if (!isRecursive)
        {
            return;
        }

        foreach (DirectoryInfo item in directory.GetDirectories())
        {
            item.Unblock(true);
        }

        foreach (FileInfo item in directory.GetFiles())
        {
            item.Unblock();
        }
    }
}