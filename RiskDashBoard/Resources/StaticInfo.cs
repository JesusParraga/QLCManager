namespace RiskDashBoard.Resources
{
    public static class StaticInfo
    {
        public enum ProjectPhasesEnum
        {
            NONE = 0,
            EXPLORATION = 1,
            VALUATION = 2,
            FOUNDATIONS = 3, 
            DEVELOPMENT = 4,
            OPERATION = 5
        }

        public enum RiskProbabilityEnum
        {
            //NONE = 0,
            RARE = 1,
            UNLIKELY = 2,
            POSSIBLE = 3,
            LIKELY = 4,
            CERTAIN = 5,
        }

        public enum RiskImpactEnum
        {
            //NONE = 0,
            INSIGNIFICAT = 1,
            MINOR = 2,
            MODERATE = 3,
            MAYOR = 4,
            CATASTROHIC = 5
        }

        public enum RiskLevelEnum
        {
            //NONE = 0,
            LOW = 1, 
            MEDIUM = 2, 
            HIGH = 3, 
            BLOCKER = 4
        }

        public enum RiskStatusEnum
        {
            PENDING = 0,
            RESOLVED = 1
        }
    }
}
