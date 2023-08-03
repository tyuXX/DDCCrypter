// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.AesEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class AesEngine : IBlockCipher
    {
        private const uint m1 = 2155905152;
        private const uint m2 = 2139062143;
        private const uint m3 = 27;
        private const uint m4 = 3233857728;
        private const uint m5 = 1061109567;
        private const int BLOCK_SIZE = 16;
        private static readonly byte[] S = new byte[256]
        {
       99,
       124,
       119,
       123,
       242,
       107,
       111,
       197,
       48,
       1,
       103,
       43,
       254,
       215,
       171,
       118,
       202,
       130,
       201,
       125,
       250,
       89,
       71,
       240,
       173,
       212,
       162,
       175,
       156,
       164,
       114,
       192,
       183,
       253,
       147,
       38,
       54,
       63,
       247,
       204,
       52,
       165,
       229,
       241,
       113,
       216,
       49,
       21,
       4,
       199,
       35,
       195,
       24,
       150,
       5,
       154,
       7,
       18,
       128,
       226,
       235,
       39,
       178,
       117,
       9,
       131,
       44,
       26,
       27,
       110,
       90,
       160,
       82,
       59,
       214,
       179,
       41,
       227,
       47,
       132,
       83,
       209,
       0,
       237,
       32,
       252,
       177,
       91,
       106,
       203,
       190,
       57,
       74,
       76,
       88,
       207,
       208,
       239,
       170,
       251,
       67,
       77,
       51,
       133,
       69,
       249,
       2,
       127,
       80,
       60,
       159,
       168,
       81,
       163,
       64,
       143,
       146,
       157,
       56,
       245,
       188,
       182,
       218,
       33,
       16,
      byte.MaxValue,
       243,
       210,
       205,
       12,
       19,
       236,
       95,
       151,
       68,
       23,
       196,
       167,
       126,
       61,
       100,
       93,
       25,
       115,
       96,
       129,
       79,
       220,
       34,
       42,
       144,
       136,
       70,
       238,
       184,
       20,
       222,
       94,
       11,
       219,
       224,
       50,
       58,
       10,
       73,
       6,
       36,
       92,
       194,
       211,
       172,
       98,
       145,
       149,
       228,
       121,
       231,
       200,
       55,
       109,
       141,
       213,
       78,
       169,
       108,
       86,
       244,
       234,
       101,
       122,
       174,
       8,
       186,
       120,
       37,
       46,
       28,
       166,
       180,
       198,
       232,
       221,
       116,
       31,
       75,
       189,
       139,
       138,
       112,
       62,
       181,
       102,
       72,
       3,
       246,
       14,
       97,
       53,
       87,
       185,
       134,
       193,
       29,
       158,
       225,
       248,
       152,
       17,
       105,
       217,
       142,
       148,
       155,
       30,
       135,
       233,
       206,
       85,
       40,
       223,
       140,
       161,
       137,
       13,
       191,
       230,
       66,
       104,
       65,
       153,
       45,
       15,
       176,
       84,
       187,
       22
        };
        private static readonly byte[] Si = new byte[256]
        {
       82,
       9,
       106,
       213,
       48,
       54,
       165,
       56,
       191,
       64,
       163,
       158,
       129,
       243,
       215,
       251,
       124,
       227,
       57,
       130,
       155,
       47,
      byte.MaxValue,
       135,
       52,
       142,
       67,
       68,
       196,
       222,
       233,
       203,
       84,
       123,
       148,
       50,
       166,
       194,
       35,
       61,
       238,
       76,
       149,
       11,
       66,
       250,
       195,
       78,
       8,
       46,
       161,
       102,
       40,
       217,
       36,
       178,
       118,
       91,
       162,
       73,
       109,
       139,
       209,
       37,
       114,
       248,
       246,
       100,
       134,
       104,
       152,
       22,
       212,
       164,
       92,
       204,
       93,
       101,
       182,
       146,
       108,
       112,
       72,
       80,
       253,
       237,
       185,
       218,
       94,
       21,
       70,
       87,
       167,
       141,
       157,
       132,
       144,
       216,
       171,
       0,
       140,
       188,
       211,
       10,
       247,
       228,
       88,
       5,
       184,
       179,
       69,
       6,
       208,
       44,
       30,
       143,
       202,
       63,
       15,
       2,
       193,
       175,
       189,
       3,
       1,
       19,
       138,
       107,
       58,
       145,
       17,
       65,
       79,
       103,
       220,
       234,
       151,
       242,
       207,
       206,
       240,
       180,
       230,
       115,
       150,
       172,
       116,
       34,
       231,
       173,
       53,
       133,
       226,
       249,
       55,
       232,
       28,
       117,
       223,
       110,
       71,
       241,
       26,
       113,
       29,
       41,
       197,
       137,
       111,
       183,
       98,
       14,
       170,
       24,
       190,
       27,
       252,
       86,
       62,
       75,
       198,
       210,
       121,
       32,
       154,
       219,
       192,
       254,
       120,
       205,
       90,
       244,
       31,
       221,
       168,
       51,
       136,
       7,
       199,
       49,
       177,
       18,
       16,
       89,
       39,
       128,
       236,
       95,
       96,
       81,
       127,
       169,
       25,
       181,
       74,
       13,
       45,
       229,
       122,
       159,
       147,
       201,
       156,
       239,
       160,
       224,
       59,
       77,
       174,
       42,
       245,
       176,
       200,
       235,
       187,
       60,
       131,
       83,
       153,
       97,
       23,
       43,
       4,
       126,
       186,
       119,
       214,
       38,
       225,
       105,
       20,
       99,
       85,
       33,
       12,
       125
        };
        private static readonly byte[] rcon = new byte[30]
        {
       1,
       2,
       4,
       8,
       16,
       32,
       64,
       128,
       27,
       54,
       108,
       216,
       171,
       77,
       154,
       47,
       94,
       188,
       99,
       198,
       151,
       53,
       106,
       212,
       179,
       125,
       250,
       239,
       197,
       145
        };
        private static readonly uint[] T0 = new uint[256]
        {
      2774754246U,
      2222750968U,
      2574743534U,
      2373680118U,
      234025727U,
      3177933782U,
      2976870366U,
      1422247313U,
      1345335392U,
      50397442U,
      2842126286U,
      2099981142U,
      436141799U,
      1658312629U,
      3870010189U,
      2591454956U,
      1170918031U,
      2642575903U,
      1086966153U,
      2273148410U,
      368769775U,
      3948501426U,
      3376891790U,
      200339707U,
      3970805057U,
      1742001331U,
      4255294047U,
      3937382213U,
      3214711843U,
      4154762323U,
      2524082916U,
      1539358875U,
      3266819957U,
      486407649U,
      2928907069U,
      1780885068U,
      1513502316U,
      1094664062U,
      49805301U,
      1338821763U,
      1546925160U,
      4104496465U,
      887481809U,
      150073849U,
      2473685474U,
      1943591083U,
      1395732834U,
      1058346282U,
      201589768U,
      1388824469U,
      1696801606U,
      1589887901U,
      672667696U,
      2711000631U,
      251987210U,
      3046808111U,
      151455502U,
      907153956U,
      2608889883U,
      1038279391U,
      652995533U,
      1764173646U,
      3451040383U,
      2675275242U,
      453576978U,
      2659418909U,
      1949051992U,
      773462580U,
      756751158U,
      2993581788U,
      3998898868U,
      4221608027U,
      4132590244U,
      1295727478U,
      1641469623U,
      3467883389U,
      2066295122U,
      1055122397U,
      1898917726U,
      2542044179U,
      4115878822U,
      1758581177U,
      0U,
      753790401U,
      1612718144U,
      536673507U,
      3367088505U,
      3982187446U,
      3194645204U,
      1187761037U,
      3653156455U,
      1262041458U,
      3729410708U,
      3561770136U,
      3898103984U,
      1255133061U,
      1808847035U,
      720367557U,
      3853167183U,
      385612781U,
      3309519750U,
      3612167578U,
      1429418854U,
      2491778321U,
      3477423498U,
      284817897U,
      100794884U,
      2172616702U,
      4031795360U,
      1144798328U,
      3131023141U,
      3819481163U,
      4082192802U,
      4272137053U,
      3225436288U,
      2324664069U,
      2912064063U,
      3164445985U,
      1211644016U,
      83228145U,
      3753688163U,
      3249976951U,
      1977277103U,
      1663115586U,
      806359072U,
      452984805U,
      250868733U,
      1842533055U,
      1288555905U,
      336333848U,
      890442534U,
      804056259U,
      3781124030U,
      2727843637U,
      3427026056U,
      957814574U,
      1472513171U,
      4071073621U,
      2189328124U,
      1195195770U,
      2892260552U,
      3881655738U,
      723065138U,
      2507371494U,
      2690670784U,
      2558624025U,
      3511635870U,
      2145180835U,
      1713513028U,
      2116692564U,
      2878378043U,
      2206763019U,
      3393603212U,
      703524551U,
      3552098411U,
      1007948840U,
      2044649127U,
      3797835452U,
      487262998U,
      1994120109U,
      1004593371U,
      1446130276U,
      1312438900U,
      503974420U,
      3679013266U,
      168166924U,
      1814307912U,
      3831258296U,
      1573044895U,
      1859376061U,
      4021070915U,
      2791465668U,
      2828112185U,
      2761266481U,
      937747667U,
      2339994098U,
      854058965U,
      1137232011U,
      1496790894U,
      3077402074U,
      2358086913U,
      1691735473U,
      3528347292U,
      3769215305U,
      3027004632U,
      4199962284U,
      133494003U,
      636152527U,
      2942657994U,
      2390391540U,
      3920539207U,
      403179536U,
      3585784431U,
      2289596656U,
      1864705354U,
      1915629148U,
      605822008U,
      4054230615U,
      3350508659U,
      1371981463U,
      602466507U,
      2094914977U,
      2624877800U,
      555687742U,
      3712699286U,
      3703422305U,
      2257292045U,
      2240449039U,
      2423288032U,
      1111375484U,
      3300242801U,
      2858837708U,
      3628615824U,
      84083462U,
      32962295U,
      302911004U,
      2741068226U,
      1597322602U,
      4183250862U,
      3501832553U,
      2441512471U,
      1489093017U,
      656219450U,
      3114180135U,
      954327513U,
      335083755U,
      3013122091U,
      856756514U,
      3144247762U,
      1893325225U,
      2307821063U,
      2811532339U,
      3063651117U,
      572399164U,
      2458355477U,
      552200649U,
      1238290055U,
      4283782570U,
      2015897680U,
      2061492133U,
      2408352771U,
      4171342169U,
      2156497161U,
      386731290U,
      3669999461U,
      837215959U,
      3326231172U,
      3093850320U,
      3275833730U,
      2962856233U,
      1999449434U,
      286199582U,
      3417354363U,
      4233385128U,
      3602627437U,
      974525996U
        };
        private static readonly uint[] Tinv0 = new uint[256]
        {
      1353184337U,
      1399144830U,
      3282310938U,
      2522752826U,
      3412831035U,
      4047871263U,
      2874735276U,
      2466505547U,
      1442459680U,
      4134368941U,
      2440481928U,
      625738485U,
      4242007375U,
      3620416197U,
      2151953702U,
      2409849525U,
      1230680542U,
      1729870373U,
      2551114309U,
      3787521629U,
      41234371U,
      317738113U,
      2744600205U,
      3338261355U,
      3881799427U,
      2510066197U,
      3950669247U,
      3663286933U,
      763608788U,
      3542185048U,
      694804553U,
      1154009486U,
      1787413109U,
      2021232372U,
      1799248025U,
      3715217703U,
      3058688446U,
      397248752U,
      1722556617U,
      3023752829U,
      407560035U,
      2184256229U,
      1613975959U,
      1165972322U,
      3765920945U,
      2226023355U,
      480281086U,
      2485848313U,
      1483229296U,
      436028815U,
      2272059028U,
      3086515026U,
      601060267U,
      3791801202U,
      1468997603U,
      715871590U,
      120122290U,
      63092015U,
      2591802758U,
      2768779219U,
      4068943920U,
      2997206819U,
      3127509762U,
      1552029421U,
      723308426U,
      2461301159U,
      4042393587U,
      2715969870U,
      3455375973U,
      3586000134U,
      526529745U,
      2331944644U,
      2639474228U,
      2689987490U,
      853641733U,
      1978398372U,
      971801355U,
      2867814464U,
      111112542U,
      1360031421U,
      4186579262U,
      1023860118U,
      2919579357U,
      1186850381U,
      3045938321U,
      90031217U,
      1876166148U,
      4279586912U,
      620468249U,
      2548678102U,
      3426959497U,
      2006899047U,
      3175278768U,
      2290845959U,
      945494503U,
      3689859193U,
      1191869601U,
      3910091388U,
      3374220536U,
      0U,
      2206629897U,
      1223502642U,
      2893025566U,
      1316117100U,
      4227796733U,
      1446544655U,
      517320253U,
      658058550U,
      1691946762U,
      564550760U,
      3511966619U,
      976107044U,
      2976320012U,
      266819475U,
      3533106868U,
      2660342555U,
      1338359936U,
      2720062561U,
      1766553434U,
      370807324U,
      179999714U,
      3844776128U,
      1138762300U,
      488053522U,
      185403662U,
      2915535858U,
      3114841645U,
      3366526484U,
      2233069911U,
      1275557295U,
      3151862254U,
      4250959779U,
      2670068215U,
      3170202204U,
      3309004356U,
      880737115U,
      1982415755U,
      3703972811U,
      1761406390U,
      1676797112U,
      3403428311U,
      277177154U,
      1076008723U,
      538035844U,
      2099530373U,
      4164795346U,
      288553390U,
      1839278535U,
      1261411869U,
      4080055004U,
      3964831245U,
      3504587127U,
      1813426987U,
      2579067049U,
      4199060497U,
      577038663U,
      3297574056U,
      440397984U,
      3626794326U,
      4019204898U,
      3343796615U,
      3251714265U,
      4272081548U,
      906744984U,
      3481400742U,
      685669029U,
      646887386U,
      2764025151U,
      3835509292U,
      227702864U,
      2613862250U,
      1648787028U,
      3256061430U,
      3904428176U,
      1593260334U,
      4121936770U,
      3196083615U,
      2090061929U,
      2838353263U,
      3004310991U,
      999926984U,
      2809993232U,
      1852021992U,
      2075868123U,
      158869197U,
      4095236462U,
      28809964U,
      2828685187U,
      1701746150U,
      2129067946U,
      147831841U,
      3873969647U,
      3650873274U,
      3459673930U,
      3557400554U,
      3598495785U,
      2947720241U,
      824393514U,
      815048134U,
      3227951669U,
      935087732U,
      2798289660U,
      2966458592U,
      366520115U,
      1251476721U,
      4158319681U,
      240176511U,
      804688151U,
      2379631990U,
      1303441219U,
      1414376140U,
      3741619940U,
      3820343710U,
      461924940U,
      3089050817U,
      2136040774U,
      82468509U,
      1563790337U,
      1937016826U,
      776014843U,
      1511876531U,
      1389550482U,
      861278441U,
      323475053U,
      2355222426U,
      2047648055U,
      2383738969U,
      2302415851U,
      3995576782U,
      902390199U,
      3991215329U,
      1018251130U,
      1507840668U,
      1064563285U,
      2043548696U,
      3208103795U,
      3939366739U,
      1537932639U,
      342834655U,
      2262516856U,
      2180231114U,
      1053059257U,
      741614648U,
      1598071746U,
      1925389590U,
      203809468U,
      2336832552U,
      1100287487U,
      1895934009U,
      3736275976U,
      2632234200U,
      2428589668U,
      1636092795U,
      1890988757U,
      1952214088U,
      1113045200U
        };
        private int ROUNDS;
        private uint[][] WorkingKey;
        private uint C0;
        private uint C1;
        private uint C2;
        private uint C3;
        private bool forEncryption;

        private static uint Shift( uint r, int shift ) => (r >> shift) | (r << (32 - shift));

        private static uint FFmulX( uint x ) => (uint)((((int)x & 2139062143) << 1) ^ ((int)((x & 2155905152U) >> 7) * 27));

        private static uint FFmulX2( uint x )
        {
            uint num1 = (uint)(((int)x & 1061109567) << 2);
            uint num2 = x & 3233857728U;
            uint num3 = num2 ^ (num2 >> 1);
            return num1 ^ (num3 >> 2) ^ (num3 >> 5);
        }

        private static uint Inv_Mcol( uint x )
        {
            uint r1 = x;
            uint x1 = r1 ^ Shift( r1, 8 );
            uint x2 = r1 ^ FFmulX( x1 );
            uint r2 = x1 ^ FFmulX2( x2 );
            return x2 ^ r2 ^ Shift( r2, 16 );
        }

        private static uint SubWord( uint x ) => (uint)(S[(int)(IntPtr)(x & byte.MaxValue)] | (S[(int)(IntPtr)((x >> 8) & byte.MaxValue)] << 8) | (S[(int)(IntPtr)((x >> 16) & byte.MaxValue)] << 16) | (S[(int)(IntPtr)((x >> 24) & byte.MaxValue)] << 24));

        private uint[][] GenerateWorkingKey( byte[] key, bool forEncryption )
        {
            int length = key.Length;
            if (length < 16 || length > 32 || (length & 7) != 0)
                throw new ArgumentException( "Key length not 128/192/256 bits." );
            int num1 = length >> 2;
            this.ROUNDS = num1 + 6;
            uint[][] workingKey = new uint[this.ROUNDS + 1][];
            for (int index = 0; index <= this.ROUNDS; ++index)
                workingKey[index] = new uint[4];
            switch (num1)
            {
                case 4:
                    uint uint32_1 = Pack.LE_To_UInt32( key, 0 );
                    workingKey[0][0] = uint32_1;
                    uint uint32_2 = Pack.LE_To_UInt32( key, 4 );
                    workingKey[0][1] = uint32_2;
                    uint uint32_3 = Pack.LE_To_UInt32( key, 8 );
                    workingKey[0][2] = uint32_3;
                    uint uint32_4 = Pack.LE_To_UInt32( key, 12 );
                    workingKey[0][3] = uint32_4;
                    for (int index = 1; index <= 10; ++index)
                    {
                        uint num2 = SubWord( Shift( uint32_4, 8 ) ) ^ rcon[index - 1];
                        uint32_1 ^= num2;
                        workingKey[index][0] = uint32_1;
                        uint32_2 ^= uint32_1;
                        workingKey[index][1] = uint32_2;
                        uint32_3 ^= uint32_2;
                        workingKey[index][2] = uint32_3;
                        uint32_4 ^= uint32_3;
                        workingKey[index][3] = uint32_4;
                    }
                    break;
                case 6:
                    uint uint32_5 = Pack.LE_To_UInt32( key, 0 );
                    workingKey[0][0] = uint32_5;
                    uint uint32_6 = Pack.LE_To_UInt32( key, 4 );
                    workingKey[0][1] = uint32_6;
                    uint uint32_7 = Pack.LE_To_UInt32( key, 8 );
                    workingKey[0][2] = uint32_7;
                    uint uint32_8 = Pack.LE_To_UInt32( key, 12 );
                    workingKey[0][3] = uint32_8;
                    uint uint32_9 = Pack.LE_To_UInt32( key, 16 );
                    workingKey[1][0] = uint32_9;
                    uint uint32_10 = Pack.LE_To_UInt32( key, 20 );
                    workingKey[1][1] = uint32_10;
                    uint num3 = 1;
                    uint num4 = SubWord( Shift( uint32_10, 8 ) ) ^ num3;
                    uint num5 = num3 << 1;
                    uint num6 = uint32_5 ^ num4;
                    workingKey[1][2] = num6;
                    uint num7 = uint32_6 ^ num6;
                    workingKey[1][3] = num7;
                    uint num8 = uint32_7 ^ num7;
                    workingKey[2][0] = num8;
                    uint num9 = uint32_8 ^ num8;
                    workingKey[2][1] = num9;
                    uint num10 = uint32_9 ^ num9;
                    workingKey[2][2] = num10;
                    uint r1 = uint32_10 ^ num10;
                    workingKey[2][3] = r1;
                    for (int index = 3; index < 12; index += 3)
                    {
                        uint num11 = SubWord( Shift( r1, 8 ) ) ^ num5;
                        uint num12 = num5 << 1;
                        uint num13 = num6 ^ num11;
                        workingKey[index][0] = num13;
                        uint num14 = num7 ^ num13;
                        workingKey[index][1] = num14;
                        uint num15 = num8 ^ num14;
                        workingKey[index][2] = num15;
                        uint num16 = num9 ^ num15;
                        workingKey[index][3] = num16;
                        uint num17 = num10 ^ num16;
                        workingKey[index + 1][0] = num17;
                        uint r2 = r1 ^ num17;
                        workingKey[index + 1][1] = r2;
                        uint num18 = SubWord( Shift( r2, 8 ) ) ^ num12;
                        num5 = num12 << 1;
                        num6 = num13 ^ num18;
                        workingKey[index + 1][2] = num6;
                        num7 = num14 ^ num6;
                        workingKey[index + 1][3] = num7;
                        num8 = num15 ^ num7;
                        workingKey[index + 2][0] = num8;
                        num9 = num16 ^ num8;
                        workingKey[index + 2][1] = num9;
                        num10 = num17 ^ num9;
                        workingKey[index + 2][2] = num10;
                        r1 = r2 ^ num10;
                        workingKey[index + 2][3] = r1;
                    }
                    uint num19 = SubWord( Shift( r1, 8 ) ) ^ num5;
                    uint num20 = num6 ^ num19;
                    workingKey[12][0] = num20;
                    uint num21 = num7 ^ num20;
                    workingKey[12][1] = num21;
                    uint num22 = num8 ^ num21;
                    workingKey[12][2] = num22;
                    uint num23 = num9 ^ num22;
                    workingKey[12][3] = num23;
                    break;
                case 8:
                    uint uint32_11 = Pack.LE_To_UInt32( key, 0 );
                    workingKey[0][0] = uint32_11;
                    uint uint32_12 = Pack.LE_To_UInt32( key, 4 );
                    workingKey[0][1] = uint32_12;
                    uint uint32_13 = Pack.LE_To_UInt32( key, 8 );
                    workingKey[0][2] = uint32_13;
                    uint uint32_14 = Pack.LE_To_UInt32( key, 12 );
                    workingKey[0][3] = uint32_14;
                    uint uint32_15 = Pack.LE_To_UInt32( key, 16 );
                    workingKey[1][0] = uint32_15;
                    uint uint32_16 = Pack.LE_To_UInt32( key, 20 );
                    workingKey[1][1] = uint32_16;
                    uint uint32_17 = Pack.LE_To_UInt32( key, 24 );
                    workingKey[1][2] = uint32_17;
                    uint uint32_18 = Pack.LE_To_UInt32( key, 28 );
                    workingKey[1][3] = uint32_18;
                    uint num24 = 1;
                    for (int index = 2; index < 14; index += 2)
                    {
                        uint num25 = SubWord( Shift( uint32_18, 8 ) ) ^ num24;
                        num24 <<= 1;
                        uint32_11 ^= num25;
                        workingKey[index][0] = uint32_11;
                        uint32_12 ^= uint32_11;
                        workingKey[index][1] = uint32_12;
                        uint32_13 ^= uint32_12;
                        workingKey[index][2] = uint32_13;
                        uint32_14 ^= uint32_13;
                        workingKey[index][3] = uint32_14;
                        uint num26 = SubWord( uint32_14 );
                        uint32_15 ^= num26;
                        workingKey[index + 1][0] = uint32_15;
                        uint32_16 ^= uint32_15;
                        workingKey[index + 1][1] = uint32_16;
                        uint32_17 ^= uint32_16;
                        workingKey[index + 1][2] = uint32_17;
                        uint32_18 ^= uint32_17;
                        workingKey[index + 1][3] = uint32_18;
                    }
                    uint num27 = SubWord( Shift( uint32_18, 8 ) ) ^ num24;
                    uint num28 = uint32_11 ^ num27;
                    workingKey[14][0] = num28;
                    uint num29 = uint32_12 ^ num28;
                    workingKey[14][1] = num29;
                    uint num30 = uint32_13 ^ num29;
                    workingKey[14][2] = num30;
                    uint num31 = uint32_14 ^ num30;
                    workingKey[14][3] = num31;
                    break;
                default:
                    throw new InvalidOperationException( "Should never get here" );
            }
            if (!forEncryption)
            {
                for (int index1 = 1; index1 < this.ROUNDS; ++index1)
                {
                    uint[] numArray = workingKey[index1];
                    for (int index2 = 0; index2 < 4; ++index2)
                        numArray[index2] = Inv_Mcol( numArray[index2] );
                }
            }
            return workingKey;
        }

        public virtual void Init( bool forEncryption, ICipherParameters parameters )
        {
            this.WorkingKey = parameters is KeyParameter keyParameter ? this.GenerateWorkingKey( keyParameter.GetKey(), forEncryption ) : throw new ArgumentException( "invalid parameter passed to AES init - " + Platform.GetTypeName( parameters ) );
            this.forEncryption = forEncryption;
        }

        public virtual string AlgorithmName => "AES";

        public virtual bool IsPartialBlockOkay => false;

        public virtual int GetBlockSize() => 16;

        public virtual int ProcessBlock( byte[] input, int inOff, byte[] output, int outOff )
        {
            if (this.WorkingKey == null)
                throw new InvalidOperationException( "AES engine not initialised" );
            Check.DataLength( input, inOff, 16, "input buffer too short" );
            Check.OutputLength( output, outOff, 16, "output buffer too short" );
            this.UnPackBlock( input, inOff );
            if (this.forEncryption)
                this.EncryptBlock( this.WorkingKey );
            else
                this.DecryptBlock( this.WorkingKey );
            this.PackBlock( output, outOff );
            return 16;
        }

        public virtual void Reset()
        {
        }

        private void UnPackBlock( byte[] bytes, int off )
        {
            this.C0 = Pack.LE_To_UInt32( bytes, off );
            this.C1 = Pack.LE_To_UInt32( bytes, off + 4 );
            this.C2 = Pack.LE_To_UInt32( bytes, off + 8 );
            this.C3 = Pack.LE_To_UInt32( bytes, off + 12 );
        }

        private void PackBlock( byte[] bytes, int off )
        {
            Pack.UInt32_To_LE( this.C0, bytes, off );
            Pack.UInt32_To_LE( this.C1, bytes, off + 4 );
            Pack.UInt32_To_LE( this.C2, bytes, off + 8 );
            Pack.UInt32_To_LE( this.C3, bytes, off + 12 );
        }

        private void EncryptBlock( uint[][] KW )
        {
            uint[] numArray1 = KW[0];
            uint num1 = this.C0 ^ numArray1[0];
            uint num2 = this.C1 ^ numArray1[1];
            uint num3 = this.C2 ^ numArray1[2];
            uint num4 = this.C3 ^ numArray1[3];
            int num5 = 1;
            while (num5 < this.ROUNDS - 1)
            {
                uint[][] numArray2 = KW;
                int index1 = num5;
                int num6 = index1 + 1;
                uint[] numArray3 = numArray2[index1];
                uint num7 = T0[(int)(IntPtr)(num1 & byte.MaxValue)] ^ Shift( T0[(int)(IntPtr)((num2 >> 8) & byte.MaxValue)], 24 ) ^ Shift( T0[(int)(IntPtr)((num3 >> 16) & byte.MaxValue)], 16 ) ^ Shift( T0[(int)(IntPtr)((num4 >> 24) & byte.MaxValue)], 8 ) ^ numArray3[0];
                uint num8 = T0[(int)(IntPtr)(num2 & byte.MaxValue)] ^ Shift( T0[(int)(IntPtr)((num3 >> 8) & byte.MaxValue)], 24 ) ^ Shift( T0[(int)(IntPtr)((num4 >> 16) & byte.MaxValue)], 16 ) ^ Shift( T0[(int)(IntPtr)((num1 >> 24) & byte.MaxValue)], 8 ) ^ numArray3[1];
                uint num9 = T0[(int)(IntPtr)(num3 & byte.MaxValue)] ^ Shift( T0[(int)(IntPtr)((num4 >> 8) & byte.MaxValue)], 24 ) ^ Shift( T0[(int)(IntPtr)((num1 >> 16) & byte.MaxValue)], 16 ) ^ Shift( T0[(int)(IntPtr)((num2 >> 24) & byte.MaxValue)], 8 ) ^ numArray3[2];
                uint num10 = T0[(int)(IntPtr)(num4 & byte.MaxValue)] ^ Shift( T0[(int)(IntPtr)((num1 >> 8) & byte.MaxValue)], 24 ) ^ Shift( T0[(int)(IntPtr)((num2 >> 16) & byte.MaxValue)], 16 ) ^ Shift( T0[(int)(IntPtr)((num3 >> 24) & byte.MaxValue)], 8 ) ^ numArray3[3];
                uint[][] numArray4 = KW;
                int index2 = num6;
                num5 = index2 + 1;
                uint[] numArray5 = numArray4[index2];
                num1 = T0[(int)(IntPtr)(num7 & byte.MaxValue)] ^ Shift( T0[(int)(IntPtr)((num8 >> 8) & byte.MaxValue)], 24 ) ^ Shift( T0[(int)(IntPtr)((num9 >> 16) & byte.MaxValue)], 16 ) ^ Shift( T0[(int)(IntPtr)((num10 >> 24) & byte.MaxValue)], 8 ) ^ numArray5[0];
                num2 = T0[(int)(IntPtr)(num8 & byte.MaxValue)] ^ Shift( T0[(int)(IntPtr)((num9 >> 8) & byte.MaxValue)], 24 ) ^ Shift( T0[(int)(IntPtr)((num10 >> 16) & byte.MaxValue)], 16 ) ^ Shift( T0[(int)(IntPtr)((num7 >> 24) & byte.MaxValue)], 8 ) ^ numArray5[1];
                num3 = T0[(int)(IntPtr)(num9 & byte.MaxValue)] ^ Shift( T0[(int)(IntPtr)((num10 >> 8) & byte.MaxValue)], 24 ) ^ Shift( T0[(int)(IntPtr)((num7 >> 16) & byte.MaxValue)], 16 ) ^ Shift( T0[(int)(IntPtr)((num8 >> 24) & byte.MaxValue)], 8 ) ^ numArray5[2];
                num4 = T0[(int)(IntPtr)(num10 & byte.MaxValue)] ^ Shift( T0[(int)(IntPtr)((num7 >> 8) & byte.MaxValue)], 24 ) ^ Shift( T0[(int)(IntPtr)((num8 >> 16) & byte.MaxValue)], 16 ) ^ Shift( T0[(int)(IntPtr)((num9 >> 24) & byte.MaxValue)], 8 ) ^ numArray5[3];
            }
            uint[][] numArray6 = KW;
            int index3 = num5;
            int index4 = index3 + 1;
            uint[] numArray7 = numArray6[index3];
            uint num11 = T0[(int)(IntPtr)(num1 & byte.MaxValue)] ^ Shift( T0[(int)(IntPtr)((num2 >> 8) & byte.MaxValue)], 24 ) ^ Shift( T0[(int)(IntPtr)((num3 >> 16) & byte.MaxValue)], 16 ) ^ Shift( T0[(int)(IntPtr)((num4 >> 24) & byte.MaxValue)], 8 ) ^ numArray7[0];
            uint num12 = T0[(int)(IntPtr)(num2 & byte.MaxValue)] ^ Shift( T0[(int)(IntPtr)((num3 >> 8) & byte.MaxValue)], 24 ) ^ Shift( T0[(int)(IntPtr)((num4 >> 16) & byte.MaxValue)], 16 ) ^ Shift( T0[(int)(IntPtr)((num1 >> 24) & byte.MaxValue)], 8 ) ^ numArray7[1];
            uint num13 = T0[(int)(IntPtr)(num3 & byte.MaxValue)] ^ Shift( T0[(int)(IntPtr)((num4 >> 8) & byte.MaxValue)], 24 ) ^ Shift( T0[(int)(IntPtr)((num1 >> 16) & byte.MaxValue)], 16 ) ^ Shift( T0[(int)(IntPtr)((num2 >> 24) & byte.MaxValue)], 8 ) ^ numArray7[2];
            uint num14 = T0[(int)(IntPtr)(num4 & byte.MaxValue)] ^ Shift( T0[(int)(IntPtr)((num1 >> 8) & byte.MaxValue)], 24 ) ^ Shift( T0[(int)(IntPtr)((num2 >> 16) & byte.MaxValue)], 16 ) ^ Shift( T0[(int)(IntPtr)((num3 >> 24) & byte.MaxValue)], 8 ) ^ numArray7[3];
            uint[] numArray8 = KW[index4];
            this.C0 = (uint)(S[(int)(IntPtr)(num11 & byte.MaxValue)] ^ (S[(int)(IntPtr)((num12 >> 8) & byte.MaxValue)] << 8) ^ (S[(int)(IntPtr)((num13 >> 16) & byte.MaxValue)] << 16) ^ (S[(int)(IntPtr)((num14 >> 24) & byte.MaxValue)] << 24)) ^ numArray8[0];
            this.C1 = (uint)(S[(int)(IntPtr)(num12 & byte.MaxValue)] ^ (S[(int)(IntPtr)((num13 >> 8) & byte.MaxValue)] << 8) ^ (S[(int)(IntPtr)((num14 >> 16) & byte.MaxValue)] << 16) ^ (S[(int)(IntPtr)((num11 >> 24) & byte.MaxValue)] << 24)) ^ numArray8[1];
            this.C2 = (uint)(S[(int)(IntPtr)(num13 & byte.MaxValue)] ^ (S[(int)(IntPtr)((num14 >> 8) & byte.MaxValue)] << 8) ^ (S[(int)(IntPtr)((num11 >> 16) & byte.MaxValue)] << 16) ^ (S[(int)(IntPtr)((num12 >> 24) & byte.MaxValue)] << 24)) ^ numArray8[2];
            this.C3 = (uint)(S[(int)(IntPtr)(num14 & byte.MaxValue)] ^ (S[(int)(IntPtr)((num11 >> 8) & byte.MaxValue)] << 8) ^ (S[(int)(IntPtr)((num12 >> 16) & byte.MaxValue)] << 16) ^ (S[(int)(IntPtr)((num13 >> 24) & byte.MaxValue)] << 24)) ^ numArray8[3];
        }

        private void DecryptBlock( uint[][] KW )
        {
            uint[] numArray1 = KW[this.ROUNDS];
            uint num1 = this.C0 ^ numArray1[0];
            uint num2 = this.C1 ^ numArray1[1];
            uint num3 = this.C2 ^ numArray1[2];
            uint num4 = this.C3 ^ numArray1[3];
            int num5 = this.ROUNDS - 1;
            while (num5 > 1)
            {
                uint[][] numArray2 = KW;
                int index1 = num5;
                int num6 = index1 - 1;
                uint[] numArray3 = numArray2[index1];
                uint num7 = Tinv0[(int)(IntPtr)(num1 & byte.MaxValue)] ^ Shift( Tinv0[(int)(IntPtr)((num4 >> 8) & byte.MaxValue)], 24 ) ^ Shift( Tinv0[(int)(IntPtr)((num3 >> 16) & byte.MaxValue)], 16 ) ^ Shift( Tinv0[(int)(IntPtr)((num2 >> 24) & byte.MaxValue)], 8 ) ^ numArray3[0];
                uint num8 = Tinv0[(int)(IntPtr)(num2 & byte.MaxValue)] ^ Shift( Tinv0[(int)(IntPtr)((num1 >> 8) & byte.MaxValue)], 24 ) ^ Shift( Tinv0[(int)(IntPtr)((num4 >> 16) & byte.MaxValue)], 16 ) ^ Shift( Tinv0[(int)(IntPtr)((num3 >> 24) & byte.MaxValue)], 8 ) ^ numArray3[1];
                uint num9 = Tinv0[(int)(IntPtr)(num3 & byte.MaxValue)] ^ Shift( Tinv0[(int)(IntPtr)((num2 >> 8) & byte.MaxValue)], 24 ) ^ Shift( Tinv0[(int)(IntPtr)((num1 >> 16) & byte.MaxValue)], 16 ) ^ Shift( Tinv0[(int)(IntPtr)((num4 >> 24) & byte.MaxValue)], 8 ) ^ numArray3[2];
                uint num10 = Tinv0[(int)(IntPtr)(num4 & byte.MaxValue)] ^ Shift( Tinv0[(int)(IntPtr)((num3 >> 8) & byte.MaxValue)], 24 ) ^ Shift( Tinv0[(int)(IntPtr)((num2 >> 16) & byte.MaxValue)], 16 ) ^ Shift( Tinv0[(int)(IntPtr)((num1 >> 24) & byte.MaxValue)], 8 ) ^ numArray3[3];
                uint[][] numArray4 = KW;
                int index2 = num6;
                num5 = index2 - 1;
                uint[] numArray5 = numArray4[index2];
                num1 = Tinv0[(int)(IntPtr)(num7 & byte.MaxValue)] ^ Shift( Tinv0[(int)(IntPtr)((num10 >> 8) & byte.MaxValue)], 24 ) ^ Shift( Tinv0[(int)(IntPtr)((num9 >> 16) & byte.MaxValue)], 16 ) ^ Shift( Tinv0[(int)(IntPtr)((num8 >> 24) & byte.MaxValue)], 8 ) ^ numArray5[0];
                num2 = Tinv0[(int)(IntPtr)(num8 & byte.MaxValue)] ^ Shift( Tinv0[(int)(IntPtr)((num7 >> 8) & byte.MaxValue)], 24 ) ^ Shift( Tinv0[(int)(IntPtr)((num10 >> 16) & byte.MaxValue)], 16 ) ^ Shift( Tinv0[(int)(IntPtr)((num9 >> 24) & byte.MaxValue)], 8 ) ^ numArray5[1];
                num3 = Tinv0[(int)(IntPtr)(num9 & byte.MaxValue)] ^ Shift( Tinv0[(int)(IntPtr)((num8 >> 8) & byte.MaxValue)], 24 ) ^ Shift( Tinv0[(int)(IntPtr)((num7 >> 16) & byte.MaxValue)], 16 ) ^ Shift( Tinv0[(int)(IntPtr)((num10 >> 24) & byte.MaxValue)], 8 ) ^ numArray5[2];
                num4 = Tinv0[(int)(IntPtr)(num10 & byte.MaxValue)] ^ Shift( Tinv0[(int)(IntPtr)((num9 >> 8) & byte.MaxValue)], 24 ) ^ Shift( Tinv0[(int)(IntPtr)((num8 >> 16) & byte.MaxValue)], 16 ) ^ Shift( Tinv0[(int)(IntPtr)((num7 >> 24) & byte.MaxValue)], 8 ) ^ numArray5[3];
            }
            uint[] numArray6 = KW[1];
            uint num11 = Tinv0[(int)(IntPtr)(num1 & byte.MaxValue)] ^ Shift( Tinv0[(int)(IntPtr)((num4 >> 8) & byte.MaxValue)], 24 ) ^ Shift( Tinv0[(int)(IntPtr)((num3 >> 16) & byte.MaxValue)], 16 ) ^ Shift( Tinv0[(int)(IntPtr)((num2 >> 24) & byte.MaxValue)], 8 ) ^ numArray6[0];
            uint num12 = Tinv0[(int)(IntPtr)(num2 & byte.MaxValue)] ^ Shift( Tinv0[(int)(IntPtr)((num1 >> 8) & byte.MaxValue)], 24 ) ^ Shift( Tinv0[(int)(IntPtr)((num4 >> 16) & byte.MaxValue)], 16 ) ^ Shift( Tinv0[(int)(IntPtr)((num3 >> 24) & byte.MaxValue)], 8 ) ^ numArray6[1];
            uint num13 = Tinv0[(int)(IntPtr)(num3 & byte.MaxValue)] ^ Shift( Tinv0[(int)(IntPtr)((num2 >> 8) & byte.MaxValue)], 24 ) ^ Shift( Tinv0[(int)(IntPtr)((num1 >> 16) & byte.MaxValue)], 16 ) ^ Shift( Tinv0[(int)(IntPtr)((num4 >> 24) & byte.MaxValue)], 8 ) ^ numArray6[2];
            uint num14 = Tinv0[(int)(IntPtr)(num4 & byte.MaxValue)] ^ Shift( Tinv0[(int)(IntPtr)((num3 >> 8) & byte.MaxValue)], 24 ) ^ Shift( Tinv0[(int)(IntPtr)((num2 >> 16) & byte.MaxValue)], 16 ) ^ Shift( Tinv0[(int)(IntPtr)((num1 >> 24) & byte.MaxValue)], 8 ) ^ numArray6[3];
            uint[] numArray7 = KW[0];
            this.C0 = (uint)(Si[(int)(IntPtr)(num11 & byte.MaxValue)] ^ (Si[(int)(IntPtr)((num14 >> 8) & byte.MaxValue)] << 8) ^ (Si[(int)(IntPtr)((num13 >> 16) & byte.MaxValue)] << 16) ^ (Si[(int)(IntPtr)((num12 >> 24) & byte.MaxValue)] << 24)) ^ numArray7[0];
            this.C1 = (uint)(Si[(int)(IntPtr)(num12 & byte.MaxValue)] ^ (Si[(int)(IntPtr)((num11 >> 8) & byte.MaxValue)] << 8) ^ (Si[(int)(IntPtr)((num14 >> 16) & byte.MaxValue)] << 16) ^ (Si[(int)(IntPtr)((num13 >> 24) & byte.MaxValue)] << 24)) ^ numArray7[1];
            this.C2 = (uint)(Si[(int)(IntPtr)(num13 & byte.MaxValue)] ^ (Si[(int)(IntPtr)((num12 >> 8) & byte.MaxValue)] << 8) ^ (Si[(int)(IntPtr)((num11 >> 16) & byte.MaxValue)] << 16) ^ (Si[(int)(IntPtr)((num14 >> 24) & byte.MaxValue)] << 24)) ^ numArray7[2];
            this.C3 = (uint)(Si[(int)(IntPtr)(num14 & byte.MaxValue)] ^ (Si[(int)(IntPtr)((num13 >> 8) & byte.MaxValue)] << 8) ^ (Si[(int)(IntPtr)((num12 >> 16) & byte.MaxValue)] << 16) ^ (Si[(int)(IntPtr)((num11 >> 24) & byte.MaxValue)] << 24)) ^ numArray7[3];
        }
    }
}
