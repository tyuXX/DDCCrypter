// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Prng.ThreadedSeedGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.Threading;

namespace Org.BouncyCastle.Crypto.Prng
{
    public class ThreadedSeedGenerator
    {
        public byte[] GenerateSeed( int numBytes, bool fast ) => new ThreadedSeedGenerator.SeedGenerator().GenerateSeed( numBytes, fast );

        private class SeedGenerator
        {
            private volatile int counter = 0;
            private volatile bool stop = false;

            private void Run( object ignored )
            {
                while (!this.stop)
                    ++this.counter;
            }

            public byte[] GenerateSeed( int numBytes, bool fast )
            {
                ThreadPriority priority = Thread.CurrentThread.Priority;
                try
                {
                    Thread.CurrentThread.Priority = ThreadPriority.Normal;
                    return this.DoGenerateSeed( numBytes, fast );
                }
                finally
                {
                    Thread.CurrentThread.Priority = priority;
                }
            }

            private byte[] DoGenerateSeed( int numBytes, bool fast )
            {
                this.counter = 0;
                this.stop = false;
                byte[] seed = new byte[numBytes];
                int num1 = 0;
                int num2 = fast ? numBytes : numBytes * 8;
                ThreadPool.QueueUserWorkItem( new WaitCallback( this.Run ) );
                for (int index1 = 0; index1 < num2; ++index1)
                {
                    while (this.counter == num1)
                    {
                        try
                        {
                            Thread.Sleep( 1 );
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    num1 = this.counter;
                    if (fast)
                    {
                        seed[index1] = (byte)num1;
                    }
                    else
                    {
                        int index2 = index1 / 8;
                        seed[index2] = (byte)((seed[index2] << 1) | (num1 & 1));
                    }
                }
                this.stop = true;
                return seed;
            }
        }
    }
}
