// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Agreement.JPake.JPakeUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System;
using System.Text;

namespace Org.BouncyCastle.Crypto.Agreement.JPake
{
    public abstract class JPakeUtilities
    {
        public static readonly BigInteger Zero = BigInteger.Zero;
        public static readonly BigInteger One = BigInteger.One;

        public static BigInteger GenerateX1( BigInteger q, SecureRandom random ) => BigIntegers.CreateRandomInRange( Zero, q.Subtract( One ), random );

        public static BigInteger GenerateX2( BigInteger q, SecureRandom random ) => BigIntegers.CreateRandomInRange( One, q.Subtract( One ), random );

        public static BigInteger CalculateS( char[] password ) => new BigInteger( Encoding.UTF8.GetBytes( password ) );

        public static BigInteger CalculateGx( BigInteger p, BigInteger g, BigInteger x ) => g.ModPow( x, p );

        public static BigInteger CalculateGA(
          BigInteger p,
          BigInteger gx1,
          BigInteger gx3,
          BigInteger gx4 )
        {
            return gx1.Multiply( gx3 ).Multiply( gx4 ).Mod( p );
        }

        public static BigInteger CalculateX2s( BigInteger q, BigInteger x2, BigInteger s ) => x2.Multiply( s ).Mod( q );

        public static BigInteger CalculateA( BigInteger p, BigInteger q, BigInteger gA, BigInteger x2s ) => gA.ModPow( x2s, p );

        public static BigInteger[] CalculateZeroKnowledgeProof(
          BigInteger p,
          BigInteger q,
          BigInteger g,
          BigInteger gx,
          BigInteger x,
          string participantId,
          IDigest digest,
          SecureRandom random )
        {
            BigInteger randomInRange = BigIntegers.CreateRandomInRange( Zero, q.Subtract( One ), random );
            BigInteger gr = g.ModPow( randomInRange, p );
            BigInteger zeroKnowledgeProof = CalculateHashForZeroKnowledgeProof( g, gr, gx, participantId, digest );
            return new BigInteger[2]
            {
        gr,
        randomInRange.Subtract(x.Multiply(zeroKnowledgeProof)).Mod(q)
            };
        }

        private static BigInteger CalculateHashForZeroKnowledgeProof(
          BigInteger g,
          BigInteger gr,
          BigInteger gx,
          string participantId,
          IDigest digest )
        {
            digest.Reset();
            UpdateDigestIncludingSize( digest, g );
            UpdateDigestIncludingSize( digest, gr );
            UpdateDigestIncludingSize( digest, gx );
            UpdateDigestIncludingSize( digest, participantId );
            return new BigInteger( DigestUtilities.DoFinal( digest ) );
        }

        public static void ValidateGx4( BigInteger gx4 )
        {
            if (gx4.Equals( One ))
                throw new CryptoException( "g^x validation failed.  g^x should not be 1." );
        }

        public static void ValidateGa( BigInteger ga )
        {
            if (ga.Equals( One ))
                throw new CryptoException( "ga is equal to 1.  It should not be.  The chances of this happening are on the order of 2^160 for a 160-bit q.  Try again." );
        }

        public static void ValidateZeroKnowledgeProof(
          BigInteger p,
          BigInteger q,
          BigInteger g,
          BigInteger gx,
          BigInteger[] zeroKnowledgeProof,
          string participantId,
          IDigest digest )
        {
            BigInteger gr = zeroKnowledgeProof[0];
            BigInteger e = zeroKnowledgeProof[1];
            BigInteger zeroKnowledgeProof1 = CalculateHashForZeroKnowledgeProof( g, gr, gx, participantId, digest );
            if (gx.CompareTo( Zero ) != 1 || gx.CompareTo( p ) != -1 || gx.ModPow( q, p ).CompareTo( One ) != 0 || g.ModPow( e, p ).Multiply( gx.ModPow( zeroKnowledgeProof1, p ) ).Mod( p ).CompareTo( gr ) != 0)
                throw new CryptoException( "Zero-knowledge proof validation failed" );
        }

        public static BigInteger CalculateKeyingMaterial(
          BigInteger p,
          BigInteger q,
          BigInteger gx4,
          BigInteger x2,
          BigInteger s,
          BigInteger B )
        {
            return gx4.ModPow( x2.Multiply( s ).Negate().Mod( q ), p ).Multiply( B ).ModPow( x2, p );
        }

