#if NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace Z80Simulator.Test
{
    [TestClass]
    public class TestLauncher
    {
        /*[TestMethod]
        public void TableGeneratorLauncher()
        {
            Z80Simulator.GenerateTables.TableGenerator gen = new Z80Simulator.GenerateTables.TableGenerator();
            gen.GenerateInstructionTypesTable();
            gen.GenerateOpCodesTable();
        }*/

        [TestMethod]
        public void RunInstructionsDefinitionTests()
        {
            TestCollection.RunInstructionsDefinitionTests();
        }

        [TestMethod]
        public void RunAssemblyTests()
        {
            TestCollection.RunAssemblyTests();
        }

        [TestMethod]
        public void RunMachineCyclesTests()
        {
            TestCollection.RunMachineCyclesTests();
        }

        [TestMethod]
        public void RunCPUTests()
        {
            TestCollection.RunCPUTests();
        }

        [TestMethod]
        public void RunALUTests()
        {
            TestCollection.RunALUTests();
        }

        [TestMethod]
        public void RunMicroInstructionsTests()
        {
            TestCollection.RunMicroInstructionsTests();
        }
                
        [TestMethod]
        public void RunFullInstructionSetTests()
        {
            TestCollection.RunFullInstructionSetTests();
        }

        [TestMethod]
        public void RunFlagsAndMemptrTests()
        {
            TestCollection.RunFlagsAndMemptrTests();
        }
    }
}
