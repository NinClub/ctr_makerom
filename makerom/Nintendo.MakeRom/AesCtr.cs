using System;
using System.Security.Cryptography;
namespace Nintendo.MakeRom
{
	public class AesCtr : ICryptoTransform, IDisposable
	{
		private AesCryptoServiceProvider m_Aes = new AesCryptoServiceProvider();
		private ICryptoTransform m_Encryptor;
		private AesCounter m_Counter;
		public byte[] key
		{
			get
			{
				return this.m_Aes.Key;
			}
		}
		public bool CanReuseTransform
		{
			get
			{
				return false;
			}
		}
		public bool CanTransformMultipleBlocks
		{
			get
			{
				return true;
			}
		}
		public int InputBlockSize
		{
			get
			{
				return this.m_Aes.BlockSize / 8;
			}
		}
		public int OutputBlockSize
		{
			get
			{
				return this.m_Aes.BlockSize / 8;
			}
		}
		public AesCtr(byte[] key, byte[] iv)
		{
			AesCounter iv2 = new AesCounter(iv);
			this.Initialize(key, iv2);
		}
		public AesCtr(byte[] key, ulong partitionId, ulong initCount)
		{
			AesCounter iv = new AesCounter(partitionId, initCount);
			this.Initialize(key, iv);
		}
		private void DumpKey()
		{
			byte[] key = this.m_Aes.Key;
			for (int i = 0; i < key.Length; i++)
			{
				byte b = key[i];
				Console.Write("{0:x2}", b);
			}
			Console.Write("\n");
		}
		private void Initialize(byte[] key, AesCounter iv)
		{
			this.m_Aes.Key = key;
			this.m_Aes.Mode = CipherMode.ECB;
			this.m_Aes.Padding = PaddingMode.None;
			this.m_Counter = iv;
			this.m_Encryptor = this.m_Aes.CreateEncryptor();
		}
		public ICryptoTransform GetEncryptor()
		{
			return this.m_Aes.CreateEncryptor();
		}
		private void IncrementCounter()
		{
			this.m_Counter.Increment();
		}
		public byte[] GetCounter()
		{
			return this.m_Counter.GetBytes();
		}
		public unsafe int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			fixed (byte* ptr = outputBuffer)
			{
				fixed (byte* ptr2 = inputBuffer)
				{
					ulong* ptr3 = (ulong*)ptr + outputOffset / 8;
					ulong* ptr4 = (ulong*)ptr2 + inputOffset / 8;
					int num;
					for (int i = 0; i < inputCount; i += num)
					{
						num = ((inputCount - i > AesCounter.BufferSize) ? AesCounter.BufferSize : (inputCount - i));
						byte[] multipleCounters = this.m_Counter.GetMultipleCounters(num);
						this.m_Encryptor.TransformBlock(multipleCounters, 0, num, outputBuffer, outputOffset + i);
						for (int j = i / 8; j < (i + num) / 8; j++)
						{
							ptr3[j] ^= ptr4[j];
						}
					}
				}
			}
			return inputCount;
		}
		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			byte[] array = new byte[inputCount];
			int num = this.m_Aes.BlockSize / 8;
			for (int i = 0; i < inputCount; i += num)
			{
				int num2 = (inputCount - i > num) ? num : (inputCount - i);
				byte[] array2 = this.m_Encryptor.TransformFinalBlock(this.m_Counter.GetBytes(), 0, num);
				for (int j = 0; j < num2; j++)
				{
					array[i + j] = (byte)(inputBuffer[inputOffset + i + j] ^ array2[j]);
				}
				this.IncrementCounter();
			}
			return array;
		}
		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