        public static void ValidateParticipantIdsDiffer( string participantId1, string participantId2 )
        {
            if (participantId1.Equals( participantId2 ))
                throw new CryptoException( "Both participants are using the same participantId (" + participantId1 + "). This is not allowed. Each participant must use a unique participantId." );
        }

        public static void ValidateParticipantIdsEqual(
          string expectedParticipantId,
          string actualParticipantId )
        {
            if (!expectedParticipantId.Equals( actualParticipantId ))
                throw new CryptoException( "Received payload from incorrect partner (" + actualParticipantId + "). Expected to receive payload from " + expectedParticipantId + "." );
        }

        public static void ValidateNotNull( object obj, string description )
        {
            if (obj == null)
                throw new ArgumentNullException( description );
        }

        public static BigInteger CalculateMacTag(
          string participantId,
          string partnerParticipantId,
          BigInteger gx1,
          BigInteger gx2,
          BigInteger gx3,
          BigInteger gx4,
          BigInteger keyingMaterial,
          IDigest digest )
        {
            byte[] macKey = CalculateMacKey( keyingMaterial, digest );
            HMac hmac = new HMac( digest );
            hmac.Init( new KeyParameter( macKey ) );
            Arrays.Fill( macKey, 0 );
            UpdateMac( hmac, "KC_1_U" );
            UpdateMac( hmac, participantId );
            UpdateMac( hmac, partnerParticipantId );
            UpdateMac( hmac, gx1 );
            UpdateMac( hmac, gx2 );
            UpdateMac( hmac, gx3 );
            UpdateMac( hmac, gx4 );
            return new BigInteger( MacUtilities.DoFinal( hmac ) );
        }

        private static byte[] CalculateMacKey( BigInteger keyingMaterial, IDigest digest )
        {
            digest.Reset();
            UpdateDigest( digest, keyingMaterial );
            UpdateDigest( digest, "JPAKE_KC" );
            return DigestUtilities.DoFinal( digest );
        }

        public static void ValidateMacTag(
          string participantId,
          string partnerParticipantId,
          BigInteger gx1,
          BigInteger gx2,
          BigInteger gx3,
          BigInteger gx4,
          BigInteger keyingMaterial,
          IDigest digest,
          BigInteger partnerMacTag )
        {
            if (!CalculateMacTag( partnerParticipantId, participantId, gx3, gx4, gx1, gx2, keyingMaterial, digest ).Equals( partnerMacTag ))
                throw new CryptoException( "Partner MacTag validation failed. Therefore, the password, MAC, or digest algorithm of each participant does not match." );
        }

        private static void UpdateDigest( IDigest digest, BigInteger bigInteger ) => UpdateDigest( digest, BigIntegers.AsUnsignedByteArray( bigInteger ) );

        private static void UpdateDigest( IDigest digest, string str ) => UpdateDigest( digest, Encoding.UTF8.GetBytes( str ) );

        private static void UpdateDigest( IDigest digest, byte[] bytes )
        {
            digest.BlockUpdate( bytes, 0, bytes.Length );
            Arrays.Fill( bytes, 0 );
        }

        private static void UpdateDigestIncludingSize( IDigest digest, BigInteger bigInteger ) => UpdateDigestIncludingSize( digest, BigIntegers.AsUnsignedByteArray( bigInteger ) );

        private static void UpdateDigestIncludingSize( IDigest digest, string str ) => UpdateDigestIncludingSize( digest, Encoding.UTF8.GetBytes( str ) );

        private static void UpdateDigestIncludingSize( IDigest digest, byte[] bytes )
        {
            digest.BlockUpdate( IntToByteArray( bytes.Length ), 0, 4 );
            digest.BlockUpdate( bytes, 0, bytes.Length );
            Arrays.Fill( bytes, 0 );
        }

        private static void UpdateMac( IMac mac, BigInteger bigInteger ) => UpdateMac( mac, BigIntegers.AsUnsignedByteArray( bigInteger ) );

        private static void UpdateMac( IMac mac, string str ) => UpdateMac( mac, Encoding.UTF8.GetBytes( str ) );

        private static void UpdateMac( IMac mac, byte[] bytes )
        {
            mac.BlockUpdate( bytes, 0, bytes.Length );
            Arrays.Fill( bytes, 0 );
        }

        private static byte[] IntToByteArray( int value ) => Pack.UInt32_To_BE( (uint)value );
    }
}
