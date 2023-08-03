// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerOutputStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.IO;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
    public class DerOutputStream : FilterStream
    {
        public DerOutputStream( Stream os )
          : base( os )
        {
        }

        private void WriteLength( int length )
        {
            if (length > sbyte.MaxValue)
            {
                int num1 = 1;
                uint num2 = (uint)length;
                while ((num2 >>= 8) != 0U)
                    ++num1;
                this.WriteByte( (byte)(num1 | 128) );
                for (int index = (num1 - 1) * 8; index >= 0; index -= 8)
                    this.WriteByte( (byte)(length >> index) );
            }
            else
                this.WriteByte( (byte)length );
        }

        internal void WriteEncoded( int tag, byte[] bytes )
        {
            this.WriteByte( (byte)tag );
            this.WriteLength( bytes.Length );
            this.Write( bytes, 0, bytes.Length );
        }

        internal void WriteEncoded( int tag, byte first, byte[] bytes )
        {
            this.WriteByte( (byte)tag );
            this.WriteLength( bytes.Length + 1 );
            this.WriteByte( first );
            this.Write( bytes, 0, bytes.Length );
        }

        internal void WriteEncoded( int tag, byte[] bytes, int offset, int length )
        {
            this.WriteByte( (byte)tag );
            this.WriteLength( length );
            this.Write( bytes, offset, length );
        }

        internal void WriteTag( int flags, int tagNo )
        {
            if (tagNo < 31)
            {
                this.WriteByte( (byte)(flags | tagNo) );
            }
            else
            {
                this.WriteByte( (byte)(flags | 31) );
                if (tagNo < 128)
                {
                    this.WriteByte( (byte)tagNo );
                }
                else
                {
                    byte[] buffer = new byte[5];
                    int length = buffer.Length;
                    int offset;
                    buffer[offset = length - 1] = (byte)(tagNo & sbyte.MaxValue);
                    do
                    {
                        tagNo >>= 7;
                        buffer[--offset] = (byte)((tagNo & sbyte.MaxValue) | 128);
                    }
                    while (tagNo > sbyte.MaxValue);
                    this.Write( buffer, offset, buffer.Length - offset );
                }
            }
        }

        internal void WriteEncoded( int flags, int tagNo, byte[] bytes )
        {
            this.WriteTag( flags, tagNo );
            this.WriteLength( bytes.Length );
            this.Write( bytes, 0, bytes.Length );
        }

        protected void WriteNull()
        {
            this.WriteByte( 5 );
            this.WriteByte( 0 );
        }

        [Obsolete( "Use version taking an Asn1Encodable arg instead" )]
        public virtual void WriteObject( object obj )
        {
            switch (obj)
            {
                case null:
                    this.WriteNull();
                    break;
                case Asn1Object _:
                    ((Asn1Object)obj).Encode( this );
                    break;
                case Asn1Encodable _:
                    ((Asn1Encodable)obj).ToAsn1Object().Encode( this );
                    break;
                default:
                    throw new IOException( "object not Asn1Object" );
            }
        }

        public virtual void WriteObject( Asn1Encodable obj )
        {
            if (obj == null)
                this.WriteNull();
            else
                obj.ToAsn1Object().Encode( this );
        }

        public virtual void WriteObject( Asn1Object obj )
        {
            if (obj == null)
                this.WriteNull();
            else
                obj.Encode( this );
        }
    }
}
