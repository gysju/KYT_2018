using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BloodInfo
{
    public enum BloodType { None, Blood, Platelet, Plasma};
    public enum BloodFamily { None, A, B, AB, O}
    public enum BloodRhesus { None, neg, pos }

    public BloodType type = BloodType.None;
    public BloodFamily family = BloodFamily.None;
    public BloodRhesus rhesus = BloodRhesus.None;

    public BloodInfo(BloodType type, BloodFamily family, BloodRhesus rhesus)
    {
        this.type = type;
        this.family = family;
        this.rhesus = rhesus;
    }

    public bool Equals(BloodInfo info, bool c_type, bool c_family, bool c_rhesus)
    {
        bool equal = true;
        if (c_type && type != info.type)
            equal = false;
        else if (c_family && family != info.family)
            equal = false;
        else if (c_rhesus && rhesus != info.rhesus)
            equal = false;

        return equal;
    }
    public bool EqualsType(BloodInfo info, bool c_type, bool c_family)
    {
        return Equals(info, c_type, c_family, false);
    }
    public bool EqualsType(BloodInfo info, bool c_type)
    {
        return Equals(info, c_type, true, true);
    }
    public bool EqualsType(BloodInfo info)
    {
        return Equals(info, true, true, true);
    }

    public static BloodInfo GetRand()
    {
        BloodType type = (BloodType)Random.Range(1, 4);
        BloodFamily fam = (BloodFamily)Random.Range(1, 5);
        BloodRhesus rhe = (BloodRhesus)Random.Range(1, 5);

        return new BloodInfo(type, fam, rhe);
    }

    /// <summary>What can he get in place</summary>
    /// <returns></returns>
    public Compoatibilities Compatibility()
    {
        Compoatibilities compt = new Compoatibilities(0,0,0,0);
        if (type == BloodType.Blood)
        {
            switch (family)
            {
                case BloodFamily.A:
                    compt = new Compoatibilities(1, 0, 0, 1);
                    break;
                case BloodFamily.B:
                    compt = new Compoatibilities(0, 1, 0, 1);
                    break;
                case BloodFamily.AB:
                    compt = new Compoatibilities(1, 1, 1, 1);
                    break;
                case BloodFamily.O:
                    compt = new Compoatibilities(0, 0, 0, 1);
                    break;
            }
        }
        else if (type == BloodType.Plasma)
        {
            switch (family)
            {
                case BloodFamily.A:
                    compt = new Compoatibilities(1, 0, 0, 1);
                    break;
                case BloodFamily.B:
                    compt = new Compoatibilities(0, 1, 0, 1);
                    break;
                case BloodFamily.AB:
                    compt = new Compoatibilities(1, 1, 1, 1);
                    break;
                case BloodFamily.O:
                    compt = new Compoatibilities(0, 0, 0, 1);
                    break;
            }
        }
        else if (type == BloodType.Platelet)
        {
            switch (family)
            {
                case BloodFamily.A:
                    compt = new Compoatibilities(1, 0, 0, 1);
                    break;
                case BloodFamily.B:
                    compt = new Compoatibilities(0, 1, 0, 1);
                    break;
                case BloodFamily.AB:
                    compt = new Compoatibilities(1, 1, 1, 1);
                    break;
                case BloodFamily.O:
                    compt = new Compoatibilities(0, 0, 0, 1);
                    break;
            }
        }

        return compt;
    }

    public struct Compoatibilities
    {
        public int a, b, ab, o;

        public Compoatibilities(int a, int b, int ab, int o)
        {
            this.a = a;
            this.b = b;
            this.ab = ab;
            this.o = o;
        }
    }
}
