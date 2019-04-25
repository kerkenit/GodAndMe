using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Android.App;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("God & Me")]
[assembly: AssemblyDescription("Everything between God and you")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Kerk en IT")]
[assembly: AssemblyProduct("God & Me")]
[assembly: AssemblyCopyright("Copyright © Kerk en IT 2019")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
#if __ANDROID__
#if DEBUG
[assembly: Application(Debuggable = true, Label = "@string/app_name")]
#else
[assembly: Application(Debuggable = false, Label = "@string/app_name")]
#endif
#else
#if DEBUG
    [assembly: Application(Debuggable = true)]
#else
    [assembly: Application(Debuggable = false)]
#endif
#endif
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// Add some common permissions, these can be removed if not needed
[assembly: UsesPermission(Android.Manifest.Permission.Internet)]
[assembly: UsesPermission(Android.Manifest.Permission.WriteExternalStorage)]
[assembly: UsesPermission(Android.Manifest.Permission.ReadExternalStorage)]
[assembly: UsesPermission(Android.Manifest.Permission.ReadContacts)]
