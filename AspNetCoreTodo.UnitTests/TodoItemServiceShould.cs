using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCoreTodo.UnitTests
{
    
    public class TodoItemServiceShould
    {  
        [Fact]
        public async Task AddNewItem()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "Test_AddNewItem").Options;

            using (var inMemoryContext = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(inMemoryContext);
                var fakeUser = new ApplicationUser { Id = "fake-000", UserName = "fake@fake" };
                await service.AddItemAsync(new NewTodoItem() { Title = "Testing?"}, fakeUser);
            }

            // Use a separate context to read the data back from the DB
            using (var inMemoryContext = new ApplicationDbContext(options))
            {
                Assert.Equal(1, await inMemoryContext.Items.CountAsync());
                var item = await inMemoryContext.Items.FirstAsync();
                Assert.Equal("Testing?", item.Title);
                Assert.False(item.IsDone);
                Assert.True(DateTimeOffset.Now.AddDays(3) - item.DueAt < TimeSpan.FromSeconds(1));
            }
        }

        [Fact]
        public async Task MarksDoneFalse()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "Test_MarksDoneFalse").Options;

            using (var inMemoryContext = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(inMemoryContext);
                var fakeUser = new ApplicationUser { Id = "fake-000", UserName = "fake@fake" };
                var retorno = await service.MarkDoneAsync(Guid.NewGuid(), fakeUser);

                Assert.False(retorno);
            }
        }



        [Fact]
        public async Task MarksDoneTrue()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "Test_MarksDoneTrue").Options;

            using (var inMemoryContext = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(inMemoryContext);
                var fakeUser = new ApplicationUser { Id = "fake-000", UserName = "fake@fake" };
                await service.AddItemAsync(new NewTodoItem() { Title = "Testing?" }, fakeUser);
            }

            // Use a separate context to read the data back from the DB
            using (var inMemoryContext = new ApplicationDbContext(options))
            {
                Assert.Equal(1, await inMemoryContext.Items.CountAsync());

                var item = await inMemoryContext.Items.FirstAsync();
                var service = new TodoItemService(inMemoryContext);
                var fakeUser = new ApplicationUser { Id = "fake-000", UserName = "fake@fake" };

                var resultado = await service.MarkDoneAsync(item.Id, fakeUser);
                Assert.True(resultado);
            }
        }






        [Fact]
        public async Task GetIncompleteItems()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "Test_GetIncompleteItems").Options;
            using (var inMemoryContext = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(inMemoryContext);
                var fakeUser = new ApplicationUser { Id = "fake-000", UserName = "fake@fake" };
                await service.AddItemAsync(new NewTodoItem() { Title = "Testing?" }, fakeUser);
            }

            // Use a separate context to read the data back from the DB
            using (var inMemoryContext = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(inMemoryContext);
                var fakeUser = new ApplicationUser { Id = "fake-000", UserName = "fake@fake" };

                IEnumerable<TodoItem> list = await service.GetIncompleteItemsAsync(fakeUser);
                var result = list.Any(x => x.IsDone == false);

                Assert.True(result);
            }
        }
    }
}