// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Utilities.Asn1Dump
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections;
using System.IO;
using System.Text;

namespace Org.BouncyCastle.Asn1.Utilities
{
    public sealed class Asn1Dump
    {
        private const string Tab = "    ";
        private const int SampleSize = 32;
        private static readonly string NewLine = Platform.NewLine;

        private Asn1Dump()
        {
        }

        private static void AsString( string indent, bool verbose, Asn1Object obj, StringBuilder buf )
        {
            switch (obj)
            {
                case Asn1Sequence _:
                    string indent1 = indent + "    ";
                    buf.Append( indent );
                    switch (obj)
                    {
                        case BerSequence _:
                            buf.Append( "BER Sequence" );
                            break;
                        case DerSequence _:
                            buf.Append( "DER Sequence" );
                            break;
                        default:
                            buf.Append( "Sequence" );
                            break;
                    }
                    buf.Append( NewLine );
                    IEnumerator enumerator1 = ((Asn1Sequence)obj).GetEnumerator();
                    try
                    {
                        while (enumerator1.MoveNext())
                        {
                            Asn1Encodable current = (Asn1Encodable)enumerator1.Current;
                            switch (current)
                            {
                                case null:
                                case Asn1Null _:
                                    buf.Append( indent1 );
                                    buf.Append( "NULL" );
                                    buf.Append( NewLine );
                                    continue;
                                default:
                                    AsString( indent1, verbose, current.ToAsn1Object(), buf );
                                    continue;
                            }
                        }
                        break;
                    }
                    finally
                    {
                        if (enumerator1 is IDisposable disposable)
                            disposable.Dispose();
                    }
                case DerTaggedObject _:
                    string indent2 = indent + "    ";
                    buf.Append( indent );
                    if (obj is BerTaggedObject)
                        buf.Append( "BER Tagged [" );
                    else
                        buf.Append( "Tagged [" );
                    DerTaggedObject derTaggedObject = (DerTaggedObject)obj;
                    buf.Append( derTaggedObject.TagNo.ToString() );
                    buf.Append( ']' );
                    if (!derTaggedObject.IsExplicit())
                        buf.Append( " IMPLICIT " );
                    buf.Append( NewLine );
                    if (derTaggedObject.IsEmpty())
                    {
                        buf.Append( indent2 );
                        buf.Append( "EMPTY" );
                        buf.Append( NewLine );
                        break;
                    }
                    AsString( indent2, verbose, derTaggedObject.GetObject(), buf );
                    break;
                case BerSet _:
                    string indent3 = indent + "    ";
                    buf.Append( indent );
                    buf.Append( "BER Set" );
                    buf.Append( NewLine );
                    IEnumerator enumerator2 = ((Asn1Set)obj).GetEnumerator();
                    try
                    {
                        while (enumerator2.MoveNext())
                        {
                            Asn1Encodable current = (Asn1Encodable)enumerator2.Current;
                            if (current == null)
                            {
                                buf.Append( indent3 );
                                buf.Append( "NULL" );
                                buf.Append( NewLine );
                            }
                            else
                                AsString( indent3, verbose, current.ToAsn1Object(), buf );
                        }
                        break;
                    }
                    finally
                    {
                        if (enumerator2 is IDisposable disposable)
                            disposable.Dispose();
                    }
                case DerSet _:
                    string indent4 = indent + "    ";
                    buf.Append( indent );
                    buf.Append( "DER Set" );
                    buf.Append( NewLine );
                    IEnumerator enumerator3 = ((Asn1Set)obj).GetEnumerator();
                    try
                    {
                        while (enumerator3.MoveNext())
                        {
                            Asn1Encodable current = (Asn1Encodable)enumerator3.Current;
                            if (current == null)
                            {
                                buf.Append( indent4 );
                                buf.Append( "NULL" );
                                buf.Append( NewLine );
                            }
                            else
                                AsString( indent4, verbose, current.ToAsn1Object(), buf );
                        }
                        break;
                    }
                    finally
                    {
                        if (enumerator3 is IDisposable disposable)
                            disposable.Dispose();
                    }
                case DerObjectIdentifier _:
                    buf.Append( indent + "ObjectIdentifier(" + ((DerObjectIdentifier)obj).Id + ")" + NewLine );
                    break;
                case DerBoolean _:
                    buf.Append( indent + "Boolean(" + ((DerBoolean)obj).IsTrue + ")" + NewLine );
                    break;
                case DerInteger _:
                    buf.Append( indent + "Integer(" + ((DerInteger)obj).Value + ")" + NewLine );
                    break;
                case BerOctetString _:
                    byte[] octets1 = ((Asn1OctetString)obj).GetOctets();
                    string str1 = verbose ? dumpBinaryDataAsString( indent, octets1 ) : "";
                    buf.Append( indent + "BER Octet String[" + octets1.Length + "] " + str1 + NewLine );
                    break;
                case DerOctetString _:
                    byte[] octets2 = ((Asn1OctetString)obj).GetOctets();
                    string str2 = verbose ? dumpBinaryDataAsString( indent, octets2 ) : "";
                    buf.Append( indent + "DER Octet String[" + octets2.Length + "] " + str2 + NewLine );
                    break;
                case DerBitString _:
                    DerBitString derBitString = (DerBitString)obj;
                    byte[] bytes = derBitString.GetBytes();
                    string str3 = verbose ? dumpBinaryDataAsString( indent, bytes ) : "";
                    buf.Append( indent + "DER Bit String[" + bytes.Length + ", " + derBitString.PadBits + "] " + str3 + NewLine );
                    break;
                case DerIA5String _:
                    buf.Append( indent + "IA5String(" + ((DerStringBase)obj).GetString() + ") " + NewLine );
                    break;
                case DerUtf8String _:
                    buf.Append( indent + "UTF8String(" + ((DerStringBase)obj).GetString() + ") " + NewLine );
                    break;
                case DerPrintableString _:
                    buf.Append( indent + "PrintableString(" + ((DerStringBase)obj).GetString() + ") " + NewLine );
                    break;
                case DerVisibleString _:
                    buf.Append( indent + "VisibleString(" + ((DerStringBase)obj).GetString() + ") " + NewLine );
                    break;
                case DerBmpString _:
                    buf.Append( indent + "BMPString(" + ((DerStringBase)obj).GetString() + ") " + NewLine );
                    break;
                case DerT61String _:
                    buf.Append( indent + "T61String(" + ((DerStringBase)obj).GetString() + ") " + NewLine );
                    break;
                case DerGraphicString _:
                    buf.Append( indent + "GraphicString(" + ((DerStringBase)obj).GetString() + ") " + NewLine );
                    break;
                case DerVideotexString _:
                    buf.Append( indent + "VideotexString(" + ((DerStringBase)obj).GetString() + ") " + NewLine );
                    break;
                case DerUtcTime _:
                    buf.Append( indent + "UTCTime(" + ((DerUtcTime)obj).TimeString + ") " + NewLine );
                    break;
                case DerGeneralizedTime _:
                    buf.Append( indent + "GeneralizedTime(" + ((DerGeneralizedTime)obj).GetTime() + ") " + NewLine );
                    break;
                case BerApplicationSpecific _:
                    buf.Append( outputApplicationSpecific( "BER", indent, verbose, (DerApplicationSpecific)obj ) );
                    break;
                case DerApplicationSpecific _:
                    buf.Append( outputApplicationSpecific( "DER", indent, verbose, (DerApplicationSpecific)obj ) );
                    break;
                case DerEnumerated _:
                    DerEnumerated derEnumerated = (DerEnumerated)obj;
                    buf.Append( indent + "DER Enumerated(" + derEnumerated.Value + ")" + NewLine );
                    break;
                case DerExternal _:
                    DerExternal derExternal = (DerExternal)obj;
                    buf.Append( indent + "External " + NewLine );
                    string indent5 = indent + "    ";
                    if (derExternal.DirectReference != null)
                        buf.Append( indent5 + "Direct Reference: " + derExternal.DirectReference.Id + NewLine );
                    if (derExternal.IndirectReference != null)
                        buf.Append( indent5 + "Indirect Reference: " + derExternal.IndirectReference.ToString() + NewLine );
                    if (derExternal.DataValueDescriptor != null)
                        AsString( indent5, verbose, derExternal.DataValueDescriptor, buf );
                    buf.Append( indent5 + "Encoding: " + derExternal.Encoding + NewLine );
                    AsString( indent5, verbose, derExternal.ExternalContent, buf );
                    break;
                default:
                    buf.Append( indent + obj.ToString() + NewLine );
                    break;
            }
        }

