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
        public async Task<List<TodoItem>> GetTodoItemsAsync()
        {
            //var response = await client.GetStringAsync("http://localhost:5000/api/todo/items");
            var response = await client.GetStringAsync("http://18.220.1.200/api/todos/QueryAll2");
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
            //var response = await client.PostAsync("http://localhost:5000/api/todo/item", content);
            var response = await client.PostAsync("http://18.220.1.200/api/todos/AddTodo", content);
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
            //var response = await client.PutAsync(string.Concat("http://localhost:5000/api/todo/", itemIndex), content);
            string url = "http://18.220.1.200/api/todos/UpdateTodo?id=" + itemIndex.ToString();
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
            //	await client.DeleteAsync(string.Concat("http://localhost:5000/api/todo/", itemIndex));
            // await client.DeleteAsync(string.Concat("http://anglicareapiserver.azurewebsites.net/api/todos/DeleteByID", itemIndex));
            string url = "http://18.220.1.200/api/todos/DeleteByID?id=" + itemIndex.ToString();
            // await client.DeleteAsync(string.Concat(url, itemIndex));
            await client.DeleteAsync(url);
        }
    }
}
