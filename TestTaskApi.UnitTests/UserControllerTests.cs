﻿using System;
using System.Collections.Generic;
using System.Text;
using TestTaskApi.Controllers;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestTaskApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace TestTaskApi.UnitTests
{
    [TestFixture]
    public class UserControllerTests
    {
        [Test]
        public async Task GetUsers_ReturnsUsers()
        {
            // Arange
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(databaseName: "Test_GetUsers")
                .Options;
            var context = new UserContext(options);

                context.Users.Add(new User
                {
                    Name = "User1",
                    Surname = "Sur1",
                    BirthDate = new DateTime(2000, 02, 02)
                });
                context.Users.Add(new User
                {
                    Name = "User2",
                    Surname = "Sur2",
                    BirthDate = new DateTime(2002, 02, 22)
                });
            

            // Act 
            
            var controller = new UsersController(context);
            var result = await controller.GetUsers();


            // Assert
            var items = Assert
        }
    }
}