        private static string outputApplicationSpecific(
          string type,
          string indent,
          bool verbose,
          DerApplicationSpecific app )
        {
            StringBuilder buf = new StringBuilder();
            if (app.IsConstructed())
            {
                try
                {
                    Asn1Sequence instance = Asn1Sequence.GetInstance( app.GetObject( 16 ) );
                    buf.Append( indent + type + " ApplicationSpecific[" + app.ApplicationTag + "]" + NewLine );
                    foreach (Asn1Encodable asn1Encodable in instance)
                        AsString( indent + "    ", verbose, asn1Encodable.ToAsn1Object(), buf );
                }
                catch (IOException ex)
                {
                    buf.Append( ex );
                }
                return buf.ToString();
            }
            return indent + type + " ApplicationSpecific[" + app.ApplicationTag + "] (" + Hex.ToHexString( app.GetContents() ) + ")" + NewLine;
        }

        [Obsolete( "Use version accepting Asn1Encodable" )]
        public static string DumpAsString( object obj )
        {
            if (!(obj is Asn1Encodable))
                return "unknown object type " + obj.ToString();
            StringBuilder buf = new StringBuilder();
            AsString( "", false, ((Asn1Encodable)obj).ToAsn1Object(), buf );
            return buf.ToString();
        }

        public static string DumpAsString( Asn1Encodable obj ) => DumpAsString( obj, false );

