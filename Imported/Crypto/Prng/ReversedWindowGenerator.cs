// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Prng.ReversedWindowGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Crypto.Prng
{
    public class ReversedWindowGenerator : IRandomGenerator
    {
        private readonly IRandomGenerator generator;
        private byte[] window;
        private int windowCount;

        public ReversedWindowGenerator( IRandomGenerator generator, int windowSize )
        {
            if (generator == null)
                throw new ArgumentNullException( nameof( generator ) );
            if (windowSize < 2)
                throw new ArgumentException( "Window size must be at least 2", nameof( windowSize ) );
            this.generator = generator;
            this.window = new byte[windowSize];
        }

        public virtual void AddSeedMaterial( byte[] seed )
        {
            lock (this)
            {
                this.windowCount = 0;
                this.generator.AddSeedMaterial( seed );
            }
        }

        public virtual void AddSeedMaterial( long seed )
        {
            lock (this)
            {
                this.windowCount = 0;
                this.generator.AddSeedMaterial( seed );
            }
        }

        public virtual void NextBytes( byte[] bytes ) => this.doNextBytes( bytes, 0, bytes.Length );

        public virtual void NextBytes( byte[] bytes, int start, int len ) => this.doNextBytes( bytes, start, len );

        private void doNextBytes( byte[] bytes, int start, int len )
        {
            lock (this)
            {
                for (int index = 0; index < len; bytes[start + index++] = this.window[--this.windowCount])
                {
                    if (this.windowCount < 1)
                    {
                        this.generator.NextBytes( this.window, 0, this.window.Length );
                        this.windowCount = this.window.Length;
                    }
                }
            }
        }
    }
}
