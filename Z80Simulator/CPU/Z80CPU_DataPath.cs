using System;
using Z80Simulator.Instructions;

namespace Z80Simulator.CPU
{
    /// <summary>
    /// ZiLOG Z80 CPU
    /// 
    /// Internal Data Path
    /// 
    /// Internal buses, temporary storage, and micro instructions used to move data inside the cpu.
    /// </summary>
    public partial class Z80CPU
    {
        #region Internal Buses

        /// <summary>
        /// Internal Data Bus (8 bits)
        /// </summary>
        private byte InternalDataBus { get; set; }

        /// <summary>
        /// Internal Address Bus (16 bits)
        /// </summary>
        private ushort InternalAddressBus { get; set; }

        #endregion

        #region Micro-instructions

        #region Internal Data Bus

        private enum InternalDataBusConnection
        {
            DataBus,
            IR,
            W,
            Z,
            ALURightBuffer,
            SPh,
            SPl,
            F,
            PCh,
            PCl
        }

        private void InternalDataBus_SampleFrom(InternalDataBusConnection source)
        {
            switch (source)
            {
                case InternalDataBusConnection.DataBus:
                    InternalDataBus = Data.SampleValue();
                    break;
                case InternalDataBusConnection.W:
                    InternalDataBus = W;
                    break;
                case InternalDataBusConnection.Z:
                    InternalDataBus = Z;
                    break;
                case InternalDataBusConnection.ALURightBuffer:
                    InternalDataBus = ALURightBuffer;
                    break;
                case InternalDataBusConnection.SPh:
                    InternalDataBus = SPh;
                    break;
                case InternalDataBusConnection.SPl:
                    InternalDataBus = SPl;
                    break;
                case InternalDataBusConnection.F:
                    InternalDataBus = F;
                    break;
                case InternalDataBusConnection.PCh:
                    InternalDataBus = PCh;
                    break;
                case InternalDataBusConnection.PCl:
                    InternalDataBus = PCl;
                    break;
                default:
                    throw new NotImplementedException();
            }
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction( new MicroInstruction(Z80MicroInstructionTypes.InternalDataBusSampleFrom, source)  );
            }
        }

