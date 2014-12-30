using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
namespace makerom.Properties
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
					ResourceManager resourceManager = new ResourceManager("makerom.Properties.Resources", typeof(Resources).Assembly);
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
		internal static string DevNcsdCfa
		{
			get
			{
				return Resources.ResourceManager.GetString("DevNcsdCfa", Resources.resourceCulture);
			}
		}
		internal static byte[] InitialData
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("InitialData", Resources.resourceCulture);
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
		internal static byte[] TitleKey
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("TitleKey", Resources.resourceCulture);
				return (byte[])@object;
			}
		}
		internal Resources()
		{
		}
	}
}
