using System;
using Foundation;
using LocalAuthentication;
using UIKit;

namespace GodAndMe.iOS.Helpers
{
    public static class LocalAuthHelper
    {
        private enum LocalAuthType
        {
            None,
            Passcode,
            TouchId,
            FaceId
        }

        public static string GetLocalAuthLabelText()
        {
            var localAuthType = GetLocalAuthType();

            switch (localAuthType)
            {
                case LocalAuthType.Passcode:
                    return NSBundle.MainBundle.GetLocalizedString("RequirePasscode");
                case LocalAuthType.TouchId:
                    return NSBundle.MainBundle.GetLocalizedString("RequireTouchID");
                case LocalAuthType.FaceId:
                    return NSBundle.MainBundle.GetLocalizedString("RequireFaceID");
                default:
                    return string.Empty;
            }
        }

        public static string GetLocalAuthIcon()
        {
            var localAuthType = GetLocalAuthType();

            switch (localAuthType)
            {
                //case LocalAuthType.Passcode:
                //    return SvgLibrary.LockIcon;
                //case LocalAuthType.TouchId:
                //    return SvgLibrary.TouchIdIcon;
                //case LocalAuthType.FaceId:
                //    return SvgLibrary.FaceIdIcon;
                default:
                    return string.Empty;
            }
        }

        public static string GetLocalAuthUnlockText()
        {
            var localAuthType = GetLocalAuthType();

            switch (localAuthType)
            {
                case LocalAuthType.Passcode:
                    return NSBundle.MainBundle.GetLocalizedString("UnlockWithPasscode");
                case LocalAuthType.TouchId:
                    return NSBundle.MainBundle.GetLocalizedString("UnlockWithTouchID");
                case LocalAuthType.FaceId:
                    return NSBundle.MainBundle.GetLocalizedString("UnlockWithFaceID");
                default:
                    return string.Empty;
            }
        }

        public static bool IsLocalAuthAvailable => GetLocalAuthType() != LocalAuthType.None;

        public static void Authenticate(Action onSuccess, Action onFailure)
        {
            var context = new LAContext();
            NSError AuthError;

            if (context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out AuthError)
                || context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthentication, out AuthError))
            {
                var replyHandler = new LAContextReplyHandler((success, error) =>
                {
                    if (success)
                    {
                        onSuccess?.Invoke();
                    }
                    else
                    {
                        onFailure?.Invoke();
                    }
                });

                context.EvaluatePolicy(LAPolicy.DeviceOwnerAuthentication, NSBundle.MainBundle.GetLocalizedString("PleaseAuthenticateToProceed"), replyHandler);
            }
        }

        private static LocalAuthType GetLocalAuthType()
        {
            var localAuthContext = new LAContext();
            NSError AuthError;

            if (localAuthContext.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthentication, out AuthError))
            {
                if (localAuthContext.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out AuthError))
                {
                    if (GetOsMajorVersion() >= 11 && localAuthContext.BiometryType == LABiometryType.FaceId)
                    {
                        return LocalAuthType.FaceId;
                    }

                    return LocalAuthType.TouchId;
                }

                return LocalAuthType.Passcode;
            }

            return LocalAuthType.None;
        }

        private static int GetOsMajorVersion()
        {
            return int.Parse(UIDevice.CurrentDevice.SystemVersion.Split('.')[0]);
        }
    }
}
