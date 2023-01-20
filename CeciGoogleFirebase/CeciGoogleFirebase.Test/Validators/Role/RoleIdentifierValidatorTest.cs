using CeciGoogleFirebase.Domain.DTO.Role;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using CeciGoogleFirebase.Service.Validators.Role;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CeciGoogleFirebase.Test.Validators.Role
{
    public class RoleIdentifierValidatorTest
    {
        private readonly RoleIdentifierValidator _validator;

        public RoleIdentifierValidatorTest()
        {
            _validator = new RoleIdentifierValidator();
        }

        [Fact]
        public void There_should_be_an_error_when_properties_are_null()
        {
            //Arrange
            var model = new IdentifierRoleDTO();

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldHaveValidationErrorFor(role => role.RoleId);
        }

        [Fact]
        public void There_should_not_be_an_error_for_the_properties()
        {
            //Arrange
            var model = new IdentifierRoleDTO{ 
                RoleId = 1};

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldNotHaveValidationErrorFor(role => role.RoleId);
        }
    }
}
