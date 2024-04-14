using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
            CATASTROPHIC = 5
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

        public enum RiskLevelEvaluationTypeEnum
        {
            Acceptable = 1,
            Addressable = 2,
            Unaddressable = 3,
            Negligible = 4
        }

        public enum PhaseDecissionProposalTypeEnum
        {
            [Description("Go forward")]
            GoForward = 1,
            [Description("Go back")]
            GoBack = 2,
            [Description("Discontinue")]
            Discontinue = 3
        }

        public enum ProjectSatusEnum
        {
            Open = 0,
            Close = 1
        }

        public static string GetDescriptionAttributeEnum(Enum valorEnum)
        {
            try
            {
                var tipoEnum = valorEnum.GetType();
                var nombreCampo = Enum.GetName(tipoEnum, valorEnum);

                var campo = tipoEnum.GetField(nombreCampo);
                var atributo = (DescriptionAttribute)Attribute.GetCustomAttribute(campo, typeof(DescriptionAttribute));
                return atributo?.Description ?? valorEnum.ToString();
            }
            catch (Exception)
            {
                return valorEnum.ToString();
            }    
        }
    }
}
