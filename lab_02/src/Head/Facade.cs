using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

using bl;
using db;

namespace Head
{
	public class Facade
	{
		IFacade conFacade;
		ILogger<Head.Facade> _loggerFacade;

		public Facade()
		{
			conFacade = new ConFacade();
			_loggerFacade = null;
		}
		// public Facade(ILogger<Head.Facade> loggerFacade)
		// {
		// 	conFacade = new MySQLFacade();
		// 	_loggerFacade = loggerFacade;
		// }

		public Facade(ILogger<Head.Facade> loggerFacade)
		{
			conFacade = new ConFacade();
			_loggerFacade = loggerFacade;
		}
		public List<bl.CompletedTask> GetCompletedTasks()
		{
			return conFacade.GetCompletedTasks();
		}

		public List<bl.User> GetUsers()
		{
			return conFacade.GetUsers();
		}

		public List<bl.Task> GetTasks()
		{
			return conFacade.GetTasks();
		}

		public int AddCompletedTask(bl.CompletedTask completedTask)
		{
			conFacade.AddCompletedTask(completedTask);
			return 0;
		}

		Head.Answer CheckPassword(string password)
		{
			if (password.Length < 6)
				return new Head.Answer((int)Constants.Errors.ShortLengthPassword, "Длина пароля должна быть больше 5 символов");

			if (Extensions.IsNumeric(password))
				return new Head.Answer((int)Constants.Errors.OnlyNumericPassword, "Пароль не должен состоять только из цифр");

			return new Head.Answer(Constants.OK, "Ok");
		}

		public Head.Answer AddUser(bl.User user)
		{
			bl.User userOld = conFacade.GetUserByEmail(user.Email);
			if (userOld != null)
				return new Head.Answer((int)Constants.Errors.EmailUserExists, "Данный email занят");

			userOld = conFacade.GetUserByLogin(user.Login);
			if (userOld != null)
				return new Head.Answer((int)Constants.Errors.LoginUserExists, "Данный логин занят");


			Head.Answer checkPassword = CheckPassword(user.Password);
			if (checkPassword.returnValue != Constants.OK)
			{
				return checkPassword;
			}

			conFacade.AddUser(user);
			return new Head.Answer(Constants.OK, "Ok");
		}

		public Head.Answer AddTask(bl.Task task)
		{
			conFacade.AddTask(task);
			return new Head.Answer(Constants.OK, "Ok");
		}

		public bl.CompletedTask GetCompletedTask(int id)
		{
			return conFacade.GetCompletedTask(id);
		}

		public bl.Task GetTask(int id)
		{
			bl.Task task = null;
			try
			{
				task = conFacade.GetTask(id);
			}
			catch (Exception e)
			{
				_loggerFacade.LogError($"data: id = {id}\n{e.Message}");
			}
			return task;
		}

		public bl.User GetUser(int id)
		{
			return conFacade.GetUser(id);
		}

		public bl.User GetUserByLogin(string login)
		{
			return conFacade.GetUserByLogin(login);
		}

	}
}
