using Z80Simulator.CPU;

namespace Z80Simulator.System
{
    public class Clock
    {
        public const long FREQ_3_5MHZ = 3500000L;

        public Clock(long frequencyHerz)
        {
            SimulatedFrequencyHerz = frequencyHerz;
            SimulatedPeriodNanoseconds = 1000000000L/frequencyHerz;

            SignalState beforeInitialState = SignalState.LOW;                
            _clk = beforeInitialState;
        }

        public long SimulatedFrequencyHerz { get; private set; }
        public long SimulatedPeriodNanoseconds { get; private set; }

        public virtual SignalState CLK
        {
            get { return _clk; }
            protected set { _clk = value; }
        }
        protected SignalState _clk;

        public void HalfTick()
        {
            CLK = CLK == SignalState.LOW ? SignalState.HIGH : SignalState.LOW;
        }

        public void Tick()
        {
            CLK = SignalState.HIGH;
            CLK = SignalState.LOW;            
        }

        public void Tick(int ticksCount)
        {
            for (int i = 0; i < ticksCount; i++)
            {
                Tick();
            }
        }

        public Z80CPU.IExitCondition TickUntilExitCondition()
        {
            try
            {
                for (; ; ) { Tick(); }
            }
            catch(Z80CPU.ExitConditionException exitConditionException)
            {
                return exitConditionException.ExitCondition;
            }
        }
    }
}