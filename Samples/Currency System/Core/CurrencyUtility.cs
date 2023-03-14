using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Voxelity.Currency
{
    public static class CurrencyUtility
    {
        private static Dictionary<string, CurrencyManager> managers = new Dictionary<string, CurrencyManager>();

        public static void AssignManager(CurrencyManager manager)
        {
            if (manager.currencyName == null)
            {
                Debug.LogError(manager.name + " currency name must be defined in order to use this manager.");
                return;
            }
            managers.Add(manager.currencyName, manager);
        }
        public static void RemoveManager(CurrencyManager manager)
        {
            if (managers.ContainsValue(manager))
            {
                managers.Remove(manager.currencyName);
            }
        }
        public static CurrencyManager GetCurrency(string nameOfCurrency)
        {
            if (managers.ContainsKey(nameOfCurrency))
            {
                return managers[nameOfCurrency];
            }
            throw new System.ArgumentNullException(nameOfCurrency + " couldn't be found.");
        }
        public static int ValueOf(string nameOfCurrency)
        {
            if (managers.ContainsKey(nameOfCurrency))
            {
                return managers[nameOfCurrency].Value;
            }
            throw new System.ArgumentNullException(nameOfCurrency + " couldn't be found.");
        }
    }
}