        public static string DumpAsString( Asn1Encodable obj, bool verbose )
        {
            StringBuilder buf = new StringBuilder();
            AsString( "", verbose, obj.ToAsn1Object(), buf );
            return buf.ToString();
        }

        private static string dumpBinaryDataAsString( string indent, byte[] bytes )
        {
            indent += "    ";
            StringBuilder stringBuilder = new StringBuilder( NewLine );
            for (int off = 0; off < bytes.Length; off += 32)
            {
                if (bytes.Length - off > 32)
                {
                    stringBuilder.Append( indent );
                    stringBuilder.Append( Hex.ToHexString( bytes, off, 32 ) );
                    stringBuilder.Append( "    " );
                    stringBuilder.Append( calculateAscString( bytes, off, 32 ) );
                    stringBuilder.Append( NewLine );
                }
                else
                {
                    stringBuilder.Append( indent );
                    stringBuilder.Append( Hex.ToHexString( bytes, off, bytes.Length - off ) );
                    for (int index = bytes.Length - off; index != 32; ++index)
                        stringBuilder.Append( "  " );
                    stringBuilder.Append( "    " );
                    stringBuilder.Append( calculateAscString( bytes, off, bytes.Length - off ) );
                    stringBuilder.Append( NewLine );
                }
            }
            return stringBuilder.ToString();
        }

        private static string calculateAscString( byte[] bytes, int off, int len )
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = off; index != off + len; ++index)
            {
                char ch = (char)bytes[index];
                if (ch >= ' ' && ch <= '~')
                    stringBuilder.Append( ch );
            }
            return stringBuilder.ToString();
        }
    }
}
