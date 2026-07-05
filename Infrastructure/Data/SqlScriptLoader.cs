using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;

namespace Infrastructure.Data;

public static class SqlScriptLoader
{
    public static string Load(string fileName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var resourceName = assembly
            .GetManifestResourceNames()
            .Single(x => x.EndsWith(fileName));

        using var stream = assembly.GetManifestResourceStream(resourceName)!;
        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }

    public static void Execute(MigrationBuilder migrationBuilder, string fileName)
    {
        migrationBuilder.Sql(Load(fileName));
    }
}