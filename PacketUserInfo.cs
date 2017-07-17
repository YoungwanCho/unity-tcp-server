using System;
using System.Text;
using NetworkLibrary;
using NetworkLibrary.PacketType;

public class PacketUserInfo : Packet
{
    private PString name = new PString();
    private PInteger age = new PInteger();
    
    private PByte pbyte = new PByte();
    private PSByte psbyte = new PSByte();
    private PShort pshort = new PShort();
    private PInteger pinteger = new PInteger();
    private PUInteger puinteger = new PUInteger();
    private PFloat pfloat = new PFloat();
    private PLong plong = new PLong();
    private PULong pulong = new PULong();

    public PacketUserInfo(int sup, int sub) : base(sup, sub, 10)
    {
        _field[0] = name;
        _field[1] = age;

        _field[2] = pbyte;
        _field[3] = psbyte;
        _field[4] = pshort;
        _field[5] = pinteger;
        _field[6] = puinteger;
        _field[7] = pfloat;
        _field[8] = plong;
        _field[9] = pulong;
     }

    public void InitPacketUserInfo()
    {
        name.str = "Cho";
        age.n = 12;

        pbyte.n = 100;
        psbyte.n = 101;
        pshort.n = 102;
        pinteger.n = 103;
        puinteger.n = 104;
        pfloat.f = 105;
        plong.n = 106;
        pulong.n = 107;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(base.ToString());

        sb.Append(string.Format("Name : {0}, Age : {1}, pbyte : {2}, psbyte : {3}, pshort : {4}, pinteger : {5}, puinteger : {6}, pfloat : {7}, plong : {8} pulong : {9}",
            name,
            age,
            pbyte,
            psbyte,
            pshort,
            pinteger,
            puinteger,
            pfloat,
            plong,
            pulong
            ));

        return sb.ToString();
    }
}