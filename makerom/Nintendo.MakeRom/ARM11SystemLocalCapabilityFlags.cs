using System;
namespace Nintendo.MakeRom
{
	internal class ARM11SystemLocalCapabilityFlags : ByteArrayData
	{
		protected override int ByteSize
		{
			get
			{
				return 8;
			}
		}
		public ARM11SystemLocalCapabilityFlags(MakeCxiOptions option)
		{
			this.SetCoreVersion(option.CoreVersion);
			base.Data[6] = this.MakeByte6(option);
			base.Data[7] = (byte)(sbyte)option.MainThreadPriority;
		}
		private void SetCoreVersion(uint coreVersion)
		{
			base.Data[0] = (byte)(coreVersion & 255u);
			base.Data[1] = (byte)(coreVersion >> 8 & 255u);
			base.Data[2] = (byte)(coreVersion >> 16 & 255u);
			base.Data[3] = (byte)(coreVersion >> 24 & 255u);
		}
		private byte MakeByte6(MakeCxiOptions option)
		{
			byte b = (byte)(option.AffinityMask.Data & 3);
			byte b2 = (byte)(option.IdealProcessor & 3);
			byte systemMode = option.SystemMode;
			return (byte)((int)systemMode << 4 | (int)b << 2 | (int)b2);
		}
	}
}
