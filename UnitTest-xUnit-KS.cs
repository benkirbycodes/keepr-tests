using Xunit;
using Keepr.Services;
using Keepr.Repositories;
using System.Collections.Generic;
using Keepr.Models;
using System.Data;
using Moq;
using Keepr.Interfaces;
using System.Linq;
using Newtonsoft.Json;

namespace Keepr
{
  public class KeepsServiceTests
  {
    private readonly KeepsService keepsService;
    private readonly Mock<IKeepsRepository<Keep>> moqKeepsRepository;


    public KeepsServiceTests() //Constructor
    {
      moqKeepsRepository = new Mock<IKeepsRepository<Keep>>();
      keepsService = new KeepsService(moqKeepsRepository.Object);
    }

    [Fact] //Checks if Get returns List<Keep>
    public void CanGetResults()
    {
      moqKeepsRepository.Setup(repo => repo.Get()).Returns(new List<Keep>());

      var result = keepsService.Get();

      Assert.Equal(new List<Keep>(), result);
    }



    [Theory]
    [InlineData("newKeep1")] //Match with Name
    [InlineData("1")] // Partial match with Name
    [InlineData("it's a keep")] // Match with Desc
    [InlineData("it's ")] // Partial Match with Desc

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

      Assert.Same(dataList[0], resultList[0]);
      //Issue: Equals failing because it checked for Reference, so cast to List<Keep> and used Same. 
    }

    [Fact] //Checks if GetPrivate returns private keep
    public void CanGetPrivate()
    {
      moqKeepsRepository.Setup(repo => repo.GetPrivate()).Returns(new List<Keep>{
          new Keep {IsPrivate= true}
        });
      var result = keepsService.GetPrivate();

      Assert.True(result.First().IsPrivate);
    }

    [Fact] //Tests if GetById can get by Id
    public void CanGetById()
    {
      moqKeepsRepository.Setup(repo => repo.GetById(1)).Returns(
          new Keep { Id = 1 }
        );
      var result = keepsService.GetById(1);

      Assert.Equal(1, result.Id);
      Assert.NotEqual(2, result.Id);
    }

    [Fact] // Checks if Create can create new Keep
    public void CanCreate()
    {
      Keep newKeep = new Keep { Name = "newKeep" };

      moqKeepsRepository.Setup(repo => repo.Create(newKeep)).Returns(newKeep);

      var result = keepsService.Create(newKeep);

      Assert.Equal(newKeep, result);
    }

    // [Fact] //How do I test a method that calls another method inside it? Something to do with Dependancy Injection. Pass instance of Interface as an arg?
    // public void CanEdit()
    // {

    // }


  }

}
