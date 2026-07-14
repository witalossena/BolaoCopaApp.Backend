using BolaoCopaApp.Domain.Entities;
using BolaoCopaApp.Domain.Enums;
using BolaoCopaApp.Infrastructure.Persistence;

namespace BolaoCopaApp.Infrastructure.Persistence.Seeders;

public static class MatchSeeder
{
    // Seeds R32 (16avos) matches. Safe to call on existing DBs — skips if already present.
    public static async Task SeedRoundOf32Async(BolaoDbContext context)
    {
        if (context.Matches.Any(m => m.Round == MatchRound.RoundOf32)) return;

        // M0-M7: group winners (E,I,A,L,G,D,B,K) vs dynamically-assigned 3rd-place teams
        // M8-M15: remaining direct qualifier matches
        var r32 = new List<Match>
        {
            new Match { ExternalId = "ko_r32_0",  HomeTeam = "1º E", AwayTeam = "3º classificado", Round = MatchRound.RoundOf32, MatchDate = new DateTime(2026, 6, 28, 16, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_r32_1",  HomeTeam = "1º I", AwayTeam = "3º classificado", Round = MatchRound.RoundOf32, MatchDate = new DateTime(2026, 6, 28, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_r32_2",  HomeTeam = "1º A", AwayTeam = "3º classificado", Round = MatchRound.RoundOf32, MatchDate = new DateTime(2026, 6, 28, 22, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_r32_3",  HomeTeam = "1º L", AwayTeam = "3º classificado", Round = MatchRound.RoundOf32, MatchDate = new DateTime(2026, 6, 29, 16, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_r32_4",  HomeTeam = "1º G", AwayTeam = "3º classificado", Round = MatchRound.RoundOf32, MatchDate = new DateTime(2026, 6, 29, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_r32_5",  HomeTeam = "1º D", AwayTeam = "3º classificado", Round = MatchRound.RoundOf32, MatchDate = new DateTime(2026, 6, 29, 22, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_r32_6",  HomeTeam = "1º B", AwayTeam = "3º classificado", Round = MatchRound.RoundOf32, MatchDate = new DateTime(2026, 6, 30, 16, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_r32_7",  HomeTeam = "1º K", AwayTeam = "3º classificado", Round = MatchRound.RoundOf32, MatchDate = new DateTime(2026, 6, 30, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_r32_8",  HomeTeam = "1º C", AwayTeam = "2º D", Round = MatchRound.RoundOf32, MatchDate = new DateTime(2026, 6, 30, 22, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_r32_9",  HomeTeam = "1º F", AwayTeam = "2º E", Round = MatchRound.RoundOf32, MatchDate = new DateTime(2026, 7,  1, 16, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_r32_10", HomeTeam = "1º H", AwayTeam = "2º G", Round = MatchRound.RoundOf32, MatchDate = new DateTime(2026, 7,  1, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_r32_11", HomeTeam = "1º J", AwayTeam = "2º I", Round = MatchRound.RoundOf32, MatchDate = new DateTime(2026, 7,  1, 22, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_r32_12", HomeTeam = "2º A", AwayTeam = "2º C", Round = MatchRound.RoundOf32, MatchDate = new DateTime(2026, 7,  2, 16, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_r32_13", HomeTeam = "2º B", AwayTeam = "2º F", Round = MatchRound.RoundOf32, MatchDate = new DateTime(2026, 7,  2, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_r32_14", HomeTeam = "2º L", AwayTeam = "2º H", Round = MatchRound.RoundOf32, MatchDate = new DateTime(2026, 7,  2, 22, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_r32_15", HomeTeam = "2º K", AwayTeam = "2º J", Round = MatchRound.RoundOf32, MatchDate = new DateTime(2026, 7,  3, 16, 0, 0, DateTimeKind.Utc) },
        };

        await context.Matches.AddRangeAsync(r32);
        await context.SaveChangesAsync();
    }

    public static async Task SeedKnockoutAsync(BolaoDbContext context)
    {
        if (context.Matches.Any(m => m.Round == MatchRound.RoundOf16)) return;

        var knockout = new List<Match>
        {
            // Oitavas de final
            new Match { ExternalId = "ko_r16_0", HomeTeam = "Canadá",         AwayTeam = "Marrocos",         Round = MatchRound.RoundOf16, MatchDate = new DateTime(2026, 7,  4, 17, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_r16_1", HomeTeam = "Paraguai",        AwayTeam = "França",           Round = MatchRound.RoundOf16, MatchDate = new DateTime(2026, 7,  4, 21, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_r16_2", HomeTeam = "Brasil",          AwayTeam = "Noruega",          Round = MatchRound.RoundOf16, MatchDate = new DateTime(2026, 7,  5, 20, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_r16_3", HomeTeam = "México",          AwayTeam = "Inglaterra",       Round = MatchRound.RoundOf16, MatchDate = new DateTime(2026, 7,  6,  0, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_r16_4", HomeTeam = "Portugal",        AwayTeam = "Espanha",          Round = MatchRound.RoundOf16, MatchDate = new DateTime(2026, 7,  6, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_r16_5", HomeTeam = "Estados Unidos",  AwayTeam = "Bélgica",          Round = MatchRound.RoundOf16, MatchDate = new DateTime(2026, 7,  7,  0, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_r16_6", HomeTeam = "Argentina",       AwayTeam = "Egito",            Round = MatchRound.RoundOf16, MatchDate = new DateTime(2026, 7,  7, 16, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_r16_7", HomeTeam = "Suíça",           AwayTeam = "Colômbia",         Round = MatchRound.RoundOf16, MatchDate = new DateTime(2026, 7,  7, 20, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_qf_0",  HomeTeam = "França",          AwayTeam = "Marrocos",         Round = MatchRound.QuarterFinal, MatchDate = new DateTime(2026, 7,  9, 20, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_qf_1",  HomeTeam = "Espanha",         AwayTeam = "Bélgica",          Round = MatchRound.QuarterFinal, MatchDate = new DateTime(2026, 7, 10, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_qf_2",  HomeTeam = "Noruega",         AwayTeam = "Inglaterra",       Round = MatchRound.QuarterFinal, MatchDate = new DateTime(2026, 7, 11, 21, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_qf_3",  HomeTeam = "Argentina",       AwayTeam = "Suíça",            Round = MatchRound.QuarterFinal, MatchDate = new DateTime(2026, 7, 12,  1, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_sf_0",  HomeTeam = "França",          AwayTeam = "Espanha",          Round = MatchRound.SemiFinal,    MatchDate = new DateTime(2026, 7, 14, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_sf_1",  HomeTeam = "Inglaterra",      AwayTeam = "Argentina",        Round = MatchRound.SemiFinal,    MatchDate = new DateTime(2026, 7, 15, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_3rd",   HomeTeam = "Perdedor SF-1",  AwayTeam = "Perdedor SF-2",    Round = MatchRound.ThirdPlace,   MatchDate = new DateTime(2026, 7, 18, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "ko_final", HomeTeam = "Vencedor SF-1",  AwayTeam = "Vencedor SF-2",    Round = MatchRound.Final,        MatchDate = new DateTime(2026, 7, 19, 19, 0, 0, DateTimeKind.Utc) },
        };

        await context.Matches.AddRangeAsync(knockout);
        await context.SaveChangesAsync();
    }

    public static async Task SeedAsync(BolaoDbContext context)
    {
        if (context.Matches.Any()) return;

        var matches = new List<Match>
        {
            // Grupo A
            new Match { ExternalId = "m0",  HomeTeam = "México",              AwayTeam = "África do Sul",        Group = "A", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 11, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m1",  HomeTeam = "Coreia do Sul",        AwayTeam = "República Tcheca",     Group = "A", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 11, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m2",  HomeTeam = "República Tcheca",     AwayTeam = "África do Sul",        Group = "A", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 18, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m3",  HomeTeam = "México",              AwayTeam = "Coreia do Sul",        Group = "A", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 18, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m4",  HomeTeam = "República Tcheca",     AwayTeam = "México",              Group = "A", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 24, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m5",  HomeTeam = "África do Sul",        AwayTeam = "Coreia do Sul",        Group = "A", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 24, 19, 0, 0, DateTimeKind.Utc) },

            // Grupo B
            new Match { ExternalId = "m6",  HomeTeam = "Canadá",              AwayTeam = "Bósnia e Herzegovina", Group = "B", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 12, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m7",  HomeTeam = "Catar",               AwayTeam = "Suíça",                Group = "B", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 13, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m8",  HomeTeam = "Suíça",               AwayTeam = "Bósnia e Herzegovina", Group = "B", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 18, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m9",  HomeTeam = "Canadá",              AwayTeam = "Catar",                Group = "B", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 19, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m10", HomeTeam = "Suíça",               AwayTeam = "Canadá",              Group = "B", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 24, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m11", HomeTeam = "Bósnia e Herzegovina", AwayTeam = "Catar",               Group = "B", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 24, 19, 0, 0, DateTimeKind.Utc) },

            // Grupo C
            new Match { ExternalId = "m12", HomeTeam = "Brasil",              AwayTeam = "Marrocos",             Group = "C", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 13, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m13", HomeTeam = "Haiti",               AwayTeam = "Escócia",              Group = "C", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 14, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m14", HomeTeam = "Escócia",             AwayTeam = "Marrocos",             Group = "C", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 19, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m15", HomeTeam = "Brasil",              AwayTeam = "Haiti",                Group = "C", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 19, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m16", HomeTeam = "Escócia",             AwayTeam = "Brasil",               Group = "C", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 24, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m17", HomeTeam = "Marrocos",            AwayTeam = "Haiti",                Group = "C", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 24, 19, 0, 0, DateTimeKind.Utc) },

            // Grupo D
            new Match { ExternalId = "m18", HomeTeam = "Estados Unidos",      AwayTeam = "Paraguai",             Group = "D", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 12, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m19", HomeTeam = "Austrália",           AwayTeam = "Turquia",              Group = "D", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 14, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m20", HomeTeam = "Turquia",             AwayTeam = "Paraguai",             Group = "D", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 19, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m21", HomeTeam = "Estados Unidos",      AwayTeam = "Austrália",            Group = "D", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 20, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m22", HomeTeam = "Turquia",             AwayTeam = "Estados Unidos",       Group = "D", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 25, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m23", HomeTeam = "Paraguai",            AwayTeam = "Austrália",            Group = "D", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 25, 19, 0, 0, DateTimeKind.Utc) },

            // Grupo E
            new Match { ExternalId = "m24", HomeTeam = "Alemanha",            AwayTeam = "Curaçau",              Group = "E", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 14, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m25", HomeTeam = "Costa do Marfim",     AwayTeam = "Equador",              Group = "E", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 15, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m26", HomeTeam = "Equador",             AwayTeam = "Curaçau",              Group = "E", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 20, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m27", HomeTeam = "Alemanha",            AwayTeam = "Costa do Marfim",      Group = "E", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 20, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m28", HomeTeam = "Equador",             AwayTeam = "Alemanha",             Group = "E", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 25, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m29", HomeTeam = "Curaçau",             AwayTeam = "Costa do Marfim",      Group = "E", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 25, 19, 0, 0, DateTimeKind.Utc) },

            // Grupo F
            new Match { ExternalId = "m30", HomeTeam = "Holanda",             AwayTeam = "Japão",                Group = "F", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 14, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m31", HomeTeam = "Suécia",              AwayTeam = "Tunísia",              Group = "F", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 15, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m32", HomeTeam = "Tunísia",             AwayTeam = "Japão",                Group = "F", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 20, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m33", HomeTeam = "Holanda",             AwayTeam = "Suécia",               Group = "F", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 21, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m34", HomeTeam = "Tunísia",             AwayTeam = "Holanda",              Group = "F", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 25, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m35", HomeTeam = "Japão",               AwayTeam = "Suécia",               Group = "F", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 25, 19, 0, 0, DateTimeKind.Utc) },

            // Grupo G
            new Match { ExternalId = "m36", HomeTeam = "Bélgica",             AwayTeam = "Egito",                Group = "G", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 15, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m37", HomeTeam = "Irã",                 AwayTeam = "Nova Zelândia",        Group = "G", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 16, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m38", HomeTeam = "Nova Zelândia",       AwayTeam = "Egito",                Group = "G", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 21, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m39", HomeTeam = "Bélgica",             AwayTeam = "Irã",                  Group = "G", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 21, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m40", HomeTeam = "Nova Zelândia",       AwayTeam = "Bélgica",              Group = "G", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 26, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m41", HomeTeam = "Egito",               AwayTeam = "Irã",                  Group = "G", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 26, 19, 0, 0, DateTimeKind.Utc) },

            // Grupo H
            new Match { ExternalId = "m42", HomeTeam = "Espanha",             AwayTeam = "Cabo Verde",           Group = "H", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 15, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m43", HomeTeam = "Arábia Saudita",      AwayTeam = "Uruguai",              Group = "H", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 16, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m44", HomeTeam = "Uruguai",             AwayTeam = "Cabo Verde",           Group = "H", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 21, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m45", HomeTeam = "Espanha",             AwayTeam = "Arábia Saudita",       Group = "H", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 21, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m46", HomeTeam = "Uruguai",             AwayTeam = "Espanha",              Group = "H", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 26, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m47", HomeTeam = "Cabo Verde",          AwayTeam = "Arábia Saudita",       Group = "H", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 26, 19, 0, 0, DateTimeKind.Utc) },

            // Grupo I
            new Match { ExternalId = "m48", HomeTeam = "França",              AwayTeam = "Senegal",              Group = "I", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 16, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m49", HomeTeam = "Iraque",              AwayTeam = "Noruega",              Group = "I", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 16, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m50", HomeTeam = "Noruega",             AwayTeam = "Senegal",              Group = "I", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 22, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m51", HomeTeam = "França",              AwayTeam = "Iraque",               Group = "I", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 22, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m52", HomeTeam = "Noruega",             AwayTeam = "França",               Group = "I", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 26, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m53", HomeTeam = "Senegal",             AwayTeam = "Iraque",               Group = "I", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 26, 19, 0, 0, DateTimeKind.Utc) },

            // Grupo J
            new Match { ExternalId = "m54", HomeTeam = "Argentina",           AwayTeam = "Argélia",              Group = "J", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 16, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m55", HomeTeam = "Áustria",             AwayTeam = "Jordânia",             Group = "J", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 17, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m56", HomeTeam = "Jordânia",            AwayTeam = "Argélia",              Group = "J", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 22, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m57", HomeTeam = "Argentina",           AwayTeam = "Áustria",              Group = "J", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 22, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m58", HomeTeam = "Jordânia",            AwayTeam = "Argentina",            Group = "J", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 27, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m59", HomeTeam = "Argélia",             AwayTeam = "Áustria",              Group = "J", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 27, 19, 0, 0, DateTimeKind.Utc) },

            // Grupo K
            new Match { ExternalId = "m60", HomeTeam = "Portugal",            AwayTeam = "RD Congo",             Group = "K", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 17, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m61", HomeTeam = "Uzbequistão",         AwayTeam = "Colômbia",             Group = "K", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 17, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m62", HomeTeam = "Colômbia",            AwayTeam = "RD Congo",             Group = "K", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 23, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m63", HomeTeam = "Portugal",            AwayTeam = "Uzbequistão",          Group = "K", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 23, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m64", HomeTeam = "Colômbia",            AwayTeam = "Portugal",             Group = "K", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 27, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m65", HomeTeam = "RD Congo",            AwayTeam = "Uzbequistão",          Group = "K", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 27, 19, 0, 0, DateTimeKind.Utc) },

            // Grupo L
            new Match { ExternalId = "m66", HomeTeam = "Inglaterra",          AwayTeam = "Croácia",              Group = "L", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 17, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m67", HomeTeam = "Gana",                AwayTeam = "Panamá",               Group = "L", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 18, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m68", HomeTeam = "Panamá",              AwayTeam = "Croácia",              Group = "L", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 23, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m69", HomeTeam = "Inglaterra",          AwayTeam = "Gana",                 Group = "L", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 23, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m70", HomeTeam = "Panamá",              AwayTeam = "Inglaterra",           Group = "L", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 27, 19, 0, 0, DateTimeKind.Utc) },
            new Match { ExternalId = "m71", HomeTeam = "Croácia",             AwayTeam = "Gana",                 Group = "L", Round = MatchRound.Group, MatchDate = new DateTime(2026, 6, 27, 19, 0, 0, DateTimeKind.Utc) },
        };

        await context.Matches.AddRangeAsync(matches);

        if (!context.Tournaments.Any())
        {
            await context.Tournaments.AddAsync(new Tournament { Season = 2026, IsActive = true, CurrentPhase = TournamentPhase.PreTournament });
        }

        await context.SaveChangesAsync();
    }
}
