namespace Domain.Constants;

public static class UserRoles
{
    public const string Student = "student";
    public const string Teacher = "teacher";
    public const string Admin = "admin";

    public static string[] All => [Student, Teacher, Admin];
}
