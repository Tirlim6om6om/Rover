using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace ITPRO.Rover.User
{
    public static class GameUsers
    {
        public static readonly UnityEvent<User> createUser = new UnityEvent<User>();
        public static readonly UnityEvent<User> deleteUser = new UnityEvent<User>();
        public static UnityEvent<User> editUser = new UnityEvent<User>();

        public static List<User> Users { get; } = new List<User>();

        private static string _path = Application.dataPath + @"/Users";
        
        public static void DeleteUser(User user)
        {
            deleteUser.Invoke(user);
            Users.Remove(user);
            File.Delete(_path + $"\\{user.ID}_{user.Name}.json");
        }

        public static void CreateUser(string name)
        {
            User user = new User()
            {
                ID = GetUniqueID(),
                Name = name
            };

            Users.Add(user);
            createUser.Invoke(user);
            SaveJson(user);
        }

        public static void CreateUser(User user)
        {
            Users.Add(user);
            createUser.Invoke(user);
        }

        public static void UpdateUser(User user)
        {
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].ID != user.ID) continue;

                Users[i] = user;
                editUser.Invoke(user);
                break;
            }
        }

        public static void SaveJson(User user)
        {
            string jsonString = JsonUtility.ToJson(user, true);
            
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
            
            File.WriteAllText(_path + $"\\{user.ID}_{user.Name}.json", jsonString);
        }

        public static void GetFolderUsers()
        {
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
                return;
            }
            
            string[] files = Directory.GetFiles(_path, "*_*.json");

            foreach (string file in files)
            {
                Debug.Log(file);
                string jsonString = File.ReadAllText(file);

                User user = JsonUtility.FromJson<User>(jsonString);

                if (!SearchingMatch(user.ID))
                {
                    Users.Add(user);
                }
            }
        }

        private static int GetUniqueID()
        {
            int max = Users.Select(user => user.ID).Prepend(0).Max();
            max += 1;
            return max;
        }

        private static bool SearchingMatch(int id)
        {
            foreach (var user in Users)
            {
                if (user.ID == id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}