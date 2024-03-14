using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.ViewModel.Auth;

namespace WebShopCore.Helper
{
    public static class SessionExtension
    {
        private static string AuthenticationName = "Authentication_Current";
        public static void SetCurrentAuthentication(this ISession session, AuthenticationModel authenticationName)
        {
            session.SetObjectAsJson(AuthenticationName, authenticationName);
        }

        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static AuthenticationModel GetCurrentAuthentication(this ISession session)
        {
            return session.GetObjectFromJson<AuthenticationModel>(AuthenticationName);
        }

        public  static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static void RemoveSession(this ISession session)
        {
            session.Clear();
            session.Remove(AuthenticationName);
        }
    }
}
