using System;
namespace Nintendo.MakeRom
{
	[Flags]
	internal enum ARM11KernelCapabilityFlag : uint
	{
		EnableDebug = 1u,
		EnableForceDebugging = 2u
	}
}
