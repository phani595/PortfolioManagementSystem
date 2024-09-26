using Domain.Entities;

namespace Infrastructure.SeedData
{
    public class SeedInitialData(PortfolioManagementDbContext dbContext) : ISeedInitialData
    {
        public async Task SeedData()
        {
            if (await dbContext.Database.CanConnectAsync())
            {
                if (!dbContext.Users.Any())
                {
                    var users = GetStaticUsers();

                    dbContext.Users.AddRange(users);
                    await dbContext.SaveChangesAsync();
                }

                if (!dbContext.Portfolios.Any())
                {
                    var portfolios = GetPortfolios();

                    dbContext.Portfolios.AddRange(portfolios);
                    await dbContext.SaveChangesAsync();
                }


            }
        }

        private IEnumerable<User> GetStaticUsers()
        {
            List<User> users = new List<User>
            {
                new User
                {
                    Username = "test1",
                    Password = "Password1",
                    Email = "test1@gmail.com",
                    Role = "Admin"
                },
                new User
                {
                    Username = "test2",
                    Password = "Password1",
                    Email = "test2@gmail.com",
                    Role = "User"

                },
                new User
                {
                    Username = "test3",
                    Password = "Password3",
                    Email = "test3@gmail.com",
                    Role = "Advisor"
                }
            };
            return users;
        }

        private ICollection<Portfolio> GetPortfolios()
        {
            List<Portfolio> portfolios = new List<Portfolio>
            {
                new Portfolio
                {
                    Name = "Portfolio1",
                    Assets = GetAssets()
                },
            };

            return portfolios;
        }


        private List<Asset> GetAssets()
        {

            List<Asset> assets = new List<Asset>
            {
                new Asset
                {
                    Name = "Asset1",
                    Type = "Stock",
                    CurrentMarketValue = 1000,
                    CostBasis = 800,
                    QuantityHeld = 10,
                    Transactions = new List<Transaction>
                    {
                        new Transaction
                        {
                            TransactionDate = DateTime.Now,
                            Quantity = 1,
                            Price = 150,
                            Type = "Buy"
                        }
                    }
                },
                new Asset
                {
                    Name = "Asset2",
                    Type = "Stock",
                    CurrentMarketValue = 2000,
                    CostBasis = 1800,
                    QuantityHeld = 20,
                    Transactions = new List<Transaction>
                    {
                        new Transaction
                        {
                            TransactionDate = DateTime.Now,
                            Quantity = 1,
                            Price = 150,
                            Type = "Buy",
                            Fees = 5
                        }
                    }
                },
                new Asset
                {
                    Name = "Asset3",
                    Type = "Stock",
                    CurrentMarketValue = 3000,
                    CostBasis = 2800,
                    QuantityHeld = 30,
                     Transactions = new List<Transaction>
                    {
                        new Transaction
                        {
                            TransactionDate = DateTime.Now,
                            Quantity = 1,
                            Price = 150,
                            Type = "Buy",
                            Fees = 5
                        }
                    }
                }

            };
            return assets;
        }
    }
}
