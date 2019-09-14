using Xunit;

namespace Z80Simulator.Test
{
    public class TestLauncher
    {
        /*[Fact]
        public void TableGeneratorLauncher()
        {
            Z80Simulator.GenerateTables.TableGenerator gen = new Z80Simulator.GenerateTables.TableGenerator();
            gen.GenerateInstructionTypesTable();
            gen.GenerateOpCodesTable();
        }*/

        [Fact]
        public void RunInstructionsDefinitionTests()
        {
            TestCollection.RunInstructionsDefinitionTests();
        }

        [Fact]
        public void RunAssemblyTests()
        {
            TestCollection.RunAssemblyTests();
        }

        [Fact]
        public void RunMachineCyclesTests()
        {
            TestCollection.RunMachineCyclesTests();
        }

        [Fact]
        public void RunCPUTests()
        {
            TestCollection.RunCPUTests();
        }

        [Fact]
        public void RunALUTests()
        {
            TestCollection.RunALUTests();
        }

        [Fact]
        public void RunMicroInstructionsTests()
        {
            TestCollection.RunMicroInstructionsTests();
        }
                
        [Fact]
        public void RunFullInstructionSetTests()
        {
            TestCollection.RunFullInstructionSetTests();
        }

        [Fact]
        public void RunFlagsAndMemptrTests()
        {
            TestCollection.RunFlagsAndMemptrTests();
        }
    }
}
