using System;
namespace Nintendo.MakeRom
{
	public class ContentsCoreInfo
	{
		internal byte[] HeaderIvRaw
		{
			get;
			set;
		}
		internal byte[] ExeFsIvRaw
		{
			get;
			set;
		}
		internal byte[] RomFsIvRaw
		{
			get;
			set;
		}
		public long ExeFsSize
		{
			get;
			set;
		}
		public long RomFsSize
		{
			get;
			set;
		}
		public string ExeFsIv
		{
			get
			{
				if (this.ExeFsIvRaw == null)
				{
					return null;
				}
				return BitConverter.ToString(this.ExeFsIvRaw);
			}
			set
			{
			}
		}
		public string RomFsIv
		{
			get
			{
				if (this.RomFsIvRaw == null)
				{
					return null;
				}
				return BitConverter.ToString(this.RomFsIvRaw);
			}
			set
			{
			}
		}
		public string HeaderIv
		{
			get
			{
				if (this.HeaderIvRaw == null)
				{
					return null;
				}
				return BitConverter.ToString(this.HeaderIvRaw);
			}
			set
			{
			}
		}
		internal ulong ProgramIdRaw
		{
			private get;
			set;
		}
		internal string ProgramId
		{
			get
			{
				return string.Format("{0:x16}", this.ProgramIdRaw);
			}
			set
			{
			}
		}
	}
}
