using nyaml;
using System;
using System.Collections.Generic;
namespace Nintendo.MakeRom
{
	internal class DependencyList : UInt64ProgramIdArray
	{
		private const int NUM_DEPENDENCIES = 48;
		private static Dictionary<string, UInt64ProgramId> s_ProgramIdMapping = new Dictionary<string, UInt64ProgramId>();
		private List<UInt64ProgramId> m_ProgramIdList = new List<UInt64ProgramId>();
		public static void MakeProgramIdMapping(CollectionElement depList)
		{
			if (depList != null && depList.GetType() == typeof(Mapping))
			{
				Mapping mapping = (Mapping)depList;
				foreach (KeyValuePair<string, CollectionElement> current in mapping)
				{
					if (current.Value == null)
					{
						throw new MakeromException(string.Format("Not found program id\nprocess = {0}", current.Key));
					}
					if (current.Value.GetType() != typeof(ScalarLong))
					{
						throw new MakeromException(string.Format("Invail dependecy list element:\n key:{0} value:{1}\n", current.Key, current.Value));
					}
					DependencyList.s_ProgramIdMapping.Add(current.Key, new UInt64ProgramId((ulong)current.Value.GetLong()));
				}
			}
		}
		public DependencyList(string[] depList) : base(48)
		{
			if (depList != null)
			{
				for (int i = 0; i < depList.Length; i++)
				{
					string text = depList[i];
					if (!DependencyList.s_ProgramIdMapping.ContainsKey(text))
					{
						throw new MakeromException(string.Format("Cannot convert program id: {0}", text));
					}
					this.m_ProgramIdList.Add(DependencyList.s_ProgramIdMapping[text]);
				}
			}
		}
		public DependencyList(Mapping depList) : base(48)
		{
			if (depList != null)
			{
				foreach (KeyValuePair<string, CollectionElement> current in depList)
				{
					if (current.Value == null)
					{
						throw new MakeromException(string.Format("Not found program id\nprocess = {0}", current.Key));
					}
					if (current.Value.GetType() != typeof(ScalarLong))
					{
						throw new MakeromException(string.Format("Invail dependecy list element:\n key:{0} value:{1}\n", current.Key, current.Value));
					}
					this.m_ProgramIdList.Add(new UInt64ProgramId((ulong)current.Value.GetLong()));
				}
			}
		}
		protected override void Update()
		{
			base.ClearArray();
			foreach (UInt64ProgramId current in this.m_ProgramIdList)
			{
				base.AddProgramId(current);
			}
			base.Update();
		}
		public void UpdateVariation(byte variation)
		{
			List<UInt64ProgramId> list = new List<UInt64ProgramId>();
			foreach (UInt64ProgramId current in this.m_ProgramIdList)
			{
				ulong programId = (current & 18446744073709551360uL) | (ulong)variation;
				list.Add(new UInt64ProgramId(programId));
			}
			this.m_ProgramIdList = list;
			this.Update();
		}
		public void UpdateCategory(uint category)
		{
			List<UInt64ProgramId> list = new List<UInt64ProgramId>();
			foreach (UInt64ProgramId current in this.m_ProgramIdList)
			{
				ulong programId = (current & 18446726485818474495uL) | (ulong)category << 32;
				list.Add(new UInt64ProgramId(programId));
			}
			this.m_ProgramIdList = list;
			this.Update();
		}
	}
}
