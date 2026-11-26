namespace HomeHunt.Common
{
    public static class Messages
    {
        public static class Account
        {
            public const string AdminCreateFailedMessage = "Could not create the admin account: ";
            public const string DemoAccountCreateFailedMessage = "Could not create a demo account: ";

            public const string PasswordRequiredMessage = "Password is required.";
            public const string PasswordLengthMessage = "Password must be between {2} and {1} characters.";
            public const string ConfirmPasswordRequiredMessage = "Please confirm your password.";
            public const string ConfirmPasswordMismatchMessage = "The password and confirmation password do not match.";
            public const string EmailTakenMessage = "An account with this email already exists.";
            public const string UpgradedToAgentMessage = "Your account is now an agent account. You can add your first property.";
        }

        public static class Admin
        {
            public const string UserNotFoundMessage = "The user could not be found.";
            public const string CannotChangeOwnAccountMessage = "You can't change or delete your own account here.";
            public const string CannotDeleteAdministratorMessage = "Remove the administrator role before deleting this user.";
            public const string RoleChangedMessage = "The user's role was changed.";
            public const string RoleInvalidMessage = "Please select a valid role.";
            public const string AgentHasPropertiesMessage = "This user still has properties listed as an agent. Delete or reassign them before changing the role.";
            public const string UserDeletedMessage = "The user was deleted.";
        }

        public static class Agent
        {
            public const string FirstNameRequiredMessage = "First name is required.";
            public const string FirstNameLengthMessage = "First name must be between {2} and {1} characters.";
            public const string LastNameRequiredMessage = "Last name is required.";
            public const string LastNameLengthMessage = "Last name must be between {2} and {1} characters.";
            public const string EmailRequiredMessage = "Email is required.";
            public const string EmailInvalidMessage = "Please enter a valid email address.";
            public const string EmailMaxLengthMessage = "Email can be at most {1} characters.";
            public const string EmailExistsMessage = "An agent with this email already exists.";
            public const string PhoneRequiredMessage = "Phone is required.";
            public const string PhoneInvalidMessage = "Please enter a valid phone number.";
            public const string PhoneFormatMessage = "Phone must start with + and contain 7 to 15 digits.";
            public const string PhoneMaxLengthMessage = "Phone can be at most {1} characters.";
            public const string PhoneTakenMessage = "An agent with this phone number already exists.";
            public const string AgencyNameLengthMessage = "Agency name must be between {2} and {1} characters.";
            public const string SaveFailedMessage = "The agent could not be saved. Please check the details and try again.";
            public const string AlreadyAgentMessage = "This account is already an agent account.";

            public const string CreatedMessage = "The agent was created.";
            public const string UpdatedMessage = "The agent was updated.";
            public const string DeletedMessage = "The agent and all of their properties were deleted.";
        }

        public static class Location
        {
            public const string CityRequiredMessage = "City is required.";
            public const string CityLengthMessage = "City must be between {2} and {1} characters.";
            public const string NeighborhoodRequiredMessage = "Neighborhood is required.";
            public const string NeighborhoodLengthMessage = "Neighborhood must be between {2} and {1} characters.";
            public const string PostalCodeRequiredMessage = "Postal code is required.";
            public const string PostalCodeFormatMessage = "Postal code must contain exactly 4 digits.";
            public const string PostalCodeBelongsToMessage = "This postal code belongs to {0}.";
            public const string AlreadyExistsMessage = "This location already exists.";
            public const string CreatedMessage = "The location was created.";
            public const string UpdatedMessage = "The location was updated.";
            public const string DeletedMessage = "The location and all of its properties were deleted.";
        }

        public static class Property
        {
            public const string TitleRequiredMessage = "Title is required.";
            public const string TitleLengthMessage = "Title must be between {2} and {1} characters.";
            public const string DescriptionRequiredMessage = "Description is required.";
            public const string DescriptionLengthMessage = "Description must be between {2} and {1} characters.";
            public const string PriceRequiredMessage = "Price is required.";
            public const string PriceRangeMessage = "Price must be between {1:N0} and {2:N0}.";
            public const string AreaRequiredMessage = "Area is required.";
            public const string AreaRangeMessage = "Area must be between {1:N0} and {2:N0} m².";
            public const string BedroomsRangeMessage = "Bedrooms must be between {1} and {2}.";
            public const string BathroomsRangeMessage = "Bathrooms must be between {1} and {2}.";
            public const string PropertyTypeInvalidMessage = "Please select a valid property type.";
            public const string ListingTypeInvalidMessage = "Please select a valid listing type.";
            public const string StatusInvalidMessage = "Please select a valid status.";
            public const string ImageUrlMaxLengthMessage = "The link can be at most {1} characters.";
            public const string ImageUrlInvalidMessage = "Enter a valid link that starts with http:// or https://";
            public const string LocationRequiredMessage = "Please select a location.";
            public const string AgentRequiredMessage = "Please select an agent.";
            public const string LocationInvalidMessage = "Please select a valid location.";
            public const string AgentInvalidMessage = "Please select a valid agent.";
            public const string MinPriceRangeMessage = "Minimum price must be between {1:N0} and {2:N0}.";
            public const string MaxPriceRangeMessage = "Maximum price must be between {1:N0} and {2:N0}.";
            public const string MinBedroomsRangeMessage = "Minimum bedrooms must be between {1} and {2}.";
            public const string MinAreaRangeMessage = "Minimum area must be between {1} and {2:N0}.";
            public const string MinPriceGreaterThanMaxMessage = "Minimum price cannot be greater than maximum price.";
            public const string CreatedMessage = "The property was created.";
            public const string UpdatedMessage = "The property was updated.";
            public const string DeletedMessage = "The property was deleted.";
        }
    }
}
