﻿using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Windows;
using TodoApiServer.Models;
using System.Net.Http.Headers;
using TodoItemApp.Models;
using MahApps.Metro.Controls.Dialogs;
using System;

namespace TodoItemApp
{

    public class DivCode
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private List<DivCode> divCodes = new List<DivCode>();
        HttpClient client = new HttpClient();
        TodoItemsCollection todoItems = new TodoItemsCollection();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            divCodes.Add(new DivCode { Key = "True", Value = "1" });
            divCodes.Add(new DivCode { Key = "False", Value = "0" });
            CboIsComplete.ItemsSource = divCodes;
            CboIsComplete.DisplayMemberPath = "Key";        // 콤보박스에 T/F 추가

            // yyyy-MM-dd HH:mm:ss 날짜 포맷 (오전/오후 포함)
            // ko => yyyy.M.d H:m:ss
            DtpTodoDate.Culture = new System.Globalization.CultureInfo("ko-KR");

            // RestAPI 호출 시작
            client.BaseAddress = new System.Uri("https://localhost:7120/");     // RestAPI 서버 기본 URL
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            GetData();      // 데이터 로드 메서드 호출        // 끝에 와있어야 함
        }

        // RestApi Get Method 호출
        private async void GetData()
        {
            GrdTodoItems.ItemsSource = todoItems;      // 미리 바인딩

            try         // API 호출
            {
                // https://localhost:7120/api/TodoItems >>> GetApi
                //var reponse = await client.GetAsync("api/TodoItems");   // GET methos 비동기로 호출
                HttpResponseMessage? reponse = await client.GetAsync("api/TodoItems");       // 윗줄과 동일
                reponse.EnsureSuccessStatusCode();     // 에러가 났으면 오류코드를 던진다(예외발생)

                // 응답에서 List<TodoItem> 형식으로 읽어옴
                var items = await reponse.Content.ReadAsAsync<IEnumerable<TodoItem>>();
                todoItems.CopyForm(items);
            }
            catch (Newtonsoft.Json.JsonException jEx)
            {
                await this.ShowMessageAsync("error", jEx.Message, MessageDialogStyle.Affirmative, new MetroDialogSettings()
                {
                    AnimateShow = true,
                    AnimateHide = true
                });
            }
            catch (HttpRequestException ex)
            {
                await this.ShowMessageAsync("error", ex.Message, MessageDialogStyle.Affirmative, new MetroDialogSettings()
                {
                    AnimateShow = true,
                    AnimateHide = true
                });
            }
        }

        private async void BtnInsert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var todoItem = new TodoItem()
                {
                    Id = 0,
                    Title = TxtTitle.Text,
                    TodoDate = ((DateTime)DtpTodoDate.SelectedDateTime).ToString("yyyy-MM-dd HH:mm:ss"),
                    IsComplete = Int32.Parse((CboIsComplete.SelectedItem as DivCode).Value)
                };

                // Insert 할 때는 POST 메서드 사용
                var reponse = await client.PostAsJsonAsync("api/TodoItems", todoItem);
                reponse.EnsureSuccessStatusCode();

                GetData();

                TxtId.Text = TxtTitle.Text = string.Empty;
                CboIsComplete.SelectedIndex = -1;
            }
            catch (Newtonsoft.Json.JsonException jEx)
            {
                await this.ShowMessageAsync("error", jEx.Message, MessageDialogStyle.Affirmative, new MetroDialogSettings()
                {
                    AnimateShow = true,
                    AnimateHide = true
                });
            }
            catch (HttpRequestException ex)
            {
                await this.ShowMessageAsync("error", ex.Message, MessageDialogStyle.Affirmative, new MetroDialogSettings()
                {
                    AnimateShow = true,
                    AnimateHide = true
                });
            }
        }

        private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var todoItem = new TodoItem()
                {
                    Id = Int32.Parse(TxtId.Text),       // 이부분 바뀜
                    Title = TxtTitle.Text,
                    TodoDate = ((DateTime)DtpTodoDate.SelectedDateTime).ToString("yyyy-MM-dd HH:mm:ss"),
                    IsComplete = Int32.Parse((CboIsComplete.SelectedItem as DivCode).Value)
                };

                // Update 할 때는 PUT 메서드 사용
                var reponse = await client.PutAsJsonAsync($"api/TodoItems/{todoItem.Id}", todoItem);    // 이부분 바뀜   // 여기까지 두 줄만 고치면 업데이트는 되지만 꺼지는 오류 발생
                reponse.EnsureSuccessStatusCode();

                GetData();

                TxtId.Text = TxtTitle.Text = string.Empty;
                CboIsComplete.SelectedIndex = -1;
            }
            catch (Newtonsoft.Json.JsonException jEx)
            {
                await this.ShowMessageAsync("error", jEx.Message, MessageDialogStyle.Affirmative, new MetroDialogSettings()
                {
                    AnimateShow = true,
                    AnimateHide = true
                });
            }
            catch (HttpRequestException ex)
            {
                await this.ShowMessageAsync("error", ex.Message, MessageDialogStyle.Affirmative, new MetroDialogSettings()
                {
                    AnimateShow = true,
                    AnimateHide = true
                });
            }
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Id 만 보냄

            try
            {
                var Id = Int32.Parse(TxtId.Text);

                // Delete 할 때는 DeleteAsyn 사용
                var reponse = await client.DeleteAsync($"api/TodoItems/{Id}");    // 이부분 바뀜
                reponse.EnsureSuccessStatusCode();

                GetData();

                TxtId.Text = TxtTitle.Text = string.Empty;
                CboIsComplete.SelectedIndex = -1;
            }
            catch (Newtonsoft.Json.JsonException jEx)
            {
                await this.ShowMessageAsync("error", jEx.Message, MessageDialogStyle.Affirmative, new MetroDialogSettings()
                {
                    AnimateShow = true,
                    AnimateHide = true
                });
            }
            catch (HttpRequestException ex)
            {
                await this.ShowMessageAsync("error", ex.Message, MessageDialogStyle.Affirmative, new MetroDialogSettings()
                {
                    AnimateShow = true,
                    AnimateHide = true
                });
            }
        }

        private async void GrdTodoItems_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Debug.WriteLine(GrdTodoItems.SelectedIndex.ToString());


            try         // API 호출
            {
                // 이 두줄은 try 밖이 아니라 내부로
                var Id = ((TodoItem)GrdTodoItems.SelectedItem).Id;
                Debug.WriteLine(Id);

                // https://localhost:7120/api/TodoItems >>> GetApi
                //var reponse = await client.GetAsync("api/TodoItems");   // GET methos 비동기로 호출
                HttpResponseMessage? reponse = await client.GetAsync($"api/TodoItems/{Id}");       // 윗줄과 동일
                reponse.EnsureSuccessStatusCode();     // 에러가 났으면 오류코드를 던진다(예외발생)

                // 응답에서 List<TodoItem> 형식으로 읽어옴
                var item = await reponse.Content.ReadAsAsync<TodoItem>();
                Debug.WriteLine(item.Title);

                TxtId.Text = item.Id.ToString();
                TxtTitle.Text = item.Title;
                DtpTodoDate.SelectedDateTime = DateTime.Parse(item.TodoDate);
                CboIsComplete.SelectedIndex = item.IsComplete == 1 ? 0 : 1;      // ((할일 목록 눌러서 확인!!!!))

            }
            catch (Newtonsoft.Json.JsonException jEx)
            {
                await this.ShowMessageAsync("error", jEx.Message, MessageDialogStyle.Affirmative, new MetroDialogSettings()
                {
                    AnimateShow = true,
                    AnimateHide = true
                });
            }
            catch (HttpRequestException ex)
            {
                await this.ShowMessageAsync("error", ex.Message, MessageDialogStyle.Affirmative, new MetroDialogSettings()
                {
                    AnimateShow = true,
                    AnimateHide = true
                });
            }

            catch (Exception ex)
            {
                Debug.WriteLine($"이외 예외 {ex.Message}");
            }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            GetData();      // 리로드 - 웹에서 추가해도 리로드를 누르면 볼 수 있음
        }
    }
}
