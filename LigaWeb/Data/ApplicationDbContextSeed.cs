using LigaWeb.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LigaWeb.Data
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<DataContext>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));                
            }

            if (!await roleManager.RoleExistsAsync("Employee"))
            {
                await roleManager.CreateAsync(new IdentityRole("Employee"));
            }

            if (!await roleManager.RoleExistsAsync("Club"))
            {
                await roleManager.CreateAsync(new IdentityRole("Club"));
            }

            if (!await roleManager.RoleExistsAsync("Anonimous"))
            {
                await roleManager.CreateAsync(new IdentityRole("Anonimous"));
            }

            // User Admin
            var adminUser = await userManager.FindByEmailAsync("bidelavitta1@gmail.com");
            if (adminUser == null)
            {
                
                var user = new User
                {
                    UserName = "bidelavitta1@gmail.com",
                    Email = "bidelavitta1@gmail.com",
                    EmailConfirmed = true,
                    FirstName = "Fabiola",
                    LastName = "Martins",                    
                    TwoFactorEnabled = false,
                };
                
                var result = await userManager.CreateAsync(user, "123456");
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            // User Employee
            var Employee = await userManager.FindByEmailAsync("employee@gmail.com");
            if (Employee == null)
            {

                var user = new User
                {
                    UserName = "employee@hotmail.com",
                    Email = "employee@hotmail.com",
                    EmailConfirmed = true,
                    FirstName = "Cristina",
                    LastName = "Andrade",
                    TwoFactorEnabled = false,
                };

                var result = await userManager.CreateAsync(user, "123456");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Employee");
                }
            }

            // User Club
            var clubUser = await userManager.FindByEmailAsync("clubuser@gmail.com");
            if (clubUser == null)
            {

                var user = new User
                {
                    UserName = "clubuser@gmail.com",
                    Email = "clubuser@gmail.com",
                    EmailConfirmed = true,
                    FirstName = "Juliana",
                    LastName = "Martins",
                    TwoFactorEnabled = false,
                };

                var result = await userManager.CreateAsync(user, "123456");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Club");
                }
            }


            // Seeding de Clubs
            await SeedClubsAsync(context);

            await SeedGamesAsync(context);
        }

        private static async Task SeedClubsAsync(DataContext context)
        {
            if (await context.Clubs.AnyAsync())
            {
                // Já existem clubes no banco de dados
                return;
            }

            var playersManchaster = new List<Player>();
            playersManchaster.Add(new Player { Name = "André Onana", Photo = "https://img.a.transfermarkt.technology/portrait/header/234509-1686929812.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersManchaster.Add(new Player { Name = "Altay Bayindir", Photo = "https://img.a.transfermarkt.technology/portrait/header/336077-1699471821.jpg?lm=1", YearOfBirth = 1998, Height = 198 });
            playersManchaster.Add(new Player { Name = "Matthijs de Ligt", Photo = "https://img.a.transfermarkt.technology/portrait/header/326031-1700659567.jpg?lm=1", YearOfBirth = 1993, Height = 194 });
            playersManchaster.Add(new Player { Name = "Harry Maguire", Photo = "https://img.a.transfermarkt.technology/portrait/header/177907-1663841733.jpg?lm=1", YearOfBirth = 1990, Height = 170 });
            playersManchaster.Add(new Player { Name = "Manuel Ugarte", Photo = "https://img.a.transfermarkt.technology/portrait/header/476701-1715107512.jpg?lm=1", YearOfBirth = 2001, Height = 182 });
            playersManchaster.Add(new Player { Name = "Christian Eriksen", Photo = "https://img.a.transfermarkt.technology/portrait/header/69633-1718628122.jpg?lm=1", YearOfBirth = 1992, Height = 182 });

            var playersBarcelona = new List<Player>();
            playersBarcelona.Add(new Player { Name = "Marc-André ter Stegen", Photo = "https://img.a.transfermarkt.technology/portrait/header/74857-1674465246.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersBarcelona.Add(new Player { Name = "Wojciech Szczęsny", Photo = "https://img.a.transfermarkt.technology/portrait/header/44058-1595847517.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersBarcelona.Add(new Player { Name = "Eric García", Photo = "https://img.a.transfermarkt.technology/portrait/header/466794-1693604801.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersBarcelona.Add(new Player { Name = "Alejandro Balde", Photo = "https://img.a.transfermarkt.technology/portrait/header/636688-1662836200.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersBarcelona.Add(new Player { Name = "Marc Bernal", Photo = "https://img.a.transfermarkt.technology/portrait/header/1018920-1724052540.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersBarcelona.Add(new Player { Name = "Frenkie de Jong", Photo = "https://img.a.transfermarkt.technology/portrait/header/326330-1656499973.png?lm=1", YearOfBirth = 1996, Height = 190 });

            var playersRealMadrid = new List<Player>();
            playersRealMadrid.Add(new Player { Name = "Thibaut Courtois", Photo = "https://img.a.transfermarkt.technology/portrait/header/108390-1717280733.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersRealMadrid.Add(new Player { Name = "Ferland Mendy", Photo = "https://img.a.transfermarkt.technology/portrait/header/291417-1701294025.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersRealMadrid.Add(new Player { Name = "Daniel Carvajal", Photo = "https://img.a.transfermarkt.technology/portrait/header/138927-1721026790.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersRealMadrid.Add(new Player { Name = "Luka Modric", Photo = "https://img.a.transfermarkt.technology/portrait/header/27992-1687776160.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersRealMadrid.Add(new Player { Name = "Brahim Díaz", Photo = "https://img.a.transfermarkt.technology/portrait/header/314678-1632307989.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersRealMadrid.Add(new Player { Name = "Vinicius Junior", Photo = "https://img.a.transfermarkt.technology/portrait/header/371998-1664869583.jpg?lm=1", YearOfBirth = 1996, Height = 190 });

            var playersBayern = new List<Player>();
            playersBayern.Add(new Player { Name = "Manuel Neuer", Photo = "https://img.a.transfermarkt.technology/portrait/header/17259-1702419450.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersBayern.Add(new Player { Name = "Min-jae Kim", Photo = "https://img.a.transfermarkt.technology/portrait/header/503482-1700208062.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersBayern.Add(new Player { Name = "Eric Dier", Photo = "https://img.a.transfermarkt.technology/portrait/header/175722-1665608595.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersBayern.Add(new Player { Name = "Raphaël Guerreiro", Photo = "https://img.a.transfermarkt.technology/portrait/header/170986-1711745629.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersBayern.Add(new Player { Name = "Sacha Boey", Photo = "https://img.a.transfermarkt.technology/portrait/header/475413-1706907003.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersBayern.Add(new Player { Name = "Joshua Kimmich", Photo = "https://img.a.transfermarkt.technology/portrait/header/161056-1700039639.jpg?lm=1", YearOfBirth = 1996, Height = 190 });

            var playersLiverpool = new List<Player>();
            playersLiverpool.Add(new Player { Name = "Alisson", Photo = "https://img.a.transfermarkt.technology/portrait/header/105470-1668522221.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersLiverpool.Add(new Player { Name = "Vitezslav Jaros", Photo = "https://img.a.transfermarkt.technology/portrait/header/486604-1696603392.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersLiverpool.Add(new Player { Name = "Andrew Robertson", Photo = "https://img.a.transfermarkt.technology/portrait/header/234803-1709147379.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersLiverpool.Add(new Player { Name = "Konstantinos Tsimikas", Photo = "https://img.a.transfermarkt.technology/portrait/header/338070-1682088468.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersLiverpool.Add(new Player { Name = "Conor Bradley", Photo = "https://img.a.transfermarkt.technology/portrait/header/624258-1708933100.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersLiverpool.Add(new Player { Name = "Luis Díaz", Photo = "https://img.a.transfermarkt.technology/portrait/header/480692-1697903145.jpg?lm=1", YearOfBirth = 1996, Height = 190 });

            var playersParis = new List<Player>();
            playersParis.Add(new Player { Name = "Gianluigi Donnarumma", Photo = "https://img.a.transfermarkt.technology/portrait/header/315858-1672304477.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersParis.Add(new Player { Name = "Willian Pacho", Photo = "https://img.a.transfermarkt.technology/portrait/header/661171-1696508666.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersParis.Add(new Player { Name = "Lucas Beraldo", Photo = "https://img.a.transfermarkt.technology/portrait/header/872171-1715107660.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersParis.Add(new Player { Name = "João Neves", Photo = "https://img.a.transfermarkt.technology/portrait/header/670681-1701295511.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersParis.Add(new Player { Name = "Vitinha", Photo = "https://img.a.transfermarkt.technology/portrait/header/487469-1672303629.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersParis.Add(new Player { Name = "Désiré Doué", Photo = "https://img.a.transfermarkt.technology/portrait/header/914562-1667317075.jpg?lm=1", YearOfBirth = 1996, Height = 190 });

            var playersMilan = new List<Player>();
            playersMilan.Add(new Player { Name = "Mike Maignan", Photo = "https://img.a.transfermarkt.technology/portrait/header/182906-1681459155.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersMilan.Add(new Player { Name = "Fikayo Tomori", Photo = "https://img.a.transfermarkt.technology/portrait/header/303254-1684856117.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersMilan.Add(new Player { Name = "Emerson Royal", Photo = "https://img.a.transfermarkt.technology/portrait/header/476344-1665609182.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersMilan.Add(new Player { Name = "Theo Hernández", Photo = "https://img.a.transfermarkt.technology/portrait/header/339808-1725532072.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersMilan.Add(new Player { Name = "Álex Jiménez", Photo = "https://img.a.transfermarkt.technology/portrait/header/741257-1689680048.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersMilan.Add(new Player { Name = "Filippo Terracciano", Photo = "https://img.a.transfermarkt.technology/portrait/header/631007-1663671440.jpg?lm=1", YearOfBirth = 1996, Height = 190 });

            var playersMilao = new List<Player>();
            playersMilao.Add(new Player { Name = "Josep Martínez", Photo = "https://img.a.transfermarkt.technology/portrait/header/388516-1686153132.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersMilao.Add(new Player { Name = "Raffaele Di Gennaro", Photo = "https://img.a.transfermarkt.technology/portrait/header/70580-1692961707.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersMilao.Add(new Player { Name = "Benjamin Pavard", Photo = "https://img.a.transfermarkt.technology/portrait/header/353366-1713819215.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersMilao.Add(new Player { Name = "Carlos Augusto", Photo = "https://img.a.transfermarkt.technology/portrait/header/585982-1725531744.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersMilao.Add(new Player { Name = "Denzel Dumfries", Photo = "https://img.a.transfermarkt.technology/portrait/header/321528-1725532091.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersMilao.Add(new Player { Name = "Nicolò Barella", Photo = "https://img.a.transfermarkt.technology/portrait/header/255942-1725531625.jpg?lm=1", YearOfBirth = 1996, Height = 190 });

            var playersJuventus = new List<Player>();
            playersJuventus.Add(new Player { Name = "Mattia Perin", Photo = "https://img.a.transfermarkt.technology/portrait/header/110923-1695107834.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersJuventus.Add(new Player { Name = "Bremer", Photo = "https://img.a.transfermarkt.technology/portrait/header/516716-1725531520.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersJuventus.Add(new Player { Name = "Danilo", Photo = "https://img.a.transfermarkt.technology/portrait/header/145707-1663577215.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersJuventus.Add(new Player { Name = "Juan Cabal", Photo = "https://img.a.transfermarkt.technology/portrait/header/686965-1692964112.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersJuventus.Add(new Player { Name = "Nicolò Savona", Photo = "https://img.a.transfermarkt.technology/portrait/header/708072-1655243945.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersJuventus.Add(new Player { Name = "Manuel Locatelli", Photo = "https://img.a.transfermarkt.technology/portrait/header/265088-1630490000.jpg?lm=1", YearOfBirth = 1996, Height = 190 });

            var playersChelsea = new List<Player>();
            playersChelsea.Add(new Player { Name = "Robert Sánchez", Photo = "https://img.a.transfermarkt.technology/portrait/header/403151-1695028195.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersChelsea.Add(new Player { Name = "Levi Colwill", Photo = "https://img.a.transfermarkt.technology/portrait/header/614258-1694614507.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersChelsea.Add(new Player { Name = "Wesley Fofana", Photo = "https://img.a.transfermarkt.technology/portrait/header/475411-1683899212.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersChelsea.Add(new Player { Name = "Moisés Caicedo", Photo = "https://img.a.transfermarkt.technology/portrait/header/687626-1660729724.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersChelsea.Add(new Player { Name = "Enzo Fernández", Photo = "https://img.a.transfermarkt.technology/portrait/header/648195-1669894717.jpg?lm=1", YearOfBirth = 1996, Height = 190 });
            playersChelsea.Add(new Player { Name = "Cole Palmer", Photo = "https://img.a.transfermarkt.technology/portrait/header/568177-1712320986.jpg?lm=1", YearOfBirth = 1996, Height = 190 });

            var clubs = new List<Club>
            {
                    new Club { Name = "Manchester United", Nickname = "Red Devils", YearOfFoundation = 1878, Location = "Manchester, Inglaterra", Coach = "Erik ten Hag", TeamEmblem="https://tmssl.akamaized.net//images/wappen/head/985.png",
                        Stadium = new Stadium {Name = "Old Trafford", Address = "Old Trafford, Manchester M16 0RA, UK", Capacity = 74140 }, Players = playersManchaster},

                    new Club { Name = "FC Barcelona", Nickname = "Blaugrana", YearOfFoundation = 1899, Location = "Barcelona, Espanha", Coach = "Xavi Hernandez", TeamEmblem = "https://tmssl.akamaized.net//images/wappen/head/2464.png",
                        Stadium = new Stadium {Name = "Camp Nou", Address = "Carrer d'Aristides Maillol, 08028 Barcelona, Espanha", Capacity = 99354}, Players = playersBarcelona},

                    new Club { Name = "Real Madrid", Nickname = "Los Blancos", YearOfFoundation = 1902, Location = "Madrid, Espanha", Coach = "Carlo Ancelotti", TeamEmblem = "https://tmssl.akamaized.net//images/wappen/head/92878.png",
                        Stadium = new Stadium {Name = "Santiago Bernabéu", Address = "\tAv. de Concha Espina, 1, 28036 Madrid, Espanha", Capacity = 81044}, Players = playersRealMadrid },

                    new Club { Name = "Bayern Munich", Nickname = "Die Roten", YearOfFoundation = 1900, Location = "Munique, Alemanha", Coach = "Thomas Tuchel", TeamEmblem = "https://tmssl.akamaized.net//images/wappen/head/27.png",
                        Stadium = new Stadium {Name = "Allianz Arena", Address = "Werner-Heisenberg-Allee 25, 80939 München, Alemanha", Capacity = 7500}, Players = playersBayern },

                    new Club { Name = "Liverpool FC", Nickname = "Reds", YearOfFoundation = 1892, Location = "Liverpool, Inglaterra", Coach = "Jürgen Klopp", TeamEmblem = "https://tmssl.akamaized.net//images/wappen/head/31.png",
                        Stadium = new Stadium {Name = "Anfield", Address = "Anfield Rd, Liverpool L4 0TH, UK", Capacity = 53394}, Players = playersLiverpool },

                    new Club { Name = "Paris Saint-Germain", Nickname = "Les Parisiens", YearOfFoundation = 1970, Location = "Paris, França", Coach = "Luis Enrique", TeamEmblem = "https://tmssl.akamaized.net//images/wappen/head/583.png",
                        Stadium = new Stadium {Name = "Parc des Princes", Address = "24 Rue Edgar Faure, 75015 Paris, França", Capacity = 48712}, Players = playersParis },

                    new Club { Name = "AC Milan", Nickname = "Rossoneri", YearOfFoundation = 1899, Location = "Milão, Itália", Coach = "Stefano Pioli", TeamEmblem = "https://tmssl.akamaized.net//images/wappen/head/5.png",
                        Stadium = new Stadium {Name = "San Siro", Address = "Piazza del Duomo, 20147 Milano MI, Itália", Capacity = 75923}, Players = playersMilan },

                    new Club { Name = "Inter de Milão", Nickname = "Nerazzurri", YearOfFoundation = 1908, Location = "Milão, Itália", Coach = "Simone Inzaghi", TeamEmblem = "https://tmssl.akamaized.net//images/wappen/head/46.png",
                        Stadium = new Stadium {Name = "San Siro", Address = "Piazza del Duomo, 20147 Milano MI, Itália", Capacity = 99354}, Players = playersMilao },

                    new Club { Name = "Juventus", Nickname = "La Vecchia Signora", YearOfFoundation = 1897, Location = "Turim, Itália", Coach = "Massimiliano Allegri", TeamEmblem = "https://tmssl.akamaized.net//images/wappen/head/506.png",
                        Stadium = new Stadium {Name = "Allianz Stadium", Address = "Corso Gaetano Scirea, 50, 10151 Torino TO, Itália", Capacity = 41507}, Players = playersJuventus },

                    new Club { Name = "Chelsea FC", Nickname = "Blues", YearOfFoundation = 1905, Location = "Londres, Inglaterra", Coach = "Mauricio Pochettino", TeamEmblem = "https://tmssl.akamaized.net//images/wappen/head/631.png",
                        Stadium = new Stadium {Name = "Stamford Bridge", Address = "Fulham Road, London SW6 1HS, UK", Capacity = 40834}, Players = playersChelsea }
            };

            await context.Clubs.AddRangeAsync(clubs);
            await context.SaveChangesAsync();
        }

        private static async Task SeedGamesAsync(DataContext context)
        {
            if (await context.Games.AnyAsync())
            {
                // Já existem jogos no banco de dados
                return;
            }

            var clubs = await context.Clubs.ToListAsync();
            var random = new Random();

            var games = new List<Game>();

            for (int i = 0; i < clubs.Count; i++)
            {
                for (int j = 0; j < clubs.Count; j++)
                {
                    if (i == j)
                        continue; // Evita que um clube jogue contra ele mesmo

                    var hostClub = clubs[i];
                    var visitingClub = clubs[j];

                    // Geração de Estatísticas
                    int hostGoals = random.Next(0, 4); // 0 a 3
                    int visitingGoals = random.Next(0, 4); // 0 a 3

                    // Garantir pelo menos 1 cartão vermelho por partida
                    int totalRedCards = 1; // Pode ser aumentado se desejar
                    // Distribuir os cartões vermelhos entre os clubes
                    int hostRedCards = random.Next(0, totalRedCards + 1); // 0 ou 1
                    int visitingRedCards = totalRedCards - hostRedCards;

                    // Gerar entre 2 a 4 cartões amarelos por partida
                    int totalYellowCards = random.Next(2, 5); // 2 a 4
                    int hostYellowCards = random.Next(0, totalYellowCards + 1);
                    int visitingYellowCards = totalYellowCards - hostYellowCards;

                    // Chutes a Gol: 5 a 20 por clube
                    int hostGoalKicks = random.Next(5, 21);
                    int visitingGoalKicks = random.Next(5, 21);

                    // Pênaltis: 0 a 2 por clube
                    int hostPenalties = random.Next(0, 3);
                    int visitingPenalties = random.Next(0, 3);

                    // Data do jogo: Aleatória no passado ou futuro
                    // Para simplificação, definir algumas datas fixas ou aleatórias
                    DateTime gameDate = DateTime.Now.AddDays(random.Next(-100, 100));

                    var game = new Game
                    {
                        Date = gameDate,
                        StadiumId = hostClub.StadiumId ?? context.Stadiums.First().Id, // Usa o estádio do clube anfitrião ou o primeiro disponível
                        HostClubId = hostClub.Id,
                        VisitingClubId = visitingClub.Id,
                        HostsGoals = hostGoals,
                        VistorsGoals = visitingGoals,
                        HostsRedCards = hostRedCards,
                        VistorsRedCards = visitingRedCards,
                        HostsYellowCards = hostYellowCards,
                        VistorsYellowCards = visitingYellowCards,
                        HostsGoalKicks = hostGoalKicks,
                        VistorsGoalKicks = visitingGoalKicks,
                        HostsPenalties = hostPenalties,
                        VistorsPenalties = visitingPenalties,
                        HostsFouls = random.Next(10, 21), // Exemplo: 10 a 20 faltas
                        VistorsFouls = random.Next(10, 21)
                    };

                    games.Add(game);
                }
            }

            await context.Games.AddRangeAsync(games);
            await context.SaveChangesAsync();
        }
    }
}
