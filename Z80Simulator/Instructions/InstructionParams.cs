using System;

namespace Z80Simulator.Instructions
{
    public enum AddressingMode
    {
        Bit,
        Extended,
        FlagCondition,
        Flags,
        Immediate,
        Immediate16,
        IOPortImmediate,
        IOPortRegister,
        Indexed,
        InterruptMode,
        ModifiedPageZero,
        Register,
        Register16,
        RegisterIndirect,
        Relative
    }

    public enum Register
    {
        A,
        B,
        C,
        D,
        E,
        H,
        L,
        IXh,
        IXl,
        IYh,
        IYl,
        I,
        R
    }

    public enum Flags
    {
        F
    }

    public enum FlagCondition
    {
        C,
        M,
        NC,
        NZ,
        P,
        PE,
        PO,
        Z
    }

    public enum Register16
    {
        BC,
        DE,
        HL,
        IX,
        IY,
        SP,
        AF,
        AF2
    }

    public static class RegisterUtils
    {
        public static Register GetLowerPart(Register16 register16)
        {
            switch (register16)
            {
                case Register16.BC:
                    return Register.C;
                case Register16.DE:
                    return Register.E;
                case Register16.HL:
                    return Register.L;
                case Register16.IX:
                    return Register.IXl;
                case Register16.IY:
                    return Register.IYl;
                case Register16.SP:
                case Register16.AF:
                case Register16.AF2:
                default:
                    throw new InvalidOperationException();
            }
        }

        public static Register GetHigherPart(Register16 register16)
        {
            switch (register16)
            {
                case Register16.BC:
                    return Register.B;
                case Register16.DE:
                    return Register.D;
                case Register16.HL:
                    return Register.H;
                case Register16.IX:
                    return Register.IXh;
                case Register16.IY:
                    return Register.IYh;
                case Register16.SP:
                case Register16.AF:
                case Register16.AF2:
                default:
                    throw new InvalidOperationException();
            }
        }

        public static Register16 GetFromAddressIndexBase(AddressIndexBase index)
        {
            if (index == AddressIndexBase.IX)
            {
                return Register16.IX;
            }
            else
            {
                return Register16.IY;
            }
        }
    }

    public enum Bit
    {
        b0,
        b1,
        b2,
        b3,
        b4,
        b5,
        b6,
        b7
    }

    public static class BitUtils
    {
        public static byte ToByte(Bit bitEnum)
        {
            switch(bitEnum)
            {
                case Bit.b0:
                    return 0;
                case Bit.b1:
                    return 1;
                case Bit.b2:
                    return 2;
                case Bit.b3:
                    return 3;
                case Bit.b4:
                    return 4;
                case Bit.b5:
                    return 5;
                case Bit.b6:
                    return 6;                
                //case Bit.b7:
                default:
                    return 7;
            }
        }
    }

    public class AddressExtended
    {
        public AddressExtended(byte lsb, byte msb)
        {
            Address = (short)(msb << 8 + lsb);
        }

        public short Address { get; private set; }
    }

    public enum AddressRegisterIndirect
    {
        BC,
        DE,
        HL,
        IX,
        IY,
        SP
    }

    public enum AddressIndexBase
    {
        IX,
        IY
    }

    public class AddressIndexed
    {
        public AddressIndexed(AddressIndexBase indexBase, sbyte displacement)
        {
            IndexBase = AddressIndexBase.IX;
            Displacement = displacement;
        }

        public AddressIndexBase IndexBase { get; private set; }
        public sbyte Displacement { get; private set; }
    }

    public class AddressRelative
    {
        public sbyte Displacement { get; set; }
    }

    public enum AddressModifiedPageZero
    {
        rst00,
        rst08,
        rst10,
        rst18,
        rst20,
        rst28,
        rst30,
        rst38
    }

    public class IOPortImmediate
    {
        byte IOPort { get; set; }
    }

    public enum IOPortRegister
    {
        C
    }

    public class ValueImmediate
    {
        byte Value { get; set; }
    }

    public class ValueImmediate16
    {
        public ValueImmediate16(byte lsb, byte msb)
        {
            Value = (short)(msb << 8 + lsb);
        }

        short Value { get; set; }
    }

    public enum InterruptMode
    {
        IM0,
        IM1,
        IM2
    }    
}