        private void InternalDataBus_SampleFrom(Register source)
        {
            switch (source)
            {
                case Register.A:
                    InternalDataBus = A;
                    break;
                case Register.B:
                    InternalDataBus = B;
                    break;
                case Register.C:
                    InternalDataBus = C;
                    break;
                case Register.D:
                    InternalDataBus = D;
                    break;
                case Register.E:
                    InternalDataBus = E;
                    break;
                case Register.H:
                    InternalDataBus = H;
                    break;
                case Register.L:
                    InternalDataBus = L;
                    break;
                case Register.IXh:
                    InternalDataBus = IXh;
                    break;
                case Register.IXl:
                    InternalDataBus = IXl;
                    break;
                case Register.IYh:
                    InternalDataBus = IYh;
                    break;
                case Register.IYl:
                    InternalDataBus = IYl;
                    break;
                case Register.I:
                    InternalDataBus = I;
                    break;
                case Register.R:
                    InternalDataBus = R;
                    break;
                default:
                    throw new NotImplementedException();
            }
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.InternalDataBusSampleFrom, source));
            }
        }

        private void InternalDataBus_SendTo(InternalDataBusConnection target)
        {
            switch (target)
            {
                case InternalDataBusConnection.DataBus:
                    Data.SetValue(InternalDataBus);
                    break;
                case InternalDataBusConnection.IR:
                    IR = InternalDataBus;
                    break;
                case InternalDataBusConnection.W:
                    W = InternalDataBus;
                    break;
                case InternalDataBusConnection.Z:
                    Z = InternalDataBus;
                    break;
                case InternalDataBusConnection.ALURightBuffer:
                    ALURightBuffer = InternalDataBus;
                    break;
                case InternalDataBusConnection.SPh:
                    SPh = InternalDataBus;
                    break;
                case InternalDataBusConnection.SPl:
                    SPl = InternalDataBus;
                    break;
                case InternalDataBusConnection.F:
                    F = InternalDataBus;
                    break;
                case InternalDataBusConnection.PCh:
                    PCh = InternalDataBus;
                    break;
                case InternalDataBusConnection.PCl:
                    PCl = InternalDataBus;
                    break;
                default:
                    throw new NotImplementedException();
            }
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.InternalDataBusSendTo, target));
                if (target == InternalDataBusConnection.DataBus)
                {
                    TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.BusConnectorSetValue, target));
                }
            }
        }        

        private void InternalDataBus_SendTo(Register target)
        {
            switch (target)
            {
                case Register.A:
                    A = InternalDataBus;
                    break;
                case Register.B:
                    B = InternalDataBus;
                    break;
                case Register.C:
                    C = InternalDataBus;
                    break;
                case Register.D:
                    D = InternalDataBus;
                    break;
                case Register.E:
                    E = InternalDataBus;
                    break;
                case Register.H:
                    H = InternalDataBus;
                    break;
                case Register.L:
                    L = InternalDataBus;
                    break;
                case Register.IXh:
                    IXh = InternalDataBus;
                    break;
                case Register.IXl:
                    IXl = InternalDataBus;
                    break;
                case Register.IYh:
                    IYh = InternalDataBus;
                    break;
                case Register.IYl:
                    IYl = InternalDataBus;
                    break;
                case Register.I:
                    I = InternalDataBus;
                    break;
                case Register.R:
                    R = InternalDataBus;
                    break;
                default:
                    throw new NotImplementedException();
            }
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.InternalDataBusSendTo, target));
            }
        }
        
        private void InternalDataBus_Reset()
        {
            InternalDataBus = 0;
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.InternalDataBusReset));
            }
        }

        #endregion

        #region Internal Address Bus

        private enum InternalAddressBusConnection
        {
            AddressBus,
            PC,
            SP,
            IandR,
            WZ,
            IX,
            IY
        }

        private void InternalAddressBus_SampleFrom(InternalAddressBusConnection source)
        {
            switch (source)
            {
                case InternalAddressBusConnection.PC:
                    InternalAddressBus = PC;
                    break;
                case InternalAddressBusConnection.SP:
                    InternalAddressBus = SP;                    
                    break;
                case InternalAddressBusConnection.WZ:
                    InternalAddressBus = WZ;
                    break;
                case InternalAddressBusConnection.IX:
                    InternalAddressBus = IX;
                    break;
                case InternalAddressBusConnection.IY:
                    InternalAddressBus = IY;
                    break;
                case InternalAddressBusConnection.IandR:
                    // During every first machine cycle (beginning of an instruction or part of it, prefixes have their
                    // own M1 two), the memory refresh cycle is issued. The whole I+R register is put on the address
                    // bus, and the RFSH pin is lowered. It is unclear whether the Z80 increases the R register before
                    // or after putting I+R on the bus.
                    InternalAddressBus = (ushort)((I << 8) + R);
                    break;
                default:
                    throw new NotImplementedException();
            }
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.InternalAddressBusSampleFrom, source));
            }
        }

        private void InternalAddressBus_SampleFrom(Register16 source)
        {
            switch (source)
            {
                case Register16.BC:
                    InternalAddressBus = BC;
                    break;
                case Register16.DE:
                    InternalAddressBus = DE;
                    break;
                case Register16.HL:
                    InternalAddressBus = HL;
                    break;
                case Register16.IX:
                    InternalAddressBus = IX;
                    break;
                case Register16.IY:
                    InternalAddressBus = IY;
                    break;
                default:
                    throw new NotImplementedException();
            }
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.InternalAddressBusSampleFrom, source));
            }
        }

        private void InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection source)
        {
            switch (source)
            {
               case InternalAddressBusConnection.PC:
                    // The PC is automatically incremented after its contents have been transferred to the address lines
                    InternalAddressBus_SampleFrom(InternalAddressBusConnection.PC);
                    Register16_Increment(InternalAddressBusConnection.PC);                 
                    break;
               case InternalAddressBusConnection.SP:
                    // The stack pointer is automatically incremented after its contents have been transferred to the address lines
                    InternalAddressBus_SampleFrom(InternalAddressBusConnection.SP);
                    Register16_Increment(InternalAddressBusConnection.SP);
                    break;
               case InternalAddressBusConnection.WZ:
                    InternalAddressBus_SampleFrom(InternalAddressBusConnection.WZ);
                    Register16_Increment(InternalAddressBusConnection.WZ);
                    break;
                case InternalAddressBusConnection.IandR:
                    // During every first machine cycle (beginning of an instruction or part of it, prefixes have their
                    // own M1 two), the memory refresh cycle is issued. The whole I+R register is put on the address
                    // bus, and the RFSH pin is lowered. It is unclear whether the Z80 increases the R register before
                    // or after putting I+R on the bus.    
                    InternalAddressBus_SampleFrom(InternalAddressBusConnection.IandR);                    
                    // The R register is increased at every first machine cycle (M1). Bit 7 of the register is never
                    // changed by this.
                    RegisterR_Increment();
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void InternalAddressBus_SampleFromAndDecrement(InternalAddressBusConnection source)
        {
            switch (source)
            {
                case InternalAddressBusConnection.WZ:
                    InternalAddressBus_SampleFrom(InternalAddressBusConnection.WZ);
                    Register16_Decrement(InternalAddressBusConnection.WZ);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void InternalAddressBus_SampleFromPCAndIncrementForInstruction()
        {
            // The PC is automatically incremented after its contents have been transferred to the address lines
            if (instructionOrigin.Source != InstructionSource.Internal)
            {
                InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.PC);
            }
            // Internal CPU instructions : do not increment PC
            else
            {
                InternalAddressBus_SampleFrom(InternalAddressBusConnection.PC);
            }
        }

        private void InternalAddressBus_DecrementAndSampleFrom(InternalAddressBusConnection source)
        {
            switch (source)
            {
                case InternalAddressBusConnection.SP:
                    // The stack pointer is automatically decremented before its contents are transferred to the address lines
                    Register16_Decrement(Register16.SP);
                    InternalAddressBus_SampleFrom(InternalAddressBusConnection.SP);                    
                    break;                
                default:
                    throw new NotImplementedException();
            }            
        }

        private void InternalAddressBus_SampleFromRegisterWZPlusDisplacement()
        {
            WZ = (ushort)(WZ + (sbyte)InternalDataBus);
            InternalAddressBus = WZ;

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.InternalAddressBusSampleFromRegisterPlusDisplacement, InternalAddressBusConnection.WZ));
            }
        }

        private void InternalAddressBus_SendTo(InternalAddressBusConnection target)
        {
            switch (target)
            {
                case InternalAddressBusConnection.AddressBus:
                    Address.SetValue(InternalAddressBus);
                    break;
                case InternalAddressBusConnection.PC:
                    PC = InternalAddressBus;
                    break;
                case InternalAddressBusConnection.WZ:
                    WZ = InternalAddressBus;
                    break;
                default:
                    throw new NotImplementedException();
            }
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.InternalAddressBusSendTo, target));
                if (target == InternalAddressBusConnection.AddressBus)
                {
                    TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.BusConnectorSetValue, target));                    
                }
            }
        }

        private void SendOperandToPC_IfFlagIsSet(FlagCondition flagCondition)
        {
            if (CheckFlagCondition(flagCondition))
            {
                InternalAddressBus_SampleFrom(InternalAddressBusConnection.WZ);
                InternalAddressBus_SendTo(InternalAddressBusConnection.PC);
            }

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.SendOperandToPCIfFlagIsSet, flagCondition));
            }
        }   

        #endregion

        #region ALU Data Path

        private enum ALULeftBufferConnection
        {
            A,
            W,
            Z
        }

        private void ALULeftBuffer_SampleFrom(ALULeftBufferConnection source)
        {
            switch (source)
            {
                case ALULeftBufferConnection.A:
                    ALULeftBuffer = A;
                    break;
                case ALULeftBufferConnection.W:
                    ALULeftBuffer = W;
                    break;
                case ALULeftBufferConnection.Z:
                    ALULeftBuffer = Z;
                    break;
                default:
                    throw new NotImplementedException();
            }
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.ALULeftBufferSampleFrom, source));
            }
        }

        private void ALULeftBuffer_SendTo(ALULeftBufferConnection target)
        {
            switch (target)
            {
                case ALULeftBufferConnection.A:
                    A = ALULeftBuffer;
                    break;
                default:
                    throw new NotImplementedException();
            }
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.ALULeftBufferSendTo, target));
            }
        }

        private void ALULeftBuffer_Reset()
        {
            ALULeftBuffer = 0;
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.ALULeftBufferReset));
            }
        }

        #endregion

        #endregion
    }
}
