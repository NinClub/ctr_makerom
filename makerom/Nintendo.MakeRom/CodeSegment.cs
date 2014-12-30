using System;
using System.Security.Cryptography;
namespace Nintendo.MakeRom
{
	internal class CodeSegment
	{
		public CodeSegmentInfo Info
		{
			get;
			private set;
		}
		public byte[] Data
		{
			get;
			private set;
		}
		public CodeSegment(uint address, uint numMaxPages, uint size, byte[] data)
		{
			this.Info = new CodeSegmentInfo(address, numMaxPages, size);
			this.Data = data;
		}
		public byte[] GetSha256Hash()
		{
			SHA256 sHA = new SHA256Managed();
			return sHA.ComputeHash(this.Data);
		}
	}
}
