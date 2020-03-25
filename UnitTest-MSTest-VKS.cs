using Keepr.Interfaces;
using Keepr.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace keepr_test

{
  [TestClass]
  public class VaultKeepsServiceTests
  {
    private VaultKeepsService vaultKeepsService;
    private Mock<IVaultKeepsRepository> moqVaultKeepsRepository;
    private Mock<IKeepsRepository> moqKeepsRepository;

    [TestInitialize]
    public void TestInit()
    {
      moqVaultKeepsRepository = new Mock<IVaultKeepsRepository>();
      vaultKeepsService = new VaultKeepsService(moqVaultKeepsRepository.Object, moqKeepsRepository.Object);
    }
  }
}