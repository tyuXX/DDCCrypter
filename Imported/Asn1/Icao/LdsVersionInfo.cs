// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Icao.LdsVersionInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Icao
{
    public class LdsVersionInfo : Asn1Encodable
    {
        private DerPrintableString ldsVersion;
        private DerPrintableString unicodeVersion;

        public LdsVersionInfo( string ldsVersion, string unicodeVersion )
        {
            this.ldsVersion = new DerPrintableString( ldsVersion );
            this.unicodeVersion = new DerPrintableString( unicodeVersion );
        }

        private LdsVersionInfo( Asn1Sequence seq )
        {
            this.ldsVersion = seq.Count == 2 ? DerPrintableString.GetInstance( seq[0] ) : throw new ArgumentException( "sequence wrong size for LDSVersionInfo", nameof( seq ) );
            this.unicodeVersion = DerPrintableString.GetInstance( seq[1] );
        }

        public static LdsVersionInfo GetInstance( object obj )
        {
            if (obj is LdsVersionInfo)
                return (LdsVersionInfo)obj;
            return obj != null ? new LdsVersionInfo( Asn1Sequence.GetInstance( obj ) ) : null;
        }

        public virtual string GetLdsVersion() => this.ldsVersion.GetString();

        public virtual string GetUnicodeVersion() => this.unicodeVersion.GetString();

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       ldsVersion,
       unicodeVersion
        } );
    }
}
