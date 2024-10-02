namespace Domain.Constants;

public static class ActivityTypes
{
    public const string Seminar = "Seminar";
    public const string Assignment = "Assignment";
    public const string GroupProject = "Group project";
    public const string Setup = "Setup";

    public static string[] All => [Seminar, Assignment, GroupProject, Setup];
}
