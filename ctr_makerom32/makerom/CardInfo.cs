using Nintendo.MakeRom;
using System;
namespace makerom
{
	internal class CardInfo : WritableBinaryRegistory
	{
		private CardInfoHeader m_Header = new CardInfoHeader();
		public ulong MediaId
		{
			set
			{
				this.m_Header.MediaId = value;
			}
		}
		public byte[] NcchHeader
		{
			set
			{
				this.m_Header.NcchHeader = value;
			}
		}
		public CardInfo(MakeCciOptions options)
		{
			this.m_Header.CardInfo = options.CardInfo;
			this.m_Header.MediaId = 0uL;
			this.m_Header.NcchHeader = new byte[256];
		}
		protected override void Update()
		{
			base.ClearBinaries();
			base.AddBinaries(new IWritableBinary[]
			{
				this.m_Header
			});
		}
	}
}
