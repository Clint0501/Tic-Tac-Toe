namespace Script.Common
{
    public class Singleton<T> where T : class, new()
    {
        private static T s_Instance;
        private static readonly object s_Lock = new object();

        protected Singleton()
        {
            
        }

        public static T GetInstance()
        {
            if (s_Instance == null)
            {
                lock (s_Lock)
                {
                    s_Instance ??= new T();
                }
            }

            return s_Instance;
        }
    }
}