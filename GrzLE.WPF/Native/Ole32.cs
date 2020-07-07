using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GrzLE.WPF.Native
{
    enum RpcAuthnLevel
    {
        Default = 0,
        None = 1,
        Connect = 2,
        Call = 3,
        Pkt = 4,
        PktIntegrity = 5,
        PktPrivacy = 6
    }

    enum RpcImpLevel
    {
        Default = 0,
        Anonymous = 1,
        Identify = 2,
        Impersonate = 3,
        Delegate = 4
    }

    enum EoAuthnCap
    {
        None = 0x00,
        MutualAuth = 0x01,
        StaticCloaking = 0x20,
        DynamicCloaking = 0x40,
        AnyAuthority = 0x80,
        MakeFullSIC = 0x100,
        Default = 0x800,
        SecureRefs = 0x02,
        AccessControl = 0x04,
        AppID = 0x08,
        Dynamic = 0x10,
        RequireFullSIC = 0x200,
        AutoImpersonate = 0x400,
        NoCustomMarshal = 0x2000,
        DisableAAA = 0x1000
    }

    enum HRESULT : uint
    {
        S_OK = 0x00000000,
        RPC_E_TOO_LATE = 0x80010119,
        RPC_E_NO_GOOD_SECURITY_PACKAGES = 0x8001011A,
        E_OUT_OF_MEMORY = 0x80290105
    }

    static class Ole32
    {
        [DllImport("ole32.dll")]
        public static extern HRESULT CoInitializeSecurity(IntPtr pVoid, int cAuthSvc, IntPtr asAuthSvc, IntPtr pReserved1, RpcAuthnLevel level, RpcImpLevel impers, IntPtr pAuthList, EoAuthnCap dwCapabilities, IntPtr pReserved3);
    }
}
