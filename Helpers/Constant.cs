
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Helpers
{
    public class Constant
    {
        public static readonly string API_KEY = "API-KEY";
        public static readonly string OK = "OK";
        public static readonly string MsgStatus400 = "Ocorreu uma falha/erro na sua requisição. Reveja os dados enviados e tente novamente!";
        public static readonly string MsgStatus401 = "Credenciais inválidas ou ausentes";
        public static readonly string MsgStatus404 = "Item não encontrado!";
        public static readonly string MsgStatus429 = "Limite de requisições ultrapassado, por favor, aguarde.";
        public static readonly string MsgStatus500 = "Erro interno no servidor!";
    }
}

