using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
namespace Nintendo.MakeRom.Properties
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
	internal class Resources
	{
		private static ResourceManager resourceMan;
		private static CultureInfo resourceCulture;
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("Nintendo.MakeRom.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}
		internal static string CrrDevKey
		{
			get
			{
				return Resources.ResourceManager.GetString("CrrDevKey", Resources.resourceCulture);
			}
		}
		internal static string Desc_Template
		{
			get
			{
				return Resources.ResourceManager.GetString("Desc_Template", Resources.resourceCulture);
			}
		}
		internal static string DevNcsdCfa
		{
			get
			{
				return Resources.ResourceManager.GetString("DevNcsdCfa", Resources.resourceCulture);
			}
		}
		internal static byte[] iQue_with_ISBN_LZ
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("iQue_with_ISBN_LZ", Resources.resourceCulture);
				return (byte[])@object;
			}
		}
		internal static byte[] iQue_without_ISBN_LZ
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("iQue_without_ISBN_LZ", Resources.resourceCulture);
				return (byte[])@object;
			}
		}
		internal static byte[] key
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("key", Resources.resourceCulture);
				return (byte[])@object;
			}
		}
		internal static byte[] Nintendo_DistributedBy_LZ
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("Nintendo_DistributedBy_LZ", Resources.resourceCulture);
				return (byte[])@object;
			}
		}
		internal static byte[] Nintendo_LicensedBy_LZ
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("Nintendo_LicensedBy_LZ", Resources.resourceCulture);
				return (byte[])@object;
			}
		}
		internal static byte[] Nintendo_LZ
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("Nintendo_LZ", Resources.resourceCulture);
				return (byte[])@object;
			}
		}
		internal static string Rsf_Default
		{
			get
			{
				return Resources.ResourceManager.GetString("Rsf_Default", Resources.resourceCulture);
			}
		}
		internal static string Rsf_Template
		{
			get
			{
				return Resources.ResourceManager.GetString("Rsf_Template", Resources.resourceCulture);
			}
		}
		internal Resources()
		{
		}
	}
}
