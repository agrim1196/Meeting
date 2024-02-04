using JWTAuth.WebApi.Models;
using MeetingPlannerAPI.Controllers;
using MeetingPlannerAPI.DAL;
using MeetingPlannerAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Sieve.Models;
using Sieve.Services;
using System.Data.Entity;
using WebApi.Models;
using WebApi.Services;

namespace MeetingPlanner.Tests
{
    [TestFixture]
    public class HomeControllerTests
    {

        private readonly Mock<IRepository<Employees>> _mockEmpRepo;
        private readonly Mock<IRepository<MeetingRooms>> _mockMeetingRoom;
        private readonly Mock<IRepository<MeetingsPlanned>> _meetingsPlannedRepo;
        private readonly Mock<IRepository<Users>> _usersRepo;
        private readonly SieveProcessor _sieve;
        private readonly Mock<DatabaseContext> _dbContext;
        private readonly Mock<IUsersService> _mockUserService;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _mockMeetingRoom = new Mock<IRepository<MeetingRooms>>();
            _mockEmpRepo = new Mock<IRepository<Employees>>();
            _usersRepo = new Mock<IRepository<Users>>();
            _sieve = new SieveProcessor(new SieveOptionsAccessor());
            _dbContext = new Mock<DatabaseContext>();
            _mockUserService = new Mock<IUsersService>();
            _meetingsPlannedRepo = new Mock<IRepository<MeetingsPlanned>>();
            _controller = new HomeController(_mockUserService.Object, _usersRepo.Object, _mockEmpRepo.Object, _mockMeetingRoom.Object, _sieve, _meetingsPlannedRepo.Object, _dbContext.Object);
        }

        [Test]
        public void Authenticate()
        {
            //Arrange

            AuthenticateRequest input = new AuthenticateRequest()
            {
                Username = "test@test.com",
                Password = "password",
            };
            Users user = new Users()
            {
                uid = 1,
                user_email = "test@test.com",
                user_password = "password",
            };
            string token = "Some random token";
            AuthenticateResponse response = new AuthenticateResponse(user, token) { Id = 1 };

            //Act

            _mockUserService.Setup(x => x.Authenticate(input)).ReturnsAsync(response);
        }

        [Test]
        [TestCase(1, "Test", "BU1", "RANDP", "password")]
        public void getAllEmployees_With_Data(int id, string name, string bu, string client, string pwd)
        {
            var expected = new Employees()
            {
                EmployeeId = id,
                EmployeeName = name,
                BU = bu,
                Client = client,
                Password = pwd
            };
            var list = new List<Employees>
            {
                expected
            };

            _mockEmpRepo.Setup(x => x.getAllAsync()).ReturnsAsync(list);

            var result = _controller.GetEmployees();

            ClassicAssert.AreEqual(expected.Password, result.Result.First().Password);
            ClassicAssert.AreEqual(expected.Client, result.Result.First().Client);
            ClassicAssert.AreEqual(expected.BU, result.Result.First().BU);
            ClassicAssert.AreEqual(expected.EmployeeName, result.Result.First().EmployeeName);
            ClassicAssert.AreEqual(expected.EmployeeId, result.Result.First().EmployeeId);
        }

        [Test]
        [TestCase(1, "Test@test.com", "password")]
        public void getAllUsers_With_Data(int id, string email, string pwd)
        {
            var expected = new Users()
            {
                user_email = email,
                user_password = pwd,
                uid = id,
            };
            var list = new List<Users>
            {
                expected
            };

            _usersRepo.Setup(x => x.getAllAsync()).ReturnsAsync(list);

            var result = _controller.GetUsers();

            ClassicAssert.AreEqual(expected.user_email, result.Result.First().user_email);
            ClassicAssert.AreEqual(expected.user_password, result.Result.First().user_password);
            ClassicAssert.AreEqual(expected.uid, result.Result.First().uid);
        }

        [Test]
        public void getMeetingRoomInformation()
        {
            var meetingPlanned = new List<MeetingsPlanned>()
            {
                new MeetingsPlanned()
                {
                    Id= 1,
                    MeetingScheduledOn = DateTime.Now,
                    MeetingScheduledFor = 1,
                    NoOfParticipants = 1,
                    RoomNo = 1,
                }

            };

            _meetingsPlannedRepo.Setup(x => x.getAllAsync()).ReturnsAsync(meetingPlanned);
            var result = _controller.GetMeetingRoomInformation(1);
            ClassicAssert.IsNotEmpty(result.Result);
            ClassicAssert.IsTrue(result.Result.Any());

        }

        [Test]
        public void getPlannedMeeting()
        {
            var meetingRooms = new List<MeetingRooms>() { new MeetingRooms()
            {
                Id = 1,
                Capacity = 10,
                IsOccupied = true,
                Roomno = 1,
            }
            };

            var meetingPlanned = new List<MeetingsPlanned>()
            {
                new MeetingsPlanned()
                {
                    Id= 1,
                    MeetingScheduledOn = DateTime.Now,
                    MeetingScheduledFor = 1,
                    NoOfParticipants = 1,
                    RoomNo = 1,
                }

            };

            _mockMeetingRoom.Setup(x => x.getAllAsync()).ReturnsAsync(meetingRooms);
            _meetingsPlannedRepo.Setup(x => x.getAllAsync()).ReturnsAsync(meetingPlanned);
            var result = _controller.GetPlannedMeetings(DateTime.Now);
            ClassicAssert.IsNotEmpty(result.Result);
            ClassicAssert.IsTrue(result.Result.Any());
        }

        [Test]
        public void ScheduleMeeting_Should_ReturnBadRequest()
        {
            var meetingRooms = new List<MeetingRooms>() { new MeetingRooms()
            {
                Id = 1,
                Capacity = 10,
                IsOccupied = true,
                Roomno = 1,
            }
            };
            _mockMeetingRoom.Setup(x => x.getAllAsync()).ReturnsAsync(meetingRooms);
            var result = _controller.ScheduleMeetingAsync(2, DateTime.Now, 1, 1);
            ClassicAssert.IsInstanceOf<BadRequestResult>(result.Result);
        }

        [Test]
        public void ScheduleMeeting_Should_ReturnSuccess()
        {
            var meetingRooms = new List<MeetingRooms>() { new MeetingRooms()
            {
                Id = 1,
                Capacity = 10,
                IsOccupied = true,
                Roomno = 1,
            }
            };
            _mockMeetingRoom.Setup(x => x.getAllAsync()).ReturnsAsync(meetingRooms);
            var result = _controller.ScheduleMeetingAsync(1, DateTime.Now, 1, 1);
            ClassicAssert.IsInstanceOf<OkObjectResult>(result.Result);
        }
    }
}