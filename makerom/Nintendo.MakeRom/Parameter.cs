using Nintendo.MakeRom.Properties;
using nyaml;
using System;
using System.Collections.Generic;
namespace Nintendo.MakeRom
{
	public class Parameter
	{
		private string m_RsfPath;
		private string m_DescPath;
		private Dictionary<string, string> m_UserVariables;
		private NyamlParameter m_UserParameter;
		private NyamlParameter m_DescDeafultParameter;
		private NyamlParameter m_MakeromDefaultParameter;
		private NyamlParameter m_MergedUserParameter;
		private NyamlDescParameter m_DescParameter;
		public Parameter(string rsf, string desc, Dictionary<string, string> variables)
		{
			this.m_RsfPath = rsf;
			this.m_DescPath = desc;
			this.m_UserVariables = variables;
		}
		public NyamlParameter GetUserParameter()
		{
			if (this.m_UserParameter == null)
			{
				this.m_UserParameter = new NyamlParameter(Nyaml.LoadFromFile(this.m_RsfPath, this.m_UserVariables));
			}
			return this.m_UserParameter;
		}
		public NyamlDescParameter GetDescParameter()
		{
			if (this.m_DescParameter == null)
			{
				this.m_DescParameter = new NyamlDescParameter(Nyaml.LoadFromFile(this.m_DescPath, this.m_UserVariables));
			}
			return this.m_DescParameter;
		}
		public NyamlParameter GetDescDefaultParameter()
		{
			if (this.m_DescDeafultParameter == null && this.m_DescPath != null)
			{
				Nyaml nyaml = Nyaml.LoadFromFile(this.m_DescPath, this.m_UserVariables);
				Nyaml nyaml2 = new Nyaml((Collection)nyaml["DefaultSpec"]);
				this.m_DescDeafultParameter = new NyamlParameter(nyaml2);
			}
			return this.m_DescDeafultParameter;
		}
		public NyamlParameter GetMakeromDefaultParameter()
		{
			if (this.m_MakeromDefaultParameter == null)
			{
				this.m_MakeromDefaultParameter = new NyamlParameter(Nyaml.LoadFromText(Resources.Rsf_Default));
			}
			return this.m_MakeromDefaultParameter;
		}
		public NyamlParameter GetMergedUserParameter()
		{
			if (this.m_MergedUserParameter == null)
			{
				this.m_MergedUserParameter = this.GetUserParameter();
				NyamlParameter descDefaultParameter = this.GetDescDefaultParameter();
				if (((this.m_MergedUserParameter.CategoryFlags != null && this.m_MergedUserParameter.CategoryFlags.GetType() != typeof(ScalarNull)) || (this.m_MergedUserParameter.TitleCategory != null && this.m_MergedUserParameter.TitleCategory.GetType() != typeof(ScalarNull))) && descDefaultParameter != null)
				{
					descDefaultParameter.CategoryFlags = null;
					descDefaultParameter.TitleCategory = null;
				}
				this.m_MergedUserParameter.Merge(descDefaultParameter);
				this.m_MergedUserParameter.Merge(this.GetMakeromDefaultParameter());
			}
			return this.m_MergedUserParameter;
		}
	}
}
