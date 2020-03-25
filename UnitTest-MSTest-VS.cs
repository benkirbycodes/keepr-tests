using Keepr.Interfaces;
using Keepr.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace keepr_test

{
  [TestClass]
  public class VaultsServiceTests
  {
    private VaultsService vaultsService;
    private Mock<IVaultsRepository> moqVaultsRepository;

    [TestInitialize]
    public void TestInit()
    {
      moqVaultsRepository = new Mock<IVaultsRepository>();
      vaultsService = new VaultsService(moqVaultsRepository.Object);
    }
  }
}