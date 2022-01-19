using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Catalog.Api.Controllers;
using Catalog.Api.Dtos;
using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Catalog.UnitTests;

public class ItemsControllerTests
{
    private readonly Random rand = new();
    private readonly Mock<IItemsRepository> repositoryStub = new();
    private readonly Mock<ILogger<ItemsController>> loggerStub = new ();
    
    [Fact]
    public void UnitOfWork_StateUnderTest_ExpectedBehavior()
    {
        
    }
    [Fact]
    public async Task GetItemAsync_WithUnexistingItem_ReturnNotFound()
    {
        // Arrange
         repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
         .ReturnsAsync((Item)null);
         var controller = new ItemsController(repositoryStub.Object , loggerStub.Object);
        // Act
         var result = await controller.GetItemAsync(Guid.NewGuid());
        // Assert
         Assert.IsType<NotFoundResult>(result.Result);
         result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem()
    {
        // Arrange
         Item expectedItem = CreateRandomItem();
         repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
         .ReturnsAsync((expectedItem));
         var controller = new ItemsController(repositoryStub.Object,loggerStub.Object);
        // Act
        var result = await controller.GetItemAsync(Guid.NewGuid());
        // Assert

        result.Value.Should().BeEquivalentTo(
            expectedItem);
        //Assert.IsType<ItemDto>(result.Value);
        //var dto = (result as ActionResult<ItemDto>).Value;
        //Assert.Equal(expectedItem.Id,dto.Id);
        //Assert.Equal(expectedItem.Name,dto.Name);
    }

    [Fact]
    public async Task GetItemsAsync_WithExistingItems_ReturnsAllItems()
    {
        //Arrange
         var expectedItems= new[]{ CreateRandomItem(),CreateRandomItem(),CreateRandomItem()};
         repositoryStub.Setup(repo=>repo.GetItemsAsync())
         .ReturnsAsync(expectedItems);
         var controller = new ItemsController(repositoryStub.Object,loggerStub.Object);
        //Act
        var actualItems = await controller.GetItemsAsync();
        //Assert
        actualItems.Should().BeEquivalentTo(
            expectedItems);
    }

    [Fact]
    public async Task GetItemsAsync_WithMatchingItems_ReturnsMatchingItems()
    {
        //Arrange
         var allItems= new[]
         { 
             new Item(){ Name = "Potion"},
             new Item(){ Name= "Antidote"},
             new Item(){ Name = "Hi-Potion"},
        };
        var nameToMatch = "Potion";
         repositoryStub.Setup(repo=>repo.GetItemsAsync())
         .ReturnsAsync(allItems);
         var controller = new ItemsController(repositoryStub.Object,loggerStub.Object);
        //Act
        IEnumerable<ItemDto>  foundItems = await controller.GetItemsAsync(nameToMatch);
        //Assert
        foundItems.Should().OnlyContain(
            item => item.Name == allItems[0].Name || item.Name == allItems[2].Name
        );
    }
    [Fact]
    public async Task CreateItemAsync_WithItemToCreate_ReturnsCreated()
    {
        //Arrange
        var itemToCreate= new CreateItemDto(
            Guid.NewGuid().ToString(),
            "descript",
            rand.Next(1000)
        );
        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);
        //Act
        var result = await controller.CreateItemAsync(itemToCreate);
        //Assert
        var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDto;;   
        itemToCreate.Should().BeEquivalentTo(
            createdItem,
            options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers()
        );
        createdItem.Id.Should().NotBeEmpty();
        createdItem.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000.Seconds());
    }
       [Fact]
    public async Task UpdateItemAsync_WithExistingItem_ReturnsNoContent()
    {
        //Arrange
        Item existingItem = CreateRandomItem();

        repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
         .ReturnsAsync((existingItem));

        var itemId = existingItem.Id;
        var itemToUpdate= new UpdateItemDto(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            existingItem.Price +3);
        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

         //Act
        var result = await controller.UpdateItemAsync(itemId,itemToUpdate);

         //Assert
         result.Should().BeOfType<NoContentResult>();
    }
    [Fact]
    public async Task DeleteItemAsync_WithExistingItem_ReturnsNoContent()
    {
        //Arrange
        Item existingItem = CreateRandomItem();
        
        repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
         .ReturnsAsync((existingItem));

     
        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

         //Act
        var result = await controller.DeleteItemAsync(existingItem.Id);

         //Assert
         result.Should().BeOfType<NoContentResult>();
    }
       [Fact]
    public async Task GetItemsAsync_WithItemToCreate_ReturnsCreated()
    {

    }
    private Item CreateRandomItem()
    {
        return new Item()
        {
            Id= Guid.NewGuid(),
            Name= Guid.NewGuid().ToString(),
            Description= "SDsdsd",
            Price = rand.Next(1000),
            CreatedDate = DateTimeOffset.UtcNow
        };
    }
}