using System.Collections.Generic;
using Keepr.Interfaces;
using Keepr.Models;
using Keepr.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace keepr_test
{
  [TestClass]
  public class KeepsServiceTests
  {
    private KeepsService keepsService;
    private Mock<IKeepsRepository> moqKeepsRepository;

    [TestInitialize] //Constructor, more or less
    public void TestInit()
    {
      moqKeepsRepository = new Mock<IKeepsRepository>();
      keepsService = new KeepsService(moqKeepsRepository.Object);

    }

    [TestMethod] //Checks if Get returns List<Keep>
    public void CanGetResults()
    {
      var list = new List<Keep>();
      moqKeepsRepository.Setup(repo => repo.Get()).Returns(list);

      var result = keepsService.Get();

      moqKeepsRepository.Verify(repo => repo.Get());
      Assert.AreEqual(list, result);

    }

    [DataTestMethod]
    [DataRow("newKeep1")] //Match with Name
    [DataRow("1")] // Partial match with Name
    [DataRow("it's a keep")] // Match with Desc
    [DataRow("it's ")] // Partial Match with Desc
    public void CanGetFiltered(string filter)
    {
      var data = new List<Keep>
    {
      new Keep {Name = "newKeep1", Description = "it's a keep"},
      new Keep {Name = "newKeep2", Description = string.Empty}
    };
      moqKeepsRepository.Setup(repo => repo.Get()).Returns(data);

      var result = keepsService.GetFiltered(filter);

      var dataList = new List<Keep>(data);
      var resultList = new List<Keep>(result);

      moqKeepsRepository.Verify(repo => repo.Get());
      Assert.AreSame(dataList[0], resultList[0]);
    }



    [TestMethod] //Checks if GetPrivate returns private keep
    public void CanGetPrivate()
    {
      moqKeepsRepository.Setup(repo => repo.GetPrivate()).Returns(new List<Keep>{
          new Keep {IsPrivate= true}
        });
      var result = keepsService.GetPrivate();

      var resultList = new List<Keep>(result);

      moqKeepsRepository.Verify(repo => repo.GetPrivate());

      Assert.IsTrue(resultList[0].IsPrivate);
    }

    [TestMethod] //Tests if GetById can get by Id
    public void CanGetById()
    {
      moqKeepsRepository.Setup(repo => repo.GetById(1)).Returns(
    new Keep { Id = 1 }
  );
      var result = keepsService.GetById(1);

      moqKeepsRepository.Verify(repo => repo.GetById(1));
      Assert.AreEqual(1, result.Id);
      Assert.AreNotEqual(2, result.Id);

    }

    [TestMethod] // Checks if Create can create new Keep
    public void CanCreate()
    {
      Keep newKeep = new Keep { Name = "newKeep" };

      moqKeepsRepository.Setup(repo => repo.Create(newKeep)).Returns(newKeep);

      var result = keepsService.Create(newKeep);

      moqKeepsRepository.Verify(repo => repo.Create(newKeep));
      Assert.AreEqual(newKeep, result);
    }

    [TestMethod]
    public void CanEdit()
    {
      Keep newKeep = new Keep { Name = "newKeep", Id = 1 };
      Keep update = new Keep { Name = "newKeepEdited", Id = 1 };


      moqKeepsRepository.Setup(repo => repo.GetById(1)).Returns(newKeep);
      moqKeepsRepository.Setup(repo => repo.Edit(update));

      var result = keepsService.Edit(update);

      Assert.AreSame(update, result);
    }

    [TestMethod]
    public void CanDelete()
    {
      Keep keepToDelete = new Keep { Name = "keepToDelete", Id = 1 };

      moqKeepsRepository.Setup(repo => repo.GetById(1)).Returns(keepToDelete);
      moqKeepsRepository.Setup(repo => repo.Delete(1));

      var result = keepsService.Delete(1);

      Assert.AreEqual("Successfully Deleted", result);
    }





  }
}
