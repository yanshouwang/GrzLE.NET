using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GrzLE.WPF.Native
{
    static class Kernel32
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern int GetCurrentPackageFullName(ref int packageFullNameLength, ref StringBuilder packageFullName);

        public static bool RunningWithIdentity
        {
            get
            {
                if (Windows7OrLower)
                {
                    return false;
                }
                else
                {
                    var sb = new StringBuilder(1024);
                    var length = 0;
                    var result = GetCurrentPackageFullName(ref length, ref sb);
                    return result != 15700;
                }
            }
        }

        static bool Windows7OrLower
        {
            get
            {
                var versionMajor = Environment.OSVersion.Version.Major;
                var versionMinor = Environment.OSVersion.Version.Minor;
                var version = versionMajor + (double)versionMinor / 10;
                return version <= 6.1;
            }
        }

        public static string GetSafeAppxLocalFolder()
        {
            try
            {
                return Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            }
            catch (Exception ioe)
            {
                System.Diagnostics.Debug.WriteLine(ioe.Message);
            }

            return null;
        }
    }
}
