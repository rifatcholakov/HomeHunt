namespace HomeHunt.Common
{
    public static class ValidationConstants
    {
        public static class Property
        {
            public const int TitleMinLength = 5;
            public const int TitleMaxLength = 300;

            public const int DescriptionMinLength = 20;
            public const int DescriptionMaxLength = 2000;

            public const int ImageUrlMaxLength = 2048;

            public const int PriceMinValue = 1;
            public const int PriceMaxValue = 100000000;

            public const int AreaMinValue = 1;
            public const int AreaMaxValue = 1000000;

            public const int BedroomsMinValue = 0;
            public const int BedroomsMaxValue = 50;

            public const int BathroomsMinValue = 0;
            public const int BathroomsMaxValue = 50;
        }

        public static class Location
        {
            public const int CityMinLength = 2;
            public const int CityMaxLength = 50;

            public const int NeighborhoodMinLength = 2;
            public const int NeighborhoodMaxLength = 60;

            public const string PostalCodeRegex = @"^[0-9]{4}$";
        }

        public static class Agent
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 40;

            public const int EmailMaxLength = 100;
            public const int PhoneMaxLength = 20;

            public const int AgencyNameMinLength = 2;
            public const int AgencyNameMaxLength = 80;
        }
    }
}
