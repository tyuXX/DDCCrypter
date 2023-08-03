// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.IO.Pem.PemWriter
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.Encoders;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Utilities.IO.Pem
{
    public class PemWriter
    {
        private const int LineLength = 64;
        private readonly TextWriter writer;
        private readonly int nlLength;
        private char[] buf = new char[64];

        public PemWriter( TextWriter writer )
        {
            this.writer = writer != null ? writer : throw new ArgumentNullException( nameof( writer ) );
            this.nlLength = Platform.NewLine.Length;
        }

        public TextWriter Writer => this.writer;

        public int GetOutputSize( PemObject obj )
        {
            int num1 = (2 * (obj.Type.Length + 10 + this.nlLength)) + 6 + 4;
            if (obj.Headers.Count > 0)
            {
                foreach (PemHeader header in (IEnumerable)obj.Headers)
                    num1 += header.Name.Length + ": ".Length + header.Value.Length + this.nlLength;
                num1 += this.nlLength;
            }
            int num2 = (obj.Content.Length + 2) / 3 * 4;
            return num1 + num2 + ((num2 + 64 - 1) / 64 * this.nlLength);
        }

        public void WriteObject( PemObjectGenerator objGen )
        {
            PemObject pemObject = objGen.Generate();
            this.WritePreEncapsulationBoundary( pemObject.Type );
            if (pemObject.Headers.Count > 0)
            {
                foreach (PemHeader header in (IEnumerable)pemObject.Headers)
                {
                    this.writer.Write( header.Name );
                    this.writer.Write( ": " );
                    this.writer.WriteLine( header.Value );
                }
                this.writer.WriteLine();
            }
            this.WriteEncoded( pemObject.Content );
            this.WritePostEncapsulationBoundary( pemObject.Type );
        }

        private void WriteEncoded( byte[] bytes )
        {
            bytes = Base64.Encode( bytes );
            for (int index = 0; index < bytes.Length; index += this.buf.Length)
            {
                int count;
                for (count = 0; count != this.buf.Length && index + count < bytes.Length; ++count)
                    this.buf[count] = (char)bytes[index + count];
                this.writer.WriteLine( this.buf, 0, count );
            }
        }

        private void WritePreEncapsulationBoundary( string type ) => this.writer.WriteLine( "-----BEGIN " + type + "-----" );

        private void WritePostEncapsulationBoundary( string type ) => this.writer.WriteLine( "-----END " + type + "-----" );
    }
}
