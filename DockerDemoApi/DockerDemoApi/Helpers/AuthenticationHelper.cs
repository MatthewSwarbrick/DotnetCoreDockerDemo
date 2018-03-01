using System;
using System.Collections.Generic;
using DockerDemoApi.Domain;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;

namespace DockerDemoApi.Helpers
{
    public class AuthenticationHelper
    {
        public static string GetIdToken(User user, JWTSettings options)
        {
            var payload = new Dictionary<string, object>
            {
                { "id", user.Id },
                { "sub", user.Email },
                { "email", user.Email },
                { "emailConfirmed", user.EmailConfirmed }
            };
            return GetToken(payload, options);
        }

        public static string GetAccessToken(string Email, JWTSettings options)
        {
            var payload = new Dictionary<string, object>
            {
                { "sub", Email },
                { "email", Email }
            };
            return GetToken(payload, options);
        }

        static string GetToken(Dictionary<string, object> payload, JWTSettings options)
        {
            var secret = options.SecretKey;

            payload.Add("iss", options.Issuer);
            payload.Add("aud", options.Audience);
            payload.Add("nbf", ConvertToUnixTimestamp(DateTime.Now));
            payload.Add("iat", ConvertToUnixTimestamp(DateTime.Now));
            payload.Add("exp", ConvertToUnixTimestamp(DateTime.Now.AddDays(7)));
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return encoder.Encode(payload, secret);
        }

        static double ConvertToUnixTimestamp(DateTime date)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

    }
}
