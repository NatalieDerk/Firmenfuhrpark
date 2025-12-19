using Xunit;
using Backend.Db_tables;

namespace Fuhrpark.Tests
{
    public class UserTest
    {
        [Fact]
        public void CreateUser_ShouldHaveCorrectName()
        {
            var user = new User { IdUser = 1, Vorname = "User1", IdRolle = 3 };
            Assert.Equal("User1", user.Vorname);
        }
    }
}