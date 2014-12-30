using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Nintendo.MakeRom
{
	internal class PlainRegionBinary : WritableBinaryRegistory
	{
		private const uint HEADERS_SIZE = 2560u;
		private List<IWritableBinary> m_PlainData = new List<IWritableBinary>();
		private ByteArrayData m_FootPadding = new byte[0];
		private byte m_PaddingChar;
		private bool m_IsCip;
		private void UpdateFootPadding()
		{
			this.m_FootPadding = new byte[0];
			uint paddingSize = Util.CalculatePaddingSize(this.Size, 512);
			this.m_FootPadding = Util.MakePaddingData(paddingSize, this.m_PaddingChar);
		}
		private List<string> GetUniqueList(List<string> list)
		{
			List<string> list2 = new List<string>();
			foreach (string current in list)
			{
				if (!list2.Contains(current))
				{
					list2.Add(current);
				}
			}
			return list2;
		}
		public PlainRegionBinary(Elf elf, MakeCxiOptions options)
		{
			this.m_IsCip = options.IsCip;
			this.m_PlainData.Add(new ByteArrayData(0u));
			this.m_PaddingChar = options.Padding;
			List<string> list = new List<string>();
			if (elf != null)
			{
				foreach (string current in options.PlainRegionSections)
				{
					ElfSectionHeader sectionHeader = elf.GetSectionHeader(current);
					if (sectionHeader != null)
					{
						byte[] data = elf.GetData(sectionHeader.Offset, sectionHeader.Size);
						List<string> arg_A9_0 = list;
						string arg_9F_0 = Encoding.UTF8.GetString(data);
						char[] separator = new char[1];
						arg_A9_0.AddRange(arg_9F_0.Split(separator).ToList<string>());
					}
				}
			}
			if (options.RomFsRoot != null && options.RomFsRoot != "")
			{
				CrrUpdater crrUpdater = new CrrUpdater(options.RomFsRoot);
				List<string> moduleIdList = crrUpdater.GetModuleIdList();
				list.AddRange(moduleIdList);
			}
			list.RemoveAll((string word) => word == "");
			list = this.GetUniqueList(list);
			byte[] bytes = Encoding.UTF8.GetBytes(string.Join("\0", list.ToArray()));
			ByteArrayData binary = new ByteArrayData(bytes);
			this.AddData(binary);
			this.UpdateFootPadding();
		}
		protected override void Update()
		{
			base.ClearBinaries();
			base.AddBinaries(new IWritableBinary[]
			{
				new ByteArrayData(0u)
			});
			if (!this.m_IsCip)
			{
				foreach (IWritableBinary current in this.m_PlainData)
				{
					base.AddBinaries(new IWritableBinary[]
					{
						current
					});
				}
				base.AddBinaries(new IWritableBinary[]
				{
					this.m_FootPadding
				});
			}
		}
		public void AddData(IWritableBinary binary)
		{
			this.m_PlainData.Add(binary);
			this.UpdateFootPadding();
		}
	}
}
