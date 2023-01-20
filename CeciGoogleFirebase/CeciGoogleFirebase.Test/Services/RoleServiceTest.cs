using AutoMapper;
using CeciGoogleFirebase.Domain.DTO.Role;
using CeciGoogleFirebase.Domain.Entities;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using CeciGoogleFirebase.Domain.Mapping;
using CeciGoogleFirebase.Service.Services;
using CeciGoogleFirebase.Test.Fakers.Role;
using Moq;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CeciGoogleFirebase.Test.Services
{
    public class RoleServiceTest
    {
        private readonly Moq.Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly IMapper _mapper;

        public RoleServiceTest()
        {
            _mockUnitOfWork = new Moq.Mock<IUnitOfWork>();

            //Auto mapper configuration
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task Add_role_successfully()
        {
            //Arrange
            var roleEntityFaker = RoleFaker.RoleEntity().Generate();
            var userAddDTO = _mapper.Map<RoleAddDTO>(roleEntityFaker);

            _mockUnitOfWork.Setup(x => x.Role.AddAsync(roleEntityFaker))
                .ReturnsAsync(roleEntityFaker);

            var roleService = RoleServiceConstrutor();

            //Act
            var result = await roleService.AddAsync(userAddDTO);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Add_role_exception()
        {
            //Arrange
            var roleEntityFaker = RoleFaker.RoleEntity().Generate();
            var roleAddDTO = _mapper.Map<RoleAddDTO>(roleEntityFaker);

            _mockUnitOfWork.Setup(x => x.Role.AddAsync(roleEntityFaker))
                .ReturnsAsync(roleEntityFaker);

            _mockUnitOfWork.Setup(x => x.CommitAsync())
                .ThrowsAsync(new Exception());

            var roleService = RoleServiceConstrutor();

            //Act
            var result = await roleService.AddAsync(roleAddDTO);

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Delete_role_successfully()
        {
            //Arrange
            _mockUnitOfWork.Setup(x => x.Role.GetFirstOrDefaultAsync(c => c.Id == 1))
                .ReturnsAsync(RoleFaker.RoleEntity().Generate());

            var roleService = RoleServiceConstrutor();

            //Act
            var result = await roleService.DeleteAsync(1);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Delete_role_not_found()
        {
            //Arrange
            _mockUnitOfWork.Setup(x => x.Role.GetFirstOrDefaultAsync(c => c.Id == 1))
                .ReturnsAsync(value: null);

            var roleService = RoleServiceConstrutor();

            //Act
            var result = await roleService.DeleteAsync(1);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.BadRequest));
        }

        [Fact]
        public async Task Delete_role_exception()
        {
            //Arrange
            _mockUnitOfWork.Setup(x => x.Role.GetFirstOrDefaultAsync(c => c.Id == 1))
                .ThrowsAsync(new Exception());

            var roleService = RoleServiceConstrutor();

            //Act
            var result = await roleService.DeleteAsync(1);

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Update_role_successfully()
        {
            //Arrange
            var roleUpdateDTOFaker = RoleFaker.RoleUpdateDTO().Generate();

            _mockUnitOfWork.Setup(x => x.Role.GetFirstOrDefaultAsync(c => c.Id == roleUpdateDTOFaker.RoleId))
                .ReturnsAsync(_mapper.Map<Role>(roleUpdateDTOFaker));

            var roleService = RoleServiceConstrutor();

            //Act
            var result = await roleService.UpdateAsync(roleUpdateDTOFaker);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Update_role_not_found()
        {
            //Arrange
            var roleUpdateDTOFaker = RoleFaker.RoleUpdateDTO().Generate();

            _mockUnitOfWork.Setup(x => x.Role.GetFirstOrDefaultAsync(c => c.Id == roleUpdateDTOFaker.RoleId))
                .ReturnsAsync(value: null);

            var roleService = RoleServiceConstrutor();

            //Act
            var result = await roleService.UpdateAsync(roleUpdateDTOFaker);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.BadRequest));
        }

        [Fact]
        public async Task Update_role_exception()
        {
            //Arrange
            var roleUpdateDTOFaker = RoleFaker.RoleUpdateDTO().Generate();

            _mockUnitOfWork.Setup(x => x.Role.GetFirstOrDefaultAsync(c => c.Id == roleUpdateDTOFaker.RoleId))
                .ThrowsAsync(new Exception());

            var roleService = RoleServiceConstrutor();

            //Act
            var result = await roleService.UpdateAsync(roleUpdateDTOFaker);

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Get_role_by_id()
        {
            //Arrange
            _mockUnitOfWork.Setup(x => x.Role.GetFirstOrDefaultAsync(c => c.Id == 1))
                .ReturnsAsync(RoleFaker.RoleEntity().Generate());

            var roleService = RoleServiceConstrutor();

            //Act
            var result = await roleService.GetByIdAsync(1);

            //Assert
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task Get_role_by_id_exception()
        {
            //Arrange
            _mockUnitOfWork.Setup(x => x.Role.GetFirstOrDefaultAsync(c => c.Id == 1))
                .ThrowsAsync(new Exception());

            var roleService = RoleServiceConstrutor();

            //Act
            var result = await roleService.GetByIdAsync(1);

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Get_all_users()
        {
            //Arrange
            _mockUnitOfWork.Setup(x => x.Role.GetAllAsync())
                .ReturnsAsync(RoleFaker.RoleEntity().Generate(2));

            var roleService = RoleServiceConstrutor();

            //Act
            var result = await roleService.GetAsync();

            //Assert
            Assert.True(result.Data.Any() && result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Get_all_users_exception()
        {
            //Arrange
            _mockUnitOfWork.Setup(x => x.Role.GetAllAsync())
                .ThrowsAsync(new Exception());

            var roleService = RoleServiceConstrutor();

            //Act
            var result = await roleService.GetAsync();

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        private RoleService RoleServiceConstrutor()
        {
            return new RoleService(_mockUnitOfWork.Object,
                _mapper);
        }
    }
}
