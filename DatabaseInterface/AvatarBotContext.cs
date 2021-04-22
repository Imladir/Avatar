using AvatarBot.Model.Servers;
using AvatarBot.Model.Characters;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using GenericServices;
using System.Collections;
using AvatarBot.Model.Identity;
using Microsoft.AspNetCore.Identity;

namespace AvatarBot.Model
{
    public class AvatarBotContext : DbContext
    {
        public static string ConnectionString;
        // Characters
        public DbSet<Character> Characters { get; set; }
        public DbSet<PC> PCs { get; set; }
        public DbSet<NPC> NPCs { get; set; }

        //Servers
        public DbSet<Server> Servers { get; set; }
        // public DbSet<DefaultCharacter> DefaultCharacters { get; set; }
        public DbSet<PrivateChannel> PrivateChannels { get; set; }
        public DbSet<Message> Messages { get; set; }

        // Identity
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }


        public AvatarBotContext(DbContextOptions<AvatarBotContext> options)
          : base(options)
        {
        }

        public AvatarBotContext() : base()
        {
            //Important performance code
            //this.Configuration.AutoDetectChangesEnabled = true;
            //this.Configuration.LazyLoadingEnabled = false;
            //this.Configuration.ProxyCreationEnabled = false;
        }

        static AvatarBotContext()
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<AvatarBotContext, AvatarBotInitializer>());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString)
                .EnableDetailedErrors()
                .UseLazyLoadingProxies(true)
                .EnableSensitiveDataLogging()
                .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.QueryClientEvaluationWarning));
            base.OnConfiguring(optionsBuilder);
        }

        public static AvatarBotContext Create()
        {
            return new AvatarBotContext();
        }
        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<DefaultCharacter>().HasKey(x => new { x.ServerID, x.PlayerID });
            mb.Entity<PrivateChannel>().HasKey(x => new { x.PlayerID, x.ServerID });
            mb.Entity<IdentityUserLogin<string>>().HasKey(x => x.UserId);
            mb.Entity<IdentityUserToken<string>>().HasKey(x => x.UserId);
            mb.Entity<UserRole>().HasKey(x => new { x.UserID, x.RoleID });

            // mb.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            foreach (var relationship in mb.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(mb);
        }

        public bool HasPrivilege(int serverID, int userID)
        {
            return Users.Single(x => x.ID == userID).Roles.Where(x => x.Server.ID == serverID).Any(x => x.Role.Name.Equals("GM")
                                                                                                     || x.Role.Name.Equals("Admin"));
        }

        public bool CheckPrivateRights(int serverID, int userID, int characterID)
        {
            var c = Characters.SingleOrDefault(x => x.ID == characterID);
            var hasPrivilege = HasPrivilege(serverID, userID);

            if (c is PC pc) return pc.Player.ID == (long)userID || hasPrivilege;
            else return hasPrivilege;
        }

        public bool CheckPrivateChannel(ulong serverID, ulong playerID, ulong chanID, ulong ownerID)
        {
            var privateC = Users.Single(x => x.DiscordID == (long)playerID).PrivateChannels.SingleOrDefault(x => x.Server.DiscordID == (long)serverID);
            if (privateC != null) return privateC.ChannelDiscordID == (long)chanID;

            // Check if user has privilege and is on owner's channel
            var hasPrivilege = Users.Single(x => x.DiscordID == (long)playerID).Roles.Where(x => x.Server.DiscordID == (long)serverID).Any(x => x.Role.Name.Equals("GM") 
                                                                                                                                             || x.Role.Name.Equals("Admin"));
            if (!hasPrivilege) return false;

            var privateO = Users.Single(x => x.DiscordID == (long)ownerID).PrivateChannels.SingleOrDefault(x => x.Server.DiscordID == (long)serverID);
            if (privateO == null) return false;
            else return privateO.ChannelDiscordID == (long)chanID;
        }

        public PC GetDefaultCharacter(ulong serverID, ulong userID) {
            var defPC = Users.SingleOrDefault(x => x.DiscordID == (long)userID).DefaultCharacters.SingleOrDefault(x => x.Server.DiscordID == (long)serverID);
            if (defPC == null) return null;
            else return defPC.Character as PC;
        }

        public PC GetPlayerCharacter(ulong serverID, ulong userID, string nameOrAlias)
        {
            try
            {
                if (nameOrAlias == "") return GetDefaultCharacter(serverID, userID);

                var user = Users.SingleOrDefault(x => x.DiscordID == (long)userID);
                // Does the user have privilegies?
                if (!user.Roles.Where(x => x.Server.DiscordID == (long)serverID).Any(x => x.Role.Name.Equals("GM") || x.Role.Name.Equals("Admin") || x.Role.Name.Equals("ServerOwner")))
                    return PCs.SingleOrDefault(x => x.Player.DiscordID == (long)userID 
                                                 && x.Server.DiscordID == (long)serverID 
                                                 && (x.Name.Equals(nameOrAlias) || x.Alias.Equals(nameOrAlias)));
                else
                    return PCs.SingleOrDefault(x => x.Server.DiscordID == (long)serverID
                                                 && (x.Name.Equals(nameOrAlias) || x.Alias.Equals(nameOrAlias)));
            } catch (Exception e)
            {
                Console.WriteLine($"Error retrieving character: {e.Message}\n{e.StackTrace}");
                throw;
            }
        }

        public Character GetCommandTarget(ulong serverID, ulong userID, string aliasOrName = "")
        {
            Character target;
            var user = Users.Single(x => x.DiscordID == (long)userID);
            if (user.Roles.Where(x => x.Server.DiscordID == (long)serverID).Any(x => x.Role.Name.Equals("GM") || x.Role.Name.Equals("Admin") || x.Role.Name.Equals("ServerOwner")) 
                && aliasOrName != "")
                target = Characters.Where(x => (x.Name.Equals(aliasOrName) || x.Alias.Equals(aliasOrName))
                                             && (x.Server.DiscordID == (long)serverID || x.Server.DiscordID == 0)).Single();
            else
                target = GetPlayerCharacter(serverID, userID, aliasOrName);
            return target;
        }

        public List<User> GetGM(ulong serverID)
        {
            return Set<UserRole>().Where(x => x.Server.DiscordID == (long)serverID && x.Role.Name.Equals("GM")).Select(x => x.User).ToList();
        }

        #region Save Changes
        public override int SaveChanges()
        {
            try
            {
                var aliasChangeSet = ChangeTracker.Entries<Character>();
                if (aliasChangeSet != null)
                {
                    foreach (var entry in aliasChangeSet.Where(x => x.State != EntityState.Unchanged))
                    {
                        var query =
                            from s in Servers
                            join a in Characters on s.ID equals a.Server.ID
                            where a.ID == entry.Entity.ID
                            select s;
                        var server = query.AsNoTracking().SingleOrDefault();

                        if (server == null) continue; // Seeding
                        var na = Characters.AsNoTracking().SingleOrDefault(x => (x.Server.ID == server.ID || x.Server.DiscordID == 0) && x.Alias.Equals(entry.Entity.Alias));
                        if (na != null && na.ID != entry.Entity.ID)
                            throw new ValidationException($"there is already a character registered with the alias '{na.Alias}'. Pick something else.");
                    }
                }

                var userChanges = ChangeTracker.Entries<User>().Where(x => x.State != EntityState.Unchanged).ToList();
                if (userChanges != null)
                {
                    for (int i = 0; i < userChanges.Count; i++)
                    {
                        userChanges[i].Entity.LastUpdate = DateTime.UtcNow;
                    }
                }

                var res = base.SaveChanges();

                return res;
            }
            catch (DbUpdateException e)
            {
                //This either returns a error string, or null if it can’t handle that error
                var error = SaveChangesExceptionHandler(e, this);
                if (error != null)
                {
                    Console.WriteLine(error); //return the error string
                } else
                {
                    Console.WriteLine($"That's a database exception I don't know what to do with: {e.Message}");
                }
                throw; //couldn’t handle that error, so rethrow
            }
            catch (Exception e)
            {
                if (e is ValidationException)
                    Console.WriteLine($"{(e as ValidationException).ValidationAttribute} with value {(e as ValidationException).Value} failed: {(e as ValidationException).ValidationResult}\n{e.Message}\n{e.StackTrace}");
                else
                    Console.WriteLine($"Exception in SaveChanges: \n{e.Message}\n{e.StackTrace}");
                var inner = e.InnerException;
                while (inner != null)
                {
                    if (e is ValidationException)
                        Console.WriteLine($"{(e as ValidationException).ValidationAttribute} with value {(e as ValidationException).Value} failed: {(e as ValidationException).ValidationResult}\n{e.Message}\n{e.StackTrace}");
                    else
                        Console.WriteLine($"Exception in SaveChanges: \n{e.Message}\n{e.StackTrace}");
                    inner = inner.InnerException;
                }
                throw;
            }
        }

        public const int SqlServerViolationOfUniqueIndex = 2601;
        public const int SqlServerViolationOfUniqueConstraint = 2627;

        public static IStatusGeneric SaveChangesExceptionHandler(Exception e, DbContext context)
        {
            var dbUpdateEx = e as DbUpdateException;
            if (dbUpdateEx?.InnerException is SqlException sqlEx)
            {
                Console.WriteLine($"SQL Exception #{sqlEx.Number}: {sqlEx.Message}");
                //This is a DbUpdateException on a SQL database

                //if (sqlEx.Number == SqlServerViolationOfUniqueIndex ||
                //    sqlEx.Number == SqlServerViolationOfUniqueConstraint)
                //{
                //We have an error we can process
                var valError = UniqueErrorFormatter(sqlEx, dbUpdateEx.Entries);
                if (valError != null)
                {
                    var status = new StatusGenericHandler();
                    status.AddValidationResult(valError);
                    return status;
                }
                //else check for other SQL errors
                //}
            }
            return null;
        }

        private static readonly Regex UniqueConstraintRegex = new Regex("'UniqueError_([a-zA-Z0-9]*)_([a-zA-Z0-9]*)'", RegexOptions.Compiled);

        public static ValidationResult UniqueErrorFormatter(SqlException ex, IReadOnlyList<EntityEntry> entitiesNotSaved)
        {
            var message = ex.Errors[0].Message;
            var matches = UniqueConstraintRegex.Matches(message);

            if (matches.Count == 0)
                return null;

            //currently the entitiesNotSaved is empty for unique constraints - see https://github.com/aspnet/EntityFrameworkCore/issues/7829
            var entityDisplayName = entitiesNotSaved.Count == 1
                ? entitiesNotSaved.Single().Entity.GetType().FullName
                : matches[0].Groups[1].Value;

            var returnError = "Cannot have a duplicate " +
                              matches[0].Groups[2].Value + " in " +
                              entityDisplayName + ".";

            var openingBadValue = message.IndexOf("(");
            if (openingBadValue > 0)
            {
                var dupPart = message.Substring(openingBadValue + 1,
                    message.Length - openingBadValue - 3);
                returnError += $" Duplicate value was '{dupPart}'.";
            }

            return new ValidationResult(returnError, new[] { matches[0].Groups[2].Value });
        }
        #endregion

        public void Seed()
        {
            try
            {
                Server server = new Server() { Prefix = "!", DiscordID = 0, Name = "Game Data Server" };
                Servers.Add(server);
                SaveChanges();

                IList<Role> roles = new List<Role>()
                {
                    new Role() { Name = "Admin", Description = "Application / Bot Admin" },
                    new Role() { Name = "ServerOwner", Description = "Discord Server Owner" },
                    new Role() { Name = "GM", Description = "Game Master" }
                };
                roles[0].Claims.Add(new RoleClaim() { Role = roles[0], ClaimType = "AuthorizationLevel", ClaimValue = "Admin" });
                roles[1].Claims.Add(new RoleClaim() { Role = roles[1], ClaimType = "AuthorizationLevel", ClaimValue = "ServerOwner" });
                roles[2].Claims.Add(new RoleClaim() { Role = roles[2], ClaimType = "AuthorizationLevel", ClaimValue = "GM" });
                Roles.AddRange(roles);
                SaveChanges();

                // Everything went fine so commit
            }
            catch (Exception e)
            {
                Console.WriteLine($"Seeding failed: {e.Message}\n{e.StackTrace}");
            }
        }
    }
}
