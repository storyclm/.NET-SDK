using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Models
{
    /// <summary>
    /// Профиль пользователя
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// Идендификатор записи
        /// Зависит от провайдера таблиц
        /// </summary>
        public string _id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Возраст
        /// </summary>
        public long Age { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        public bool Gender { get; set; }

        /// <summary>
        /// Рейтинг
        /// </summary>
        public double Rating { get; set; }

        /// <summary>
        /// Дата регистрации
        /// </summary>
        public DateTime Created { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Profile);
        }

        public bool Equals(Profile p)
        {
            if (Object.ReferenceEquals(p, null)) return false;
            if (Object.ReferenceEquals(this, p)) return true;
            if (this.GetType() != p.GetType()) return false;
            return (_id == p._id) && (Name == p.Name) && (Age == p.Age) && (Gender == p.Gender) && (Rating == p.Rating);
        }


        #region TestData

        public static IEnumerable<Profile> UpdateProfiles(IEnumerable<Profile> o)
        {
            return o.Select(t => UpdateProfile(t));
        }

        /// <summary>
        /// Обновляет профиль
        /// </summary>
        /// <param name="o">Исходный профиль</param>
        /// <returns>Обновленный профиль</returns>
        public static Profile UpdateProfile(Profile o)
        {
            return new Profile()
            {
                _id = o._id,
                Name = "Anna",
                Age = 33,
                Gender = false,
                Rating = 3.3D,
                Created = DateTime.Now
            };
        }

        /// <summary>
        /// Создает коллекцию профилей
        /// </summary>
        /// <returns>Коллекция профилей</returns>
        public static IEnumerable<Profile> CreateProfiles()
        {
            List<Profile> result = new List<Profile>();
            for (int i = 0; i < 3; i++)
                result.Add(CreateProfile());
            return result;
        }

        /// <summary>
        /// Создает новый профимль
        /// </summary>
        /// <returns></returns>
        public static Profile CreateProfile()
        {
            Profile test = new Profile()
            {
                Name = "Vladimir",
                Age = 22,
                Gender = true,
                Rating = 2.2D,
                Created = DateTime.Now
            };
            return test;
        }

        /// <summary>
        /// Создает новый профиль
        /// </summary>
        /// <returns>Профиль</returns>
        public static Profile CreateProfile1()
        {
            Profile test = new Profile()
            {
                Name = "Valentina",
                Age = 22,
                Gender = false,
                Rating = 2.2D,
                Created = DateTime.Now
            };
            return test;
        }

        /// <summary>
        /// Создает новый профиль
        /// </summary>
        /// <returns>Профиль</returns>
        public static Profile CreateProfile2()
        {
            Profile test = new Profile()
            {
                Name = "Vladimir",
                Age = 28,
                Gender = true,
                Rating = 2.2D,
                Created = DateTime.Now
            };
            return test;
        }
        /// <summary>
        /// Создает новый профиль
        /// </summary>
        /// <returns>Профиль</returns>
        public static Profile CreateProfile3()
        {
            Profile test = new Profile()
            {
                Name = "Stanislav",
                Age = 22,
                Gender = true,
                Rating = 2.2D,
                Created = DateTime.Now
            };
            return test;
        }

       public static Profile CreateProfile4()
        {
            Profile test = new Profile()
            {
                Name = "Tamerlan",
                Age = 22,
                Gender = true,
                Rating = 2.2D,
                Created = DateTime.Now
            };
            return test;
        }



        #endregion

    }
}
