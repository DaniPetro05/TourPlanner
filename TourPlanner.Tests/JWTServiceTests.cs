using NUnit.Framework;
using TourPlanner.Services;
using TourPlanner.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace TourPlanner.Tests;

[TestFixture]
public class JWTServiceTests
{
    private JWTService _service;

    [SetUp]
    public void Setup()
    {
        /*var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                {"Jwt:Key", "THIS_IS_A_SUPER_SECRET_TEST_KEY_12345"},
                {"Jwt:Issuer", "test"},
                {"Jwt:Audience", "test"}
            })
            .Build();*/
        
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string?>("Jwt:Key", "THIS_IS_A_SUPER_SECRET_TEST_KEY_12345"),
                new KeyValuePair<string, string?>("Jwt:Issuer", "test"),
                new KeyValuePair<string, string?>("Jwt:Audience", "test")
            })
            .Build();

        _service = new JWTService(config);
    }

    [Test]
    public void Token_ShouldBeCreated()
    {
        var user = new User { Id = 1, Username = "test" };

        var token = _service.CreateToken(user);

        Assert.That(token, Is.Not.Null);
    }

    [Test]
    public void Token_ShouldContainUserId()
    {
        var user = new User { Id = 1, Username = "test" };

        var token = _service.CreateToken(user);
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        Assert.That(jwt.Claims.Any(c => c.Type.Contains("nameidentifier")), Is.True);
    }

    [Test]
    public void Token_ShouldContainUsername()
    {
        var user = new User { Id = 1, Username = "test" };

        var token = _service.CreateToken(user);
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        Assert.That(jwt.Claims.Any(c => c.Value == "test"), Is.True);
    }

    [Test]
    public void Token_ShouldHaveExpiration()
    {
        var user = new User { Id = 1, Username = "test" };

        var token = _service.CreateToken(user);
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        Assert.That(jwt.ValidTo, Is.GreaterThan(System.DateTime.UtcNow));
    }
}