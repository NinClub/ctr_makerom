using System;
using System.Linq;
namespace Nintendo.MakeRom
{
	internal class SystemCallAccessControl : WritableBinaryRegistory
	{
		private const int NUM_MAX_DESCRIPTORS = 8;
		private uint[] m_Flags = new uint[8];
		public SystemCallAccessControl(int[] defaults, int[] append)
		{
			for (int i = 0; i < 8; i++)
			{
				this.m_Flags[i] = 0u;
			}
			defaults = ((defaults != null) ? defaults : new int[0]);
			append = ((append != null) ? append : new int[0]);
			int[] array = defaults.Concat(append).ToArray<int>();
			int[] array2 = array;
			for (int j = 0; j < array2.Length; j++)
			{
				int id = array2[j];
				this.EnableSystemCall(id);
			}
		}
		private void EnableSystemCall(int id)
		{
			int num = id / 24;
			int num2 = id % 24;
			this.m_Flags[num] |= 1u << num2;
		}
		private void EnableSystemCall(int[] ids)
		{
			for (int i = 0; i < ids.Length; i++)
			{
				int id = ids[i];
				this.EnableSystemCall(id);
			}
		}
		private void DisableSystemCall(int[] ids)
		{
			for (int i = 0; i < ids.Length; i++)
			{
				int id = ids[i];
				this.DisableSystemCall(id);
			}
		}
		private void DisableSystemCall(int id)
		{
			int num = id / 24;
			int num2 = id % 24;
			this.m_Flags[num] &= ~(1u << num2);
		}
		protected override void Update()
		{
			base.ClearBinaries();
			base.AddBinaries(new IWritableBinary[]
			{
				new ByteArrayData(0u)
			});
			for (int i = 0; i < 8; i++)
			{
				if ((this.m_Flags[i] & 16777215u) != 0u)
				{
					SystemCallAccessControlDescriptor systemCallAccessControlDescriptor = new SystemCallAccessControlDescriptor((uint)i, this.m_Flags[i]);
					SystemCallAccessControlDescriptor systemCallAccessControlDescriptor2 = new SystemCallAccessControlDescriptor((uint)i, 0u);
					if (systemCallAccessControlDescriptor != systemCallAccessControlDescriptor2)
					{
						base.AddBinaries(new IWritableBinary[]
						{
							systemCallAccessControlDescriptor
						});
					}
				}
			}
		}
	}
}
