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
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace TaskQuest.PagSeguro
{
    public class Assinatura
    {

        public Assinatura()
        {
            PreApprovalCharge = "auto";

            PreApprovalPeriod = "Monthly";

            Currency = "BRL";

            PreApprovalAmountPerPayment = (40m).ToString("F").Replace(",", ".");

            PreApprovalFinalDate = DateTime.Now.AddYears(2).ToString("yyyy-MM-dd") + "T01:00:00.45-03:00";

            //Valor da assinatura mensal vezes 2 anos vezes 12 meses
            PreApprovalMaxTotalAmount = (Convert.ToDecimal(PreApprovalAmountPerPayment) * 2 * 12).ToString("F").Replace(",", ".");
        }

        public IDictionary<string, string> CriarAssinatura()
        {

            var IsSandBox = ConfigurationManager.AppSettings["IsSandBox"];
            var path = "";
            if (IsSandBox == "True")
                path = "https://ws.sandbox.pagseguro.uol.com.br/v2/pre-approvals/request";
            else
                path = "https://ws.pagseguro.uol.com.br/v2/pre-approvals/request";

            try
            {
                var response = Service.Request(urlPath: path, query: Service.ParseQuery(this), method: Service.Post);
                if (HttpStatusCode.OK.Equals(response.StatusCode))
                    return Service.ReadXml(XDocument.Load(response.GetResponseStream()));
                
                else
                    return null;
            }
            catch (WebException ex)
            {
                var doc = new XmlDocument();
                return Service.ReadXml(XDocument.Load(ex.Response.GetResponseStream()));
            }

        }

        [Required]
        //Email do vendedor
        public string Email { get; set; }

        [Required]
        //Token (senha) do vendedor
        public string Token { get; set; }

        [Required]
        //Tipo de moeda
        public string Currency { get; set; }

        [StringLength(50)]
        //Nome do comprador
        public string SenderName { get; set; }

        [StringLength(2)]
        public string SenderAreaCode { get; set; }

        [StringLength(9, MinimumLength = 7)]
        public string SenderPhone { get; set; }

        [StringLength(60)]
        public string SenderEmail { get; set; }

        [Required]
        //Se a cobrança for automática ou manual
        public string PreApprovalCharge { get; set; }

        [Required]
        [StringLength(100)]
        //Reconhece a cobrança por nome para facilitar a verificação da cobrança
        public string PreApprovalName { get; set; }

        [StringLength(255)]
        //Descrição do produto
        public string PreApprovalDetails { get; set; }

        [Required]
        //O valor cobrando por mensalidade
        public string PreApprovalAmountPerPayment { get; set; }

        [Required]
        //O período que se realiza a combrança
        public string PreApprovalPeriod { get; set; }

        [Required]
        /*
        YYYY-MM-DDThh:mm:ss.sTZD. Assume valores maiores que a data atual
        ou maiores que o valor definido em preApprovalInitialDate, 
        não podendo ter uma diferença superior a 2 anos da data de início.
        */
        public string PreApprovalFinalDate { get; set; }

        [Required]
        //Valor total a ser cobrado pela assinatura
        public string PreApprovalMaxTotalAmount { get; set; }

        [EmailAddress]
        [StringLength(60)]
        //Email do vendedor (é exibido ao cliente)
        public string ReceiverEmail { get; set; }

        [StringLength(255)]
        //Onde o comprador é redirecionado após a transação
        public string RedirectURL { get; set; }

        [StringLength(255)]
        //Outra forma de identificar a assinatura
        public string Reference { get; set; }

        [StringLength(255)]
        //URL para onde o comprador será redirecionado, 
        //durante o fluxo de aprovação, 
        //caso deseje alterar/revisar as regras da assinatura
        public string ReviewURL { get; set; }

    }
}