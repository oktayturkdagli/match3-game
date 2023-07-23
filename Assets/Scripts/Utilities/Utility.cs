public static class Utility
{
    public static void CallASingleton<T>() where T : Singleton<T>
    {
        var singletonInstance = Singleton<T>.Instance;
    }
}