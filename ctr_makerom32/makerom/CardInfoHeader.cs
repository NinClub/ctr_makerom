using makerom.Properties;
using Nintendo.MakeRom;
using System;
namespace makerom
{
	internal class CardInfoHeader : StructData<CardInfoHeaderStruct>
	{
		public ulong CardInfo
		{
			get
			{
				return this.Struct.CardInfo;
			}
			set
			{
				this.Struct.CardInfo = value;
			}
		}
		public ulong MediaId
		{
			get
			{
				return this.Struct.MediaId;
			}
			set
			{
				this.Struct.MediaId = value;
			}
		}
		public byte[] TitleKey
		{
			get
			{
				return this.Struct.TitleKey;
			}
			set
			{
				this.Struct.TitleKey = value;
			}
		}
		public byte[] InitialData
		{
			get
			{
				return this.Struct.InitialData;
			}
			set
			{
				this.Struct.InitialData = value;
			}
		}
		public byte[] NcchHeader
		{
			get
			{
				return this.Struct.NcchHeader;
			}
			set
			{
				this.Struct.NcchHeader = value;
			}
		}
		protected override void Update()
		{
			this.UpdateTitleKey();
			base.Update();
		}
		private void UpdateTitleKey()
		{
			this.InitialData = Resources.InitialData;
			this.TitleKey = Resources.TitleKey;
		}
	}
}
