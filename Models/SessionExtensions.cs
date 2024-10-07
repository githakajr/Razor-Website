using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace RazorWebsite.Extensions
{
    public static class SessionExtensions
    {
        // Set complex object in session
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Get complex object from session
        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        // Set object as JSON in session
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value)); // Use JsonSerializer.Serialize
        }

        // Get object from JSON in session
        public static T? GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value); // Use JsonSerializer.Deserialize
        }
    }
}
