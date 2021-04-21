using GoalballAnalysisSystem.WPF.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.WPF.Tests.Services
{
    [SetUpFixture]
    public class TestsSetup
    {
        [OneTimeSetUp]
        public async Task GlobalSetup()
        {
            var identityService = new IdentityService();
            var id = Guid.NewGuid().ToString();

            Credentials.testUsername = id + "Test";
            Credentials.testEmail = id + "test@gas.com";
            Credentials.testPassword = "Password123!";

            var response = await identityService.RegisterAsync(Credentials.testUsername, Credentials.testEmail, Credentials.testPassword);
        }
    }
}
