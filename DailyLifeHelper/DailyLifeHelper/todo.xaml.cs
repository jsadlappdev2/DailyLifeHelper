using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DailyLifeHelper.Models;
using DailyLifeHelper.DataService;

namespace DailyLifeHelper
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class todo : ContentPage
    {
        DataService.DataService dataService;
        List<TodoItem> items;
        public todo()
        {
            InitializeComponent();
            dataService = new DataService.DataService();
            RefreshData();
        }

        async void AddButton_Clicked(object sender, System.EventArgs e)
        {
            TodoItem newItem = new TodoItem
            {
                Description = txtTodoItem.Text.Trim(),
                DueDate = dpDueDate.Date.ToString("d"),
                isDone = false
            };
            await dataService.AddTodoItemAsync(newItem);
            RefreshData();
        }

        async void RefreshButton_Clicked(object sender, System.EventArgs e)
        {
            RefreshData();
        }
        public async void OnDone(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            TodoItem itemToUpdate = (TodoItem)mi.CommandParameter;
            itemToUpdate.isDone = true;
            //int itemIndex = items.IndexOf(itemToUpdate);
            int itemIndex = itemToUpdate.id;
            await dataService.UpdateTodoItemAsync(itemIndex, itemToUpdate);
            RefreshData();
        }

        public async void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            TodoItem itemToDelete = (TodoItem)mi.CommandParameter;
            //int itemIndex = items.IndexOf(itemToDelete);
            int itemIndex = itemToDelete.id;

            await dataService.DeleteTodoItemAsync(itemIndex);
            RefreshData();
        }

        async void RefreshData()
        {
            items = await dataService.GetTodoItemsAsync();
            todoList.ItemsSource = items.OrderBy(item => item.isDone).ThenBy(item => item.id).ToList();
        }
    }
}