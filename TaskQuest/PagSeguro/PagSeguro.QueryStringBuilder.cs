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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace TaskQuest.PagSeguro
{
    public class QueryStringBuilder
    {
        private const char Separator = '&';
        private const char Equal = '=';
        private StringBuilder builder;

        public QueryStringBuilder()
        {
            builder = new StringBuilder();
        }

        private void AppendCore(string parameterName, string value)
        {
            if (builder.Length > 0)
            {
                builder.Append(QueryStringBuilder.Separator);
            }
            builder.Append(HttpUtility.UrlEncode(parameterName));
            builder.Append(QueryStringBuilder.Equal);
            builder.Append(HttpUtility.UrlEncode(value));
        }

        public QueryStringBuilder Append(string parameterName, string value)
        {
            AppendCore(parameterName, value);
            return this;
        }

        public override string ToString()
        {
            return builder.ToString();
        }

    }
}

