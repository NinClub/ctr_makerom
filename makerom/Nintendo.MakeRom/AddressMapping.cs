using System;
using System.Collections.Generic;
using System.Globalization;
namespace Nintendo.MakeRom
{
	internal class AddressMapping : WritableBinaryRegistory
	{
		private const int PAGE_SIZE = 4096;
		private List<MappingDescriptor> m_MappingList = new List<MappingDescriptor>();
		public AddressMapping(string[] ioMappings, string[] memoryMappings)
		{
			ioMappings = ((ioMappings == null) ? new string[0] : ioMappings);
			memoryMappings = ((memoryMappings == null) ? new string[0] : memoryMappings);
			this.Initialize(ioMappings, memoryMappings);
		}
		private void AddStaticMappings(string[] staticMappings)
		{
			if (staticMappings == null)
			{
				return;
			}
			for (int i = 0; i < staticMappings.Length; i++)
			{
				string text = staticMappings[i];
				if (!text.Equals(""))
				{
					string[] array = text.Split(new char[]
					{
						':'
					});
					string text2 = (array.Length == 1) ? "" : array[1];
					string[] array2 = array[0].Split(new char[]
					{
						'-'
					});
					bool isReadOnly = text2.ToLower().Equals("r");
					uint num = uint.Parse(array2[0], NumberStyles.AllowHexSpecifier);
					if (!this.IsStartAddress(num))
					{
						throw new AddressMappingException(string.Format("Address {0:x} is not valid mapping start address.", num));
					}
					if (array2.Length == 2)
					{
						uint num2 = uint.Parse(array2[1], NumberStyles.AllowHexSpecifier);
						if (!this.IsEndAddress(num2))
						{
							throw new AddressMappingException(string.Format("Address {0:x} is not valid mapping end address.", num2));
						}
					}
					switch (array2.Length)
					{
					case 1:
						this.AddStaticMapping(new StaticMapping(num, isReadOnly));
						break;
					case 2:
					{
						StaticMapping staticMapping = new StaticMapping(num, isReadOnly);
						StaticMapping staticMapping2 = new StaticMapping(uint.Parse(array2[1], NumberStyles.AllowHexSpecifier) + 4096u, true);
						if (staticMapping.Equals(staticMapping2))
						{
							this.AddStaticMapping(new StaticMapping(num, isReadOnly));
						}
						else
						{
							this.AddStaticMapping(staticMapping, staticMapping2);
						}
						break;
					}
					default:
						throw new AddressMappingException(string.Format("Invalid mapping format: {0}", array[0]));
					}
				}
			}
		}
		private void AddIoMappings(string[] ioMappings)
		{
			if (ioMappings == null)
			{
				return;
			}
			for (int i = 0; i < ioMappings.Length; i++)
			{
				string text = ioMappings[i];
				if (!text.Equals(""))
				{
					string[] array = text.Split(new char[]
					{
						'-'
					});
					uint num = uint.Parse(array[0], NumberStyles.AllowHexSpecifier);
					if (!this.IsStartAddress(num))
					{
						throw new AddressMappingException(string.Format("Address {0:x} is not valid mapping start address.", num));
					}
					if (array.Length == 2)
					{
						uint num2 = uint.Parse(array[1], NumberStyles.AllowHexSpecifier);
						if (!this.IsEndAddress(num2))
						{
							throw new AddressMappingException(string.Format("Address {0:x} is not valid mapping end address.", num2));
						}
					}
					switch (array.Length)
					{
					case 1:
					{
						IoMapping desc = new IoMapping(num);
						this.AddIoMapping(desc);
						break;
					}
					case 2:
					{
						StaticMapping staticMapping = new StaticMapping(num, false);
						StaticMapping staticMapping2 = new StaticMapping(uint.Parse(array[1], NumberStyles.AllowHexSpecifier) + 4096u, false);
						if (staticMapping.Equals(staticMapping2))
						{
							this.AddIoMapping(new IoMapping(num));
						}
						else
						{
							this.AddStaticMapping(staticMapping, staticMapping2);
						}
						break;
					}
					default:
						throw new AddressMappingException(string.Format("Invalid mapping format: {0}", text));
					}
				}
			}
		}
		public void Initialize(string[] ioMappings, string[] staticMappings)
		{
			this.AddIoMappings(ioMappings);
			this.AddStaticMappings(staticMappings);
		}
		public AddressMapping(string mapSpec)
		{
			string[] ioMappings = mapSpec.Split(new char[]
			{
				','
			});
			this.Initialize(ioMappings, null);
		}
		public AddressMapping(params MappingDescriptor[] descs)
		{
			this.m_MappingList.AddRange(descs);
		}
		private void AddIoMapping(IoMapping desc)
		{
			this.AddMapping(desc);
		}
		private void AddStaticMapping(StaticMapping begin)
		{
			this.AddMapping(begin);
			this.AddMapping(new StaticMapping((begin.Data << 12) + 4096u, true));
		}
		private void AddStaticMapping(StaticMapping begin, StaticMapping end)
		{
			this.AddMapping(begin);
			this.AddMapping(end);
		}
		private void AddMapping(MappingDescriptor desc)
		{
			if (!this.m_MappingList.Contains(desc))
			{
				this.m_MappingList.Add(desc);
			}
		}
		protected override void Update()
		{
			base.ClearBinaries();
			base.AddBinaries(new IWritableBinary[]
			{
				new ByteArrayData(0u)
			});
			base.AddBinaries(this.m_MappingList.ToArray());
		}
		private bool IsStartAddress(uint address)
		{
			return (address & 4095u) == 0u;
		}
		private bool IsEndAddress(uint address)
		{
			return (address & 4095u) == 4095u;
		}
	}
}
