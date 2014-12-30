using System;
using System.Diagnostics;
using System.Net;
namespace Nintendo.MakeRom
{
	public class AesCounter
	{
		public static readonly int BufferSize = 1048576;
		private byte[] m_Data = new byte[16];
		private byte[] m_Buffer = new byte[AesCounter.BufferSize];
		private bool m_isInitBuffer;
		public AesCounter(ulong high, ulong low)
		{
			this.Initialize(high, low);
		}
		public AesCounter(byte[] iv)
		{
			long high = IPAddress.HostToNetworkOrder(BitConverter.ToInt64(iv, 0));
			long low = IPAddress.HostToNetworkOrder(BitConverter.ToInt64(iv, 8));
			this.Initialize((ulong)high, (ulong)low);
		}
		private unsafe void Initialize(ulong high, ulong low)
		{
			high = (ulong)IPAddress.HostToNetworkOrder((long)high);
			low = (ulong)IPAddress.HostToNetworkOrder((long)low);
			ulong[] array = new ulong[]
			{
				high,
				low
			};
			fixed (ulong* ptr = array)
			{
				fixed (byte* data = this.m_Data)
				{
					byte* ptr2 = (byte*)ptr;
					for (int i = 0; i < 16; i++)
					{
						data[i] = ptr2[i];
					}
				}
			}
		}
		public void Increment()
		{
			byte[] expr_0D_cp_0 = this.m_Data;
			int expr_0D_cp_1 = 15;
			if ((expr_0D_cp_0[expr_0D_cp_1] += 1) != 0)
			{
				return;
			}
			byte[] expr_2E_cp_0 = this.m_Data;
			int expr_2E_cp_1 = 14;
			if ((expr_2E_cp_0[expr_2E_cp_1] += 1) != 0)
			{
				return;
			}
			byte[] expr_4F_cp_0 = this.m_Data;
			int expr_4F_cp_1 = 13;
			if ((expr_4F_cp_0[expr_4F_cp_1] += 1) != 0)
			{
				return;
			}
			byte[] expr_70_cp_0 = this.m_Data;
			int expr_70_cp_1 = 12;
			if ((expr_70_cp_0[expr_70_cp_1] += 1) != 0)
			{
				return;
			}
			byte[] expr_91_cp_0 = this.m_Data;
			int expr_91_cp_1 = 11;
			if ((expr_91_cp_0[expr_91_cp_1] += 1) != 0)
			{
				return;
			}
			byte[] expr_B4_cp_0 = this.m_Data;
			int expr_B4_cp_1 = 10;
			if ((expr_B4_cp_0[expr_B4_cp_1] += 1) != 0)
			{
				return;
			}
			byte[] expr_D7_cp_0 = this.m_Data;
			int expr_D7_cp_1 = 9;
			if ((expr_D7_cp_0[expr_D7_cp_1] += 1) != 0)
			{
				return;
			}
			byte[] expr_F9_cp_0 = this.m_Data;
			int expr_F9_cp_1 = 8;
			if ((expr_F9_cp_0[expr_F9_cp_1] += 1) != 0)
			{
				return;
			}
			byte[] expr_11B_cp_0 = this.m_Data;
			int expr_11B_cp_1 = 7;
			if ((expr_11B_cp_0[expr_11B_cp_1] += 1) != 0)
			{
				return;
			}
			byte[] expr_13D_cp_0 = this.m_Data;
			int expr_13D_cp_1 = 6;
			if ((expr_13D_cp_0[expr_13D_cp_1] += 1) != 0)
			{
				return;
			}
			byte[] expr_15F_cp_0 = this.m_Data;
			int expr_15F_cp_1 = 5;
			if ((expr_15F_cp_0[expr_15F_cp_1] += 1) != 0)
			{
				return;
			}
			byte[] expr_181_cp_0 = this.m_Data;
			int expr_181_cp_1 = 4;
			if ((expr_181_cp_0[expr_181_cp_1] += 1) != 0)
			{
				return;
			}
			byte[] expr_1A3_cp_0 = this.m_Data;
			int expr_1A3_cp_1 = 3;
			if ((expr_1A3_cp_0[expr_1A3_cp_1] += 1) != 0)
			{
				return;
			}
			byte[] expr_1C5_cp_0 = this.m_Data;
			int expr_1C5_cp_1 = 2;
			if ((expr_1C5_cp_0[expr_1C5_cp_1] += 1) != 0)
			{
				return;
			}
			byte[] expr_1E7_cp_0 = this.m_Data;
			int expr_1E7_cp_1 = 1;
			if ((expr_1E7_cp_0[expr_1E7_cp_1] += 1) != 0)
			{
				return;
			}
			byte[] expr_209_cp_0 = this.m_Data;
			int expr_209_cp_1 = 0;
			byte b = expr_209_cp_0[expr_209_cp_1] += 1;
		}
		public byte[] GetBytes()
		{
			return this.m_Data;
		}
		public byte[] GetMultipleCounters(int size)
		{
			if (this.m_isInitBuffer && size == this.m_Buffer.Length && this.m_Data[14] == 0 && this.m_Data[15] == 0 && this.m_Data[13] != 255)
			{
				for (int i = 0; i < size / 16; i++)
				{
					byte[] expr_52_cp_0 = this.m_Buffer;
					int expr_52_cp_1 = i * 16 + 13;
					expr_52_cp_0[expr_52_cp_1] += 1;
				}
				byte[] expr_78_cp_0 = this.m_Data;
				int expr_78_cp_1 = 13;
				expr_78_cp_0[expr_78_cp_1] += 1;
				return this.m_Buffer;
			}
			for (int j = 0; j < size / 16; j++)
			{
				Array.Copy(this.m_Data, 0, this.m_Buffer, j * 16, 16);
				this.Increment();
			}
			if (size == this.m_Buffer.Length)
			{
				this.m_isInitBuffer = true;
			}
			return this.m_Buffer;
		}
		[Conditional("DEBUG")]
		public static void CounterBytesTest()
		{
			AesCounter aesCounter = new AesCounter(4096uL, 16776960uL);
			AesCounter aesCounter2 = new AesCounter(4096uL, 16776960uL);
			aesCounter.GetMultipleCounters(1048576);
			for (int i = 0; i < 65536; i++)
			{
				byte[] bytes = aesCounter2.GetBytes();
				for (int j = 0; j < bytes.Length; j++)
				{
				}
				aesCounter2.Increment();
			}
			for (int k = 1024; k <= 1048576; k += 1024)
			{
				aesCounter.GetMultipleCounters(k);
				for (int l = 0; l < k / 16; l++)
				{
					byte[] bytes2 = aesCounter2.GetBytes();
					for (int m = 0; m < bytes2.Length; m++)
					{
					}
					aesCounter2.Increment();
				}
			}
		}
	}
}
