using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;
namespace Nintendo.MakeRom
{
	internal class MulticoreCryptoWorker
	{
		private byte[] m_workingMemory;
		private int m_size;
		private AesCtr m_aes;
		private Thread m_depThread;
		public event FinishAesEventHandler m_handler;
		public MulticoreCryptoWorker(int memorySize)
		{
		}
		public void SetupMemory(byte[] buffer, int size)
		{
			this.m_workingMemory = buffer;
			this.m_size = size;
		}
		public void SetupAes(byte[] key, ulong partitionID, ulong initCount)
		{
			this.m_aes = new AesCtr(key, partitionID, initCount);
		}
		public void SetupDependency(Thread depThread)
		{
			this.m_depThread = depThread;
		}
		public void DoWork()
		{
			using (MemoryStream memoryStream = new MemoryStream(this.m_workingMemory, 0, this.m_size))
			{
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, this.m_aes, CryptoStreamMode.Read))
				{
					cryptoStream.Read(this.m_workingMemory, 0, this.m_size);
				}
			}
			if (this.m_depThread != null)
			{
				this.m_depThread.Join();
				this.m_depThread = null;
			}
			if (this.m_handler != null)
			{
				this.m_handler(this, null);
			}
		}
		public void Write(Stream st)
		{
			st.Write(this.m_workingMemory, 0, this.m_size);
		}
	}
}
