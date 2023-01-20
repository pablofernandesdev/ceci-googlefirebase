using CeciGoogleFirebase.Domain.DTO.Role;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using CeciGoogleFirebase.Service.Validators.Role;
using CeciGoogleFirebase.Test.Fakers.Role;
using FluentValidation.TestHelper;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CeciGoogleFirebase.Test.Validators.Role
{
    public class RoleUpdateValidatorTest
    {
        private readonly RoleUpdateValidator _validator;
        private readonly Moq.Mock<IUnitOfWork> _mockUnitOfWork;

        public RoleUpdateValidatorTest()
        {
            _mockUnitOfWork = new Moq.Mock<IUnitOfWork>();
            _validator = new RoleUpdateValidator(_mockUnitOfWork.Object);
        }

        [Fact]
        public void There_should_be_an_error_when_properties_are_null()
        {
            //Arrange
            var model = new RoleUpdateDTO();

            _mockUnitOfWork.Setup(x => x.Role.GetFirstOrDefaultAsync(c => c.Id.Equals(It.IsAny<int>())))
                .ReturnsAsync(value: null);

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldHaveValidationErrorFor(role => role.RoleId);
            result.ShouldHaveValidationErrorFor(role => role.Name);
        }

        [Fact]
        public void There_should_not_be_an_error_for_the_properties()
        {
            //Arrange
            var model = RoleFaker.RoleUpdateDTO().Generate();

            _mockUnitOfWork.Setup(x => x.Role.GetFirstOrDefaultAsync(c => c.Id.Equals(It.IsAny<int>())))
                .ReturnsAsync(RoleFaker.RoleEntity().Generate());

            //act
            var result = _validator.TestValidate(model);

            //assert
            //result.ShouldNotHaveValidationErrorFor(role => role.RoleId);
            result.ShouldNotHaveValidationErrorFor(role => role.Name);
        }
    }
}
