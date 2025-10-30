using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceHelper.Dependencies
{
    public class EntityType
    {
        public const char SEPERATOR = ':';

        public static EntityType Centre = new EntityType("centre");

        public static EntityType CentreInventoryEntry = new EntityType(Centre, "entry");

        public static EntityType Vehicle = new EntityType("vehicle");

        public static EntityType VehicleInventoryEntry = new EntityType(Vehicle, "entry");

        public static EntityType DRSCollection = new EntityType("drscollection");

        public static EntityType Reward = new EntityType("reward");

        public static EntityType MaterialRate = new EntityType("materialrate");

        public static EntityType Wallet = new EntityType("wallet");

        public static EntityType WalletTransaction = new EntityType(Wallet, "transaction");

        public static EntityType UserWallet = new EntityType("userwallet");

        public static EntityType UserWalletTransaction = new EntityType(UserWallet, "transaction");

        public static IReadOnlyList<EntityType> List { get; } = new List<EntityType>
    {
        Centre, Vehicle, CentreInventoryEntry, VehicleInventoryEntry, DRSCollection, Reward, MaterialRate, Wallet, WalletTransaction, UserWallet,
        UserWalletTransaction
    };


        public string Value { get; }

        public EntityType? Parent { get; }

        private EntityType(string name)
        {
            Value = name;
        }

        private EntityType(EntityType parent, string name)
        {
            Parent = parent;
            Value = parent.Value + ":" + name;
        }
    }
}
