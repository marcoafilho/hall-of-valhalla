﻿namespace HallOfValhalla.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        public static class Conventions
        {
            public const string Index = Base + "/conventions";

            public const string Show = Base + "/conventions/{conventionId:Guid}";

            public const string Create = Base + "/conventions";

            public const string Update = Base + "/conventions/{conventionId:Guid}";

            public const string Destroy = Base + "/conventions/{conventionId:Guid}";
        }

        public static class Registrations
        {
            public const string Create = Base + "/conventions/{conventionId:Guid}/registrations";
        }
    }
}