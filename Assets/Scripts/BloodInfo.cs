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

    /// <summary>What can he get in place</summary>
    /// <returns></returns>
    public Compatibilities Compatibility()
    {
        Compatibilities compt = new Compatibilities(BloodType.None, 0, 0, 0, 0);
        if (type == BloodType.Blood || type == BloodType.Platelet)
        {
            switch (family)
            {
                case BloodFamily.A:
                    compt = new Compatibilities(type, 1, 0, 0, 1);
                    break;
                case BloodFamily.B:
                    compt = new Compatibilities(type, 0, 1, 0, 1);
                    break;
                case BloodFamily.AB:
                    compt = new Compatibilities(type, 1, 1, 1, 1);
                    break;
                case BloodFamily.O:
                    compt = new Compatibilities(type, 0, 0, 0, 1);
                    break;
            }
        }
        else if (type == BloodType.Plasma)
        {
            switch (family)
            {
                case BloodFamily.A:
                    compt = new Compatibilities(type, 1, 0, 1, 0);
                    break;
                case BloodFamily.B:
                    compt = new Compatibilities(type, 0, 1, 1, 0);
                    break;
                case BloodFamily.AB:
                    compt = new Compatibilities(type, 0, 0, 1, 0);
                    break;
                case BloodFamily.O:
                    compt = new Compatibilities(type, 1, 1, 1, 1);
                    break;
            }
        }

        return compt;
    }

    public struct Compatibilities
    {
        public BloodType type;
        public int[] ababo;

        public int a { get { return ababo[0]; } set { ababo[0] = value; } }
        public int b { get { return ababo[1]; } set { ababo[1] = value; } }
        public int ab { get { return ababo[2]; } set { ababo[2] = value; } }
        public int o { get { return ababo[3]; } set { ababo[3] = value; } }

        public Compatibilities(BloodType type)
        {
            this.type = type;
            ababo = new int[] { 0, 0, 0, 0 };
        }

        public Compatibilities(BloodType type, int a, int b, int ab, int o)
        {
            this.type = type;
            ababo = new int[] { a, b, ab, o };
        }

        public void Increase(Compatibilities compt)
        {
            if (type != compt.type) return;
            ababo[0] += compt.ababo[0];
            ababo[1] += compt.ababo[1];
            ababo[2] += compt.ababo[2];
            ababo[3] += compt.ababo[3];
        }
    }
}
