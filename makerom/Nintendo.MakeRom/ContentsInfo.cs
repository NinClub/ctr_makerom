using System;
namespace Nintendo.MakeRom
{
	public class ContentsInfo
	{
		public string Title
		{
			get;
			set;
		}
		internal ulong ProgramIdRaw
		{
			private get;
			set;
		}
		internal byte[] Arm11Flags
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
		public ContentsCoreInfo CoreInfo
		{
			get;
			set;
		}
		public RomFsInfo RomFsInfo
		{
			get;
			set;
		}
		public CodeInfo CodeInfo
		{
			get;
			set;
		}
		public ContentsInfo()
		{
			this.CodeInfo = new CodeInfo();
		}
		public byte[] GetCxidData()
		{
			byte[] array = new byte[8];
			BitConverter.GetBytes(this.ProgramIdRaw).CopyTo(array, 0);
			return array;
		}
	}
}
