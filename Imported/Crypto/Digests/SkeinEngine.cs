// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Digests.SkeinEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Crypto.Digests
{
    public class SkeinEngine : IMemoable
    {
        public const int SKEIN_256 = 256;
        public const int SKEIN_512 = 512;
        public const int SKEIN_1024 = 1024;
        private const int PARAM_TYPE_KEY = 0;
        private const int PARAM_TYPE_CONFIG = 4;
        private const int PARAM_TYPE_MESSAGE = 48;
        private const int PARAM_TYPE_OUTPUT = 63;
        private static readonly IDictionary INITIAL_STATES = Platform.CreateHashtable();
        private readonly ThreefishEngine threefish;
        private readonly int outputSizeBytes;
        private ulong[] chain;
        private ulong[] initialState;
        private byte[] key;
        private SkeinEngine.Parameter[] preMessageParameters;
        private SkeinEngine.Parameter[] postMessageParameters;
        private readonly SkeinEngine.UBI ubi;
        private readonly byte[] singleByte = new byte[1];

        static SkeinEngine()
        {
            InitialState( 256, 128, new ulong[4]
      {
        16217771249220022880UL,
        9817190399063458076UL,
        1155188648486244218UL,
        14769517481627992514UL
      } );
            InitialState( 256, 160, new ulong[4]
      {
        1450197650740764312UL,
        3081844928540042640UL,
        15310647011875280446UL,
        3301952811952417661UL
      } );
            InitialState( 256, 224, new ulong[4]
      {
        14270089230798940683UL,
        9758551101254474012UL,
        11082101768697755780UL,
        4056579644589979102UL
      } );
            InitialState( 256, 256, new ulong[4]
      {
        18202890402666165321UL,
        3443677322885453875UL,
        12915131351309911055UL,
        7662005193972177513UL
      } );
            InitialState( 512, 128, new ulong[8]
      {
        12158729379475595090UL,
        2204638249859346602UL,
        3502419045458743507UL,
        13617680570268287068UL,
        983504137758028059UL,
        1880512238245786339UL,
        11730851291495443074UL,
        7602827311880509485UL
      } );
            InitialState( 512, 160, new ulong[8]
      {
        2934123928682216849UL,
        14047033351726823311UL,
        1684584802963255058UL,
        5744138295201861711UL,
        2444857010922934358UL,
        15638910433986703544UL,
        13325156239043941114UL,
        118355523173251694UL
      } );
            InitialState( 512, 224, new ulong[8]
      {
        14758403053642543652UL,
        14674518637417806319UL,
        10145881904771976036UL,
        4146387520469897396UL,
        1106145742801415120UL,
        7455425944880474941UL,
        11095680972475339753UL,
        11397762726744039159UL
      } );
            InitialState( 512, 384, new ulong[8]
      {
        11814849197074935647UL,
        12753905853581818532UL,
        11346781217370868990UL,
        15535391162178797018UL,
        2000907093792408677UL,
        9140007292425499655UL,
        6093301768906360022UL,
        2769176472213098488UL
      } );
            InitialState( 512, 512, new ulong[8]
      {
        5261240102383538638UL,
        978932832955457283UL,
        10363226125605772238UL,
        11107378794354519217UL,
        6752626034097301424UL,
        16915020251879818228UL,
        11029617608758768931UL,
        12544957130904423475UL
      } );
        }

        private static void InitialState( int blockSize, int outputSize, ulong[] state ) => INITIAL_STATES.Add( VariantIdentifier( blockSize / 8, outputSize / 8 ), state );

        private static int VariantIdentifier( int blockSizeBytes, int outputSizeBytes ) => (outputSizeBytes << 16) | blockSizeBytes;

        public SkeinEngine( int blockSizeBits, int outputSizeBits )
        {
            if (outputSizeBits % 8 != 0)
                throw new ArgumentException( "Output size must be a multiple of 8 bits. :" + outputSizeBits );
            this.outputSizeBytes = outputSizeBits / 8;
            this.threefish = new ThreefishEngine( blockSizeBits );
            this.ubi = new SkeinEngine.UBI( this, this.threefish.GetBlockSize() );
        }

        public SkeinEngine( SkeinEngine engine )
          : this( engine.BlockSize * 8, engine.OutputSize * 8 )
        {
            this.CopyIn( engine );
        }

        private void CopyIn( SkeinEngine engine )
        {
            this.ubi.Reset( engine.ubi );
            this.chain = Arrays.Clone( engine.chain, this.chain );
            this.initialState = Arrays.Clone( engine.initialState, this.initialState );
            this.key = Arrays.Clone( engine.key, this.key );
            this.preMessageParameters = Clone( engine.preMessageParameters, this.preMessageParameters );
            this.postMessageParameters = Clone( engine.postMessageParameters, this.postMessageParameters );
        }

        private static SkeinEngine.Parameter[] Clone(
          SkeinEngine.Parameter[] data,
          SkeinEngine.Parameter[] existing )
        {
            if (data == null)
                return null;
            if (existing == null || existing.Length != data.Length)
                existing = new SkeinEngine.Parameter[data.Length];
            Array.Copy( data, 0, existing, 0, existing.Length );
            return existing;
        }

        public IMemoable Copy() => new SkeinEngine( this );

        public void Reset( IMemoable other )
        {
            SkeinEngine engine = (SkeinEngine)other;
            if (this.BlockSize != engine.BlockSize || this.outputSizeBytes != engine.outputSizeBytes)
                throw new MemoableResetException( "Incompatible parameters in provided SkeinEngine." );
            this.CopyIn( engine );
        }

        public int OutputSize => this.outputSizeBytes;

        public int BlockSize => this.threefish.GetBlockSize();

        public void Init( SkeinParameters parameters )
        {
            this.chain = null;
            this.key = null;
            this.preMessageParameters = null;
            this.postMessageParameters = null;
            if (parameters != null)
            {
                if (parameters.GetKey().Length < 16)
                    throw new ArgumentException( "Skein key must be at least 128 bits." );
                this.InitParams( parameters.GetParameters() );
            }
            this.CreateInitialState();
            this.UbiInit( 48 );
        }

        private void InitParams( IDictionary parameters )
        {
            IEnumerator enumerator = parameters.Keys.GetEnumerator();
            IList arrayList1 = Platform.CreateArrayList();
            IList arrayList2 = Platform.CreateArrayList();
            while (enumerator.MoveNext())
            {
                int current = (int)enumerator.Current;
                byte[] parameter = (byte[])parameters[current];
                if (current == 0)
                    this.key = parameter;
                else if (current < 48)
                    arrayList1.Add( new SkeinEngine.Parameter( current, parameter ) );
                else
                    arrayList2.Add( new SkeinEngine.Parameter( current, parameter ) );
            }
            this.preMessageParameters = new SkeinEngine.Parameter[arrayList1.Count];
            arrayList1.CopyTo( preMessageParameters, 0 );
            Array.Sort( (Array)this.preMessageParameters );
            this.postMessageParameters = new SkeinEngine.Parameter[arrayList2.Count];
            arrayList2.CopyTo( postMessageParameters, 0 );
            Array.Sort( (Array)this.postMessageParameters );
        }

        private void CreateInitialState()
        {
            ulong[] data = (ulong[])INITIAL_STATES[VariantIdentifier( this.BlockSize, this.OutputSize )];
            if (this.key == null && data != null)
            {
                this.chain = Arrays.Clone( data );
            }
            else
            {
                this.chain = new ulong[this.BlockSize / 8];
                if (this.key != null)
                    this.UbiComplete( 0, this.key );
                this.UbiComplete( 4, new SkeinEngine.Configuration( this.outputSizeBytes * 8 ).Bytes );
            }
            if (this.preMessageParameters != null)
            {
                for (int index = 0; index < this.preMessageParameters.Length; ++index)
                {
                    SkeinEngine.Parameter messageParameter = this.preMessageParameters[index];
                    this.UbiComplete( messageParameter.Type, messageParameter.Value );
                }
            }
            this.initialState = Arrays.Clone( this.chain );
        }

        public void Reset()
        {
            Array.Copy( initialState, 0, chain, 0, this.chain.Length );
            this.UbiInit( 48 );
        }

        private void UbiComplete( int type, byte[] value )
        {
            this.UbiInit( type );
            this.ubi.Update( value, 0, value.Length, this.chain );
            this.UbiFinal();
        }

        private void UbiInit( int type ) => this.ubi.Reset( type );

        private void UbiFinal() => this.ubi.DoFinal( this.chain );

        private void CheckInitialised()
        {
            if (this.ubi == null)
                throw new ArgumentException( "Skein engine is not initialised." );
        }

        public void Update( byte inByte )
        {
            this.singleByte[0] = inByte;
            this.Update( this.singleByte, 0, 1 );
        }

        public void Update( byte[] inBytes, int inOff, int len )
        {
            this.CheckInitialised();
            this.ubi.Update( inBytes, inOff, len, this.chain );
        }

        public int DoFinal( byte[] outBytes, int outOff )
        {
            this.CheckInitialised();
            if (outBytes.Length < outOff + this.outputSizeBytes)
                throw new DataLengthException( "Output buffer is too short to hold output" );
            this.UbiFinal();
            if (this.postMessageParameters != null)
            {
                for (int index = 0; index < this.postMessageParameters.Length; ++index)
                {
                    SkeinEngine.Parameter messageParameter = this.postMessageParameters[index];
                    this.UbiComplete( messageParameter.Type, messageParameter.Value );
                }
            }
            int blockSize = this.BlockSize;
            int num = (this.outputSizeBytes + blockSize - 1) / blockSize;
            for (int outputSequence = 0; outputSequence < num; ++outputSequence)
            {
                int outputBytes = System.Math.Min( blockSize, this.outputSizeBytes - (outputSequence * blockSize) );
                this.Output( (ulong)outputSequence, outBytes, outOff + (outputSequence * blockSize), outputBytes );
            }
            this.Reset();
            return this.outputSizeBytes;
        }

        private void Output( ulong outputSequence, byte[] outBytes, int outOff, int outputBytes )
        {
            byte[] numArray = new byte[8];
            ThreefishEngine.WordToBytes( outputSequence, numArray, 0 );
            ulong[] output = new ulong[this.chain.Length];
            this.UbiInit( 63 );
            this.ubi.Update( numArray, 0, numArray.Length, output );
            this.ubi.DoFinal( output );
            int num = (outputBytes + 8 - 1) / 8;
            for (int index = 0; index < num; ++index)
            {
                int length = System.Math.Min( 8, outputBytes - (index * 8) );
                if (length == 8)
                {
                    ThreefishEngine.WordToBytes( output[index], outBytes, outOff + (index * 8) );
                }
                else
                {
                    ThreefishEngine.WordToBytes( output[index], numArray, 0 );
                    Array.Copy( numArray, 0, outBytes, outOff + (index * 8), length );
                }
            }
        }

        private class Configuration
        {
            private byte[] bytes = new byte[32];

            public Configuration( long outputSizeBits )
            {
                this.bytes[0] = 83;
                this.bytes[1] = 72;
                this.bytes[2] = 65;
                this.bytes[3] = 51;
                this.bytes[4] = 1;
                this.bytes[5] = 0;
                ThreefishEngine.WordToBytes( (ulong)outputSizeBits, this.bytes, 8 );
            }

            public byte[] Bytes => this.bytes;
        }

        public class Parameter
        {
            private int type;
            private byte[] value;

            public Parameter( int type, byte[] value )
            {
                this.type = type;
                this.value = value;
            }

            public int Type => this.type;

            public byte[] Value => this.value;
        }

        private class UbiTweak
        {
            private const ulong LOW_RANGE = 18446744069414584320;
            private const ulong T1_FINAL = 9223372036854775808;
            private const ulong T1_FIRST = 4611686018427387904;
            private ulong[] tweak = new ulong[2];
            private bool extendedPosition;

            public UbiTweak() => this.Reset();

            public void Reset( SkeinEngine.UbiTweak tweak )
            {
                this.tweak = Arrays.Clone( tweak.tweak, this.tweak );
                this.extendedPosition = tweak.extendedPosition;
            }

            public void Reset()
            {
                this.tweak[0] = 0UL;
                this.tweak[1] = 0UL;
                this.extendedPosition = false;
                this.First = true;
            }

            public uint Type
            {
                get => (uint)((this.tweak[1] >> 56) & 63UL);
                set => this.tweak[1] = (ulong)(((long)this.tweak[1] & -274877906944L) | ((value & 63L) << 56));
            }

            public bool First
            {
                get => ((long)this.tweak[1] & 4611686018427387904L) != 0L;
                set
                {
                    if (value)
                    {
                        ulong[] tweak;
                        (tweak = this.tweak)[1] = tweak[1] | 4611686018427387904UL;
                    }
                    else
                    {
                        ulong[] tweak;
                        (tweak = this.tweak)[1] = tweak[1] & 13835058055282163711UL;
                    }
                }
            }

            public bool Final
            {
                get => ((long)this.tweak[1] & long.MinValue) != 0L;
                set
                {
                    if (value)
                    {
                        ulong[] tweak;
                        (tweak = this.tweak)[1] = tweak[1] | 9223372036854775808UL;
                    }
                    else
                    {
                        ulong[] tweak;
                        (tweak = this.tweak)[1] = tweak[1] & long.MaxValue;
                    }
                }
            }

            public void AdvancePosition( int advance )
            {
                if (this.extendedPosition)
                {
                    ulong[] numArray = new ulong[3]
                    {
            this.tweak[0] &  uint.MaxValue,
            (this.tweak[0] >> 32) &  uint.MaxValue,
            this.tweak[1] &  uint.MaxValue
                    };
                    ulong num1 = (ulong)advance;
                    for (int index = 0; index < numArray.Length; ++index)
                    {
                        ulong num2 = num1 + numArray[index];
                        numArray[index] = num2;
                        num1 = num2 >> 32;
                    }
                    this.tweak[0] = (ulong)((((long)numArray[1] & uint.MaxValue) << 32) | ((long)numArray[0] & uint.MaxValue));
                    this.tweak[1] = (ulong)(((long)this.tweak[1] & -4294967296L) | ((long)numArray[2] & uint.MaxValue));
                }
                else
                {
                    ulong num = this.tweak[0] + (uint)advance;
                    this.tweak[0] = num;
                    if (num <= 18446744069414584320UL)
                        return;
                    this.extendedPosition = true;
                }
            }

            public ulong[] GetWords() => this.tweak;

            public override string ToString() => this.Type.ToString() + " first: " + First + ", final: " + Final;
        }

        private class UBI
        {
            private readonly SkeinEngine.UbiTweak tweak = new();
            private readonly SkeinEngine engine;
            private byte[] currentBlock;
            private int currentOffset;
            private ulong[] message;

            public UBI( SkeinEngine engine, int blockSize )
            {
                this.engine = engine;
                this.currentBlock = new byte[blockSize];
                this.message = new ulong[this.currentBlock.Length / 8];
            }

            public void Reset( SkeinEngine.UBI ubi )
            {
                this.currentBlock = Arrays.Clone( ubi.currentBlock, this.currentBlock );
                this.currentOffset = ubi.currentOffset;
                this.message = Arrays.Clone( ubi.message, this.message );
                this.tweak.Reset( ubi.tweak );
            }

            public void Reset( int type )
            {
                this.tweak.Reset();
                this.tweak.Type = (uint)type;
                this.currentOffset = 0;
            }

            public void Update( byte[] value, int offset, int len, ulong[] output )
            {
                int num1 = 0;
                while (len > num1)
                {
                    if (this.currentOffset == this.currentBlock.Length)
                    {
                        this.ProcessBlock( output );
                        this.tweak.First = false;
                        this.currentOffset = 0;
                    }
                    int num2 = System.Math.Min( len - num1, this.currentBlock.Length - this.currentOffset );
                    Array.Copy( value, offset + num1, currentBlock, this.currentOffset, num2 );
                    num1 += num2;
                    this.currentOffset += num2;
                    this.tweak.AdvancePosition( num2 );
                }
            }

            private void ProcessBlock( ulong[] output )
            {
                this.engine.threefish.Init( true, this.engine.chain, this.tweak.GetWords() );
                for (int index = 0; index < this.message.Length; ++index)
                    this.message[index] = ThreefishEngine.BytesToWord( this.currentBlock, index * 8 );
                this.engine.threefish.ProcessBlock( this.message, output );
                for (int index = 0; index < output.Length; ++index)
                    output[index] ^= this.message[index];
            }

            public void DoFinal( ulong[] output )
            {
                for (int currentOffset = this.currentOffset; currentOffset < this.currentBlock.Length; ++currentOffset)
                    this.currentBlock[currentOffset] = 0;
                this.tweak.Final = true;
                this.ProcessBlock( output );
            }
        }
    }
}
