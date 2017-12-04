// Copyright [2011] [PagSeguro Internet Ltda.]
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace TaskQuest.PagSeguro
{
    public static class Service
    {

        public const string Post = "POST";
        public const string Get = "GET";

        private static string FirstLetterToLowerCase(string @string)
        {
            return @string.Substring(0, 1).ToLower() + @string.Substring(1, @string.Length - 1);
        }

        public static string ParseQuery(object obj)
        {
            var query = new QueryStringBuilder();
            foreach (var prop in obj.GetType().GetProperties())
                if (prop.GetValue(obj) != null)
                    query.Append(FirstLetterToLowerCase(prop.Name), prop.GetValue(obj).ToString());
            return query.ToString();
        }

        public static HttpWebResponse Request(string urlPath, string query, string method)
        {
            HttpWebRequest request;
            if (Util.HasInternetConnection())
            {
                try
                {

                    request = (HttpWebRequest)WebRequest.Create(urlPath);

                    request.ContentType = "application/x-www-form-urlencoded; charset= ISO-8859-1";
                    request.Method = method;
                    request.Timeout = 10000;
                    request.Headers.Add("lib-description", ".net:" + "2.4.0");
                    request.Headers.Add("language-engine-description", ".net:" + Environment.Version.ToString());

                    if (!string.IsNullOrEmpty(query))
                    {
                        using (Stream requestStream = request.GetRequestStream())
                        {
                            byte[] byteArray = Encoding.UTF8.GetBytes(query);
                            requestStream.Write(byteArray, 0, byteArray.Length);
                            requestStream.Close();
                        }
                    }
                    return (HttpWebResponse)request.GetResponse();
                }
                catch (WebException) { return null; }
            }
            return null;
        }

        public static IDictionary<string, string> ReadXml(XDocument doc)
        {
            return doc.Root.DescendantNodes().OfType<XElement>().ToDictionary(n => n.Name.ToString(), n => n.Value);
        }

        public static string DictToString(this IDictionary<string, string> dict)
        {
            var sb = new StringBuilder();

            foreach (var item in dict)
            {
                if (sb.Length > 0)
                    sb.Append(", ");
                sb.Append(item.Key);
                sb.Append(": ");
                sb.Append(item.Value);
            }

            return sb.ToString();
        }

    }
}