public static class StaticFunctions
{
    public static string JobTypeToString(JobType jobType)
    {
        switch (jobType)
        {
            case JobType.shootout:
                return "SHOOTOUT";
            case JobType.heist:
                return "HEIST";
            case JobType.teamdeathmatch:
                return "TEAMDEATHMATCH";
            default:
                return "";
        }
    }
}
