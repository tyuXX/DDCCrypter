// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Tsp.GenTimeAccuracy
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Tsp;

namespace Org.BouncyCastle.Tsp
{
    public class GenTimeAccuracy
    {
        private Accuracy accuracy;

        public GenTimeAccuracy( Accuracy accuracy ) => this.accuracy = accuracy;

        public int Seconds => this.GetTimeComponent( this.accuracy.Seconds );

        public int Millis => this.GetTimeComponent( this.accuracy.Millis );

        public int Micros => this.GetTimeComponent( this.accuracy.Micros );

        private int GetTimeComponent( DerInteger time ) => time != null ? time.Value.IntValue : 0;

        public override string ToString() => this.Seconds.ToString() + "." + this.Millis.ToString( "000" ) + this.Micros.ToString( "000" );
    }
}
