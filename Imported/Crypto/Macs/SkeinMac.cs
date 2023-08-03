// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Macs.SkeinMac
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Macs
{
    public class SkeinMac : IMac
    {
        public const int SKEIN_256 = 256;
        public const int SKEIN_512 = 512;
        public const int SKEIN_1024 = 1024;
        private readonly SkeinEngine engine;

        public SkeinMac( int stateSizeBits, int digestSizeBits ) => this.engine = new SkeinEngine( stateSizeBits, digestSizeBits );

        public SkeinMac( SkeinMac mac ) => this.engine = new SkeinEngine( mac.engine );

        public string AlgorithmName => "Skein-MAC-" + (this.engine.BlockSize * 8) + "-" + (this.engine.OutputSize * 8);

        public void Init( ICipherParameters parameters )
        {
            SkeinParameters parameters1;
            switch (parameters)
            {
                case SkeinParameters _:
                    parameters1 = (SkeinParameters)parameters;
                    break;
                case KeyParameter _:
                    parameters1 = new SkeinParameters.Builder().SetKey( ((KeyParameter)parameters).GetKey() ).Build();
                    break;
                default:
                    throw new ArgumentException( "Invalid parameter passed to Skein MAC init - " + Platform.GetTypeName( parameters ) );
            }
            if (parameters1.GetKey() == null)
                throw new ArgumentException( "Skein MAC requires a key parameter." );
            this.engine.Init( parameters1 );
        }

        public int GetMacSize() => this.engine.OutputSize;

        public void Reset() => this.engine.Reset();

        public void Update( byte inByte ) => this.engine.Update( inByte );

        public void BlockUpdate( byte[] input, int inOff, int len ) => this.engine.Update( input, inOff, len );

        public int DoFinal( byte[] output, int outOff ) => this.engine.DoFinal( output, outOff );
    }
}
