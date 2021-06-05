using System;
using System.Security.Principal;
using Xunit;

namespace TakeOwnership.Test
{
    public class UnitTest1
    {
        [Fact]
        public void DomainUserTest()
        {
            var identity = new NTAccount(Environment.UserDomainName, Environment.UserName);
            Assert.Equal($"{Environment.UserDomainName}\\{Environment.UserName}", identity.Value);
        }
    }
}