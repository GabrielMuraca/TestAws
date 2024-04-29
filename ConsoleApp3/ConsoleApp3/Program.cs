using ConsoleApp3;

var claseUtil = new Util();

string esPalindromo = string.Empty;

esPalindromo = Console.ReadLine();

MostrarResultado(claseUtil.RevisionPalindromo(esPalindromo));

/// <summary>
/// Aqui se hace el print
/// </summary>
static void MostrarResultado(bool siLoEs)
{
    if (siLoEs)
        Console.WriteLine("Es Palindromo");
    else
        Console.WriteLine("No es palindromo");
}