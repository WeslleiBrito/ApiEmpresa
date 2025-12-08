namespace ApiEmpresas.Helpers
{
    public static class CnpjValidatorHelper
    {
        public static bool IsCnpj(string cnpj)
        {
            if (string.IsNullOrEmpty(cnpj)) return false;

            // Remove caracteres não numéricos
            cnpj = cnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", "");

            if (cnpj.Length != 14) return false;

            // Elimina CNPJs com todos os dígitos iguais (ex: 00000000000000)
            // Embora raro em CNPJ, é tecnicamente inválido.
            if (new string(cnpj[0], cnpj.Length) == cnpj) return false;

            int[] multiplicador1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
            int[] multiplicador2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            int resto = (soma % 11);
            if (resto < 2) resto = 0;
            else resto = 11 - resto;

            string digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            if (resto < 2) resto = 0;
            else resto = 11 - resto;

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);
        }
    }
}