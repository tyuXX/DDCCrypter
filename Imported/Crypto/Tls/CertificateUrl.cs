// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.CertificateUrl
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class CertificateUrl
    {
        protected readonly byte mType;
        protected readonly IList mUrlAndHashList;

        public CertificateUrl( byte type, IList urlAndHashList )
        {
            if (!CertChainType.IsValid( type ))
                throw new ArgumentException( "not a valid CertChainType value", nameof( type ) );
            if (urlAndHashList == null || urlAndHashList.Count < 1)
                throw new ArgumentException( "must have length > 0", nameof( urlAndHashList ) );
            this.mType = type;
            this.mUrlAndHashList = urlAndHashList;
        }

        public virtual byte Type => this.mType;

        public virtual IList UrlAndHashList => this.mUrlAndHashList;

        public virtual void Encode( Stream output )
        {
            TlsUtilities.WriteUint8( this.mType, output );
            CertificateUrl.ListBuffer16 output1 = new CertificateUrl.ListBuffer16();
            foreach (UrlAndHash mUrlAndHash in (IEnumerable)this.mUrlAndHashList)
                mUrlAndHash.Encode( output1 );
            output1.EncodeTo( output );
        }

        public static CertificateUrl parse( TlsContext context, Stream input )
        {
            byte num = TlsUtilities.ReadUint8( input );
            if (!CertChainType.IsValid( num ))
                throw new TlsFatalAlert( 50 );
            int length = TlsUtilities.ReadUint16( input );
            MemoryStream input1 = length >= 1 ? new MemoryStream( TlsUtilities.ReadFully( length, input ), false ) : throw new TlsFatalAlert( 50 );
            IList arrayList = Platform.CreateArrayList();
            while (input1.Position < input1.Length)
            {
                UrlAndHash urlAndHash = UrlAndHash.Parse( context, input1 );
                arrayList.Add( urlAndHash );
            }
            return new CertificateUrl( num, arrayList );
        }

        internal class ListBuffer16 : MemoryStream
        {
            internal ListBuffer16() => TlsUtilities.WriteUint16( 0, this );

            internal void EncodeTo( Stream output )
            {
                long i = this.Length - 2L;
                TlsUtilities.CheckUint16( i );
                this.Position = 0L;
                TlsUtilities.WriteUint16( (int)i, this );
                this.WriteTo( output );
                Platform.Dispose( this );
            }
        }
    }
}
