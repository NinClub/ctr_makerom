using System;
using System.Security.Cryptography;
namespace Nintendo.MakeRom
{
	public class AesCcm
	{
		private byte[] Key
		{
			get;
			set;
		}
		private byte[] Nonce
		{
			get;
			set;
		}
		private int L
		{
			get
			{
				return 15 - this.Nonce.Length;
			}
		}
		private int Ld
		{
			get
			{
				return this.L - 1;
			}
		}
		private int M
		{
			get
			{
				return 16;
			}
		}
		private int Md
		{
			get
			{
				return (this.M - 2) / 2;
			}
		}
		public AesCcm(byte[] key, byte[] nonce)
		{
			this.Initialize(key, nonce);
		}
		private void Initialize(byte[] key, byte[] nonce)
		{
			if (key.Length != 16)
			{
				throw new ArgumentException("Invalid key length");
			}
			if (nonce.Length != 12)
			{
				throw new ArgumentException("Invalid nonce length");
			}
			this.Key = key;
			this.Nonce = nonce;
		}
		private byte[] GetTag(byte[] plainData)
		{
			int num = plainData.Length;
			int num2 = num + 16;
			byte[] array = new byte[num2];
			byte[] array2 = new byte[num2];
			byte[] array3 = new byte[16];
			Array.Copy(this.MakeFirstBlock(plainData.Length), 0, array, 0, 16);
			Array.Copy(plainData, 0, array, 16, num);
			new RijndaelManaged
			{
				Mode = CipherMode.CBC,
				IV = new byte[16],
				Key = this.Key
			}.CreateEncryptor().TransformBlock(array, 0, array.Length, array2, 0);
			Array.Copy(array2, array2.Length - 16, array3, 0, 16);
			return array3;
		}
		private void TransformAesCtr(byte[] output, byte[] input)
		{
			byte[] iv = this.MakeCounter(0, this.Nonce);
			AesCtr aesCtr = new AesCtr(this.Key, iv);
			aesCtr.TransformBlock(input, 0, input.Length, output, 0);
		}
		private void Transform(byte[] outData, byte[] outFirstBlock, byte[] inputData, byte[] inputFirstBlock)
		{
			int num = inputData.Length;
			int num2 = num + 16;
			byte[] array = new byte[num2];
			byte[] array2 = new byte[num2];
			Array.Copy(inputFirstBlock, 0, array, 0, 16);
			Array.Copy(inputData, 0, array, 16, num);
			this.TransformAesCtr(array2, array);
			Array.Copy(array2, 0, outFirstBlock, 0, 16);
			Array.Copy(array2, 16, outData, 0, array2.Length - 16);
		}
		public void EncryptAndSign(byte[] dest, byte[] mac, byte[] plainData)
		{
			if (plainData.Length % 16 != 0)
			{
				throw new ArgumentException("Plain data length must be 16 byte align");
			}
			if (mac.Length != 16)
			{
				throw new ArgumentException("Invalid mac length");
			}
			if (dest.Length != plainData.Length)
			{
				throw new ArgumentException("Output and input array length must be same");
			}
			byte[] tag = this.GetTag(plainData);
			this.Transform(dest, mac, plainData, tag);
		}
		public bool DecryptAndVerify(byte[] dest, byte[] encryptedData, byte[] mac)
		{
			if (encryptedData.Length % 16 != 0)
			{
				throw new ArgumentException("Plain data length must be 16 byte align");
			}
			if (mac.Length != 16)
			{
				throw new ArgumentException("Invalid mac length");
			}
			if (dest.Length != encryptedData.Length)
			{
				throw new ArgumentException("Output and input array length must be same");
			}
			byte[] array = new byte[16];
			this.Transform(dest, array, encryptedData, mac);
			byte[] tag = this.GetTag(dest);
			for (int i = 0; i < array.Length; i++)
			{
				if (tag[i] != array[i])
				{
					return false;
				}
			}
			return true;
		}
		private byte MakeCounterFlag()
		{
			return (byte)this.Ld;
		}
		private byte[] MakeCounter(int n, byte[] nonce)
		{
			byte[] array = new byte[16];
			array[0] = this.MakeCounterFlag();
			Array.Copy(nonce, 0, array, 1, nonce.Length);
			byte[] bytes = BitConverter.GetBytes(n);
			array[13] = bytes[2];
			array[14] = bytes[1];
			array[15] = bytes[0];
			return array;
		}
		private byte MakeFirstBlockFlag()
		{
			uint num = 0u;
			return (byte)((ulong)(64u * num) + (ulong)((long)(8 * this.Md)) + (ulong)((long)this.Ld));
		}
		private byte[] MakeFirstBlock(int messageLength)
		{
			byte[] array = new byte[16];
			array[0] = this.MakeFirstBlockFlag();
			Array.Copy(this.Nonce, 0, array, 1, this.Nonce.Length);
			byte[] bytes = BitConverter.GetBytes(messageLength);
			array[13] = bytes[2];
			array[14] = bytes[1];
			array[15] = bytes[0];
			return array;
		}
	}
}
