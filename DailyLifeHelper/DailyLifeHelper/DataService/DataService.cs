using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using DailyLifeHelper.Models;

namespace DailyLifeHelper.DataService
{
    public class DataService
    {
        HttpClient client = new HttpClient();
        public DataService()
        {
        }

        /// <summary>
        /// Gets the todo items async.
        /// </summary>
        /// <returns>The todo items async.</returns>
        /// change a bit to pick up right todo for specifc USERNAME
        public async Task<List<TodoItem>> GetTodoItemsAsync(string username)        {
         
            var response = await client.GetStringAsync("http://18.220.1.200/api/newtodos/QueryAll2?username="+username);
            var todoItems = JsonConvert.DeserializeObject<List<TodoItem>>(response);
            return todoItems;
        }

        /// <summary>
        /// Adds the todo item async.
        /// </summary>
        /// <returns>The todo item async.</returns>
        /// <param name="itemToAdd">Item to add.</param>
        public async Task<int> AddTodoItemAsync(TodoItem itemToAdd)
        {
            var data = JsonConvert.SerializeObject(itemToAdd);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://18.220.1.200/api/newtodos/AddTodo", content);
            // var result = JsonConvert.DeserializeObject<int>(response.Content.ReadAsStringAsync().Result);
            //return result;
            return 1;
        }

        /// <summary>
        /// Updates the todo item async.
        /// </summary>
        /// <returns>The todo item async.</returns>
        /// <param name="itemIndex">Item index.</param>
        /// <param name="itemToUpdate">Item to update.</param>
        public async Task<int> UpdateTodoItemAsync(int itemIndex, TodoItem itemToUpdate)
        {
            var data = JsonConvert.SerializeObject(itemToUpdate);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
         
            string url = "http://18.220.1.200/api/newtodos/UpdateTodo?id=" + itemIndex.ToString();
            var response = await client.PutAsync(url, content);
            //return JsonConvert.DeserializeObject<int>(response.Content.ReadAsStringAsync().Result);
            return 1;
        }

        /// <summary>
        /// Deletes the todo item async.
        /// </summary>
        /// <returns>The todo item async.</returns>
        /// <param name="itemIndex">Item index.</param>
        public async Task DeleteTodoItemAsync(int itemIndex)
        {            
            string url = "http://18.220.1.200/api/newtodos/DeleteByID?id=" + itemIndex.ToString();
            await client.DeleteAsync(url);
        }


        //send password to email api
       public async Task<int> SendPasswordJustCheckemail(EmailUser newemailto)
        {
           var data = JsonConvert.SerializeObject(newemailto);
           var content = new StringContent(data, Encoding.UTF8, "application/json");
           var response = await client.PostAsync("http://18.220.1.200/api/email/SendPasswordJustCheckemail", content);
           var result = JsonConvert.DeserializeObject<int>(response.Content.ReadAsStringAsync().Result);
           return result;
         
       }



        //add new user API
        public async Task<int> AddNewUserAsync(newuser userAdd)
        {
            var data = JsonConvert.SerializeObject(userAdd);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://18.220.1.200/api/users/AddNewUser", content);
            var result = JsonConvert.DeserializeObject<int>(response.Content.ReadAsStringAsync().Result);
            return result;        
        }

        //check user name and passowrd
        public async Task<int> ValidUser(string username, string password)
        {
            string url = "http://18.220.1.200/api/users/LoginUserValidation?usernameoremail=" + username + "&password=" + password;
            var response = await client.GetAsync(url);
            var result = JsonConvert.DeserializeObject<int>(response.Content.ReadAsStringAsync().Result);
            return result;
        }
    }
}
