namespace AllupBackendProject.Helpers
{
    public class Helper
    {
        public static void DeleteImg(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        public enum UserRoles
        {
            Admin,
            Member,
            SuperAdmin
        }
    }
}
