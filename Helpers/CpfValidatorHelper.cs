namespace ApiEmpresas.Helpers
{
    public static class CpfValidatorHelper
    {
        public static bool IsCpf(string cpf)
        {
            if (string.IsNullOrEmpty(cpf)) return true; // Deixe o RuleFor().NotEmpty() cuidar da obrigatoriedade

            cpf = cpf.Trim().Replace(".", "").Replace("-", "");
            if (cpf.Length != 11) return false;

            // Elimina CPFs com todos os dígitos iguais (ex: 11111111111)
            if (new string(cpf[0], cpf.Length) == cpf) return false;

            int[] multiplicador1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
            int[] multiplicador2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

            string tempCpf = cpf[..9];
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            tempCpf = tempCpf + digito1;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            return cpf.EndsWith(digito1.ToString() + digito2.ToString());
        }
    }
}