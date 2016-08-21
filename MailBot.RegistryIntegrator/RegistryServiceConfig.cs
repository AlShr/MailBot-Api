using System;
using Microsoft.Win32;

namespace MailBot.RegistryIntegrator
{
    public static class RegistryServiceConfig
    {
        private static readonly string GangName = "GangOfFour";
        private static readonly string AppName = "MailBot";
        private static readonly string SoftwareSubkey = "Software";

        public static void WriteEntry(RegistryEntry destination, string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException( "value" );
            }
            GetWritableKey( rk => rk.SetValue( destination.ToString(), value ) );
        }

        private static void GetReadableKey(Action<RegistryKey> action)
        {
            if (action != null)
            {
                string error;
                using (var softKey = Registry.CurrentUser.OpenSubKey( SoftwareSubkey ))
                {
                    if (softKey == null)
                    {
                        error = "HKEY_CURRENT_USER/Software key doesn't exist or is inaccessible, which is pretty unexpected.";
                    }
                    else
                    {
                        using (var gangKey = softKey.OpenSubKey( GangName ))
                        {
                            if (gangKey == null)
                            {
                                error = "Could not open " + GangName + " key.";
                            }
                            else
                            {
                                using (var appKey = gangKey.OpenSubKey( AppName ))
                                {
                                    if (appKey == null)
                                    {
                                        error = "Could not open " + GangName + "\\" + AppName + " key.";
                                    }
                                    else
                                    {
                                        action( appKey );
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                Console.WriteLine( "Something went wrong. Check if you're running application with admin rights.{0}{1}", Environment.NewLine, error );
            }
        }

        private static void GetWritableKey(Action<RegistryKey> action)
        {
            WorkOnAppKey( action, true );
        }

        private static void WorkOnAppKey(Action<RegistryKey> action, bool write)
        {
            string error;
            using (var softKey = Registry.CurrentUser.OpenSubKey( SoftwareSubkey, write ))
            {
                if (softKey == null)
                {
                    error = "HKEY_CURRENT_USER/Software key doesn't exist or is inaccessible, which is pretty unexpected.";
                }
                else
                {
                    using (var gangKey = softKey.CreateSubKey( GangName ))
                    {
                        if (gangKey == null)
                        {
                            error = "Could not open " + GangName + " key.";
                        }
                        else
                        {
                            using (var appKey = gangKey.CreateSubKey( AppName ))
                            {
                                if (appKey == null)
                                {
                                    error = "Could not open " + GangName + "\\" + AppName + " key.";
                                }
                                else
                                {
                                    if (action != null)
                                    {
                                        action( appKey );
                                    }
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine( "Something went wrong. Check if you're running application with admin rights.{0}{1}", Environment.NewLine, error );
        }

        public static string ReadEntry(RegistryEntry entry)
        {
            string data = null;
            GetReadableKey( rk => data = (string) rk.GetValue( entry.ToString() ) );
            return data;
        }

        public static void RemoveEntries()
        {
            try
            {
                using (var softKey = Registry.CurrentUser.OpenSubKey( SoftwareSubkey, true ))
                {
                    if (softKey != null)
                    {
                        try
                        {
                            softKey.DeleteSubKeyTree( GangName, true );
                            Console.WriteLine( "Successfully removed existing configuration!" );
                        }
                        catch (Exception)
                        {
                            Console.WriteLine( "There are no configuration to remove!" );
                        }
                    }
                    else
                    {
                        Console.WriteLine( "Software key doesn't exist, which is pretty unexpected." );
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine( e.Message );
            }
        }
    }
}