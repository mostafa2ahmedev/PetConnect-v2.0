using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.DTOs;
using Microsoft.Extensions.DependencyInjection;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.Data;
using Newtonsoft.Json.Linq; // Response Message Namespace

namespace PetConnect.UnitTests
{
    public class PetControllerIntegrationTest:IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory; // Store the factory instance
        public PetControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _factory = factory;
          
        }
        [Fact]
        public async Task GetAllPets_ShouldReturnAllPetsWithStatusCode200()
        {

            // Arrange - Create complete test data with all required relationships
            #region Data Seeding

            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                //// Clear existing data
                db.Pets.RemoveRange(db.Pets);
                db.Customers.RemoveRange(db.Customers);
                db.PetBreeds.RemoveRange(db.PetBreeds);
                db.PetCategory.RemoveRange(db.PetCategory);

                // Create complete test data
                var category = new PetCategory { Id = 1, Name = "Dogs" };
                var breed = new PetBreed { Id = 1, Name = "Labrador", Category = category };
                var customer = new Customer
                {
                    Id = "test-user",
                    FName = "Test",
                    LName = "User",
                    Address = new Address
                    {
                        City = "Test City",
                        Country = "Test Country",
                        Street = "Test Street"
                    }
                };

                var pet = new Pet
                {
                    Id = 1,
                    Name = "Buddy",
                    Age = 3,
                    IsApproved = true,
                    Breed = breed,
                    ImgUrl = "test.jpg",
                    Status = DAL.Data.Enums.PetStatus.ForAdoption,
                    Notes = "Test notes",
                    CustomerAddedPets = new CustomerAddedPets { Customer = customer }
                };

                db.PetCategory.Add(category);
                db.PetBreeds.Add(breed);
                db.Customers.Add(customer);
                db.Pets.Add(pet);
                db.SaveChanges();
            }

            #endregion
            // Act
            var response = await _client.GetAsync("api/Pet/");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Read and parse the response
            #region Read the respone and assert the response with the seedingData

            var responseString = await response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(responseString); // Using Newtonsoft.Json

            // Verify the structure matches GeneralResponse
            responseJson.Should().NotBeNull();
            responseJson["statusCode"]!.Value<int>().Should().Be(200);

            // Extract and verify pets data
            var petsArray = responseJson["data"] as JArray;
            petsArray.Should().NotBeNull().And.NotBeEmpty();

            // Convert to PetDataDto for stronger assertions
            var pets = petsArray!.ToObject<List<PetDataDto>>();
            pets.Should().Contain(p => p.Name == "Buddy");

            // Verify nested properties
            var testPet = pets.First(p => p.Name == "Buddy");
            testPet.CategoryName.Should().Be("Dogs");
            testPet.CustomerName.Should().Be("Test User"); 
            #endregion
        }
    }

}

