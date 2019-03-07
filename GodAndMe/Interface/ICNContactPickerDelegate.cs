using System;
namespace GodAndMe.Interface
{
    [Foundation.Protocol(Name = "CNContactPickerDelegate", WrapperType = typeof(ContactPickerDelegate))]
    [ObjCRuntime.Introduced(ObjCRuntime.PlatformName.iOS, 9, 0, ObjCRuntime.PlatformArchitecture.All, null)]
    public interface ICNContactPickerDelegate : IDisposable, ObjCRuntime.INativeObject
}
