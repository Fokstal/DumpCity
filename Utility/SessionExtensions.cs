using System.Text.Json;

namespace DumpCity.Utility
{
    public static class SessionExtensions
    {
        public static void Set<TValue>(this ISession session, string key, TValue value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static TValue? Get<TValue>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default : JsonSerializer.Deserialize<TValue>(value);
        }
    }
}